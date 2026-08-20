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

    public Pool PhaseTwoPool;
    public Pool PhaseThreePool;

    public Queue<GameObject> _cannonPool2;
    public Queue<GameObject> _cannonPool3;
    Queue<GameObject> _currentPool;

    public Transform _secondPosition;
    public Transform _thirdPosition;

    public Phase Phase;

    [SerializeField] float _cannonballSpeed2;
    [SerializeField] float _cannonballSpeed3;
    protected override void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
         Phase = Phase.One;

        for (int i = 0; i < PhaseTwoPool.size; i++)
        {
            GameObject obj = Instantiate(PhaseTwoPool.prefab);
            obj.SetActive(false);
            Cannonball ball = obj.GetComponent<Cannonball>();
            if (ball != null)
            {
                ball.Start = _secondPosition.position;
                ball._speed = cannonballSpeed;
                ball.Destination = Direction.position;
                ball.SetSpeed();

            }
            _cannonPool2.Enqueue(obj);
        }

        for (int i = 0; i < PhaseThreePool.size; i++)
        {
            GameObject obj = Instantiate(PhaseThreePool.prefab);
            obj.SetActive(false);
            Cannonball ball = obj.GetComponent<Cannonball>();
            if (ball != null)
            {
                ball.Start = _thirdPosition.position;
                ball._speed = cannonballSpeed;
                ball.Destination = Direction.position;
                ball.SetSpeed();

            }
            _cannonPool3.Enqueue(obj);
        }

        _currentPool = _cannonPool;

        base.Start();
    }

   void Launch()
    {
        GameObject cannonball = _currentPool.Dequeue();
        Cannonball ball = cannonball.GetComponent<Cannonball>();
        ball.Destination = _player.transform.position;
        ball.SetSpeed();
        cannonball.SetActive(false);
        cannonball.SetActive(true);
        _currentPool.Enqueue(cannonball);
        Debug.Log($"Should Launch {_currentPool.Count}");
    }

    void Update()
    {
        if (Phase == Phase.Two)
        {
            _currentPool = _cannonPool2;
            transform.position = _secondPosition.position;
        }
        else if (Phase == Phase.Three)
        {
            _currentPool = _cannonPool3;
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
