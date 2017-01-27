using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using DG.Tweening;

public class Mouse : MonoBehaviour {

	private bool isMoving = false;	// Tweenの二重起動防止用
	private AudioSource soundJump;

	void Start () {
		soundJump = GetComponent<AudioSource> ();
	}
	
	void Update () {
	}

	public void Shake()
	{
		if (isMoving == false) {
			isMoving = true;

			transform.DOJump (
				new Vector3 (0, 0),   // 移動終了地点
				1,                     // ジャンプする力
				3,                     // 移動終了までにジャンプする回数
				1.5f                      // アニメーション時間
			).SetRelative ().SetEase (Ease.Linear).OnComplete (callback);
		}
	}

	private void callback()
	{
		isMoving = false;
	}

	void OnCollisionEnter2D(Collision2D coll)
	{
		if (coll.gameObject.tag == "RiceBall") {
			//Destroy (coll.gameObject);

			GameObject gmObj = GameObject.Find ("GameManager");
			GameManager gm = (GameManager)gmObj.GetComponent<GameManager> ();

			gm.AddScore ();
			gm.setMessageText ("やったね！");

			// Tween
			this.Shake ();
			StartCoroutine (JumpSoundPlay());

			gm.RiceLostAction ();
		}
	}

	IEnumerator JumpSoundPlay(){
		soundJump.Play ();
		yield return new WaitForSeconds (0.5f);
		soundJump.Play ();
		yield return new WaitForSeconds (0.5f);
		soundJump.Play ();
		yield return 0;
	}
}
