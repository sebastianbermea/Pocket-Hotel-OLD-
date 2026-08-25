using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public Animator anim;
    public SpriteRenderer sr;
    Vector2 dest;
    bool tapped;
    int id;
    // Start is called before the first frame update
    void Start()
    {
        
        if (Random.Range(0, 2) == 0)
        {
            if (Random.Range(0, 2) == 0)
            {
                if (Random.Range(0, 2) == 0)
                {
                    if (Random.Range(0, 2) == 0)
                    {
                        id = Random.Range(10, 14);
                    }
                    else
                    {
                        id = Random.Range(10, 12);
                    }
                }
                else
                {
                    id = Random.Range(7, 10);
                }
            }
            else
            {
                id = Random.Range(3, 7);
            }
        }
        else
        {
            id = Random.Range(0,3);
        }
        sr.sprite = SM.INS.items[id];
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (tapped)
        {
            if (((Vector2)transform.position - dest).magnitude > 0.1f)
                transform.position = Vector3.MoveTowards(transform.position, dest, Time.deltaTime * 10);
            else
            {
                Destroy(gameObject);
            }
        }
       

    }
    private void OnMouseDown()
    {
        if (transform.parent == null)
            return;
        dest = new Vector2(transform.position.x + 3f, transform.position.y - 3f);
        GC.INS.backController.AddItem(id);
        GC.INS.AddXp(50);
        transform.parent = null;
        anim.SetTrigger("Rotate");
        SC.INS.PlaySound(0, 15, 0);
        Invoke("Tapped", .5f);
    }
    void Tapped()
    {
        tapped = true;
    }

    public static int[] costs =
    {
		


    };
}
