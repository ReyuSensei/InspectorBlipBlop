using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class CodeInput : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI codigo;
    [SerializeField] private GameObject codePanel;
    [SerializeField] private string codigoSecreto;
    [SerializeField] private GameObject blockedDoor;
    public bool deactive;


    void Start()
    {
        codigo.text = null;
        deactive = false;
    }


    void Update()
    {
        HandleCode();

    }

    public void Number0()
    {
        codigo.text += 0;
    }

    public void Number1()
    {
        codigo.text += 1;
    }

    public void Number2()
    {
        codigo.text += 2;
    }

    public void Number3()
    {
        codigo.text += 3;
    }

    public void Number4()
    {
        codigo.text += 4;
    }

    public void Number5()
    {
        codigo.text += 5;
    }

    public void Number6()
    {
        codigo.text += 6;
    }

    public void Number7()
    {
        codigo.text += 7;
    }

    public void Number8()
    {
        codigo.text += 8;
    }

    public void Number9()
    {
        codigo.text += 9;
    }

    void HandleCode()
    {
        if(codigo.text.Length == 4 && !deactive) 
        { 
            if(codigo.text == codigoSecreto)
        
            {
                BlockedDoor doorScript = blockedDoor.GetComponent<BlockedDoor>();
                doorScript.isBlocked = false;
                codigo.color = Color.green;
                StartCoroutine(HandleCorrect());


            } else if(codigo.text != "XXXX")
            {
                StartCoroutine(HandleError());
            }

        }
    }

    private IEnumerator HandleError()
    {
        string error = "XXXX";
        codigo.text = error;
        codigo.color = Color.red;
        yield return new WaitForSeconds(0.3f);
        codigo.color = new Color(codigo.color.r, codigo.color.g, codigo.color.b, 0);
        yield return new WaitForSeconds(0.1f);
        codigo.color = new Color(codigo.color.r, codigo.color.g, codigo.color.b, 255);
        yield return new WaitForSeconds(0.3f);
        codigo.color = new Color(codigo.color.r, codigo.color.g, codigo.color.b, 0);
        yield return new WaitForSeconds(0.1f);
        codigo.color = new Color(codigo.color.r, codigo.color.g, codigo.color.b, 255);
        yield return new WaitForSeconds(1f);
        codigo.color = Color.white;
        codigo.text = null;
    }

    private IEnumerator HandleCorrect()
    {
        yield return new WaitForSeconds(1.5f);
        codePanel.SetActive(false);
        deactive = true;
    }

}
