﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunElectric : Gun
{
    public int electricWaveMaxIndex;
    public float electricWaveArea;
    public float electricWaveAngle;
    public SpawnManager spawnManager;

    private List<GameObject> objectList;
    private List<GameObject> noneTargetObjectList;

    void Start()
    {
        gunType = GunState.Electric;
        bulletPoolCount = 50;

        objectList = new List<GameObject>();
        noneTargetObjectList = new List<GameObject>();


        foreach (var citizen in spawnManager.activeCitizenList)
        {
            objectList.Add(citizen.gameObject);
        }

        InitGun();
        base.InitBullet("Electric");
    }


    protected override void Update()
    {
        base.UpdateDirection();
        UpdateKeyInput();
    }
    protected override void UpdateKeyInput()
    {
        if (Input.GetKey(KeyCode.A) || isShot)
        {
            shootDelta += Time.deltaTime;
            if (shootInterval < shootDelta)
            {
                FireGun();
                shootDelta = .0f;
            }
        }
        
        else if (!Input.GetKey(KeyCode.A))
        {
            StopGun();
        }
    }



    void FireGun()
    {
        List<BulletElectric> activeBulletList = new List<BulletElectric>();
        List<GameObject> targetObjects = GetObjectsInAttackRange(userObject.transform.position, objectList);

        noneTargetObjectList.Clear();
        foreach (var item in objectList)
        {
            if (item.gameObject.activeInHierarchy == false)
            {
                continue;
            }

            noneTargetObjectList.Add(item);
        }

        foreach (var target in targetObjects)
        {
            BulletElectric fireBullet = (BulletElectric)ShootSingleBullet(userObject.transform.position);
            fireBullet.SetTarget(target);
            activeBulletList.Add(fireBullet);

            noneTargetObjectList.Remove(target);
        }


        ElectricWave(noneTargetObjectList, activeBulletList, electricWaveMaxIndex);
    }


    void ElectricWave(List<GameObject> objectList, List<BulletElectric> activeBulletList,  int count)
    {
        if (count <= 0 || activeBulletList.Count == 0 || objectList.Count == 0)
        {
            return;
        }

        List<BulletElectric> nextWaveList = new List<BulletElectric>();

        foreach (var bulletObj in activeBulletList)
        {
            Vector3 triggerPos = bulletObj.gameObject.transform.position;
            Vector3 rightVector = 
                bulletObj.gameObject.transform.right * bulletObj.gameObject.transform.localScale.x * 4.0f;
            triggerPos += rightVector;
            List<GameObject> targetObjects = GetObjectsInAttackRange(triggerPos, objectList);

            foreach (var target in targetObjects)
            {
                
                BulletElectric fireBullet = (BulletElectric)ShootSingleBullet(triggerPos);
                fireBullet.SetTarget(target);
                nextWaveList.Add(fireBullet);
                noneTargetObjectList.Remove(target);
            }

            ElectricWave(noneTargetObjectList, nextWaveList,--count);
        }

    }


    void StopGun()
    {
        DisableAllBullet();
    }

    List<GameObject> GetObjectsInAttackRange(Vector3 centerPos, List<GameObject> targetList)
    {
        List<GameObject> returnList = new List<GameObject>();
        foreach (var item in targetList)
        {
            if (CheckAttackRange(centerPos, item))
            {
                returnList.Add(item);
            }
        }

        return returnList;
    }

    bool CheckAttackRange(Vector3 waveCenterPos, GameObject targetObj)
    {
        // 거리 필터링
        Vector3 toTargetDir = targetObj.transform.position - waveCenterPos;
        if (electricWaveArea < toTargetDir.magnitude)
        {
            return false;
        }


        Vector3 toTargetDirNormalize = toTargetDir;

        toTargetDirNormalize.Normalize();

        // 방향 필터링

        float dotToTarget = Vector3.Dot(userObject.transform.forward, toTargetDirNormalize);

        if ((dotToTarget < .0f) ||
            (dotToTarget > Mathf.Sin(electricWaveAngle * Mathf.Deg2Rad)))
        {
            return false;
        }



        // 물체 감지 필터링
        Ray originRay = new Ray();
        RaycastHit originRayHit = new RaycastHit();

        originRay.origin = waveCenterPos;
        originRay.direction = toTargetDir;
        bool isHit = Physics.Raycast(originRay, out originRayHit, electricWaveArea);
        Debug.DrawRay(originRay.origin, originRay.direction * toTargetDir.magnitude, Color.red, .0f);

        if (isHit &&
            originRayHit.transform.gameObject != targetObj)
        {
            return false;
        }

        return true;
    }
}