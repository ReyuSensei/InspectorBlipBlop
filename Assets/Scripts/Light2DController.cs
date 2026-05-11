using UnityEngine;
using UnityEngine.InputSystem;

public class ImageFollower : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private float offset;

    void Update()
    {
        Vector3 mousePos = Mouse.current.position.ReadValue();
        mousePos.z = 10f;
        Vector3 targetPos = Camera.main.ScreenToWorldPoint(mousePos) * offset;
        targetPos.z = 0f;

        transform.localPosition = Vector3.Lerp(transform.localPosition, targetPos, speed * Time.deltaTime);
    }
}