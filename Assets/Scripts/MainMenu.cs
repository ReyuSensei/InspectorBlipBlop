using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void Play()
    {
        //LE DIGO AL SINGLETON LA SIGUIENTE ESCENA DESPUES DE ESTA
        GameManager.instance.nextLevelToLoad = 2;
        //CARGO LA PANTALLA DE LOADING
        SceneManager.LoadScene(1);
    }

    //MENSAJES PARA EL COMPILADOR EMPIEZAN CON #, SIN CORCHETES Y SE PONEN A LA IZQ DEL TODO
    public void Quit()
    {
#if UNITY_EDITOR
    EditorApplication.ExitPlaymode();
#else
        Application.Quit();
#endif
    }

    public void Main()
    {
        //LE DIGO AL SINGLETON LA SIGUIENTE ESCENA DESPUES DE ESTA
        GameManager.instance.nextLevelToLoad = 2;
        //CARGO LA PANTALLA DE LOADING
        SceneManager.LoadScene(0);
    }
}