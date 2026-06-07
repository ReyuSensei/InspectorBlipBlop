using UnityEngine;
using UnityEngine.SceneManagement;

public class SkipCredits : MonoBehaviour
{
    [SerializeField] private string nombreDeLaSiguienteEscena;
    public void SkipCinematic()
    {
        SceneManager.LoadScene(nombreDeLaSiguienteEscena);
    }
}
