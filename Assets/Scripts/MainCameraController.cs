using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;
using System;

public class MainCameraController : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private float cameraOffset;
    private GameObject player;
    [SerializeField] private float rotationSpeed;
    private int currentCamera;

    [SerializeField] private List<GameObject> cameraPoints = new List<GameObject>();

    void Start()
    {
        player = GameManager.instance.playerController.gameObject;
    }


    void Update()
    {
        Vector3 playerDirection = (player.transform.position - mainCamera.transform.position).normalized;
        Quaternion targetRotation = Quaternion.LookRotation(playerDirection);
        mainCamera.transform.rotation = Quaternion.Slerp(mainCamera.transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }

    void OnTriggerEnter(Collider other)
    {
        if (cameraPoints.Contains(other.gameObject))
        {
            mainCamera.transform.position = other.transform.position;
            Vector3 playerDirection = (player.transform.position - mainCamera.transform.position).normalized;
            Quaternion targetRotation = Quaternion.LookRotation(playerDirection);
            mainCamera.transform.rotation = targetRotation;
        }
    }


}
