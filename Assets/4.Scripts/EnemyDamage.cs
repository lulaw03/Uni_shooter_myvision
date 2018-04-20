using UnityEngine;
using System.Collections;

public class EnemyDamage : MonoBehaviour {

	//宣告:生命值、替代物、死亡音效、血條貼圖材質
	public float hitPoints = 300.0f;

	public Transform deadReplacement;
	public AudioClip dieSound;
	public Texture2D enemyblood;

	float maxHP;
	float GUIDisplayTime;//GUI顯示的時間

	Camera cam;

	// Use this for initialization
	void Start () {
	
		maxHP = hitPoints;
		cam = Camera.main;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	//功能:ApplyDamage(傷害值)
	//如果生命值=0，發送訊息到功能"deadReceiver"
	void ApplyDamage (float damage) {
		GUIDisplayTime = 1;
		// We already have less than 0 hitpoints, maybe we got killed already?
		if (hitPoints <= 0.0f)
			return;

		if(enabled)	
			hitPoints -= damage;

		if (hitPoints <= 0.0f)
			BroadcastMessage ("deadReceiver", SendMessageOptions.DontRequireReceiver);
	}
	//功能:deadReceiver
	//刪除自己、撥放死亡音效、產生一個替代物
	void deadReceiver () {
		
		// Destroy ourselves
		Destroy(gameObject);
		
		// Play a dying audio clip
		if (dieSound)
			AudioSource.PlayClipAtPoint(dieSound, transform.position);
		
		// Replace ourselves with the dead body
		if (deadReplacement) {
			Transform dead = Instantiate(deadReplacement, transform.position, transform.rotation) as Transform;
			
			// Copy position & rotation from the old hierarchy into the dead replacement
			CopyTransformsRecurse(transform, dead);
			
		}
	}
	
	void OnGUI(){

		if (!cam)
			return;

		//GUI顯示時間
		GUIDisplayTime 	-= Time.deltaTime;
		
		if (GUIDisplayTime < 0) return;
		
		var HPRatioh = Mathf.Clamp01(hitPoints / maxHP);		

		Vector3 ac = cam.WorldToScreenPoint(gameObject.transform.position);
		float y = ac.y;
		y = Screen.height-y;
		ac.y = y;
		Rect myRect = new Rect(ac.x-27, ac.y-20, 100 * HPRatioh, 5);

		GUI.DrawTexture(myRect, enemyblood, ScaleMode.ScaleAndCrop, false, 0);
	}
	
	static void CopyTransformsRecurse (Transform src, Transform dst) {
		
		dst.position = src.position;
		dst.rotation = src.rotation;

		for (int i = 0; i < dst.childCount; i++) {
			// Match the transform with the same name
			var curSrc = src.Find(dst.GetChild(i).name);

			if (curSrc)
				CopyTransformsRecurse(curSrc, dst.GetChild(i));
		}
	}
}
