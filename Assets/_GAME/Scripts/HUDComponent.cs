using System.Collections.Generic;
using UnityEngine;

public class HUDComponent : MonoBehaviour
{
    private int playerScore = 0;
    CanvasGroup canvasGroup;
    [Header("References")]
    public Transform letterUIHolders;

    bool isSubscribed;
    public List<LetterUI> WordLetters = new List<LetterUI>();
    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            Debug.LogError("HUDComponent requires a CanvasGroup component.");
        }
    }
    public void Initialize()
    {
        playerScore = 0;
    }

    public void AddScore(int score)
    {
        playerScore += score;
    }
    public int GetScore()
    {
        return playerScore;
    }
    public void LightUp(bool state)
    {
        if(state) canvasGroup.alpha = 1f;
        else canvasGroup.alpha = 0.15f;
    }
    public void OnSphereClick(Sphere sphere)
    {
        WordLetters.Add(LetterUI.Create(sphere.Letter));
    }
    public void CheckWord()
    {
        string word = WordMaker.CreateWord(WordLetters);
        API.CheckWord(word, (isValid) =>
        {
            if (isValid)
            {
                Debug.Log($"Word '{word}' is valid!");
                AddScore(word.Length);
            }
            else
            {
                Debug.LogWarning($"Word '{word}' is NOT valid.");
            }
            WordLetters.Clear();
        });
    }
    public void Reset()
    {

    }
}

public enum PlayerOneTwo
{
    Player1, Player2
}
