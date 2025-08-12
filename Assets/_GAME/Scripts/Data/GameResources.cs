using Assets._GAME.Scripts;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New Game Resources", menuName = "Dimensional/Game Resources")]
public class GameResources : ScriptableObject
{
    [Header("Constants")]
    public List<char> Letters = new List<char>()
    {
        'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J',
        'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T',
        'U', 'V', 'W', 'X', 'Y', 'Z'
    };
    public List<LetterDisplay> LetterDisplays = new List<LetterDisplay>();

    [Header("Resources")]
    public LetterUI letterUIPrefab;


    public static void GetLetterOffsetVal(char letter, out Vector2 offset)
    {
        var letterDisplay = GameManager.GameResources.LetterDisplays.Find(ld => ld.Letter == letter);
        if (letterDisplay != null)
        {
            offset = letterDisplay.Offset;
        }
        else
        {
            Debug.LogWarning($"Letter {letter} not found in LetterDisplays. Using default offset.");
            offset = Vector2.zero; // Default offset if not found
        }
    }
}

