using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car_Train : Car
{
    public bool isrunning = false;
    [SerializeField] float acceleration= 1;
    [SerializeField] public float maxVelocity = 100;
    [SerializeField] public int fuel = 1000;
    public int maxFuel;
    
    
    Vector3 startPos;
    private void Start()
    {
        startPos = transform.position;
        maxFuel = fuel;

    }
    override public void Run() 
    {
        isrunning = !isrunning;
    }
   public void DecreaseFuel(int x)
    {
        fuel -= x;
        if (fuel <= 0)
        {
            Run();
            tm.gm.Lose("The Train ran out of fuel");
            tm.CancelInvoke();
        }
    }
    public float GetFuelPercent()
    {
        return (float)fuel / (float)maxFuel;
    }
    override public void DisconnectCars()
    {
        Car x = car_after;
        while (x!=null)
        {
            
            x.gameObject.GetComponent<Car_Child>().OnDisconnected();

            x = x.car_after;
            
        }

        car_after.AssignCarBefore(null);
        tm.gm.Lose("You lost all Cars!");
        Run();

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
        Debug.Log("WARNING, ASSIGNING TRAIN TO A TRAIN");
    }
    public override void AssignCarAfter(Car car)
    {
        car_after = car;
    }
    public override Car ReturnCar(bool Before)
    {
        if (Before)
        {
            return null;
        }
        else
        {
            return car_after;
        }
    }
    void Update()
    {

        TrainTick();
    }
    public override void ApplyVelocity(float vel)
    {
        velocity += vel;
        if (velocity < 0) velocity = 0;
    }
    public override void TrainTick()
    {
        Move();
        //accelerate if running
        if (isrunning)
        {
            ApplyVelocity(Time.deltaTime * acceleration);

            if (velocity > maxVelocity) velocity = maxVelocity;
            if (car_after != null) {
                if (start.x != -1 && car_after.GetComponent<Car_Child>().isApproaching) car_after.ApplyVelocity(railVelocity);

                else car_after.ApplyVelocity(velocity);
            }
        }
        //Slow down when not POWERED
        else {
            ApplyVelocity((float)-1 * Time.deltaTime);
            if (car_after != null)
            {
                if (start.x != -1 && car_after.GetComponent<Car_Child>().isApproaching) car_after.ApplyVelocity(railVelocity);

                else car_after.ApplyVelocity(velocity);
            }
        }



    }
    public List<Car_Child> Children() 
    {
        List<Car_Child> tmp = new List<Car_Child>();
        Car tmp1 = FindLastCar();
        while (true)
        {
            tmp.Add(tmp1.gameObject.GetComponent<Car_Child>());
            if(tmp1!=this)if (tmp1.car_before != this) tmp1 = tmp1.car_before;
            else break;
        }
        return tmp;
    }
    private void Move()
    {

        //interpolate between points
        {
            Vector3 tmp = transform.position;
            alpha += velocity * Time.deltaTime;
            transform.rotation = Quaternion.LookRotation((destination - startPosition).normalized);
            transform.position = Vector3.Lerp(startPosition, destination, alpha);
            railVelocity = Vector3.Distance(transform.position,tmp)/Time.deltaTime;
        }

        if (alpha > 1)
        {
            RequestNextPos();
        }
    }
    public Car GetCarAt(int index)
    {
        Car tmp = this;
        for(int i = 0; i < index; i++)
        {
            tmp = tmp.ReturnCar(false);
            if (tmp == null) break;
        }
        return tmp;
    }
    public override void RequestNextPos()
    {
        alpha -= 1;
        
        tm.rm.Next(ref start, ref end);
        tm.rm.GetNext(ref startPosition, ref destination, start, end);
        



    }
    public override Vector3 GetRearPoint()
    {
        
        return transform.position + Vector3.back * (length / 2);
    }

}
