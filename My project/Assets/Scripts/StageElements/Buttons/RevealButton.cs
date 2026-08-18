using UnityEngine;

public class RevealButton : MonoBehaviour, IStompable
{
    public GameObject[] _challenge;
    protected virtual void Awake()
    {
        foreach (var challenge in _challenge)
        {
            challenge.gameObject.SetActive(false);
        }
    }
    public virtual void Stomped()
    {
        Destroy(gameObject);
        foreach (var challenge in _challenge)
        {
            challenge.gameObject.SetActive(true);
        }
    }
}
