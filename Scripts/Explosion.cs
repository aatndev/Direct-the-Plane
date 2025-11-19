using UnityEngine;

public class Explosion : MonoBehaviour
{
    public ParticleSystem Light;
    public ParticleSystem Smoke1;
    public ParticleSystem Smoke2;
    public ParticleSystem Smoke3;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }
    
    public void Explode()
    {
        Light.Play();
        Smoke1.Play();
        Smoke2.Play();
        Smoke3.Play();
    }

    // Update is called once per frame
    void Update()
    {

    }
    
    
}
