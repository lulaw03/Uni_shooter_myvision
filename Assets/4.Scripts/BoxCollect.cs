using UnityEngine;
using System.Collections;

public class BoxCollect : MonoBehaviour {

	//宣告 :
	//box是一個遊戲物件，box獲取數值(可被其它腳本連結)
	//注意!!標籤box 需拖入製作好的預製物，聲音才會正常播放
	static public int GetCount = 0;
	public AudioClip CollectSound;

	//遊戲初始化 : 將box獲取數量歸零
	// Use this for initialization
	void Start () {
		GetCount = 0;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	//功能 : 角色碰撞
	//如果角色撞到了一個帶有 box 標簽的遊戲物件時
	//增加獲取數值，消除此物件，播放box自身的音效(此音效不會從玩家自身發出，會在box物件處生成一個音效，撥放完畢後自動移除)
	void OnControllerColliderHit(ControllerColliderHit hit) {
		if (hit.gameObject.tag == "box") {
			hit.gameObject.SendMessage ("deadReceiver", SendMessageOptions.DontRequireReceiver);
			Destroy(hit.gameObject);
			hit.gameObject.tag = "Finish";
			GetCount++;
			if (CollectSound)
				AudioSource.PlayClipAtPoint(CollectSound, hit.transform.position);
		}
	}
}
