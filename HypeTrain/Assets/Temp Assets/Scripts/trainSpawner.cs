﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class trainSpawner : MonoBehaviour {


	//variables
	private int carsCompleted;
	public GameObject[] possTrains;
	private Queue<GameObject> trains;
	private GameObject tempTrain;
	public GameObject player;
	private getWidthCar widthFind;
	private float theWidth;
	public float widthBetween = 2f;
	private float begTim;
	public float railHeight = 0f;
	public float despawnBuffer = 8f; //if this is increased, there could be problems with entering trains properly
	public float trainFallSpeed = .07f;
	public GameObject deadTrain;
	public float deathDelay = 8f;
	public Vector2 fallAwayPoint;
	
	void Start () {
		//begTim = Time.time;
		trains = new Queue<GameObject>();
		if (MainMenu.tutorial) { //if tutorial is on, load tutorial cars first
			QueueAndMove(possTrains[0]);
			QueueAndMove(possTrains[1]);		
		} else {  				 //otherwise load 2 random cars
			QueueAndMove();
			QueueAndMove();
		}
		player = GameObject.Find ("character");
	}

	//loads a random car from a list of posible cars, spawn car right justified of this objects position, then moves transform by car width
	void QueueAndMove(){ 
		tempTrain = (GameObject)Instantiate(possTrains[Random.Range(2, possTrains.GetLength(0))], transform.position, Quaternion.identity); //Instantiate random train at position of trainspawner
		theWidth = tempTrain.GetComponent<getWidthCar> ().carWidth (); //get car width				
		float railToCenter = railHeight - tempTrain.transform.Find ("base").transform.localPosition.y;
		tempTrain.transform.position = new Vector2(tempTrain.transform.position.x + theWidth/2, railToCenter); //right justify the car to properly space them
		trains.Enqueue(tempTrain); //put train game object into trains queue
		gameObject.transform.position = new Vector2(transform.position.x + theWidth + widthBetween, transform.position.y); //move transform width forward
	}

	void QueueAndMove(GameObject traincar){ //loads specific car passed in rather than random car
		tempTrain = (GameObject)Instantiate(traincar, transform.position, Quaternion.identity); //Instantiate random train at position of trainspawner
		theWidth = tempTrain.GetComponent<getWidthCar> ().carWidth (); //get car width				
		float railToCenter = railHeight - tempTrain.transform.Find ("base").transform.localPosition.y;
		tempTrain.transform.position = new Vector2(tempTrain.transform.position.x + theWidth/2, railToCenter);
		trains.Enqueue(tempTrain); //put train game object into trains queue
		gameObject.transform.position = new Vector2(transform.position.x + theWidth + widthBetween, transform.position.y); //move transform width forward
	}
	

	public void KillTrain() { 
		//NOTE: if trains are despawning improperly, may need to make this function on its own instantiated object
		if(!playerWithinFirst ()){
			Destroy (deadTrain);
			CancelInvoke();
			deadTrain = (GameObject)trains.Dequeue();
			fallAwayPoint = new Vector2(deadTrain.transform.position.x - deadTrain.GetComponent<getWidthCar>().carWidth() * 3f,  deadTrain.transform.position.y);
			Destroy(deadTrain, deathDelay);
			Invoke ("emptyDeadTrain", deathDelay);
			QueueAndMove();
		}
	}

	public void emptyDeadTrain ()
	{
		deadTrain = null;
		//fallAwayPoint = null;
	}

	public bool playerWithinFirst() //checks if player is within the left and right bounds of the first car in the queue, with a bit of a buffer to make sure cars despawn correctly
	{
		GameObject trainCheck = (GameObject)trains.Peek();
		float leftPos = trainCheck.transform.Find ("left").transform.position.x;
		float rightPos = trainCheck.transform.Find ("right").transform.position.x;
		return (leftPos - despawnBuffer < player.transform.position.x && player.transform.position.x < rightPos + despawnBuffer);
	}

	public float headCenter() 
	{
		GameObject trainCheck = (GameObject)trains.Peek ();
		if (trainCheck.tag == "bigCar") {
			return 1f; //Camera2D knows that 1 means it's a long car
		} else {
			return trainCheck.transform.Find ("center").transform.position.x;
		}
	}

	public GameObject headPanel()
	{
		GameObject trainCheck = (GameObject)trains.Peek();
		return trainCheck.transform.FindChild ("sidepanel").gameObject;
	}

	public Vector3 headVault()
	{
		GameObject trainCheck = (GameObject)trains.Peek();
		return trainCheck.transform.FindChild ("objects").transform.FindChild ("vault").transform.position;
	}

	// Update is called once per frame
	void Update () {
		if (!playerWithinFirst () && player.transform.position.x > -40f) { //if the player isn't within bounds of a train car, kill the first train of the queue.
			//bool timer = (Time.time > begTim + 2.0f);
			//if(timer)
			//{
			KillTrain ();
			//	begTim = Time.time;
			//}
		}
	}
	void FixedUpdate() {
		if(deadTrain != null) {
			deadTrain.transform.position = Vector2.Lerp (deadTrain.transform.position, fallAwayPoint, Time.deltaTime * trainFallSpeed);
		}
	}

	public Vector3 exitTele() 
	{
		GameObject trainCheck = (GameObject)trains.Peek();

		return  trainCheck.transform.Find("train_car_roof").transform.Find ("exit").gameObject.transform.position;
	}


} 
