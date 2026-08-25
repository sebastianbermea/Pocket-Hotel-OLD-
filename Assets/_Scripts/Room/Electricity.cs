using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Electricity : MonoBehaviour
{
    public Transform fill, shadow;
    float fillCount, fillmax=3, roomSizeX, roomSizeY, speed = 0.3f, playerWork;
    public Animator anim;
    Slot slot;
    public GameObject coin;
    bool fixing;
    Staff currentStaff;
    AudioSource aud;

    public void Create(Slot slot)
    {
        aud = GetComponent<AudioSource>();
        fillmax = Random.Range(2.5f, 4.2f);
        this.slot = slot;
        if(!GC.INS.visit)
            GC.INS.AddElect(slot.id);
        else
         VC.INS.AddElect(slot.id);

        Room temp = transform.parent.parent.parent.GetComponentInChildren<Room>();
        roomSizeX = temp.roomSize.x;
        roomSizeY = temp.roomSize.y;
        shadow.localScale = new Vector3(roomSizeX*0.99f,roomSizeY*0.99f,1);
        SC.INS.PlaySound(0, 2, 0);
    }
    private void OnMouseDown()
    {
        anim.SetBool("Hold", true);
        aud.Play();
    }
    private void OnMouseDrag()
    {
        if (fillCount < fillmax)
        {
            fillCount += Time.deltaTime;
            playerWork += Time.deltaTime;
            fill.localScale = new Vector3((fillCount / fillmax)*0.5f, 0.5f, 1);
        }
        else
        {
            if (!IsInvoking("Finish"))
            {
                anim.SetBool("Hold", false);
                anim.SetTrigger("Out");
                Invoke("Finish", .4f);
            }
        }
       
    }
    private void FixedUpdate()
    {
        if (fixing)
        {
            if (fillCount < fillmax)
            {
                fillCount += Time.deltaTime*speed;
                fill.localScale = new Vector3((fillCount / fillmax) * 0.5f, 0.5f, 1);
            }
            else
            {
                if (!IsInvoking("Finish"))
                {
                    anim.SetBool("Hold", false);
                    anim.SetTrigger("Out");
                    Invoke("Finish", .4f);
                }
            }
        }
    }
    public void StartFix(Staff staff)
    {
        fixing = true;
        currentStaff = staff;
        anim.SetBool("Hold", true);
    }
    private void OnMouseUp()
    {
        if(currentStaff==null)
            anim.SetBool("Hold", false);

        aud.Stop();
    }
    void Finish()
    {
        if (!GC.INS.visit)
        {
            if (playerWork >= (fillmax / 3f))
            {
                coin.SetActive(true);
                coin.transform.parent = transform.parent.parent;
                GC.INS.AddCoins(1);
                GC.INS.AddXp(2);
                GC.INS.pg.Achivements(3, 0);
                GC.INS.dm.AddTask(0, 1);
                GC.INS.dm.AddTask(5, 1);
            }
            if (currentStaff == null && GC.INS.electricity.Contains(slot.id))
            {
                GC.INS.RemoveElect(slot);
            }
            else
            {
                currentStaff.FinishPipe();
                currentStaff = null;
            }
        }
        else
        {
            if (playerWork >= (fillmax / 2.2f))
            {
                coin.SetActive(true);
                coin.transform.parent = transform.parent.parent;
                VC.INS.AddCoins(1, transform.parent.position);
                GC.INS.pg.Achivements(3, 0);
                GC.INS.dm.AddTask(0, 1);
                GC.INS.dm.AddTask(5, 1);
            }
            if (currentStaff == null && VC.INS.electricity.Contains(slot.id))
            {
                VC.INS.RemoveElect(slot);
            }
            else
            {
                currentStaff.FinishPipe();
                currentStaff = null;
            }
        }
           

        Destroy(transform.parent.gameObject);
    }
}
