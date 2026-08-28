using UnityEngine;

public class MainMenu : MonoBehaviour
{
   public void Return()
    {
        GameManager.Instance.HideInventory();
    }

    public void Quit()
    {
        Application.Quit();
    }
}
