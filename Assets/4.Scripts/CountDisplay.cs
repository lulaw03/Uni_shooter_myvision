using UnityEngine;
using System.Collections;

public class CountDisplay : MonoBehaviour {
    static public int Getcount;
    public AudioClip CollectSound;
	GUIText gui;

	// Use this for initialization
	void Start () {
	
		gui = GetComponent<GUIText> ();
	}
	
	// Update is called once per frame
	void Update () {
	
		gui.text = "BOX Collected: " + BoxCollect.GetCount + "/" + OpenDoor.boxCount;
	}
    void OnControllerColliderHIt(ControllerColliderHit hit)
    {
        if(hit.gameObject.tag == "box")
        {
            hit.gameObject.SendMessage("deadReciver", SendMessageOptions.DontRequireReceiver);
            Destroy(hit.gameObject);
            Getcount++;
            if (CollectSound)
                AudioSource.PlayClipAtPoint(CollectSound, hit.transform.position);
        }
    }
}
