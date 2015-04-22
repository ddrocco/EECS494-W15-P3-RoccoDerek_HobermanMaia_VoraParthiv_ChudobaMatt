using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class IntroJuiceText : MonoBehaviour {
	public Vector3 velocity = Vector3.right;
	public Color baseColor = Color.white;
	Text text;
	float age = 0f;
	float lifetime = 6f;

	void Start() {
		text = GetComponent<Text>();
	}
	
	void Update () {
		age += Time.deltaTime;
		transform.position += velocity * Time.deltaTime;
		float ratio = 1f - 2*Mathf.Abs(0.5f - (age / lifetime));
		text.color = baseColor * ratio;
		if (age > lifetime) {
			Destroy(gameObject);
		}
	}
}
