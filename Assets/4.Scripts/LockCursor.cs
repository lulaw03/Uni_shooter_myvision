using UnityEngine;
using System.Collections;

public class LockCursor : MonoBehaviour {

	//宣告:被鎖定(是/否)
	public bool wasLocked = true;

	GUIText gui;

	//功能:初始化
	//如果使用平台是網頁平台，功能"SetPause"=是，反之"SetPause"=否、滑鼠游標隱藏
	// Use this for initialization
	void Start () {
	
		gui = GetComponent<GUIText> ();

		if (Application.platform == RuntimePlatform.OSXWebPlayer ||
		    Application.platform == RuntimePlatform.WindowsWebPlayer)
		{
			SetPause(true);
		}
		else
		{
			SetPause(false);
			Cursor.lockState = CursorLockMode.Locked;
			Cursor.visible = false; 
		}
	}
	//功能: 每個 Frame 持續執行
	//如果按下滑鼠右鍵，滑鼠游標隱藏
	//如果滑鼠游標沒隱藏且被鎖定，"SetPause"=是
	//如果滑鼠游標隱藏且沒鎖定，"SetPause"=否
	// Update is called once per frame
	void Update () {
	
		if (Input.GetKeyDown ("escape")) {
			Cursor.lockState = CursorLockMode.None;
			Cursor.visible = true; 
		}

		if (Input.GetMouseButton (0)) {

			Cursor.lockState = CursorLockMode.Locked;
			Cursor.visible = false; 
		}

		// Did we lose cursor locking?
		// eg. because the user pressed escape
		// or because he switched to another application
		// or because some script set Screen.lockCursor = false;
		if (Cursor.visible && wasLocked)
		{
			wasLocked = false;
			SetPause(true);
		}
		// Did we gain cursor locking?
		else if (!Cursor.visible && !wasLocked)
		{
			wasLocked = true;
			SetPause(false);
		}
	}

	//功能:在應用退出之前發送給所有的遊戲物體
	//時間縮放=1(1表示正常0表示暫停)
	void OnApplicationQuit ()
	{
		Time.timeScale = 1;
	}
	//功能:SetPause(是/否)
	//對功能"DidPause"發送訊息
	void SetPause (bool pause)
	{
		Input.ResetInputAxes();
		GameObject[] gos = FindObjectsOfType<GameObject>();

		foreach (var go in gos)
			go.SendMessage("DidPause", pause, SendMessageOptions.DontRequireReceiver);
		
		transform.position = Vector3.zero;
		
		if (pause)
		{
			Time.timeScale = 0.0001f;
			transform.position = new Vector3 (0.5f, 0.5f, 0f);
			gui.anchor = TextAnchor.MiddleCenter;
		}
		else
		{
			gui.anchor = TextAnchor.UpperLeft;
			transform.position = new Vector3(0, 1, 0);
			Time.timeScale = 1;
		}
	}
	//功能:DidPause(是/否)
	//如果是，顯示"Click to start playing"，反之顯示"Escape to show the cursor"
	void DidPause (bool pause)
	{
		if (pause)
		{
			// Show the button again
			gui.enabled = true;
			gui.text = "Click to start playing";
		}
		else
		{
			// Disable the button
			gui.enabled = true;
			gui.text = "Escape to show the cursor";
		}
	}
	//功能:當滑鼠按下
	//滑鼠游標隱藏
	void OnMouseDown ()
	{
		// Lock the cursor
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false; 
	}
}
