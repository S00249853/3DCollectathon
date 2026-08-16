using UnityEngine;

public class FinalBossEnteranceHandler : MonoBehaviour
{
    [SerializeField] GameObject _finalBossEntrance;

    private void Awake()
    {
        _finalBossEntrance.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (GameManager.Instance.Collected >= 15)
        {
            _finalBossEntrance.gameObject.SetActive(true);
        }
    }
}
