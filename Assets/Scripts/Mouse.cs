using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using DG.Tweening;

public class Mouse : MonoBehaviour {

	private bool isMoving = false;
	private bool yPlus = false; 

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

		/*
		if( yPlus ) {
			transform.position += new Vector3(2f*Time.deltaTime, 0f, 0f);
			if( transform.position.x >= 3 )
				yPlus = false;
		} else {
			transform.position -= new Vector3(2f*Time.deltaTime, 0f, 0f);
			if( transform.position.x <= -3 )
				yPlus = true;
		}
		*/

	}

	public void Shake()
	{
		//transform.DOShakePosition (2f);
		//transform.DOJump (new Vector3 (-3f, 0, 0), 3, 3, 3f).SetEase (Ease.Linear);
		//transform.DOJump (new Vector3 (0, -1f, 0), 2, 1, 1f, false);

		if (isMoving == false) {
			isMoving = true;

			transform.DOJump (
				new Vector3 (0, 0),   // 移動終了地点
				1,                     // ジャンプする力
				2,                     // 移動終了までにジャンプする回数
				1f                      // アニメーション時間
			).SetRelative ().SetEase (Ease.Linear).OnComplete (callback);
		}
		//RectTransform rt = this.gameObject.GetComponent<RectTransform> ();
		//rt.DOJump (new Vector3 (-3f, 0, 0), 3, 3, 3f).SetEase (Ease.Linear);
	}

	private void callback()
	{
		isMoving = false;
	}

	void OnCollisionEnter2D(Collision2D coll)
	{
		Destroy(coll.gameObject);

		GameObject gmObj = GameObject.Find ("GameManager");
		GameManager gm = (GameManager)gmObj.GetComponent<GameManager> ();
		gm.AddScore ();

		// Tween
		this.Shake ();

	}

}
