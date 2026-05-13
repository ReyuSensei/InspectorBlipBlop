using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class BlockedDoor : MonoBehaviour
{
    [SerializeField]
    private Animator animator;
    public bool isBlocked;



    private void Update()
    {
        if(!isBlocked) animator.SetTrigger("enter");
    }
}
