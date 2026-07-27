using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    //Singleton
    public static GameManager Instance;

    //Variables
    private int _collected;
    private int _maxHealth = 100;
    private PlayerStateMachine _player;
    private Transform _checkpoint;
    [SerializeField] private TMP_Text _collectables;
    [SerializeField] private TMP_Text _challengeTimer;
    [SerializeField] private int _health;

    //Getters and Setters
    public int Collected { get { return _collected; } set { _collected = value; } }
    public int Health { get { return _health; } set { _health = value; } }
    public int MaxHealth { get { return _maxHealth; } set { _maxHealth = value; } }
    public TMP_Text ChallengeTimer { get { return _challengeTimer; } set { _challengeTimer = value; } }
    public Transform Checkpoint { get { return _checkpoint; }  set { _checkpoint = value; } }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnPlayerDeath()
    {
        CharacterController characterController = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterController>();
        characterController.enabled = false;
        characterController.transform.position = _checkpoint.position;
        characterController.enabled = true;
        _health = _maxHealth;
        _player.IsDead = false;
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
        _health = _maxHealth;
    }

    private void Update()
    {
        if (_health <= 0)
        {
            _player.IsDead = true;
            OnPlayerDeath();
        }
        _collectables.text = $": {_collected}";
    }
}
