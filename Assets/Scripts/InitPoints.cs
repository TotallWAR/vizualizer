using UnityEngine;
using System.Collections;
using System.IO;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

public class InitPoints : MonoBehaviour {

	public GameObject house;
	public GameObject constructor;
	public GameObject sorter;
	public static PointArray points;
	GameObject marker;
	//public static string info = null;
	public static float cooldownMessOfGoods = 8.0f;
	void Start () {
		
		marker =  GameObject.Find ("Marker Scene 1 - ARTrackedObject");
		InitHouses ("/Users/aleksandrvadimovic/Dropbox/University/7_term/распределенные системы/Delivery/New Unity Project/Assets/Scripts/config.txt");
		var devices = WebCamTexture.devices;
		WebCamTexture webcamTexture = new WebCamTexture();

		if (devices.Length > 0) {
			webcamTexture.deviceName = devices[0].name;
			webcamTexture.Play();
		}

	}

	void Update () {
		
	}

	public static void SetLayerRecursively(GameObject obj, int newLayer)
	{
		if (null == obj)
		{
			return;
		}

		obj.layer = newLayer;

		foreach (Transform child in obj.transform)
		{
			if (null == child)
			{
				continue;
			}
			SetLayerRecursively(child.gameObject, newLayer);
		}
	}

	void InitHouses (string filename)
	{

		using (StreamReader sr = new StreamReader (filename)) {
			//парсинг конфига точек
			String line = "{\"Points\":" + sr.ReadToEnd () + "}";
			points = JsonUtility.FromJson<PointArray> (line);
		}
		//построение точек на карте
		int i = 1;
		foreach (var item in points.Points) {
			//point
			GameObject house = GameObject.Instantiate (this.house) as GameObject;
			house.name = "House" + i;
			house.transform.localPosition = new Vector3((item.X*0.001f), (item.Y*0.001f), -0.001f);
			house.transform.Rotate(new Vector3(-90, 0, 0));
			house.transform.localScale = new Vector3 (0.0060f, 0.0060f, 0.0060f);
			house.transform.SetParent(marker.transform);


			house.AddComponent<BoxCollider>();
			BoxCollider boxColl = house.GetComponent<BoxCollider>();
			boxColl.size = new Vector3(10,10,10);
			house.AddComponent<ShowGoods>();



			house.layer = LayerMask.NameToLayer ("AR foreground");
			SetLayerRecursively (house, house.layer);

			//constructor/sort
			if (item.Constructor) {
				GameObject constructor = GameObject.Instantiate (this.constructor) as GameObject;
				constructor.transform.localPosition = new Vector3((item.X*0.001f), (item.Y*0.001f), -0.0095f*4.3f);
				constructor.transform.Rotate(new Vector3(-90, 0, 0));
				constructor.transform.localScale = new Vector3 (24*0.001f, 24*0.001f, 24*0.001f);
				constructor.transform.SetParent(house.transform);


				constructor.layer = LayerMask.NameToLayer ("AR foreground");
				SetLayerRecursively (constructor, constructor.layer);

			}
			if (item.Sorter) {
				GameObject sorter = GameObject.Instantiate (this.sorter) as GameObject;
				sorter.transform.localPosition = new Vector3((item.X*0.001f), (item.Y*0.001f), -0.0095f*4.3f);
				sorter.transform.Rotate(new Vector3(-90, 0, 0));
				sorter.transform.localScale = new Vector3 (24*0.001f, 24*0.001f, 24*0.001f);
				sorter.transform.SetParent(house.transform);


				sorter.layer = LayerMask.NameToLayer ("AR foreground");
				SetLayerRecursively (sorter, sorter.layer);
			}
			i++;
		}
	}
}

[System.Serializable]
public class PointArray
{
	public Point[] Points = null;
}

[System.Serializable]
public class Point
{
	public int Id = 0;
	public float X = 0;
	public float Y = 0;
	public _Type Type = null;
	public bool Constructor = false;       
	public bool Sorter = false;
}
[System.Serializable]
public class _Type
{
	public bool Auto = false;
	public bool Train = false;
	public bool Plane = false;
}