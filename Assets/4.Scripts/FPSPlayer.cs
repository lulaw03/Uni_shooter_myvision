using UnityEngine;
using System.Collections;

public class FPSPlayer : MonoBehaviour {

	//宣告:
	//生命值、子彈GUI、火箭彈GUI、血條GUI、受傷聲音、死亡聲音
	public static float maximumHitPoints;

	public float hitPoints = 100.0f;
	public GUIText bulletGUI;
	public DrawRockets rocketGUI;
	public GUITexture healthGUI;
	public AudioClip painLittle;
	public AudioClip painBig;
	public AudioClip die;

	MachineGun machineGun;
	RocketLauncher rocketLauncher;

	float healthGUIWidth = 0.0f;
	float gotHitTimer = -1.0f;

	AudioSource audio;

	Rect hpRect;

	//功能:初始化
	//計算血條長度、執行功能"PlayStepSounds"
	void Awake () {

		maximumHitPoints=hitPoints;
		machineGun = GetComponentInChildren<MachineGun>();
		rocketLauncher = GetComponentInChildren<RocketLauncher>();

		audio = GetComponent<AudioSource> ();

		healthGUIWidth = healthGUI.pixelInset.width;
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	//功能:ApplyDamage(傷害值)
	//撥放受傷音效
	//如果生命值=0，執行功能"Die"
	void ApplyDamage (float damage) {
		if (hitPoints < 0.0f)
			return;
		
		// Apply damage
		hitPoints -= damage;
		
		// Play pain sound when getting hit - but don't play so often
		if (Time.time > gotHitTimer && painBig && painLittle) {
			// Play a big pain sound
			if (hitPoints < maximumHitPoints * 0.2f || damage > 20f) {
				audio.PlayOneShot(painBig, 1.0f / audio.volume);
				gotHitTimer = Time.time + Random.Range(painBig.length * 2, painBig.length * 3);
			} else {
				// Play a small pain sound
				audio.PlayOneShot(painLittle, 1.0f / audio.volume);
				gotHitTimer = Time.time + Random.Range(painLittle.length * 2, painLittle.length * 3);
			}
		}
		
		// Are we dead?
		if (hitPoints < 0.0)
			Die();
	}
	//功能:Die
	//撥放死亡音效
	//發送訊息到功能"DidPause"
	//產生白色畫面後，重新讀取關卡
	void Die () {

		if (die)
			AudioSource.PlayClipAtPoint(die, transform.position);

		GameObject[]  gos = FindObjectsOfType<GameObject>();

		foreach (var go in gos) {
			go.SendMessage ("DidPause", true, SendMessageOptions.DontRequireReceiver);
		}
	
		LevelLoadFade.FadeAndLoadLevel(Application.loadedLevel, Color.white, 2.0f);
	}
	//功能:在Update之後執行
	//執行功能"UpdateGUI"
	void LateUpdate () {
		// Update gui every frame
		// We do this in late update to make sure machine guns etc. were already executed
		UpdateGUI();
	}

	//功能:UpdateGUI
	//更新生命值GUI、子彈GUI、火箭彈GUI的狀況
	void UpdateGUI () {
		// Update health gui
		// The health gui is rendered using a overlay texture which is scaled down based on health
		// - Calculate fraction of how much health we have left (0...1)
		var healthFraction = Mathf.Clamp01(hitPoints / maximumHitPoints);

		hpRect = healthGUI.pixelInset;
		hpRect.xMax = healthGUI.pixelInset.xMin + healthGUIWidth * healthFraction;

		// - Adjust maximum pixel inset based on it
		healthGUI.pixelInset = hpRect;
		
		// Update machine gun gui
		// Machine gun gui is simply drawn with a bullet counter text
		if (machineGun) {
			bulletGUI.text = machineGun.GetBulletsLeft().ToString() + "/" + machineGun.Getclips().ToString();
			
		}
		
		// Update rocket gui
		// This is changed from the tutorial PDF. You need to assign the 20 Rocket textures found in the GUI/Rockets folder
		// to the RocketTextures property.
		if (rocketLauncher)	{
			rocketGUI.UpdateRockets((int)rocketLauncher.ammoCount);
			
		}
	}
}
