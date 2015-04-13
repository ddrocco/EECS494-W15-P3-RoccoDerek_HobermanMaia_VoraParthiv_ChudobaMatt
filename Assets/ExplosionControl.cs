using UnityEngine;
using System.Collections;

public class ExplosionControl : MonoBehaviour {
	public static ExplosionControl Instance;
	public ParticleSystem smoke;
	public ParticleSystem fire;
	
	void Awake() {
		if (Instance != null) {
			Debug.LogError ("Multiple instances of ExplosionControl!");
		}
		Instance = this;
	}
	
	public void Explosion(Vector3 position) {
		instantiate (smoke, position);
		instantiate (fire, position);
	}
	
	private ParticleSystem instantiate(ParticleSystem prefab, Vector3 pos) {
		ParticleSystem newSystem = Instantiate(prefab, pos,
		                                       Quaternion.identity)
		                                       as ParticleSystem;
		Destroy (newSystem.gameObject, newSystem.startLifetime);
		return newSystem;
	}
}
