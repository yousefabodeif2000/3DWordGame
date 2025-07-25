using UnityEngine;
using UnityEngine.InputSystem;

public class CameraOrbit : MonoBehaviour
{
    public Transform target;          // The cubicle to orbit around
    public float distance = 5f;       // How far the camera stays from the target
    public float xSpeed = 120f;       // Horizontal drag speed
    public float ySpeed = 80f;        // Vertical drag speed
    public float yMin = 10f;          // Minimum vertical angle
    public float yMax = 80f;          // Maximum vertical angle
    public float smoothTime = 0.1f;   // Smooth damping time

    private float x = 0.0f;
    private float y = 20.0f;
    private Vector3 velocity;
    private Vector2 dragInput;

    private GameInput inputActions;

    void Awake()
    {
        inputActions = new GameInput();
    }

    void OnEnable()
    {
        inputActions.Enable();
    }

    void OnDisable()
    {
        inputActions.Disable();
    }

    void Start()
    {
        Vector3 angles = transform.eulerAngles;
        x = angles.y;
        y = angles.x;
    }

    void Update()
    {
        dragInput = inputActions.Touch.Look.ReadValue<Vector2>();

        if (dragInput.sqrMagnitude > 0.001f)
        {
            x += dragInput.x * xSpeed * Time.deltaTime;
            y -= dragInput.y * ySpeed * Time.deltaTime;
            y = Mathf.Clamp(y, yMin, yMax);
        }
    }

    void LateUpdate()
    {
        // Build rotation from Euler angles
        Quaternion rotation = Quaternion.Euler(y, x, 0);

        // Get desired camera position
        Vector3 desiredPosition = target.position - (rotation * Vector3.forward * distance);

        // Smooth camera movement
        transform.position = Vector3.SmoothDamp(transform.position, desiredPosition, ref velocity, smoothTime);

        // Make camera look at the target
        transform.LookAt(target.position, Vector3.up);
    }
}
