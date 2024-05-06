using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Linq;

[System.Serializable]
public class Item
{
	public string name;
	public string key;
	public JellyControl jelly;
	public GameObject temp;
	public bool readyToBuy = false;
	public bool unlocked = false;
	public float position;
	
}

public class JellySelector : MonoBehaviour
{
	public GameManager manager;
	public List<Item> jellyies;
	public GameObject table;
	public Transform basePlatform;
	public int id;

	public GameObject selectedObject;
	public Transform center;
	
	public float gravity;
	public float offset;
	
	public bool drag = false;
	
	public bool check = false;
	
	public float minLenght;
	public float maxLenght;
	
	
	public float minDetectPos;
	public float maxDetectPos;
	
	
	
	public bool move = false;
	
	public float time;
	
	public AnimationCurve y;
	public AnimationCurve z;
	

	public void UpdateTheme(Theme theme)
	{
		Debug.LogWarning(theme._names);
		foreach(Item _item in jellyies)
		{
			if(_item.temp!= null && _item.temp.name != "BuyTable")
			{
				_item.temp.GetComponent<Renderer>().sharedMaterial.SetColor("_Color", theme.jellyColor);
				JellyControl control = _item.temp.GetComponent<JellyControl>() ;
				control.splatFX.GetComponent<ParticleSystem>().startColor = theme.jellyColor;
				control.mainColor = theme.jellyColor;
				control.jumpFX = theme.jumpFX;
			}
		}
	}

	void Awake()
	{
		SPlayerPrefs.SetString(jellyies[0].key,"TRUE");
		if(SPlayerPrefs.GetString(jellyies[1].key) == "")
		{
			SPlayerPrefs.SetString(jellyies[1].key,"FALSE");
		}
	}



	void Start()
	{
		Refresh();
	}

	void Refresh()
	{
		minLenght -= jellyies[jellyies.Count-1].position;
		foreach(Item item in jellyies)
		{
			if(SPlayerPrefs.GetString(item.key) == "TRUE")
			{
				if(item.temp)
					Destroy(item.temp);



				item.temp = Instantiate(item.jelly.gameObject,new Vector3(item.position,-7,transform.position.z),Quaternion.identity) as GameObject;
				item.temp.transform.parent = transform;

				JellyControl controler = item.temp.GetComponent<JellyControl>();

				controler.manager = manager;	



				if(jellyies.IndexOf(item) == id)
				{
					manager.CloseBuyButton();
					selectedObject = item.temp;
				}


			}

			if(SPlayerPrefs.GetString(item.key) == "FALSE")
			{
				if(item.temp)
					Destroy(item.temp);

				item.temp = Instantiate(table,new Vector3(item.position ,-7,transform.position.z),Quaternion.identity) as GameObject;
				item.temp.transform.parent = transform;
				item.temp.transform.localPosition = new Vector3(item.position,-7,transform.position.z);
				item.temp.name = "BuyTable";
			
			}
		}
		check = true;
	}
	
	void Update()
	{
		
		if(drag && !move)
		{
			if(offset>0)
			{
				if(transform.localPosition.x < maxLenght)
					transform.Translate(Vector3.right * Time.unscaledDeltaTime * offset);
				
				offset -= Time.unscaledDeltaTime * gravity;
				if(offset<0)
				{
					offset = 0;
					check = true;
					drag = false;
				}
			}
			if(offset<0)
			{
				if(transform.localPosition.x > minLenght)
					transform.Translate(Vector3.right * Time.unscaledDeltaTime * offset);
				
				offset += Time.unscaledDeltaTime * gravity;
				if(offset>0)
				{
					offset = 0;
					check = true;
					drag = false;
				}
			}
		}
		if(check)
		{
			foreach(Item item in jellyies)
			{
				if(SPlayerPrefs.GetString(item.key) == "TRUE"|| SPlayerPrefs.GetString(item.key) == "FALSE")
				{
					if(item.temp.transform.position.x > -2.5f && item.temp.transform.position.x < 2.5f)
					{
						selectedObject = item.temp.gameObject;
	
						if(selectedObject.name == "BuyTable")
						{
							manager.ShowBuyButton();
						}
						else
						{
							PlayerPrefs.SetInt("SelectedJellyId",jellyies.IndexOf(item));
							manager.CloseBuyButton();
						}

						check = false;
						move = true;
					}
				}
			}
		}
		if(move)
		{
			time += Time.unscaledDeltaTime;
			transform.position = new Vector3(Mathf.Lerp(transform.position.x, -selectedObject.transform.localPosition.x,time),transform.position.y,0);

			if(time > 0.5f)
			{
				time = 0;
				move = false;
			}
		}
		
		if(!move && manager.gameStart)
		{
			manager.generator.platformTemp.Add(basePlatform.gameObject);
			if(selectedObject)
			{
				selectedObject.GetComponent<JellyControl>().m_JellyMesh.m_ReferencePointParent.transform.position = new Vector3(0,-7,0);
				selectedObject.transform.SetParent(null);
			}
			
			time += Time.unscaledDeltaTime;
			transform.position = new Vector3(transform.position.x,y.Evaluate(time),z.Evaluate(time));
			
			if(time>1)
			{
				foreach(Item m_jelly in jellyies)
				{
					if(m_jelly.temp)
					{
						if(m_jelly.temp != selectedObject)
						m_jelly.temp.SendMessage("Harakiri",SendMessageOptions.DontRequireReceiver);
					}
				}

				gameObject.SetActive(false);
			}
		}
		
	}

	//BILLING
	void Buy()
	{
		Item currentItem = new Item();

		foreach(Item item in jellyies)
		{
			if(SPlayerPrefs.GetString(item.key) == "FALSE")
			{
				if(SPlayerPrefs.GetInt("dgkjhkdfjbbrwe7tr7erbbx7542bvjxcucbugd") >= 200)
				{
					currentItem = item;

					SPlayerPrefs.SetInt("dgkjhkdfjbbrwe7tr7erbbx7542bvjxcucbugd", SPlayerPrefs.GetInt("dgkjhkdfjbbrwe7tr7erbbx7542bvjxcucbugd") - 200);
					manager.UpdateCompendium();

				}

			}
		}
		if(currentItem.name != "")
		{
			currentItem.unlocked = true;
			currentItem.readyToBuy = false;
			SPlayerPrefs.SetString(currentItem.key,"TRUE");
			id = jellyies.IndexOf(currentItem);
			if(id + 1 < jellyies.Count)
			{
				SPlayerPrefs.SetString(jellyies[id + 1].key,"FALSE");
			}
			
			Refresh();
		}


	}

}