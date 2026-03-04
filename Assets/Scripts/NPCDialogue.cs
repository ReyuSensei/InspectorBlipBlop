using UnityEngine;

public class NPCDialogue : MonoBehaviour, IInteractable
{
    //CASO 1: EL NPC SOLO OFRECE CONVERSACION SIN ESTAR VINCULADA A UNA QUEST
    [Header("General")]
    public Node defaultDialogue;

    //CASO 2: EL NPC OFRECE UNA QUEST EN ALGUN MOMENTO DE LA CONVERSACION   
    [Header("Quest Giver")]
    public Quest questToGive;
    public Node questOfferDialogue;
    public Node endQuestDialogue;
    public Node busyQuestDialogue;

    //CASO 3: EL NPC DA INFORMACION SOBRE UNA QUEST QUE YA TE HAN OFRECIDO EN OTRO SITIO
    [Header("Quest Related")]
    public Quest relatedQuest;
    public Node beforeQuestDialogue;
    public Node duringQuestDialogue;
    public Node afterQuestDialogue;

    public void Interact()
    {
        //SI EL DIALOGO VA VINCULADO A UNA QUEST

        //SI EL DIALOGO DA UNA QUEST NUEVA

        //DIALOGO ESTANDAR
        DialogueManager.instance.StartDialogue(defaultDialogue);
    }

}
