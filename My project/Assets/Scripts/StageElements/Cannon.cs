using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class Cannon : MonoBehaviour
{
    GameObject _player;
    public Queue<GameObject> _cannonPool;
    public Pool Cannonballs;
    public Transform LaunchPoint;
    public Transform Direction;
    [SerializeField] bool _homing;
    [SerializeField] float _maxHomingDistance;
    [SerializeField] float launchCd;
    [SerializeField] float cannonballSpeed;
    float launchTimer;
  
    void Start()
    {
        launchTimer = launchCd;
        _cannonPool = new Queue<GameObject>();
        _player = GameObject.FindGameObjectWithTag("Player");

       
        for (int i = 0; i < Cannonballs.size; i++)
        {
            GameObject obj = Instantiate(Cannonballs.prefab);
            obj.SetActive(false);
            Cannonball ball = obj.GetComponent<Cannonball>();
            if (ball != null)
            {
                ball.Start = LaunchPoint.position;
                ball._speed = cannonballSpeed;
                if (!_homing)
                {
                    ball.Destination = Direction.position;
                    ball.SetSpeed();
                } 
            }
            _cannonPool.Enqueue(obj);
        }
    }

    private void Launch()
    {
        GameObject cannonball = _cannonPool.Dequeue();
        if (_homing)
        {
            Cannonball ball = cannonball.GetComponent<Cannonball>();
            ball.Destination = _player.transform.position;
            ball.SetSpeed();
        }
        cannonball.SetActive(false);
        cannonball.SetActive(true);
        _cannonPool.Enqueue(cannonball);
        Debug.Log($"Should Launch {_cannonPool.Count}");
    }

    void Update()
    {
        if (!_homing)
        {
            launchTimer -= Time.deltaTime;
            if (launchTimer <= 0)
            {
                launchTimer = launchCd;
                Launch();
            }
        }
        else
        {
            if (Vector3.Distance(_player.transform.position, LaunchPoint.position) <= _maxHomingDistance)
            {
                launchTimer -= Time.deltaTime;
                if (launchTimer <= 0)
                {
                    launchTimer = launchCd;
                    Launch();
                }
            }
        }
    }
}
