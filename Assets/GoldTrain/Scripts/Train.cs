using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using GoogleMobileAds.Api;

public class Train : MonoBehaviour {

	public InterstitialAd interstitial;
	public bool adClosed = true;

	public GameObject Money;
	public GameObject Coal;
	public GameObject EndPanel;
	public bool ended = false;

	public AudioClip cash;
	public AudioClip coal;
	public AudioClip crash;

	public string highscore;
	// Use this for initialization
	void Start () {
		
		#if UNITY_ANDROID
		string appId = "ca-app-pub-6234576313597738~4571733855";
		string adUnitId = "ca-app-pub-6234576313597738/2683937112";
		#elif UNITY_IOS
		string appId = ""
		string adUnitId = "";
		#endif

		MobileAds.Initialize (appId);
		this.interstitial = new InterstitialAd (adUnitId);
		AdRequest request = new AdRequest.Builder ().Build ();
		this.interstitial.LoadAd (request);
		this.interstitial.OnAdClosed += setAdClosed;


		int currentHighScore = 0;
		if (PlayerPrefs.HasKey ("TrainHighScore")) {
			currentHighScore = PlayerPrefs.GetInt ("TrainHighScore");
		}
		GameObject.Find ("HighScore").GetComponent<Text> ().text = "HIGH SCORE: " + currentHighScore;

	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void EndGame (){
		
		if(ended==false){


			
			GameObject leadO = GameObject.Find ("Lead");
			Lead lead = leadO.GetComponent<Lead> ();
			playSound (2);
			lead.activateDestroy ();
			lead.power = 0;
			int score = lead.score;
			if(score > PlayerPrefs.GetInt("TrainHighScore")){
				PlayerPrefs.SetInt("TrainHighScore", score);
			}
			Canvas can = FindObjectOfType (typeof(Canvas)) as Canvas;
			ended = true;
			lead.Speed = 0f;
			GameObject endpan = Instantiate (EndPanel) as GameObject;
			endpan.transform.SetParent (can.transform, false);
			endpan.GetComponentInChildren<Text> ().text = " x " + score;

			if (this.interstitial.IsLoaded ()) {
				this.adClosed = false;
				this.interstitial.Show ();
			}
		}
	}

	public void setAdClosed(object sender, System.EventArgs args) {
		this.adClosed = true;
	}

	public void CreateMoney(){
		float Randx = Random.Range (-6f, 18f);
		float Randy = Random.Range (-3.5f, 9f);
		GameObject money = Instantiate (Money) as GameObject;
		money.transform.position = new Vector3 (Randx, Randy, 0);
	}

	public void CreateCoal(){
		float Randx = Random.Range (-6f, 18f);
		float Randy = Random.Range (-3.5f, 9f);
		GameObject coal = Instantiate (Coal) as GameObject;
		coal.transform.position = new Vector3 (Randx, Randy, 0);
	}

	public void reLoadGame() {
		SceneManager.LoadScene ("GoldTrain");
	}

	public void playSound(int sound) {
		AudioSource audio = gameObject.GetComponent<AudioSource> ();
		switch (sound) {
		case 0:
			audio.clip = cash;
			break;
		case 1:
			audio.clip = coal;
			break;
		case 2:
			audio.clip = crash;
			break;
		default:
			audio.clip = coal;
			break;
		}
		audio.Play ();
	}

}
