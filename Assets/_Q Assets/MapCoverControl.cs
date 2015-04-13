using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class MapCoverControl : MonoBehaviour
{
	private static List<List<QInteractable>> groups;
	private static List<QInteractable> group1;
	private static List<QInteractable> group2;
	private static List<QInteractable> group3;
	public static Image mapCover1;
	public static Image mapCover2;
	
	void Awake()
	{
		groups = new List<List<QInteractable>>();
		group1 = new List<QInteractable>();
		group2 = new List<QInteractable>();
		group3 = new List<QInteractable>();
		groups.Add(group1);
		groups.Add(group2);
		groups.Add(group3);

		mapCover1 = GameObject.Find("Cover1").GetComponent<Image>();
		mapCover2 = GameObject.Find("Cover2").GetComponent<Image>();
		mapCover1.enabled = true;
		mapCover2.enabled = true;

		QInteractable[] allObjs = FindObjectsOfType(typeof(QInteractable)) as QInteractable[];

		foreach (QInteractable temp in allObjs)
		{
			if (temp.group == MapGroup.One)
				group1.Add(temp);
			else if (temp.group == MapGroup.Two)
				group2.Add(temp);
			else if (temp.group == MapGroup.Three)
				group3.Add(temp);
		}

		for (int i = 1; i <= 2; i++)
		{
			foreach (QInteractable obj in groups[i])
			{
				if (obj.QInteractionButton)
					obj.QInteractionButton.SetActive(false);
			}
		}
	}

	public static void ToggleMapGroup(int group, bool state)
	{		
		if (group < 1 || group > 3)
		{
			Debug.LogError("ToggleMapGroup(int, bool) int must be between 1 and 3");
			return;
		}

		foreach (QInteractable obj in groups[group - 1])
		{
			if (obj.QInteractionButton)
				obj.QInteractionButton.SetActive(state);
		}

		if (group == 2)
			mapCover1.enabled = !state;
		else if (group == 3)
			mapCover2.enabled = !state;
	}
}