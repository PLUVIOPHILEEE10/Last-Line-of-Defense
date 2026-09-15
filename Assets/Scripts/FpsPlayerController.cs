using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public sealed class FpsPlayerController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField, Min(0f)] private float moveSpeed = 5f;
    [SerializeField, Min(1f)] private float sprintMultiplier = 1.55f;
    [SerializeField, Min(0f)] private float jumpHeight = 1.2f;
    [SerializeField] private float gravity = -20f;

    [Header("Mouse Look")]
    [SerializeField, Min(0f)] private float mouseSensitivity = 0.12f;
    [SerializeField] private Transform playerCamera;

    private CharacterController controller;
    private float verticalVelocity;
    private float cameraPitch;
    private bool inputEnabled = true;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        if (playerCamera == null)
        {
            Camera childCamera = GetComponentInChildren<Camera>();
            if (childCamera != null) playerCamera = childCamera.transform;
        }
    }

    private void Start()
    {
        LockCursor();
    }

    private void Update()
    {
        if (!inputEnabled || GameRuntime.IsPaused) return;

        Keyboard keyboard = Keyboard.current;
        Mouse mouse = Mouse.current;
        if (keyboard == null || mouse == null) return;

        Move(keyboard);
        Look(mouse);
    }

    private void Move(Keyboard keyboard)
    {
        float horizontal = 0f;
        float forward = 0f;
        if (keyboard.aKey.isPressed) horizontal -= 1f;
        if (keyboard.dKey.isPressed) horizontal += 1f;
        if (keyboard.sKey.isPressed) forward -= 1f;
        if (keyboard.wKey.isPressed) forward += 1f;

        Vector3 direction = Vector3.ClampMagnitude(
            transform.right * horizontal + transform.forward * forward, 1f);

        if (controller.isGrounded && verticalVelocity < 0f) verticalVelocity = -2f;
        if (controller.isGrounded && keyboard.spaceKey.wasPressedThisFrame)
        {
            verticalVelocity = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        verticalVelocity += gravity * Time.deltaTime;
        float speed = keyboard.leftShiftKey.isPressed
            ? moveSpeed * sprintMultiplier
            : moveSpeed;
        Vector3 velocity = direction * speed + Vector3.up * verticalVelocity;
        controller.Move(velocity * Time.deltaTime);
    }

    private void Look(Mouse mouse)
    {
        if (Cursor.lockState != CursorLockMode.Locked || playerCamera == null) return;

        Vector2 mouseDelta = mouse.delta.ReadValue() * mouseSensitivity;
        transform.Rotate(Vector3.up * mouseDelta.x);
        cameraPitch = Mathf.Clamp(cameraPitch - mouseDelta.y, -85f, 85f);
        playerCamera.localRotation = Quaternion.Euler(cameraPitch, 0f, 0f);
    }

    public void SetInputEnabled(bool enabled)
    {
        inputEnabled = enabled;
        if (!enabled) verticalVelocity = 0f;
    }

    public static void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void OnValidate()
    {
        moveSpeed = Mathf.Max(0f, moveSpeed);
        sprintMultiplier = Mathf.Max(1f, sprintMultiplier);
        jumpHeight = Mathf.Max(0f, jumpHeight);
        gravity = Mathf.Min(-0.01f, gravity);
        mouseSensitivity = Mathf.Max(0f, mouseSensitivity);
    }
}
