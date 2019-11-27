﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarSpawnManager : MonoSingleton<CarSpawnManager>
{
    [Header("최초에는 ActiveList에 Pool의 오브젝트 삽입.")]
    public List<CarManager> activeCarList = new List<CarManager>();
    public List<CarManager> deactiveCarList = new List<CarManager>();
    // Start is called before the first frame update
    void Start()
    {
        CarPositionInit();
        InvokeRepeating("RespawnDisabledCar", 0.5f, 0.5f);
    }

    // Update is called once per frame

    public void CarPositionInit() //중복 방지 생성
    {
        List<Vector3> position = new List<Vector3>();

        for (int i = 0; i < WaypointManager.instance.allWaypointsForCar.Length; i++)
        {
            position.Add(WaypointManager.instance.allWaypointsForCar[i].transform.position);
        }

        foreach (var car in activeCarList)
        {
            int randomIndex = Random.Range(0, position.Count);
            //print(randomIndex + " " + position.Count + " " + );
            car.gameObject.transform.position = position[randomIndex];
            car.gameObject.SetActive(false);
            car.gameObject.SetActive(true);
            position.RemoveAt(randomIndex);
        }
    }
    public void CarRepositioning(CarDamage car)
    {
        int randomIndex = Random.Range(0, WaypointManager.instance.allWaypointsForCar.Length);
        car.gameObject.transform.position = WaypointManager.instance.allWaypointsForCar[randomIndex].transform.position;
    }
    /*   void CheckDeactiveCar()
    {
        List<CarController> tempRemoveCarList = new List<CarController>();

        foreach (var car in deactiveCarList)
        {
            if (!IsSpawnRange(car.transform.position) && !car.gameObject.activeSelf)
            {
                car.gameObject.SetActive(true);
                activeCarList.Add(car);
                tempRemoveCarList.Add(car);
            }
        }
        for (int i = 0; i < tempRemoveCarList.Count; i++)
        {
            CarController removeCar = tempRemoveCarList[i];
            deactiveCarList.Remove(removeCar);
        }
    }
    void CheckActiveCar()
    {
        List<CarController> tempRemoveCarList = new List<CarController>();

        foreach (var car in activeCarList)
        {
            if (IsSpawnRange(car.transform.position) && car.gameObject.activeSelf)
            {
                car.gameObject.SetActive(false);
                deactiveCarList.Add(car);
                tempRemoveCarList.Add(car);
            }
        }
        for (int i = 0; i < tempRemoveCarList.Count; i++)
        {
            CarController removeCar = tempRemoveCarList[i];
            activeCarList.Remove(removeCar);
        }
    }*/

    // 꺼진차 켜기
    void RespawnDisabledCar()
    {
        foreach (var car in activeCarList)
        {
            if (car.gameObject.activeSelf)
                continue;

            GameObject go = WaypointManager.instance.FindRandomCarSpawnPosition();

            
            if (Physics.Raycast(go.transform.position + (Vector3.up * 5), Vector3.down, 10, 1<<8))
            {
                print("스폰지점에 차가 있음");
            }
            else
            {
                car.transform.position = go.transform.position;
                car.gameObject.SetActive(true);
                car.GetComponent<CarManager>().movement.curSpeed = 200;

                Debug.DrawLine(GameManager.Instance.player.transform.position, car.transform.position, Color.red, 0.5f);
                break;
            }
        }
    }
}