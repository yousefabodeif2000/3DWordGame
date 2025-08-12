using System;
using UnityEngine;
using Screen = Assets._GAME.Scripts.Screen;
public class Sphere : MonoBehaviour
{
    static public event Action<Sphere> OnSphereClicked;


    public SphereColor SphereColor = SphereColor.None;
    GameManager gameManager => GameManager.Instance;

    bool takenp1, takenp2;
    public bool IsTakenByPlayer1
    {
        get
        {
            return takenp1;
        }
        set
        {
            takenp1 = value;
            p1Ring.SetActive(takenp1);
        }
    }
    public bool IsTakenByPlayer2
    {
        get
        {
            return takenp2;
        }
        set
        {
            takenp2 = value;
            p2Ring.SetActive(takenp2);
        }
    }
    public bool IsInteractable;
    public Transform cameraTransform { get; set; }
    public float lookAtCameraSpeed = 10f;
    public GameObject p1Ring;
    public GameObject p2Ring;
    public Screen Screen;
    private char letter;
    private Material sphereMaterial;
    public char Letter
    {
        get
        {
            return letter;
        }
        set
        {
            letter = value;
            if (Screen != null)
            {
                Screen.DisplayLetter(letter);
            }
            else
            {
                Debug.LogWarning("Screen reference is not set. Cannot display letter.");
            }
        }
    }
    private void Start()
    {
        cameraTransform = Camera.main.transform;
        sphereMaterial = GetComponent<Renderer>().material;
    }
    public void Initialize()
    {
        Screen.Initialize();
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
        if (!IsInteractable)
            return;
        if (IsTakenByPlayer1 && gameManager.PlayerOne.IsTurn || IsTakenByPlayer2 && gameManager.PlayerTwo.IsTurn)
            return;
        LightenSphere();
        OnSphereClicked?.Invoke(this);
        if (gameManager.PlayerOne.IsTurn)
            gameManager.PlayerOne.OnSphereClick(this);
        else if (gameManager.PlayerTwo.IsTurn)
            gameManager.PlayerTwo.OnSphereClick(this);
    }
    public void Reset()
    {
        UnLightenSphere();
        IsTakenByPlayer1 = false;
        IsTakenByPlayer2 = false;
    }
    void LightenSphere()
    {
        sphereMaterial.EnableKeyword("_EMISSION");
    }
    void UnLightenSphere()
    {
        sphereMaterial.DisableKeyword("_EMISSION");
    }
}
public enum SphereColor
{
    None,
    Red,
    Green,
    Blue
}