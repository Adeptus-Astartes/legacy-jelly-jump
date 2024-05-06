using UnityEngine;
using System.Collections;

public class CollisionDetector : MonoBehaviour
{
	public Platform platform;

	void OnCollisionEnter(Collision other)
	{
		//platform.SendMessage("Detected");
	}
}
