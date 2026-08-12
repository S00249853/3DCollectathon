using UnityEngine;

public class BattleArena : MonoBehaviour
{
    [SerializeField] GameObject[] _enemies;
    [SerializeField] GameObject[] _activated;
    int _completion;
    int _maxCompletion;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        foreach (GameObject active in _activated)
        {
            active.SetActive(false);
        }
        _maxCompletion = _enemies.Length;
    }

    // Update is called once per frame
    void Update()
    {
        foreach (GameObject enemy in _enemies)
        {
            if (enemy == null)
            {
                _completion++;
            }
        }

        if (_completion == _maxCompletion)
        {
            foreach (GameObject active in _activated)
            {
                active.SetActive(true);
            }
        }
        else
        {
            _completion = 0;
        }
    }
}
