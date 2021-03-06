﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    // Start is called before the first frame update
    [Header("Bullet Value")]
    public int bulletDamage;

    public float bulletLifeTime;
    public float bulletSpeed;
    public float explosionArea;

    [Header("Bullet Info")]
    public BulletInformation bulletInfo;

    [Header("Bullet Explosion")]
    public GameObject explosionPref;
    public AudioClip explosionSound;


    protected Vector3 bulletStartPos;
    protected Vector3 bulletDir;
    protected GunState bulletType;

    protected CapsuleCollider bulletcollider;
    protected float bulletLifeDelta = .0f;
    protected float bulletArea;

    protected bool isLife = false;
    protected float bulletActiveDelta = .0f;
    protected float bulletDeActiveTime = .01f;

    protected ExplosionEffect explosionEffect;

    protected virtual void Awake()
    {
        bulletcollider = GetComponentInChildren<CapsuleCollider>();
        bulletcollider.isTrigger = true;

        bulletArea = bulletcollider.radius;

        bulletDamage = bulletInfo.bulletDamage;
        bulletLifeTime = bulletInfo.bulletLifeTime;
        bulletSpeed = bulletInfo.bulletSpeed;
        explosionArea = bulletInfo.explosionArea;

        if (explosionPref != null)
        {
            explosionEffect = Instantiate(explosionPref).GetComponent<ExplosionEffect>();
            explosionEffect.gameObject.transform.SetParent(PoolManager.Instance.transform);
        }
    }

    public virtual void SetBullet(GunState type, Vector3 triggerPos, Vector3 dir, float bullettoSize)
    {
        bulletType = type;
        bulletDir = dir;

        bulletStartPos = triggerPos + bulletDir * bullettoSize;
        bulletStartPos.y = triggerPos.y;
        bulletLifeDelta = .0f;

        transform.eulerAngles = new Vector3(90.0f, dir.y + 90.0f, 90.0f);
        transform.position = bulletStartPos;
        gameObject.SetActive(true);
        isLife = true;
        bulletActiveDelta = .0f;

        if (bulletcollider != null)
        {
            bulletcollider.radius = bulletArea;
        }
    }


    protected virtual void FixedUpdate()
    {
        UpdateBullet();
        UpdateActive();
    }
    protected virtual void UpdateBullet()
    {
        bulletDir.y = .0f;
        transform.position +=  bulletDir * bulletSpeed * Time.deltaTime;

        bulletLifeDelta += Time.deltaTime;
        if (bulletLifeTime < bulletLifeDelta)
        {
            bulletLifeDelta = .0f;
            Explosion();
        }
    }
    protected void UpdateActive()
    {
        if (isLife)
        {
            return;
        }

        bulletActiveDelta += Time.deltaTime;
        if (bulletActiveDelta > bulletDeActiveTime)
        {
            PoolManager.ReleaseObject(gameObject);
        }          
    }




    public virtual void Explosion()
    {
        if (bulletcollider != null)
        {
            bulletcollider.radius = explosionArea;
        }

        isLife = false;

        if (explosionEffect != null)
        {
            explosionEffect.SetExplosion(transform.position);
        }

        SoundManager.Instance.PlayClipToPosition(explosionSound, SoundPlayMode.ExplosionSFX, transform.position);
    }


    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Wall"))
        {
            Explosion();
        }
    }


    protected virtual void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            Explosion();
        }
    }
}
