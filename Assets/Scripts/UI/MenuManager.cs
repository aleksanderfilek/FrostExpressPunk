using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private GameObject[] canvasObjects;
    [SerializeField] private float[] canvasTimes;
        
    [SerializeField] private GameObject[] worlds;
    private static MenuManager instance;

    public static MenuManager Get{ get => instance; }

    public bool skip = true;
    
    private int canvasIndex = 0;
    private float timer = 0.0f;

    private void Awake()
    {
        instance = this;
    }
    
    private void Start()
    {
        canvasObjects[0].SetActive(true);
        for (int i = 1; i < canvasObjects.Length; i++)
        {
            canvasObjects[i].SetActive(false);
        }

        if (skip)
        {
            canvasObjects[0].SetActive(false);
            canvasObjects[2].SetActive(true);
            canvasIndex = canvasTimes.Length;
        }
        worlds[0].SetActive(true);
        worlds[1].SetActive(false);
    }

    private void Update()
    {
        if (canvasIndex == canvasTimes.Length)
            return;
        
        timer += Time.deltaTime;
        if (canvasTimes[canvasIndex] <= timer || Input.anyKey)
        {
            timer = 0.0f;
            canvasObjects[canvasIndex].SetActive(false);
            canvasIndex++;
            canvasObjects[canvasIndex].SetActive(true);
        }
    }

    public void HideAll()
    {
        foreach (var canvas in canvasObjects)
        {
            canvas.SetActive(false);
        }
    }

    public void ShowCanvas(int index)
    {
        Time.timeScale = 1.0f;

        HideAll();
        canvasObjects[index].SetActive(true);

        if (index == 2)
        {
            worlds[0].SetActive(true);
            worlds[1].SetActive(false);
        }
        else if (index == 3)
        {
            worlds[0].SetActive(false);
            worlds[1].SetActive(true);
        }
    }
}
