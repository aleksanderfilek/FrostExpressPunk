using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Car : MonoBehaviour
{
    [SerializeField]
    public Car car_after;
    [SerializeField]
    public float length;
    public float velocity;
    protected float alpha;
    public TrainManager tm;
    public Vector2Int start=new Vector2Int();
    public Vector2Int end = new Vector2Int();
    protected float railVelocity = 0;
    public Vector3 startPosition;
    public Vector3 destination;
    public Car car_before;



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    
    void Update()
    {
        
    }
    abstract public void Run();
    abstract public void DisconnectCars();
    abstract public Car FindLastCar();
    abstract public Car ReturnCar(bool before);
    abstract public void AssignCarBefore(Car_Train train_to_add);
    abstract public void AssignCarAfter(Car car);
    abstract public void ApplyVelocity(float vel);
    abstract public void TrainTick();
    abstract public void RequestNextPos();
    abstract public Vector3 GetRearPoint();


}
