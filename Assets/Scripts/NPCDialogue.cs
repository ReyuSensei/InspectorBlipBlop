using System.Collections.Generic;
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
    public Node activeQuestDialogue;

    //CASO 3: EL NPC DA INFORMACION SOBRE UNA QUEST QUE YA TE HAN OFRECIDO EN OTRO SITIO
    [Header("Quest Related")]
    public Quest relatedQuest;
    public Node beforeQuestDialogue;
    public Node duringQuestDialogue;
    public Node afterQuestDialogue;

    public void Interact()
    {
        //SI EL DIALOGO VA VINCULADO A UNA QUEST QUE AUN NO HA EMPEZADO
        if (relatedQuest != null && relatedQuest != QuestManager.instance.activeQuest)
        {
            DialogueManager.instance.StartDialogue(beforeQuestDialogue);
            return;
        }
        //SI EL DIALOGO VA VINCULADO A UNA QUEST ACTIVA
        if (relatedQuest != null && relatedQuest == QuestManager.instance.activeQuest)
        {
            DialogueManager.instance.StartDialogue(duringQuestDialogue);
            return;
        }
        //SI EL DIALOGO VA VINCULADO A UNA QUEST PERO YA HA FINALIZADO
        if(relatedQuest != null && QuestManager.instance.HasCompleted(relatedQuest))
        {
            DialogueManager.instance.StartDialogue(afterQuestDialogue);
            return;
        }

        //SI EL DIALOGO DA UNA QUEST NUEVA
        //SI YA TENGO OTRA QUEST ACTIVA
        if(questToGive != null && QuestManager.instance.activeQuest != null && questToGive != QuestManager.instance.activeQuest)
        {
            DialogueManager.instance.StartDialogue(busyQuestDialogue);
            return;
        }
        //SI YA HE COMPLETADO ESTA QUEST
        if(questToGive != null && QuestManager.instance.HasCompleted(questToGive))
        {
            DialogueManager.instance.StartDialogue(endQuestDialogue);
            return;
        }
        //CUANDO SIMPLEMENTE ME DA LA QUEST
        if (questToGive != null && QuestManager.instance.activeQuest == null)
        {
            DialogueManager.instance.StartDialogue(questOfferDialogue);
            return;
        }
        //CUANDO ESTA MISMA QUEST ESTÁ ACTIVA
        if (questToGive != null && questToGive == QuestManager.instance.activeQuest)
        {
            DialogueManager.instance.StartDialogue(activeQuestDialogue);
            return;
        }

        //DIALOGO ESTANDAR
        DialogueManager.instance.StartDialogue(defaultDialogue);
    }

}
