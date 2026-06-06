using UnityEngine;

public class PlatformParent : MonoBehaviour
{
    // En el script del ascensor
    private Transform player;
    private Vector3 lastPosition;

    void Update()
    {
        Vector3 deltaMovement = transform.position - lastPosition;
        lastPosition = transform.position;

        if (player != null)
        {
            CharacterController cc = player.GetComponent<CharacterController>();
            if (cc != null)
                cc.Move(deltaMovement);
            else
                player.position += deltaMovement;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            player = other.transform;
            lastPosition = transform.position;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            player = null;
    }
}
