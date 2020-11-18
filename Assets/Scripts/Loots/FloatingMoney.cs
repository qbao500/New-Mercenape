using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FloatingMoney: MonoBehaviour
{
    private TextMeshPro textMesh;

    [SerializeField] private int speed, destroyTime;

    private void Awake()
    {
        textMesh = transform.GetChild(0).GetComponent<TextMeshPro>();
        textMesh.sortingOrder = 15;
        PlayerCurrency.OnAddGold += SetGoldText;
    }

    private void SetGoldText(int gold)
    {
        textMesh.SetText(gold.ToString());
    }

    private void OnEnable()
    {
        Invoke("Off", destroyTime);
    }

    protected virtual void Update()
    {
        transform.Translate(Vector3.up * speed * Time.deltaTime);
    }

    protected void Off() => gameObject.SetActive(false);

}
