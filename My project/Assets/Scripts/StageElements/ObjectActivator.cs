using UnityEngine;

public class ObjectActivator : MonoBehaviour
{
    [SerializeField] GameObject _unactive;

    public GameObject Unactive {  get { return _unactive; } set { _unactive = value; } }

    private void Awake()
    {
        _unactive.gameObject.SetActive(false);
    }
}
