using UnityEngine;
using System.Collections;

public class GameScores
{
	public static int score = 0;
	public static int selectedTheme = 0;
	public static int gamePlayed = 0;
	public static int compendium;
	private static string keyCurrentScore = "ysy734nfhdndjslgc89yyhagvdfxg_jsjkkjs";
	private static string keyMaxScore = "kwy234nfhdjdjsjgu84tyhfgjdffg_jsjkghkjs";
	private static string keyCompendium = "dgkjhkdfjbbrwe7tr7erbbx7542bvjxcucbugd";
	public static int maxScore = 0;

	public static int compendiumCount = 0;

    public static void AddScore()
	{
		SPlayerPrefs.SetInt(keyCurrentScore,SPlayerPrefs.GetInt(keyCurrentScore) + 1);
		//Debug.Log(SPlayerPrefs.GetInt(keyCurrentScore));
	}

	public static void UpdateMaxScore()
	{
		if(SPlayerPrefs.GetInt(keyCurrentScore) > SPlayerPrefs.GetInt(keyMaxScore))
		{
			SPlayerPrefs.SetInt(keyMaxScore,SPlayerPrefs.GetInt(keyCurrentScore));
		}
	}

}
