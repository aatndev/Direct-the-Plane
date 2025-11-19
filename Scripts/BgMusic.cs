using UnityEngine;
using UnityEngine.Rendering;

public class BgMusic : MonoBehaviour
{
    //Audios
    private AudioSource audioSource;
    [SerializeField] private AudioClip planeMenuAudio;
    [SerializeField] private AudioClip planePlayAudio;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void MenuAudio()
    {
        audioSource.Stop();
        audioSource.clip = planeMenuAudio;
        audioSource.Play();
    }

    public void PlayAudio()
    {
        audioSource.Stop();
        audioSource.clip = planePlayAudio;
        audioSource.Play();
    }

    public void StopAudio()
    {
        audioSource.Stop();
        audioSource.clip = null;
        audioSource.Play();
    }
}
