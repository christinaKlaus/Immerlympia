using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TMPro.TextMeshProUGUI))]
public class UITMPTextSetter : MonoBehaviour {

	public string intFormat = "00";
	public string floatFormat = "0.0";
	private TMPro.TextMeshProUGUI thisText;

	public void Reset(){
		thisText = GetComponent<TMPro.TextMeshProUGUI>();
	}

	public void Awake(){
		thisText = GetComponent<TMPro.TextMeshProUGUI>();
	}

	public void SetText(string newText){
		thisText.SetText(newText);
	}

	public void SetText(int value){
		thisText.SetText(value.ToString(intFormat));
	}

	public void SetText(float value){
		thisText.SetText(value.ToString(floatFormat));
	}

	public void SetColor(Color newColor){
		thisText.color = newColor;
	}

}
