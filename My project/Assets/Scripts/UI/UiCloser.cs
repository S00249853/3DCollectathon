using System.Collections.Generic;
using UnityEngine;

public class UiCloser : MonoBehaviour
{
    public static List<string> UI = new List<string>();
    [SerializeField] GameObject _ui;
    void Start()
    {
        if (UI.Contains(gameObject.name))
        {
            Destroy(gameObject);
        }
        else
        {
            Cursor.visible = true;
        }
    }
    public void CloseUI()
    {
        Destroy(_ui);
       Cursor.visible = false;
    }

    void OnDestroy()
    {
       UI.Add(gameObject.name);   
    }
}
