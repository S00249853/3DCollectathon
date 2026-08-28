using UnityEngine;

public class UiCloser : MonoBehaviour
{
    [SerializeField] GameObject _ui;
    private void Awake()
    {
        Cursor.visible = true;
    }
    public void CloseUI()
    {
        _ui.SetActive(false);
       Cursor.visible = false;
    }    
}
