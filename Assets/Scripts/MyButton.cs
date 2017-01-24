using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyButton : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	/// ボタンをクリックした時の処理
	public void OnClick() {
		Debug.Log("Button click!");

		GameObject gmObj = GameObject.Find ("GameManager");
		GameManager gm = (GameManager)gmObj.GetComponent<GameManager> ();
		gm.ChangeGameState (GameManager.GameState.GameInit);

	}
}
