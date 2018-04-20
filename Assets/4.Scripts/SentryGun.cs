using UnityEngine;
using System.Collections;

public class SentryGun : MonoBehaviour {

	//宣告:
	//攻擊距離、射擊精準度、目標	
	public float attackRange = 30.0f;
	public float shootAngleDistance = 10.0f;
	public Transform target;

	//功能:遊戲初始化
	//如果沒有目標，目標=場景中標籤是"Player"的物件
	// Use this for initialization
	void Start () {
	
		if (target == null && GameObject.FindWithTag("Player"))
			target = GameObject.FindWithTag("Player").transform;
	}

	//功能 : 每個 Frame 持續執行
	//如果發現目標，發送訊息到"Fire"
	// Update is called once per frame
	void Update () {
	
		if (target == null)
			return;
		
		if (!CanSeeTarget ())
			return;
		
		// Rotate towards target	
		var targetPoint = target.position;
		var targetRotation = Quaternion.LookRotation (targetPoint - transform.position, Vector3.up);
		transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 2.0f);
		
		// If we are almost rotated towards target - fire one clip of ammo
		var forward = transform.TransformDirection(Vector3.forward);
		var targetDir = target.position - transform.position;
		if (Vector3.Angle(forward, targetDir) < shootAngleDistance)//射擊精準度
			SendMessage("Fire");
	}

	//功能:CanSeeTarget
	//偵測目標
	bool CanSeeTarget ()
	{  
		if (Vector3.Distance(transform.position, target.position) > attackRange)
			return false;
		
		RaycastHit hit;
		if (Physics.Linecast (transform.position, target.position, out hit))
			return hit.transform == target;
		
		return false;
	}
}
