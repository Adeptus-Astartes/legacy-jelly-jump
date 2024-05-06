using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class Theme
{
	public string _names;
	public GameObject CameraSetup;
	public Color fogColor = Color.white;
	public Color ambientColor = Color.white;
	public Color jellyColor = Color.white;
	public GameObject jumpFX;
	public GameObject lootFX;
}

public class ThemeManager : MonoBehaviour 
{
	public GameManager manager;
	public List<Theme> themes;

	public static bool manual = false;

	void Start () 
	{
		foreach(Theme _theme in themes)
		{
			_theme.CameraSetup.SetActive(false);
		}
		Theme myTheme = new Theme();
		if(!manual)
		{
			myTheme = themes[Random.Range(0,themes.Count)];
		}
		else
		{
			myTheme = themes[GameScores.selectedTheme];
		}
		manager.elements.scoreOutput.color = myTheme.jellyColor;
		myTheme.CameraSetup.SetActive(true);
		this.GetComponent<TestShare>().screenShotCamera = myTheme.CameraSetup.GetComponent<Camera>();
		RenderSettings.fogColor = myTheme.fogColor;
		RenderSettings.ambientLight = myTheme.ambientColor;

		manager.jellies.UpdateTheme(myTheme);
		manager.generator.lootObj = myTheme.lootFX;

	}

	public void UpdateThemeId(int id)
	{
		GameScores.selectedTheme = id;
		manual = false;
		foreach(Theme _theme in themes)
		{
			_theme.CameraSetup.SetActive(false);
		}
		
		Theme myTheme = themes[id];
		manager.elements.scoreOutput.color = myTheme.jellyColor;
		myTheme.CameraSetup.SetActive(true);
		RenderSettings.fogColor = myTheme.fogColor;
		RenderSettings.ambientLight = myTheme.ambientColor;

		manager.panels.closeChangeTheme = true;
		//manager.SendMessage("StartGame",false);

		manager.jellies.UpdateTheme(myTheme);
		manager.generator.lootObj = myTheme.lootFX;
		
	}
}
