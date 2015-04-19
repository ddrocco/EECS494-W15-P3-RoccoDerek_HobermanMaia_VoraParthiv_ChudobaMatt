using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CompassScript : MonoBehaviour {

	public QCameraControl qcc;

	public GameObject QCompass;
	public GameObject StanCompass;
	public GameObject StanCamera;

	public GameObject QCompassItem;
	public GameObject StanCompassItem;

	RectTransform QCIRT;
	RectTransform SCIRT;

	// Use this for initialization
	void Start () {
		StanCamera = GameObject.Find ("PlayerCamera");
		QCompass.GetComponent<Canvas> ().worldCamera = GameObject.Find ("QCamera").GetComponent<Camera>();
		StanCompass.GetComponent<Canvas> ().worldCamera = GameObject.Find ("PlayerCanvasCamera").GetComponent<Camera>();
		qcc = GameObject.Find ("QCamera").GetComponent<QCameraControl>();
		QCIRT = QCompassItem.GetComponent<RectTransform> ();
		SCIRT = StanCompassItem.GetComponent<RectTransform> ();
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 eulerrot = SCIRT.localRotation.eulerAngles;
		Vector3 pcamrot = StanCamera.GetComponent<Transform> ().rotation.eulerAngles;
		eulerrot.z = pcamrot.y;
		Quaternion rot = new Quaternion();
		rot.eulerAngles = eulerrot;
		SCIRT.localRotation = rot;

		if(!qcc.enabled || qcc.currentCam == qcc.camOverview){
			Quaternion comprot = new Quaternion();
			comprot.eulerAngles = Vector3.zero;
			QCIRT.localRotation = comprot;
			QCIRT.sizeDelta = new Vector2(40f,40f);
			QCIRT.anchoredPosition3D = new Vector3(20f,20f,0f);
			return;
		}

		pcamrot = qcc.currentCam.gameObject.GetComponent<Transform> ().rotation.eulerAngles;
		eulerrot.x = 60f;
		eulerrot.y = -15f;
		eulerrot.z = pcamrot.y;
		rot = new Quaternion();
		rot.eulerAngles = eulerrot;
		QCIRT.localRotation = rot;
		QCIRT.sizeDelta = new Vector2(60f,60f);
		QCIRT.anchoredPosition3D = new Vector3 (50f, 35f, 0f);
	}
}
