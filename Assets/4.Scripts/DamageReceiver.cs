using UnityEngine;
using System.Collections;

public class DamageReceiver : MonoBehaviour {

	//宣告:生命值、死亡延遲時間、爆炸特效、替代物
	public float hitPoints = 100.0f;
	public float detonationDelay = 0.0f;
	public Transform explosion;
	public Rigidbody deadReplacement;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	//功能:ApplyDamage(傷害值)
	//如果生命值=0，執行功能"DelayedDetonate"
	void ApplyDamage (float damage) {
		// We already have less than 0 hitpoints, maybe we got killed already?
		if (hitPoints <= 0.0f)
			return;
		if(enabled)	
			hitPoints -= damage;
		if (hitPoints <= 0.0f) {
			// Start emitting particles
			ParticleEmitter emitter = GetComponentInChildren<ParticleEmitter>();
			if (emitter)
				emitter.emit = true;
			
			Invoke("DelayedDetonate", detonationDelay);
		}
	}
	//功能:DelayedDetonate
	//發送訊息到功能"deadReceiver"
	void DelayedDetonate () {
		BroadcastMessage ("deadReceiver");
	}
	//功能:deadReceiver
	//刪除自己
	//產生爆炸特效、產生替代物
	void deadReceiver () {
		// Destroy ourselves
		Destroy(gameObject);
		
		// Create the explosion
		if (explosion)
			Instantiate (explosion, transform.position, transform.rotation);
		
		// If we have a dead barrel then replace ourselves with it!
		if (deadReplacement) {
			Rigidbody dead = Instantiate(deadReplacement, transform.position, transform.rotation) as Rigidbody;
			
			// For better effect we assign the same velocity to the exploded barrel
			dead.velocity = GetComponent<Rigidbody>().velocity;
			dead.angularVelocity = GetComponent<Rigidbody>().angularVelocity;
		}
		
		// If there is a particle emitter stop emitting and detach so it doesnt get destroyed
		// right away
		ParticleEmitter emitter = GetComponentInChildren<ParticleEmitter>();
		if (emitter) {
			emitter.emit = false;
			emitter.transform.parent = null;
		}
	}

}
