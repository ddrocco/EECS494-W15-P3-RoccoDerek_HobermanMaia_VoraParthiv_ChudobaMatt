using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerInteraction : MonoBehaviour
{
	private Camera cam;
	private GameObject reticle;
	private Image reticleRender;
	private Color reticleNormal;
	private Color reticleInteract;
	private PlayerController player;

	public float detectionDistance;
	private int cullingMask;

	void Awake()
	{
		cam = GetComponent<Camera>();
		reticle = GameObject.Find("Reticle");
		player = GameObject.Find("Player").GetComponent<PlayerController>();

		reticleRender = reticle.GetComponent<Image>();
		reticleNormal = Color.black;
		reticleNormal.a = 0.5f;
		reticleInteract = Color.red;
		reticleInteract.a = 0.5f;
		
		cullingMask = (1 << Layerdefs.interactable) + (1 << Layerdefs.door);
	}

	void Update()
	{
		ResizeReticle();
		Interact();
	}

	void ResizeReticle()
	{
		Vector3 scale = reticle.transform.localScale;
		
		scale.x = 40f / (cam.pixelWidth);
		scale.y = scale.x;
		
		reticle.transform.localScale = scale;
	}

	void Interact()
	{
		Ray ray = new Ray(transform.position, transform.forward);
		Debug.DrawRay(ray.origin, ray.direction + transform.forward * (detectionDistance - 1f));
		RaycastHit hitInfo;
		
		if (Physics.Raycast(ray, out hitInfo, detectionDistance, cullingMask))
		{
			if (hitInfo.transform.tag == "Interactive")
			{
				player.canInteract = true;
				player.interactiveObj = hitInfo.transform.gameObject;
				reticleRender.color = reticleInteract;
			}
			else
			{
				player.canInteract = false;
				reticleRender.color = reticleNormal;
			}
		}
		else
		{
			player.canInteract = false;
			reticleRender.color = reticleNormal;
		}
	}
}
