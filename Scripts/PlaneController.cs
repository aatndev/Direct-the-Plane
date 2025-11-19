using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Video;

public class planeController : MonoBehaviour
{
    //Basic Movements
    private float speed = 5;
    private float ySpeed = 30.0f;
    private float xSpeed = 30.0f;
    private float verIn;
    private float horIn;


    //RigidBody
    private Rigidbody planeRb;

    //Boosting

    [SerializeField] private ParticleSystem boostParticle;
    [SerializeField] private AudioClip passAudio;
    [SerializeField] private AudioClip boostAudio;
    private AudioSource audioSource;
    private bool isBoosted = false;
    private bool isBoosting = false;
    private float boostCoolDown = 8;
    private float boostForce = 5000;

    //Scripts
    private GameManager gameManager;
    private Powerup powerup;
    private SettingsManager settingsManager;


    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        planeRb = GetComponent<Rigidbody>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        powerup = GameObject.Find("GameManager").GetComponent<Powerup>();
        settingsManager = GameObject.Find("GameManager").GetComponent<SettingsManager>();
    }

    void Update()
    {
        horIn = Input.GetAxis("Horizontal");
        verIn = Input.GetAxis("Vertical");

        xSpeed = isBoosting == true ? 50 : 30;
        ySpeed = isBoosting == true ? 50 : 30;
        
        BasicMovements();

        //reset
        if (gameManager.gameOver)
        {
            speed = 5;
            isBoosted = false;
            isBoosting = false;
        }
    }

    void FixedUpdate()
    {
        if (!gameManager.isMenu && !gameManager.gameOver)
        {
            Boost();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("RealPath"))
        {
            gameManager.ChangingPosition();

            //Add score
            speed = gameManager.ScoreControl(speed);
            
            //vfx
            audioSource.PlayOneShot(passAudio);
        }
        
    }
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Destroy(collision.gameObject);

            if (!powerup.shieldOn)
            {
                gameManager.GameOver();
            }
        }
    }
    private void BasicMovements()
    {
        //movement
        if(!gameManager.gameOver && !gameManager.isMenu)
        {
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }

        //turning
        if(settingsManager.invertXCheck.isOn)
        {    
            transform.Rotate(Vector3.down, xSpeed * Time.deltaTime * horIn); //left right
            transform.Rotate(Vector3.forward, xSpeed * Time.deltaTime * horIn); //tilt
        }
        else
        {    
            transform.Rotate(Vector3.up, xSpeed * Time.deltaTime * horIn); //left right
            transform.Rotate(Vector3.back, xSpeed * Time.deltaTime * horIn); //tilt
        }
        
        //up down
        if(settingsManager.invertYCheck.isOn)
        {
            transform.Rotate(Vector3.right, ySpeed * Time.deltaTime * verIn); 
        }
        else
        {
            transform.Rotate(Vector3.left, ySpeed * Time.deltaTime * verIn); 
        }
    }

    private void Boost()
    {
        float normalSpeed = planeRb.linearVelocity.magnitude;

        if ((Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.Keypad0)) && !isBoosted)
        {
            ImmediateBoost();
            StartCoroutine(BoostCooldown());
        }

        float boostSpeed = planeRb.linearVelocity.magnitude;
        
        if (boostSpeed <= normalSpeed + 0.5f)
        {
            isBoosting = false;
        }
    }
    IEnumerator BoostCooldown()
    {
        Debug.Log("boosting time started");
        yield return new WaitForSeconds(boostCoolDown);
        isBoosted = false;
        
        gameManager.boostReadyIcon.SetActive(true);
        gameManager.boostDelayIcon.SetActive(false);

        Debug.Log("Boost Cool Down over");
    }

    private void ImmediateBoost()
    {
        boostParticle.Play();
        audioSource.PlayOneShot(boostAudio);
        
        gameManager.boostReadyIcon.SetActive(false);
        gameManager.boostDelayIcon.SetActive(true);

        planeRb.AddForce(transform.forward.normalized * boostForce, ForceMode.Impulse);
        isBoosting = true;
        isBoosted = true;
        Debug.Log("Boosted");
    }
    
}

