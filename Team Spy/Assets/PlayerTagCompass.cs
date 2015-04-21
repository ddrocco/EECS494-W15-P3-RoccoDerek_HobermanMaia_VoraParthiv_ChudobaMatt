using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerTagCompass : MonoBehaviour {
	bool isVisible = true;

	public void SetDirection(Quaternion direction) {
		/*if (!DetectTaggedObjects.tagCompassVisible) {
			if (isVisible) {
				GetComponent<Image>().enabled = false;
				isVisible = false;
			}
			return;
		}
		if (!isVisible) {
			GetComponent<Image>().enabled = true;
			isVisible = true;
		}*/
		float angle = direction.eulerAngles.y;
		
		if (FindObjectOfType<PlayerController>().yRotation.x < 0f) {
			angle = 180 - angle;
			print ("Choo choo");
		}
		
		transform.localEulerAngles = Vector3.forward * angle;
		
		Rect canvas = GetComponentInParent<Canvas>().pixelRect;
		float cutoff = canvas.height / canvas.width;
		
		float theta = angle / 180 * Mathf.PI;
		
		Vector2 basePosition =  new Vector2(Mathf.Sin (-theta), Mathf.Cos (theta));
		float magnitude = Mathf.Max(Mathf.Abs(basePosition.x), Mathf.Abs(basePosition.y));
		Vector2 newPosition = new Vector2(canvas.width * 0.43f * basePosition.x,
										canvas.height * 0.32f * basePosition.y) / magnitude;
		Vector2 offset = newPosition.normalized * -50f * Mathf.PingPong(Time.time, .25f);
		GetComponent<RectTransform>().anchoredPosition = newPosition + offset;
	}
}
