using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using DG.Tweening;

public class Mouse : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void Shake()
	{
		//transform.DOShakePosition (2f);
		//transform.DOJump (new Vector3 (-3f, 0, 0), 3, 3, 3f).SetEase (Ease.Linear);
		//transform.DOJump (new Vector3 (0, -1f, 0), 2, 1, 1f, false);
		transform.DOJump(
			new Vector3(0, 0),   // 移動終了地点
			1,                     // ジャンプする力
			2,                     // 移動終了までにジャンプする回数
			1f                      // アニメーション時間
		).SetRelative ().SetEase (Ease.Linear);

		//RectTransform rt = this.gameObject.GetComponent<RectTransform> ();
		//rt.DOJump (new Vector3 (-3f, 0, 0), 3, 3, 3f).SetEase (Ease.Linear);
	}
}
