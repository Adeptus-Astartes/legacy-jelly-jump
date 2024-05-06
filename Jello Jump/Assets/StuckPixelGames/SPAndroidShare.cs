using UnityEngine;
using System;
using System.IO;

/* READ ME:
	Place this file in your "Plugins" folder. 
	You MUST add the WRITE_EXTERNAL_STORAGE permission to your Android manifest: <uses-permission android:name="android.permission.WRITE_EXTERNAL_STORAGE" />
	From any script, you just call StuckPixel.SPAndroidShare.METHOD(params). 
	For example: StuckPixel.SPAndroidShare.ShareScreenshot("Check this!", "Look at my score!", true)
 */

namespace StuckPixel
{
	public class SPAndroidShare
	{
		/// <summary>
		/// Share the picture at the given file path. Subject and body will pre-fill for the user when possible
		/// </summary>
		public static void ShareFile(string path, string subject, string body)
		{
		#if UNITY_ANDROID
			if(!File.Exists(path))
			{
				Debug.Log("File does not exist");
			}
			else if(subject == null || body == null)
			{
				Debug.Log("Body and subject cannot be null");
			}
			else
			{
				AndroidJavaClass intentClass = new AndroidJavaClass ("android.content.Intent");
				AndroidJavaObject intentObject = new AndroidJavaObject ("android.content.Intent");
				AndroidJavaClass uriClass = new AndroidJavaClass("android.net.Uri");
				AndroidJavaObject uriObject = uriClass.CallStatic<AndroidJavaObject>("parse","file://"+path);
				intentObject.Call<AndroidJavaObject> ("setAction", intentClass.GetStatic<string> ("ACTION_SEND"));
				intentObject.Call<AndroidJavaObject> ("setType", "image/*");
				intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_SUBJECT"), subject);
				intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_TEXT"), body);
				intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_STREAM"), uriObject);
				AndroidJavaClass unity = new AndroidJavaClass ("com.unity3d.player.UnityPlayer");
				AndroidJavaObject currentActivity = unity.GetStatic<AndroidJavaObject> ("currentActivity");
				currentActivity.Call ("startActivity", intentObject);
			}
		#else
			Debug.Log("NOT an Android device");
		#endif
		}

		/// <summary>
		/// Share the picture's byte array. Subject and body will pre-fill for the user when possible. Unique will create a new file rather than overwrite an existing. Please note that this creates a new file if one does not exist.
		/// </summary>
		public static void ShareByteArray(byte[] picBytes, string subject, string body, bool unique) 
		{
		#if UNITY_ANDROID
			if(subject == null || body == null)
			{
				Debug.Log("Body and subject cannot be null");
			}
			else
			{
				string filePath = "";
				if(unique)
				{
					filePath = Path.Combine(Application.persistentDataPath, DateTime.Now.ToString("yyyy-MM-dd-HHmmss") + ".png");
				}
				else
				{
					filePath = Path.Combine(Application.persistentDataPath, "SPScreenshot.png");
				}
				File.WriteAllBytes(filePath, picBytes);
				ShareFile(filePath, subject, body);
			}
		#else
			Debug.Log("NOT an Android device");
		#endif
		}

		/// <summary>
		/// Share the current screen pixels. Subject and body will pre-fill for the user when possible. Unique will create a new file rather than overwrite an existing. Please note that this creates a new file if one does not exist.
		/// </summary>
		public static void ShareScreenBytes(string subject, string body, bool unique)
		{
		#if UNITY_ANDROID
			ShareByteArray(GetScreenBytes(), subject, body, unique);
		#else
			Debug.Log("NOT an Android device");
		#endif
		}

		public static void ShareScreenBytes(string subject, string body, bool unique, TextureFormat textureFormat)
		{
			#if UNITY_ANDROID
			ShareByteArray(GetScreenBytes(textureFormat), subject, body, unique);
			#else
			Debug.Log("NOT an Android device");
			#endif
		}

		/// <summary>
		/// Share the current Unity screenshot. Subject and body will pre-fill for the user when possible. Unique will create a new file rather than overwrite an existing. Please note that this creates a new file if one does not exist.
		/// </summary>
		public static void ShareScreenshot(string subject, string body, bool unique)
		{
		#if UNITY_ANDROID
			if(subject == null || body == null)
			{
				Debug.Log("Body and subject cannot be null");
			}
			else
			{
				string filePath = "";
				string fileName = "";
				if(unique)
				{
					fileName = DateTime.Now.ToString("yyyy-MM-dd-HHmmss") + ".png";
				}
				else
				{
					fileName = "SPScreenshot.png";
				}
				filePath = Path.Combine(Application.persistentDataPath, fileName);
				Application.CaptureScreenshot(fileName);
				ShareFile(filePath, subject, body);
			}
		#else
			Debug.Log("NOT an Android device");
		#endif
		}

