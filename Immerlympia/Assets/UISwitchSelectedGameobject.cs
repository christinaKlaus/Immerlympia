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

	[SerializeField] private Selectable selectedOnSubmit;
	[SerializeField] private Selectable selectedOnCancel;
	[SerializeField] private UnityEvent submitEvent, cancelEvent;
	
	private Canvas associatedCanvasSubmit, associatedCanvasCancel;
	private EventSystem eventSystem;

	// Use this for initialization
	void Start () {
		if(selectedOnSubmit != null)
		associatedCanvasSubmit = selectedOnSubmit.GetComponentInParent<Canvas>();
		if(selectedOnCancel != null)
		associatedCanvasCancel = selectedOnCancel.GetComponentInParent<Canvas>();

		eventSystem = EventSystem.current;
	}

    public void OnSubmit(BaseEventData eventData)
    {
		if(selectedOnSubmit == null && submitEvent.GetPersistentEventCount() <= 0) return;
        if(selectedOnSubmit != null){
			if(!associatedCanvasSubmit.enabled) associatedCanvasSubmit.enabled = true;
			EventSystem.current.SetSelectedGameObject(selectedOnSubmit.gameObject);
		} else {
			EventSystem.current.SetSelectedGameObject(null);
		}
		if(submitEvent != null) submitEvent.Invoke();
    }
    public void OnCancel(BaseEventData eventData)
    {
		if(selectedOnCancel == null && cancelEvent.GetPersistentEventCount() <= 0) return;
        if(selectedOnCancel != null){
			if(!associatedCanvasCancel.enabled) associatedCanvasCancel.enabled = true;
			EventSystem.current.SetSelectedGameObject(selectedOnCancel.gameObject);
		} else {
			EventSystem.current.SetSelectedGameObject(null);
		}
		if(cancelEvent != null) cancelEvent.Invoke();
    }
}
