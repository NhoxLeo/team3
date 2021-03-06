﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DoctorData", menuName = "DB/Doctor")]
public class DoctorData : ScriptableObject
{
	public int maxHp = 100;
	public float moveSpeed = 2.0f;
	//public float jumpTime = 1.0f;
	public float downTime = 3.0f;
    public float jumpTime = 0.5f;
    public float carOpenTime = 0.5f;
	public float findRange = 10.0f;
	public float punchRange = 0.08f;
	public float shotRange = 10.0f;
	
	public float minIdleTime = 0.3f;
	public float maxIdleTime = 1.0f;
	public float minWalkTime = 10.0f;
	public float maxWalkTime = 15.0f;
	public float healTime = 3.0f;

	public int money;
}