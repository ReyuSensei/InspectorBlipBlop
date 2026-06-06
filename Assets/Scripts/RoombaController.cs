using System.Collections;
using UnityEngine;

public class RoombaController : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float rotationMin;
    [SerializeField] private float rotationMax;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float distanceDetect;
    [SerializeField] private LayerMask obstacleLayer;
    private bool isRotating;

    void Update()
    {
        HandleMovement();
    }

    void HandleMovement()
    {
        if (!isRotating)
            transform.Translate(Vector3.forward * speed * Time.deltaTime);

        Debug.DrawRay(transform.position, transform.forward * distanceDetect, Color.red);

        if (!isRotating && Physics.Raycast(transform.position, transform.forward, distanceDetect, obstacleLayer))
        {
            isRotating = true;
            float randomRotation = Random.Range(rotationMin, rotationMax);
            StartCoroutine(Rotate(randomRotation));
        }
    }

    IEnumerator Rotate(float rotation)
    {
        Quaternion targetRotation = transform.rotation * Quaternion.Euler(0, rotation, 0);

        while (Quaternion.Angle(transform.rotation, targetRotation) > 0.5f)
        {
            transform.rotation = Quaternion.RotateTowards(
                transform.rotation,
                targetRotation,
                rotationSpeed * Time.deltaTime
            );
            yield return null;
        }

        transform.rotation = targetRotation;
        isRotating = false;
    }
}