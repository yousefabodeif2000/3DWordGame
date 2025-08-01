using Dimensional;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
public class GameManager : MonoBehaviour
{
    static public GameManager Instance;
    public GameConfiguration GameConfiguration;
    static public GameResources GameResources;

    public Player PlayerOne;
    public Player PlayerTwo;

    [Header("Settings")]
    public float delayBetweenTurns = 2f;


    [Header("References")]
    public List<Sphere> Spheres = new List<Sphere>();
    


    private void Awake()
    {
        Instance = this;
        GameResources = Resources.Load<GameResources>("GameResources");
    }
    private void Start()
    {
        InitializeGame();
    }
    private void OnEnable()
    { 
        // Subscribe to events if any
        Sphere.OnSphereClicked += OnSphereClicked;

    }
    private void OnDisable()
    {
        // Unsubscribe from events if any
        Sphere.OnSphereClicked -= OnSphereClicked;
    }
    public void InitializeGame()
    {
        GameResources.Letters.Shuffle();
        AssignLettersToSpheres();
        PlayerOne.Initialize(GameConfiguration);
        PlayerTwo.Initialize(GameConfiguration);
        PlayerOne.IsTurn = true;
        PlayerTwo.IsTurn = false;
        SphereUtility.LockInteraction(false);
    }
    public void AssignLettersToSpheres()
    {
        if (Spheres.Count == 0)
        {
            Debug.LogError("No spheres found to assign letters to.");
            return;
        }
        for (int i = 0; i < Spheres.Count; i++)
        {
            if (i < GameResources.Letters.Count)
            {
                Spheres[i].Initialize();
                Spheres[i].Letter = GameResources.Letters[i];
            }
            else
            {
                Debug.LogWarning($"Not enough letters to assign to all spheres. {Spheres.Count - GameResources.Letters.Count} spheres will not have a letter assigned.");
                break;
            }
        }
    }
    public static void SwitchTurns() 
    {
        Instance.StartCoroutine(enumerator());
        IEnumerator enumerator()
        {
            SphereUtility.LockInteraction(true);
            yield return new WaitForSeconds(Instance.delayBetweenTurns); 
            Instance.PlayerOne.IsTurn = !Instance.PlayerOne.IsTurn;
            Instance.PlayerTwo.IsTurn = !Instance.PlayerTwo.IsTurn;
            SphereUtility.LockInteraction(false);
        }

    }
    void OnSphereClicked(Sphere sphere)
    {

    }
    public void Reset()
    {
        PlayerOne.Reset();
        PlayerTwo.Reset();
    }
}

public static class ListExtensions
{
    private static System.Random rng = new System.Random();

    public static void Shuffle<T>(this IList<T> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }
}

public static class SphereUtility
{
    public static void LockInteraction(bool lockInteraction)
    {
        GameManager manager = GameManager.Instance;
        foreach (Sphere sphere in manager.Spheres)
        {
            sphere.IsInteractable = !lockInteraction;
        }
    }
}
[CustomEditor(typeof(GameManager))]
public class GameDebugEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector(); // Draw normal fields

        if (Application.isPlaying) // Only show at runtime
        {
            GameManager myTarget = (GameManager)target;

            if (GUILayout.Button("Initialize Game"))
            {
                myTarget.InitializeGame();
            }
            if(GUILayout.Button("Reset Game"))
            {
                myTarget.Reset();
            }
        }
    }
}