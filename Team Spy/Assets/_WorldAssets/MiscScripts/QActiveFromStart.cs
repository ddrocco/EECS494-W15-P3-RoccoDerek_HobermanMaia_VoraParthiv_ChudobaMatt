using UnityEngine;
using System.Collections;

public class QActiveFromStart : MonoBehaviour
{
	public string QMessage;

	void Start()
	{
		FindObjectOfType<QUI>().showCamera(true);

		QMessage = QMessage.Replace("NEWLINE", "\n");
		QUI.setText(QMessage);
	}
}
