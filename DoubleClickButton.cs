using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;

[AddComponentMenu("UI/DoubleClickeButton")]
public class DoubleClickButton : MonoBehaviour, IPointerClickHandler
{
    [SerializeField]
    private UnityEvent m_method = new UnityEvent();
    private float delay = 0.25f;
    private int clickCount = 0;

    private Coroutine delayIE = null;

    private void Start()
    {
        
    }
    IEnumerator Delay()
    {
        yield return new WaitForSecondsRealtime(delay);
        clickCount = 0;
        delayIE = null;
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if (delayIE != null)
            StopCoroutine(delayIE);

        delayIE = StartCoroutine(Delay());
        clickCount += 1;
        if (clickCount == 2)
        {
            clickCount = 0;
            m_method.Invoke();
        }
    }
}
