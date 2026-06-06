using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField]
    private Animator animator;
    [SerializeField] private AudioClip clipClose;
    [SerializeField] private AudioClip clipOpen;
    [SerializeField] private AudioSource audioSource;


    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            audioSource.PlayOneShot(clipOpen);
            animator.SetTrigger("enter");
        }
        
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            audioSource.PlayOneShot(clipClose);
            animator.SetTrigger("exit");
        }
    }
}
