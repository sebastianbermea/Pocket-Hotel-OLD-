using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Keyloss : MonoBehaviour
{
    Slot slot;
    Vector2 limitDown, limitUp;
    public GameObject key, coin, fill;
    public Animator anim, outlineAnim;
    [HideInInspector]
    public Staff currentStaff;
    public bool playerFind;
    Room room;
    public void Create(Slot slot)
    {
        this.slot = slot;
        room = transform.parent.parent.parent.GetComponentInChildren<Room>();
        room.fix = true;
        limitDown.x = GC.INS.limitL;
        limitDown.y = -1;
        limitUp.x = GC.INS.limitR;
        limitUp.y = GC.INS.limitY *0.75f;
        Invoke("RandomPos", .5f);
        key.SetActive(false);
        if(!GC.INS.visit)
            GC.INS.AddKeyLoss(slot.id);
        else
            VC.INS.AddKeyLoss(slot.id);
        SC.INS.PlaySound(0, 6, 0);
    }
    
    void RandomPos()
    {
        key.SetActive(true);
        float x = Random.Range(limitDown.x, limitUp.x);
        float y = Random.Range(limitDown.y, limitUp.y);
        if (GC.INS.level < 6)
        {
            x = Random.Range(transform.parent.position.x-GC.INS.level*.6f, transform.parent.position.x + GC.INS.level * .6f);
            y = Random.Range(transform.parent.position.y - GC.INS.level * .5f, transform.parent.position.y + GC.INS.level * .6f);
        }
        if(x< limitDown.x || x>limitUp.y || y<limitDown.y || y>limitUp.y)
        {
            x = Random.Range(transform.parent.position.x - 2, transform.parent.position.x + 2);
            y = Random.Range(transform.parent.position.y - 1, transform.parent.position.y + 3);
        }
        key.transform.position = new Vector2(x, y);
    }
    public void Found()
    {
        if (IsInvoking("Found"))
            CancelInvoke("Found");
        fill.SetActive(false);
        key.transform.localPosition = Vector2.zero;
        anim.SetTrigger("Out");
        outlineAnim.enabled = false;
        Invoke("Finish", .25f);
     
    }
    void Finish()
    {
        if (!GC.INS.visit)
        {
            if (playerFind)
            {
                coin.SetActive(true);
                coin.transform.parent = transform.parent.parent;
                GC.INS.AddCoins(1);
                GC.INS.AddXp(2);
                GC.INS.pg.Achivements(3, 0);
                GC.INS.dm.AddTask(0, 1);
                GC.INS.dm.AddTask(4, 1);
            }
            
            if (currentStaff == null && GC.INS.key.Contains(slot.id))
            {
                GC.INS.RemoveKey(slot);
            }
            else
            {
                if (currentStaff)
                {
                    currentStaff.FinishPipe();
                    currentStaff = null;
                }
                
            }
        }
        else
        {
            if (playerFind)
            {
                coin.SetActive(true);
                coin.transform.parent = transform.parent.parent;
                VC.INS.AddCoins(1, transform.parent.position);
                GC.INS.pg.Achivements(3, 0);
                GC.INS.dm.AddTask(0, 1);
                GC.INS.dm.AddTask(4, 1);
            }
            if (currentStaff == null && VC.INS.key.Contains(slot.id))
            {
                VC.INS.RemoveKey(slot);
            }
            else
            {
                currentStaff.FinishPipe();
                currentStaff = null;
            }
        }
        room.fix = false;
        Destroy(transform.parent.gameObject);
    }
    public void StartFix(Staff staff)
    {
        currentStaff = staff;
        fill.SetActive(true);
        Invoke("Found", 12f);
    }
    
}
