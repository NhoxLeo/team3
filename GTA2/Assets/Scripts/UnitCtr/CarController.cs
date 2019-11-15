﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class CarController : MonoBehaviour
{
    Rigidbody rbody;
    public TrailRenderer trailLeft;
    public TrailRenderer trailRight;
    public GameObject shadow;

    public enum CarState
    {
        idle, aiNormal, controlledByPlayer, aiChase, aiChaseRight, aiChaseLeft, aiChaseFront, aiChaseBlock, Evade
    }
    public CarState carState = CarState.aiNormal;
    public GameObject chaseTarget; // 나중엔 숨겨야함.
    Vector3 destination;

    public float maxSpeed;
    float maxSpeedMultiplier = 1.0f;
    float curSpeed;
    float targetSpeed;
    public float rotSpeed;

    public LayerMask collisionLayer;
    RaycastHit hit;
    float distToObstacle = Mathf.Infinity;
    Vector3 reboundForce = Vector3.zero;

    float inputH;
    float inputV;

    void Awake()
    {
        rbody = GetComponent<Rigidbody>();

        SetMaxSpeedMultiplier();
    }

    void Update()
    {
        DrawSkidMark();
        UpdateShadowPosition();        
    }

    void FixedUpdate()
    {
        if (carState == CarState.controlledByPlayer)
        {
            PlayerInput();
            MoveCarByInput();
        }
        else
        {
            Raycast();

            switch (carState)
            {
                case CarState.aiNormal:
                    {
                        CarMoveAI();
                    }
                    break;
                case CarState.aiChase:
                case CarState.aiChaseRight:
                case CarState.aiChaseLeft:
                case CarState.aiChaseFront:
                case CarState.aiChaseBlock:
                    {
                        CarChaseAI();
                    }
                    break;
            }
        }
    }

    void SetMaxSpeedMultiplier()
    {
        switch (carState)
        {
            case CarState.idle:
                maxSpeedMultiplier = 1.0f;
                break;
            case CarState.aiNormal:
                maxSpeedMultiplier = 0.4f;
                break;
            case CarState.controlledByPlayer:
                maxSpeedMultiplier = 1.0f;
                break;
            case CarState.aiChase:
                maxSpeedMultiplier = 1.1f;
                break;
            case CarState.aiChaseRight:
            case CarState.aiChaseLeft:
                maxSpeedMultiplier = 1.2f;
                break;
            case CarState.aiChaseFront:
                maxSpeedMultiplier = 1.1f;
                break;
            case CarState.aiChaseBlock:
                maxSpeedMultiplier = 1.0f;
                break;
            case CarState.Evade:
                maxSpeedMultiplier = 1.0f;
                break;
            default:
                maxSpeedMultiplier = 1.0f;
                break;
        }
    }

    void PlayerInput()
    {
        inputV = Input.GetAxisRaw("Vertical");
        inputH = Input.GetAxisRaw("Horizontal");
    }

    void MoveCarByInput()
    {
        reboundForce *= 0.85f;

        curSpeed += 100 * inputV * Time.deltaTime;

        if (inputV == 0)
        {
            curSpeed *= 0.97f;
            if (curSpeed < 1)
                curSpeed = 0;
        }

        curSpeed *= 1 - (Mathf.Abs(inputH) / 70);

        curSpeed = Mathf.Clamp(curSpeed, (maxSpeed * maxSpeedMultiplier / 2) * -1, maxSpeed * maxSpeedMultiplier);

        transform.Rotate(0, inputH * rotSpeed * Time.deltaTime * (Mathf.Abs(curSpeed) / 400), 0);
        rbody.velocity = transform.forward * curSpeed * Time.deltaTime + reboundForce;
    }

    void DrawSkidMark()
    {
        if (Mathf.Abs(inputH) > 0.3f && curSpeed > 150)
        {
            trailLeft.emitting = true;
            trailRight.emitting = true;
        }
        else
        {
            trailLeft.emitting = false;
            trailRight.emitting = false;
        }
    }

    void UpdateShadowPosition()
    {
        shadow.transform.position = transform.position + new Vector3(0.05f, 0, -0.05f);
    }

    void Raycast()
    {
        distToObstacle = Mathf.Infinity;

        if (Physics.Raycast(transform.position, transform.forward, out hit, 1f, collisionLayer))
        {
            if(hit.transform.tag == "TrafficLight")
            {
                if(Vector3.Dot(transform.forward, hit.transform.forward) < -0.8f)
                {
                    distToObstacle = hit.distance;
                }                
            }
            else
            {
                distToObstacle = hit.distance;
            }
        }

        DrawRaycastDebugLine();
    }

    void DrawRaycastDebugLine()
    {
        if (distToObstacle < Mathf.Infinity)
        {
            Debug.DrawRay(transform.position, transform.forward * hit.distance, Color.red);
        }
        else
        {
            Debug.DrawRay(transform.position, transform.forward * 1f, Color.blue);
        }
    }

    void CarChaseAI()
    {
        if (chaseTarget == null)
        {
            carState = CarState.aiNormal;
            return;
        }

        ChaseStateLoop();
        CarMoveAI();
    }

    void ChaseStateLoop()
    {
        switch (carState)
        {
            case CarState.aiChaseLeft:
                {
                    destination = chaseTarget.transform.position + (chaseTarget.transform.forward * 1.5f) + (chaseTarget.transform.right * -1.5f);
                }
                break;
            case CarState.aiChaseRight:
                {
                    destination = chaseTarget.transform.position + (chaseTarget.transform.forward * 1.5f) + (chaseTarget.transform.right * 1.5f);
                }
                break;
            case CarState.aiChaseFront:
            case CarState.aiChaseBlock:
                {
                    destination = chaseTarget.transform.position + (chaseTarget.transform.forward * 2f);
                }
                break;
            default:
                {
                    destination = chaseTarget.transform.position;
                }
                break;
        }

        Vector3 dir = destination - transform.position;
        float dist = dir.magnitude;

        switch (carState)
        {
            case CarState.aiChase:
                {
                    if (dist < 2.7f)
                    {
                        if (Random.Range(0, 2) == 0)
                        {
                            carState = CarState.aiChaseRight;
                        }
                        else
                        {
                            carState = CarState.aiChaseLeft;
                        }
                    }
                }
                break;
            case CarState.aiChaseLeft:
            case CarState.aiChaseRight:
                {
                    if (dist < 1)
                    {
                        carState = CarState.aiChaseFront;
                    }
                    else if (dist > 3.5f)
                    {
                        carState = CarState.aiChase;
                    }
                }
                break;
            case CarState.aiChaseFront:
                {
                    if (dist > 3.5f)
                    {
                        carState = CarState.aiChase;
                    }
                    else if (dist < 1)
                    {
                        carState = CarState.aiChaseBlock;
                    }
                }
                break;
            case CarState.aiChaseBlock:
                {
                    if (dist > 3)
                    {
                        carState = CarState.aiChase;
                    }
                }
                break;
        }

        SetMaxSpeedMultiplier();
    }

    void CarMoveAI()
    {
        Vector3 dir = destination - transform.position;
        dir.y = 0;

        if(carState != CarState.aiChaseBlock)
        {
            float angle = Vector3.SignedAngle(dir, transform.forward, Vector3.up) / 40;
            angle = Mathf.Clamp(angle, -1, 1);

            inputH = -angle;
        }

        if (carState == CarState.aiChaseBlock)
        {
            targetSpeed = 0;
        }
        else
        {
            targetSpeed = Mathf.Clamp(distToObstacle - 1, 0, 1) * maxSpeed * maxSpeedMultiplier;
        }

        targetSpeed *= (1 - (Mathf.Abs(inputH) / 1.5f));

        if (targetSpeed < curSpeed)
        {
            inputV = -4;
        }
        else if (targetSpeed * 0.9f > curSpeed)
        {
            inputV = 1;
        }
        else
        {
            inputV = 0;
        }

        MoveCarByInput();
    }

    public void SetDestination(Vector3 pos)
    {
        destination = pos;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(destination, 0.25f);
    }

    void OnDrawGizmosSelected()
    {
        Handles.Label(destination, "destination");
        Handles.Label(transform.position + Vector3.right, "spd: " + curSpeed);
    }

    void OnCollisionEnter(Collision col)
    {
        if (col.transform.tag == "Wall")
        {
            curSpeed *= 0.25f;
            Vector3 inDirection = transform.forward;
            reboundForce = Vector3.Reflect(inDirection, col.contacts[0].normal) * curSpeed * 0.15f;
            //Debug.DrawLine(transform.position, transform.position - inDirection, Color.blue, 1f);
            //Debug.DrawLine(transform.position, transform.position + reboundForce, Color.red, 1f);
        }
        else if (col.transform.tag == "Car")
        {
            curSpeed *= 0.25f;
        }
    }

    void OnCollisionStay(Collision col)
    {
        if (col.transform.tag == "Wall")
            curSpeed *= 0.9f;
    }
}
