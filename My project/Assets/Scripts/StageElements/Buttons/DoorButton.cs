using UnityEngine;

public class DoorButton : MonoBehaviour , IStompable
{
    [SerializeField] private GameObject _door;

    public void Stomped()
    {
        Destroy(_door);
        Destroy(this.gameObject);
    }
}
