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
		isVisible = TagCompassVisible();
		pic.enabled = isVisible;
	
		if (!isVisible) {
			return;
		}
		
		Vector3 toTagVector = detector.taggedObject.transform.position - player.cam.transform.position;
		Vector3 fwd = player.cam.transform.forward;
		Vector3 up = player.cam.transform.up;
		Vector3 right = player.cam.transform.right;
		Debug.DrawRay(player.cam.transform.position, toTagVector, Color.blue);
		Debug.DrawRay(player.cam.transform.position + fwd, toTagVector.normalized, Color.blue);
		Vector3 cross = (toTagVector.normalized - (Vector3.Dot(fwd, toTagVector.normalized) * fwd)).normalized;
		Debug.DrawRay(player.cam.transform.position + player.cam.transform.forward, cross, Color.green);
		
		float x = Vector3.Dot(right, cross);
		float y = Vector3.Dot(up, cross);
			
		float radians = Mathf.Atan2(y, x);
		float angle = (radians / Mathf.PI * 180f) - 90f;
		
		transform.localEulerAngles = Vector3.forward * angle;
		
		Rect canvas = thisCanvas.pixelRect;
		//float cutoff = canvas.height / canvas.width;
				
		float magnitude = Mathf.Max(Mathf.Abs(x), Mathf.Abs(y));
		Vector2 newPosition = new Vector2((canvas.width * 0.5f - 50f) * x,
										(canvas.height * 0.5f - 50f) * y) / magnitude;
		Vector2 offset = newPosition.normalized * -100f * Mathf.PingPong(Time.time, .25f);
		RectTrans.anchoredPosition = newPosition + offset;
	}
	
	bool TagCompassVisible() {
		if (!detector.taggedObjectValid) {
			return false;
		} else {
			Vector3 stanLookDirection = player.cam.transform.forward;
			Vector3 objectDirection = detector.taggedObject.transform.position - player.cam.transform.position;
			if (Vector3.Angle(stanLookDirection, objectDirection) < 27.5f) {
				return false;
			} else {
				return true;
			}
		}
	}
}
