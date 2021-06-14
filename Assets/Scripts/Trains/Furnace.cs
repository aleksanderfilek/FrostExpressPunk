using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Furnace : MonoBehaviour
{
    [SerializeField] public float startTime = 100.0f;
    public float timer;
    bool isRunning=false;
    [SerializeField] public int CoalNeeded;
    int maxCoal;
    // Start is called before the first frame update

    [SerializeField] private Sprite workingSprite;
    [SerializeField] private Sprite doneSprite;

    [SerializeField] private Image fillImage;
    [SerializeField] private Text _text;
    
    void Start()
    {
        maxCoal = CoalNeeded;
        timer = startTime;

        //Vector3 rotation = transform.rotation.eulerAngles + new Vector3(0.0f, 45.0f, 0.0f);
        //fillImage.transform.parent.rotation = Quaternion.Euler(rotation);
		fillImage.transform.parent.rotation = Quaternion.LookRotation(GameManager.Camera.transform.forward, Vector3.up);
        fillImage.sprite = workingSprite;
		CoalNeeded = GameManager.Get.GetCurrentLevel().maxBlastCoal;
        _text.text = CoalNeeded.ToString();

    }

    void Update()
    {
        if (CoalNeeded == 0)
            return;
        
        timer -= Time.deltaTime;
        float perc = timer / startTime;
        fillImage.fillAmount = perc;
        if (timer <= 0)
        {
            GameManager.Get.Lose("The City got cold...");
        }

        
    }

    public void ResetTime()
    {
        timer = startTime;
    }

    private void OnCollisionEnter(Collision collision)
    {
        var gm = GameManager.Get;
        if (!isRunning) Debug.Log("not running");
        Car_Child car = collision.gameObject.GetComponent<Car_Child>();
        if (car != null)
        {
            if (car.car_before.GetComponent<Car_Train>() != null)
            {
                int x = (int)gm.coalAmount;

                gm.DecreaseCoal(Mathf.Clamp(CoalNeeded, 0, (int)(gm.coalAmount)));
                CoalNeeded -= Mathf.Clamp(CoalNeeded, 0, x);
                _text.text = CoalNeeded.ToString();

                if (CoalNeeded == 0)
                {
                    Debug.Log("Furnace Full");
                    gm.AddBlast(-1);
                    fillImage.fillAmount = 1.0f;
                    fillImage.sprite = doneSprite;
                    _text.text = CoalNeeded.ToString();
                }
            }


        }
    }

}
