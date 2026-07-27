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
            if (challenge.activeSelf == true )
            {
                challenge.SetActive(false);
            }
            else if (challenge.activeSelf == false)
            {
                challenge.SetActive(true);
            }

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
            if (challenge.activeSelf == true)
            {
                challenge.SetActive(false);
            }
            else if (challenge.activeSelf == false)
            {
                challenge.SetActive(true);
            }
        }
    }

    private void Update()
    {
        if ( _onTimer > 0)
        {
            GameManager.Instance.ChallengeTimer.text = Mathf.Floor(_onTimer).ToString();
           _onTimer -= Time.deltaTime;
        }
        if (_activated && _onTimer <= 0)
        {
            GameManager.Instance.ChallengeTimer.text = "";
            _activated = false;
            ResetChallenge();
        }
    }
}
