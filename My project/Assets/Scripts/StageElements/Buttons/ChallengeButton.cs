using UnityEngine;

public class ChallengeButton : MonoBehaviour, IStompable
{
    public GameObject[] _challenge;
    [SerializeField] private float _on;
    private float _onTimer;
    private Collider collider;
    private MeshRenderer mesh;
    private bool _activated;
    private void Awake()
    {
        collider = GetComponent<Collider>();
        mesh = GetComponent<MeshRenderer>();
        ResetChallenge();

    }
    private void ResetChallenge()
    {
       collider.enabled = true;
        mesh.enabled = true;
        foreach (GameObject challenge in _challenge)
        {
            challenge.SetActive(false);
        }
    }
    public void Stomped()
    {
        _onTimer = _on;
        _activated = true;
        collider.enabled = false;
        mesh.enabled = false;
        foreach (GameObject challenge in _challenge)
        {
            challenge.SetActive(true);
        }
    }

    private void Update()
    {
        if ( _onTimer > 0)
        {
            _onTimer -= Time.deltaTime;
        }
        if (_activated && _onTimer <= 0)
        {
            _activated = false;
            _onTimer = _on;
            ResetChallenge();
        }
    }
}
