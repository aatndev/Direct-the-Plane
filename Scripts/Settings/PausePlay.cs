using UnityEngine;
using UnityEngine.UI;

public class PausePlay : MonoBehaviour
{
    public Button playButton;
    public Button pauseButton;    
    private bool isPause;
    private GameManager gameManager;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        isPause = false;
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if(gameManager.isMenu || gameManager.gameOver)
        {
            isPause = false;
            pauseButton.gameObject.SetActive(false);
            playButton.gameObject.SetActive(false);
        }

        if(!gameManager.isMenu && !gameManager.gameOver)
        {
            if(Input.GetKeyDown(KeyCode.Escape))
            {
                PauseOrPlay();
            }

            if(!isPause)
            {
                pauseButton.gameObject.SetActive(true);
                playButton.gameObject.SetActive(false);
            }
            else
            {
                pauseButton.gameObject.SetActive(false);
                playButton.gameObject.SetActive(true);
            }
        }
    }

    public void PauseOrPlay()
    {
        if(!isPause)
        {
            Time.timeScale = 0;
            isPause = true;
        }
        else if (isPause)
        {
            Time.timeScale = 1;
            isPause = false;
        }
    }
}
