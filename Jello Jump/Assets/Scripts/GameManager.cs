using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using GooglePlayGames;
using UnityEngine.SocialPlatforms;
using GooglePlayGames.BasicApi;

using UnityEngine.Advertisements;


[System.Serializable]
public class UIPanels
{
	public CanvasGroup menu;
	public bool showMenu = false;
	public bool closeMenu;

	public CanvasGroup game;
	public bool showGame = false;
	public bool closeGame;

	public CanvasGroup endGame;
	public bool showEndGame = false;
	public bool closeEndGame = false;

	public CanvasGroup changeTheme;
	public bool showChangeTheme;
	public bool closeChangeTheme;

	public GameObject buyButton;

	public Text tapToStart;

}

[System.Serializable]
public class UIElements
{
	public Text scoreOutput;
	public Text finalScore;
	public Text highScore;

	public Text compendiumCount;
	public Button no_ads;
}

public class GameManager : MonoBehaviour
{
	public JellyControl myJelly;
	public WorldGenerator generator;
	public JellySelector jellies;

	[HideInInspector]
	public bool gameStart = false;
	[HideInInspector]
	public bool playerLoose = false;

	public UIPanels panels;
	public UIElements elements;

	List<float> timers = new List<float>(){0,0,0,0,0,0};

	public List<AudioSource> clips;

	[Header("Sound")]
	public Sprite soundOn;
	public Sprite soundOff;
	public Image soundIcon;
	[Space(10)]

	public Sprite melodyOn;
	public Sprite melodyOff;
	public Image melodyIcon;

	public static bool restart = false;

	public void Awake ()
	{

		CheckSKU();

		Social.localUser.Authenticate((bool success) => {
			if (success) {
				Debug.Log("You've successfully logged in");
			} else {
				Debug.Log("Login failed for some reason");
			}
		});
		
		PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder()
			
			.Build();
		
		PlayGamesPlatform.InitializeInstance(config);
		// recommended for debugging:
		PlayGamesPlatform.DebugLogEnabled = true;
		// Activate the Google Play Games platform
		PlayGamesPlatform.Activate();
		
	}

	void CheckSKU()
	{
		if(SPlayerPrefs.GetString("jgdfkjgbbsdfusdhufhfksfggrwebnasksdfsd") == "SUCCED")
		{
			print("REMOVEBANNER");
			elements.no_ads.interactable = false;
			this.SendMessage("RemoveBanner");
		}
		else
		{
			print("!REMOVEBANNER");
			this.SendMessage("ShowBanner");
		}
	}

	void Start()
	{
		UpdateCompendium();

		Social.localUser.Authenticate((bool success) =>
		                              {
			if (success)
			{
				Debug.Log("You've successfully logged in");
			}
			else
			{
				Debug.Log("Login failed for some reason");
			}
		});
	}

	public void UICallback(string value)
	{
		if(value == "Restart")
		{
			PlayerPrefs.SetInt("Restart",1);
			Application.LoadLevelAsync(Application.loadedLevel);
		}

		if(value == "Sound")
		{
			if(AudioListener.volume == 1)
			{
				soundIcon.sprite = soundOff;
		    	AudioListener.volume = 0;
			}
			else
			{
				soundIcon.sprite = soundOn;
				AudioListener.volume = 1;
			}
		}

		if(value == "RateApp")
		{

		}

		if(value == "Leaderboards")
		{
			PlayGamesPlatform.Instance.ShowLeaderboardUI("CgkIgbX_npMUEAIQAA");
		}

		if(value == "OpenChangeThemeTab")
		{
			panels.showChangeTheme = true;
			panels.closeChangeTheme = false;
		}

		if(value == "CloseChangeThemeTab")
		{
			panels.showChangeTheme = false;
			panels.closeChangeTheme = true;
		}

		if(value == "Refund")
		{
			Application.OpenURL("https://support.google.com/googleplay/contact/play_request_refund_apps?rd=2&rd=1&ctx=problems_with_inapp_purchases");
		}

		if(value == "Home")
		{
			PlayerPrefs.SetInt("SelectedThemeId",0);
			PlayerPrefs.SetInt("Restart",0);
			Application.LoadLevelAsync(Application.loadedLevel);
		}
	}

