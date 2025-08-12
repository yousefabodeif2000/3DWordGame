using Dimensional;
using System;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class LetterUI : MonoBehaviour
{
    char letter;
    public char Letter
    {
        get { return letter; }
        set
        {
            letter = value;
            letterText.text = letter.ToString();
        }
    }
    public TMPro.TMP_Text letterText;
    public void Initialize()
    {
        letterText.gameObject.SetActive(false);
    }
    public static LetterUI Create(char letter)
    {
        GameObject letterObject = GameManager.GameResources.letterUIPrefab.gameObject;
        Player currentPlayer = GameManager.Instance.PlayerOne.IsTurn ? GameManager.Instance.PlayerOne : GameManager.Instance.PlayerTwo;
        LetterUI letterUI = Instantiate(letterObject).GetComponent<LetterUI>();
        letterUI.transform.SetParent(currentPlayer.playerHUD.letterUIHolders);
        letterUI.transform.position = Vector3.zero;
        letterUI.Letter = letter;
        return letterUI;
    }

}
