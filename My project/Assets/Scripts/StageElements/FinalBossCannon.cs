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

    public Transform _SecondPosition;
    public Transform _ThirdPosition;

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
                ball.Start = _SecondPosition.position;
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
                ball.Start = _ThirdPosition.position;
                ball._speed = cannonballSpeed;
                ball.Destination = Direction.position;
                ball.SetSpeed();

            }
            _cannonPool3.Enqueue(obj);
        }

        base.Start();
    }

   void Launch(Queue<GameObject> queue)
    {
        GameObject cannonball = queue.Dequeue();
        Cannonball ball = cannonball.GetComponent<Cannonball>();
        ball.Destination = _player.transform.position;
        ball.SetSpeed();
        cannonball.SetActive(false);
        cannonball.SetActive(true);
        queue.Enqueue(cannonball);
        Debug.Log($"Should Launch {queue.Count}");
    }

    void Update()
    {
        launchTimer -= Time.deltaTime;
        if (launchTimer <= 0)
        {
            launchTimer = launchCd;
            if (Phase == Phase.One)
            {
                Launch(_cannonPool);
            }
            else if (Phase == Phase.Two)
            {
                Launch(_cannonPool2);
                transform.position = _SecondPosition.position;
            }
            else if (Phase == Phase.Three)
            {
                Launch(_cannonPool3);
                transform.position = _ThirdPosition.position;
            }
        }
    }
}
