using System.Collections;
using UnityEngine;

public class TimeControl : MonoBehaviour
{
    private Powerup powerup;
    private int powerupID = 2;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        powerup = GameObject.Find("GameManager").GetComponent<Powerup>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("time control access");

            //Powerup enable
            powerup.timeControlOn = true;
            powerup.DoSloMo();

            powerup.powerupOn(powerupID);

            Destroy(gameObject);
        }
    }
}
