using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonScript : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler
{
    private Vector3 normalScale;

    public float enterRate = 1.1f;
    public float clickRate = 1.3f;


    private void Awake()
    {
        normalScale = transform.localScale;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        transform.localScale = normalScale * clickRate;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        transform.localScale = normalScale;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        transform.localScale = normalScale * enterRate;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        transform.localScale = normalScale;
    }

}
