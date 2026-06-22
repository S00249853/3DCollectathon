using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    private PlayerCollectables playerCollectables;
    private PlayerStateMachine playerStateMachine;

   

    private void Awake()
    {
        playerCollectables = GetComponent<PlayerCollectables>();
        playerStateMachine = GetComponent<PlayerStateMachine>();
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
        //if (other.gameObject.tag == "MovingPlatform")
        //{
        //    Debug.Log("Should be colliding");
            
        //    transform.SetParent(other.transform) ;
        //}

        //if (other.gameObject.tag == "Victory")
        //{
        //    Destroy(other.gameObject);
        //    pm.BaseJumpCount = 10000;
        //}
    }

    //private void OnTriggerExit(Collider other)
    //{
    //    if (other.gameObject.tag == "MovingPlatform")
    //    {
    //        Debug.Log("Should be OVER");
    //        transform.parent = null;
    //    }
    //}
}
