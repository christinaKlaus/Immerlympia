using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UICameraPosition : MonoBehaviour, ISelectHandler {

	[Range(0,2), Tooltip("0 = play, 1 = settings, 2 = quit")]
	public int menuPosition;
	int goToMenuHash = -1;

	Animator mainMenuParentAnimator;

	void Start(){
		MainMenuInput mainMenuInput = GetComponentInParent<MainMenuInput>();
		goToMenuHash = mainMenuInput.goToMenuHash;
		mainMenuParentAnimator = mainMenuInput.GetComponent<Animator>();
	}

    public void OnSelect(BaseEventData eventData)
    {
        mainMenuParentAnimator.SetInteger(goToMenuHash, menuPosition);
    }
}
