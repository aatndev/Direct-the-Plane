using System;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    [SerializeField] private Material eaglePlaneMat;
    
    public GameObject settingsPage;

    public GameObject firstPage;
    public Button changeAppButton;
    public Material[] bg;
    int i = 0;    
    public Button changeResButton;
    public Button keybindsButton;
    public Button menuButton;

    public GameObject resPage;
    public TMP_InputField widthResIn;
    public TMP_InputField heightResIn;
    public Button applyButton;
    public Toggle fullscreenTog;
    private bool isFullscreen;

    public GameObject keybindsPage;
    public Toggle invertXCheck;
    public Toggle invertYCheck;

    //scripts
    private GameManager gameManager;

    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    void Update()
    {
        if (gameManager.gameOver)
        {
            eaglePlaneMat.SetColor("_BaseColor", Color.black); 
        }
        else
        {
            eaglePlaneMat.SetColor("_BaseColor", Color.white); 
        }
    }

    public void SettingsPage()
    {
        gameManager.welcomePage.SetActive(false);
        settingsPage.SetActive(true);

        FirstPage();
    }

    //first page
    void FirstPage()
    {
        resPage.SetActive(false);
        firstPage.SetActive(true);
        keybindsPage.SetActive(false);

        changeAppButton.onClick.AddListener(ChangeApp);
        changeResButton.onClick.AddListener(ResPage);
        keybindsButton.onClick.AddListener(Keybinds);
        menuButton.onClick.AddListener(SettingsToMenu);
    }

    private void ChangeApp()
    {
        RenderSettings.skybox = bg[i];
        if(i < (bg.Length-1))
        {
            i++;
        }
        else
        {
            i = 0;
        }
    }

    private void SettingsToMenu()
    {
        settingsPage.SetActive(false);
        gameManager.Menu();
    }

    //resolution page
    private void ResPage()
    {
        firstPage.SetActive(false);
        resPage.SetActive(true);
        keybindsPage.SetActive(false);

        isFullscreen = fullscreenTog.isOn;
        applyButton.onClick.AddListener(ChangeRes);
    }

    private void ChangeRes()
    {
        int width = int.Parse(widthResIn.text);
        int height = int.Parse(heightResIn.text);

        Screen.SetResolution(width, height, isFullscreen);
    }

    //keybinds page
    private void Keybinds()
    {
        firstPage.SetActive(false);
        resPage.SetActive(false);
        keybindsPage.SetActive(true);
    }

}
