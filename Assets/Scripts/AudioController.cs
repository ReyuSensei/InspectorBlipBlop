using UnityEngine;

public class AudioController : MonoBehaviour
{
    public AudioSource audioSource;
    public void PlayAudio(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
    }
}
