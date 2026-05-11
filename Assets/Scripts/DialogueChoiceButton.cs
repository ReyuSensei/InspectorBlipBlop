using UnityEngine;
using TMPro;

public class DialogueChoiceButton : MonoBehaviour
{
    public Choice choice;
    public TextMeshProUGUI message;

    public void ChooseChoice()
    {
        DialogueManager.instance.ChooseChoice(choice);
    }
}
