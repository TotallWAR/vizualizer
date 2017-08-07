using UnityEngine;
using System.Collections;
using System;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Threading;
using System.Collections.Generic;
public class GetMQCar : MonoBehaviour {

	public GameObject car;
	public List<GameObject> carList;
	public static List<Car> carListNotGameObj;
	Car curCar;
	GameObject marker;
	GameObject carGameObj;

	const float timeOut = 8.0f;
	public static float targetTime = timeOut;

	void Start () {
		carList = new List<GameObject>();
		carListNotGameObj = new List<Car> ();
		marker =  GameObject.Find ("Marker Scene 1 - ARTrackedObject");

		InitCars ();

		WatchMQCars();

	}
	void Update () {

		targetTime -= Time.deltaTime;
		if (carListNotGameObj != null) {
			for (int i = 0; i < carListNotGameObj.Count; i++) {
				carGameObj = carList.Find (q => q.name == "Car" + carListNotGameObj [i].Id);
				if (targetTime > 0.0f) {

					if (carListNotGameObj [i].isStartPosition == true) {
						carGameObj.transform.localPosition = new Vector3 (carListNotGameObj [i].X * 0.001f, carListNotGameObj [i].Y * 0.001f, -0.001f);
						carListNotGameObj [i].isStartPosition = false;
					}
//				var step = curCar.Speed * 0.125f;
					//				trainGameObj.transform.Translate(new Vector3(curTrain.X*0.001f, curTrain.Y*0.001f, -0.1f) * step);
//				carGameObj.transform.localPosition += new Vector3(carListNotGameObj[i].X*0.001f, carListNotGameObj[i].Y*0.001f, 0) * step;
//				carGameObj.transform.rotation = new Quaternion (carListNotGameObj[i].X * 0.001f, carListNotGameObj[i].Y * 0.001f, 0f, 0f);
//				carGameObj.transform.Translate(new Vector3 (carListNotGameObj[i].X * 0.001f, carListNotGameObj[i].Y * 0.001f, carListNotGameObj[i].Z) * Time.deltaTime*1500f);
					carGameObj.transform.localPosition = new Vector3 (carListNotGameObj [i].X * 0.001f, carListNotGameObj [i].Y * 0.001f, -0.001f);

				} else {
					carGameObj.transform.localPosition = new Vector3 (carListNotGameObj [i].X * 0.001f, carListNotGameObj [i].Y * 0.001f, -10f);
					carListNotGameObj.Remove (carListNotGameObj [i]);
					Debug.LogError ("Car-Component has stooped sending message");
					break;
				}
			}
		}


	}

	void InitCars()
	{
		for (int i = 1; i < 11; i++) {
			GameObject car = GameObject.Instantiate (this.car) as GameObject;
			car.name = "Car" + i;
			car.transform.localPosition = new Vector3(0.001f, 0.001f, -10f);
			car.transform.Rotate(new Vector3(-90, 0, 0));
			car.transform.localScale = new Vector3 (0.016f, 0.016f, 0.016f);
			car.transform.SetParent(marker.transform);
			car.AddComponent<BoxCollider>();
			BoxCollider boxColl = car.GetComponent<BoxCollider>();
			boxColl.size = new Vector3(10,10,10);
			car.AddComponent<ShowGoods>();
			car.layer = LayerMask.NameToLayer ("AR foreground");
			InitPoints.SetLayerRecursively (car, car.layer);
			carList.Add (car);
		}
	}

	void WatchMQCars()
	{
		ConnectionFactory factory = new ConnectionFactory () {
			Uri = "amqp://46.101.211.105",
			UserName = "test",
			Password = "test"
		};
		IConnection connection = factory.CreateConnection ();
		IModel channel = connection.CreateModel ();

		channel.QueueDeclare (queue: "car",
			durable: false,
			exclusive: false,
			autoDelete: false,
			arguments: null);

		EventingBasicConsumer consumer = new EventingBasicConsumer (channel);
		consumer.Received += (model, ea) => {
			targetTime = timeOut;

			var body = ea.Body;
			var message = Encoding.UTF8.GetString (body);
			curCar = JsonUtility.FromJson<Car> (message);
			var obj = carListNotGameObj.Find(q => q.Id == curCar.Id);
			if (obj == null)
				carListNotGameObj.Add(curCar);
			else
			{
				obj.X = curCar.X;
				obj.Y = curCar.Y;
				obj.Goods = curCar.Goods;
			}


			Debug.Log ("Car Id: " + curCar.Id + " X: " + curCar.X + "Y: " + curCar.Y + "\n");

		};
		channel.BasicConsume (queue: "car",
			noAck: true,
			consumer: consumer);
		//connection.Close ();
	}

}


[System.Serializable]
public class Car: Transport
{
	
}