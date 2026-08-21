using System.Collections.Generic;
using UnityEngine;

public class KeepActive : MonoBehaviour
{
    public static List<string> Activated = new List<string>();

    void Start()
    {
        if (Activated.Contains(gameObject.name))
        {
            RevealButton button = gameObject.GetComponent<RevealButton>();
            button.Stomped();
        }
    }

    void OnDestroy()
    {
        Activated.Add(gameObject.name);
    }
}
