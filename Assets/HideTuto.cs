using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HideTuto : MonoBehaviour {
	public Text hideText;
	
	bool hid = false;
	public Transform toHide;
	public Transform toHide2;
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.H))
		{
			hid = !hid;
			toHide.gameObject.SetActive(!hid);
			toHide2.gameObject.SetActive(!hid);
			hideText.text = hid ? "H : Show" : "H : Hide Instructions";
		}
	}
}
