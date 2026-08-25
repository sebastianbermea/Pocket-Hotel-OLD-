using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaffOutfit : MonoBehaviour
{
    public SpriteRenderer[] outfit;
    Camera mainCam;
    Vector3 screenPoint;
    bool dragging, trashing, gift, gifting;
    Staff tempStaff;
    int id;
    public GameObject content;
    Sprite[] outfitSprite;
    OutfitButton ob;
    private void Awake()
    {
        mainCam = Camera.main;
    }

    public void Purchased(int id)
    {
        this.id = id;
        outfitSprite = SM.INS.GetOutfit(id);
        for (int i = 0; i < outfitSprite.Length; i++)
            outfit[i].sprite = outfitSprite[i];

        BeginDrag();
        screenPoint = mainCam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0));
        screenPoint.z = 0;
        transform.parent.position = screenPoint;
    }
    public void SetGift(int id, OutfitButton ob)
    {
        this.ob = ob;
        gift = true;
        Rigidbody2D rig = GetComponent<Rigidbody2D>();
        Destroy(rig);
        Purchased(id);
    }
    public void Gift(OutfitButton obn)
    {
        ob = obn;
        Purchased(ob.id);
    }

    public void Gifting()
    {
        gifting = true;
        content.SetActive(false);
        SC.INS.PlaySound(0, 14, 0);
    }
    public void GiftingExit()
    {
        gifting = false;
        
        content.SetActive(true);
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

            transform.parent.position = Vector3.MoveTowards(transform.parent.position, screenPoint, Time.fixedDeltaTime * (25 + (transform.parent.position - screenPoint).magnitude * 15));
            Vector3 tempPos = transform.parent.localPosition;
            tempPos.z = 0;
            transform.parent.localPosition = tempPos;

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
        if (gift)
        {
            if (gifting)
            {
                if (ob == null)
                    VC.INS.AddGift(new Gift(3, 1, id, false), costs[id]);
                else
                {
                    VC.INS.AddGift(new Gift(3, 1, id, true), 0);
                    ob.Purchased();
                    ob = null;
                }
            }
            VC.INS.EndDrag();
            if (GC.INS.isDragging)
            {
                GC.INS.EndDrag();
            }
            Destroy(transform.parent.gameObject);
            return;
        }
        if (tempStaff != null)
        {

            if (!trashing && tempStaff.character.outfitId != id)
            {
                if (ob == null)
                {
                    tempStaff.PurchasedOutfit(id);
                }
                else
                {
                    tempStaff.GiftOutfit(ob);
                    ob = null;
                }
               
            }
            else
                tempStaff.ResetOutfit();
        }
            
        Destroy(transform.parent.gameObject);
    }
    public void Trashing()
    {
        content.SetActive(false);
        trashing = true;
    }
    public void TrashingExit()
    {
        content.SetActive(true);
        trashing = false;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Staff" && dragging)
        {
            if (tempStaff != null)
            {
                tempStaff.ResetOutfit();
            }
            tempStaff = collision.gameObject.GetComponent<Staff>();
            if (tempStaff && tempStaff.character.outfitId !=id)
            {
                content.SetActive(false);
                tempStaff.SetOutfit(id);
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Staff" && dragging)
        {
            Staff temp = collision.gameObject.GetComponent<Staff>();
            if (temp)
            {
                if (temp == tempStaff)
                {
                    content.SetActive(true);
                    tempStaff = null;
                }
                temp.ResetOutfit();
            }
        }
    }

    public static int[] costs =
   {
		//When negative, its gems
		300, 500, 1000,1000, 2500, 
        3000,3000, 2500, -5, 2500, 
        -10, -10,4000,4000, 5000,
        //15
        6000,5000, 5000, -15, -15, 
        6000, -20, 7000, -20, -25,
        8000,-30,-30, -50, -50, 
    };
}
