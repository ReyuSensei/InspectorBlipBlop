using UnityEngine;
using System.Collections.Generic;

public class GoalInteract : MonoBehaviour, IInteractable
{
    [SerializeField]
    private Quest quest;
    [SerializeField]
    private string targetId;
    [SerializeField]
    private string itemId;
    [SerializeField]
    private List<GameObject> objectsToShow = new List<GameObject>();
    [SerializeField]
    private List<GameObject> objectsToHide = new List<GameObject>();


    public void Interact()
    {

        if (quest != null && QuestManager.instance.activeQuest == quest)
        {
            if(QuestManager.instance.activeQuest.objectives[QuestManager.instance.activeObjectiveIndex].targetID == targetId)
            {
                InventoryManager.instance.Has(itemId);
                foreach (GameObject g in objectsToShow)
                {
                    g.SetActive(true);
                }
                foreach (GameObject g in objectsToHide)
                {
                    g.SetActive(false);
                }
                QuestManager.instance.CompleteObjective(QuestManager.instance.activeObjectiveIndex);
                //Destroy(gameObject);    //DESTRUIRÁ TODO EL GAMEOBJECT
                //Destroy(this); SOLO BORRARÍA EL SCRIPT PERO DEJARÍA EL OBJETO
            }
        }
    }
}
