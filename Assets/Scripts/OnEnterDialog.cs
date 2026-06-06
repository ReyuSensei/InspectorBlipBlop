using UnityEngine;

public class OnEnterDialog : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            DialogueManager.instance.StartDialogue(GetComponent<NPCDialogue>().defaultDialogue);
        }

        Destroy(gameObject);
    }
}
