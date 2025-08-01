using System;
using UnityEngine;

public class Sphere : MonoBehaviour
{
    static public event Action<Sphere> OnSphereClicked;

    public Transform cameraTransform { get; set; }
    public float lookAtCameraSpeed = 10f;
    public Screen Screen;
    public char Letter;
    public bool IsClicked { get; private set; } = false;
    private void Start()
    {
        cameraTransform = Camera.main.transform;
    }
    public void Initialize()
    {
        
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

    public void Click()
    {
        OnSphereClicked?.Invoke(this);
        LightenSphere();
    }

    void LightenSphere()
    {


    }
}