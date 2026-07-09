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
    private void OnCollisionEnter(Collision collision)
    {
        
        if (collision.gameObject.tag == "Cannonball")
        {
            Debug.Log("Cannonball Hit");
            Vector3 hitDirection = collision.gameObject.transform.forward;
            if (hitDirection.y <= 0.8)
            {
                hitDirection = hitDirection.normalized;
                playerStateMachine.OnHurt(hitDirection);
                collision.gameObject.SetActive(false);
            }
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
        if (other.gameObject.tag == "Climbable")
        {
            RaycastHit hit;
            Physics.Raycast(gameObject.transform.position, other.transform.position, out hit);
            playerStateMachine.Hit = hit;
            playerStateMachine.IsClimb = true;
        }

        if (other.gameObject.tag == "Teleporter")
        {
            Teleporter teleporter = other.gameObject.GetComponent<Teleporter>();
            CharacterController characterController = GetComponent<CharacterController>();
            characterController.enabled = false;
            characterController.transform.position = teleporter.TeleportSpawn.position;
            characterController.enabled = true;
            Debug.Log($"Teleport successful, player now at {characterController.transform.position}");
        }

        if (other.gameObject.tag == "Checkpoint")
        {
            playerStateMachine.Checkpoint = other.transform;
            Debug.Log($"Checkpoint is {playerStateMachine.Checkpoint.position}");
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

    //private void OnTriggerStay(Collider other)
    //{
    //    if (other.gameObject.tag == "Climbable")
    //    {

    //    }
    //}

    private void OnTriggerExit(Collider other)
    {
        //if (other.gameObject.tag == "MovingPlatform")
        //{
        //    Debug.Log("Should be OVER");
        //    transform.parent = null;
        //}

        if (other.gameObject.tag == "Climbable")
        {
            playerStateMachine.IsClimb = false;
        }
    }
}
