using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "Quest/Quest")]
public class Quest : ScriptableObject
{
    public string questName;
    public Sprite questIcon;
    public List<QuestObjective> objectives;

}
public enum E_OBJECTIVE_TYPE { COLLECT, REACHLOCATION, HIT, PUSHOBJECT }

[System.Serializable]   //ME PERMITE REPRESENTAR UNA CLASE QUE NO SEA UN COMPONENTE EN EL EDITOR
public class QuestObjective
{
    public E_OBJECTIVE_TYPE type;
    public string targetID;
    [TextArea]
    public string description;
}