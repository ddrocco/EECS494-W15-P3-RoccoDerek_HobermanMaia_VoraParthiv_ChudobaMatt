using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerTagCompass : MonoBehaviour {
	public bool isVisible = false;
	public Image pic = null;
	public Canvas thisCanvas = null;
	public RectTransform RectTrans = null;
	public PlayerController player = null;
	public DetectTaggedObjects detector = null;
	
	void Start () {
		pic = GetComponent<Image>();
		thisCanvas = GetComponentInParent<Canvas>();
		RectTrans = GetComponent<RectTransform>();
		player = FindObjectOfType<PlayerController>();
		detector = FindObjectOfType<DetectTaggedObjects>();
		pic.enabled = false;
	}
	
	int numframes = 0;
	void FixedUpdate() {
		++numframes;
	}
	
	public void SetDirection(Quaternion direction) {
		print (numframes);
		isVisible = detector.tagCompassVisible;
		pic.enabled = isVisible;
		if (!isVisible) {
			return;
		}
		float angle = direction.eulerAngles.y;
		
		//SWAP AT z = 90f, -90f
		if (player.yRotation.x < 0f) {
			angle = 180 - angle;
		}
		
		transform.localEulerAngles = Vector3.forward * angle;
		
		Rect canvas = thisCanvas.pixelRect;
		//float cutoff = canvas.height / canvas.width;
		
		float theta = angle / 180 * Mathf.PI;
		
		Vector2 basePosition =  new Vector2(Mathf.Sin (-theta), Mathf.Cos (theta));
		float magnitude = Mathf.Max(Mathf.Abs(basePosition.x), Mathf.Abs(basePosition.y));
		Vector2 newPosition = new Vector2(canvas.width * 0.43f * basePosition.x,
										canvas.height * 0.32f * basePosition.y) / magnitude;
		Vector2 offset = newPosition.normalized * -50f * Mathf.PingPong(Time.time, .25f);
		RectTrans.anchoredPosition = newPosition + offset;
	}
}
