using Dimensional;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
public class GameManager : MonoBehaviour
{
    static public GameManager Instance;
    public GameConfiguration GameConfiguration;
    public GameResources GameResources;

    public Player PlayerOne;
    public Player PlayerTwo;


    [Header("References")]
    public List<Sphere> Spheres = new List<Sphere>();


    private void Awake()
    {
        Instance = this;
    }

    public void InitializeGame()
    {
        GameResources.Letters.Shuffle();
        AssignLettersToSpheres();
        PlayerOne.Initialize(GameConfiguration);
        PlayerTwo.Initialize(GameConfiguration);
        PlayerOne.IsTurn = true;
        PlayerTwo.IsTurn = false;
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
                Spheres[i].Letter = GameResources.Letters[i];
                Spheres[i].Initialize();
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
        Instance.PlayerOne.IsTurn = !Instance.PlayerOne.IsTurn;
        Instance.PlayerTwo.IsTurn = !Instance.PlayerTwo.IsTurn;
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
}