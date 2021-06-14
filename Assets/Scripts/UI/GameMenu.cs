using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class GameMenu : MonoBehaviour
{
    private GameManager _gameManager;

    [SerializeField] private GameObject hud;
    [SerializeField] private GameObject pause;
    [SerializeField] private GameObject win;
    [SerializeField] private GameObject lose;
    [SerializeField] private Image carImage;
    [SerializeField] private Image locomotiveImage;
    [SerializeField] private Text coalAmount;
    [SerializeField] private Text FailText;
    [SerializeField] AudioSource audioData;

    private Car_Train train;
    private void OnEnable()
    {
        _gameManager = GameManager.Get;
        _gameManager.gameMenu = GetComponent<GameMenu>();
        
        hud.SetActive(true);
        pause.SetActive(false);
        win.SetActive(false);
        lose.SetActive(false);
        
        train = _gameManager.gameObject.GetComponent<TrainManager>().TrainOnTheMap;
    }

    private void Update()
    {
        if (!_gameManager.isRunning)
            return;
        
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Time.timeScale = 0.0f;
            pause.SetActive(true);
            _gameManager.isRunning = false;
        }
        
        //float firstChildCoalPerc = (float)firstCar.coalAmount / (float)firstCar.maxCoalAmount;
        //float carCoal = (_gameManager.coalAmount - (float)firstCar.coalAmount) / (_gameManager.GetMaxCoal() - (float)firstCar.maxCoalAmount);
        
        locomotiveImage.fillAmount = train.GetFuelPercent();
        carImage.fillAmount = _gameManager.GetCoalPercentage();
        coalAmount.text = _gameManager.coalAmount.ToString();
    }
    private void Start()
    {
        audioData.volume = 0;
    }
    public void ResumeBtn_Click()
    {
        Time.timeScale = 1.0f;
        pause.SetActive(false);
        _gameManager.isRunning = true;
    }

    public void QuitBtn_Click()
    {
        //Clear game level
        Time.timeScale = 1.0f;
        _gameManager.Unload();
        MenuManager.Get.ShowCanvas(2);
        _gameManager.isRunning = false;
    }
    

    public void ShowWinScreen()
    {
        hud.SetActive(false);
        pause.SetActive(false);
        win.SetActive(true);
        lose.SetActive(false);
    }

    public void ShowLoseScreen(string reason)
    {
        hud.SetActive(false);
        pause.SetActive(false);
        win.SetActive(false);
        lose.SetActive(true);
        FailText.text = "Failed!\n" + reason;
    }
}
