using UnityEngine;
using System.Collections;
using System;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Threading;
using System.Collections.Generic;
public class GetMQPlane : MonoBehaviour {

	public GameObject plane;
	public List<GameObject> planeList;
	public static List<Plane> planeListNotGameObj;
	Plane curPlane;
	GameObject marker;
	GameObject planeGameObj;

	const float timeOut = 15.0f;
	public static float targetTime = timeOut;

	void Start () {
		planeList = new List<GameObject>();
		planeListNotGameObj = new List<Plane> ();
		marker =  GameObject.Find ("Marker Scene 1 - ARTrackedObject");

		InitPlanes ();

		WatchMQPlanes();

	}
	void Update () {

		targetTime -= Time.deltaTime;
		if (planeListNotGameObj != null) {
			for (int i = 0; i < planeListNotGameObj.Count; i++) {

				planeGameObj = planeList.Find (q => q.name == "Plane" + planeListNotGameObj [i].Id);

				if (targetTime > 0.0f) {

					if (planeListNotGameObj [i].isStartPosition == true) {
						planeGameObj.transform.localPosition = new Vector3 (planeListNotGameObj [i].X * 0.001f, planeListNotGameObj [i].Y * 0.001f, planeListNotGameObj [i].Z);
						planeListNotGameObj [i].isStartPosition = false;
					}
//				var step = curPlane.Speed * 0.125f;
					//				trainGameObj.transform.Translate(new Vector3(curTrain.X*0.001f, curTrain.Y*0.001f, -0.1f) * step);
//				planeGameObj.transform.localPosition += new Vector3(planeListNotGameObj[i].X*0.001f, planeListNotGameObj[i].Y*0.001f, 0) * step;
//				planeGameObj.transform.rotation = new Quaternion (planeListNotGameObj[i].X * 0.001f, planeListNotGameObj[i].Y * 0.001f, 0f, 0f);
					planeGameObj.transform.localPosition = new Vector3 (planeListNotGameObj [i].X * 0.001f, planeListNotGameObj [i].Y * 0.001f, planeListNotGameObj [i].Z);
//				planeGameObj.transform.Translate(new Vector3 (planeListNotGameObj[i].X * 0.001f, planeListNotGameObj[i].Y * 0.001f, planeListNotGameObj[i].Z) * Time.deltaTime*1500f);

				} else {
					planeGameObj.transform.localPosition = new Vector3 (planeListNotGameObj [i].X * 0.001f, planeListNotGameObj [i].Y * 0.001f, -10f);
					planeListNotGameObj.Remove (planeListNotGameObj [i]);
					Debug.LogError ("Plane-Component has stooped sending message");
					break;
				}
			}
		}


	}

	void InitPlanes()
	{
		for (int i = 1; i < 3; i++) {
			GameObject plane = GameObject.Instantiate (this.plane) as GameObject;
			plane.name = "Plane" + i;
			plane.transform.localPosition = new Vector3(-0.0129f, 0.0106f, -10f);
			plane.transform.Rotate(new Vector3(-90, 0, 0));
			plane.transform.localScale = new Vector3 (0.008f, 0.008f, 0.008f);
			plane.transform.SetParent(marker.transform);
			plane.AddComponent<BoxCollider>();
			BoxCollider boxColl = plane.GetComponent<BoxCollider>();
			boxColl.size = new Vector3(10,10,10);
			plane.AddComponent<ShowGoods>();
			plane.layer = LayerMask.NameToLayer ("AR foreground");
			InitPoints.SetLayerRecursively (plane, plane.layer);
			planeList.Add (plane);
		}
	}

	void WatchMQPlanes()
	{
		ConnectionFactory factory = new ConnectionFactory () {
			Uri = "amqp://46.101.211.105",
			UserName = "test",
			Password = "test"
		};
		IConnection connection = factory.CreateConnection ();
		IModel channel = connection.CreateModel ();

		channel.QueueDeclare (queue: "plane",
			durable: false,
			exclusive: false,
			autoDelete: false,
			arguments: null);

		EventingBasicConsumer consumer = new EventingBasicConsumer (channel);
		consumer.Received += (model, ea) => {
			targetTime = timeOut;

			var body = ea.Body;
			var message = Encoding.UTF8.GetString (body);
			curPlane = JsonUtility.FromJson<Plane> (message);
			var obj = planeListNotGameObj.Find(q => q.Id == curPlane.Id);
			if (obj == null)
				planeListNotGameObj.Add(curPlane);
			else
			{
				obj.X = curPlane.X;
				obj.Y = curPlane.Y;
				obj.Z = curPlane.Z;

				obj.Goods = curPlane.Goods;
			}


			Debug.Log ("Plane Id: " + curPlane.Id + " X: " + curPlane.X + " Y: " + curPlane.Y + " Z: " + curPlane.Z + "\n");
			foreach (var item in curPlane.Goods) {
				Debug.Log("Goods: " + item.Name + "; ");
			}

		};
		channel.BasicConsume (queue: "plane",
			noAck: true,
			consumer: consumer);
		//connection.Close ();
	}

}


[System.Serializable]
public class Plane: Transport
{

}