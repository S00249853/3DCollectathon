using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    private PlayerCollectables playerCollectables;
    private PlayerStateMachine playerStateMachine;

    public bool Test;

    private void Awake()
    {
        playerCollectables = GetComponent<PlayerCollectables>();
        playerStateMachine = GetComponent<PlayerStateMachine>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (playerStateMachine.IsGroundPounding)
        {
         
        }
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

        IStompable stomped = other.gameObject.GetComponent<IStompable>();
        if (stomped != null)
        {
            stomped.Stomped();
        }

        //if (other.gameObject.tag == "Victory")
        //{
        //    Destroy(other.gameObject);
        //    pm.BaseJumpCount = 10000;
        //}
    }
}
