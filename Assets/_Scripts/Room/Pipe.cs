using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pipe : MonoBehaviour
{
    public Transform pivotPos, pipe, water, waves;
    private Camera myCam;
    private Vector3 screenPos;
    private float angleOffset;
    int count = 0, playerWork;
    float roomSizeX, roomSizeY, sizeX, sizeY, waterSizeX, waveSpawnSize=0;
    int limit = 3000;
    float lastAngle=1, spawningSize=0, fillSpeed = 500;
    public Animator pipeAnim;
    Slot slot;
    public GameObject[] wavesObj;
    bool ready, fixing, mouseDown;
    public GameObject coin;
    Staff currentStaff;
    AudioSource aud;
    void Start()
    {
        myCam = Camera.main;
    }

    public void Create(Slot slot)
    {
        aud = GetComponent<AudioSource>();
        limit = 2000 + Random.Range(0, 7) * 100;
        this.slot = slot;
        Room temp = transform.parent.parent.parent.GetComponentInChildren<Room>();
        roomSizeX = temp.roomSize.x;
        roomSizeY = temp.roomSize.y;
        sizeX = roomSizeX + (roomSizeX - 1) * 0.065f;
        sizeY = roomSizeY * 0.8f;
        water.localScale = new Vector2(0, sizeX);
        waterSizeX = .95f + (roomSizeX - 1) * .026f;

        transform.parent.localPosition = new Vector2(0, (roomSizeY - 1) *- .5f);
        waves.localScale = new Vector2(waterSizeX, 0);
        for(int i=1; i<roomSizeX; i++)
        {
            wavesObj[i].SetActive(true);
        }
        if (roomSizeX % 2 == 0)
        {
            waves.localPosition = new Vector2(-.5f + (roomSizeX - 1) * .0142f, 0.285f);
        }
        InvokeRepeating("Spawning", 0f, 0.05f);
        ready = false;
        if(!GC.INS.visit)
            GC.INS.AddPipe(slot.id);
        else
            VC.INS.AddPipe(slot.id);

        SC.INS.PlaySound(0, 1, 0);
    } 
    void Spawning()
    {
        if (water.localScale.x < sizeY)
        {
            spawningSize += .04f*roomSizeY;
            waveSpawnSize += 0.05f;
            water.localScale = new Vector2(spawningSize,sizeX);
            waves.localScale = new Vector2(waterSizeX,waveSpawnSize);
            waves.position = new Vector3(waves.position.x, pivotPos.position.y, pivotPos.position.z);
            waves.position = new Vector3(waves.position.x, pivotPos.position.y, pivotPos.position.z);
        }
        else
        {
            ready = true;
            CancelInvoke("Spawning");
        }
    }
    private void OnMouseDown()
    {
        if (!ready)
            return;
        GC.INS.isDragging = true;
        screenPos = myCam.WorldToScreenPoint(pipe.position);
        Vector3 v3 = Input.mousePosition - screenPos;
        angleOffset = (Mathf.Atan2(pipe.right.y, pipe.right.x) - Mathf.Atan2(v3.y, v3.x)) * Mathf.Rad2Deg;
        pipeAnim.SetBool("Spin", true);
        mouseDown = true;
        aud.Play();
    }
    private void OnMouseDrag()
    {
        if (!ready)
            return;
        if (count < limit)
        {
            Vector3 v3 = Input.mousePosition - screenPos;
            float angle = Mathf.Atan2(v3.y, v3.x) * Mathf.Rad2Deg;
            pipe.eulerAngles = new Vector3(0, 0, angle + angleOffset);
            int add = (int)(lastAngle - pipe.eulerAngles.z);
            if (add < 0)
            {
                if (add < -340)
                    lastAngle = pipe.eulerAngles.z;
                else
                    pipe.eulerAngles = new Vector3(0, 0, lastAngle);
                return;
            }
            count += add;
            playerWork += add;
            lastAngle = pipe.eulerAngles.z;
            water.localScale = new Vector3(sizeY - ((count * 1f) / (limit * 1f)) * sizeY, sizeX, 1);
            if(((count * 1f) / (limit * 1f)) > .5f)
            {
                waves.localScale = new Vector3(waterSizeX, 2 - ((count * 1f) / (limit * 1f)) * 2f, 1);
            }
            waves.position = new Vector3(waves.position.x,pivotPos.position.y, pivotPos.position.z);
            //waves.localPosition = new Vector3(waves.localPosition.x, waves.localPosition.y, 0.21f);
        }
        else
        {
            if (!IsInvoking("Finish"))
            {
                water.localScale = new Vector3(0, sizeX, 1);
                waves.localScale = new Vector3(waterSizeX,0, 1);
                pipeAnim.SetTrigger("Out");
                Invoke("Finish", .4f);
            }
      
        }
        
    }
    public void StartFix(Staff staff)
    {

        currentStaff = staff;
        fixing = true;
    }
    private void FixedUpdate()
    {
        if (fixing && ready)
        {
            if (count < limit)
            {
                count += (int)(Time.fixedDeltaTime * fillSpeed);
                water.localScale = new Vector3(sizeY - ((count * 1f) / (limit * 1f)) * sizeY, sizeX, 1);
                if (((count * 1f) / (limit * 1f)) > .5f)
                {
                    waves.localScale = new Vector3(waterSizeX, 2 - ((count * 1f) / (limit * 1f)) * 2f, 1);
                }
                waves.position = new Vector3(waves.position.x, pivotPos.position.y, pivotPos.position.z);
            }
            else
            {
                if (!IsInvoking("Finish"))
                {
                    water.localScale = new Vector3(0, sizeX, 1);
                    waves.localScale = new Vector3(waterSizeX, 0, 1);
                    pipeAnim.SetTrigger("Out");
                    Invoke("Finish", .4f);
                }
            }
            
        }
    }
    void Finish()
    {
        if (!GC.INS.visit)
        {
            if (playerWork >= (limit / 3) - 300)
            {
                coin.SetActive(true);
                coin.transform.parent = transform.parent.parent;
                GC.INS.AddCoins(1);
                GC.INS.AddXp(2);
                GC.INS.pg.Achivements(3, 0);
                GC.INS.dm.AddTask(0, 1);
                GC.INS.dm.AddTask(3, 1);
            }
            if (currentStaff == null && GC.INS.pipe.Contains(slot.id))
            {
                GC.INS.RemovePipe(slot);
            }
            else
            {
                if (currentStaff != null)
                    currentStaff.FinishPipe();
                currentStaff = null;
            }
        }
        else
        {
            if (playerWork >= (limit / 3) - 300)
            {
                coin.SetActive(true);
                coin.transform.parent = transform.parent.parent;
                VC.INS.AddCoins(1, transform.parent.position);
                GC.INS.pg.Achivements(3, 0);
                GC.INS.dm.AddTask(0, 1);
                GC.INS.dm.AddTask(3, 1);
            }
            if (currentStaff == null && VC.INS.pipe.Contains(slot.id))
            {
                VC.INS.RemovePipe(slot);
            }
            else
            {
                if (currentStaff != null)
                    currentStaff.FinishPipe();
                currentStaff = null;
            }
        }
       
        if (mouseDown)
        {
            GC.INS.isDragging = false;
        }
        Destroy(transform.parent.gameObject);
    }
    private void OnMouseUp()
    {
        if (!ready)
            return;
        GC.INS.isDragging = false;
        pipeAnim.SetBool("Spin", false);
        mouseDown = false;
        aud.Stop();
    }
    
}
