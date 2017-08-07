using UnityEngine;
using System.Collections;
using System;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Threading;
using System.Collections.Generic;
public class GetMQTrain : MonoBehaviour {
	
	public GameObject train;
	public float speed = 150f;
	public List<GameObject> trainList;
	public static List<Train> trainListNotGameObj;
	Train curTrain;
	GameObject marker;
	GameObject trainGameObj;
	const float timeOut = 8.0f;
	public static float targetTime = timeOut;

	void Start () {
		trainList = new List<GameObject>();
		trainListNotGameObj = new List<Train> ();
		marker =  GameObject.Find ("Marker Scene 1 - ARTrackedObject");

		InitTrains ();

		WatchMQTrain();

	}
	void Update () {
		
		targetTime -= Time.deltaTime;

		if (trainListNotGameObj != null) {
			for (int i = 0; i < trainListNotGameObj.Count; i++) {
			
				trainGameObj = trainList.Find (q => q.name == "Train" + trainListNotGameObj [i].Id);
				if (targetTime > 0.0f) {
					if (trainListNotGameObj [i].isStartPosition == true) {
						trainGameObj.transform.localPosition = new Vector3 (trainListNotGameObj [i].X * 0.001f, trainListNotGameObj [i].Y * 0.001f, -0.001f);
						trainListNotGameObj [i].isStartPosition = false;
					}
//				var step = curTrain.Speed * 0.00125f;
					//var step = trainListNotGameObj[i].Speed * 0.125f;
					//				trainGameObj.transform.Translate(new Vector3(curTrain.X*0.001f, curTrain.Y*0.001f, -0.1f) * step);
					//trainGameObj.transform.Rotate (trainListNotGameObj[i].X, trainListNotGameObj[i].Y, 0f) * Time.deltaTime;// = new Quaternion (trainListNotGameObj[i].X * 0.001f, trainListNotGameObj[i].Y * 0.001f, 0f, 0f);
					//trainGameObj.transform.Translate(new Vector3 (trainListNotGameObj[i].X, trainListNotGameObj[i].Y, 0));

					trainGameObj.transform.position = new Vector3 (trainListNotGameObj [i].X * 0.001f, trainListNotGameObj [i].Y * 0.001f, -0.001f);
				} else {
					trainGameObj.transform.localPosition = new Vector3 (trainListNotGameObj [i].X * 0.001f, trainListNotGameObj [i].Y * 0.001f, -10f);
					trainListNotGameObj.Remove (trainListNotGameObj [i]);
					Debug.LogError ("Train-Component has stooped sending message");
					break;
				}
			}
		}
	}

	void InitTrains ()
	{
		for (int i = 1; i < 3; i++) {
			GameObject train = GameObject.Instantiate (this.train) as GameObject;
			train.name = "Train" + i;
			train.transform.localPosition = new Vector3(0.001f, 0.001f, -10f);
			train.transform.Rotate(new Vector3(-90, 0, 0));
			train.transform.localScale = new Vector3 (0.004f, 0.004f, 0.004f);
			train.transform.SetParent(marker.transform);
			train.AddComponent<BoxCollider>();
			BoxCollider boxColl = train.GetComponent<BoxCollider>();
			boxColl.size = new Vector3(10,10,10);
			train.AddComponent<ShowGoods>();
			train.layer = LayerMask.NameToLayer ("AR foreground");
			InitPoints.SetLayerRecursively (train, train.layer);
			trainList.Add (train);
		}
	}

	void WatchMQTrain()
	{
		ConnectionFactory factory = new ConnectionFactory () {
			Uri = "amqp://46.101.211.105",
			UserName = "test",
			Password = "test"
		};
		IConnection connection = factory.CreateConnection ();
		IModel channel = connection.CreateModel ();

		channel.QueueDeclare (queue: "train",
			durable: false,
			exclusive: false,
			autoDelete: false,
			arguments: null);

		EventingBasicConsumer consumer = new EventingBasicConsumer (channel);
		consumer.Received += (model, ea) => {
			targetTime = timeOut;

			var body = ea.Body;
			var message = Encoding.UTF8.GetString (body);
			curTrain = JsonUtility.FromJson<Train> (message);

			var obj = trainListNotGameObj.Find(q => q.Id == curTrain.Id);
			if (obj == null)
				trainListNotGameObj.Add(curTrain);
			else
			{
				obj.X = curTrain.X;
				obj.Y = curTrain.Y;
				obj.Goods = curTrain.Goods;
			}

			Debug.Log ("Train Id: " + curTrain.Id + " X: " + curTrain.X + "Y: " + curTrain.Y + "\n");

		};
		channel.BasicConsume (queue: "train",
			noAck: true,
			consumer: consumer);
		//connection.Close ();
	}

}


[System.Serializable]
public class Train: Transport
{
	
}

[System.Serializable]
public class Good
{
	public int Id = 0;
	public string Name = null;
	public int mass = 0;
	public int GoodsSetId = 0;
	public string Status = null;
	public int Srok = 0;
	//public int SendTo = 0;
}

public class Transport
{
	public int Id = 1;
	public float X = 0;
	public float Y = 0;
	public float Z = 0;
	public float Speed = 0;
	public List<Good> Goods = null;
	public bool isStartPosition = true;
}