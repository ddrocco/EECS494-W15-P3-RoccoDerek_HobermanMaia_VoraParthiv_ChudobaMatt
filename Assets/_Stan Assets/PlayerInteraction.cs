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
	private PlayerController player;
	private Image toolTipFrame;
	private Text toolTipText;
	private RaycastHit hitInfo;
	
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
		
		toolTipFrame = GameObject.Find ("ToolTips").GetComponent<Image>();
		toolTipText = GameObject.Find ("ToolTipsText").GetComponent<Text>();

		cullingMask =
			(1 << Layerdefs.prop) +
			(1 << Layerdefs.env_camera) +
			(1 << Layerdefs.foe);
	}

	void Update()
	{
		ResizeReticle();
		Interact();

		if (!player.canInteract) {
			reticleRender.color = reticleNormal;
			reticleLetter.enabled = false;
			EndTip ();
		}
		else if (player.canInteract) {
			reticleRender.color = reticleInteract;
			reticleLetter.enabled = true;
			//ToolTips
			GiveTip ();
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
	
	void GiveTip() {
		toolTipFrame.enabled = true;
		toolTipText.enabled = true;
		if (hitInfo.transform.GetComponent<DoorControl>()) {
			toolTipText.text = "Open door";
		} else if (hitInfo.transform.GetComponent<CameraControl>()){
			toolTipText.text = "Blind camera";
		} else if (hitInfo.transform.GetComponent<ComputerConsole>()) {
			toolTipText.text = "Use computer";
		} else if (hitInfo.transform.GetComponent<ElevatorControl>()) {
			toolTipText.text = "Call elevator";
		} else if (hitInfo.transform.GetComponent<FileCabinetControl>()) {
			toolTipText.text = "Open drawer";
		} else if (hitInfo.transform.GetComponent<BoxControl>()) {
			toolTipText.text = "Open box";
		} else if (hitInfo.transform.GetComponent<InformationForPlayer>()) {
			toolTipText.text = "Read";
		}
		
	}

	void EndTip() {
		toolTipFrame.enabled = false;
		toolTipText.enabled = false;
	}
}