		/// <summary>
		/// Share the provided render texture from the provided camera. Subject and body will pre-fill for the user when possible. Unique will create a new file rather than overwrite an existing. Please note that this creates a new file if one does not exist.
		/// </summary>
		public static void ShareRenderTexture(Camera renderCamera, RenderTexture thisRenderTexture, string subject, string body, bool unique)
		{
		#if UNITY_ANDROID
			if(renderCamera == null)
			{
				Debug.Log ("Render Camera must be set");
			}
			else if(thisRenderTexture == null)
			{
				Debug.Log ("Render Texture must be set");
			}
			else if(subject == null || body == null)
			{
				Debug.Log("Body and subject cannot be null");
			}
			else
			{
				RenderTexture currentRT = RenderTexture.active;
				RenderTexture.active = thisRenderTexture;
				renderCamera.Render();
				Texture2D screenTexture = new Texture2D(thisRenderTexture.width, thisRenderTexture.height, TextureFormat.RGB24, false);
				screenTexture.ReadPixels(new Rect(0, 0, screenTexture.width, screenTexture.height), 0, 0);
				screenTexture.Apply();
				RenderTexture.active = currentRT;
				ShareByteArray(screenTexture.EncodeToPNG(), subject, body, unique);
			}
		#else
			Debug.Log("NOT an Android device");
		#endif
		}

		public static void ShareRenderTexture(Camera renderCamera, RenderTexture thisRenderTexture, string subject, string body, bool unique, TextureFormat textureFormat)
		{
			#if UNITY_ANDROID
			if(renderCamera == null)
			{
				Debug.Log ("Render Camera must be set");
			}
			else if(thisRenderTexture == null)
			{
				Debug.Log ("Render Texture must be set");
			}
			else if(subject == null || body == null)
			{
				Debug.Log("Body and subject cannot be null");
			}
			else
			{
				RenderTexture currentRT = RenderTexture.active;
				RenderTexture.active = thisRenderTexture;
				renderCamera.Render();
				Texture2D screenTexture = new Texture2D(thisRenderTexture.width, thisRenderTexture.height, textureFormat, false);
				screenTexture.ReadPixels(new Rect(0, 0, screenTexture.width, screenTexture.height), 0, 0);
				screenTexture.Apply();
				RenderTexture.active = currentRT;
				ShareByteArray(screenTexture.EncodeToPNG(), subject, body, unique);
			}
			#else
			Debug.Log("NOT an Android device");
			#endif
		}
		
		/// <summary>
		/// Return the current screen bytes
		/// </summary>
		public static byte[] GetScreenBytes()
		{
			Texture2D screenTexture = new Texture2D(Screen.width, Screen.height,TextureFormat.RGB24,true);
			screenTexture.ReadPixels(new Rect(0f, 0f, Screen.width, Screen.height),0,0);
			screenTexture.Apply();
			byte[] byteArray = screenTexture.EncodeToPNG();
			return byteArray;
		}

		public static byte[] GetScreenBytes(TextureFormat textureFormat)
		{
			Texture2D screenTexture = new Texture2D(Screen.width, Screen.height,textureFormat,true);
			screenTexture.ReadPixels(new Rect(0f, 0f, Screen.width, Screen.height),0,0);
			screenTexture.Apply();
			byte[] byteArray = screenTexture.EncodeToPNG();
			return byteArray;
		}

		/// <summary>
		/// Save a byte array to a file. File will be PNG format. It is not necessary to add .png to the end of fileName
		/// </summary>
		public static void SaveByteArray(byte[] picBytes, string fileName)
		{
		#if !UNITY_WEBPLAYER
			string filePath = "";
			if(fileName.EndsWith(".png"))
			{
				filePath = Path.Combine(Application.persistentDataPath, fileName);
			}
			else
			{
				filePath = Path.Combine(Application.persistentDataPath, fileName + ".png");
			}
			File.WriteAllBytes(filePath, picBytes);
		#endif
		}
	}
}
