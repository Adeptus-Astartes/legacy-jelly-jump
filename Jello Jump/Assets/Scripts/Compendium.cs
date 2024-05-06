using UnityEngine;
using System.Collections;

public class Compendium : MonoBehaviour 
{
	public GameObject manager;
	public GameObject deadEffect;
	bool active = false;
	void OnTriggerEnter(Collider other)
	{
		if(!active)
		{
			if(deadEffect)
	     		Instantiate(deadEffect,transform.position,transform.rotation);


			int current = SPlayerPrefs.GetInt("dgkjhkdfjbbrwe7tr7erbbx7542bvjxcucbugd");
			print("LOOT" + current.ToString());
			SPlayerPrefs.SetInt("dgkjhkdfjbbrwe7tr7erbbx7542bvjxcucbugd",current + 3);
			manager.SendMessage("UpdateCompendium");
			active = true;
			Destroy(gameObject);
		}
	}
}