	public void ShowAds()
	{
		if (Advertisement.IsReady("rewardedVideo"))
		{
			var options = new ShowOptions { resultCallback = HandleShowResult };
			Advertisement.Show("rewardedVideo", options);
		}
	}

	private void HandleShowResult(ShowResult result)
	{
		switch (result)
		{
		case ShowResult.Finished:
			Debug.Log("The ad was successfully shown.");
			SPlayerPrefs.SetInt("dgkjhkdfjbbrwe7tr7erbbx7542bvjxcucbugd",SPlayerPrefs.GetInt("dgkjhkdfjbbrwe7tr7erbbx7542bvjxcucbugd") + 50);
			UpdateCompendium();
			clips[4].Play();
			//
			// YOUR CODE TO REWARD THE GAMER
			// Give coins etc.
			break;
		case ShowResult.Skipped:
			Debug.Log("The ad was skipped before reaching the end.");
			break;
		case ShowResult.Failed:
			Debug.LogError("The ad failed to be shown.");
			break;
		}
	}
	void UpdateScore()
	{
		elements.scoreOutput.SendMessage("DoTween");
		elements.scoreOutput.text = SPlayerPrefs.GetInt("ysy734nfhdndjslgc89yyhagvdfxg_jsjkkjs").ToString();
		this.SendMessage("MakeSquarePngFromOurVirtualThingy");
	}

	public void UpdateCompendium()
	{
		elements.compendiumCount.text = SPlayerPrefs.GetInt("dgkjhkdfjbbrwe7tr7erbbx7542bvjxcucbugd").ToString();
	}


	void StartGame(bool value)
	{
		print(value);

		if(jellies.selectedObject.GetComponent<JellyControl>())
		{
			myJelly = jellies.selectedObject.GetComponent<JellyControl>();

		panels.closeMenu = true;
		panels.showGame = true;
        myJelly.canJump = true;

		gameStart = true;
		generator.enabled = true;
		}
	}

	public void Jump()
	{
		if(!panels.buyButton.activeSelf && !playerLoose)
		{
			if(!gameStart)
			{
				StartGame(false);
			}
			else
			{
				myJelly.Jump();
			}
		}
	}

