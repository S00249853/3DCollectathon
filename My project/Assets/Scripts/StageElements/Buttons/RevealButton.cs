using UnityEngine;

public class RevealButton : MonoBehaviour, IStompable
{
    public GameObject[] _challenge;
    bool _activated;

    public bool Activated { get { return _activated; } }
    protected virtual void Awake()
    {
        foreach (var challenge in _challenge)
        {
            challenge.gameObject.SetActive(false);
        }
    }
    public virtual void Stomped()
    {
        _activated = true;
        Destroy(gameObject);
        foreach (var challenge in _challenge)
        {
            challenge.gameObject.SetActive(true);
        }
    }
}
