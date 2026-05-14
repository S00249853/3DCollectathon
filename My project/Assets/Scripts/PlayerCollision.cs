using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    private PlayerMovement pm;

    private void Awake()
    {
        pm = GetComponent<PlayerMovement>();
    }

    private void OnTriggerEnter(Collider other)
    {
        //if (other.gameObject.tag == "Jump")
        //{
        //    pm.JumpCount++;
        //}

        //if (other.gameObject.tag == "JumpPermanent")
        //{
        //    Destroy(other.gameObject);
        //    pm.BaseJumpCount++;
        //}

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
