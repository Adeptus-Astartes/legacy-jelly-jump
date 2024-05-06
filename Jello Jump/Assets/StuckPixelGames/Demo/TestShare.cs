using UnityEngine;
using System.Collections;
using System.IO;
using System.Threading;


public class TestShare : MonoBehaviour
{
	public Camera screenShotCamera;
	public TextMesh score;
	public Texture2D screenShot;
	public int scorenum = 0;
	byte[] mbytes;

	void Start()
	{
		MakeSquarePngFromOurVirtualThingy();
	}

	public void MakeSquarePngFromOurVirtualThingy()
	{

		// capture the virtuCam and save it as a square PNG.
		score.gameObject.SetActive(true);
		score.text = SPlayerPrefs.GetInt("ysy734nfhdndjslgc89yyhagvdfxg_jsjkkjs").ToString();

		int sqr = 512;

		RenderTexture tempRT = new RenderTexture(sqr,sqr, 0 );

		screenShotCamera.targetTexture = tempRT;
		screenShotCamera.Render();
		
		RenderTexture.active = tempRT;
		Texture2D virtualPhoto =
			new Texture2D(sqr-128,sqr-128, TextureFormat.ARGB32, false);
		// false, meaning no need for mipmaps
		virtualPhoto. ReadPixels( new Rect(64, 64, sqr-64,sqr-64), 0, 0);
		screenShot = virtualPhoto;
		
		RenderTexture.active = null; //can help avoid errors 
		screenShotCamera.targetTexture = null;
		score.gameObject.SetActive(false);
	}

	private string OurTempSquareImageLocation()
	{
		string r = Application.persistentDataPath + "/p.png";
		print(r);
		return r;
	}

	public void ShareScore()
	{
		StartCoroutine("WaitToShare");
	}

	IEnumerator WaitToShare()
	{
		string url = " https://play.google.com/store/apps/details?id=com.AppSerrGamingStudio.JelloJump ";
		StuckPixel.SPAndroidShare.ShareByteArray(screenShot.EncodeToPNG(),"","I just scored " + scorenum.ToString() + " points in #JelloJump! It was awesome!\n" + url,false);
		yield return null;
	}
}
