using UnityEngine;
using System.Collections;

public class Locks : MonoBehaviour {

	//宣告:
	//鑰匙(可複數)、鑰匙數量、旋轉腳本
	public GameObject[] keys;

	int keysCount;
	RotateObject rotateScript;

	//功能:遊戲初始化
	//旋轉腳本=RotateObject
	//旋轉腳本(關閉)
	//子物件(關閉)
	//collider.isTrigge(開啟)
	//計算鑰匙數量
	//發送訊息到keyReceiver(複數)
	//發送訊息到lockReceiver
	// Use this for initialization
	void Start () {
	
		rotateScript = GetComponent<RotateObject>();
		rotateScript.enabled = false;

		transform.GetChild(0).gameObject.SetActive(false);
		transform.GetComponent<Collider>().isTrigger = true;
		keysCount = keys.Length;

		for (int i = 0; i < keys.Length; i++)
			keys[i].gameObject.SendMessage ("keyReceiver",transform.gameObject, SendMessageOptions.DontRequireReceiver);

		lockReceiver(0);	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	//功能:lockReceiver
	//如果鑰匙=0，旋轉腳本開啟、collider.isTrigge關閉、子物件開啟
	void lockReceiver (int x) {
		keysCount -= x;

		if(keysCount == 0){

			rotateScript.enabled = true;
			transform.GetComponent<Collider>().isTrigger = false;
			transform.GetChild(0).gameObject.SetActive(true);		
		}
	}
}
