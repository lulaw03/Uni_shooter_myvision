using UnityEngine;
using System.Collections;

public class Trigger_LevelPass : MonoBehaviour {

	//功能: 觸發區域，如果有物件進入觸發區域時，讀取並執行 1 關卡
	public int a =0;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider other){   

		Application.LoadLevel(a);
	}
}
