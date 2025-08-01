using System.Collections.Generic;
using System.Linq;
using UnityEngine;
public class GameManager : MonoBehaviour
{

    public GameConfiguration GameConfiguration;

    [Header("References")]
    public List<Sphere> Spheres = new List<Sphere>();


    public void InitializeGame()
    {
        Spheres.Shuffle();
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