using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;

public class QuestManager : MonoBehaviour
{
    public static QuestManager instance;
    public Quest activeQuest;
    public int activeObjectiveIndex;
    public QuestUI ui;

    private List<Quest> completedQuests = new List<Quest>();

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public bool TryStartQuest(Quest quest)
    {
        if(activeQuest != null)
        {
            return false;
        }
        activeQuest = quest;
        activeObjectiveIndex = 0;

        ui.ShowQuest(activeQuest, activeObjectiveIndex);

        return true;
    }

    public void CompleteObjective(int objectiveIndexToCompare)
    {
        if(activeQuest == null)
        {
            return;
        }
        if(activeObjectiveIndex != objectiveIndexToCompare)
        {
            return;
        }
        activeObjectiveIndex = activeObjectiveIndex + 1;
        if(activeObjectiveIndex >= activeQuest.objectives.Count)
        {
            CompleteQuest();
        }
        else
        {
            ui.ShowQuest(activeQuest, activeObjectiveIndex);
        }
    }

    void CompleteQuest()
    {
        completedQuests.Add(activeQuest);
        activeQuest = null;

        ui.HideQuest();
    }

    public bool HasActiveQuest()
    {
        return activeQuest != null;
    }

    public bool HasCompleted(Quest quest)
    {
        return completedQuests.Contains(quest);
    }
}