using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

	// Outlets
	public GameObject riceBallPrefab;
	public GameObject mousePrefab;
	public Camera myCamera;
	public GameObject Tile;

	// Outlets for UI
	public Text textLife;
	public Text textTime;
	public Text textMessage;
	public Text labelGameOver;
	public Button buttonRestart;

	// Public Properties
	public int initialLife = 3;

	//private int fireForce = 0;
	private int Life;
	private int Score;
	private float ErapsedTime;

	private GameObject riceBallInstance;
	private GameState gameState;
	private AudioSource mainBGM;

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
		mainBGM = GetComponent<AudioSource> ();

		this.labelGameOver.enabled = false;
		this.textMessage.enabled = false;
		this.buttonRestart.gameObject.SetActive(false);
	}


	private bool isDemoCameraDown = true;

	// Update is called once per frame
	void Update () {

		//print (gameState);

		switch (gameState) {

		case GameState.GameInit:
			GameInit ();
			break;

		case GameState.StageInit:
			StageInit ();
			break;

		case GameState.Demo:

			if (isDemoCameraDown) {
				myCamera.transform.position += new Vector3(0f, -4f * Time.deltaTime, 0f);
					if(myCamera.transform.position.y < -10.5)
					{
						isDemoCameraDown = false;
					}
				}
			else
			{
				myCamera.transform.position += new Vector3(0f, 4f * Time.deltaTime, 0f);
				if(myCamera.transform.position.y > 0)
				{
					gameState = GameState.Start;
				}
			}
			break;

		case GameState.Start:

			if (this.Life > 0) {
				this.Life--;
				this.textLife.text = this.Life.ToString ();

				GameObject riceBall = Instantiate (riceBallPrefab, new Vector3 (-1.8f, 4.2f, 0), Quaternion.identity);

				this.riceBallInstance = riceBall;	//カメラ追跡用

				offset = myCamera.transform.position - riceBall.transform.position;

				//Rigidbody2D rb = riceBall.GetComponent<Rigidbody2D> ();
				//rb.AddForce (new Vector2 (30, 25));

				this.gameState = GameState.Playing;
			}
			else
			{
				this.gameState = GameState.Over;
			}

			break;

		case GameState.Playing:

			ErapsedTime += Time.deltaTime;
			this.textTime.text = ErapsedTime.ToString ("F0");


			TouchInfo info = AppUtil.GetTouch ();
			if (info == TouchInfo.Began) {
				Vector3 tPos = AppUtil.GetTouchWorldPosition (myCamera);
				Collider2D col = Physics2D.OverlapPoint (new Vector2 (tPos.x, tPos.y));
				if (col) {
					if (col.gameObject.tag == "RiceBall") {
						Rigidbody2D rb = col.GetComponent<Rigidbody2D> ();
						rb.AddForce (new Vector2 (0, 100));
					}
				}
			}
			break;

		case GameState.Over:
			this.labelGameOver.enabled = true;
			this.textMessage.enabled = true;
			this.textMessage.text = string.Format ("{0:d}かいせいこう\nかかったじかん{1:f0}びょう", Score, ErapsedTime);
			this.buttonRestart.gameObject.SetActive (true);

			this.mainBGM.Stop ();

			break;

		default:
			break;
		
		}
	}

	Vector3 offset = Vector3.zero;

	void LateUpdate () {

		// カメラ追跡処理
		if (this.gameState == GameState.Playing && myCamera.transform.position.y > -10.5) {
			Vector3 newPosition = myCamera.transform.position;
			newPosition.y = this.riceBallInstance.transform.position.y + offset.y;

			if (newPosition.y > 0) {
				newPosition.y = 0;
			}

			//myCamera.transform.position = newPosition;
			myCamera.transform.position = Vector3.Lerp (myCamera.transform.position, newPosition, 5.0f * Time.deltaTime);
		}
	
	}

	// ゲーム全体の開始の処理
	private void GameInit()
	{
		this.Life = this.initialLife;
		this.Score = 0;
		this.ErapsedTime = 0;

		this.textLife.text = Life.ToString ();
		this.textTime.text = ErapsedTime.ToString ("f0");

		this.labelGameOver.enabled = false;
		this.textMessage.enabled = false;
		this.buttonRestart.gameObject.SetActive(false);

		// おにぎりが存在したら削除
		GameObject[] gos = GameObject.FindGameObjectsWithTag ("RiceBall");
		foreach (GameObject go in gos) {
			Destroy (go);
		}


		this.gameState = GameState.StageInit;

		for (int i = -3; i < 4; i++) {
			for (int j = 6; j > -30; j--) {
				Instantiate (Tile, new Vector3 (i, j, 0), Quaternion.identity);
			}
		}

		this.mainBGM.Play ();
	}

	// ステージ（ねずみ・障害物）生成時の処理
	private void StageInit()
	{
		if (this.Life <= 0) {
			this.gameState = GameState.Over;
			return;
		}

		//カメラリセット
		this.myCamera.transform.position = new Vector3(0,0,-10);

		// ねずみ削除・作成
		GameObject[] gos = GameObject.FindGameObjectsWithTag ("Mouse");
		foreach (GameObject go in gos) {
			Destroy (go);
		}

		Instantiate(mousePrefab,new Vector3 (Random.Range(-2.6f,2.6f),-14, 0), Quaternion.identity);
	
		// ステージデモに遷移
		this.isDemoCameraDown = true;
		this.gameState = GameState.Demo;
	}
		
	public void AddScore()
	{
		Score++;
	}

	public void ChangeGameState(GameState state)
	{
		this.gameState = state;		
	}
		
}
