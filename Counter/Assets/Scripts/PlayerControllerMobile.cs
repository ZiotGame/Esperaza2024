using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControllerMobile : MonoBehaviour
{
    public float speed = 5.0f;
    private Vector2 moveInput;
    private PlayerInputActions playerInputActions;

    private void Awake()
    {
        playerInputActions = new PlayerInputActions();
    }

    private void OnEnable()
    {
        playerInputActions.Player.Enable();
        playerInputActions.Player.Move.performed += OnMove;
        playerInputActions.Player.Move.canceled += OnMove;
    }

    private void OnDisable()
    {
        playerInputActions.Player.Move.performed -= OnMove;
        playerInputActions.Player.Move.canceled -= OnMove;
        playerInputActions.Player.Disable();
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
        Debug.Log("Move Input: " + moveInput);

    }

    void Update()
    {
        Vector3 position = transform.position;
        position.x += moveInput.x * speed * Time.deltaTime;
        transform.position = position;
    }
}