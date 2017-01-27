using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RiceBall : MonoBehaviour {


	private AudioSource pongSound;

	// Use this for initialization
	void Start () {
		pongSound = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {

		if (this.gameObject.transform.position.y < -20) {
			
			GameObject gmObj = GameObject.Find ("GameManager");
			GameManager gm = (GameManager)gmObj.GetComponent<GameManager> ();

			gm.setMessageText ("ざんねん");
			gm.RiceLostAction ();
		}
	}

	void OnCollisionEnter2D(Collision2D coll)
	{
		pongSound.Play ();
	}

}
