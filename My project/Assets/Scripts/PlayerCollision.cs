using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    private PlayerCollectables playerCollectables;

    private void Awake()
    {
        playerCollectables = GetComponent<PlayerCollectables>();
    }

    private void OnTriggerEnter(Collider other)
    {
        //Handles the player collecting collectables
        if (other.gameObject.tag == "Star")
        {
            Destroy(other.gameObject);
            playerCollectables.starCount++;
        }

        if (other.gameObject.tag == "Coin")
        {
            Destroy(other.gameObject);
            playerCollectables.coinCount++;
        }

        if (other.gameObject.tag == "Destroy")
        {
            Destroy(gameObject);
        }

        //if (other.gameObject.tag == "Victory")
        //{
        //    Destroy(other.gameObject);
        //    pm.BaseJumpCount = 10000;
        //}
    }
}
