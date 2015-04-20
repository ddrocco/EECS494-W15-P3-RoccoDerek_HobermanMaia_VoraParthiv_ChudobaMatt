// C# example
// Simple script that lets you render the main camera in an editor Window.

using UnityEngine;
using UnityEditor;

public class ThirdWindow : EditorWindow {
	static Camera mainCamera, displayCamera, uiCamera;
	static int i = 0;
	static bool hasBeenEnabled = false;
	RenderTexture renderTexture;
	
	//This adds an item to the EDITOR PANE which creates a Custom Window
	[MenuItem("CustomViews/Split View")]
	static void Init() {
		ReloadCameras();
		
		hasBeenEnabled = true;
		
		EditorWindow editorWindow = GetWindow(typeof(ThirdWindow), utility: false, title: "Agent View");
		editorWindow.autoRepaintOnSceneChange = true;
		editorWindow.Show();
	}
	public void Awake () {
		renderTexture = new RenderTexture((int)position.width, 
		                                  (int)position.height, 
		                                  (int)RenderTextureFormat.ARGB32 );
	}
	public void Update() {
		if(mainCamera != null) {
			//Debug.Log(mainCamera.name + " " + displayCamera.name + " " + uiCamera.name);
			i = (i + 1) % 3;
			mainCamera.targetTexture = renderTexture;
			displayCamera.targetTexture = renderTexture;
			uiCamera.targetTexture = renderTexture;
			

			mainCamera.Render();
			displayCamera.Render();
			uiCamera.Render();
			//camera.targetTexture = null;	
		} else if (hasBeenEnabled) {
			ReloadCameras();
		}
		try {
			if(renderTexture.width != position.width || 
			   renderTexture.height != position.height)
				renderTexture = new RenderTexture((int)position.width, 
				                                  (int)position.height, 
				                                  (int)RenderTextureFormat.ARGB32 );
		} catch (MissingReferenceException) {
			/*...*/
		}
	}
	void OnGUI() {
		if (hasBeenEnabled) {
			GUI.DrawTexture( new Rect( 0.0f, 0.0f, position.width, position.height), renderTexture );
		} else {
			//Do something to clean this up
		}
	}
	
	static void ReloadCameras() {
		for (int i=0; i<Camera.allCamerasCount; ++i) {
			if (Camera.allCameras[i].name == "PlayerCamera") {
				mainCamera = Camera.allCameras[i];
				displayCamera = mainCamera.GetComponentsInChildren<Camera>()[1];
				uiCamera = mainCamera.GetComponentsInChildren<Camera>()[2];
				break;
			}
		}
		if (!mainCamera) {
			Debug.LogError("Could not find PlayerCamera");
		}
		
		mainCamera.rect = new Rect(0f, 0f, 1f, 1f);
		mainCamera.enabled = false;
		displayCamera.rect = new Rect(0f, 0f, 1f, 1f);
		displayCamera.enabled = false;
		uiCamera.rect = new Rect(0f, 0f, 1f, 1f);
		uiCamera.enabled = false;
		GameObject.Find("QCamera").GetComponent<Camera>().rect = new Rect(0f, 0f, 1f, 1f);
	}
}