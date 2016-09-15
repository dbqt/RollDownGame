using UnityEngine;
using UnityEngine.UI;
using System.Collections;

using UnityEngine.Advertisements;
public class UIManager : MonoBehaviour {

	public Text Title, HighScore, Score, ControlText, ShopText, AdsText, Credits;
	public Button Play, Control, Ads, Back, ButtonShop;
	public GameObject Shop;

	public Text[] ShopButtons;

	private string[] baseColors, prices;

	void Start() {
		Menu();
		baseColors = new string[8];
		prices = new string[8];
		baseColors[0] = "Red"; prices[0] = "Free";
		baseColors[1] = "Blue"; prices[1] = "Free";
		baseColors[2] = "Green"; prices[2] = "100";
		baseColors[3] = "Yellow"; prices[3] = "100";
		baseColors[4] = "Black"; prices[4] = "200";
		baseColors[5] = "Gray"; prices[5] = "200";
		baseColors[6] = "Rainbow"; prices[6] = "1000";
		baseColors[7] = "Hearts"; prices[7] = "1000";

	}


	// Update is called once per frame
	void Update () {
		Score.text = "Points : "+PlayerPrefs.GetInt("CurrentScore");
		ShopText.text = "Total points: "+PlayerPrefs.GetInt("TotalPoints");
		
		System.DateTime lastTime;
		System.DateTime.TryParse(PlayerPrefs.GetString("Date"), out lastTime);
		System.TimeSpan diff = (lastTime - System.DateTime.Now);
		if(diff.Seconds > 0)
		{
			Ads.interactable = false;
			AdsText.text = "Come back in "+ string.Format("{0:D2}:{1:D2}:{2:D2}", diff.Hours, diff.Minutes, diff.Seconds);
		}
		else
		{
			Ads.interactable = true;
			if(Advertisement.IsReady("rewardedVideo")){
				AdsText.text = "Watch video for 100 points";
			}
			else {
				AdsText.text = "Failed to load video";
			}
		}

		if(PlayerPrefs.GetInt("Control") == 0) ControlText.text = "Touch";
		if(PlayerPrefs.GetInt("Control") == 1) ControlText.text = "Tilt";
		if(PlayerPrefs.GetInt("Control") == -1) ControlText.text = "GamePad";

	}

	public void Menu() {
		HighScore.text = "Best: "+PlayerPrefs.GetInt("BestScore")+" \nTotal points: "+PlayerPrefs.GetInt("TotalPoints");

		Title.enabled = true;
		Credits.enabled = true;
		Play.gameObject.SetActive(true);
		Control.gameObject.SetActive(true);
		HighScore.enabled = true;
		ButtonShop.gameObject.SetActive(true);

		Score.enabled = true;
		ShopText.enabled = false;
		Shop.SetActive(false);
		Back.gameObject.SetActive(false);
		Ads.gameObject.SetActive(false);
	}

	public void PlayGame() {
		Title.enabled = false;
		Credits.enabled = false;
		Play.gameObject.SetActive(false);
		Control.gameObject.SetActive(false);
		HighScore.enabled = false;
		ButtonShop.gameObject.SetActive(false);
		
		Score.enabled = true;
		ShopText.enabled = false;
		Shop.SetActive(false);
		Back.gameObject.SetActive(false);
		Ads.gameObject.SetActive(false);
	}

	public void GoShop() {
		Title.enabled = false;
		Credits.enabled = false;
		Play.gameObject.SetActive(false);
		Control.gameObject.SetActive(false);
		HighScore.enabled = false;
		ButtonShop.gameObject.SetActive(false);
		
		Score.enabled = false;
		ShopText.enabled = true;
		
		Shop.SetActive(true);
		Back.gameObject.SetActive(true);
		Ads.gameObject.SetActive(true);
		SetShopButtons();
	}

	public void ToggleControl() {
		int type = PlayerPrefs.GetInt("Control");
		if(type == 0) PlayerPrefs.SetInt("Control", 1);
		if(type == 1) PlayerPrefs.SetInt("Control", -1);
		if(type == -1) PlayerPrefs.SetInt("Control", -0);

	}

	public void SetShopButtons(){

		for(int i = 0; i < ShopButtons.Length; ++i){
			
			ShopButtons[i].text = baseColors[i];

			if(PlayerPrefs.GetInt("mat"+i) == 0)
			{
				ShopButtons[i].text += " - "+prices[i];
			}
			
		}

		int active = PlayerPrefs.GetInt("ActiveMaterial");
		ShopButtons[active].text = baseColors[active]+" - Active";
	}
}
