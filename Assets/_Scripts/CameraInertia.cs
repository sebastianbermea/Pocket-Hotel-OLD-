using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class CameraInertia : MonoBehaviour
{
    Transform target;
    public float speed = 4;
    bool isInerting;
    Camera mainC;
    private void Awake()
    {
        target = Camera.main.transform;
        mainC = Camera.main;
    }
    // Update is called once per frame
    void Update()
    {
        if(!isInerting)
        transform.position = Vector3.MoveTowards(transform.position, (Vector2)target.position, Time.deltaTime*speed*mainC.orthographicSize/2);
    }
    public void StartInerting()
    {
        transform.position = (Vector2)transform.position + ((Vector2)target.position - (Vector2)transform.position)*2;
        isInerting = true;
    }
    private void OnEnable()
    {
        
    }
    private void OnDisable()
    {
        
    }
    public void StopInerting()
    {
        transform.position = (Vector2)target.position;
        isInerting = false;
       
    }
}
