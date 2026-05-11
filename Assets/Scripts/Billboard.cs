using UnityEngine;

public class Billboard : MonoBehaviour
{
    [SerializeField]
    private Camera mainCamera;

    private void LateUpdate()
    {
        Vector3 cameraPosition = mainCamera.transform.position;
        cameraPosition.y = transform.position.y;
        transform.LookAt(cameraPosition);
        transform.Rotate(0f, 180f, 0f);
    }
}
