using UnityEngine;
using System.Collections;

public class AIAnimation : MonoBehaviour {
	//宣告:最小移動速度
	public float minimumRunSpeed = 1.0f;

	Animation anim;

	// Use this for initialization
	void Start () {
	
		anim = GetComponent<Animation> ();

		// Set all animations to loop
		anim.wrapMode = WrapMode.Loop;
		
		// Except our action animations, Dont loop those
		anim["shoot"].wrapMode = WrapMode.Once;
		
		// Put idle and run in a lower layer. They will only animate if our action animations are not playing
		anim["idle"].layer = -1;
		anim["walk"].layer = -1;
		anim["run"].layer = -1;
		
		anim.Stop();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void SetSpeed (float speed) {

		if (!anim)
			return;

		if (speed > minimumRunSpeed)
			anim.CrossFade("run");
		else
			anim.CrossFade("idle");
	}
}
