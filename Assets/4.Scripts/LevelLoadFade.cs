using UnityEngine;
using System.Collections;

public class LevelLoadFade : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

/*
	Usage:

	// Load my level	
	LevelLoadFade.FadeAndLoadLevel("mylevel", Color.white, 0.5);

	// Reset the current level
	LevelLoadFade.FadeAndLoadLevel(Application.loadedLevel, Color.white, 0.5);
*/

	static public void FadeAndLoadLevel (int level, Texture2D fadeTexture, float fadeLength)
	{
		if (fadeTexture == null) {
			FadeAndLoadLevel (level, Color.white, fadeLength);

			return;
		}
		
		var fade = new GameObject ("Fade");
		fade.AddComponent<LevelLoadFade>();
		fade.AddComponent<GUITexture>();
		fade.transform.position = new Vector3 (0.5f, 0.5f, 1000f);
		fade.GetComponent<GUITexture>().texture = fadeTexture;
		fade.GetComponent<LevelLoadFade>().DoFade(level, fadeLength, false);
	}
	
	static public void FadeAndLoadLevel (int level, Color color, float fadeLength)
	{
		var fadeTexture = new Texture2D (1, 1);
		fadeTexture.SetPixel(0, 0, color);
		fadeTexture.Apply();
		
		var fade = new GameObject ("Fade");
		fade.AddComponent<LevelLoadFade>();
		fade.AddComponent<GUITexture>();
		fade.transform.position = new Vector3 (0.5f, 0.5f, 1000f);
		fade.GetComponent<GUITexture>().texture = fadeTexture;
		
		DontDestroyOnLoad(fadeTexture);
		fade.GetComponent<LevelLoadFade>().DoFade(level, fadeLength, true);
	}

	public void DoFade (int level, float fadeLength, bool destroyTexture)
	{
		StartCoroutine (IEDoFade (level, fadeLength, destroyTexture));
	}

	public IEnumerator IEDoFade (int level, float fadeLength, bool destroyTexture)
	{
		// Dont destroy the fade game object during level load
		DontDestroyOnLoad(gameObject);

		GUITexture gui = GetComponent<GUITexture> ();

		// Fadeout to start with
		gui.color = new Color(gui.color.r, gui.color.g, gui.color.b, 0);
		
		// Fade texture in
		var time = 0.0f;

		while (time < fadeLength)
		{
			time += Time.deltaTime;
			gui.color = new Color(gui.color.r, gui.color.g, gui.color.b, Mathf.InverseLerp(0.0f, fadeLength, time));

			yield return null;
		}

		gui.color = new Color(gui.color.r, gui.color.g, gui.color.b, 1);

		yield return null;;
		
		// Complete the fade out (Load a level or reset player position)
		Application.LoadLevel(level);
		
		// Fade texture out
		time = 0.0f;

		while (time < fadeLength)
		{
			time += Time.deltaTime;
			gui.color = new Color(gui.color.r, gui.color.g, gui.color.b, Mathf.InverseLerp(fadeLength, 0.0f, time));

			yield return null;
		}

		gui.color = new Color(gui.color.r, gui.color.g, gui.color.b, 0);

		yield return null;
		
		Destroy (gameObject);
		
		// If we created the texture from code we used DontDestroyOnLoad,
		// which means we have to clean it up manually to avoid leaks
		if (destroyTexture)
			Destroy (gui.texture);
	}
}
