using UnityEngine;
using System.Collections;

public class PlayerWeapons : MonoBehaviour {

	//遊戲初始化 : 選擇第一把武器
	// Use this for initialization
	void Start () {
		// Select the first weapon
		SelectWeapon(0);
	}

	//功能 : 每個 Frame 持續執行
	//如果按下Fire1(通常是滑鼠右鍵或鍵盤Ctrl)，發送訊息到Fire(通常在武器的腳本中會有個Fire接收器)
	//如果按下鍵盤 1或2 切換武器
	// Update is called once per frame
	void Update () {
	
		// Did the user press fire?
		if (Input.GetButton ("Fire1"))
			BroadcastMessage("Fire");
		if (Input.GetKeyDown("1")) 
			SelectWeapon(0);
		else if (Input.GetKeyDown("2")) 
			SelectWeapon(1);
	}

	//功能:SelectWeapon
	//被選擇的武器顯示，沒被選擇的武器隱藏
	void SelectWeapon (int index) {
		for (var i=0;i<transform.childCount;i++)	{
			// Activate the selected weapon
			if (i == index)
				transform.GetChild(i).gameObject.SetActive(true);
			// Deactivate all other weapons
			else
				transform.GetChild(i).gameObject.SetActive(false);
		}
	}
}
