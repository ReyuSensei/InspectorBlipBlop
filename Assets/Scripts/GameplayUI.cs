using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;

public class GameplayUI : MonoBehaviour
{
    [SerializeField]
    private Image healthBar;
    [SerializeField]
    private GameObject panelPause;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
    void Start()
    {
        GameManager.instance.gameplayUI = this; //  LE DIGO AL GAMEMANAGER QUE EL GESTOR DE GAMEPLAYUI SOY YO, CREANDO UNA REFERENCIA INDIRECTA
        GameManager.instance.UpdateMasterVolume();
        GameManager.instance.UpdateBackgroundMusic();
        GameManager.instance.UpdateSfxVolume();
        GameManager.instance.UpdateVoicesVolume();
        GameManager.instance.UpdateBright();
        GameManager.instance.UpdateTextSize();
        GameManager.instance.UpdateColorBlindnessFilter();
    }

    public void UpdateHealthBar(float barFillAmount)
    {
        healthBar.fillAmount = barFillAmount;
    }

    public void ShowPause()
    {
        GameManager.instance.StopGameTime();
        panelPause.SetActive(true);
    }

    public void HidePause()
    {
        GameManager.instance.StartGameTime();
        panelPause.SetActive(false);

    }

    public void TogglePause()
    {
        if(panelPause.activeInHierarchy == false)
        {
            ShowPause();
        } else
        {
            HidePause();
        }
    }

    IEnumerator ShowMouse()
    {
        yield return new WaitForSeconds(0.1f);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
}
