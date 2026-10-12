using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    [Header("Target & Positioning")]
    [Tooltip("The player transform to follow.")]
    public Transform target;
    [Tooltip("Offset relative to target's position (e.g., shoulder/head height).")]
    public Vector3 targetOffset = new Vector3(0f, 1.5f, 0f);
    [Tooltip("Distance behind the target.")]
    public float distance = 5.0f;

    [Header("Rotation & Input")]
    [Tooltip("Sensitivity for horizontal and vertical mouse look.")]
    public float sensitivityX = 3.0f;
    public float sensitivityY = 3.0f;

    [Header("Pitch Limits")]
    [Tooltip("Lowest angle the camera can look down (degrees).")]
    public float minPitch = -20.0f;
    [Tooltip("Highest angle the camera can look up (degrees).")]
    public float maxPitch = 80.0f;

    [Header("Smoothing")]
    [Tooltip("Smooth time for camera movement dampening.")]
    public float smoothTime = 0.1f;

    private float currentYaw = 0.0f;
    private float currentPitch = 0.0f;
    private Vector3 currentVelocity;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Initialize orientation from current camera transform
        Vector3 angles = transform.eulerAngles;
        currentYaw = angles.y;
        currentPitch = angles.x;
    }

    void LateUpdate()
    {
        if (target == null) return;

        // 1. Gather mouse input
        float mouseX = Input.GetAxis("Mouse X") * sensitivityX;
        float mouseY = Input.GetAxis("Mouse Y") * sensitivityY;

        currentYaw += mouseX;
        // Invert vertical input so pushing mouse up tilts camera up
        currentPitch -= mouseY;
        currentPitch = Mathf.Clamp(currentPitch, minPitch, maxPitch);

        // 2. Calculate target rotation & position
        Quaternion targetRotation = Quaternion.Euler(currentPitch, currentYaw, 0f);
        Vector3 focusPoint = target.position + targetOffset;
        Vector3 desiredPosition = focusPoint - (targetRotation * Vector3.forward * distance);

        // 3. Smoothly interpolate position and apply rotation
        transform.position = Vector3.SmoothDamp(transform.position, desiredPosition, ref currentVelocity, smoothTime);
        transform.LookAt(focusPoint);
    }
}