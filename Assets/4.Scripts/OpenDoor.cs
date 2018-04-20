using UnityEngine;
using System.Collections;

public class OpenDoor : MonoBehaviour {
	
	bool doorOpened = false;//開門狀態(是/否)
	float OpenDoorTimer = 0.0f;//開門計時器是一個帶有小數點的數值
	Transform theDoor;//門，是一個遊戲物件
	GameObject[] box;//box，是多個遊戲物件
	//開關門音效
	public AudioClip doorOpenAudio;
	public AudioClip doorCloseAudio;

	static public int boxCount;//box數量

	//功能:遊戲初始化
	//計算box在遊戲中的總數量
	void Awake(){
		box = GameObject.FindGameObjectsWithTag("box");
		boxCount = box.Length;
	}
	// Use this for initialization
	void Start () {
	
	}

	//功能 : 每個 Frame 持續執行(開門計時器)
	//如果開門狀態為 "是"，產生計時秒數，如果計時秒數大於 2 秒時，執開關門功能
	// Update is called once per frame
	void Update () {
		if (doorOpened) 
			OpenDoorTimer += Time.deltaTime;	  
		if (OpenDoorTimer >= 2) 
			shutDoor();
	}

	//功能 : 角色碰撞功能
	//如果角色撞擊到有doorstep標籤的物件，(並且開門狀態為"否")，門=碰撞到的物件
	//如果門的父級標籤為 door2 ，且如果box獲取數量=box總數量，執行開門功能
	//如果父級標籤不是 door2 ，執行開門功能
	void OnControllerColliderHit(ControllerColliderHit hit){  		
		if (hit.gameObject.tag	== "doorstep" && doorOpened == false) {
			theDoor = hit.transform;
			if (theDoor.parent.tag == "door2") {
				if (BoxCollect.GetCount == boxCount)
					openDoor();
			}
			else
				openDoor();
		}
	}

	//功能 : openDoor
	//將開門狀態設定為"是"，播放房子的開門動畫，並播放開門播放器的聲音
	void openDoor() {
		doorOpened = true;
		theDoor.parent.GetComponent<Animation>().Play("dooropen");
		if (doorOpenAudio)
			AudioSource.PlayClipAtPoint(doorOpenAudio, theDoor.transform.position);
		
	}
	//功能 : shutDoor
	//將開門狀態設定為 "否"，播放房子的關門動畫，並播放關門播放器的聲音，將計時秒數設定為 0
	void shutDoor() {
		doorOpened = false;
		theDoor.parent.GetComponent<Animation>().Play("doorshuts");
		if (doorCloseAudio)
			AudioSource.PlayClipAtPoint(doorCloseAudio, theDoor.transform.position);
		OpenDoorTimer = 0;
	}
}
