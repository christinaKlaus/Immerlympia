using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Pixelplacement;
public class UIGameTimer : MonoBehaviour {

	bool countdownActive = true, standOffActive = false;

	float maxGameTime = 60f, currentTime;
	int lastTimeInt = 0, currentTimeInt = 0;
	Vector3 defaultLocalScale;
	//Image maskedProgressImage;
	Coroutine GameTimerUIRoutine;

	[ReadOnly(false)]
	UITMPTextSetter[] countdownTexts = null;
	//[SerializeField] RectTransform maskedProgressTransform = null;
	[SerializeField] Color standOffColor = Color.grey;
	[SerializeField] Image centerProgressOuterBackground = null;
	[SerializeField] Image centerProgressOuterDiscreet = null;
	[SerializeField] Image centerProgressOuterContinuous = null, centerProgressInner = null;
	WaitForSeconds OneSecondWait = new WaitForSeconds(1f);

	public void Awake(){
		defaultLocalScale = transform.localScale;
		countdownTexts = GetComponentsInChildren<UITMPTextSetter>();
		// maskedProgressImage = maskedProgressTransform.GetComponent<Image>();
		PlayerManager.startWinCamEvent += OnWinCamStarted;
		PlayerManager.startStandoffEvent += OnStandoffStarted;
		countdownActive = true;
		centerProgressOuterBackground.color = Color.red;
	}

	public void Start(){
		maxGameTime = GameTimer.current.playTime;
		GameTimerUIRoutine = StartCoroutine(UpdateGameTimerUI(maxGameTime));
	}

	IEnumerator UpdateGameTimerUI(float currentMaxGameTime){
		foreach(UITMPTextSetter textSetter in countdownTexts){
			textSetter.gameObject.SetActive(true);
		}

		currentTime = currentMaxGameTime;
		//Debug.Log("started game time UI routine", this);
		yield return new WaitForEndOfFrame();
		while(countdownActive){
			currentTime = GameTimer.current.CurrentTime;

			currentTimeInt = (int) (currentTime);

			if(currentTimeInt != lastTimeInt){
				foreach(UITMPTextSetter textSetter in countdownTexts){
					textSetter.SetText(currentTimeInt);
					textSetter.SetColor(Color.Lerp(Color.red, Color.green, currentTime / (currentMaxGameTime * 0.5f)));
					if(currentTime < currentMaxGameTime / 2){
						transform.localScale = transform.localScale * 1.2f;
						Tween.LocalScale(transform, defaultLocalScale, 0.4f, 0f, null, Tween.LoopType.None, null, null, false);
					}
				}
			}

			// maskedProgressImage.color = Color.Lerp(Color.red, Color.green, currentTime / (currentMaxGameTime * 0.5f));
			centerProgressOuterDiscreet.fillAmount = (currentTimeInt / currentMaxGameTime);
			if(currentTimeInt != lastTimeInt){
				StartCoroutine(LerpCenterProgressOuterContinuous((float)lastTimeInt / currentMaxGameTime, (float) currentTimeInt / currentMaxGameTime, 0.5f));
			}
			centerProgressInner.fillAmount = Mathf.Repeat(currentTime, 1f);

			lastTimeInt = currentTimeInt;
			yield return null;
		}
		centerProgressInner.fillAmount = 0;
	}

	IEnumerator PulseGameTimerText(){
		while(standOffActive){
			foreach(UITMPTextSetter textSetter in countdownTexts){
				transform.localScale = transform.localScale * 1.2f;
				Tween.LocalScale(transform, defaultLocalScale, 0.4f, 0f, null, Tween.LoopType.None, null, null, false);
			}
			yield return OneSecondWait;
		}
	}

	IEnumerator LerpCenterProgressOuterContinuous(float from, float to, float time){
		for(float t = 0; t < time; t = t + Time.deltaTime){
			centerProgressOuterContinuous.fillAmount = Mathf.Lerp(from, to, t / time);
			yield return null;
		}
		centerProgressOuterContinuous.fillAmount = to;
	}

	void OnStandoffStarted(){
		centerProgressOuterBackground.color = standOffColor;
		countdownActive = false;
		standOffActive = true;
		StartCoroutine(PulseGameTimerText());
	}

	void OnWinCamStarted(){
		currentTime = currentTimeInt = 0;
		countdownActive = false;
		standOffActive = false;
		if(GameTimerUIRoutine != null){
			StopCoroutine(GameTimerUIRoutine);
		}
		foreach(UITMPTextSetter textSetter in countdownTexts){
			textSetter.gameObject.SetActive(false);
		}
	}

	void OnDestroy(){
		PlayerManager.startWinCamEvent -= OnWinCamStarted;
		PlayerManager.startStandoffEvent -= OnStandoffStarted;
	}

}
