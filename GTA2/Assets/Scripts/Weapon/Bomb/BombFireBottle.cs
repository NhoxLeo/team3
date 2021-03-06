﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombFireBottle : Bullet
{
    // Start is called before the first frame update
    [Header("Physics")]
    public Rigidbody rigidBody;

    [Header("Y Valuable")]
    public float yLaunchPos;
    public float yLaunchPower;



    float explosionPower = 2.0f;
    protected override void Awake()
    {
        base.Awake();
        bulletDeActiveTime = .1f;
    }

    // Update is called once per frame
    public override void SetBullet(GunState type, Vector3 pos, Vector3 dir, float bullettoSize)
    {
        base.SetBullet(type, pos, dir, bullettoSize);
        transform.position += Vector3.up * yLaunchPos;
    }

    public void SetForce(float forceValue)
    {
        Vector3 newVec3 = bulletDir * bulletSpeed * forceValue;
        newVec3.y = yLaunchPower;

        rigidBody.velocity = Vector3.zero;
        rigidBody.AddForce(newVec3, ForceMode.Impulse);
    }



    protected override void OnTriggerEnter(Collider other)
    {
        if (
            other.gameObject.CompareTag("Ground") ||
            other.gameObject.CompareTag("Wall"))
        {
            base.Explosion();
            CameraController.Instance.StartShake(explosionPower, transform.position);
        }
    }
}
