using UnityEngine;
using System.Collections.Generic;
public class WordMaker : MonoBehaviour
{
    public static string CreateWord(List<LetterUI> letters)
    {
        string word = "";
        foreach (var letter in letters)
        {
            word += letter.Letter;
        }
        return word;
    }
}
