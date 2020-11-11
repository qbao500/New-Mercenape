using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Created by Arttu Paldán 1.10.2020: This script used to navigate the Forge scene. Aka these button methods set screens active and false. 
public class ScreenNavigation : MonoBehaviour
{
    private GameObject shop, forge;

    void Awake()
    {
        shop = GameObject.FindGameObjectWithTag("Shop");
        forge = GameObject.FindGameObjectWithTag("Forge");
    }

    void Start()
    {
        shop.SetActive(false);
    }

    public void OpenShop()
    {
        forge.SetActive(false);
        shop.SetActive(true);
    }

    public void OpenForge()
    {
        shop.SetActive(false);
        forge.SetActive(true);
    }
}
