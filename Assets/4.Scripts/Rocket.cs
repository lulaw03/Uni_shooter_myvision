using UnityEngine;
using System.Collections;

[RequireComponent (typeof (Rigidbody))]
public class Rocket : MonoBehaviour {

	//宣告:爆炸特效、終止時間
	// The reference to the explosion prefab
	public GameObject explosion;
	public float timeOut = 3.0f;

	//功能:遊戲初始化
	//執行功能Kill
	// Use this for initialization
	void Start () {
	
		Invoke("Kill", timeOut);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	//功能:觸發區域
	//在碰撞到的地方動態產生一個爆炸特效
	//執行功能"Kill"
	void OnCollisionEnter (Collision collision) {
		// Instantiate explosion at the impact point and rotate the explosion
		// so that the y-axis faces along the surface normal
		ContactPoint contact = collision.contacts[0];
		var rotation = Quaternion.FromToRotation(Vector3.up, contact.normal);
		Instantiate (explosion, contact.point, rotation);
		
		// And kill our selves
		Kill ();    
	}
	//功能:Kill
	//關閉子物件煙霧特效
	//子物件分離
	//刪除物件
	void Kill () {
		// Stop emitting particles in any children
		ParticleEmitter emitter = GetComponentInChildren<ParticleEmitter>();
		if (emitter)
			emitter.emit = false;
		// Detach children - We do this to detach the trail rendererer which should be set up to auto destruct
		transform.DetachChildren();
		// Destroy the projectile
		Destroy(gameObject);
	}

}
