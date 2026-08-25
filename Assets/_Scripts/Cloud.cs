using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cloud : MonoBehaviour
{
    public SpriteRenderer sr;
    public Sprite[] sprites;
    bool right, reward;
    public GameObject coin;
    float dest, speed;
    public Color[] colors;
    int colorId;
    // Start is called before the first frame update
    void Start()
    {
        sr.sprite = sprites[Random.Range(0, sprites.Length)];
        sr.flipX = (Random.Range(0, 2) == 0);
       
        float tempScale = Random.Range(0.8f, 2f);
        transform.localScale = Vector2.one * tempScale;
        right = Random.Range(0, 2) == 0;
        speed = Random.Range(0.2f, 1f);
        float y = Random.Range(3, GC.INS.limitY/3);
        reward = (Random.Range(0, 12) == 0);
        if (reward)
        {
            y = Random.Range(3, GC.INS.limitY / 5);
            colorId = 1;
        }
        if (Random.Range(0, 3) == 0)
        {
            y = Random.Range(3, (GC.INS.limitY*2)/3);
        }
        if (right)
        {
            dest = GC.INS.limitR +1;
            transform.position = new Vector3(GC.INS.limitL-1,y, -1);
        }
        else
        {
            dest = GC.INS.limitL -1;
            speed*=-1;
            transform.position = new Vector3(GC.INS.limitR + 1,y, -1);
        }
        
        sr.color = new Color(colors[colorId].r, colors[colorId].g, colors[colorId].b, Random.Range(0.4f, 0.9f));
    }

    private void FixedUpdate()
    {
        if (Mathf.Abs(transform.position.x - dest) > 0.1f)
        {
            transform.Translate(Vector2.right * speed * Time.fixedDeltaTime);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    public void Puff()
    {
        if (!IsInvoking("Puffing"))
        {
            GC.INS.dm.AddTask(16, 1);
            GC.INS.AddXp(1);
            SC.INS.PlaySound(1, 7, 0);
            if (reward)
            {
                GC.INS.AddXp(2);
                GC.INS.dm.AddTask(17, 1);
                Instantiate(coin, new Vector3(transform.position.x, transform.position.y + 0.3f, 0), transform.rotation);
                GC.INS.AddCoins(1);
            }
         
            InvokeRepeating("Puffing", 0, 0.03f);
        }
      
    }
    void Puffing()
    {
        if (sr.color.a > 0)
        {
            sr.color = new Color(colors[colorId].r, colors[colorId].g, colors[colorId].b, sr.color.a-0.02f);
            transform.localScale = new Vector2(transform.localScale.x + 0.02f, transform.localScale.y+0.02f);
        }
        else
        {
            CancelInvoke("Puffing");
           
            Destroy(gameObject);
        }
    }
    
}
