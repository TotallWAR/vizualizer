using UnityEngine;
using System.Collections;
using System;
using System.Linq;
public class ShowGoods : MonoBehaviour {
	int id;
	bool showGoods;
	float coolDownShowGood;
	private GUIStyle guiStyle;
	string nameofGameObj;
	public float cooldownMessageOfGoods = InitPoints.cooldownMessOfGoods;
	Transport curTransport;
	GameObject house;
	public string info = null;
	public bool isShowHouse = false;


	// Use this for initialization
	private static int width = 200*2;
	private static int height = 240;
	private int screenBoxX  = width*2;
	private int screenBoxY  = height;

	//private int scrollBoxX  = width * 2;
	private int scrollBoxX  = width-20;
	private int scrollBoxY  = height * 10-5;

	public Vector2 scrollPosition = Vector2.zero;


	// Use this for initialization
	void Start () {
		this.id = -1;
		showGoods = false;
		coolDownShowGood = 0;
		GUI.color = Color.white;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnMouseDown() {
		var s = this.name.TakeWhile(c => !Char.IsDigit(c))
			.ToArray();
		nameofGameObj = new string(s);
		var showGoodsObjects = GetComponents<ShowGoods> ();
		foreach (var item in showGoodsObjects) {
			item.info = null;
		}

		if (nameofGameObj == "House") {
			isShowHouse = true;
			coolDownShowGood = cooldownMessageOfGoods;
		}
			

		switch (nameofGameObj) {
			case "Train":
			{
				var digits = this.name.SkipWhile (c => !Char.IsDigit (c))
				.TakeWhile (Char.IsDigit)
				.ToArray ();
				var str = new string (digits);
				id = int.Parse (str);
				foreach (var item in GetMQTrain.trainListNotGameObj) {
					if (id == item.Id) {
						curTransport = item as Transport;
						showGoods = true;
						coolDownShowGood = cooldownMessageOfGoods;
					}
				}
				break;
			}
			case "Plane":
			{
				var digits = this.name.SkipWhile(c => !Char.IsDigit(c))
					.TakeWhile(Char.IsDigit)
					.ToArray();
				var str = new string(digits);
				id = int.Parse(str);
				foreach (var item in GetMQPlane.planeListNotGameObj) {
					if (id == item.Id) {
						curTransport = item as Transport;
						showGoods = true;
						coolDownShowGood = cooldownMessageOfGoods;
					}
				}
				break;
			}
			case "Car":
			{
				var digits = this.name.SkipWhile (c => !Char.IsDigit (c))
				.TakeWhile (Char.IsDigit)
				.ToArray ();
				var str = new string (digits);
				id = int.Parse (str);
				foreach (var item in GetMQCar.carListNotGameObj) {
					if (id == item.Id) {
						curTransport = item as Transport;
						showGoods = true;
						coolDownShowGood = cooldownMessageOfGoods;
					}
				}
				break;
			}
		}

	}

	void OnGUI ()
	{
		if (showGoods && coolDownShowGood > 0) {
			
			//Train obj = GetMQTrain.trainListNotGameObj.Find (q => q.Id == this.id);
			if (curTransport != null) {
				int i = 1;
				this.info = "";
				this.info += "Transport: " + this.name + "\n";
				foreach (var item in curTransport.Goods) {
					this.info += "Id товара " + item.Id + ": Name: " + item.Name + " Weight: " + item.mass + " Status: " + item.Status + " Urgency: " + item.Srok + "\n";
					i++;
				}


				if (this.info != null) {
					scrollPosition = GUI.BeginScrollView (new Rect (0, 0, screenBoxX, screenBoxY),
						scrollPosition, new Rect (0, 0, scrollBoxX, scrollBoxY));
					GUI.skin.textArea.fontSize = 20;
					GUI.TextArea (new Rect (0, 0, width * 2, height * 10), this.info);
					GUI.EndScrollView ();
				}
			}

			switch (nameofGameObj) {
				case "Train":
					if (GetMQTrain.targetTime <= 0) {
						showGoods = false;
					}
					coolDownShowGood -= Time.deltaTime;
						break;

				case "Plane":
					if (GetMQPlane.targetTime <= 0) {
						showGoods = false;
					}
					coolDownShowGood -= Time.deltaTime;
					break;

				case "Car":
					if (GetMQCar.targetTime <= 0) {
						showGoods = false;
					}
					coolDownShowGood -= Time.deltaTime;
					break;
			}
		}

		if (isShowHouse && coolDownShowGood > 0) {
			this.info = "";
			this.info += "Point: " + this.name + "\n";
			scrollPosition = GUI.BeginScrollView (new Rect (0, 0, screenBoxX, screenBoxY),
				scrollPosition, new Rect (0, 0, scrollBoxX, scrollBoxY));
			GUI.skin.textArea.fontSize = 20;
			GUI.TextArea (new Rect (0, 0, width * 2, height * 10), this.info);
			GUI.EndScrollView ();
			coolDownShowGood -= Time.deltaTime;
		}
	}
}
