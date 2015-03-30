using UnityEngine;
using System.Collections;

public class Taggable : MonoBehaviour
{
	public enum TagType
	{
		door,
		camera,
		guard
	};

	public TagType type;

	public void TagObject()
	{
		return; // temporarily disabling tagging
/*
		if (type == TagType.door)
		{
			Debug.Log("Door");
			//Change color?
		}
		else if (type == TagType.camera)
		{
			Debug.Log("Camera");
			//Appear
			QInteractable obj = GetComponentInParent<QInteractable>();
			obj.enabled = true;
			QCameraControl QCam = QCamera.GetComponent<QCameraControl>();
			CameraControl cam = GetComponentInParent<CameraControl>();
			cam.QIsWatching = true;
			QCam.ToggleCamera(cam.ID, true);
			
		}

		else if (type == TagType.guard)
		{
			Debug.Log("Guard");
			//Appear
		}*/
	}
}