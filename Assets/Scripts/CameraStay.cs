using UnityEngine;
using Unity.Cinemachine;

public class CameraStay : MonoBehaviour
{
    [SerializeField] private CinemachineCamera _camera;
    [SerializeField] private GameObject dialogue;
    [SerializeField] private GameObject codePanel;
    [SerializeField] private GameObject clockPanel;
    [SerializeField] private GameObject pipePanel;
    [SerializeField] private GameObject colorPanel;
    [SerializeField] private GameObject gamePanel;
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
        if (dialogue.activeSelf || codePanel.activeSelf || clockPanel.activeSelf || pipePanel.activeSelf || colorPanel.activeSelf || gamePanel.activeSelf)
        {
            cameraControl.enabled = false;
        }
        else
        {
            cameraControl.enabled = true;
        }
    }
}
