﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunFire : PlayerGun
{
    // Start is called before the first frame update.
    float soundPlayDelta;


    public override void Init()
    {
        base.Init();
        base.InitGun();

        soundPlayDelta = .0f;
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        soundPlayDelta += Time.deltaTime;
    }

    protected override void SFXPlay()
    {
        SoundManager.Instance.PlayClipToPosition(gunSound, SoundPlayMode.ObjectSFX, userObject.transform.position);
    }
}
