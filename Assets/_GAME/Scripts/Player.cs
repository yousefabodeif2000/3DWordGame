using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

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
                playerHUD.LightUp(isTurn);
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
            playerHUD.Initialize();
            isTurn = false;
            ChosenSpheres.Clear();
        }
        public void OnSphereClick(Sphere sphere)
        {
            if (PlayerType == PlayerOneTwo.Player1)
                sphere.IsTakenByPlayer1 = PlayerType == PlayerOneTwo.Player1 ? true : false;
            else
                sphere.IsTakenByPlayer2 = PlayerType == PlayerOneTwo.Player2 ? true : false;
            GameManager.SwitchTurns();
            playerHUD.OnSphereClick(sphere);
            ChosenSpheres.Add(sphere);
        }
        public void Reset()
        {
            IsTurn = false;
            PlayerScore = 0;
            ChosenSpheres.Clear();
        }
    }
}