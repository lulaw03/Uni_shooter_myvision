using UnityEngine;
using System.Collections;

[ExecuteInEditMode]//使腳本在編輯模式下就已經執行
public class Radar : MonoBehaviour {

	//宣告:雷達底圖，敵人底圖，雷達中心點物件
	//雷達各項參數:各敵人間隔、偵測範圍、雷達底圖位置、雷達貼圖大小、敵人底圖位置、敵人貼圖大小
	public Texture radarBG;
	public Texture enemyBG; 
	public Transform centerObject; 

	[System.Serializable]
	public class radarParameter{

		public float distScale = 0; 
		public int radarDist = 30;
		public Vector2 mapCenter = new Vector2(0,0);
		public Vector2 mapTexture = new Vector2(256,256); 
		public Vector2 enemyCenter = new Vector2(0,0);
		public Vector2 enemyTexture = new Vector2(20,20);
	}

	public radarParameter Parameter;


	// Use this for initialization
	void Start () {
	
		if (centerObject == null)
			centerObject = GameObject.FindWithTag("Player").transform;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	//功能:GUI
	//如果沒有雷達中心點物件，雷達中心點物件=有Player標籤的物件
	//建立雷達底圖
	void OnGUI () {
		
		var mapCenter = Parameter.mapCenter;
		var mapTexture = Parameter.mapTexture;
		
		GUI.DrawTexture(new Rect(mapCenter.x,mapCenter.y,mapTexture.x,mapTexture.y),radarBG);
		
		a();
	}
	//功能:a
	//計算敵人數量
	void a(){
	
		GameObject[] gos = GameObject.FindGameObjectsWithTag("Enemy");

		if (gos == null)
			return;

		foreach (var go in gos)  
			b(go, enemyBG);  
	}
	//功能:b
	//將所有敵人顯示在雷達偵測範圍內
	void b(GameObject go, Texture texture){

		var distScale = Parameter.distScale;
		var radarDist = Parameter.radarDist;
		var enemyCenter = Parameter.enemyCenter;
		var enemyTexture = Parameter.enemyTexture;   
		var centerPos = centerObject.position;
		
		var extPos = go.transform.position;
		
		var dist = Vector3.Distance(centerPos,extPos);
		
		var dx = centerPos.x - extPos.x; 
		var dz = centerPos.z - extPos.z; 
		var deltay = Mathf.Atan2(dx,dz)*Mathf.Rad2Deg - 270 - centerObject.eulerAngles.y;
		
		var bX = dist * Mathf.Cos(deltay * Mathf.Deg2Rad);
		var bY = dist * Mathf.Sin(deltay * Mathf.Deg2Rad);
		bX = bX * distScale; 
		bY = bY * distScale; 
		
		if(dist <= radarDist)
			GUI.DrawTexture(new Rect(enemyCenter.x + bX, enemyCenter.y + bY, enemyTexture.x, enemyTexture.y), texture);
	}
}
