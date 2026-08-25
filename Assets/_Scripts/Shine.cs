using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shine : MonoBehaviour
{
    RectTransform rect;
    // Start is called before the first frame update
    void Start()
    {
        rect = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        rect.Rotate(new Vector3(0, 0, -1));
    }
}
