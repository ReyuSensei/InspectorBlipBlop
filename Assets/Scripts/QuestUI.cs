using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class QuestUI : MonoBehaviour
{
    [SerializeField]
    private GameObject rootPanel;
    [SerializeField]
    private TextMeshProUGUI questName;
    [SerializeField]
    private TextMeshProUGUI objectiveDesc;
    [SerializeField]
    private Image icon;

    public void ShowQuest(Quest questToShow, int objectiveIndex)
    {
        rootPanel.SetActive(true);
        questName.text = questToShow.questName;
        objectiveDesc.text = questToShow.objectives[objectiveIndex].description;
        icon.sprite = questToShow.questIcon;
    }

    public void HideQuest()
    {
        rootPanel.SetActive(false);
    }

    private void Start()
    {
        QuestManager.instance.ui = this;
    }
}
