using UnityEngine;
using System.Collections;

// Make sure there is always a character controller
[RequireComponent (typeof (CharacterController))]
public class AI : MonoBehaviour {

	//宣告:速度、旋轉速度、射程、攻擊範圍、射角、禁止攻擊範圍、攻擊延遲、下個移動點、目標
	public float speed = 3.0f;
	public float rotationSpeed = 5.0f;
	public float shootRange = 15.0f;
	public float attackRange = 30.0f;
	public float shootAngle = 4.0f;
	public float dontComeCloserRange = 5.0f;
	public float delayShootTime = 0.35f;
	public float pickNextWaypointDistance = 2.0f;

	public Transform target;

	float lastShot = -10.0f;

	Animation anim;

	//功能:初始化
	//如果沒有目標，找尋遊戲中有"Player"標籤的物件為目標
	//執行功能:Patrol
	// Use this for initialization
	void Start () {
		// Auto setup player as target through tags
		if (target == null && GameObject.FindWithTag("Player"))
			target = GameObject.FindWithTag("Player").transform;

		anim = GetComponent<Animation> ();

		StartCoroutine (Patrol ());
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	//功能:Patrol
	
	IEnumerator Patrol () {

		var curWayPoint = AutoWayPoint.FindClosest(transform.position);

		while (true) {

			var waypointPosition = curWayPoint.transform.position;
			// Are we close to a waypoint? -> pick the next one!
			if (Vector3.Distance(waypointPosition, transform.position) < pickNextWaypointDistance)
				curWayPoint = PickNextWaypoint (curWayPoint);
			
			// Attack the player and wait until
			// - player is killed
			// - player is out of sight			
			if (CanSeeTarget ())
				yield return StartCoroutine(AttackPlayer());
			
			// Move towards our target
			MoveTowards(waypointPosition);
			
			yield return null;
		}
	}

	//功能:CanSeeTarget
	//偵測目標
	bool CanSeeTarget () {

		if (Vector3.Distance(transform.position, target.position) > attackRange)
			return false;
		
		RaycastHit hit;
		if (Physics.Linecast (transform.position, target.position, out hit))
			return hit.transform == target;
		
		return false;
	}
	
	IEnumerator Shoot () {
		// Start shoot animation
		anim.CrossFade("shoot", 0.3f);
		
		// Wait until half the animation has played
		yield return new WaitForSeconds(delayShootTime);
		
		// Fire gun
		BroadcastMessage("Fire", SendMessageOptions.DontRequireReceiver);
		
		// Wait for the rest of the animation to finish
		yield return new WaitForSeconds(anim["shoot"].length - delayShootTime);
	}
	
	IEnumerator AttackPlayer () {

		var lastVisiblePlayerPosition = target.position;

		while (true) {

			if (CanSeeTarget ()) {
				// Target is dead - stop hunting
				if (target == null)
					yield return null;
				
				// Target is too far away - give up	
				var distance = Vector3.Distance(transform.position, target.position);

				if (distance > shootRange * 3)
					yield return null;
				
				lastVisiblePlayerPosition = target.position;
				if (distance > dontComeCloserRange)
					MoveTowards (lastVisiblePlayerPosition);
				else
					RotateTowards(lastVisiblePlayerPosition);
				
				var forward = transform.TransformDirection(Vector3.forward);
				var targetDirection = lastVisiblePlayerPosition - transform.position;
				targetDirection.y = 0;
				
				var angle = Vector3.Angle(targetDirection, forward);
				
				// Start shooting if close and play is in sight
				if (distance < shootRange && angle < shootAngle)
					yield return StartCoroutine(Shoot());
			} 
			else {

				yield return StartCoroutine(SearchPlayer(lastVisiblePlayerPosition));

				// Player not visible anymore - stop attacking
				if (!CanSeeTarget ())
					yield break;
			}
			
			yield return null;
		}
	}
	
	IEnumerator SearchPlayer (Vector3 position) {
		// Run towards the player but after 3 seconds timeout and go back to Patroling
		var timeout = 3.0f;

		while (timeout > 0.0f) {
			MoveTowards(position);
			
			// We found the player
			if (CanSeeTarget ())
				yield return null;
			
			timeout -= Time.deltaTime;
			yield return null;
		}
	}
	
	void RotateTowards (Vector3 position) {
		SendMessage("SetSpeed", 0.0f);
		
		var direction = position - transform.position;
		direction.y = 0;
		if (direction.magnitude < 0.1f)
			return;
		
		// Rotate towards the target
		transform.rotation = Quaternion.Slerp (transform.rotation, Quaternion.LookRotation(direction), rotationSpeed * Time.deltaTime);
		transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
	}
	
	void MoveTowards (Vector3 position) {

		var direction = position - transform.position;
		direction.y = 0;
		if (direction.magnitude < 0.5f) {
			SendMessage("SetSpeed", 0.0f);
			return;
		}
		
		// Rotate towards the target
		transform.rotation = Quaternion.Slerp (transform.rotation, Quaternion.LookRotation(direction), rotationSpeed * Time.deltaTime);
		transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
		
		// Modify speed so we slow down when we are not facing the target
		var forward = transform.TransformDirection(Vector3.forward);
		var speedModifier = Vector3.Dot(forward, direction.normalized);
		speedModifier = Mathf.Clamp01(speedModifier);
		
		// Move the character
		direction = forward * speed * speedModifier;
		GetComponent<CharacterController>().SimpleMove(direction);
		
		SendMessage("SetSpeed", speed * speedModifier, SendMessageOptions.DontRequireReceiver);
	}
	
	AutoWayPoint PickNextWaypoint (AutoWayPoint currentWaypoint) {
		// We want to find the waypoint where the character has to turn the least
		
		// The direction in which we are walking
		var forward = transform.TransformDirection(Vector3.forward);
		
		// The closer two vectors, the larger the dot product will be.
		var best = currentWaypoint;
		var bestDot = -10.0;

		foreach (var cur in currentWaypoint.connected) {

			var direction = Vector3.Normalize(cur.transform.position - transform.position);
			var dot = Vector3.Dot(direction, forward);
			if (dot > bestDot && cur != currentWaypoint) {
				bestDot = dot;
				best = cur;
			}
		}
		
		return best;
	}
}
