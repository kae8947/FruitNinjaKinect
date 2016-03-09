﻿using UnityEngine;
using System.Collections;

public class MoveWithMouse : MonoBehaviour {

    public float speed = 3.0f;
    private Vector3 targetPos;

    // Use this for initialization
    void Start () {
        targetPos = transform.position;
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButtonDown(0))
        {
            float distance = transform.position.z - Camera.main.transform.position.z;
            targetPos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, distance);
            targetPos = Camera.main.ScreenToWorldPoint(targetPos);
        }

        transform.position = Vector3.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);
    }
}
