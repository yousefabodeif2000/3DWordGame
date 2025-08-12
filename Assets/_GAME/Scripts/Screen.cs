using System;
using UnityEngine;

namespace Assets._GAME.Scripts
{
    public class Screen : MonoBehaviour
    {
        private Material screenMat;
        private Vector2 letterOffset;
        public void Initialize()
        {
            screenMat = GetComponent<Renderer>().materials[1];

        }
        public void DisplayLetter(char letter)
        {
            GameResources.GetLetterOffsetVal(letter, out letterOffset);
            if (screenMat != null)
            {
                screenMat.mainTextureOffset = letterOffset;
            }
            else
            {
                Debug.LogError("Screen material is not set. Cannot display letter.");
            }
        }
    }
    [Serializable]
    public class LetterDisplay
    {
        public char Letter;
        public Vector2 Offset;
    }
}