using UnityEngine;

public class Cannonball : MonoBehaviour
{
    public Vector3 Start;
    public Vector3 Destination;
    float _timeToDestination;
    float _elapsedTime;

    void Update()
    {
        if (gameObject.activeSelf == true)
        {
            _elapsedTime += Time.deltaTime;

            float elapsedPercentage = _elapsedTime / _timeToDestination;
            transform.position = Vector3.Lerp(Start, Destination, elapsedPercentage);
        }
    }

    public void SetSpeed()
    {
        float distanceToWaypoint = Vector3.Distance(Start, Destination);
        _timeToDestination = distanceToWaypoint / 6f;
        transform.forward = Destination;
    }

    private void OnEnable()
    {
        _elapsedTime = 0;
        transform.position = Start;
    }

    private void OnCollisionEnter(Collision collision)
    {
        gameObject.SetActive(false);
    }
}
