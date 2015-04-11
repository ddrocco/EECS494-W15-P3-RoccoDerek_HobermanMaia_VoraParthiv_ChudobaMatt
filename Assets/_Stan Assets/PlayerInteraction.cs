using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerInteraction : MonoBehaviour
{
	private Camera cam;
	private GameObject reticle;
	private Image reticleRender;
	private Text reticleLetter;
	private Color reticleNormal;
	private Color reticleInteract;
	private Color reticleTag;
	private PlayerController player;

	public float detectionDistance;
	private int cullingMask;

	void Awake()
	{
		cam = GetComponent<Camera>();
		reticle = GameObject.Find("Reticle");
		player = GameObject.Find("Player").GetComponent<PlayerController>();

		reticleRender = reticle.GetComponent<Image>();
		reticleLetter = GameObject.Find ("ReticleLetter").GetComponent<Text>();
		reticleNormal = Color.black;
		reticleNormal.a = 0.5f;
		reticleInteract = Color.green;
		reticleInteract.a = 0.5f;
		reticleTag = new Color(0f, 1f, 1f, 0.5f);

		cullingMask =
			(1 << Layerdefs.prop) +
			(1 << Layerdefs.env_camera) +
			(1 << Layerdefs.foe);
	}

	void Update()
	{
		ResizeReticle();
		Interact();
		//Tag(); temporarily disabling

		if (!player.canTag && !player.canInteract) {
			reticleRender.color = reticleNormal;
			reticleLetter.enabled = false;
		}
		else if (player.canTag){
			reticleRender.color = reticleTag;
			reticleLetter.enabled = false;
		}
		else if (player.canInteract) {
			reticleRender.color = reticleInteract;
			reticleLetter.enabled = true;
		}
		else
			Debug.LogError("Reticle color change error");
	}

	void ResizeReticle()
	{
		Vector3 scale = reticle.transform.localScale;
		
		scale.x = 100f / (cam.pixelWidth);
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
			if (hitInfo.transform.tag == "Interactive" || hitInfo.transform.tag == "FoeBody")
			{
				player.canInteract = true;
				player.interactiveObj = hitInfo.transform.gameObject;
			}
			else
			{
				player.canInteract = false;
			}
		}
		else
		{
			player.canInteract = false;
		}
	}

	void Tag()
	{
		Ray ray = new Ray(transform.position, transform.forward);
		Debug.DrawRay(ray.origin, ray.direction + transform.forward * (detectionDistance - 1f));
		RaycastHit hitInfo;
		
		if (Physics.Raycast(ray, out hitInfo, detectionDistance + 1f, cullingMask))
		{
			GameObject obj = hitInfo.transform.gameObject;
			if (obj.GetComponent<Taggable>() != null)
			{
				player.canTag = true;
				player.taggableObj = obj;
			}
			else
			{
				player.canTag = false;
			}
		}
		else
		{
			player.canTag = false;
		}
	}
}
