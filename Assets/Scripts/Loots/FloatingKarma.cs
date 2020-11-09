using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FloatingKarma : MonoBehaviour
{
    PlayerCurrency playerCurrency;
    TextMeshPro textMesh;

    public int karmaAmount;

    public float speed, destroyTime;

    private int sortingOrder;

    void Awake()
    {
        playerCurrency = GameObject.FindGameObjectWithTag("GameManager").GetComponent<PlayerCurrency>();
        
        textMesh = textMesh = transform.GetChild(0).GetComponent<TextMeshPro>();
        sortingOrder = 15;
        textMesh.sortingOrder = sortingOrder;
        textMesh.SetText(karmaAmount.ToString());        
    }

    private void OnEnable()
    {
        AddKarma();
        Invoke("Off", destroyTime);
    }

    void Update()
    {
        transform.Translate(Vector3.up * speed * Time.deltaTime); 
    }

    void AddKarma()
    { 
        playerCurrency.AddKarma(karmaAmount);
    }

    void Off()
    {
        gameObject.SetActive(false);
    }
}
