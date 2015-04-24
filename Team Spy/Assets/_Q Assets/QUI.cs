using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class QUI : MonoBehaviour {
	public class TextSnippet {
		public Text text { get; set; }
		public float time { get; set; }
	}
	
	static string objectiveTextContents;
	static List<TextSnippet> dynamicTextList = new List<TextSnippet>();

	public Text nosignal;
	public Text objectiveTextOutput;
	public static GameObject dynamicTextContainer;
	public static GameObject QUIObject;
	public Image objectivePanel;
	public GameObject player;
	public GameObject QCompass;

	int frameInvisibleMask = (1 << Layerdefs.ui);

	static float timeToClearDynamicText = 8f;
	static Color dynamicTextColor;

	// Use this for initialization
	void Start () {
		if (objectiveTextOutput == null) {
			objectiveTextOutput = GameObject.Find ("ObjectiveText").GetComponent<Text>();
		}
		if (dynamicTextContainer == null) {
			dynamicTextContainer = GameObject.Find ("DynamicTextContainer");
		}
		QUIObject = gameObject;
		objectiveTextContents = objectiveTextOutput.text;
		dynamicTextColor = Color.white;
		GetComponent<Camera>().cullingMask = frameInvisibleMask;
		GameObject.Find ("InteractionCanvas").GetComponent<CanvasGroup> ().alpha = 0;
		QCompass = GameObject.Find ("QCompass");
		QCompass.SetActive (false);
	}
	
	// Update is called once per frame
	void Update () {
		objectiveTextOutput.text = objectiveTextContents;
		foreach (TextSnippet snippet in dynamicTextList) {
			if (snippet.time > timeToClearDynamicText) {
				dynamicTextList.Remove(snippet);
				Destroy(snippet.text.gameObject);
				break;
			} else {
				snippet.text.color = dynamicTextColor * (1 - Mathf.Pow(snippet.time / timeToClearDynamicText,4));	
				snippet.time += Time.deltaTime;
			}	
		}
		
		if (objectiveTextContents == "")
			objectivePanel.enabled = false;
		else
			objectivePanel.enabled = true;
	}
	
	public void Interact() {
		showCamera(true);
		QUI.setText("Objective: Find the elevator key", objective: true);
		GameController.SendPlayerMessage("System access granted:\nFind more terminals", 5);
	}

	public static void setText(string newtext, bool objective) {
		if (objective) {
			objectiveTextContents = newtext;
		} else {
			foreach (TextSnippet snippet in dynamicTextList) {
				if (string.Compare(snippet.text.text, newtext) == 0) {
					snippet.time = 0;
					return;
				}
			}
			TextSnippet newSnippet = new TextSnippet();
			newSnippet.text = GetNewTextObject(newtext);
			newSnippet.time = 0f;
			dynamicTextList.Add (newSnippet);
		}
	}

	public static void appendText(string newtext, bool objective) {
		if (objective) {
			objectiveTextContents += "\n" + newtext;
		} else {
			setText(newtext, objective: false);
		}
	}

	public static void clearText(bool objective) {
		if (objective) {
			objectiveTextContents = "";
		} else {
			foreach(TextSnippet snippet in dynamicTextList) {
				Destroy(snippet.text.gameObject);
			}
			dynamicTextList = new List<TextSnippet>();
		}
	}

	public void showCamera(bool visible){
		if(visible){
			nosignal.enabled = false;
			clearText(objective: true);
			GetComponent<Camera>().cullingMask = GetComponent<QCameraControl>().overviewCullingMask;
			GetComponent<QCameraControl>().enabled = true;
			GameObject.Find("CamOverview").GetComponent<QCameraOverview>().camActive = true;
			GameObject.Find ("InteractionCanvas").GetComponent<CanvasGroup> ().alpha = 1;
			QCompass.SetActive (true);
			//GetComponent<QCameraControl>().DisableCameras();
		} else {
			nosignal.enabled = true;
			setText("Tell the agent to hack the computer.", objective: true);
			GetComponent<Camera>().cullingMask = frameInvisibleMask;
			GetComponent<QCameraControl>().enabled = false;
			GameObject.Find ("InteractionCanvas").GetComponent<CanvasGroup> ().alpha = 0;
			QCompass.SetActive (false);
		}
	}
	
	static Text GetNewTextObject(string contents) {
		GameObject newTextObj = Instantiate(ObjectPrefabDefinitions.main.QDynamicText) as GameObject;
		Text text = newTextObj.GetComponent<Text>();
		text.transform.SetParent(dynamicTextContainer.transform);
		text.transform.localPosition = Vector3.zero;
		text.transform.localEulerAngles = Vector3.zero;
		text.transform.localScale = Vector3.one;
		text.text = contents;
		text.name = dynamicTextList.Count.ToString();
		return text;
	}
}
