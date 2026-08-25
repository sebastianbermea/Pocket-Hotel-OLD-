using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CostumerThrower : MonoBehaviour
{
    Transform costumer;
    float speed=20;
    private void OnEnable()
    {
        costumer = transform.parent;
        transform.parent = transform.parent.parent;
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position = Vector3.MoveTowards(transform.position, (Vector2)costumer.position, Time.deltaTime * speed);
    }
}
