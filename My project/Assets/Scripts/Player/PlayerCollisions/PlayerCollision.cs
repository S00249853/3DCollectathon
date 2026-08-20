using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    private PlayerStateMachine _player;
    private CharacterController _cc;

    private void Awake()
    {
        _player = GetComponent<PlayerStateMachine>();
        _cc = GetComponent<CharacterController>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Cannonball")
        {
            Debug.Log("Cannonball Hit");
            Vector3 hitDirection = collision.gameObject.transform.forward;
            if (_cc.collisionFlags != CollisionFlags.Below)
            {
                hitDirection = hitDirection.normalized;
                _player.OnHurt(hitDirection, 10);
                collision.gameObject.SetActive(false);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Star")
        {
            Destroy(other.gameObject);
            GameManager.Instance.Collected++;
        }

        //Resets players dash in mid air
        if (other.gameObject.tag == "DashOrb")
        {
            _player.DashCdTimer = 0;
            _player.CanDash = true;
        }

        if (other.gameObject.tag == "Destroy")
        {
            Destroy(gameObject);
        }

        //Resets player upon climbing off of a climbing wall
        if (other.gameObject.tag == "ClimbStop")
        {
            Debug.Log("StopClimbing should be true");
            _player.StopClimbing = true;
        }

        //Teleports player, character controller needs to be disabled for the players transform to be changed
        if (other.gameObject.tag == "Teleporter")
        {
            Teleporter teleporter = other.gameObject.GetComponent<Teleporter>();
            _cc.enabled = false;
            _cc.transform.position = teleporter.TeleportSpawn.position;
            _cc.enabled = true;
            Debug.Log($"Teleport successful, player now at {_cc.transform.position}");
        }

        if (other.gameObject.tag == "Checkpoint")
        {
            GameManager.Instance.Checkpoint = other.transform;
            Debug.Log($"Checkpoint is {GameManager.Instance.Checkpoint.position}");
        }

        if (other.gameObject.tag == "Boundary")
        {
            GameManager.Instance.Health = 0;
        }

        if (other.gameObject.tag == "Healer")
        {
            GameManager.Instance.Health = 100;
        }

        if (other.gameObject.tag == "Activator")
        {
            ObjectActivator activator = other.gameObject.GetComponent<ObjectActivator>();
            activator.Activate();
            Debug.Log("Object Activated");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "ClimbStop")
        {
            _player.StopClimbing = false;
        }
    }
}
