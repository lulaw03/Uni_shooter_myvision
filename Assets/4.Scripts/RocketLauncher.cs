using UnityEngine;
using System.Collections;

public class RocketLauncher : MonoBehaviour {

	//宣告:
	//火箭彈的來源、飛行速度、射擊頻率、火箭彈數量、前次射擊時間
	public Rigidbody projectile;
	public float initialSpeed = 20.0f;
	public float  reloadTime = 0.5f;
	public float  ammoCount = 20f;

	float lastShot = -10.0f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void Fire () {
		//如果遊戲時間>射擊頻率且火箭彈數量>0，動態產生一個火箭彈並給予方向及力量投射出去
		if (Time.time > reloadTime + lastShot && ammoCount > 0) {
			//產生火箭彈，位置於發射點的位置
			Rigidbody instantiatedProjectile = Instantiate (projectile, transform.position, transform.rotation) as Rigidbody;
			//使火箭彈於發射點的前方發射出去，飛行速度為initialSpeed變數
			instantiatedProjectile.velocity = transform.TransformDirection(new Vector3 (0, 0, initialSpeed));
			//忽略父級碰撞
			if (transform.root.GetComponent<Collider>())
				Physics.IgnoreCollision(instantiatedProjectile.GetComponent<Collider>(), transform.root.GetComponent<Collider>());		
			lastShot = Time.time;
			//火箭彈數量減少
			ammoCount--;
		}
	}
}
