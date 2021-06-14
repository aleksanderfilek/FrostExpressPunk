using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car_Child : Car
{
    public int coalAmount=0;
    public bool isApproaching=true;
    bool doOnce = false;
    [SerializeField] public int maxCoalAmount=25;
    [SerializeField] GameObject coal;
    private void Start()
    {
        UpdateCoalMesh();
    }
    public int DecreaseCoal(int amount) 
    {
        if (coalAmount > 0)
        {
            coalAmount--;
            amount--;
            UpdateCoalMesh();
            if (amount > 0) return DecreaseCoal(amount);
            else return 0;
        }
        else return amount;
        
    }
    override public void Run()
    {
    }
    override public void DisconnectCars()
    {
        Car x = car_after;
        while (true)
        {
            x.gameObject.GetComponent<Car_Child>().OnDisconnected();

            x = x.car_after;
            if (x == null) break;
        }
        
        car_after.AssignCarBefore(null);
        car_after = null;
    }
    public override Car FindLastCar()
    {
        if (car_after != null)
        {
            return car_after.FindLastCar();
        }
        else return this;
    }
    public override void AssignCarBefore(Car_Train train_to_add)
    {
        if (train_to_add == null)
        {
            car_before.AssignCarAfter(null);
            car_before = null;
            return;
        }
        else
        {
            car_before = train_to_add.FindLastCar();
            car_before.AssignCarAfter(this);
        }

    }
    public override void AssignCarAfter(Car car)
    {
        car_after = car;
    }
    public override Car ReturnCar(bool Before)
    {
        if (Before)
        {
            return car_before;
        }
        else
        {
            return car_after;
        }
    }
    public override void ApplyVelocity(float vel)
    {
        velocity = vel;
        if (velocity < 0) velocity = 0;

    }
    private void Move()
    {


        //interpolate between points
        if (isApproaching)
        {
            transform.rotation = Quaternion.LookRotation((destination-transform.position).normalized);
            transform.position=Vector3.MoveTowards(transform.position, destination,velocity*Time.deltaTime);
            if(transform.position==destination )
            {
                tm.rm.GetNext(ref startPosition, ref destination, start, end);
                isApproaching = false;
                alpha = 0;
            }
        }
        else 
        {
            alpha += velocity * Time.deltaTime;
            transform.rotation = Quaternion.LookRotation((destination - startPosition).normalized);
            transform.position = Vector3.Lerp(startPosition, destination, alpha);
        }

        if (alpha > 1)
        {
            RequestNextPos();
        }
    }
    public override void TrainTick()
    {
        Move();
        if (car_after != null)
        {
            car_after.ApplyVelocity(velocity);
        }
        if (car_before == null) ApplyVelocity(velocity- (float)2*Time.deltaTime) ;



    }
    private void Update()
    {
        TrainTick();
    }
    void UpdateCoalMesh()
    {
        coal.transform.localScale.Set(coal.transform.localScale.x, Mathf.Lerp(0, 0.3f, coalAmount / maxCoalAmount), coal.transform.localScale.z);
    }
    public override void RequestNextPos()
    {
        alpha -= 1;

            tm.rm.Next(ref start, ref end);
            tm.rm.GetNext(ref startPosition, ref destination, start, end);
        if (car_before != null)
        {
            Vector2Int ss = start, ee=end;
            tm.rm.Next(ref ss, ref ee);
            //check if the car is on the same rails

            if ((car_before.start != start&& car_before.start != ss) || (car_before.end != end && car_before.end != ee)) 
            {
                if (car_before.GetComponent<Car_Train>() != null)
                {
                    if (car_before.GetComponent<Car_Train>().isrunning && !doOnce)
                    {
                        tm.gm.Lose("You lost All cars!");
                        doOnce = true;

                    }


                }
                else car_before.DisconnectCars();
            }
        }

        



    }
    public override Vector3 GetRearPoint()
    {
        return transform.position + Vector3.back * (length / 2);
    }
    public void FillWithCoal(int amount)
    {
        if (amount == 0) return;
        tm.gm.coalAmount += Mathf.Clamp(amount,0,maxCoalAmount-coalAmount);
        coalAmount += Mathf.Clamp(amount, 0, maxCoalAmount - coalAmount);
        UpdateCoalMesh();
        Debug.Log(tm.gm.coalAmount);
    }
    public void OnDisconnected()
    {
        tm.gm.DecreaseCoal(coalAmount);
        StartCoroutine(tm.DeleteCarCoroutine(this));
    }
        
    
}
