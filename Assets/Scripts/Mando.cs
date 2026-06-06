using System.Collections;
using UnityEngine;
using Unity.Cinemachine;
using UnityEngine.UI;

public class Mando : MonoBehaviour
{
    public static Mando Instance;
    [SerializeField] private string actualCode;
    [SerializeField] private string code;
    [SerializeField] private Animator anim;
    [SerializeField] private Animator bttAnim;
    public GameObject mandoPanel;
    [SerializeField] private GameObject objectToShow;
    [SerializeField] private InteractableObject interactableObject;
    [SerializeField] private GameObject lights;
    [SerializeField] private GameObject blueLights;
    [SerializeField] private GameObject decals;
    [SerializeField] private GameObject decalsUV;
    public bool puzzleSolved = false;

    private void Start()
    {
        Instance = this;   
    }
    void CheckCode()
    {
        if (actualCode == code)
        {
            puzzleSolved = true;
            StartCoroutine(codeCorrect());
        }
    }

    public void AddColor(string letter)
    {
        if (puzzleSolved) return;

        if (actualCode.Length >= code.Length)
        {
            actualCode = actualCode.Substring(1);
        }
        actualCode += letter;
        CheckCode();
    }

    IEnumerator codeCorrect()
    {
        yield return new WaitForSeconds(2f);
        mandoPanel.SetActive(false);
        anim.SetTrigger("_start");
        //objectToShow.SetActive(true);
        interactableObject.isActive = false;
        interactableObject.OFF();
        yield return new WaitForSeconds(1f);
        lights.SetActive(false);
        decals.SetActive(false);
        blueLights.SetActive(true);
        decalsUV.SetActive(true);
        objectToShow.SetActive(true);
    }
}
