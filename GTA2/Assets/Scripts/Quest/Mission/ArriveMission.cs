﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// 누굴 죽이는가
// 어디에 도착하는가.
public class ArriveMission : QuestCondition
{
    public GameObject arrivePosTarget;
    void Start()
    {
        questStatus = QuestStatus.Arrive;
    }

    public override bool CheckCondition()
    {



        return true;
    }
}