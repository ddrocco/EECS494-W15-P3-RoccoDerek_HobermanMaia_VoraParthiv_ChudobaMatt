using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class QPlayerVisibilityToggle : MonoBehaviour {
	public Renderer stanRenderer;
	Renderer arrowRenderer;
	
	void Start() {
		stanRenderer = FindObjectOfType<N_PlayerWireframe>().GetComponent<Renderer>();
		stanRenderer.enabled = false;
		arrowRenderer = stanRenderer.GetComponentInChildren<SpriteRenderer>();
		arrowRenderer.enabled = false;
		GetComponent<Image>().color = Color.gray;
	}
	
	void Update () {
		if (Input.GetKeyDown(KeyCode.P)) {
			if (stanRenderer.enabled) {
				stanRenderer.enabled = false;
				arrowRenderer.enabled = false;
				GetComponent<Image>().color = Color.gray;
			} else {
				stanRenderer.enabled = true;
				arrowRenderer.enabled = true;
				GetComponent<Image>().color = Color.green;
			}
		}
	}
}
