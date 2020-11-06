using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Created by Arttu Paldán on 28.10.2020:Temporary camera script for play testing. This version of the camera switches between to cameras depending on whether player zooms in or not. 
public class CameraController : MonoBehaviour
{
    public GameObject[] cameras;

    public Transform playerTransform;

    public Vector3 directionChange;

    private bool zoomedIN;

    void Start()
    {
        cameras[1].SetActive(false);
        zoomedIN = true;
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        Zoom();
    }

    void LateUpdate()
    {
        if (cameras[0].activeSelf)
        {
            cameras[0].transform.position = playerTransform.position + directionChange;
        }
    }

    void Zoom()
    {
        if (Input.GetKeyDown("c") && zoomedIN) 
        {
            cameras[0].SetActive(false);
            cameras[1].SetActive(true);
            zoomedIN = false;
        }
        else if(Input.GetKeyDown("c") && !zoomedIN)
        {
            cameras[0].SetActive(true);
            cameras[1].SetActive(false);
            zoomedIN = true;
        } 
    }
}
