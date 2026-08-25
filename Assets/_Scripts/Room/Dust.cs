using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dust : MonoBehaviour
{
    public Animator anim;
    public Sprite[] sprites;
    public SpriteRenderer sr;
    int tapTimes, limit1, limit2, playerWork;
    Slot slot;
    public ParticleSystem particle;
    public GameObject coin, arrowTuto;
    Room room;
    public void Create(Slot slot)
    {
        this.slot = slot;
        room = transform.parent.parent.parent.parent.GetComponentInChildren<Room>();
        room.fix = true;
        tapTimes = Random.Range(6, 12);
        limit1 = tapTimes - (tapTimes / 3);
        limit2 = tapTimes - (tapTimes / 3)*2;
        transform.parent.localPosition = slot.pos;
        if(!GC.INS.visit)
            GC.INS.AddDust(slot.id);
        else
            VC.INS.AddDust(slot.id);
        SC.INS.PlaySound(0, 3, 0);

        if (GC.INS.tutoOn)
        {
            if (GC.INS.tuto.current == 14)
            {
                Instantiate(arrowTuto, transform.parent);
            }
        }
    }
  
    private void OnMouseDown()
    {
        anim.SetTrigger("Click");

        
    }
    private void OnMouseUp()
    {
        anim.SetTrigger("Up");
    }
    private void OnMouseUpAsButton()
    {
        if (tapTimes <= 0)
            return;
        Tap();
        playerWork++;
        Instantiate(particle, transform.parent);
        SC.INS.PlaySound(0, 4, 0);
    }
    public bool Cleaner()
    {
        if (tapTimes <= 0)
            return true;
        anim.SetTrigger("Click");
        Invoke("ClickUp", .1f);
        return Tap();
    }
    void ClickUp()
    {
        anim.SetTrigger("Up");
    }
    bool Tap()
    {
        if (tapTimes <= 0)
            return true;

        
        tapTimes--;
        if (tapTimes == limit1)
            sr.sprite = sprites[1];
        if (tapTimes == limit2)
            sr.sprite = sprites[2];

        if (tapTimes <= 0)
        {
            if (!IsInvoking("Finish"))
            {
                anim.SetTrigger("Out");
                Invoke("Finish", .4f);
            }
            return true;
        }
        return false;
    }
    void Finish()
    {
        if (!GC.INS.visit)
        {
            if (playerWork >= 3)
            {
                if(GC.INS.tutoOn && GC.INS.tuto.current == 14)
                {
                    GC.INS.tuto.Next();
                }
                coin.SetActive(true);
                coin.transform.parent = transform.parent.parent;
                GC.INS.AddCoins(1);
                GC.INS.AddXp(2);
                GC.INS.pg.Achivements(3, 0);
                GC.INS.dm.AddTask(0,1);
                GC.INS.dm.AddTask(1,1);
            }
            if (GC.INS.dust.Contains(slot.id))
            {

                GC.INS.RemoveDust(slot);

            }
        }
        else
        {
            if (playerWork >=3)
            {
                coin.SetActive(true);
                coin.transform.parent = transform.parent.parent;
                VC.INS.AddCoins(1, transform.parent.position);
                GC.INS.pg.Achivements(3, 0);
                GC.INS.dm.AddTask(0, 1);
                GC.INS.dm.AddTask(1, 1);
            }
            if (VC.INS.dust.Contains(slot.id))
            {
                VC.INS.RemoveDust(slot);
            }
        }


        room.fix = false;
        Destroy(transform.parent.gameObject);
    }
}
