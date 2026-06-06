using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class ControladorVideo : MonoBehaviour
{
    private VideoPlayer miVideoPlayer;

    [SerializeField] private string nombreDeLaSiguienteEscena;

    void Start()
    {
        // Buscamos el componente VideoPlayer en este objeto
        miVideoPlayer = GetComponent<VideoPlayer>();

        if (miVideoPlayer != null)
        {
            // Nos suscribimos al evento que avisa cuando el video termina
            miVideoPlayer.loopPointReached += AlTerminarVideo;
        }
    }

    // Esta función se ejecutará automáticamente cuando acabe el video
    void AlTerminarVideo(VideoPlayer vp)
    {
        // Cargamos la siguiente escena usando el nombre que pusimos en el Inspector
        SceneManager.LoadScene(nombreDeLaSiguienteEscena);
    }

    void OnDestroy()
    {
        // Buena práctica: desuscribirse del evento al destruir el objeto para evitar errores
        if (miVideoPlayer != null)
        {
            miVideoPlayer.loopPointReached -= AlTerminarVideo;
        }
    }

    public void SkipCinematic()
    {
        SceneManager.LoadScene(nombreDeLaSiguienteEscena);
    }
}