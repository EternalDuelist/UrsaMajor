// AUTHORS
// Hayden Platt     (platt@ursamajorgames.com)

using UnityEngine;
using System.Collections.Generic;

public class TrainExit : MonoBehaviour {
	
	private GameObject Player = null;
	[HideInInspector] public GameObject cameraObj;
	[HideInInspector] public GameObject sidePanel;
	private GameObject trainSpawn;
	private Vector2 exitPos;
	private Collider2D playerColl;
	public float vertForce = 7500f;
	public float zoomOutSpeed = 2.75f;
	public bool check;
	private Animator hatchAnimator;
	private bool justExit = false;

	
	//Audio variables
	private bool soundPlayed;
	public AudioClip exitSound;

	//Find the hatch's Animator
	void Awake() {
		hatchAnimator = transform.parent.gameObject.GetComponent<Animator>();
	}

	// Use this for initialization
	void Start () {
		cameraObj = GameObject.Find("Main Camera");
		Player = GameObject.Find("character");
		sidePanel = GameObject.Find ("sidepanel");
		trainSpawn = GameObject.Find ("trainSpawner");
	}
	
	// Update is called once per frame
	void Update () {
		if (justExit && Player.GetComponent<Rigidbody2D>().velocity.y < 0) {
			cameraObj.GetComponent<Camera2D> ().zoomIn ();
		}
	}
	//Check if E is pressed in trigger zone
	void OnTriggerEnter2D(Collider2D hit) {
		playerColl = hit;
		if (Input.GetButton ("Interact") && hit.tag == "Player")
		{
			ExitTrain (hit);
		}
	}
	void OnTriggerStay2D(Collider2D hit) {
		if (Input.GetButton ("Interact") && hit.tag == "Player")
		{
			ExitTrain (hit);
		}
	}
	void OnTriggerExit2D(Collider2D hit) 
	{
		if(transform.parent.transform.parent.transform.parent.name.Contains("TutorialCar_2")){ //if this car is the 2nd Tutorial Car, turn off tutorial when you leave it.
			TutShopController.tutorial = false;
		}
		check = false;
		if(!transform.parent.transform.parent.transform.parent.name.Contains("DinoCar")){ //if this car is the 2nd Tutorial Car, turn off tutorial when you leave it.
			Invoke ("ignoreExitCollide", .5f);
		}
	}

	void ignoreExitCollide()
	{
		Physics2D.IgnoreCollision (playerColl, transform.parent.gameObject.GetComponent<Collider2D>(), false);
	}

	void soundPlayedOff()
	{
		soundPlayed = false;
	}

	void ExitTrain(Collider2D hit)
	{
		//Play exit sound
		if (!soundPlayed) {
			soundPlayed = true;
			Invoke ("soundPlayedOff", 3f);
			AudioSource.PlayClipAtPoint (exitSound, transform.position);

			hatchAnimator.Play ("Entry"); //Play exit animation once
		
			ScoreKeeper.carsCompleted += 1;

			Physics2D.IgnoreCollision (hit, transform.parent.gameObject.GetComponent<Collider2D>(), true);

			Player.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
			exitPos = trainSpawn.GetComponent<trainSpawner> ().exitTele ();
			exitPos.y -= .4f;
			Player.GetComponent<Rigidbody2D>().position = exitPos;
			
			Player.GetComponent<Rigidbody2D>().AddForce (new Vector2 (0, 2500f));
			//Make sidePanel visible again
			sidePanel = trainSpawn.GetComponent<trainSpawner> ().headPanel ();
			sidePanel.SetActive (true);
			//Unlock camera, hard zoom out, slow zoom in
			cameraObj.GetComponent<Camera2D> ().setLock (false);
			Player.GetComponent<Rigidbody2D>().gravityScale = 2f; //Temporarily lower gravity's effect
			Camera2D.setCameraTarget (40f, zoomOutSpeed);
			//cameraObj.GetComponent<Camera2D> ().scheduleZoomIn ();
			justExit = true; //Calls zoom in once player starts falling

		}
	}	
}
