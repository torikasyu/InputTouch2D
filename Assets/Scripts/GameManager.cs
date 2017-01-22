using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

	// Outlets
	public GameObject riceBallPrefab;
	public GameObject mousePrefab;
	public Camera myCamera;
	public Text textLife;

	// Public Properties
	public int Life = 10;
	public int Score = 0;
	//private int fireForce = 0;

	private GameState gameState;

	public enum GameState
	{
		GameInit,
		StageInit,
		Demo,
		Start,
		Playing,
		Over
	}

	// Use this for initialization
	void Start () {
		gameState = GameState.GameInit;
	}


	private bool isDemoCameraDown = true;

	// Update is called once per frame
	void Update () {


		switch (gameState) {

		case GameState.GameInit:
			GameInit ();
			break;

		case GameState.StageInit:
			StageInit ();
			break;

		case GameState.Demo:

			if (isDemoCameraDown) {
				myCamera.transform.position += new Vector3(0f, -2f*Time.deltaTime, 0f);
					if(myCamera.transform.position.y < -10)
					{
						isDemoCameraDown = false;
					}
				}
			else
			{
				myCamera.transform.position += new Vector3(0f, 2f*Time.deltaTime, 0f);
				if(myCamera.transform.position.y > 0)
				{
					gameState = GameState.Start;
				}
			}
			break;

		case GameState.Start:

			GameObject firstGo = Instantiate (riceBallPrefab, new Vector3 (-1.8f, 4.2f, 0), Quaternion.identity);

			this.riceBallPrefab = firstGo;
			offset = myCamera.transform.position - this.riceBallPrefab.transform.position;

			Rigidbody2D rb = firstGo.GetComponent<Rigidbody2D> ();
			rb.AddForce (new Vector2 (30, 25));

			this.gameState = GameState.Playing;

			break;

		case GameState.Playing:

			/*
			TouchInfo info = AppUtil.GetTouch ();

			if (info == TouchInfo.Began) {
				fireForce = 0;
			} else if (info == TouchInfo.Moved || info == TouchInfo.Stationary) {
				fireForce++;
			} else if (info == TouchInfo.Ended) {
				Vector3 tPos = AppUtil.GetTouchWorldPosition (myCamera);

				if (tPos.y > 4 && tPos.x < -2) {
					GameObject go = Instantiate (prefab, new Vector3 (tPos.x, tPos.y, 0), Quaternion.identity);
					Rigidbody2D rb = go.GetComponent<Rigidbody2D> ();
					rb.AddForce (new Vector2 ((25 + fireForce) * 3, (25 + fireForce) / 2 * 2));

					fireForce = 0;

					//Life--;
					//this.textLife.text = this.Life.ToString ();
				}
			}
			*/

			break;

		case GameState.Over:
			break;

		default:
			break;
		
		}
	}

	Vector3 offset = Vector3.zero;

	void LateUpdate () {

		// Camera Chase to RiceBall
		if (this.gameState == GameState.Playing && myCamera.transform.position.y > -10) {
			Vector3 newPosition = myCamera.transform.position;
			newPosition.y = this.riceBallPrefab.transform.position.y + offset.y;
			//myCamera.transform.position = newPosition;
			myCamera.transform.position = Vector3.Lerp (myCamera.transform.position, newPosition, 5.0f * Time.deltaTime);
		}
	
	}

	private void GameInit()
	{
		GameObject[] gos = GameObject.FindGameObjectsWithTag ("RiceBall");
		foreach (GameObject go in gos) {
			Destroy (go);
		}

		Score = 0;
		this.gameState = GameState.StageInit;
	}

	private void StageInit()
	{
		// mouse init

		GameObject mouse = Instantiate(mousePrefab,new Vector3 (Random.Range(-2.8f,2.8f),-14, 0), Quaternion.identity);

		this.gameState = GameState.Demo;
	}
		
	public void AddScore()
	{
		Score++;
		this.textLife.text = Score.ToString();
	}
		
}
