using UnityEngine;
using System.Collections.Generic;

/*
[CreateAssetMenu(menuName = "Dialogue/Dialogue")]
public class Dialogue : ScriptableObject
{
    public Node startNode;
}
*/

[CreateAssetMenu(menuName = "Dialogue/Node")]
public class Node : ScriptableObject
{
    public Sprite portrait;
    public RuntimeAnimatorController animCtrl;
    public string speaker;
    [TextArea]  //CONVIERTE EL TEXTO EN MULTILINEA
    public string message;
    public List<Choice> choices;
    public bool endsDialogue;
}

[System.Serializable]
public class Choice
{
    public string choiceText;
    public Node nextNode;
    public Quest questToStart;
}