	void Update()
	{

		if(panels.showGame)
		{
			timers[0]+= Time.deltaTime;

			panels.game.alpha = Mathf.Lerp(0,1,timers[0]);

			if(panels.game.alpha == 1)
			{
				timers[0] = 0;
				panels.showGame = false;
			}
		}

		if(panels.closeGame)
		{
			timers[0]+=Time.deltaTime;
			panels.game.alpha = Mathf.Lerp(1,0,timers[0]);
			
			if(panels.game.alpha == 0)
			{
				timers[0] = 0;
				panels.closeGame = false;
			}
		}

		if(panels.showEndGame)
		{
			timers[1]+=Time.deltaTime;
			panels.endGame.alpha = Mathf.Lerp(0,1,timers[1]);
			panels.endGame.interactable = true;
			panels.endGame.blocksRaycasts = true;
			panels.endGame.ignoreParentGroups = false;

			if(panels.endGame.alpha == 1)
			{
				timers[1] = 0;
				panels.showEndGame = false;
			}
		}

		if(panels.closeEndGame)
		{
			timers[1]+=Time.deltaTime;
			panels.endGame.alpha = Mathf.Lerp(1,0,timers[1]);
			panels.endGame.interactable = false;
			panels.endGame.blocksRaycasts = false;
			panels.endGame.ignoreParentGroups = true;
			
			if(panels.endGame.alpha == 0)
			{
				timers[1] = 0;
				panels.closeEndGame = false;
			}
		}

		if(panels.showMenu)
		{
			timers[2]+=Time.deltaTime;
			panels.menu.alpha = Mathf.Lerp(0,1,timers[2]);
			panels.menu.interactable = true;
			panels.menu.blocksRaycasts = true;
			panels.menu.ignoreParentGroups = false;

			if(panels.menu.alpha == 1)
			{
				timers[2] = 0;
				panels.showMenu = false;
			}
		}
		if(panels.closeMenu)
		{
			timers[2]+=Time.deltaTime;
			panels.menu.alpha = Mathf.Lerp(1,0,timers[2]);
			panels.menu.interactable = false;
			panels.menu.blocksRaycasts = false;
			panels.menu.ignoreParentGroups = true;
			
			if(panels.menu.alpha == 0)
			{
				timers[2] = 0;
				panels.closeMenu = false;
			}
		}

		if(panels.showChangeTheme)
		{
			timers[3]+=Time.deltaTime;
			panels.changeTheme.alpha = Mathf.Lerp(0,1,timers[3]);
			panels.changeTheme.interactable = true;
			panels.changeTheme.blocksRaycasts = true;
			panels.changeTheme.ignoreParentGroups = false;

			if(panels.changeTheme.alpha == 1)
			{
				timers[3] = 0;
				panels.showChangeTheme = false;
			}
		}

		if(panels.closeChangeTheme)
		{
			timers[3]+=Time.deltaTime;

			panels.changeTheme.alpha = Mathf.Lerp(1,0,timers[3]);
			panels.changeTheme.interactable = false;
			panels.changeTheme.blocksRaycasts = false;
			panels.changeTheme.ignoreParentGroups = true;

			if(panels.changeTheme.alpha == 0)
			{
				timers[3] = 0;
				panels.closeChangeTheme = false;
			}
		}
		
	}

	public void ShowBuyButton()
	{
		panels.buyButton.SetActive(true);
		panels.tapToStart.gameObject.SetActive(false);
	}

	public void CloseBuyButton()
	{
		panels.buyButton.SetActive(false);
		panels.tapToStart.gameObject.SetActive(true);
	}

	private void Loose()
	{
		if(!playerLoose)
		{
			if(SPlayerPrefs.GetString("jgdfkjgbbsdfusdhufhfksfggrwebnasksdfsd") != "SUCCED")
			{
			GameScores.gamePlayed ++;
			if(GameScores.gamePlayed >= 5)
			{
				this.SendMessage("ShowInterstetial");
				GameScores.gamePlayed = 0;
			}
			}
			GameScores.UpdateMaxScore();

			playerLoose = true;
			myJelly.canJump = false;
			generator.enabled = false;

			panels.closeGame = true;
			panels.showEndGame = true;

			elements.finalScore.text = elements.scoreOutput.text;
			elements.highScore.text = SPlayerPrefs.GetInt("kwy234nfhdjdjsjgu84tyhfgjdffg_jsjkghkjs").ToString();

			if (Social.localUser.authenticated)
			{
				Social.ReportScore(SPlayerPrefs.GetInt("kwy234nfhdjdjsjgu84tyhfgjdffg_jsjkghkjs"), "CgkIgbX_npMUEAIQAA", (bool success) =>
				{
					if (success)
					{
						//((PlayGamesPlatform)Social.Active).ShowLeaderboardUI("CgkIgbX_npMUEAIQAA");
					}
					else
					{
						//Debug.Log("Login failed for some reason");
					}
				});
			}
			this.GetComponent<TestShare>().scorenum = SPlayerPrefs.GetInt("ysy734nfhdndjslgc89yyhagvdfxg_jsjkkjs");

			SPlayerPrefs.SetInt("ysy734nfhdndjslgc89yyhagvdfxg_jsjkkjs",0);
		}

	}

	void OnApplicationQuit() 
	{
		PlayerPrefs.SetInt("SelectedJellyId",0);
		PlayerPrefs.SetInt("Restart",0);
		PlayerPrefs.SetInt("SelectedThemeId",0);
	}


}
