using UnityEngine;

public class HUDComponent : MonoBehaviour
{
    public Player CurrentPlayer = Player.Player1;
    private string playerName;
    private int playerScore = 0;



    public void Initialize(GameConfiguration config)
    {
        playerScore = 0;
    }

    public void AddScore(int score)
    {
        playerScore += score;
    }

}

public enum Player
{
    Player1, Player2
}
