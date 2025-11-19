using System.Collections;
using System.Threading;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    public GameObject immuneSphere;
    
    //Powerup bools
    public bool shieldOn = false;
    public bool timeControlOn = false;
    
    private float duration = 5;

    //slowmo
    public AudioSource planeAudioSource;
    [SerializeField] private AudioClip slowmoAudio;
    private float sloMoSpeed = 0.3f;
    

    //color changing
    private Light directionalLight;
    private Color violet;
    private Color lime;
    private Color white;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        directionalLight = GameObject.Find("Directional Light").GetComponent<Light>();

        violet = new Color(108 / 255f, 70f / 255f, 1);
        lime = new Color(160f / 255f, 1, 0);
        white = Color.white;
    }

    // Update is called once per frame
    void Update()
    {

        if (timeControlOn)
        {
            directionalLight.color = violet;
        }
        else if (shieldOn)
        {
            directionalLight.color = lime;
        }
        else
        {
            directionalLight.color = white;
        }

        immuneSphere.SetActive(shieldOn);
    }
    
    public void powerupOn(int id)
    {
        duration = id == 2 ? 1.5f : 5;
        StartCoroutine(Timer(id));
    }
    
    public IEnumerator Timer(int id)
    {
        yield return new WaitForSeconds(duration);
        
        //Powerup disable
        if(id == 1)
        {
            shieldOn = false;
        }
        else if(id == 2)
        {
            timeControlOn = false;
            StopSloMo();
        }
    }

    public void DoSloMo()
    {
        Time.timeScale = sloMoSpeed;
        Time.fixedDeltaTime = Time.timeScale * 0.02f;
        planeAudioSource.PlayOneShot(slowmoAudio);
    }
    
    void StopSloMo()
    {
        Time.timeScale = 1f;
        Time.fixedDeltaTime = 0.02f;
    }
}
