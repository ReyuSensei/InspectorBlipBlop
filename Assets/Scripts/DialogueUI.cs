using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;

public class DialogueUI : MonoBehaviour
{
    public GameObject root;
    public TextMeshProUGUI speakerName;
    public TextMeshProUGUI dialogueText;
    public Image portraitSprite;
    public Animator anim;
    public GameObject choiceButtonPrefab;
    public Transform choiceParent;

    public float typeSpeed = 0.03f;
    private Coroutine typing;


    void Start()
    {
        DialogueManager.instance.ui = this;
        Hide();
    }

    public void Show(Node node)
    {
        root.SetActive(true);
        //RELLENO LOS CAMPOS CON LA INFORMACION PROVINIENTE DEL NODO
        speakerName.text = node.speaker;
        //dialogueText.text = node.message;
        portraitSprite.sprite = node.portrait;
        if (node.animCtrl != null)
        {
            anim.runtimeAnimatorController = node.animCtrl;
        }
        //ARRANCO LA CORUTINA QUE HARA EL EFECTO DE MAQUINA DE ESCRIBIR
        if (typing != null)
        {
            StopCoroutine(typing);
        }
        typing = StartCoroutine(TypeText(node.message));
        //INSTANCIO LAS OPCIONES DE RESPUESTAS
        SetupChoices(node);
    }
    
    IEnumerator TypeText(string text)
    {
        dialogueText.text = "";
        foreach (char c in text) //POR CADA CARACTER LLAMEMOSLE C EN EL TEXTO
        {
            dialogueText.text += c; //+= SUMO Y ASIGNO
            yield return new WaitForSeconds(typeSpeed);
        }
    }

    private void SetupChoices(Node node)
    {
        //BORRO LAS OPCIONES ANTERIORES
        foreach (Transform t in choiceParent)
        {
            Destroy(t.gameObject);
        }
        foreach (Choice c in node.choices)
        {
            GameObject button = Instantiate(choiceButtonPrefab, choiceParent);  //NO LE INDICO NI ROTACION NI POSICION, LE INDICO EL PADRE        }
            DialogueChoiceButton dialogueChoiceButton = button.GetComponent<DialogueChoiceButton>();
            if (dialogueChoiceButton != null)
            {
                dialogueChoiceButton.choice = c;
                dialogueChoiceButton.message.text = c.choiceText;
            }
        }

    } 
        
    public void Hide()
    {
        root.SetActive(false);
    }
}