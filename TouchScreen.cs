using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
/*
 * Precisa de um Collider
 */
public class TouchScreen : MonoBehaviour, IPointerClickHandler, IDragHandler, IEndDragHandler
{
    [System.Serializable]
    public class TouchEvents
    {
        public enum Type
        {
            click,
            Move,
            Up,
            Down,
            Left,
            Right
        }
        public Type eventType;
        public UnityEvent events;
    }
    [SerializeField]
    public List<TouchEvents> touchEvents = new List<TouchEvents>();
    private bool drag = false;
    private Vector2 lastTouchPosition;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (drag)
            return;
        List<TouchEvents> clickEvents = touchEvents.FindAll((x) => x.eventType == TouchEvents.Type.click);
        foreach (TouchEvents clickEvent in clickEvents)
        {
            clickEvent.events.Invoke();
        }
    }

    public void OnDrag(PointerEventData eventData)
    {

        drag = true;
        List<TouchEvents> clickEvents = new List<TouchEvents>();
        clickEvents.AddRange(touchEvents.FindAll((i) => i.eventType == TouchEvents.Type.Move));
        
        Vector2 position = eventData.position;
        float x = lastTouchPosition.x - position.x;
        float y = lastTouchPosition.y - position.y;

        if (x == 0 && y == 0)
            return;
        if (lastTouchPosition == Vector2.zero)
        {

        }
        else if (Mathf.Abs(x) > Mathf.Abs(y))
        {
            //Horizontal
            if (x > 0)//Left
                clickEvents.AddRange(touchEvents.FindAll((i) => i.eventType == TouchEvents.Type.Left));
            else//Right
                clickEvents.AddRange(touchEvents.FindAll((i) => i.eventType == TouchEvents.Type.Right));
        }
        else
        {
            //Vertical
            if (y < 0)//Up
                clickEvents.AddRange(touchEvents.FindAll((i) => i.eventType == TouchEvents.Type.Up));
            else//Down
                clickEvents.AddRange(touchEvents.FindAll((i) => i.eventType == TouchEvents.Type.Down));
        }
        foreach (TouchEvents clickEvent in clickEvents)
        {
            clickEvent.events.Invoke();
        }
        lastTouchPosition = position;

    }
    public void OnEndDrag(PointerEventData eventData)
    {
        lastTouchPosition = Vector2.zero;
        drag = false;

    }
}
