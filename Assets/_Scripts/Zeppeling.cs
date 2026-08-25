using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zeppeling : MonoBehaviour
{
    float speed, dest, limitL,limitX;
    bool block;
    public SpriteRenderer sr;
    public Sprite[] sprites;
    // Start is called before the first frame update
    void Start()
    {
        speed = Random.Range(0.2f, 0.25f);
        dest = 0.3f;
        sr.sprite = sprites[Random.Range(0, 3)];
    }

   
    public void Create(float limitL, float limitR, float limitY)
    {
        limitX = limitR;
        this.limitL = limitL;
        transform.position = new Vector3(limitL-2, Random.Range(5, limitY / 2));
    }
    private void FixedUpdate()
    {
        if (Mathf.Abs(transform.position.x - dest) > 0.1f)
        {
            transform.Translate(Vector2.right * speed * Time.fixedDeltaTime);
        }
        else
        {
            if (block)
                Destroy(gameObject);
            else
                Leave();
        }
    }
    private void OnMouseDown()
    {
        Click();
    }
    public void Click()
    {
        if (block)
            return;
        GC.INS.ad.SetNote();
    }
    public void Claim()
    {
        GC.INS.InstantiateCostumerZeppeling(transform.position);
        Leave();
    }
    public void Leave()
    {
        if (block)
            return;
        speed = 2.5f;
        block = true;
        dest = limitX+2;
        GC.INS.ad.Close();
    }
}
