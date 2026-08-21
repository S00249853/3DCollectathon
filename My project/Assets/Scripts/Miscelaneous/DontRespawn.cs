using System.Collections.Generic;
using UnityEngine;

public class DontRespawn : MonoBehaviour
{

    public static List<string> Collected = new List<string>();

    void Start()
    {
        if (Collected.Contains(gameObject.name))
        {
            Destroy(gameObject);
        }
    }

    void OnDestroy()
    {
        Collected.Add(gameObject.name);
    }
}


