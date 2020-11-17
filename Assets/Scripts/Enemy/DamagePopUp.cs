using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DamagePopUp : MonoBehaviour
{  
    // Create a Damage Popup
    public static DamagePopUp Create(Vector3 pos, float damage, Color color, int size)
    {        
        ObjectPooler.Instance.SpawnFromPool("DamagePopUp", pos, Quaternion.identity);

        staticPopUp.Setup(damage, color, size);

        return staticPopUp;
    }

    public void Setup(float damage, Color color, int size)
    {
        textMesh.SetText(damage.ToString());
        textMesh.fontSize = size;
        
        if(color == Color.clear) { textMesh.color = textColor; }
        else { textMesh.color = color; }
       
        sortingOrder++;
        textMesh.sortingOrder = sortingOrder;
    }

    private static int sortingOrder = 0;

    private TextMeshPro textMesh;
    private Color textColor;

    private static DamagePopUp staticPopUp;

    private void Awake()
    {
        textMesh = transform.GetChild(0).GetComponent<TextMeshPro>();
        textColor = textMesh.color;
    }

    private void OnEnable()
    {
        staticPopUp = this;
        Invoke("Sleep", 1.1f);
    }
 
    private void Sleep()
    {
        gameObject.SetActive(false);
    }
}
