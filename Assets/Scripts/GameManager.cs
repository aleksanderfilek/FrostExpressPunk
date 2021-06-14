using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Level_Info[] levels;
	private int currentLevel;	

	[SerializeField] private GameObject _camera;
	public static GameObject Camera { get => instance._camera; }
    
    private static GameManager instance;
    public static GameManager Get{ get => instance; }

    [SerializeField] public float coalAmount;

	public int blastCount = 0;

	public event EventHandler GameStarted;

	public GameMenu gameMenu;

	public bool isRunning = false;
	private float timer = 0.0f;

    private void Awake()
    {
        instance = this;
    }

	void Update()
	{
		if(isRunning)
		{
			Debug.Log("Game time - " + timer);
			timer += Time.deltaTime;
		}
	}

	public Level_Info GetCurrentLevel()
	{
		return levels[currentLevel];
	}
	

    public void StartGame(int index)
    {
		currentLevel = index;
        GetComponent<LevelGenerator>().GenerateLevel(levels[index]);
        GetComponent<TrainManager>().Init(levels[index].carNumber, levels[index].initialFuel);
		MenuManager.Get.ShowCanvas(3);
		GameStarted?.Invoke(this, EventArgs.Empty);
		isRunning = true;
    }

	public Vector4 GetWorldSize()
	{
		return levels[currentLevel].worldSize;
	}

    public void DecreaseCoal(int amount)
    {
        coalAmount -= amount;
        
        foreach (Car_Child x in GetComponent<TrainManager>().TrainOnTheMap.Children())
        {
            int rest = x.DecreaseCoal(amount);
            if (rest == 0) break;
        }
        Debug.Log(coalAmount);
    }

	public void Unload()
	{
		GetComponent<ScenographyManager>().Unload();
		GetComponent<LevelGenerator>().railsObject.GetComponent<MeshFilter>().mesh = null;
		GetComponent<RailManager>().Clear();
        GetComponent<TrainManager>().Unload();
		coalAmount = 0;
	}
    public float GetMaxCoal()
    {
        int tmp = 0;
        foreach(Car_Child x in GetComponent<TrainManager>().TrainOnTheMap.Children())
        {
            tmp += x.maxCoalAmount;
        }
        return tmp;   
    }

	public float GetCoalPercentage()
	{
		return coalAmount / GetMaxCoal();
	}

    public void Lose(string defeatReason)
    {
        GetComponent<TrainManager>().CancelInvoke();
        Debug.Log("You Lost! Reason: " + defeatReason);
		gameMenu.ShowLoseScreen(defeatReason);
        Time.timeScale = 0.0f;
		isRunning = false;
    }

	public void AddBlast(int amount)
	{
		blastCount += amount;
		if(blastCount == 0)
		{
			gameMenu.ShowWinScreen();
        	Time.timeScale = 0.0f;
			isRunning = false;
		}
	}

    public void NextClick()
    {
        Time.timeScale = 1.0f;
        Unload();
		if(levels.Length < currentLevel)
		{	
        	StartGame(currentLevel + 1);
		}
		else
		{
			MenuManager.Get.ShowCanvas(2);
		}
    }
    
    public void AgainClick()
    {
        Time.timeScale = 1.0f;
        Unload();
        StartGame(currentLevel);
    }
}
