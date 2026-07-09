using UnityEngine;

public class Teleporter : MonoBehaviour
{
    [SerializeField] Transform _teleporter;
    
    public Transform TeleportSpawn {  get { return _teleporter; } }
}
