using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemGift : MonoBehaviour
{
    public SpriteRenderer sr;
    public Sprite[] sprites;
    ItemButton ib;
    Camera mainCam;
    int id, cost;
    bool gifting, dragging;
    Vector3 screenPoint;

    private void Awake()
    {
        mainCam = Camera.main;
    }

    public void SetGift(int id, int cost, ItemButton ib)
    {
        this.ib = ib;
        this.cost = cost;
        this.id = id;
        sr.sprite = sprites[id];
        BeginDrag();
        screenPoint = mainCam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0));
        screenPoint.z = 0;
        transform.position = screenPoint;
    }
    public void Gifting()
    {
        gifting = true;
        sr.enabled = false;
        SC.INS.PlaySound(0, 14, 0);
    }
    public void GiftingExit()
    {
        gifting = false;
        sr.enabled = true;
    }
    void BeginDrag()
    {
        dragging = true;
    }

    void FixedUpdate()
    {
        if (dragging)
        {
            screenPoint = mainCam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0));
            screenPoint.z = 0;

            transform.position = Vector3.MoveTowards(transform.position, screenPoint, Time.fixedDeltaTime * (25 + (transform.position - screenPoint).magnitude * 15));
            Vector3 tempPos = transform.localPosition;
            tempPos.z = 0;
            transform.localPosition = tempPos;

        }
    }
    private void Update()
    {
        if (Input.GetMouseButtonUp(0) && dragging)
        {
            EndDrag();
        }
    }
    void EndDrag()
    {
        dragging = false;
        GC.INS.EndDrag();

        if (gifting)
        {
            if (ib == null)
                VC.INS.AddGift(new Gift(4, 0, id, false), -cost);
            else
            {
                VC.INS.AddGift(new Gift(4, 0, id, true), 0);
                ib.Gifted();
                ib = null;
            }
        }
        VC.INS.EndDrag();
       
        Destroy(gameObject);
        return;

       
    }
}
