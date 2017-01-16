using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

	public GameObject prefab;
	public Camera camera;

	// Use this for initialization
	void Start () {
		
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
		if (info == TouchInfo.Began)
		{
			// タッチ開始
			//Vector3 tPos = AppUtil.GetTouchPosition();
			Vector3 tPos = AppUtil.GetTouchWorldPosition(camera);

			Instantiate(prefab, new Vector3(tPos.x,tPos.y,0), Quaternion.identity);
		}
	}
}
