using Unity.Cinemachine;
using UnityEditor.Animations;
using UnityEngine;

public class PlatformElevator : MonoBehaviour
{

    [SerializeField] private Animator anim;
    private bool isDown = true;
    private bool isInside = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
           
            if (isDown == true && !isInside)
            {
                anim.SetTrigger("_up");
                isDown = false;
                isInside = true;

            }
            else if (isDown == false && !isInside)
            {
                anim.SetTrigger("_down");
                isDown = true;
            }
     
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isInside = false;
        


        }
    }

}
