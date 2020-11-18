using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FloatingKarma : MonoBehaviour
{
    private TextMeshPro textMesh;

    [SerializeField] private int speed, destroyTime;
  
    private void Awake()
    {
        textMesh = transform.GetChild(0).GetComponent<TextMeshPro>();
        textMesh.sortingOrder = 15;
        PlayerCurrency.OnAddKarma += SetKarmaText;
    }

    private void SetKarmaText(int karma)
    {
        textMesh.SetText(karma.ToString());
    }

    private void OnEnable()
    {        
        Invoke("Off", destroyTime);
    }

    private void Update()
    {
        transform.Translate(Vector3.up * speed * Time.deltaTime);
    }

    private void Off() => gameObject.SetActive(false);
}
