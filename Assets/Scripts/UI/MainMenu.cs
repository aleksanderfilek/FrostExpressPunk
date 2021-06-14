using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject[] menuObjects;
    
    void Start()
    {
        menuObjects[0].SetActive(true);
        for (int i = 1; i < menuObjects.Length; i++)
        {
            menuObjects[i].SetActive(false);
        }
    }


    private void OnEnable()
    {
        menuObjects[0].SetActive(true);
        for (int i = 1; i < menuObjects.Length; i++)
        {
            menuObjects[i].SetActive(false);
        }
    }
    
    public void GameBtn_Click()
    {
        menuObjects[0].SetActive(false);
        menuObjects[1].SetActive(true);
    }

    public void QuitBtn_Click()
    {
        Application.Quit();
    }

    public void BackBtn_Click()
    {
        foreach (var menu in menuObjects)
        {
            menu.SetActive(false);
        }
        
        menuObjects[0].SetActive(true);
        Time.timeScale = 1.0f;

    }

    public void StartLevelBtn_Click(int index)
    {
        Debug.Log("Start Level " + index);
        MenuManager.Get.HideAll();
        GameManager.Get.StartGame(index);
    }
    
}
