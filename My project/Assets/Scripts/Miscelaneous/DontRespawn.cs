using System.Collections.Generic;
using UnityEngine;

public class DontRespawn : MonoBehaviour
{

    public static List<string> Collected = new List<string>();
    [SerializeField] Collectable _collectable;

    void Start()
    {
        if (Collected.Contains(gameObject.name))
        {
            Destroy(gameObject);
        }
    }

    void OnDestroy()
    {
        if (_collectable.Collected == true)
        {
            Collected.Add(gameObject.name);
        }
    }
}


