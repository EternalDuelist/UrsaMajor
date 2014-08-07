﻿using UnityEngine;
using System.Collections;

public enum EnemyState
{
	IDLE,
	ATTACK,
	CHASE
}

public class Enemy : MonoBehaviour {


	private EnemyState State = EnemyState.IDLE;
	public float EnemySpeed = 2f;
	public GameObject Player;
	private float AttackDist = 10f;
	public int direction = 1;
	public float StrollDist = 3f;
	
	private Vector2 StrollStart = new Vector2(0, 0);
	
	private bool strolling = false;


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
		var distToPlayer = Vector2.Distance (transform.position, Player.transform.position);
		//Debug.Log (distToPlayer);
		if (distToPlayer < AttackDist) 
		{
			State = EnemyState.ATTACK;
		}
		Act();
	}

	void Act()
	{
		switch(State)
		{
		case EnemyState.ATTACK: Attack(); break;
		case EnemyState.IDLE: Idle(); break;
		case EnemyState.CHASE: Chase(); break;
		default: Idle();break;
			
		}
	}

	virtual public void Attack()
	{
		if (transform.position.x < Player.transform.position.x) 
		{
			rigidbody2D.velocity = new Vector2 (EnemySpeed, rigidbody2D.velocity.y); 
		} 
		else 
		{
			rigidbody2D.velocity = new Vector2 (EnemySpeed *-1, rigidbody2D.velocity.y); 
		}

	}

	virtual public void Idle()
	{
		if (!strolling)
		{
			StrollStart = transform.position;
			strolling = true;
		}
		
		if (Vector2.Distance (transform.position, StrollStart) > StrollDist) 
		{
			direction *= -1;
			strolling = false;
		}
		
		rigidbody2D.velocity = new Vector2 (EnemySpeed *direction, rigidbody2D.velocity.y); 
		
	}

	void Chase()
	{
		//move until reach edge or near enough to player
		
	}
}
