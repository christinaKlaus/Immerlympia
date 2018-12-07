using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Animator))]
public class MainMenuInput : MonoBehaviour {

	private Animator animator;

	int goToMenuHash = Animator.StringToHash("GoToMenu");
	[SerializeField] private int numMaxMenuItems = 3;
	[SerializeField] private int goToMenu = 0;

	void Start(){
		animator = GetComponent<Animator>();
		animator.SetInteger(goToMenuHash, goToMenu);
	}


	void Update(){
		if(animator.GetAnimatorTransitionInfo(1).nameHash == 0){
			if(Input.GetAxis("Horizontal0") < -0.5f){
				goToMenu = (goToMenu + 1) % numMaxMenuItems;
				animator.SetInteger(goToMenuHash, goToMenu);
			} else if (Input.GetAxis("Horizontal0") > 0.5f){
				goToMenu = (goToMenu + (numMaxMenuItems - 1)) % numMaxMenuItems;
				animator.SetInteger(goToMenuHash, goToMenu);
			}
		}
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

	public void LoadScene(string sceneName){
		SceneManager.LoadScene(sceneName);
		Time.timeScale = 1;
	}


}
