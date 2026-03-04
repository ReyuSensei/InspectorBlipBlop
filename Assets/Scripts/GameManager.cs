using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance; //STATIC: CUALQUIER OBJETO PUEDE ACCEDER A ELLA SIN UNA REFERENCIA DIRECTA // GAMEMANAGER: PARA ASEGURARME DE QUE SOLO HAY UNA INSTANCIA EN EJECUCION, SI YA EXISTE E INTENTO CREAR OTRA SE DESTRUYE
    public GameplayUI gameplayUI;
    public PlayerController playerController;
    public int nextLevelToLoad;

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

    public void UpdateHealthBar(float value)
    {
        gameplayUI.UpdateHealthBar(value);
    }

}
