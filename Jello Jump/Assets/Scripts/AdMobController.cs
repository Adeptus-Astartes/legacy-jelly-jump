using UnityEngine;
using System.Collections;
using GoogleMobileAds.Api;

public class AdMobController : MonoBehaviour
{
	public BannerView banner;
	public InterstitialAd interstitial;

	void Start()
	{
		StartCoroutine ("LoadInterstetial");
	}

	public void ShowBanner () 
	{
		// Create a 320x50 banner at the top of the screen.
    	banner = new BannerView(
			"ca-app-pub-5644143403091841/5377960012", AdSize.SmartBanner, AdPosition.Bottom);
		// Create an empty ad request.
		AdRequest request = new AdRequest.Builder().Build();
		// Load the banner with the request.
		banner.LoadAd(request);
		banner.Show();
	}

	void RemoveBanner()
	{
		if(banner!=null)
		banner.Destroy();
	}

	public void ShowInterstetial()
	{
		interstitial.Show ();
	}
	
	public IEnumerator LoadInterstetial()
	{
		// Initialize an InterstitialAd.
		interstitial = new InterstitialAd("ca-app-pub-5644143403091841/6854693218");
		// Create an empty ad request.
		AdRequest request = new AdRequest.Builder().Build();
		// Load the interstitial with the request.
		interstitial.LoadAd(request);
		
		while (!interstitial.IsLoaded())
			yield return null;	
	}


}
