using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    int _collected;
   

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
}
