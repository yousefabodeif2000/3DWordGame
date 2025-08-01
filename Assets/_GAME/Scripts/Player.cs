using System;
using System.Collections.Generic;
using UnityEngine;

namespace Dimensional
{
    public class Player : MonoBehaviour
    {
        public PlayerOneTwo PlayerType = PlayerOneTwo.Player1;
        public string playerName = "Player1";
        public HUDComponent playerHUD;

        private bool isTurn;
        public bool IsTurn 
        { 
            get { 
                return isTurn; 
            } 
            set { 
                isTurn = value; 
            } 
        }
        public int playerScore = 0;
        public int PlayerScore
        {
            get 
            { 
                return playerHUD.GetScore(); 
            }
            set 
            { 
                playerScore = value; 
                playerHUD.AddScore(playerScore);
            }
        }
        [HideInInspector] public List<Sphere> ChosenSpheres = new List<Sphere>();

        public void Initialize(GameConfiguration config)
        {
            playerHUD.Initialize(config);
            isTurn = false;
            ChosenSpheres.Clear();
        }

    }
}