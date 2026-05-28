using UnityEngine;

public class NPCHeadLook : MonoBehaviour
{
    [Header("Referencias")]
    public Transform neckBone;
    public Transform player;

    [Header("Rotación")]
    public float rotationSpeed = 5f;

    public float maxAngle = 60f;

    private Quaternion initialRotation;
    private bool playerInside;

    void Start()
    {
        initialRotation = neckBone.localRotation;
    }

    void Update()
    {
        if (playerInside && player != null)
        {
            LookAtPlayer();
        }
        else
        {
            neckBone.localRotation = Quaternion.Slerp(
                neckBone.localRotation,
                initialRotation,
                Time.deltaTime * rotationSpeed
            );
        }
    }

    void LookAtPlayer()
    {
        
        Vector3 direction = player.position - neckBone.position;

        Quaternion targetRotation = Quaternion.LookRotation(direction);

        Quaternion localTarget =
            Quaternion.Inverse(neckBone.parent.rotation) * targetRotation;

        Vector3 euler = localTarget.eulerAngles;

        if (euler.y > 180) euler.y -= 360;

        euler.x = 0;
        euler.z = 0;

        euler.y = Mathf.Clamp(euler.y, -maxAngle, maxAngle);

        Quaternion finalRot = Quaternion.Euler(euler);

        neckBone.localRotation = Quaternion.Slerp(
            neckBone.localRotation,
            finalRot,
            Time.deltaTime * rotationSpeed
        );
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = true;
            player = other.transform;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = false;
            player = null;
        }
    }
}
