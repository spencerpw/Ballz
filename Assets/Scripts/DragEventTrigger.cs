using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class DragEventTrigger : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler {
	public DragEvent beginDrag;
	public DragEvent endDrag;
	public DragEvent onDrag;

	public void OnBeginDrag(PointerEventData e) {
		beginDrag.Invoke(e);
	}

	public void OnEndDrag(PointerEventData e) {
		endDrag.Invoke(e);
	}

	public void OnDrag(PointerEventData e) {
		onDrag.Invoke(e);
	}
}

[System.Serializable]
public class DragEvent : UnityEvent<PointerEventData> {}
