using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;
#if UNITY_EDITOR
using UnityEditor;
#endif

[RequireComponent(typeof(Selectable))]
public class UISwitchSelectedGameobject : MonoBehaviour, ISubmitHandler, ICancelHandler {

	[SerializeField] private Selectable selectedOnSubmit = null;
	[SerializeField] private Selectable selectedOnCancel = null;
	[SerializeField] private UnityEvent submitEvent = null, cancelEvent = null;
	
	private Canvas associatedCanvasSubmit, associatedCanvasCancel;
	private EventSystem eventSystem;
	private UICameraPosition cameraPosition;
	private MainMenuInput mainMenuInput;

	// Use this for initialization
	void Start () {
		mainMenuInput = GetComponentInParent<MainMenuInput>();

		if(selectedOnSubmit != null)
		associatedCanvasSubmit = selectedOnSubmit.GetComponentInParent<Canvas>();
		if(selectedOnCancel != null)
		associatedCanvasCancel = selectedOnCancel.GetComponentInParent<Canvas>();

		eventSystem = EventSystem.current;
	}

    public void OnSubmit(BaseEventData eventData)
    {
		if(mainMenuInput.InTransition || (selectedOnSubmit == null && submitEvent.GetPersistentEventCount() <= 0)) return;
		Debug.Log("Submit callback on " + gameObject.name, this);
        if(selectedOnSubmit != null){
			Debug.Log("Selecting on submit: " + selectedOnSubmit.gameObject.name, this);
			if(!associatedCanvasSubmit.enabled) associatedCanvasSubmit.enabled = true;
			EventSystem.current.SetSelectedGameObject(selectedOnSubmit.gameObject);
		} /* else {
			EventSystem.current.SetSelectedGameObject(null);
		} */
		if(submitEvent != null) submitEvent.Invoke();
    }

    public void OnCancel(BaseEventData eventData)
    {
		if(mainMenuInput.InTransition || (selectedOnSubmit == null && cancelEvent.GetPersistentEventCount() <= 0)) return;
		Debug.Log("Cancel callback on " + gameObject.name, this);
        if(selectedOnCancel != null){
			Debug.Log("Selecting on cancel: " + selectedOnCancel.gameObject.name, this);
			if(!associatedCanvasCancel.enabled) associatedCanvasCancel.enabled = true;
			EventSystem.current.SetSelectedGameObject(selectedOnCancel.gameObject);
		} /* else {
			EventSystem.current.SetSelectedGameObject(null);
		} */
		if(cancelEvent != null) cancelEvent.Invoke();
    }

}
