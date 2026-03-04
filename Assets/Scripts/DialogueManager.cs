using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager instance;
    public DialogueUI ui;
    private Node currentNode;
    public bool isActive;

    void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void StartDialogue(Node node)
    {
        currentNode = node;
        isActive = true;
        ui.Show(node);
    }

    public void EndDialogue()
    {
        isActive = false;
        ui.Hide();
    }

    public void ChooseChoice(Choice choice)
    {
        //SI LA OPCION OFRECE UNA QUEST SE DEBERIA INICIAR
        if(choice.nextNode == null)
        {
            EndDialogue();
            return;
        }
        currentNode = choice.nextNode;
        ui.Show(currentNode);
    }
}
