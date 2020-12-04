using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LongClickButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
	private bool pointerDown;
	private float pointerDownTimer;

	[SerializeField]
	private float requiredHoldTime;

	public UnityEvent onLongClick;

	[SerializeField]
	private Image fillImage;

	[SerializeField]
	private Image highlightBackground;

	void Awake()
    {
		fillImage.gameObject.SetActive(false);
    }


	public void OnPointerDown(PointerEventData eventData)
	{
		pointerDown = true;
		fillImage.gameObject.SetActive(true);
		highlightBackground.gameObject.SetActive(false);
		Debug.Log("OnPointerDown");

	}

	public void OnPointerUp(PointerEventData eventData)
	{
		Reset();
		fillImage.gameObject.SetActive(false);
		highlightBackground.gameObject.SetActive(true);
		Debug.Log("OnPointerUp");
	}

	private void Update()
	{
		if (pointerDown)
		{
			pointerDownTimer += Time.deltaTime;
			if (pointerDownTimer >= requiredHoldTime)
			{
				if (onLongClick != null)
					onLongClick.Invoke();

				Reset();
			}
			fillImage.fillAmount = pointerDownTimer / requiredHoldTime;
		}
	}

	private void Reset()
	{
		pointerDown = false;
		pointerDownTimer = 0;
		fillImage.fillAmount = pointerDownTimer / requiredHoldTime;
	}

}