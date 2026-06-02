using System.Collections;
using UnityEngine;
using Unity.Cinemachine;
using UnityEngine.UI;

public class LockBox : MonoBehaviour
{
    [SerializeField] private string actualCode;
    [SerializeField] private string code;
    [SerializeField] private Animator anim;
    [SerializeField] private Animator bttAnim;
    [SerializeField] private GameObject lockPanel;
    [SerializeField] private Button BttR;
    [SerializeField] private Button BttB;
    [SerializeField] private Button BttY;
    [SerializeField] private Button BttG;
    [SerializeField] private GameObject objectToShow;
    [SerializeField] private InteractableObject interactableObject;
    public bool puzzleSolved = false;

    void CheckCode()
    {
        if (actualCode == code)
        {
            puzzleSolved = true;
            BttR.interactable = false;
            BttB.interactable = false;
            BttY.interactable = false;
            BttG.interactable = false;
            StartCoroutine(codeCorrect());
        }
    }

    private void AddColor(char color)
    {
        if (puzzleSolved) return;

        if (actualCode.Length >= code.Length)
        {
            actualCode = actualCode.Substring(1);
        }
        actualCode += color;
        CheckCode();
    }

    public void Red()
    {
        AddColor('R');
    }

    public void Blue()
    {
        AddColor('B');
    }

    public void Green()
    {
        AddColor('G');
    }

    public void Yellow()
    {
        AddColor('Y');
    }
    IEnumerator codeCorrect()
    {
        bttAnim.SetTrigger("_on");
        yield return new WaitForSeconds(2f);
        lockPanel.SetActive(false);
        anim.SetTrigger("_open");
        objectToShow.SetActive(true);
        interactableObject.isActive = false;
        interactableObject.OFF();
    }
}
