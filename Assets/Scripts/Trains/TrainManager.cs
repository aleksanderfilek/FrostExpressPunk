using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainManager : MonoBehaviour
{
    [SerializeField] List<GameObject> trainPrefabs;
    public Car_Train TrainOnTheMap;
    public List<Vector3> points;
    public RailManager rm;
    public GameManager gm;

    // Start is called before the first frame update
    void Start()
    {
        rm = GetComponent<RailManager>();
        gm = GetComponent<GameManager>();
    }
    public IEnumerator DeleteCarCoroutine(Car car)
    {
        //Print the time of when the function is first called.
        Debug.Log("Started Coroutine at timestamp : " + Time.time);

        //yield on a new YieldInstruction that waits for 5 seconds.
        yield return new WaitForSeconds(3);

        //After we have waited 5 seconds print the time again.
        Debug.Log("Finished Coroutine at timestamp : " + Time.time);
        Destroy(car.gameObject);
    }
    void DecreaseCoal()
    {

        //yield on a new YieldInstruction that waits for 5 seconds.
        TrainOnTheMap.DecreaseFuel(1);
        Debug.Log(TrainOnTheMap.fuel);

    }

    public void Init (int amountOfCars,int initialFuel)
    {
        CreateTrain();
        TrainOnTheMap.fuel = initialFuel;
        TrainOnTheMap.maxFuel = TrainOnTheMap.fuel;
        for(int i=0; i < amountOfCars; i++)
        {
            CreateCar(1, TrainOnTheMap);
        }
        
        TrainOnTheMap.Run();
        InvokeRepeating("DecreaseCoal", 1.0f, 1.0f);

    }
    

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(1))// Click
        {
            OnClick();
        }

    }
    void CreateTrain()
    {
        GameObject tmp = GameObject.Instantiate(trainPrefabs[0], gameObject.transform, true);
        //set the Train
        TrainOnTheMap = tmp.GetComponent<Car_Train>();
        TrainOnTheMap.start = rm.firstStart;

        TrainOnTheMap.end = rm.secondStart;

        rm.GetNext(ref TrainOnTheMap.startPosition, ref TrainOnTheMap.destination, TrainOnTheMap.start, TrainOnTheMap.end);

        tmp.transform.position = TrainOnTheMap.startPosition;

        tmp.transform.rotation = Quaternion.LookRotation((TrainOnTheMap.destination - TrainOnTheMap.startPosition).normalized);

        TrainOnTheMap.tm = this;



    }
    void OnClick()
    {

        if (TrainOnTheMap != null)
        {
            if ((TrainOnTheMap.FindLastCar() != TrainOnTheMap)) 
                { 
                    if (TrainOnTheMap.FindLastCar().car_before != TrainOnTheMap)
                {

                    TrainOnTheMap.FindLastCar().car_before.DisconnectCars();
                    TrainOnTheMap.maxVelocity += (float)1.5;
                } 
            }
        }
    }



        void CreateCar(int id, Car_Train parent)
        {//Can Be done only when main train is on the map
            if (parent != null)
            {
                GameObject tmp = GameObject.Instantiate(trainPrefabs[id], gameObject.transform, true);
                Car lastCar = TrainOnTheMap.FindLastCar();

                tmp.transform.position = lastCar.gameObject.transform.position - (lastCar.length / 2 + tmp.GetComponent<Car>().length / 2)*lastCar.transform.forward;
                tmp.transform.eulerAngles = parent.gameObject.transform.eulerAngles;
            if (lastCar == TrainOnTheMap) 
            {
                tmp.GetComponent<Car>().destination = lastCar.startPosition;
                tmp.GetComponent<Car>().start = lastCar.start;
                tmp.GetComponent<Car>().end = lastCar.end;

            } 
            else 
            {
                tmp.GetComponent<Car>().destination = lastCar.destination;
                tmp.GetComponent<Car>().start = lastCar.start;
                tmp.GetComponent<Car>().end = lastCar.end;
            }
            
            tmp.GetComponent<Car>().startPosition = transform.position;


            //Assign Train car hierarchy to car
            tmp.GetComponent<Car>().AssignCarBefore(TrainOnTheMap);
            tmp.GetComponent<Car>().tm = this;

            TrainOnTheMap.maxVelocity -= (float)1.5;

            }
           

        }
    public void Unload()
    {
        foreach(Car a in TrainOnTheMap.Children())
        {
            Destroy(a.gameObject);
        }
        Destroy(TrainOnTheMap.gameObject);
        CancelInvoke();

    }



    }

