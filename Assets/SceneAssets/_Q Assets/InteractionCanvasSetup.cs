using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class InteractionCanvasSetup : MonoBehaviour {

	public GameObject QInteractiveButton;
	public GameObject OptionButton;

	// Use this for initialization
	void Start () {
		GetComponent<Canvas> ().worldCamera = GameObject.Find ("QCamera").GetComponent<Camera>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
