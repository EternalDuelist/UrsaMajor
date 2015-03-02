using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class buySkin1 : MonoBehaviour {

	public int price;
	public Sprite gunArm;
	[HideInInspector] public GameObject arm;

	Text text;
	ParticleSystem particles;


	// Use this for initialization
	void Start () {
		particles = transform.Find ("glow").GetComponent<ParticleSystem> ();
		text = transform.Find ("Canvas/Text").GetComponent <Text> ();
		text.text = "[E] $" + price;
		arm = GameObject.Find("character/gun");
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter2D(Collider2D colObj){
		if(colObj.tag == "Player"){
			if(Game.skin1 == false){
			if(!PlayerPrefsBool.GetBool ("skin1")){
				text.enabled = true;
				particles.Play();
			} else {
				text.enabled = true;
				text.text = "OWNED";
			} 
		}
	}
	void OnTriggerExit2D(Collider2D colObj){
		if(colObj.tag == "Player"){
				text.enabled = false;
				particles.Stop();
		}
	}
	void OnTriggerStay2D(Collider2D colObj){
		if(colObj.tag == "Player"){
			if(Input.GetKey (KeyCode.E) && Game.skin1 == false && (Game.lifetimeLoot - price) >= 0) {
			if(Input.GetKey (KeyCode.E) && !PlayerPrefsBool.GetBool ("skin1") && (PlayerPrefs.GetInt ("lifetimeLoot") - price) >= 0) {
				text.enabled = false;
				Game.skin1 = true;
				PlayerPrefsBool.GetBool ("skin1", true);
				particles.Stop ();
				Game.lifetimeLoot -= price;
				PlayerPrefs.SetInt ("lifetimeLoot", (PlayerPrefs.GetInt ("lifetimeLoot") - price));
				arm.GetComponentInParent<SpriteRenderer>().sprite = gunArm;
				gameObject.GetComponentInParent<Shop>().player.GetComponent<Animator>().runtimeAnimatorController = gameObject.GetComponentInParent<Shop>().skin;
			}
			if(Input.GetKey (KeyCode.E) && Game.skin1 == true) {
			if(Input.GetKey (KeyCode.E) && !PlayerPrefsBool.GetBool ("skin1")) {
				arm.GetComponentInParent<SpriteRenderer>().sprite = gunArm;
				gameObject.GetComponentInParent<Shop>().player.GetComponent<Animator>().runtimeAnimatorController = gameObject.GetComponentInParent<Shop>().skin;
			}
		}
	}

}
