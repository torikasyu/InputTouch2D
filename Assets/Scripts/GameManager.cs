using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

	//Static instance of GameManager which allows it to be accessed by any other script.
	//public static GameManager instance = null;

	// Outlets for Game
	public GameObject riceBallPrefab;
	public GameObject mousePrefab;
	public Camera myCamera;
	public GameObject Tile;
	public GameObject[] SeeSaw;
	public GameObject[] SeeSawlarge;
	public GameObject SeeSawNormal;
	public GameObject SeeSawAlt;

	// Outlets for UI
	public Text textLife;
	public Text textTime;
	public Text textMessage;
	public Text textHiScore;
	public Button buttonRestart;
	public Image imageTutorial;

	// Public Properties
	public int initialLife = 3;
	public int initialTime = 90;

	private int Life;
	private int Score;
	private float RemainTime;

	private int HiScore;
	private int BestTime;

	private GameObject riceBallInstance;
	private GameObject mouseInstance;
	private AudioSource mainBGM;

	private GameState gameState;
	private DemoCameraState demoCameraState;

	public enum GameState
	{
		WaitForStart,	//最初とゲームオーバー後にこの状態になる
		GameInit,	//ゲーム初期化（１ゲーム毎に呼ばれる）
		StageInit,	//ステージ生成（おにぎり１個毎にこの状態になる）
		Demo,
		Start,
		Playing,
		Over
	}

	private enum DemoCameraState
	{
		Down,
		MouseJumpStart,
		Stop,
		Up
	}


	void Awake()
	{
		/*
		if (instance == null) {
			instance = this;
		} else if (instance != this) {
			Destroy (gameObject);	
		}
		DontDestroyOnLoad(gameObject);
		*/

		gameState = GameState.WaitForStart;
		demoCameraState = DemoCameraState.Down;
	}
		
	// Use this for initialization
	void Start () {
		mainBGM = GetComponent<AudioSource> ();

		this.textMessage.enabled = false;

		// ハイスコアの取得
		this.HiScore = PlayerPrefs.GetInt ("HiScore");
		this.BestTime = PlayerPrefs.GetInt ("BestTime");

		this.textHiScore.enabled = true;
		this.textHiScore.text = string.Format("さいこうきろく{0:d}こ\nのこり{1:d}びょう",HiScore,BestTime);

		this.Life = this.initialLife;
		this.Score = 0;
		this.RemainTime = this.initialTime;

		this.textLife.text = Life.ToString ();
		this.textTime.text = RemainTime.ToString ("f0");

		// 背景タイル
		for (int xPos = -3; xPos < 4; xPos++) {
			for (int yPos = 4; yPos > -30; yPos--) {
				Instantiate (Tile, new Vector3 (xPos, yPos, 0), Quaternion.identity);
			}
		}

	}
		
	// Update is called once per frame
	void Update () {

		print (gameState);

		switch (gameState) {

		case GameState.WaitForStart:
			break;

		case GameState.GameInit:
			GameInit ();
			break;
		
		case GameState.StageInit:
			StageInit ();
			break;

		case GameState.Demo:

			if (demoCameraState == DemoCameraState.Down) {
				myCamera.transform.position += new Vector3 (0f, -2.5f * Time.deltaTime, 0f);
				if (myCamera.transform.position.y < -10.5) {
					demoCameraState = DemoCameraState.MouseJumpStart;
				}
			} else if (demoCameraState == DemoCameraState.MouseJumpStart) {

				this.mouseInstance.GetComponent<Mouse> ().Shake ();
				demoCameraState = DemoCameraState.Stop;
			
			} else if (demoCameraState == DemoCameraState.Stop) {
				StartCoroutine(waitForDemoMouse());
			}
			else
			{
				myCamera.transform.position += new Vector3(0f, 4f * Time.deltaTime, 0f);
				if(myCamera.transform.position.y > 0)
				{
					gameState = GameState.Start;
					this.textHiScore.enabled = false;
					this.textMessage.text = "おむすびをころがして\nねずみさんまではこぼう";
				}
			}
			break;

		case GameState.Start:

			if (this.Life > 0) {
				this.Life--;
				this.textLife.text = this.Life.ToString ();

				//カメラ追跡用
				this.riceBallInstance = Instantiate (riceBallPrefab, new Vector3 (-1.8f, 4.5f, 0), Quaternion.identity);

				this.gameState = GameState.Playing;
			}
			else
			{
				this.gameState = GameState.Over;
			}

			break;

		case GameState.Playing:

			if (this.riceBallInstance != null) {
				offset = myCamera.transform.position - this.riceBallInstance.transform.position;
			}

			RemainTime -= Time.deltaTime;
			if (RemainTime < 0) {
				RemainTime = 0;
			}
			this.textTime.text = RemainTime.ToString ("F0");

			// 時間切れ
			if (RemainTime == 0) {
				this.gameState = GameState.Over;
				//this.riceBallInstance.gameObject.SetActive (false);
				Destroy(this.riceBallInstance.gameObject);

				break;
			}

			/*
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
			*/

			break;

		case GameState.Over:
			
			string message = "{0:d}かいせいこう\nのこり{1:f0}びょう";

			bool isHiScore = true;

			if (this.HiScore < Score) {
				message += "\nハイスコア！";			
			} else if (this.HiScore == Score && this.BestTime < RemainTime) {
				message += "\nベストタイム！";			
			} else {
				isHiScore = false;
			}
			this.textMessage.text = string.Format (message, Score, RemainTime);

			this.textHiScore.enabled = !isHiScore;
			this.buttonRestart.gameObject.SetActive (true);

			if (isHiScore) {
				PlayerPrefs.SetInt ("HiScore", Score);
				PlayerPrefs.SetInt ("BestTime", (int)RemainTime);
			}

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
			newPosition.y = myCamera.transform.position.y - offset.y;

			if (newPosition.y > 0) {
				newPosition.y = 0;
			}

			myCamera.transform.position = Vector3.Lerp (myCamera.transform.position, newPosition, 5.0f * Time.deltaTime);
		}
	}

	// ゲーム全体の開始の処理
	private void GameInit()
	{
		this.demoCameraState = DemoCameraState.Down;

		this.Life = this.initialLife;
		this.Score = 0;
		this.RemainTime = this.initialTime;

		this.textLife.text = Life.ToString ();
		this.textTime.text = RemainTime.ToString ("f0");

		this.textMessage.enabled = false;
		this.buttonRestart.gameObject.SetActive(true);
		this.imageTutorial.enabled = true;

		// ハイスコアの取得
		this.HiScore = PlayerPrefs.GetInt ("HiScore");
		this.BestTime = PlayerPrefs.GetInt ("BestTime");

		this.textHiScore.enabled = true;
		this.textHiScore.text = string.Format("さいこうきろく{0:d}こ\nのこり{1:d}びょう",HiScore,BestTime);
		this.textMessage.enabled = true;
		this.textMessage.text = "";

		// おにぎりが存在したら削除
		GameObject[] gos = GameObject.FindGameObjectsWithTag ("RiceBall");
		foreach (GameObject go in gos) {
			Destroy (go);
		}

		// シーソーを全てノーマルに
		int i = 0;
		foreach (GameObject tmp in this.SeeSaw) {
			Vector3 pos = tmp.transform.position;
			Destroy (tmp);
			this.SeeSaw[i] = Instantiate (this.SeeSawNormal, pos,Quaternion.identity);
			i++;
		}

		int j = 0;
		foreach (GameObject tmp in this.SeeSawlarge) {
			Vector3 pos = tmp.transform.position;
			Destroy (tmp);
			this.SeeSawlarge[j] = Instantiate (this.SeeSawNormal, pos,Quaternion.identity);
			this.SeeSawlarge [j].transform.localScale = new Vector3 (1, 1, 1);
			j++;
		}


		this.mainBGM.Play ();
	
		this.gameState = GameState.StageInit;
	}

	// ステージ（ねずみ・障害物）生成時の処理
	private void StageInit()
	{
		// タイトルUI非表示
		this.buttonRestart.gameObject.SetActive(false);
		this.imageTutorial.enabled = false;

		if (this.Life <= 0) {
			print ("Life is Zero");

			this.gameState = GameState.Over;
			return;
		}
			
		// ねずみ削除・作成
		GameObject[] gos = GameObject.FindGameObjectsWithTag ("Mouse");
		foreach (GameObject go in gos) {
			Destroy (go);
		}

		this.mouseInstance = Instantiate(mousePrefab,new Vector3 (Random.Range(-2.6f,2.6f),-14, 0), Quaternion.identity);

		//難易度アップ　シーソー小の置き換え
		if (this.Score > 0) {
			int i = 0;
			foreach (GameObject tmp in this.SeeSaw) {
				Vector3 pos = tmp.transform.position;
				int rand = Random.Range (0, 3);
				if (rand == 0) {
					Destroy (tmp);
					this.SeeSaw [i] = Instantiate (this.SeeSawAlt, pos, Quaternion.identity);
					this.SeeSaw [i].transform.localScale = new Vector3 (0.8f, 0.8f, 1);
				}
				i++;
			}
		}

		//難易度アップ　シーソー大の置き換え
		if (this.Score > 0) {
			int i = 0;
			foreach (GameObject tmp in this.SeeSawlarge) {
				Vector3 pos = tmp.transform.position;
				int rand = Random.Range (0, 2);
				if (rand == 0) {
					Destroy (tmp);
					this.SeeSawlarge [i] = Instantiate (this.SeeSawAlt, pos, Quaternion.identity);
					this.SeeSawAlt.transform.localScale = new Vector3 (1, 1, 1);
				}
				i++;
			}
		}
			
		this.textMessage.text = "ねずみさんはどこかな？";

		//カメラリセット
		this.gameState = GameState.Demo;
		this.demoCameraState = DemoCameraState.Down;
		this.myCamera.transform.position = new Vector3(0,0,-10);
		print ("camera y :" + this.myCamera.transform.position.y.ToString());
	}
		
	public void AddScore()
	{
		Score++;
	}

	public void ChangeGameState(GameState gs)
	{
		this.gameState = gs;
	}

	public void setMessageText(string msg)
	{
		this.textMessage.text = msg;
	}

	/*
	public void DestryRiceBall()
	{
		Destroy (this.riceBallInstance.gameObject);
		this.riceBallInstance = null;
	}
	*/

	public void RiceLostAction()
	{
		Destroy (this.riceBallInstance.gameObject);
		this.riceBallInstance = null;
	
		StartCoroutine (waitForNextStage ());
	}

	IEnumerator waitForNextStage()
	{	
		yield return new WaitForSeconds (1.5f);
		this.gameState = GameState.StageInit;
	}
		
	IEnumerator waitForDemoMouse()
	{
		yield return new WaitForSeconds (1.5f);
		demoCameraState = DemoCameraState.Up;
	}
}
