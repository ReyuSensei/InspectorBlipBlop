using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class ExitShipEnding : MonoBehaviour, IInteractable
{
    public GameObject finalBPanel;
    public void Interact()
    {
        finalBPanel.SetActive(true);
    }

    public void EndingB()
    {
        SceneManager.LoadScene("EndB");
    }
}
