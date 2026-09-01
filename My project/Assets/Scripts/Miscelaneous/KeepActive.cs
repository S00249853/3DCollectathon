using System.Collections.Generic;
using UnityEngine;

public class KeepActive : MonoBehaviour
{
    public static List<string> Activated = new List<string>();
    [SerializeField] RevealButton _button;

    void Start()
    {
        if (Activated.Contains(gameObject.name))
        {
            _button.Stomped();
        }
    }

    void OnDestroy()
    {
        if (_button.Activated == true)
        {
            Activated.Add(gameObject.name);
        }
    }
}
