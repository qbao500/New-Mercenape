using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FloatingUpgrade : MonoBehaviour
{
    private TextMeshPro textMesh;

    [SerializeField] private int speed, destroyTime;

    protected virtual void Update()
    {
        transform.Translate(Vector3.up * speed * Time.deltaTime);
    }

    protected void Off() => gameObject.SetActive(false);

    private void Awake()
    {
        
        textMesh = transform.GetChild(0).GetComponent<TextMeshPro>();
        textMesh.SetText("You found a rare weapon upgrade!");
        textMesh.sortingOrder = 15;
    }

   private void OnEnable()
    {
        Invoke("Off", destroyTime);
    }

   

}
