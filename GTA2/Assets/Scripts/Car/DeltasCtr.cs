﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeltasCtr : MonoBehaviour
{
    //asdlkifjsadl;kflk;sadfjk;slak
    // dasjfklsdj;fasdfjals;kfjas;dlfjdfjklsadfjsljk

    public enum DamageDirection
    {
        FrontLeft, FrontRight, RearLeft, RearRight
    }

    public GameObject sirenL;
    public GameObject sirenR;
    public float sirenSpeed;

    public GameObject lightFL, lightFR, lightRL, lightRR;
    public GameObject deltaFL, deltaFR, deltaRL, deltaRR;

    void Start()
    {
        if(sirenL != null && sirenR != null)
        {
            StartCoroutine(SirenCor());
        }
    }

    IEnumerator SirenCor()
    {
        while (true)
        {
            sirenL.SetActive(true);
            sirenR.SetActive(false);
            yield return new WaitForSeconds(sirenSpeed);

            sirenL.SetActive(false);
            sirenR.SetActive(true);
            yield return new WaitForSeconds(sirenSpeed);
        }
    }

    public void TurnOnFrontLight()
    {
        if (deltaFL.activeSelf || deltaFR.activeSelf)
            return;

        lightFL.SetActive(true);
        lightFR.SetActive(true);
    }

    public void TurnOffFrontLight()
    {
        lightFL.SetActive(false);
        lightFR.SetActive(false);
    }

    public void TurnOnRearLight()
    {
        if (deltaRL.activeSelf || deltaRR.activeSelf)
            return;

        lightRL.SetActive(true);
        lightRR.SetActive(true);
    }

    public void TurnOffRearLight()
    {
        lightRL.SetActive(false);
        lightRR.SetActive(false);
    }

    public void Damage(DamageDirection damageDirection)
    {
        switch (damageDirection)
        {
            case DamageDirection.FrontLeft:
                {
                    deltaFL.SetActive(true);
                }
                break;
            case DamageDirection.FrontRight:
                {
                    deltaFR.SetActive(true);
                }
                break;
            case DamageDirection.RearLeft:
                {
                    deltaRL.SetActive(true);
                }
                break;
            case DamageDirection.RearRight:
                {
                    deltaRR.SetActive(true);
                }
                break;
        }
    }
}
