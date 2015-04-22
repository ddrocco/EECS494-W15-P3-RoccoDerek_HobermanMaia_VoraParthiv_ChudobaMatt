using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class MainMenuJuice : MonoBehaviour {
	public List<string> snippets = new List<string>();
	public GameObject JuiceText;
	Canvas canv;
	float timer = 0;
	float timeBetweenSpawns = 0.75f;
	
	void Start() {
		canv = GetComponent<Canvas>();
		snippets.Add ("data.clear();\nfor (int i = 0; start != end; start++, i++)\n\tdata[i] = *start;\ncompare = comp;");
		snippets.Add ("data.clear();\ncompare = comp;");
		snippets.Add ("int opt = 0, index = 0;\ndata.verbose = false;\ndata.median = false;\ndata.summary = false;\ndata.transfers = false;\n");
		snippets.Add ("switch(opt)\n{\n\tcase 's':\n\t\tdata.summary = true;\n\t\tbreak;\n\tcase 'v':\n\t\tdata.verbose = true;\n\t\tbreak;\n\tcase 'm':\n\t\tdata.median = true;\n\t\tbreak;\n\tcase 't':\n\t\tdata.transfers = true;\n\t\tbreak;\n\tcase 'i':\n\t\tdata.i.push_back(optarg);\n\t\tbreak;\n\tcase 'g':\n\t\tdata.g.push_back(optarg);\n\tbreak;");
	}
	
	void Update () {
		timer += Time.deltaTime;
		if (timer > timeBetweenSpawns) {
			timer -= timeBetweenSpawns;
			SpawnText();
		}
	}
	
	void SpawnText() {
		Rect rect = canv.pixelRect;
		Vector3 displacement = new Vector3(Random.Range(-rect.width / 2, rect.width / 2), Random.Range(-rect.height / 2, rect.height / 2), 0);
		Vector2 velocity = Random.Range (0, 5f) * Random.insideUnitCircle;
		
		GameObject JuicyText = Instantiate(JuiceText) as GameObject;
		JuicyText.transform.SetParent(parent: transform, worldPositionStays: false);
		JuicyText.GetComponent<RectTransform>().anchoredPosition = rect.center;
		JuicyText.transform.localPosition = displacement;
		JuicyText.transform.localScale = Vector3.one;
		JuicyText.GetComponent<IntroJuiceText>().velocity = new Vector3(velocity.x, velocity.y, 0);
		JuicyText.GetComponent<IntroJuiceText>().baseColor = new Color(0, 0.5f, 0, 0.5f);
		JuicyText.GetComponent<Text>().text = GetRandom (snippets);
	}
	
	string GetRandom(List<string> snippets) {
		float f = Random.Range(0, snippets.Count);
		int i = Mathf.FloorToInt(f);
		if (i == snippets.Count) {
			return snippets[0];
		} else {
			return snippets[i];
		}
	}
}
