using UnityEngine;
using UnityEngine.Audio;
using TMPro;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager instance; //STATIC: CUALQUIER OBJETO PUEDE ACCEDER A ELLA SIN UNA REFERENCIA DIRECTA // GAMEMANAGER: PARA ASEGURARME DE QUE SOLO HAY UNA INSTANCIA EN EJECUCION, SI YA EXISTE E INTENTO CREAR OTRA SE DESTRUYE
    public GameplayUI gameplayUI;
    public PlayerController playerController;
    public int nextLevelToLoad;
    public bool autoAimActive = false;

    [Header("PREFERENCES")]
    public AudioMixer mixer;
    public float volumeMasterPref;
    public float volumeMusicPref;
    public float volumeSfxPref;
    public float volumeVoicesPref;

    public float brightPref;
    public VolumeProfile profile;

    public float textSizePref;

    public VolumeComponent lastFilter;
    public int currentFilterType;
    public enum E_FilterType { Normal, Achromatopsia, Deuteranomaly, Protanopia, Tritanopia }

    private void Awake()    //SE EJECUTA ANTES DEL START, ES DECIR AWAKE -> START -> UPDATE
    {
        //COMPRUEBO QUE NO HAYA NINGUNA INSTANCIA
        if (instance == null)
        {
            //SI NO HAY NINGUNA INSTANCIA, DIGO QUE LA INSTANCIA SOY YO MISMO
            instance = this;
            //HAGO QUE ESTE OBJETO NO SE DESTRUYA AUNQUE CAMBIE DE ESCENA
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            //SI YA HAY UNA INSTANCIA, ME SUICIDO
            Destroy(gameObject); //DESTRUYO EL OBJECTO QUE TIENE EL SCRIPT
        }
    }

    /*public void UpdateHealthBar(float value)
    {
        gameplayUI.UpdateHealthBar(value);
    }*/

    public void UpdateMasterVolume()
    {
        mixer.SetFloat("MasterVolume", volumeMasterPref);
    }
    public void UpdateBackgroundMusic()
    {
        mixer.SetFloat("BGMVolume", volumeMusicPref);
    }
    public void UpdateSfxVolume()
    {
        mixer.SetFloat("SFXVolume", volumeSfxPref);
    }
    public void UpdateVoicesVolume()
    {
        mixer.SetFloat("VoicesVolume", volumeVoicesPref);
    }
    public void UpdateBright()
    {
        //BUSCO TODOS LOS VOLUMENES
        Volume[] volumes = FindObjectsByType<Volume>(FindObjectsSortMode.None);
        foreach (Volume vol in volumes)
        {
            //ME ASEGURO DE QUE LOS VOLUMENES SEAN GLOBALES Y TENGAN EFECTOS
            if (vol.isGlobal && vol.profile != null)
            {
                //ME QUEDO CON EL EFECTO DE COLOR ADJUSTMENTS SI LO TIENE
                ColorAdjustments adjustments;
                vol.profile.TryGet(out adjustments);
                if (adjustments != null)
                {
                    adjustments.postExposure.value = brightPref;
                }
            }
        }
    }
    public void UpdateTextSize()
    {
        //BUSCAMOS TODOS LOS TEXTOS DE TMPRO AUNQUE ESTEN OCULTOS
        TextMeshProUGUI[] texts = FindObjectsByType<TextMeshProUGUI>(FindObjectsInactive.Include, FindObjectsSortMode.None);

        //ACTUALIZAMOS EL TAMAÑO DE TODOS LOS TEXTOS
        foreach (TextMeshProUGUI text in texts)
        {
            if (textSizePref == 0)
            {
                text.fontSize = text.fontSizeMin;
            }
            else
            {
                text.fontSize = text.fontSizeMax;
            }
        }
    }
    public void StopGameTime()
    {
        Time.timeScale = 0;
    }
    public void StartGameTime()
    {
        Time.timeScale = 1;
    }

    public void UpdateColorBlindnessFilter()
    {
        StartCoroutine(ApplyFilter());
    }

    IEnumerator ApplyFilter()   //ESTO ES UNA CORRUTINA AYUDA POR FAVOR
    {
        //BUSCAMOS TODOS LOS OBJETOS DE TIPO VOLUME
        Volume[] volumes = FindObjectsByType<Volume>(FindObjectsInactive.Include, FindObjectsSortMode.None);
        //CARGAMOS TODOS LOS PROFILES DE FILTRO DE DALTONISMO
        ResourceRequest loadRequest = Resources.LoadAsync<VolumeProfile>($"Colorblind/{(E_FilterType)currentFilterType}");
        //NO HACEMOS NADA HASTA QUE NO SE HAYAN CARGADO TODOS LOS PROFILES
        while (!loadRequest.isDone)
        {
            yield return null;
        }
        //
        var filter = loadRequest.asset as VolumeProfile;
        if (filter == null)
        {
            Debug.LogError("An error has ocurred! Please, report");
            yield break;
        }
        //SI YA HABÍAMOS USADO UN FILTRO
        if (lastFilter != null)
        {
            //QUITAMOS EL FILTRO
            foreach (Volume volume in volumes)
            {
                volume.profile.components.Remove(lastFilter);
                //AÑADIMOS EL FILTRO SELECCIONADO
                foreach (var component in filter.components)
                {
                    volume.profile.components.Add(component);
                }
            }
        }
        lastFilter = filter.components[0];
    }

}