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
}

