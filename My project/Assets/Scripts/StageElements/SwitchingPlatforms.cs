using UnityEngine;

public class SwitchingPlatforms : MonoBehaviour
{
    [SerializeField] private GameObject[] RightPlatforms;
    [SerializeField] private GameObject[] LeftPlatforms;
    [SerializeField] private float PlatformsCd;
    private float PlatformsCdTimer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        foreach (GameObject go in LeftPlatforms)
        {
            Renderer renderer = go.GetComponent<Renderer>();
            Collider collider = go.GetComponent<Collider>();
            renderer.enabled = false;
            collider.enabled = false;
            PlatformsCdTimer = PlatformsCd;
        }
    }
    private void Update()
    {
      if (PlatformsCdTimer > 0) 
        {
            PlatformsCdTimer -= Time.deltaTime;
        } 
      
      else
        {
            foreach (GameObject go in RightPlatforms)
            {
                Renderer renderer = go.GetComponent<Renderer>();
                Collider collider = go.GetComponent<Collider>();
                renderer.enabled = !renderer.enabled;
                collider.enabled = !collider.enabled;
            }

            foreach (GameObject go in LeftPlatforms)
            {
                Renderer renderer = go.GetComponent<Renderer>();
                Collider collider = go.GetComponent<Collider>();
                renderer.enabled = !renderer.enabled;
                collider.enabled = !collider.enabled;
            }
            PlatformsCdTimer = PlatformsCd;
        }
    }
}
