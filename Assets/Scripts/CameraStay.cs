using UnityEngine;
using Unity.Cinemachine;

public class CameraStay : MonoBehaviour
{
    [SerializeField] private CinemachineCamera _camera;
    [SerializeField] private GameObject dialogue;
    [SerializeField] private GameObject codePanel;
    private CinemachineInputAxisController cameraControl;

    void Start()
    {
        cameraControl = _camera.GetComponent<CinemachineInputAxisController>();
    }

    void Update()
    {
        HandleCameraControl();
    }

    void HandleCameraControl()
    {
        if (dialogue.activeSelf || codePanel.activeSelf)
        {
            cameraControl.enabled = false;
        }
        else
        {
            cameraControl.enabled = true;
        }
    }
}
