using UnityEngine;
using System.Collections;

public class MachineGun : MonoBehaviour {

	//宣告:
	//射程、射擊頻率、推力、傷害值、子彈數量、彈匣數量、裝填時間、火花特效、槍口閃光、下次射擊時間
	public float range = 100.0f;
	public float fireRate = 0.05f;
	public float force = 10.0f;
	public float damage = 5.0f;
	public int bulletsPerClip = 40;
	public int clips = 20;
	public float reloadTime = 0.5f;

	ParticleEmitter hitParticles;

	public Renderer muzzleFlash;

	int bulletsLeft = 0;
	float nextFireTime;
	int m_LastFrameShot = -1;

	//功能:遊戲初始化
	//獲取子物件內的分子特效並隱藏、定義子彈初始數量
	// Use this for initialization
	void Start () {

		hitParticles = GetComponentInChildren<ParticleEmitter>();
		// We don't want to emit particles all the time, only when we hit something.
		if (hitParticles)
			hitParticles.emit = false;
		bulletsLeft = bulletsPerClip;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	//功能:在Update函數調用後被調用
	//如果正在射擊，槍口閃光開啟、撥放音效、音效循環開啟，反之，槍口閃光關閉、音效循環關閉
	void LateUpdate() {

		if (muzzleFlash) {
			// We shot this frame, enable the muzzle flash
			if (m_LastFrameShot == Time.frameCount) {
				muzzleFlash.transform.localRotation = Quaternion.AngleAxis(Random.value * 360, Vector3.forward);
				muzzleFlash.enabled = true;
				if (GetComponent<AudioSource>()) {
					if (!GetComponent<AudioSource>().isPlaying)
						GetComponent<AudioSource>().Play();
					GetComponent<AudioSource>().loop = true;
				}
			}
			else {
				// We didn't, disable the muzzle flash
				muzzleFlash.enabled = false;
				enabled = false;
				// Play sound
				if (GetComponent<AudioSource>())
					GetComponent<AudioSource>().loop = false;	
			}
		}
	}
	//功能:Fire
	//如果子彈=0，執行功能"Reload"
	//迴圈，子彈不等於0且下次射擊時間<遊戲時間，執行功能"FireOneShot"
	void Fire () {
		// Reload gun in reload Time	
		if (bulletsLeft == 0)
			StartCoroutine (Reload ());
		// If there is more than one bullet between the last and this frame
		// Reset the nextFireTime
		if (Time.time - fireRate > nextFireTime)
			nextFireTime = Time.time - Time.deltaTime;
		// Keep firing until we used up the fire time
		while( nextFireTime < Time.time && bulletsLeft != 0) {
			FireOneShot();
			nextFireTime += fireRate;
		}	
	}
	//功能:FireOneShot
	//如果射到鋼體物件，傳達子彈力量給鋼體物件
	//在彈著點產生火花
	//對射擊到的物件傳達傷害值(要接受傷害值的物件腳本上需要有功能"ApplyDamage")
	//子彈數量減少
	void FireOneShot () {
		var direction = transform.TransformDirection(Vector3.forward);
		RaycastHit hit;
		// Did we hit anything?
		if (Physics.Raycast (transform.position, direction, out hit, range)) {
			// Apply a force to the rigidbody we hit
			if (hit.rigidbody != null)
				hit.rigidbody.AddForceAtPosition(force * direction, hit.point);
			// Place the particle system for spawing out of place where we hit the surface!
			// And spawn a couple of particles
			if (hitParticles) {
				hitParticles.transform.position = hit.point;
				hitParticles.transform.rotation = Quaternion.FromToRotation(Vector3.up, hit.normal);
				hitParticles.Emit();
			}
			// Send a damage message to the hit object			
			hit.collider.SendMessageUpwards("ApplyDamage", damage, SendMessageOptions.DontRequireReceiver);
		}
		bulletsLeft--;
		// Register that we shot this frame,
		// so that the LateUpdate function enabled the muzzleflash renderer for one frame
		m_LastFrameShot = Time.frameCount;
		enabled = true;
	}
	//功能:Reload
	//如果彈匣>0且子彈=0，彈匣減少、子彈=初始設定值
	IEnumerator Reload () {

		// Wait for reload time first - then add more bullets!
		yield return new WaitForSeconds(reloadTime);
		// We have a clip left reload
		if (clips > 0 && bulletsLeft == 0) {
			clips--;
			bulletsLeft = bulletsPerClip;
		}
	}
	//功能:GetBulletsLeft(給FPSPlayer腳本的子彈GUI內的參數)
	public int GetBulletsLeft () {
		return bulletsLeft;
	}
	
	public int Getclips () {
		return clips;
	}
}
