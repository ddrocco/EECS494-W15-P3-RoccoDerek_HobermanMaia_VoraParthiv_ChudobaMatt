using UnityEngine;
using System.Collections;

public class GoodBoxPositioning : MonoBehaviour {
	//Does nothing if "GoodBoxRandomPosition" is unused
	
	void Start () {
		if (GoodBoxRandomPosition.IsUsed) {
			GoodBoxRandomPosition.GetPos(gameObject);
		}
	}
}
