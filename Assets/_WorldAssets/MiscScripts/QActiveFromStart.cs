using UnityEngine;
using System.Collections;

public class QActiveFromStart : MonoBehaviour
{
	void Start()
	{
		FindObjectOfType<QUI>().showCamera(true);
	}
}
