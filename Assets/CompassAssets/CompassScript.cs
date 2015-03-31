using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CompassScript : MonoBehaviour {

	public GameObject QCompass;
	public GameObject StanCompass;
	public GameObject StanCamera;

	public GameObject StanCompassItem;

	// Use this for initialization
	void Start () {
		StanCamera = GameObject.Find ("PlayerCamera");
		QCompass.GetComponent<Canvas> ().worldCamera = GameObject.Find ("QCamera").GetComponent<Camera>();
		StanCompass.GetComponent<Canvas> ().worldCamera = GameObject.Find ("PlayerCamera").GetComponent<Camera>();
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 eulerrot = StanCompassItem.GetComponent<RectTransform>().localRotation.eulerAngles;
		Vector3 pcamrot = StanCamera.GetComponent<Transform> ().rotation.eulerAngles;
		eulerrot.z = pcamrot.y;
		Quaternion rot = new Quaternion();
		rot.eulerAngles = eulerrot;
		StanCompassItem.GetComponent<RectTransform> ().localRotation = rot;
	}
}
