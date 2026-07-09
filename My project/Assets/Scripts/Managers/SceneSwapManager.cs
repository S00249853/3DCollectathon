using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneSwapManager : MonoBehaviour
{
    public static SceneSwapManager Instance;
    public string PlayersSpawn;
    public bool LoadFromTransition;
    private GameObject _player;
    [SerializeField]private Transform _spawnLocation;
    [SerializeField] Image _image;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene,LoadSceneMode mode)
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        Debug.Log($"Player is {_player.name} and player is at {_player.transform.position}");
        if (LoadFromTransition)
        {
            CharacterController player = _player.GetComponent<CharacterController>();
            FindSpawn(PlayersSpawn);
            player.enabled = false;
            player.transform.position = _spawnLocation.position;
            player.enabled = true;
            Debug.Log($"Player is {_player.name} and player is at {_player.transform.position}");
            LoadFromTransition = false;
        }
        StartCoroutine(FadeOut());
    }
    public void FadeAndLoad(string sceneName, float duration)
    {
        StartCoroutine(Fade(sceneName, duration));
    }
    IEnumerator Fade(string sceneName, float duration)
    {
        float t = 0;
        Color c = _image.color;
        while (t < duration)
        {
            t += Time.deltaTime;
            c.a = t / duration;
            _image.color = c;
            yield return null;
        }

        SceneManager.LoadScene(sceneName);
    }
    IEnumerator FadeOut()
    {
        float t = 0;
        Color c = _image.color;
        while (t < 1)
        {
            t += Time.deltaTime;
            c.a = 1f - (t / 1f);
            _image.color = c;
            yield return null;
        }
    }

    private void FindSpawn(string name)
    {
        SceneFader[] spawns = FindObjectsByType<SceneFader>(FindObjectsSortMode.None);

        for (int i = 0; i < spawns.Length; i++)
        {
            if (spawns[i].ThisTag == name)
            {
                _spawnLocation = spawns[i].Spawner;

                return;
            }
        }
    }
}
