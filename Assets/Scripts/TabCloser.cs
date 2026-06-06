using UnityEngine;

public class TabCloser : MonoBehaviour
{
    public void CloseTab(GameObject tab)
    {
        tab.SetActive(false);
    }
}
