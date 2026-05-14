using UnityEngine;
using System.Collections.Generic;

public class GoalReachLocation : MonoBehaviour
{
    [SerializeField]
    private Quest quest;
    [SerializeField]
    private string targetId;
    [SerializeField]
    private List<GameObject> objectsToShow = new List<GameObject>();
    [SerializeField]
    private List<GameObject> objectsToHide = new List<GameObject>();

    public void OnTriggerEnter(Collider other)
    {
        if (quest != null && QuestManager.instance.activeQuest == quest)
        {
            if (QuestManager.instance.activeQuest.objectives[QuestManager.instance.activeObjectiveIndex].targetID == targetId)
            {
                    foreach (GameObject g in objectsToShow)
                    {
                        g.SetActive(true);
                    }
                    foreach (GameObject g in objectsToHide)
                    {
                        g.SetActive(false);
                    }
                    QuestManager.instance.CompleteObjective(QuestManager.instance.activeObjectiveIndex);
                    //Destroy(gameObject);
            }
        }
    }
}