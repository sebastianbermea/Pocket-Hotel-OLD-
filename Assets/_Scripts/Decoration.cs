using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Decoration : MonoBehaviour
{
    Vector3 screenPoint, normalSize, doubleSize, boughtPos;
    bool dragging, placing, buying, bought;
    Camera mainCam;
    public GameObject preview, content, minusText;
    public float anchorY;
    int roomSizeY;
    BoxCollider2D bc;
    Animator anim;
    public bool spriteAnchor;
    public int type;
    Sprite anchoredSprite;
    int _id, number;
    Room currentRoom;
    bool trashing, gift, gifting;
    float halfWidth, halfHeight;
    SpriteRenderer contentSr;
    DecorationButton decbutton;
    // Start is called before the first frame update
    void Awake()
    {
        mainCam = Camera.main;
       
        normalSize = preview.transform.localScale;
        doubleSize = preview.transform.localScale * 2.5f;
        preview.transform.localScale = doubleSize;
        bc = GetComponent<BoxCollider2D>();
        anim = GetComponentInParent<Animator>();
    }
    public void SetObject(int id, float posX, float posY, GameObject parent, Room room, int number)
    {
        transform.parent.parent = parent.transform;
        transform.parent.localPosition = new Vector3(posX, posY, 0);
        bought = true;
        gameObject.SetActive(false);
        content.SetActive(true);
        preview.SetActive(false);
        Sprite tempSp = SM.INS.GetRoomObject(type, id);
        contentSr = content.GetComponentInChildren<SpriteRenderer>();
        contentSr.sprite = tempSp;
        preview.GetComponent<SpriteRenderer>().sprite = tempSp;
        currentRoom = room;
       
        this.number = number;
        _id = id;
        anim.enabled = false;
        boughtPos = transform.parent.localPosition;
        roomSizeY = (int)room.roomSize.y;
    }
    public void Purchased(Sprite sprite, int id)
    {
        if (spriteAnchor)
            anchoredSprite = sprite;

        contentSr = content.GetComponentInChildren<SpriteRenderer>();
        contentSr.sprite = sprite;
        preview.GetComponent<SpriteRenderer>().sprite = sprite;
        halfWidth = contentSr.bounds.size.x / 2;
        halfHeight = contentSr.bounds.size.y / 2;
        BeginDrag();
        _id = id;

        screenPoint = mainCam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0));
        screenPoint.z = 0;

        transform.parent.position = screenPoint;
    }
    public void SetGift(DecorationButton dbtn, int id)
    {
        decbutton = dbtn;
        _id = id;
        anim.enabled = false;
        content.SetActive(true);
        preview.SetActive(false);
        bc.size = Vector2.one;
        Sprite tempSp = SM.INS.GetRoomObject(type, id);
        contentSr = content.GetComponentInChildren<SpriteRenderer>();
        contentSr.sprite = tempSp;
        gift = true;
        content.transform.localScale = doubleSize;
        screenPoint = mainCam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0));
        screenPoint.z = 0;
        BeginDrag();
        transform.parent.position = screenPoint;
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
    public void GiftPurchased(Sprite sprite, int id, DecorationButton decb)
    {
        decbutton = decb;
        Purchased(sprite, id);
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
            if (gift)
                return;
            if (placing)
            {
                if (transform.parent.parent != null)
                {
                    if (CheckBounds(transform.parent.localPosition))
                    {
                        if (!buying)
                        {
                            preview.transform.localScale = normalSize;
                            if (!bought)
                            {
                                if (spriteAnchor)
                                {
                                    currentRoom.OnDecorating(type, anchoredSprite, _id);
                                    preview.SetActive(false);
                                }
                                else
                                {
                                    currentRoom.OnDecorating(type, null, _id);
                                }

                            }

                            buying = true;
                        }

                        if (anchorY != 0 && Mathf.Abs(screenPoint.y - transform.parent.parent.position.y - (anchorY * roomSizeY -(roomSizeY-1)*0.03f)) < .5f)
                        {
                            transform.parent.localPosition = new Vector3(transform.parent.localPosition.x, (anchorY>0)?(anchorY * roomSizeY - (roomSizeY - 1) * 0.03f): anchorY * roomSizeY, 0);
                        }
                        else if (anchorY != 0)
                        {

                            if (buying)
                            {
                                if (!bought)
                                {
                                    if (spriteAnchor)
                                    {
                                        preview.SetActive(true);
                                    }
                                    currentRoom.OnDecoratingOff();
                                }
                                buying = false;
                                preview.transform.localScale = doubleSize;
                            }

                        }
                    }
                    else
                    {
                        if (buying)
                        {
                            if (!bought)
                            {
                                if (spriteAnchor)
                                {
                                    preview.SetActive(true);
                                }
                                currentRoom.OnDecoratingOff();
                            }
                            preview.transform.localScale = doubleSize;
                            buying = false;
                        }


                    }

                }

            }
  

        }
    }
     private void Update()
     {
         if (Input.GetMouseButtonUp(0) && dragging)
         {
             EndDrag();
         }
     }
    //Checar si el objeto esta dentro de la zona permitida del cuarto
    private bool CheckBounds(Vector3 checking)
    {
        bool inBound = true;

        //Si el objeto se ancla a la parte de arriba hay mas espacio en la zona de arriba 
        float distanceDown = (anchorY < 0) ? .5f : .35f;
        float factorRight = 0;
        if (transform.parent.parent.localScale.x > 1)
            factorRight += 0.07f;
        if (transform.parent.parent.localScale.x > 2)
            factorRight += 0.03f;
        if (transform.parent.parent.localScale.x > 3)
            factorRight += 0.01f;
      
        if (checking.x < (-.5f - factorRight + halfWidth - (halfWidth - .15f) * factorRight*7))
            inBound = false;
        if (checking.x > (.5f + factorRight - halfWidth +(halfWidth-.15f)*factorRight*7))
            inBound = false;
        if (checking.y < -distanceDown + halfHeight - (transform.parent.parent.localScale.y - 1)*0.07f - (transform.parent.parent.localScale.y - 1)*halfHeight*.5f)
            inBound = false;
        if (checking.y > .5f - halfHeight + (transform.parent.parent.localScale.y - 1) * halfHeight * .5f)
            inBound = false;

        return inBound;
    }

   
    private void OnMouseDrag()
    {
        if (bought && !dragging)
        {
            boughtPos = transform.parent.localPosition;
            preview.transform.localScale = normalSize;
            dragging = true;
            content.SetActive(false);
            preview.SetActive(true);
            GC.INS.BeginDrag(this);
            buying = true;
            placing = true;
        }

    }
    public void SetNumber(int x)
    {
        number = x;
    }
    void EndDrag()
    {
        dragging = false;
        GC.INS.EndDrag();
        if (gift)
        {
            if (gifting)
            {
                if (decbutton == null)
                    VC.INS.AddGift(new Gift(1, type, _id, false), costs[type, _id]);
                else
                {
                    VC.INS.AddGift(new Gift(1, type, _id, true), 0);
                    decbutton.Purchased();
                    decbutton = null;
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
        if (buying)
        {
            content.SetActive(!spriteAnchor);
            Vector3 tempPos = transform.parent.localPosition;
            tempPos.z = 0;
            transform.parent.localPosition = tempPos;
            SC.INS.PlaySound(0, 11, 0);
            preview.SetActive(false);
            anim.SetTrigger("Place");
            Invoke("ResetTime", .24f);
            if (!bought)
            {
                number = currentRoom.SetDecoration(transform.parent.gameObject, _id, anchoredSprite);
                Invoke("DisableObject", .5f);
                bought = true;
                if (costs[type, _id] < 0)
                    GC.INS.AddXp(-costs[type, _id] / 5);
                else
                    GC.INS.AddXp(costs[type, _id] / 1000);
                if (decbutton == null)
                {
                    GC.INS.PurchaseMute(costs[type, _id]);
                    TextMeshPro tempText = Instantiate(minusText, transform.parent.parent.parent).GetComponentInChildren<TextMeshPro>();
                    tempText.transform.parent.position = screenPoint;

                    if (costs[type, _id] < 0)
                    {
                        tempText.color = new Color(0.5f, 0.80f, 1f);
                        tempText.text = costs[type, _id].ToString("n0");
                    }
                    else
                    {
                        tempText.text = "-" + costs[type, _id].ToString("n0");
                    }
                }
                else
                {
                    decbutton.Purchased();
                    decbutton = null;
                }
                
                /* if (spriteAnchor)
                     Destroy(transform.parent.gameObject);*/
            }
            else
            {
                currentRoom.ModifyDecoration(number, _id, type, transform.parent.gameObject);
            }
        }
        else if (bought)
        {
            transform.parent.localPosition = boughtPos;
            content.SetActive(true);
            preview.SetActive(false);
            if (trashing)
            {
                currentRoom.RemoveDecoration(transform.parent.gameObject, _id, type);
                Destroy(transform.parent.gameObject);
              
            }
        }
        else
        {
            Destroy(transform.parent.gameObject);
        }
        buying = false;

    }
    void ResetTime()
    {
        anim.enabled = false;
    }
    public void DisableObject()
    {
        gameObject.SetActive(false);
    }
    public void Trashing()
    {
        preview.SetActive(false);
        SC.INS.PlaySound(0, 14, 0);
        trashing = true;
    }
    public void TrashingExit()
    {
        preview.SetActive(true);
        trashing = false;
    }
   
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (bought||gift)
            return;
        if (collision.gameObject.tag == "Room" && dragging)
        {
            Room temp = collision.gameObject.GetComponent<Room>();
            currentRoom = temp;
            if (temp && temp.canDecorate)
            {
                if (spriteAnchor)
                {
                    switch (type)
                    {
                        case 0:
                            if (_id == temp._wallId)
                                return;
                            break;
                        case 1:
                            if (_id == temp._bedId)
                                return;
                            break;
                        case 2:
                            if (_id == temp._floorId)
                                return;
                            break;
                    }
                }
                //if (!(spriteAnchor && type == 1))
                if (!(currentRoom.isRecepcion && type == 1))
                {
                    transform.parent.parent = temp.decorationsP.transform;
                    placing = true;
                    roomSizeY = (int)transform.parent.parent.localScale.y;     
                }
 
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (bought || gift)
            return;
        if (collision.gameObject.tag == "Room" && dragging)
        {
            Room temp = collision.gameObject.GetComponent<Room>();
            if (spriteAnchor)
            {
                switch (type)
                {
                    case 0:
                        if (_id == temp._wallId)
                            return;
                        break;
                    case 1:
                        if (_id == temp._bedId)
                            return;
                        break;
                    case 2:
                        if (_id == temp._floorId)
                            return;
                        break;
                }
            }
            if (temp && !bought && temp.canDecorate)
                collision.gameObject.GetComponent<Room>().OnDecoratingOff();
            if (transform.parent.parent != null && !bought)
            {
                if (transform.parent.parent.position == collision.gameObject.transform.position)
                {
                    transform.parent.parent = null;
                    placing = false;
                    preview.transform.localScale = doubleSize;
                    bc.enabled = false;
                    bc.enabled = true;  
                }
            }
        }
    }
    public static int[,] costs =
    {
        //Paint
        {
            500,500,1000,1000,1500,
            1500,2000,2000,2500,2500,
            3000,3000,3500,3500,4000,
            4000,4500,4500,-5,-5,
            //20
            5000,5000,6000,6000,-10,
            -10,7500,-15,7500,-15,
            -25,-25,8000,8000,-40,
            -50,5000,5000,5000,5000,
            //40
             5000,5000,5000,5000,5000,
        },
        //Bed
         {
            500,1000,2500,2500,3000,
            3000,-10,-10,4000,4000,
            5000,5000,-15,-15,-25,
            -25,6000,6000,-40,-40,
            //20
            7500,7500,-50,-50,-75,
            5000,5000,5000,5000,5000,
            5000,5000,5000,5000,5000,
            5000,5000,5000,5000,5000,
            //40
             5000,5000,5000,5000,5000,
        },
         //Floor
         {
            500,500,1000,1000,1500,
            1500,2000,2000,2500,2500,
            3000,3000,3500,3500,-5,
            -5,4000,4000,4500,4500,
            //20
            5000,5000,-10,-10,6000,
            6000,-20,-20,7500,7500,
            -25,-25,-50,-50,5000,
            5000,5000,5000,5000,5000,
            //40
            5000,5000,5000,5000,5000,
        },
         //Wall object
         {
            500,500,1000,1000,1500,
            1500,2000,2000,-5,2500,
            2500,-10,3000,3000,3500,
            3500,-15,-15,4000,-25,
            //20
            -30,-30,5000,5000,-40,
            -40,6000,6000,-50,-50,
            7500,7500,8000,8000,-75,
            -75,5000,5000,5000,5000,
            //40
             5000,5000,5000,5000,5000,
        },
         //DownObject
         {
            500,500,1000,1000,2000,
            2000,2500,2500,3000,3000,
            3500,3500,-5,-5,4000,
            4000,5000,5000,-10,-10,
            //20
            6000,6000,-20,-20,6500,
            6500,7000,-25,7500,-25,
            -40,8000,8000,8500,-50,
            -50,9000,9000,-60,-60,
            //40
             -75,-75,5000,5000,5000,
        },
         //Up object
         {
            500,1000,1500,1500,2000,
            2000,-10,2500,4000,4000,
            -10,5000,-15,-15,6000,
            -25,-30,7000,7000,7000,
            //20
            -50,7500,7500,7500,-50,
            -65,5000,5000,5000,5000,
            5000,5000,5000,5000,5000,
            5000,5000,5000,5000,5000,
            //40
             5000,5000,5000,5000,5000,
        },
    };
}
