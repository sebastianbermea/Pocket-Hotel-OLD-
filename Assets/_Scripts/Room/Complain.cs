using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Complain : MonoBehaviour
{
    public Animator phone, boxAnim, faceAnim, anim;
    public SpriteRenderer face, mess;
    public Sprite[] faces, goodM, badM;
    public BoxCollider2D bc;
    Slot slot;
    public GameObject coin, box;
    int current;
    bool good, finish;
    [HideInInspector]
    public Staff currentStaff;
    bool fixing;
    public bool playerWork;
    Room room;
    public void Create(Slot slot)
    {
        this.slot = slot;

        current = 0;
        bc.enabled = false;
        Invoke("Message", .25f);
        room = transform.parent.parent.parent.GetComponentInChildren<Room>();
        room.fix = true;
        float roomSizeY = room.roomSize.y;
        if(!GC.INS.visit)
            GC.INS.AddComplaint(slot.id);
        else
            VC.INS.AddComplaint(slot.id);
        transform.parent.localPosition = new Vector2(0, (roomSizeY - 1) * -.5f);
        SC.INS.PlaySound(0, 5, 0);

    }
    void Message()
    {
        
        good = Random.Range(0, 2) == 0;
        box.SetActive(true);
        bc.enabled = true;
        if (good)
        {
            mess.sprite = goodM[Random.Range(0,3)];
            if(!fixing && gameObject.activeInHierarchy)
            SC.INS.PlaySound(0, 9, 0);
        }
        else
        {
            if (!fixing && gameObject.activeInHierarchy)
                SC.INS.PlaySound(0, 8, 0);
            mess.sprite = badM[Random.Range(0, 3)];
            if (fixing && !IsInvoking("Click"))
                Invoke("Click", 1.5f);
        }
        phone.SetTrigger("Ring");
        Invoke("Close", (fixing)? 2f : 1f);
    }
    public bool Click()
    {
        
        if (IsInvoking("Click"))
        {
            CancelInvoke("Click");
        }
        boxAnim.SetTrigger("Pop");
        CancelInvoke("Close");
        if (good)
        {
            faceAnim.SetTrigger("Good");
            current--;
            if (current < 0)
                current = 0;
            face.sprite = faces[current];
        }
        else
        {
            faceAnim.SetTrigger("Bad");
            current++;
            if (current >= 3)
            {
                finish = true;
                box.SetActive(false);
                anim.SetTrigger("Out");
                Invoke("Finish", .25f);
            }
            else
            {
                face.sprite = faces[current];
            }
            
        }
        if (IsInvoking("Message"))
            CancelInvoke("Message");
        if(!finish)
            Invoke("Message", .25f);
        return good;
    }
    void Close()
    {
        if (!good)
        {
            faceAnim.SetTrigger("Bad");
            current--;
            if (current < 0)
                current = 0;
            face.sprite = faces[current];
        }
        bc.enabled = false;
        boxAnim.SetTrigger("Close");
        if(!IsInvoking("Message"))
            Invoke("Message", .25f);
    }
    void Finish()
    {
        
        if (!GC.INS.visit)
        {
            if (playerWork)
            {
                GC.INS.AddCoins(1);
                coin.SetActive(true);
                coin.transform.parent = transform.parent.parent;
                GC.INS.AddXp(2);
                GC.INS.pg.Achivements(3, 0);
                GC.INS.dm.AddTask(0, 1);
                GC.INS.dm.AddTask(2, 1);
            }
           
            if (currentStaff == null && GC.INS.complaint.Contains(slot.id))
            {
                GC.INS.RemoveComplaint(slot);
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
            if (playerWork)
            {
                VC.INS.AddCoins(1, transform.parent.position);
                coin.SetActive(true);
                coin.transform.parent = transform.parent.parent;
                GC.INS.pg.Achivements(3, 0);
                GC.INS.dm.AddTask(0, 1);
                GC.INS.dm.AddTask(2, 1);

            }
            if (currentStaff == null && VC.INS.complaint.Contains(slot.id))
            {
                VC.INS.RemoveComplaint(slot);
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
        room.fix = false;
        Destroy(transform.parent.gameObject);
    }

    public void StartFix(Staff staff)
    {
        fixing = true;
        currentStaff = staff;
        //outlineAnim.SetBool("Hold", true);
    }
    private void OnDisable()
    {
        
    }
}
