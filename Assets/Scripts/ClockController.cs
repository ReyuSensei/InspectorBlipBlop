using UnityEngine;
using UnityEngine.InputSystem;

public class ClockController : MonoBehaviour
{
    [SerializeField] private GameObject bttE;
    [SerializeField] private GameObject minutes;
    [SerializeField] private GameObject hour;
    [SerializeField] private GameObject panel;
    [SerializeField] private float velocity;
    [SerializeField] private PlayerController player;
    private PlayerInput playerInput;
    private InputAction moveAction;
    private Vector2 moveInput;
    void Start()
    {
        playerInput = player.GetComponent<PlayerInput>();
        moveAction = playerInput.actions["Move"];
    }


    void Update()
    {
        HandleMinutes();
        HandleHours();
        HandlePuzzle();
        moveInput = moveAction.ReadValue<Vector2>();
    }

    public void bttEON()
    { 
        bttE.SetActive(true);
    }

    public void bttEOFF()
    {
        bttE.SetActive(false);
    }

    void HandleMinutes()
    {
        if (moveInput.y != 0 && panel.activeInHierarchy)
        {
            float rotationAmount = moveInput.y * velocity * Time.deltaTime;
            minutes.transform.Rotate(0, 0, -rotationAmount);
        }
    }

    void HandleHours()
    {
        if (moveInput.x != 0 && panel.activeInHierarchy)
        {
            float rotationAmount = moveInput.x * velocity * Time.deltaTime;
            hour.transform.Rotate(0, 0, -rotationAmount);
        }
    }

    void HandlePuzzle()
    {

    }
}
