using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(TextMeshProUGUI))]
public class UIPlayerRespawnTimer : MonoBehaviour {

	[ReadOnly(false)] public int playerIndex = -1;
	TextMeshProUGUI text;

	public float textIncrease = 50f, textFlashTime = 0.15f;
	[Header("value formats")]
	public string intFormat = "00";
	public string floatFormat = "0.0";
	WaitForSeconds halfSecondDelay, quarterSecondDelay;
	Coroutine respawnCountdown;

    private void Start() {
		halfSecondDelay = new WaitForSeconds(0.5f);
		quarterSecondDelay = new WaitForSeconds(0.25f);
        text = GetComponent<TextMeshProUGUI>();
		text.SetText("");
    }

    public void StartTimer(float respawnTime) {
		if(respawnCountdown != null) {
			StopCoroutine(respawnCountdown);
		}
		respawnCountdown = StartCoroutine(RespawnCountdown(respawnTime));
    }

	public void MarkPlayerInactive(){
		if(respawnCountdown != null)
			StopCoroutine(respawnCountdown);
		text.color = Color.red;
		text.SetText("X");
	}

	IEnumerator RespawnCountdown(float respawnTime){
		float textSize = text.fontSize;
		float elapsedTime = 0f;
		int countDown = (int) respawnTime;
		while(elapsedTime < respawnTime + 0.5f){
			if((int) elapsedTime != countDown){
				countDown = (int) elapsedTime;
				text.fontSize += textIncrease;
				text.SetText((respawnTime - elapsedTime).ToString(intFormat));
				for(float t = 0; t < textFlashTime; t = t + Time.deltaTime){
					text.fontSize = Mathf.Lerp(textSize + textIncrease, textSize, t / textFlashTime);
					elapsedTime += Time.deltaTime;
					yield return null;
				}
			}

			text.color = Color.Lerp(Color.red, Color.green, elapsedTime / respawnTime);
			elapsedTime += Time.deltaTime;
			yield return null;
		}
		text.SetText(0.ToString(intFormat));
		yield return halfSecondDelay;
		text.SetText("");
	}

}
