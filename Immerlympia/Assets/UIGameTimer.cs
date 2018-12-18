using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIGameTimer : MonoBehaviour {

	float maxGameTime = 60f, currentTime;
	Vector2 maskedProgressAnchorMax = Vector2.one;
	Image maskedProgressImage;

	[ReadOnly(false)]
	UITMPTextSetter[] countdownTexts;
	[SerializeField] RectTransform maskedProgressTransform;

	public void Awake(){
		countdownTexts = GetComponentsInChildren<UITMPTextSetter>();
		maskedProgressImage = maskedProgressTransform.GetComponent<Image>();
	}

	public void Start(){
		maxGameTime = GameTimer.current.playTime;
	}

	public void Update(){
		currentTime = GameTimer.current.getCurrentTime();
		foreach(UITMPTextSetter textSetter in countdownTexts){
			textSetter.SetText((int) (maxGameTime - currentTime));
		}
		//maskedProgressAnchorMax.x = 1 - (currentTime / maxGameTime);
		maskedProgressImage.color = Color.Lerp(Color.red, Color.green, (maxGameTime - currentTime) / (maxGameTime * 0.5f));
		//maskedProgressTransform.anchorMax = maskedProgressAnchorMax;
	}

}
