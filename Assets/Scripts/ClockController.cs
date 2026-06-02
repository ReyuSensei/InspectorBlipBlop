using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ClockController : MonoBehaviour
{
    [SerializeField] private GameObject minutes;
    [SerializeField] private GameObject hour;
    [SerializeField] private GameObject panel;
    [SerializeField] private float velocity;
    [SerializeField] private PlayerController player;
    [SerializeField] private float minHour;
    [SerializeField] private float maxHour;
    [SerializeField] private float minMinute;
    [SerializeField] private float maxMinute;
    [SerializeField] private float actualHourZ;
    [SerializeField] private float actualMinuteZ;
    [SerializeField] private Animator anim;
    [SerializeField] private InteractableObject interactableObject;
    public bool puzzleSolved = false;
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


    void HandleMinutes()
    {
        if (moveInput.y != 0 && panel.activeInHierarchy && !puzzleSolved)
        {
            float rotationAmount = moveInput.y * velocity * Time.deltaTime;
            minutes.transform.Rotate(0, 0, -rotationAmount);
        }
    }

    void HandleHours()
    {
        if (moveInput.x != 0 && panel.activeInHierarchy && !puzzleSolved)
        {
            float rotationAmount = moveInput.x * velocity * Time.deltaTime;
            hour.transform.Rotate(0, 0, -rotationAmount);
        }
    }

    void HandlePuzzle()
    {
        actualHourZ = hour.transform.eulerAngles.z;
        actualMinuteZ = minutes.transform.eulerAngles.z;

        actualHourZ = (actualHourZ + 360) % 360;
        actualMinuteZ = (actualMinuteZ + 360) % 360;

        if (actualHourZ > minHour && actualHourZ < maxHour)
        {
            if (actualMinuteZ > minMinute && actualMinuteZ < maxMinute && !puzzleSolved)
            {
                puzzleSolved = true;
                StartCoroutine(openClockWait());
            }
        }
    }
    IEnumerator openClockWait()
    {
        yield return new WaitForSeconds(1.5f);
        anim.SetTrigger("_open");
        panel.SetActive(false);
        interactableObject.isActive = false;
        interactableObject.OFF();
    }
}
