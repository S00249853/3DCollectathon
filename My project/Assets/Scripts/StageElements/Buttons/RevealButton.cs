using UnityEngine;

public class RevealButton : MonoBehaviour, IStompable
{
    public GameObject[] _challenge;
    private void Awake()
    {
        foreach (var challenge in _challenge)
        {
            challenge.gameObject.SetActive(false);
        }
    }
    public void Stomped()
    {
        Destroy(gameObject);
        foreach (var challenge in _challenge)
        {
            challenge.gameObject.SetActive(true);
        }
    }
}
