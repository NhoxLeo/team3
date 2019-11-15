﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class People : MonoBehaviour
{
    protected float rotateSpeed = 0.1f;
    protected float moveSpeed = 1.0f;
    protected float runSpeed = 2.0f;

    protected Vector3 movement;
    protected Vector3 direction;
    protected Vector3 targetDirectionVector = Vector3.zero;
    
    [SerializeField]
    protected int hp = 100;

    // ------------ 방향 수정
    protected float hDir = 0;
    protected float vDir = 0;

    //TODO : isDie없이 작동할 수 있도록 수정
    protected bool isDie = false;
    protected abstract void Die();
    protected virtual void Move()
    {
        Vector3 Pos = transform.position;

        Pos.x += transform.forward.x * Time.deltaTime * moveSpeed;
        Pos.z += transform.forward.z * Time.deltaTime * moveSpeed;

        transform.position = Pos;
    }
    
    public void Hurt(int damage)
    {
        hp -= damage;

        //사망시 true
        if (hp <= 0)
        {
            isDie = true;
            Die();
        }
    }
    
    protected void UpdateTargetRotation()
    {
        targetDirectionVector = new Vector3(hDir, 0, vDir).normalized;
    }
    protected void UpdateSlerpedRotation()
    {
        if (0 != hDir || 0 != vDir)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(targetDirectionVector), 0.4f);
        }        
    }
   
}