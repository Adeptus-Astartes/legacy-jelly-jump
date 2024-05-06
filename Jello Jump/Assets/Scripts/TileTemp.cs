using UnityEngine;
using System.Collections;
using System.Collections.Generic;
[System.Serializable]
public class TileElement
{
	public Transform element;
	public float startY;
	public float finishY;
}

public class TileTemp : MonoBehaviour 
{
	public GameObject worldTile;
	public List<TileElement> elements;

	public float height;

	public int count;

	public float interval = 2;

	public float time = 0;
	
	public bool move = false;
	public bool stopped = false;

	public float m = 100;

	void Awake()
	{
		foreach(Transform child in transform)
		{
			Destroy(child.gameObject);
		}
	}


	void Start()
	{

		for(int i = 0;i<count;i++)
		{
			GameObject new_tile = Instantiate(worldTile,new Vector3(transform.position.x,transform.position.y + m,transform.position.z),Quaternion.identity) as GameObject;
			new_tile.transform.parent = this.transform;
			TileElement element = new TileElement();
			element.element = new_tile.transform;
			element.startY = new_tile.transform.position.y;
			element.finishY = element.startY - interval;
			elements.Add(element);
			m-=100;
		}
	}

	void OnDrawGizmos()
	{
		Gizmos.color = Color.green;
		Gizmos.DrawLine(new Vector3(transform.position.x,transform.position.y + height,transform.position.z),new Vector3(transform.position.x,transform.position.y - height,transform.position.z));
	}

	void Refresh()
	{
		time = 0;
		
		elements.Clear();
		
		foreach(Transform element in transform)
		{
			TileElement _element = new TileElement();
			_element.element = element;
			_element.startY = element.localPosition.y;
			_element.finishY = element.localPosition.y - interval;
			
			if(_element.finishY < transform.position.y -height)
			{
				GameObject new_tile = Instantiate(worldTile,new Vector3(transform.position.x,transform.position.y + height,transform.position.z),Quaternion.identity) as GameObject;
				new_tile.transform.parent = this.transform;
				Destroy(element.gameObject);
			}
			else
			{
				elements.Add(_element);
			}
		}
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
		}
		else
		{
			foreach(TileElement element in elements)
			{
				element.element.localPosition = new Vector3(0,Mathf.Lerp(element.startY,element.finishY,time),0);

			}
		}
	}
}
