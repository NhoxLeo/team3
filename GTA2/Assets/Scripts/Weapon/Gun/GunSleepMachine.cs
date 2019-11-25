﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunSleepMachine : PlayerGun
{
    void Start()
    {
        gunType = GunState.SleepMachinegun;
        bulletPoolCount = 50;

        base.InitGun();
        base.InitBullet("SleepMachine");
    }
}
