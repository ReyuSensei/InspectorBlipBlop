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
    [SerializeField] private GameObject pausePanel;
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
        if (dialogue.activeSelf || codePanel.activeSelf || clockPanel.activeSelf || pipePanel.activeSelf || colorPanel.activeSelf || gamePanel.activeSelf || pausePanel.activeSelf)
        {
            cameraControl.enabled = false;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            cameraControl.enabled = true;
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }
}
