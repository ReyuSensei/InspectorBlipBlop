using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class OptionsUI : MonoBehaviour
{
    [SerializeField]
    private TMP_Dropdown colorBlindnessMenu;

    public void SetMasterVolume(float value)
    {
        GameManager.instance.volumeMasterPref = value;
        GameManager.instance.UpdateMasterVolume();
    }

    public void SetBackgroundMusicVolume(float value)
    {
        //GUARDO EL VALOR DEL SLIDER EN EL GAMEMANAGER
        GameManager.instance.volumeMusicPref = value;
        GameManager.instance.UpdateBackgroundMusic();
    }

    public void SetSfxVolume(float value)
    {
        GameManager.instance.volumeSfxPref = value;
        GameManager.instance.UpdateSfxVolume();
    }

    public void SetVoicesVolume(float value)
    {
        GameManager.instance.volumeVoicesPref = value;
        GameManager.instance.UpdateVoicesVolume();
    }

    public void SetBright(float value)
    {
        GameManager.instance.brightPref = value;
        GameManager.instance.UpdateBright();
    }

    public void SetTextSize(float value)
    {
        GameManager.instance.textSizePref = value;
        GameManager.instance.UpdateTextSize();
    }

    public void SetColorBlindnessFilter(int value)
    {
        //GUARDAMOS LA SELECCIÓN EN EL GAMEMANAGER
        GameManager.instance.currentFilterType = value;
        //HACEMOS EL CAMBIO DE FILTRO
        GameManager.instance.UpdateColorBlindnessFilter();
    }
    private void OnEnable()
    {
        //HACEMOS UNA LISTA CON LOS EFECTOS
        string[] colorBlindnessEnumNames = Enum.GetNames(typeof(GameManager.E_FilterType));
        List<string> colorBlindnessNames = new List<string>(colorBlindnessEnumNames);
        //BORRAMOS LAS OPCIONES DEL DROPDOWN
        colorBlindnessMenu.ClearOptions();
        //RELLENAMOS EL MENÚ CON LA LISTA
        colorBlindnessMenu.AddOptions(colorBlindnessNames);
        //MARCAMOS COMO VALOR ACTUAL LO QUE DIGA EL GAMEMANAGER
        colorBlindnessMenu.value = GameManager.instance.currentFilterType;
    }
}