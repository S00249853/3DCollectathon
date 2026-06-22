using UnityEngine;

public class EnemyCollisions : MonoBehaviour
{
    PlayerStateMachine _playerStateMachine;
    GroundEnemyStateMachine _groundEnemyStateMachine;

    private void Awake()
    {
        _groundEnemyStateMachine = GetComponent<GroundEnemyStateMachine>();
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Vector3 hitDirection = transform.forward;
            if (hitDirection.y <= 0.8)
            {
                _playerStateMachine = collision.gameObject.GetComponent<PlayerStateMachine>();
                hitDirection = hitDirection.normalized;
                _playerStateMachine.OnHurt(hitDirection);
            }
        }
    }
}
