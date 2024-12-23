using System;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Chroma {
public class GradientDrawer : MaterialPropertyDrawer {
    private readonly int _resolution = 256;
    private readonly bool _hdr;

    public GradientDrawer() { }

    public GradientDrawer(float resolution) {
        _resolution = (int)resolution;
    }

    public GradientDrawer(bool hdr) {
        _hdr = hdr;
    }

    public GradientDrawer(string parameters) {
        _hdr = ExtractHdrParameter(parameters);
    }

    public GradientDrawer(float resolution, string parameters) {
        _resolution = (int)resolution;
        _hdr = ExtractHdrParameter(parameters);
    }

    private static bool ExtractHdrParameter(string parameters) {
        var split = parameters.Split(',');
        return split.Any(s => string.Equals(s, "true", StringComparison.OrdinalIgnoreCase) ||
                              string.Equals(s, "hdr", StringComparison.OrdinalIgnoreCase));
    }

    public override void OnGUI(Rect position, MaterialProperty prop, string label, MaterialEditor editor) {
        var guiContent = new GUIContent(label);
        OnGUI(position, prop, guiContent, editor);
    }

    public void OnGUI(Rect position, MaterialProperty prop, string label, MaterialEditor editor, string tooltip) {
        var guiContent = new GUIContent(label, tooltip);
        OnGUI(position, prop, guiContent, editor);
    }

    public override void OnGUI(Rect position, MaterialProperty prop, GUIContent label, MaterialEditor editor) {
        if (prop.type != MaterialProperty.PropType.Texture) {
            EditorGUI.HelpBox(position, $"[Gradient] used on property {prop.name} of type {prop.type}.",
                              MessageType.Error);
            return;
        }

        if (!AssetDatabase.Contains(prop.targets.FirstOrDefault())) {
            EditorGUI.HelpBox(position, $"Material {prop.targets.FirstOrDefault()?.name} is not an Asset.",
                              MessageType.Error);
            return;
        }

        var namePrefix = DrawerUtils.TextureNamePrefix(prop.name);
        Gradient currentGradient = null;
        if (prop.targets.Length == 1) {
            var target = (Material)prop.targets[0];
            var path = AssetDatabase.GetAssetPath(target);
            var textureAsset = DrawerUtils.LoadSubAsset(path, namePrefix);
            if (textureAsset != null) {
                if ((_hdr && textureAsset.format != TextureFormat.RGBAHalf) ||
                    (!_hdr && textureAsset.format == TextureFormat.RGBAHalf)) {
                    Debug.Log($"The gradient texture format changed from {textureAsset.format}.");
                    var previousGradient = DrawerUtils.Deserialize(prop, textureAsset.name);
                    HandleGradientUpdated(previousGradient, namePrefix, prop);
                }

                currentGradient = DrawerUtils.Deserialize(prop, textureAsset.name);
            }

            var materialReset = target.GetTexture(prop.name) == null;
            if (currentGradient == null || materialReset) {
                // Create the default gradient.
                var colorKeys = new GradientColorKey[2];
                var alphaKeys = new GradientAlphaKey[2];
                colorKeys[0] = new GradientColorKey(Color.white, 0f);
                alphaKeys[0] = new GradientAlphaKey(1, 0f);
                colorKeys[1] = new GradientColorKey(Color.white, 1f);
                alphaKeys[1] = new GradientAlphaKey(1, 1f);
                currentGradient = new Gradient { colorKeys = colorKeys, alphaKeys = alphaKeys };
            }

            EditorGUI.showMixedValue = false;
        } else {
            EditorGUI.showMixedValue = true;
        }

        using (var changeScope = new EditorGUI.ChangeCheckScope()) {
            EditorGUILayout.Space(-18);
            EditorGUILayout.BeginHorizontal();

            // Using different offsets for SG and text shaders since `EditorGUILayout.GradientField` does not expand to
            // the full width for text shaders.
            var target = (Material)prop.targets[0];
            var guiContent = new GUIContent(label.text, label.tooltip);
            if (HasShaderGraphTag.Check(target)) {
                // Shader graph.
                currentGradient = EditorGUILayout.GradientField(guiContent, currentGradient, _hdr);
            } else {
                // Text shader.
                EditorGUILayout.LabelField(guiContent, EditorStyles.label,
                                           GUILayout.Width(EditorGUIUtility.labelWidth));
                var options = new[] { GUILayout.MinWidth(0) };
                var rect = EditorGUILayout.GetControlRect(true, 18, EditorStyles.colorField, options);
                rect.xMin = EditorGUIUtility.currentViewWidth - EditorGUIUtility.labelWidth + rect.width + 19;
                rect.xMin = Mathf.Min(rect.xMin, EditorGUIUtility.labelWidth + 32);
                currentGradient = EditorGUI.GradientField(rect, new GUIContent(), currentGradient, _hdr);
            }

            if (changeScope.changed) {
                HandleGradientUpdated(currentGradient, namePrefix, prop);
            }

            // Draw buttons.
            var buttonRect = GUILayoutUtility.GetRect(label, EditorStyles.iconButton);
            buttonRect.y += 2.5f;
            var buttonIcon = EditorGUIUtility.IconContent("CustomTool@2x");
            var contextButtonClicked = GUI.Button(buttonRect, buttonIcon, EditorStyles.iconButton);
            if (contextButtonClicked) {
                var menu = new GenericMenu();

                menu.AddItem(new GUIContent("Reverse"), false, () => {
                    var colorKeys = currentGradient.colorKeys;
                    for (var i = 0; i < colorKeys.Length / 2; i++) {
                        (colorKeys[i].color, colorKeys[colorKeys.Length - 1 - i].color) = (
                            colorKeys[colorKeys.Length - 1 - i].color, colorKeys[i].color);
                    }

                    currentGradient = new Gradient {
                        colorKeys = colorKeys, alphaKeys = currentGradient.alphaKeys, mode = currentGradient.mode
                    };
                    HandleGradientUpdated(currentGradient, namePrefix, prop);
                });

                menu.AddItem(new GUIContent("Distribute evenly"), false, () => {
                    GeneratorUtils.DistributeEvenly(ref currentGradient);
                    HandleGradientUpdated(currentGradient, namePrefix, prop);
                });

                menu.AddSeparator("");

                menu.AddItem(new GUIContent("Random"), false, () => {
                    var mode = currentGradient.mode;
                    currentGradient = Palettes.GetRandom();
                    currentGradient.mode = mode;
                    GeneratorUtils.DistributeEvenly(ref currentGradient);
                    HandleGradientUpdated(currentGradient, namePrefix, prop);
                });

                menu.AddItem(new GUIContent("Random complimentary"), false, () => {
                    var mode = currentGradient.mode;
                    currentGradient = GeneratorUtils.RandomComplimentary();
                    currentGradient.mode = mode;
                    GeneratorUtils.DistributeEvenly(ref currentGradient);
                    HandleGradientUpdated(currentGradient, namePrefix, prop);
                });

                menu.AddItem(new GUIContent("Random shades"), false, () => {
                    var mode = currentGradient.mode;
                    currentGradient = GeneratorUtils.RandomShades();
                    currentGradient.mode = mode;
                    GeneratorUtils.DistributeEvenly(ref currentGradient);
                    HandleGradientUpdated(currentGradient, namePrefix, prop);
                });

                menu.AddItem(new GUIContent("Select random..."), false, () => {
                    // Open the Gradient Set window.
                    var window = EditorWindow.GetWindow<GradientSetWindow>();
                    window.OnGradientSelected += gradient => {
                        currentGradient = gradient;
                        HandleGradientUpdated(currentGradient, namePrefix, prop);
                    };
                });

                menu.AddSeparator("");

                menu.AddItem(new GUIContent("Open Adobe Color"), false,
                             () => { Application.OpenURL("https://color.adobe.com/"); });

                menu.AddItem(new GUIContent("Paste XML from Adobe Color"), false, () => {
                    var systemCopyBuffer = EditorGUIUtility.systemCopyBuffer;
                    if (string.IsNullOrEmpty(systemCopyBuffer)) {
                        Debug.LogWarning("Clipboard is empty.");
                        return;
                    }

                    if (systemCopyBuffer.Contains("Gradient in Hex")) {
                        Debug.LogWarning("Clipboard contains a CSS gradient, which can not be reproduced in Unity. " +
                                         "Please use 'Extract Theme' in Adobe Color and copy the gradient in XML " +
                                         "format.");
                        return;
                    }

                    var gradient = AdobeColorParser.ParseXml(systemCopyBuffer);
                    if (gradient == null) {
                        Debug.LogError("Could not parse XML from clipboard.");
                        return;
                    }

                    gradient.mode = currentGradient.mode;
                    currentGradient = gradient;
                    GeneratorUtils.DistributeEvenly(ref currentGradient);
                    HandleGradientUpdated(currentGradient, namePrefix, prop);
                });

                menu.AddSeparator("");

                menu.AddItem(new GUIContent("Open ColorHunt.co"), false,
                             () => { Application.OpenURL("https://colorhunt.co/"); });

                menu.AddItem(new GUIContent("Paste URL from ColorHunt.co"), false, () => {
                    var systemCopyBuffer = EditorGUIUtility.systemCopyBuffer;
                    if (string.IsNullOrEmpty(systemCopyBuffer)) {
                        Debug.LogWarning("Clipboard is empty.");
                        return;
                    }

                    var gradient = ColorHuntParser.UrlToGradient(systemCopyBuffer);
                    if (gradient == null) {
                        Debug.LogError("Could not parse the link.");
                        return;
                    }

                    gradient.mode = currentGradient.mode;
                    currentGradient = gradient;
                    GeneratorUtils.DistributeEvenly(ref currentGradient);
                    HandleGradientUpdated(currentGradient, namePrefix, prop);
                });

                menu.ShowAsContext();
            }

            EditorGUILayout.EndHorizontal();
        }

        EditorGUI.showMixedValue = false;
    }

    private void HandleGradientUpdated(Gradient gradient, string namePrefix, MaterialProperty prop) {
        var encodedGradient = DrawerUtils.Serialize(gradient);
        var fullAssetName = namePrefix + encodedGradient;
        foreach (var target in prop.targets) {
            if (!AssetDatabase.Contains(target)) continue;

            var path = AssetDatabase.GetAssetPath(target);
            var filterMode = gradient.mode == GradientMode.Blend ? FilterMode.Bilinear : FilterMode.Point;
            var textureAsset = DrawerUtils.GetOrCreateSubAssetTexture(path, namePrefix, filterMode, _resolution, _hdr);
            Undo.RecordObject(textureAsset, "Change Material Gradient");
            textureAsset.name = fullAssetName;
            DrawerUtils.Bake(gradient, textureAsset);

            var material = (Material)target;
            material.SetTexture(prop.name, textureAsset);
            EditorUtility.SetDirty(material);
        }
    }

    [Serializable]
    internal class GradientData {
        public GradientMode mode;
        public ColorKey[] colorKeys;
        public AlphaKey[] alphaKeys;

        public GradientData() { }

        public GradientData(Gradient source) {
            FromGradient(source);
        }

        public void FromGradient(Gradient source) {
            mode = source.mode;
            colorKeys = source.colorKeys.Select(key => new ColorKey(key)).ToArray();
            alphaKeys = source.alphaKeys.Select(key => new AlphaKey(key)).ToArray();
        }

        public void ToGradient(Gradient gradient) {
            gradient.mode = mode;
            gradient.colorKeys = colorKeys.Select(key => key.ToGradientKey()).ToArray();
            gradient.alphaKeys = alphaKeys.Select(key => key.ToGradientKey()).ToArray();
        }

        public Gradient ToGradient() {
            var gradient = new Gradient();
            ToGradient(gradient);
            return gradient;
        }

        [Serializable]
        public struct ColorKey {
            public Color color;
            public float time;

            public ColorKey(GradientColorKey source) {
                color = default;
                time = default;
                FromGradientKey(source);
            }

            public void FromGradientKey(GradientColorKey source) {
                color = source.color;
                time = source.time;
            }

            public GradientColorKey ToGradientKey() {
                GradientColorKey key;
                key.color = color;
                key.time = time;
                return key;
            }
        }

        [Serializable]
        public struct AlphaKey {
            public float alpha;
            public float time;

            public AlphaKey(GradientAlphaKey source) {
                alpha = default;
                time = default;
                FromGradientKey(source);
            }

            public void FromGradientKey(GradientAlphaKey source) {
                alpha = source.alpha;
                time = source.time;
            }

            public GradientAlphaKey ToGradientKey() {
                GradientAlphaKey key;
                key.alpha = alpha;
                key.time = time;
                return key;
            }
        }
    }
}
}