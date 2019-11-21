﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunFireBottle : Gun
{
    // Start is called before the first frame update
    public GameObject smokePref;
    public List<FireBottleSmoke> smokeList;
    public int smokePoolCnt;
    public float moveThrowPower;

    private float intervalDelta;
    private int smokeIdx = 0;
    private bool isPrevShot;
    void Start()
    {
        gunType = GunState.FireBottle;

        InitGun();
        base.InitBullet("FireBottle");
        SetSmoke();

        player = userObject.GetComponent<Player>();
        intervalDelta = .0f;
        isPrevShot = true;
    }



    void SetSmoke()
    {
        GameObject EffectPool = new GameObject();
        EffectPool.name = "SmokePool";

        smokeList =
            GetPool<FireBottleSmoke>.GetListComponent(
            SetPool.PoolMemory(smokePref, EffectPool, smokePoolCnt, "Smoke"));

        foreach (var item in smokeList)
        {
            item.gameObject.SetActive(true);
        }
    }

    protected override void UpdateShot()
    {
        if (isKeyShot || isButtonShot)
        {
            intervalDelta += Time.deltaTime;
            isPrevShot = false;
        }

        else if ((!isKeyShot && !isPrevShot))
        {
            isPrevShot = true;

            if (shootInterval < intervalDelta)
            {
                intervalDelta = shootInterval;
            }
            if (player.isWalk)
            {
                intervalDelta += moveThrowPower;
            }

            smokeList[smokeIdx].SetTargetbullet(bulletList[bulletPoolIndex].gameObject);
            smokeIdx = GetPool<FireBottleSmoke>.PlusListIdx(smokeList, smokeIdx);
            
            BombFireBottle LaunchBullet = (BombFireBottle)ShootSingleBullet(userObject.transform.position);
            LaunchBullet.SetForce(intervalDelta);
            intervalDelta = .0f;
        }
    }
}
