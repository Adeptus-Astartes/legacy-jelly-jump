using UnityEngine;
using System.Collections;

public class ScoreAnimationText : MonoBehaviour {

	public bool doTween;
	public AnimationCurve x;
	public AnimationCurve y;

	public float time = 0;

	void DoTween()
	{
		doTween = true;
	}

	void Update () 
	{
	    if(doTween)
		{
			time += Time.unscaledDeltaTime*3;

			this.transform.localScale = new Vector2(x.Evaluate(time),y.Evaluate(time));
			if(time>1)
			{
				time = 0;
				doTween = false;
			}
		}
	}
}
