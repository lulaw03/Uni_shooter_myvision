using UnityEngine;
using System.Collections;

public class SwitchComponentOnPause : MonoBehaviour {

	//配合LockCursor腳本使用的開關腳本
	public MonoBehaviour[] escDisplay;
	public MonoBehaviour[] escHidden;

	public bool GuiReturn = false;

	GUITexture guiA;
	GUIText guiB;

	// Use this for initialization
	void Start () {
	
		guiA = GetComponent<GUITexture> ();
		guiB = GetComponent<GUIText> ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void DidPause (bool pause) {

		foreach(var a in escDisplay)
		{
			a.enabled = pause;
		}

		foreach(var b in escHidden)
		{
			b.enabled = !pause;
		}

		if(!GuiReturn){
			if (guiA)
				guiA.enabled = !pause;
			if (guiB)
				guiB.enabled = !pause;
		}
		else{
			if (guiA)
				guiA.enabled = pause;
			if (guiB)
				guiB.enabled = pause;
		}
	}
}
