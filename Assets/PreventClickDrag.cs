using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;

/*https://forum.unity.com/threads/scroll-rect-disable-dragging.303841/*/
public class PreventClickDrag : ScrollRect
{

    public override void OnBeginDrag(PointerEventData eventData) { }
    public override void OnDrag(PointerEventData eventData) { }
    public override void OnEndDrag(PointerEventData eventData) { }
}
