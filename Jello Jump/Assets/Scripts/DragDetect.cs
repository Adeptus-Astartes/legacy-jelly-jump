using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class DragDetect : MonoBehaviour ,IBeginDragHandler, IDragHandler{

	public JellySelector jellySelcetor;

	public void OnBeginDrag (PointerEventData eventData)
	{
		jellySelcetor.drag = true;
	}
	
	public void OnDrag (PointerEventData eventData)
	{
		jellySelcetor.offset = Mathf.Clamp(eventData.delta.x/2.5f,-20,20);
	}

}
