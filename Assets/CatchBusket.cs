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

		GameObject gmObj = GameObject.Find ("GameManager");
		GameManager gm = (GameManager)gmObj.GetComponent<GameManager> ();
		gm.AddScore ();

		// Tween
		GameObject mouseObj = GameObject.Find ("pMouse");
		Mouse mouse = mouseObj.GetComponent<Mouse>();
		mouse.Shake ();

	}
}
