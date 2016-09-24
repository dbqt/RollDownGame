using UnityEngine;
using System;
using System.Collections;

//using UnityEngine.Advertisements;
using GoogleMobileAds.Api;

public class GameManager : MonoBehaviour {

	static public GameManager instance = null;

	public GameObject PlayerPrefab,
					  PlatformPrefab;

	public float Offset;
	public float UpSpeed;

	public UIManager ui;

	public int nbPlaysBeforeAds;

	public Camera cam;

	private Transform last;
	private float difficulty;
	private GameObject player;
	private bool isPlaying;

	private float upSpeedInternal;
	private int lastHolePosition;
	private RewardBasedVideoAd rewardBasedVideo;

	public float GetUpSpeed()
	{ return upSpeedInternal; }
	

	// Use this for initialization
	void Start () {
		PlayerPrefs.SetInt("CurrentScore", 0);
		if(instance != null) Destroy(this.gameObject);
		instance = this;
		isPlaying = false;
		ui.Menu();
		upSpeedInternal = 0f;

	
		RequestInterstitial();
		

		

		//PlayerPrefs.SetInt("TotalPoints", 0);
		//PlayerPrefs.SetInt("mat7", 0);
		//PlayerPrefs.SetInt("mat6", 1);
	}
	
	// Update is called once per frame
	void Update () {
	}

	public void StartGame() {
		Debug.Log("start game gm");
		cam.transform.position = new Vector3(0f,1f,-1f);
		ui.PlayGame();
		isPlaying = true;
		difficulty = 1f;
		PlayerPrefs.SetInt("CurrentScore", 0);
		upSpeedInternal = UpSpeed;

		for(int i = 0; i < 6; ++i) {
			GeneratePlatform(-i*Offset);
		}

		player = Instantiate(PlayerPrefab, this.transform.position, Quaternion.identity) as GameObject;
		
	}

	public void EndGame() {
		
		isPlaying = false;
		upSpeedInternal = 0f;
		Destroy(player);
		foreach(GameObject o in GameObject.FindGameObjectsWithTag("Platform")) {
			Destroy(o);
		}

		//update best score
		int best = PlayerPrefs.GetInt("BestScore");
		int current = PlayerPrefs.GetInt("CurrentScore");
		int total = PlayerPrefs.GetInt("TotalPoints") + current;
		PlayerPrefs.SetInt("TotalPoints", total);
		//Debug.Log("b "+best+", c "+current);
		if(best < current) PlayerPrefs.SetInt("BestScore", current);

		/*count++;
		if (count >= nbPlaysBeforeAds && Advertisement.IsReady())
	    {
	    	count = 0;
	    	Advertisement.Show();
	    }*/

		ui.Menu();
	}

	public void NextPlatform() {
		GeneratePlatform(last.position.y -2f);
	}

	private void GeneratePlatform(float height = -6f) {

		if(!isPlaying) return;

		GameObject n = Instantiate(PlatformPrefab, new Vector3(0f,height,0f) , Quaternion.identity) as GameObject;

		int mode = Mathf.FloorToInt(UnityEngine.Random.Range(0f, difficulty+1));
		switch(mode) {
			case 0: {
				// make sure the new hole isnt too far away
				int hole = lastHolePosition + Mathf.RoundToInt(UnityEngine.Random.Range(-3.49f, 3.49f));
				hole = Mathf.RoundToInt(Mathf.Clamp(hole,-0.49f, 8.5f));
				lastHolePosition = hole;
				n.GetComponent<PlatformReferences>().Pieces[hole].gameObject.SetActive(false);
				n.GetComponent<PlatformReferences>().Pieces[hole+1].gameObject.SetActive(false);
				n.GetComponent<PlatformReferences>().Pieces[hole+2].gameObject.SetActive(false);
				last = n.transform;
			}
			break;
			case 1: {
				int hole = Mathf.RoundToInt(UnityEngine.Random.Range(-0.49f, 4f));
				int hole2 = Mathf.RoundToInt(UnityEngine.Random.Range(5f, 9.5f));
				// choose randomely a hole as last hole
				lastHolePosition = (UnityEngine.Random.Range(-1f, 1f) > 0) ? hole : hole2;
				n.GetComponent<PlatformReferences>().Pieces[hole].gameObject.SetActive(false);
				n.GetComponent<PlatformReferences>().Pieces[hole+1].gameObject.SetActive(false);
				n.GetComponent<PlatformReferences>().Pieces[hole2].gameObject.SetActive(false);
				n.GetComponent<PlatformReferences>().Pieces[hole2+1].gameObject.SetActive(false);
				last = n.transform;
			}
			break;
			default:
			break;
		}

		upSpeedInternal *= 1.005f;
		Debug.Log("speed "+upSpeedInternal);
	}

	public void WatchAds() {
		Debug.Log("ads");
		/*if (Advertisement.IsReady("rewardedVideo"))
		{
			var options = new ShowOptions { resultCallback = HandleShowResult };
			Advertisement.Show("rewardedVideo", options);
		}
		else {		
			Debug.Log("fail ad");
		}*/

		if (rewardBasedVideo.IsLoaded()) {
    		rewardBasedVideo.Show();
  		} else {
  			Debug.Log("failed firebase ad");
  		}

		
		
	}

	/*private void HandleShowResult(ShowResult result)
	{
		switch (result)
		{
			case ShowResult.Finished:
				Debug.Log("The ad was successfully shown.");
				//
				// YOUR CODE TO REWARD THE GAMER
				// Give coins etc.
				int n = PlayerPrefs.GetInt("TotalPoints")+100;
				PlayerPrefs.SetInt("TotalPoints", n);
				PlayerPrefs.SetString("Date", System.DateTime.Now.AddHours(1).ToString());

			break;
			case ShowResult.Skipped:
				Debug.Log("The ad was skipped before reaching the end.");
			break;
			case ShowResult.Failed:
				Debug.LogError("The ad failed to be shown.");
			break;
		}
	}*/

	private void RequestInterstitial()
	{
	    #if UNITY_ANDROID
	        string adUnitId = "ca-app-pub-5954594840677307";
	    #else
	        string adUnitId = "unexpected_platform";
	    #endif

	    rewardBasedVideo = RewardBasedVideoAd.Instance;

	    AdRequest request = new AdRequest.Builder().Build();
	    rewardBasedVideo.LoadAd(request, adUnitId);
	    rewardBasedVideo.OnAdRewarded += HandleRewardBasedVideoRewarded;
	    rewardBasedVideo.OnAdLeavingApplication += HandleOnAdLeavingApplication;
	}

	public void HandleRewardBasedVideoRewarded(object sender, EventArgs args)
	{
	    Debug.Log("rewared event received.");
	    int n = PlayerPrefs.GetInt("TotalPoints")+100;
		PlayerPrefs.SetInt("TotalPoints", n);
		PlayerPrefs.SetString("Date", System.DateTime.Now.AddHours(1).ToString());
	    // Handle the ad loaded event.
	}

	public void HandleOnAdLeavingApplication(object sender, EventArgs args)
	{
	    Debug.Log("OnAdLoaded event received.");
	    //rewardBasedVideo.Destroy();
	    // Handle the ad loaded event.
	}
}
