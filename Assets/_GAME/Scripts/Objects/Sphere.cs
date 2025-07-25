using UnityEngine;

public class Sphere : MonoBehaviour
{
    public Transform cameraTransform { get; set; }
    public float lookAtCameraSpeed = 10f;
    private void Start()
    {
        cameraTransform = Camera.main.transform;
    }
    private void FixedUpdate()
    {
        SphereRotation();
    }
    void SphereRotation()
    {
        Vector3 direction = cameraTransform.position - transform.position;

        Quaternion targetRotation = Quaternion.LookRotation(direction);

        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, lookAtCameraSpeed * Time.deltaTime);
    }
}
