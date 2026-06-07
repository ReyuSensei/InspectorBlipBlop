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

    [SerializeField] private Animator anim;

    public void ShowQuest(Quest questToShow, int objectiveIndex)
    {
        anim.SetBool("Show", true);
        questName.text = questToShow.questName;
        objectiveDesc.text = questToShow.objectives[objectiveIndex].description;
        icon.sprite = questToShow.questIcon;
    }

    public void HideQuest()
    {
        anim.SetBool("Show", false);
    }

    private void Start()
    {
        QuestManager.instance.ui = this;
    }
}
