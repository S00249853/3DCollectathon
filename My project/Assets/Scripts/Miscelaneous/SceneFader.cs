using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneFader : MonoBehaviour
{
    [SerializeField] Image _image;
    [SerializeField] string Destination;
    [SerializeField] Transform _direction;

    private void Start()
    {
        StartCoroutine(FadeOut());
    }
    private void FadeAndLoad(string sceneName, float duration)
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            CharacterController player = other.GetComponent<CharacterController>();
            player.Move(_direction.position * Time.deltaTime);
            FadeAndLoad(Destination, 3);
        }
    }


}
