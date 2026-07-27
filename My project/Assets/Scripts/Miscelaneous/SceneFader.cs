using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static UnityEditor.Experimental.GraphView.GraphView;

public class SceneFader : MonoBehaviour
{ 
    [SerializeField] string _nextScene;
    [SerializeField] string _nextSceneSpawnerTag;
    [SerializeField] string _spawnerTag;
    [SerializeField] Transform _spawner;

    public string DestinationSpawner { get { return _nextSceneSpawnerTag; } }
    public string ThisTag { get { return _spawnerTag; } }
    public Transform Spawner { get { return _spawner; } }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {  
            PlayerStateMachine player = other.gameObject.GetComponent<PlayerStateMachine>();
            player.StopMoving = true;
            SceneSwapManager.Instance.LoadFromTransition = true;
            SceneSwapManager.Instance.PlayersSpawn = _nextSceneSpawnerTag;
            SceneSwapManager.Instance.FadeAndLoad(_nextScene, 3);
        }
    }


}
