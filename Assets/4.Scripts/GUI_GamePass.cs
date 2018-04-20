using UnityEngine;
using System.Collections;

public class GUI_GamePass : MonoBehaviour {

	//宣告 : 介面模版，背景圖片，預載狀態(是/否) 
	public GUISkin PassSkin;
	public Texture BackGround;

	bool isLoading = false;

	//遊戲初始化 : 解除滑鼠游標鎖定
	// Use this for initialization
	void Start () {
	
		Cursor.lockState = CursorLockMode.None;
		Cursor.visible = true;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	//功能 : GUI 
	//使用介面模版 PassSkin , 建立介面圖像，建立再玩一次按鈕，建立離開遊戲按鈕
	//如果按下再玩一次按鈕時，啟動預載模式，讀取並執行 0 關卡
	//如果預載狀態為 "是"，則在介面上顯示文字 "遊戲載入中..."
	//如果按下離開遊戲按鈕，關閉程式
	//如果按下鍵盤 esc ，關閉程式
	//如果按下鍵盤 space ，啟動預載模式，讀取並執行 0 關卡
	void OnGUI ()
	{
		GUI.skin = PassSkin;
		GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), BackGround);
		GUI.Label(new Rect(Screen.width *0.5f-100, Screen.height *0.5f-100, 400, 200), "奪 得 軍 旗！");

		if(GUI.Button(new Rect(Screen.width *0.5f -200, Screen.height *0.5f, 200, 40), "再玩一次")) {
			isLoading = true;
			Application.LoadLevel(0);
		}
		if((isLoading == true))
			GUI.Label(new Rect(Screen.width *0.5f -40, Screen.height *0.5f +140, 300, 80), "遊戲載入中 ...");
		if(GUI.Button(new Rect(Screen.width *0.5f +100, Screen.height *0.5f, 200, 40), "離開遊戲"))
			Application.Quit();
		if (Input.GetKey("escape"))
			Application.Quit();
		if  (Input.GetKey("space")) {
			isLoading = true;
			Application.LoadLevel(0);
		}
	}
}
