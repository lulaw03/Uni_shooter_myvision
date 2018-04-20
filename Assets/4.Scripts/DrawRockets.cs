using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class DrawRockets : MonoBehaviour {

	//宣告:火箭彈貼圖材質、XY軸、貼圖寬度
	public Texture2D rocketTexture;
	public float posX;
	public float posY;
	public float rocketWidth = 10f;

	int maxRockets = 0;
	int rocketCount = 20;

	//功能:初始化
	//計算火箭彈GUI長度
	// Use this for initialization
	void Start () {
	
		maxRockets = (int)(rocketTexture.width / rocketWidth);	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	//功能GUI
	//顯示火箭彈GUI
	void OnGUI()
	{
		GUI.BeginGroup(new Rect(posX,Screen.height - posY,rocketTexture.width - ((maxRockets - rocketCount) * rocketWidth), rocketTexture.height));
		GUI.DrawTexture(new Rect(0,0,rocketTexture.width, rocketTexture.height), rocketTexture);
		GUI.EndGroup();
		
	}
	//功能:UpdateRockets
	public void UpdateRockets(int input)
	{
		rocketCount = input;
	}
}
