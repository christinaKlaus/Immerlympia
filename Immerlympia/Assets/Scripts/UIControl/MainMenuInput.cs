using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using Pixelplacement;
using UnityEngine.Audio;

[RequireComponent(typeof(Animator))]
public class MainMenuInput : MonoBehaviour {

	[SerializeField] SimpleAudioEvent rotateAudio, snapAudio, riseAudio;
	private bool inTransition = false;
	private Animator animator;
	private AudioSource audioSource;
	[Header("Audio")]
	[SerializeField] private AudioSource musicSource;

	[HideInInspector] public int goToMenuHash = Animator.StringToHash("GoToMenu");
	[SerializeField] private int goToMenu = 0;
	[ReadOnly(false)] public int currentPosition = 0;
	[ReadOnly(false)] public float currentTargetAngle = 0;
	[SerializeField] Transform cameraOriginTransform = null;
	[Header("Platform rise parameters")]
	[SerializeField] float platformRiseDelay = 0.5f;
	[SerializeField] float platformRiseDuration = 2f;
	[SerializeField] Transform[] platformTransforms;
	public bool InTransition {
		get { return inTransition; }
	}

	void Awake(){
		animator = GetComponent<Animator>();
		audioSource = GetComponent<AudioSource>();
		// Cursor.visible = false;
		// Cursor.lockState = CursorLockMode.Locked;
	}

	void Start(){
		Time.timeScale = 1f;
		//animator.SetInteger(goToMenuHash, goToMenu);
		riseAudio.Play(audioSource);
		float musicDelay = (platformRiseDelay * platformTransforms.Length) + platformRiseDuration;
		musicSource.PlayDelayed(musicDelay);
		musicSource.volume = 0f;
		Tween.Volume(musicSource, 1f, 1.5f, musicDelay, null, Tween.LoopType.None, null, null, false);
		if(platformTransforms != null && platformTransforms.Length > 0){
			inTransition = true;
			for(int i = 0; i < platformTransforms.Length-1; i++){
				Transform t = platformTransforms[i];
				Tween.Position(
					t,
					transform.position + Vector3.down * 100f,
					transform.position,
					platformRiseDuration,
					i * platformRiseDelay,
					Tween.EaseOutBack,
					Tween.LoopType.None,
					null,
					null,//() => snapAudio.PlayOneShot(audioSource),
					false);
			}
			Transform last = platformTransforms[platformTransforms.Length-1];
			Tween.Position(
					last,
					transform.position + Vector3.down * 100f,
					transform.position,
					platformRiseDuration,
					(platformTransforms.Length - 1) * platformRiseDelay,
					Tween.EaseOutBack,
					Tween.LoopType.None,
					null,
					() => ToggleTransition(false),//() => snapAudio.PlayOneShot(audioSource),
					false);
		}
		Invoke("FadeInMenus", musicDelay - 0.5f);
	}

	void FadeInMenus(){
		animator.SetTrigger("platformEntry");
	}

	void GoToScene(){
		switch(goToMenu){
			case 0: 
				SceneManager.LoadScene("Immerlympia_Game");
				Time.timeScale = 1;
				break;
			case 1:
				SceneManager.LoadScene("Credits");
				break;
			case 2:
				Application.Quit();
				break;
			default:
				break;
		}
	}

	public void SetInteger(int menuHash, int value){
		currentPosition = value;
		animator.SetInteger(menuHash, value);
	}

	public void RotateMenu(float toAngle){
		if(inTransition || currentTargetAngle == toAngle) return;
		currentTargetAngle = toAngle;
		Tween.Rotation(cameraOriginTransform, new Vector3(0f, currentTargetAngle, 0f), 1f, 0f, Tween.EaseInOutBack, Tween.LoopType.None, () => ToggleTransition(true), () => ToggleTransition(false));
		rotateAudio.PlayOneShot(audioSource);
	}

	public void ToggleTransition(bool onOff){
		inTransition = onOff;
		EventSystem.current.sendNavigationEvents = !onOff;
	}
	
}
