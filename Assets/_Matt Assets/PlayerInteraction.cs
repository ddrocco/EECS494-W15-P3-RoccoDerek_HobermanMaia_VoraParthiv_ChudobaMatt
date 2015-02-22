﻿using UnityEngine;
using System.Collections;

public class PlayerInteraction : MonoBehaviour
{
	private Camera cam;
	private GameObject reticle;
	private PlayerController player;

	public float detectionDistance;

	void Awake()
	{
		cam = GetComponent<Camera>();
		reticle = GameObject.Find("Reticle");
		player = GetComponentInParent<PlayerController>();
	}

	void Update()
	{
		ResizeReticle();
		Interact();
	}

	void ResizeReticle()
	{
		Vector3 scale = reticle.transform.localScale;
		
		scale.x = 50f / (cam.pixelWidth);
		scale.y = scale.x;
		
		reticle.transform.localScale = scale;
	}

	void Interact()
	{
		Ray ray = new Ray(transform.position, transform.forward);
		Debug.DrawRay(ray.origin, ray.direction + transform.forward * (detectionDistance - 1f));
		RaycastHit hitInfo;

		if (Physics.Raycast(ray, out hitInfo, detectionDistance))
		{
			if (hitInfo.transform.tag == "Interactive")
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
}
