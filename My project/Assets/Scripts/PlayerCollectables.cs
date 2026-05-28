using UnityEngine;

public class PlayerCollectables : MonoBehaviour
{
    //Variables representing the amount of collectables the player currently has
    public int starCount;
    public int coinCount;

    //Variables representing the max amount of collectables the player can collect
    private int maxStarCount = 20;
    private int maxCoinCount = 100;

    private void Update()
    {
        //Ensures the two variables cannot go over their max
        if (starCount > maxStarCount)
        {
            starCount = maxStarCount;
        }

        if (coinCount > maxCoinCount)
        {
            coinCount = maxCoinCount;
        }
    }
}
