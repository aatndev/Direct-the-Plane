using System.Collections.Generic;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    //bool
    public bool gameOver;
    public bool isMenu;
    public bool rbReset;

    //Game Objects - Live and Prefabs
    public GameObject realPathPrefab;
    public GameObject newPathPrefab;
    private GameObject plane;
    private float tiltDirection = 15;
    private GameObject realPath;
    private GameObject newPath;

    private Rigidbody planeRb;

    //Enemy GameObjects
    public GameObject[] obstaclePrefab;
    private GameObject[] obstacle;

    int i; //index

    //invoking vars
    private float startDelay;
    private float repeatDelay;
    private bool isSlowinvDone = false;

    //Reset Positions, bounds and teleportation
    private Vector3 planeResetPos;
    private float frontBound = 100;
    private float backBound = -10;

    //scores
    private int score;
    private int scoreLimit = 50;
    private int addScore = 10;
    private float addSpeed = 1;
    private int highScore;

    //texts
    public GameObject welcomePage;
    public TextMeshProUGUI titleText;

    public TextMeshProUGUI scoreText;
    private RectTransform rectScore;

    public GameObject gameOverPage;
    public TextMeshProUGUI highScoreText;
    public TextMeshProUGUI gameOverText;

    //Button
    public Button menuButton;

    //Vector
    private Vector3 PRPVector;

    //scripts
    private Powerup powerup;
    private Explosion explosion;
    private BgMusic bgMusic;

    //Particles and sound effects
    public AudioSource planeAudioSource;
    public GameObject explosionPart;
    [SerializeField] private AudioClip explosionAudio;

    //boost icons
    public GameObject boostDelayIcon;
    public GameObject boostReadyIcon;


    void Start()
    {
        rbReset = false;
        planeResetPos = new Vector3(0, 0, 0);

        plane = GameObject.Find("Plane");
        planeRb = plane.GetComponent<Rigidbody>();

        rectScore = scoreText.rectTransform;
        obstacle = new GameObject[obstaclePrefab.Length];

        powerup = GetComponent<Powerup>();
        explosion = explosionPart.GetComponent<Explosion>();

        bgMusic = GameObject.Find("Main Camera").GetComponent<BgMusic>();

        Menu();

    }

    void Update()
    {
        if (!gameOver && !isMenu)
        {
            PRPVector = realPath.transform.position - plane.transform.position;
            Teleportation();
            realPathUnfollow();
            TimeControlSpawnObs();
        }

        if(gameOver || isMenu)
        {
            boostReadyIcon.SetActive(false);
            boostDelayIcon.SetActive(false);
        }
    }

    public void Menu()
    {
        Cursor.visible = true;

        planeRb.constraints = RigidbodyConstraints.FreezeAll;
        planeRb.constraints = RigidbodyConstraints.None;   

        Reset();
        
        //destroy existing paths
        Destroy(realPath);
        Destroy(newPath);

        isMenu = true;
        gameOver = false;

        welcomePage.SetActive(true);
        gameOverPage.SetActive(false);
        scoreText.gameObject.SetActive(false);

        bgMusic.MenuAudio();
    }

    public void ExitButton()
    {
        Application.Quit();
    }

    private void Reset()
    {
        planeRb.constraints = RigidbodyConstraints.FreezeAll;
        planeRb.constraints = RigidbodyConstraints.None;
        
        rbReset = true;
        plane.transform.position = planeResetPos;
        plane.transform.rotation = Quaternion.Euler(0, 0, 0);

        score = 0;
        scoreText.text = "Score: " + score;
        isSlowinvDone = false;
        scoreLimit = 50;
    }

    public void StartGame()
    {
        //audio
        bgMusic.PlayAudio();

        Reset();
        rbReset = false;

        Cursor.visible = false;
        
        //destroy existing paths
        Destroy(realPath);
        Destroy(newPath);

        //set bools and uis
        gameOver = false;
        isMenu = false;

        welcomePage.SetActive(false);
        gameOverPage.SetActive(false);
        
        scoreText.gameObject.SetActive(true);
        rectScore.anchorMin = new Vector2(0, 1);
        rectScore.anchorMax = new Vector2(0, 1);
        rectScore.anchoredPosition = new Vector2(203, -183);

        //boosticons
        boostReadyIcon.SetActive(true);
        boostDelayIcon.SetActive(false);

        //forming stuffs
        realPath = Instantiate(realPathPrefab);
        FormNewPath();
        SpawnObstacle();

    }

    //GAME OVER
    public void GameOver()
    {
        Cursor.visible = true;

        //uis and bools
        gameOver = true;
        isMenu = false;

        //explosion animation and vfx
        explosion.Explode();
        planeAudioSource.PlayOneShot(explosionAudio);

        welcomePage.SetActive(false);
        gameOverPage.SetActive(true);

        rectScore.anchorMin = new Vector2(0.5f, 0);
        rectScore.anchorMax = new Vector2(0.5f, 0);
        rectScore.anchoredPosition = new Vector2(-42, 480);

        //cancel spawn and destroy objs
        CancelInvoke("SpawnEachObs");

        for (int j = 0; j < obstaclePrefab.Length; j++)
        {
            if (obstacle[j] != null)
            {
                Destroy(obstacle[j].gameObject);
            }
        }

        //score management
        if (score > highScore)
        {
            highScore = score;
            highScoreText.text = "New Highscore: " + highScore;
        }

        menuButton.onClick.AddListener(Menu);

        bgMusic.StopAudio();
    }

    //ScoreControl
    public float ScoreControl(float speed)
    {
        score += addScore;
        scoreText.text = "Score: " + score;

        if (score == scoreLimit)
        {
            speed += addSpeed;
            scoreLimit += 50;
        }
        Debug.Log("Speed: " + speed);
        return speed;
    }

    //realPathUnfollow
    private void realPathUnfollow()
    {
        if (!gameOver && Vector3.Dot(plane.transform.forward, PRPVector) < -2)
        {
            Debug.Log("Wrong way!");
            GameOver();
        }
    }
    
    //TELEPORTATION
    private void Teleportation()
    {
        if (plane.transform.position.z > frontBound)
        {
            //Setting Distance
            Vector3 PNPVector = newPath.transform.position - plane.transform.position;

            Vector3[] PEVector = new Vector3[obstaclePrefab.Length];

            for (int j = 0; j < obstaclePrefab.Length; j++)
            {
                if(obstacle[j] != null)
                {
                    PEVector[j] = obstacle[j].transform.position - plane.transform.position;
                }
            }

            //Changing positions
            plane.transform.position = planeResetPos;

            realPath.transform.position = plane.transform.position + PRPVector;
            newPath.transform.position = plane.transform.position + PNPVector;

            for (int j = 0; j < obstaclePrefab.Length; j++)
            {
                if(obstacle[j] != null)
                {
                    obstacle[j].transform.position = plane.transform.position + PEVector[j];
                }
            }
        }

        if (plane.transform.position.z < backBound)
        {
            //Destroy(plane.gameObject);
            GameOver();
        }
    }

    //SPAWN MANAGER
    //path
    public void ChangingPosition()
    {
        realPath.transform.position = newPath.transform.position;
        realPath.transform.rotation = newPath.transform.rotation;

        Destroy(newPath);
        FormNewPath();
    }
    private void FormNewPath()
    {
        Vector3 newPathPos = realPath.transform.position + realPath.transform.forward * 10f;
        Quaternion newPathRot = realPath.transform.rotation * NewPathRotChange();
        newPath = Instantiate(newPathPrefab, newPathPos, newPathRot);
    }
    Quaternion NewPathRotChange()
    {
        return Quaternion.Euler(Random.Range(-tiltDirection, tiltDirection), Random.Range(-tiltDirection, tiltDirection), 0);
    }
    //enemy
    private void SpawnObstacle()
    {
        startDelay = 2;
        repeatDelay = Random.Range(1, 4);
        InvokeRepeating("SpawnEachObs", startDelay, repeatDelay);
    }

    private void TimeControlSpawnObs()
    {
        if (powerup.timeControlOn && !isSlowinvDone)
        {
            CancelInvoke("SpawnEachObs");
            startDelay = .3f;
            repeatDelay = Random.Range(.9f, 1.8f);
            InvokeRepeating("SpawnEachObs", startDelay, repeatDelay);
            isSlowinvDone = true;
        }
        else if (!powerup.timeControlOn && isSlowinvDone)
        {
            CancelInvoke("SpawnEachObs");
            SpawnObstacle();
            isSlowinvDone = false;
        }
    }
    private void SpawnEachObs()
    {
        int zAxis = powerup.timeControlOn ? 18 : 60;
        int yAxis = 0;
        int xAxis = 10;         

        Vector3 bombSpawnPos = plane.transform.position 
                                + plane.transform.forward * zAxis
                                + plane.transform.right * Random.Range(xAxis, -xAxis) 
                                + plane.transform.up * yAxis;

        i = Random.Range(0, obstaclePrefab.Length);
        obstacle[i] = Instantiate(obstaclePrefab[i], bombSpawnPos, obstaclePrefab[i].transform.rotation);
    }

    
}
