using System.Collections;
using UnityEngine;

public class ObjectActivator : MonoBehaviour
{
    Coroutine _activeRoutine = null;
    [SerializeField] GameObject _unactive;

    private void Awake()
    {
        _unactive.gameObject.SetActive(false);
    }

    IEnumerator ActivateRoutine()
    {
        Debug.Log("Active Routine Happening");
        _unactive.gameObject.SetActive(true);
        yield return new WaitForSeconds(5f);
        _unactive.gameObject.SetActive(false);
        Debug.Log("Active Routine Ending");
        
    }

    public void Activate()
    {
        Debug.Log("Activate Called");
        if (_activeRoutine == null)
        {
            Debug.Log("Active Routine Called");
            _activeRoutine = StartCoroutine(ActivateRoutine());
        }
    }
}
