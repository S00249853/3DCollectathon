using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    int _collected;
    Transform _checkpoint;
    PlayerStateMachine _player;

    public Transform Checkpoint { get { return _checkpoint; }  set { _checkpoint = value; } }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        _player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStateMachine>();
    }
        
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
}
