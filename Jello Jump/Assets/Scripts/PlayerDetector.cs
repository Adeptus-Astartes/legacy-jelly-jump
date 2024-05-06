using UnityEngine;
using System.Collections;

public class PlayerDetector : MonoBehaviour 
{
	
	void OnTriggerEnter(Collider other)
	{
		transform.parent.SendMessage("Loose");
	}
}
