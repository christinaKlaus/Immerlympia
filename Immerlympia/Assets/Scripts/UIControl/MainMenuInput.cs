using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using Pixelplacement;

[RequireComponent(typeof(Animator))]
public class MainMenuInput : MonoBehaviour {

	private bool inTransition = false;
	private Animator animator;
	private GameObject lastSelected;
	

	[HideInInspector] public int goToMenuHash = Animator.StringToHash("GoToMenu");
	[SerializeField] private int goToMenu = 0;
	[ReadOnly(false)] public int currentPosition = 0;
	[ReadOnly(false)] public float currentTargetAngle;
	[SerializeField] Transform cameraOriginTransform = null;
	public bool InTransition {
		get { return inTransition; }
	}

	void Awake(){
		animator = GetComponent<Animator>();
		Cursor.visible = false;
	}

	void Start(){
		animator.SetInteger(goToMenuHash, goToMenu);
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
		if(inTransition) return;
		currentTargetAngle = toAngle;
		Tween.Rotation(cameraOriginTransform, new Vector3(0f, currentTargetAngle, 0f), 1f, 0f, Tween.EaseInOut, Tween.LoopType.None, () => ToggleTransition(true), () => ToggleTransition(false));
	}

	public void ToggleTransition(bool onOff){
		inTransition = onOff;
		EventSystem.current.sendNavigationEvents = !onOff;
	}
	
}
