using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Shield : MonoBehaviour
{
    private Powerup powerup;
    private int powerupID = 1;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        powerup = GameObject.Find("GameManager").GetComponent<Powerup>();
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            //Powerup enable
            powerup.shieldOn = true;

            powerup.powerupOn(powerupID);

            Destroy(gameObject);
        }
    }
}
