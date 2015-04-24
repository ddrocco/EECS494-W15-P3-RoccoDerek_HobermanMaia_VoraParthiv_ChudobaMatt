using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class InteractionCanvasSetup : MonoBehaviour {

	public GameObject QInteractiveButton;
	public GameObject OptionButton;

	void Start () {
		GetComponent<Canvas> ().worldCamera = GameObject.Find ("QCamera").GetComponent<Camera>();
	}
}
