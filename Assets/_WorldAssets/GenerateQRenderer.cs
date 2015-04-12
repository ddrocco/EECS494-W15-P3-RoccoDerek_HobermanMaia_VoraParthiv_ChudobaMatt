using UnityEngine;
using System.Collections;

public class GenerateQRenderer : MonoBehaviour {
	
	public enum QViewType {
		StaticGreen,
		GoodBlue,
		BadRed,
		RoomGray
	};
	public QViewType type;
	
	//For computer-groups
	public int group = 0;
	
	GameObject qRendererObject;
	
	void Start () {
		qRendererObject = Instantiate(ObjectPrefabDefinitions.main.QRenderer,
				transform.position, Quaternion.identity) as GameObject;
		qRendererObject.transform.parent = transform;
		qRendererObject.transform.localScale = Vector3.one;
		qRendererObject.GetComponent<MeshFilter>().mesh = GetComponent<MeshFilter>().mesh;
		switch (type) {
		case QViewType.StaticGreen:
			qRendererObject.GetComponent<Renderer>().material.SetColor("_Color",new Color(0f, 0.1f, 0f));
			break;
		case QViewType.GoodBlue:
			qRendererObject.GetComponent<Renderer>().material.SetColor("_Color",Color.cyan);
				break;
		case QViewType.BadRed:
			qRendererObject.GetComponent<Renderer>().material.SetColor("_Color",Color.red);
			break;
		case QViewType.RoomGray:
			qRendererObject.GetComponent<Renderer>().material.SetColor("_Color",new Color(0.01f, 0.01f, 0.01f));
			qRendererObject.transform.position += new Vector3(0, -15f, 0);
			break;	
		}
		if (group != 0) {
			qRendererObject.GetComponent<Renderer>().enabled = false;
		}
	}
	
	public void Activate(int activateGroup) {
		if (group == activateGroup) {
			qRendererObject.GetComponent<Renderer>().enabled = true;
		}
	}
	
	public void Disable() {
		qRendererObject.GetComponent<Renderer>().material.SetColor("_Color",Color.gray);
	}
}
