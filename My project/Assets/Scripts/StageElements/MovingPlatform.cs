using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField] Transform[] _waypoints;
    Transform _previousWaypoint;
    Transform _targetWaypoint;
    int _index;
    [SerializeField] float _speed;
    float _elapsedTime;
    float _timeToWaypoint;

    private void Start()
    {
        _index = 0;
        GetWaypoint();
    }

    private void Update()
    {
        _elapsedTime += Time.deltaTime;

        float elapsedPercentage = _elapsedTime / _timeToWaypoint;
         elapsedPercentage = Mathf.SmoothStep(0, 1, elapsedPercentage);
        transform.position = Vector3.Lerp(_previousWaypoint.position, _targetWaypoint.position, elapsedPercentage);

        if (elapsedPercentage >= 1)
        {
            GetWaypoint();
        }
        
    }

    private void GetWaypoint()
    {
        _previousWaypoint = _waypoints[_index];
        _index++;
        if (_waypoints.Length <= _index )
        {
            _index = 0;
        }
        _targetWaypoint = _waypoints[_index];

        _elapsedTime = 0;

        float distanceToWaypoint = Vector3.Distance(_previousWaypoint.position, _targetWaypoint.position);
        _timeToWaypoint = distanceToWaypoint / _speed;
    }

    private void OnTriggerEnter(Collider other)
    {
       
        if (other.gameObject.tag == "Player")
        {
            Debug.Log("Platform Entered");
            Debug.Log($"Parent is {other.gameObject.transform.parent}");
            other.transform.SetParent(transform);
            Debug.Log($"Parent now is {other.gameObject.transform.parent}");
        }
        
    }

    private void OnTriggerExit(Collider other)
    {
     
        if (other.gameObject.tag == "Player")
        {
            Debug.Log("Platform Exited");
            other.gameObject.transform.parent = null;
        }
    }
}
