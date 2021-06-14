using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoalFill : MonoBehaviour
{
    Car_Child car;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnCollisionEnter(Collision collision)
    {
        car = collision.gameObject.GetComponent<Car_Child>();
        if (car != null)
        {

            if (car.car_before.GetComponent<Car_Train>() != null)
            {
                int initialcoal = 20;

                foreach (Car a in car.tm.TrainOnTheMap.Children())
                {
                    Car_Child x = a.gameObject.GetComponent<Car_Child>();

                    x.FillWithCoal(Mathf.Clamp(initialcoal, 0, x.maxCoalAmount));
                    initialcoal -= Mathf.Clamp(initialcoal, 0, x.maxCoalAmount);
                    if (initialcoal == 0) break;

                }
            }
            else Debug.Log("car is not first");

        }
        else Debug.Log("car is null");

    }
}
