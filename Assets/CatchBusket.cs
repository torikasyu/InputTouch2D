using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatchBusket : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}

	// Update is called once per frame
	void Update () {

	}

	void OnCollisionEnter2D(Collision2D coll)
	{
		Destroy(coll.gameObject);

		GameObject go = GameObject.Find ("GameManager");
		GameManager gm = (GameManager)go.GetComponent<GameManager> ();
		gm.AddScore ();
	}
}
