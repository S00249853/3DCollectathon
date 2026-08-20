using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder.MeshOperations;
using UnityEngine.WSA;

public enum Phase
{
    One,
    Two, 
    Three
}

public class FinalBossCannon : Cannon
{
    private GameObject _player;

    public Transform _secondPosition;
    public Transform _thirdPosition;
    public Transform _currentPosition;

    public Phase Phase;

    [SerializeField] float _cannonballSpeed2;
    [SerializeField] float _cannonballSpeed3;
    float _currentSpeed;
    protected override void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
         Phase = Phase.One;
        _currentSpeed = cannonballSpeed;
        _currentPosition = LaunchPoint;
         base.Start();
    }

   void Launch()
    {
        GameObject cannonball = _cannonPool.Dequeue();
        Cannonball ball = cannonball.GetComponent<Cannonball>();
        ball.Destination = _player.transform.position;
        ball._speed = _currentSpeed;
        ball.Start = _currentPosition.position;
        ball.SetSpeed();
        cannonball.SetActive(false);
        cannonball.SetActive(true);
        _cannonPool.Enqueue(cannonball);
        Debug.Log($"Should Launch {_cannonPool.Count}");
    }

    void Update()
    {
        if (Phase == Phase.Two)
        {
            _currentSpeed = _cannonballSpeed2;
            _currentPosition = _secondPosition;
            transform.position = _secondPosition.position;
        }
        else if (Phase == Phase.Three)
        {
            _currentSpeed = _cannonballSpeed3;
            _currentPosition = _thirdPosition;
            transform.position = _thirdPosition.position;
        }
        launchTimer -= Time.deltaTime;
        if (launchTimer <= 0)
        {
            launchTimer = launchCd;
            Launch();
        }
    }
}
