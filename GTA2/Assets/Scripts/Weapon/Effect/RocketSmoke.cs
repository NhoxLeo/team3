﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketSmoke : BulletEffect
{

    

    public void SetTargetbullet(GameObject target)
    {
        myParticle = GetComponent<ParticleSystem>();
        myParticle.Play();
        gameObject.SetActive(true);
        bulletTarget = target;
        releaseDelta = .0f;
    }

    protected override void Update()
    {
        base.Update();
    }

}
