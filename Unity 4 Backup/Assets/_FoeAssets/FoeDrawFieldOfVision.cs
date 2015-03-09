using UnityEngine;
using System.Collections;

public class FoeDrawFieldOfVision : MonoBehaviour {
	LineRenderer[] lines;
	int maxIndex;
	public float visionAngle = 90;
	public int numLines;
	public GameObject foeVisionLineRendererPrefab;
	//0								N/A	/1
	//-45, 45						45	/2
	//-45, 0, 45					45	/2
	//-45, -15, 15, 45				30	/3
	//-45, -22.5, 0, 22.5, 45		22.5 /4
	
	int cullingMask;

	void Start () {
		lines = new LineRenderer[numLines];
		for (int i = 0; i < numLines; ++i) {
			GameObject line = Instantiate(foeVisionLineRendererPrefab) as GameObject;
			lines[i] = line.GetComponent<LineRenderer>();
			line.transform.parent = transform;
		}
		maxIndex = lines.Length - 1;
		
		cullingMask = (1 << Layerdefs.wall) + (1 << Layerdefs.floor)
				+ (1 << Layerdefs.interactable) + (1 << Layerdefs.door);
	}
	
	void Update () {
		if (maxIndex > 0) {
			for (int i = 0; i <= maxIndex; ++i) {
				float fraction = (float)i / maxIndex;
				float angle = (-visionAngle / 2) + (visionAngle * fraction);
				Vector3 direction = Quaternion.Euler(new Vector3(0, angle, 0)) * transform.forward;
				AdjustVisionLine(lines[i], direction);
			}
		} else {
			AdjustVisionLine(lines[0], transform.forward);
		}
	}
	
	void AdjustVisionLine(LineRenderer line, Vector3 direction) {
		Ray ray = new Ray(transform.position, direction);
		RaycastHit hit;
		
		line.SetPosition(0, ray.origin);
		
		if(Physics.Raycast(ray, out hit, 100, cullingMask)){
			line.SetPosition(1, hit.point);
		}
		else {
			line.SetPosition(1, ray.GetPoint(100));	
		}
	}
}
