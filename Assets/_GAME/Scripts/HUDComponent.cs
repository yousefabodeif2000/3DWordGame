using UnityEngine;

public class HUDComponent : MonoBehaviour
{
    private int playerScore = 0;



    public void Initialize(GameConfiguration config)
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
}

public enum PlayerOneTwo
{
    Player1, Player2
}
