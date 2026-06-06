using UnityEngine;

public class OnEnterQuest : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            DialogueManager.instance.StartDialogue(GetComponent<NPCDialogue>().questOfferDialogue);
        }

        gameObject.SetActive(false);
    }
}
