using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AccelGravity : MonoBehaviour {

	private Rigidbody2D rb;
	private float force = 180.0f; 

	// Use this for initialization
	void Start () {
		rb = this.gameObject.GetComponent<Rigidbody2D> ();
	}
	
	// Update is called once per frame
	void Update () {

		// 加速度与える
		Vector3 dir = Vector3.zero;
		dir.x = Input.acceleration.x;
		dir.y = 0;
		dir.z = 0;

		if (dir.sqrMagnitude > 1)
			dir.Normalize ();

		dir *= Time.deltaTime;
		rb.AddForce (dir * force);


	}

	void FixedUpdate () {
	}

}
