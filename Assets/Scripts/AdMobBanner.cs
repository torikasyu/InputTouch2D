using UnityEngine;
using System.Collections;
using GoogleMobileAds.Api;

public class AdMobBanner : MonoBehaviour {

	public string Android_Banner;
	public string ios_Banner;

	public BannerView bannerView;
	AdRequest request;

	// Use this for initialization
	void Start () {
	}

	void Awake(){
		DontDestroyOnLoad (this);
		// バナー広告を表示
		RequestBanner ();
	}

	// Update is called once per frame
	void Update () {

	}

	void OnDestroy() {
		bannerView.Destroy();
	}

	private void RequestBanner()
	{
		#if UNITY_ANDROID
		string adUnitId = Android_Banner;
		#elif UNITY_IPHONE
		string adUnitId = ios_Banner;
		#else
		string adUnitId = "unexpected_platform";
		#endif

		// Create a 320x50 banner at the top of the screen.
		bannerView = new BannerView (adUnitId, AdSize.Banner, AdPosition.Bottom);

		// Create an empty ad request.
		//request = new AdRequest.Builder ().AddTestDevice("").Build ();
		request = new AdRequest.Builder ().Build ();

		// Load the banner with the request.
		bannerView.LoadAd (request);
	}
}