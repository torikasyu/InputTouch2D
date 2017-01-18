using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

	// Outlets
	public GameObject prefab;
	public Camera myCamera;
	public Text textLife;

	// Public Properties
	public int Life = 10;
	public int Score = 0;

	// Use this for initialization
	void Start () {
	
		//this.textLife.text = Life.ToString ();

	}

	
	// Update is called once per frame
	void Update () {

		if(Input.touchCount > 0)
		{
			/*
			Touch touch = Input.GetTouch(0);
			if(touch.phase == TouchPhase.Began)
			{
				Instantiate(prefab, new Vector3(-1, -1, 0), Quaternion.identity);
			}
			*/
		}

		TouchInfo info = AppUtil.GetTouch();
		if (info == TouchInfo.Began) {
			if (this.Life > 0) {
				// タッチ開始
				Vector3 tPos = AppUtil.GetTouchWorldPosition (myCamera);

				Instantiate (prefab, new Vector3 (tPos.x, tPos.y, 0), Quaternion.identity);

				if (tPos.y > 4) {
					//Instantiate (prefab, new Vector3 (tPos.x, tPos.y, 0), Quaternion.identity);

					//Life--;
					//this.textLife.text = this.Life.ToString ();
				}
			}
		}
	}

	public void AddScore()
	{
		Score++;
		this.textLife.text = Score.ToString();
	}
		
}
