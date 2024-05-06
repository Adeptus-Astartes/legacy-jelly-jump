using UnityEngine;
using System.Collections;

public class Platform : MonoBehaviour 
{
	public WorldGenerator myGenerator;

	public Transform leftPart;
	public Transform rightPart;

	public float speed;

	public bool locked = false;

	public float startPosition;
	public float endPosition;

	float currentPosition;
	float currentTime = 0.0f;
	[Header("SubParts")]
	public bool advance = false;
	public Transform leftSubPart;
	public Transform rightSubPart;
	public float rotationSpeed;

	[Header("Loot")]
	public GameObject loot;

	public float minChance;
	public float maxChance;

	float rotationTempTime = 0;
    float rotationTempAngle = 0;



	public bool itsFirstPlatform = false;
	public JellyControl jelly;

	void Start()
	{
		if(!itsFirstPlatform)
		{
		    float dice = Random.Range(0.0f,1.0f);
			if(dice>minChance && dice<maxChance)
			{
				GameObject m_loot = Instantiate(loot,new Vector3(transform.position.x,transform.position.y + 4.0f,transform.position.z),Quaternion.identity) as GameObject;
				m_loot.transform.parent = transform;
				m_loot.GetComponent<Compendium>().manager = myGenerator.gameObject;
			}

			leftPart.localPosition = new Vector3(-startPosition,leftPart.localPosition.y,leftPart.localPosition.z);
			rightPart.localPosition =  new Vector3(startPosition,rightPart.localPosition.y,rightPart.localPosition.z);

			if(advance)
			{
				leftSubPart.localEulerAngles = new Vector3(0,90,0);
				rightSubPart.localEulerAngles = new Vector3(0,90,0);
			}
		}
	}
	void Update () 
	{
		if(!locked)
		{
			currentTime += speed * Time.unscaledDeltaTime;
			currentPosition = Mathf.Lerp(startPosition,endPosition,currentTime);
		
			leftPart.localPosition = new Vector3(-currentPosition,leftPart.localPosition.y,leftPart.localPosition.z);
			rightPart.localPosition = new Vector3(currentPosition,rightPart.localPosition.y,rightPart.localPosition.z);

			if(rightPart.localPosition.x == endPosition)
			{
				if(advance)
				{
					rotationTempTime += rotationSpeed * Time.unscaledDeltaTime;

					rotationTempAngle = Mathf.LerpAngle(90,0,rotationTempTime);
					leftSubPart.localEulerAngles = new Vector3(0,rotationTempAngle,0);
					rightSubPart.localEulerAngles = new Vector3(0,rotationTempAngle,0);

					if(leftSubPart.localEulerAngles.y == 0)
					{
						transform.parent.GetComponent<PlatformTemp>().platformLocks = true;

						GameScores.AddScore();

						myGenerator.gameObject.SendMessage("UpdateScore");

						locked = true;
					}
				}
				else
				{			
					GameScores.AddScore();

					myGenerator.gameObject.SendMessage("UpdateScore");

					transform.parent.GetComponent<PlatformTemp>().platformLocks = true;
					locked = true;
				}
			}
		}

   }

/*	void Detected()
	{
		jelly.SendMessage("CheckGround");
	}*/


}
