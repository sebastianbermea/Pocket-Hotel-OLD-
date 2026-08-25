using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackParallax : MonoBehaviour
{
    private float startPos, dist;
    public GameObject cam;
    public float parallax;

    void Start()
    {
        startPos = transform.position.x;

    }

    private void FixedUpdate()
    {
        dist = cam.transform.position.x * parallax;
        transform.position = new Vector3(startPos+dist,transform.position.y, transform.position.z);
    }

}
