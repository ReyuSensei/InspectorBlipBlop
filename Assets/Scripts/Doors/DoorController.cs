using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField]
    private Animator animator;


    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            animator.SetTrigger("enter");
        }
        
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            animator.SetTrigger("exit");
        }
    }
}
