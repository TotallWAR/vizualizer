using UnityEngine;
using System.Collections;

public class goodsLabel : MonoBehaviour {

	// Use this for initialization
	private static int width = 200;
	private static int height = 240;
	private int screenBoxX  = width;
	private int screenBoxY  = height;

	//private int scrollBoxX  = width * 2;
	private int scrollBoxX  = width-20;
	private int scrollBoxY  = height * 10;

	public Vector2 scrollPosition = Vector2.zero;

	void Start ()
	{
		var devices = WebCamTexture.devices;
		WebCamTexture webcamTexture = new WebCamTexture();

		if (devices.Length > 0) {
			webcamTexture.deviceName = devices[0].name;
			webcamTexture.Play();
		}
	}

	void OnGUI()     
	{
		GUIStyle guiStyle = new GUIStyle();
		guiStyle.fontSize = 15;
		guiStyle.fontStyle = FontStyle.Bold;
		GUI.color = Color.white;
		scrollPosition = GUI.BeginScrollView(new Rect(0, 0, screenBoxX, screenBoxY), scrollPosition, new Rect(0, 0, scrollBoxX, scrollBoxY));
		GUI.skin.textArea.fontSize = 20;
		GUI.skin.textArea.font = Resources.GetBuiltinResource<Font> ("Arial.ttf");
		GUI.TextArea (new Rect (0, 0, width-5, height*10), "ajdflkjasldkfjalskflkasjfkl;asjfklasjklfjsdlkfjlkfjalskjflkasdjflkasjflkasjflkasjdlfkjasldkfjalskdjflkasjdflksajflkas");
		//GUI.skin.textField.fontSize = 35;

		GUI.EndScrollView();
	}
}
