using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannon : MonoBehaviour
{

    public Queue<GameObject> _cannonPool;
    public Pool Cannonballs;
    public Transform LaunchPoint;
    public Transform Direction;
    [SerializeField] float launchCd;
    float launchTimer;
  
    void Start()
    {
        launchTimer = launchCd;
        _cannonPool = new Queue<GameObject>();

       
        for (int i = 0; i < Cannonballs.size; i++)
        {
            GameObject obj = Instantiate(Cannonballs.prefab);
            obj.SetActive(false);
            Cannonball ball = obj.GetComponent<Cannonball>();
            if (ball != null)
            {
                ball.Start = LaunchPoint.position;
                ball.Destination = Direction.position;

                ball.SetSpeed();
            }
            _cannonPool.Enqueue(obj);
        }
    }

    private void Launch()
    {
        GameObject cannonball = _cannonPool.Dequeue();
        cannonball.SetActive(false);
        cannonball.SetActive(true);
        _cannonPool.Enqueue(cannonball);
        Debug.Log($"Should Launch {_cannonPool.Count}");
    }
    
    void Update()
    {
        launchTimer -= Time.deltaTime;
        if (launchTimer <= 0)
        {
            launchTimer = launchCd;
            Launch();
        }
    }
}
