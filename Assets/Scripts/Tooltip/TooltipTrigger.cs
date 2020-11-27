using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TooltipTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public string content;
    public string header;

    private Coroutine coroutine;

    public void OnPointerEnter(PointerEventData eventData)
    {
        coroutine = StartCoroutine(ShowToolTip());
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (coroutine != null) { StopCoroutine(coroutine); }
        TooltipSystem.Hide();
    }

    // Show after 0.5 sec
    private IEnumerator ShowToolTip()
    {
        yield return new WaitForSeconds(.5f);

        TooltipSystem.Show(content, header);
    }
}
