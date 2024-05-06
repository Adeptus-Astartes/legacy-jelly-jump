using UnityEngine;
using System.Collections;
using System.Collections.Generic;
[System.Serializable]
public class Element
{
	public Transform element;
	public float startY;
	public float finishY;
}

public class PlatformTemp : MonoBehaviour 
{
	public WorldGenerator generator;
	public Platform currentPlatform;
	public List<Element> elements;
	public float interval = 2;

	public float time = 0;

	public bool platformLocks = false;
	public bool move = false;
	public bool stopped = false;

	void Start()
	{
		Refresh();
	}

	void Refresh()
	{
		time = 0;
		
		elements.Clear();
		
		foreach(Transform element in transform)
		{
			Element _element = new Element();
			_element.element = element;
			_element.startY = element.localPosition.y;
			_element.finishY = element.localPosition.y - interval;
			
			if(_element.finishY < -30)
			{
				Destroy(element.gameObject);
			}
			else
			{
				elements.Add(_element);
			}
		}
	}


	public void Add(GameObject _platform)
	{
		currentPlatform = _platform.GetComponent<Platform>();
		_platform.transform.parent = this.transform;
		//Refresh();
	}

	void Update () 
	{

		if(move)
		{
			stopped = false;
			time += Time.unscaledDeltaTime;
			Move();
		}
		if(stopped)
		{
			Refresh();
		}
	}


	void Move()
	{

		if(time>1)
		{
			time = 1;
			move = false;
			stopped = true;
			generator.reload = false;
		}
		else
		{
			foreach(Element element in elements)
			{
				element.element.localPosition = new Vector3(0,Mathf.Lerp(element.startY,element.finishY,time),0);

			}
		}
	}
}
