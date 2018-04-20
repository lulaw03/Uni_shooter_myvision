using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Keys : MonoBehaviour {

	//宣告:
	List<GameObject> locks = new List<GameObject>();

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	//功能:keyReceiver
	//計算鎖數量、指定鎖的物件
	void keyReceiver (GameObject obj) {
		if(enabled)	
			locks.Add(obj);
	}
	
	void deadReceiver () {
		for(int i = 0; i < locks.Count; i ++)
			locks[i].SendMessage ("lockReceiver",1 , SendMessageOptions.DontRequireReceiver);	
	}
}
