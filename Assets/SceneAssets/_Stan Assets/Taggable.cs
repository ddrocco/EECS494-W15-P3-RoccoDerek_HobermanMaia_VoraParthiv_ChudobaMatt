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
		if (type == TagType.door) Debug.Log("Door");
		if (type == TagType.camera) Debug.Log("Camera");
		if (type == TagType.guard) Debug.Log("Guard");
	}
}