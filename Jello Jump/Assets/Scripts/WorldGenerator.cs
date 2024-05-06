using UnityEngine;
using System.Collections;
using System.Timers;

[System.Serializable]
public class SimplePlatform
{
	public GameObject platformObject;

	[Header("Translate Speed Settings")]

	public float minSpeed;
	public float maxSpeed;
}

[System.Serializable]
public class AdvancePlatform
{
	public GameObject platformObject;
	
	[Header("Translate Speed Settings")]
	
	public float t_minSpeed;
	public float t_maxSpeed;

	[Header("Rotate Speed Settings")]
	
	public float r_minSpeed;
	public float r_maxSpeed;
}

public class WorldGenerator : MonoBehaviour 
{
	[Header("SimplePlatformSetUp")]
	public SimplePlatform simplePlatform;

	[Header("AdvancePlatformSetUp")]
	public AdvancePlatform advancePlatform;

	[Header("MainParameters")]
	public PlatformTemp platformTemp;
	public TileTemp tileTemp;
	public GameObject lootObj;
	public Platform currentPlatform;

	public float minSpawnInterval;
	public float maxSpawnInterval;
	[HideInInspector]
	public bool reload = false;
	[HideInInspector]
    public float randomizedSpawnIntervalValue;
	[HideInInspector]
	public float m_intervalTempTime;

	void Update()
	{
		if(reload == false)
		{
			randomizedSpawnIntervalValue = Random.Range(minSpawnInterval,maxSpawnInterval);

			m_intervalTempTime +=Time.unscaledDeltaTime;
			
			if(m_intervalTempTime > randomizedSpawnIntervalValue)
			{
				//Do Spawn
				Random.seed = (int)System.DateTime.Now.Ticks;
				int dice = Random.Range(1,5);

				if(dice >= 3)
				{
					SpawnAdvancePlatform();
				}
				if(dice >= 1 && dice<3)
				{
					SpawnSimplePlatform();
				}
				
				m_intervalTempTime = 0;
				m_intervalTempTime = 0;
				reload = true;
			}
	
		}

		if(platformTemp.platformLocks)
		{
			tileTemp.move = true;
			platformTemp.move = true;
			platformTemp.platformLocks = false;
		}
	}


	

	void SpawnSimplePlatform()
	{
		GameObject _platform = Instantiate(simplePlatform.platformObject,new Vector3(0,0,0),Quaternion.identity) as GameObject;
		platformTemp.Add(_platform);

		Platform platform = _platform.GetComponent<Platform>();
		currentPlatform = platform;
		platform.loot = lootObj;
		platform.speed = Random.Range(simplePlatform.minSpeed,simplePlatform.maxSpeed);
		platform.myGenerator = this;
		platform.jelly = this.GetComponent<GameManager>().myJelly;

	}

	void SpawnAdvancePlatform()
	{
		GameObject _platform = Instantiate(advancePlatform.platformObject,new Vector3(0,0,0),Quaternion.identity) as GameObject;
		platformTemp.Add(_platform);

		Platform platform = _platform.GetComponent<Platform>();
		currentPlatform = platform;
		platform.speed = Random.Range(advancePlatform.t_minSpeed,advancePlatform.t_maxSpeed);

		platform.loot = lootObj;
		platform.rotationSpeed = Random.Range(advancePlatform.r_minSpeed,advancePlatform.r_maxSpeed);
		platform.myGenerator = this;
		platform.jelly = this.GetComponent<GameManager>().myJelly;

	}

}
