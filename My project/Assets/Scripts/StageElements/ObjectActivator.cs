using System.Collections;
using UnityEngine;

public class ObjectActivator : MonoBehaviour
{
    Coroutine _activeRoutine = null;
    [SerializeField] GameObject _unactive;
    bool _active = false;

    private void Awake()
    {
        _unactive.gameObject.SetActive(false);
    }

    IEnumerator ActivateRoutine()
    {
        _active = true;
        Debug.Log("Active Routine Happening");
        _unactive.gameObject.SetActive(true);
        yield return new WaitForSeconds(5f);
        _unactive.gameObject.SetActive(false);
        Debug.Log("Active Routine Ending");
        _active = false;
    }

    public void Activate()
    {
        Debug.Log("Activate Called");
        if (_active == false)
        {
            Debug.Log("Active Routine Called");
            _activeRoutine = StartCoroutine(ActivateRoutine());
        }
    }
}
