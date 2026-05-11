using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CodeInput : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI codigo;
    [SerializeField] private string codigoSecreto;

    void Start()
    {
        codigo.text = null;
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
        if(codigo.text.Length == 4) 
        { 
            if(codigo.text == codigoSecreto)
        
            {
                codigo.color = Color.green;
            } else
            {
                codigo.text = null;
            }

        }
    }

}
