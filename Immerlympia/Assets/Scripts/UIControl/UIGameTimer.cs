using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Pixelplacement;
public class UIGameTimer : MonoBehaviour {

	bool gameRunning = true;

	float maxGameTime = 60f, currentTime;
	int lastTimeInt = 0, currentTimeInt = 0;
	Vector3 defaultLocalScale;
	Image maskedProgressImage;
	RectTransform thisRect;
	Coroutine GameTimerUIRoutine;

	[ReadOnly(false)]
	UITMPTextSetter[] countdownTexts = null;
	[SerializeField] RectTransform maskedProgressTransform = null;
	[SerializeField] Image centerProgressOuterDiscreet = null;
	[SerializeField] Image centerProgressOuterContinuous = null, centerProgressInner = null;
	[SerializeField] ParticleSystem warningLights = null;

	public void Awake(){
		defaultLocalScale = transform.localScale;
		countdownTexts = GetComponentsInChildren<UITMPTextSetter>();
		maskedProgressImage = maskedProgressTransform.GetComponent<Image>();
		PlayerManager.gameEndEvent += OnGameEnded;
		gameRunning = true;
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
		Debug.Log("started game time UI routine", this);
		yield return new WaitForEndOfFrame();
		while(gameRunning){
			currentTime = GameTimer.current.CurrentTime;

			currentTimeInt = (int) (currentTime);

			foreach(UITMPTextSetter textSetter in countdownTexts){
				if(currentTimeInt != lastTimeInt){
					textSetter.SetText(currentTimeInt);
					if(currentTime < currentMaxGameTime / 2){
						transform.localScale = transform.localScale * 1.2f;
						Tween.LocalScale(transform, defaultLocalScale, 0.4f, 0f, null, Tween.LoopType.None, null, null, false);
					}
				}
			}

			if(currentTimeInt <= 10 && !warningLights.isPlaying){
				warningLights.Play(true);
			}

			//maskedProgressAnchorMax.x = 1 - (currentTime / maxGameTime);
			maskedProgressImage.color = Color.Lerp(Color.red, Color.green, currentTime / (currentMaxGameTime * 0.5f));
			//maskedProgressTransform.anchorMax = maskedProgressAnchorMax;
			centerProgressOuterDiscreet.fillAmount = (currentTimeInt / currentMaxGameTime);
			if(currentTimeInt != lastTimeInt){
				//centerProgressOuterContinuous.fillAmount = currentTime / currentMaxGameTime;
				StartCoroutine(LerpCenterProgressOuterContinuous((float)lastTimeInt / currentMaxGameTime, (float) currentTimeInt / currentMaxGameTime, 0.5f));
			}
			centerProgressInner.fillAmount = Mathf.Repeat(currentTime, 1f);

			lastTimeInt = currentTimeInt;
			yield return null;
		}
	}

	IEnumerator LerpCenterProgressOuterContinuous(float from, float to, float time){
		for(float t = 0; t < time; t = t + Time.deltaTime){
			centerProgressOuterContinuous.fillAmount = Mathf.Lerp(from, to, t / time);
			yield return null;
		}
		centerProgressOuterContinuous.fillAmount = to;
	}

	void OnGameEnded(){
		currentTime = currentTimeInt = 0;
		gameRunning = false;
		if(GameTimerUIRoutine != null){
			StopCoroutine(GameTimerUIRoutine);
		}
		warningLights.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
		foreach(UITMPTextSetter textSetter in countdownTexts){
			textSetter.gameObject.SetActive(false);
		}
	}

	void OnDestroy(){
		PlayerManager.gameEndEvent -= OnGameEnded;
	}

}
