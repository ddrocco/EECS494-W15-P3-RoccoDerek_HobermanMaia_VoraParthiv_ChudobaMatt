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
		Debug.Log("Tagged this object");
	}
}