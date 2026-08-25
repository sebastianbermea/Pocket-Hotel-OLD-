using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class OutsideO : MonoBehaviour
{
    Vector3 screenPoint, normalSize, doubleSize, camTempPos;
    bool dragging, buying, bought;
    Camera mainCam;
    public GameObject preview, content, minusText, clown, guitarrist;
    Animator anim;
    int number;
    public int _id;
    bool trashing;
    public SpriteRenderer contentSr, previewSr;
    float startPos, holdCountDown;
    OutsideButton ob;
    bool gifting, gift;
    void Awake()
    {
        mainCam = Camera.main;
        normalSize = preview.transform.localScale;
        doubleSize = preview.transform.localScale * 1.5f;
        preview.transform.localScale = doubleSize;
        anim = GetComponentInParent<Animator>();
    }
    public void SetObject(Outside outside)
    {
        if (!GC.INS.visit)
            transform.parent.parent = GC.INS.roomsArrange.transform;
        else
        {
            GetComponent<BoxCollider2D>().enabled = false;
            transform.parent.parent = VC.INS.roomsArrange.transform;
        }
        transform.parent.position = new Vector3(outside.positionX, -0.1f, 0);
        startPos = outside.positionX;
        bought = true;
        holdCountDown = 2;
        content.SetActive(true);
        preview.SetActive(false);
        Sprite tempSp = SM.INS.outsideO[SM.INS.outsideO.Length-1];
        if (SM.INS.outsideO.Length>outside.id)
           tempSp = SM.INS.outsideO[outside.id];
        contentSr.sprite = tempSp;
        previewSr.sprite = tempSp;
        if(!GC.INS.visit)
            number = GC.INS.AddOutside(outside);
        _id = outside.id;
        anim.enabled = false;
        if (_id == 16)
        {
            content.transform.position = new Vector3(content.transform.position.x, content.transform.position.y, -0.1f);
        }
        if (_id == 29)
        {
            contentSr.enabled = false;
            Instantiate(clown, content.transform);
        }
        if (_id == 30)
        {
            contentSr.enabled = false;
            Instantiate(guitarrist, content.transform);
        }
    }
    public void Purchased(int id)
    {
        Sprite sprite = SM.INS.outsideO[id];
        contentSr.sprite = sprite;
        previewSr.sprite = sprite;
        BeginDrag();
        _id = id;
        screenPoint = mainCam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0));
        screenPoint.z = 0;
        transform.parent.position = screenPoint;
    }
    public void SetGift(int id, OutsideButton ob)
    {
        this.ob = ob;
        gift = true;
        Purchased(id);
    }
    public void Gift(OutsideButton obu)
    {
        ob = obu;
        Purchased(obu.id);
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
        if (bought)
        {
            holdCountDown = 2;
        }
    }

    void FixedUpdate()
    {
        if (dragging)
        {

            screenPoint = mainCam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0));
            screenPoint.z = 0;

            transform.parent.position = Vector3.MoveTowards(transform.parent.position, screenPoint, Time.fixedDeltaTime * (25 + (transform.parent.position - screenPoint).magnitude * 15));
            Vector3 tempPos = transform.parent.position;
            tempPos.z = 0;
            transform.parent.position = tempPos;

            if (!buying)
            {
                preview.transform.localScale = normalSize;
                buying = true;
            }

            if (Mathf.Abs(screenPoint.y + 0.1f) < .5f)
            {
                transform.parent.position = new Vector3(transform.parent.position.x, -0.1f, 0);
            }
            else
            {

                if (buying)
                {
                    buying = false;
                    preview.transform.localScale = doubleSize;
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

    void OnMouseDrag()
    {
        if (EventSystem.current.IsPointerOverGameObject())
            return;
        if (holdCountDown > 0)
        {
            holdCountDown -= Time.deltaTime;
            return;
        }
        ReDrag();
    }


    void ReDrag()
    {

        if (GC.INS.coins < 100)
        {
            GC.INS.errorM.Error(4);
            holdCountDown = 2;
            return;
        }

        if (!GC.INS.isDragging)
        {
            BeginDrag();
            GC.INS.BeginDrag(this);
            startPos = transform.parent.position.x;
            anim.enabled = true;
            preview.transform.localScale = normalSize;
            dragging = true;
            content.SetActive(false);
            preview.SetActive(true);
            buying = true;

        }
        else
        {
            holdCountDown = 2;
        }

    }
    void ResetTime()
    {
        anim.enabled = false;
    }
    public void SetNumber(int x)
    {
        number = x;
    }
    void EndDrag()
    {
        if (!dragging)
            return;
        dragging = false;
        GC.INS.EndDrag();
        if (gift)
        {
            if (gifting)
            {
                if (ob == null)
                    VC.INS.AddGift(new Gift(2, 0, _id, false), costs[_id]);
                else
                {
                    VC.INS.AddGift(new Gift(2, 0, _id, true), 0);
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
        holdCountDown = 2;
        if (_id == 16)
        {
            content.transform.position = new Vector3(content.transform.position.x, content.transform.position.y, -0.1f);
        }
        if (buying)
        {
            content.SetActive(true);
            Vector3 tempPos = transform.parent.position;
            tempPos.z = 0;
            transform.parent.position = tempPos;
            SC.INS.PlaySound(0, 11, 0);
            preview.SetActive(false);
            anim.SetTrigger("Place");
            Invoke("ResetTime", .24f);
            if (!bought)
            {
                number = GC.INS.AddOutside(new Outside(_id, transform.parent.position.x));
                bought = true;
                if (ob == null)
                {
                    GC.INS.PurchaseMute(costs[_id]);
                    TextMeshPro tempText = Instantiate(minusText, transform.parent.parent.parent).GetComponentInChildren<TextMeshPro>();
                    tempText.transform.parent.position = screenPoint;
                    if (costs[_id] < 0)
                    {
                        tempText.color = new Color(0.5f, 0.80f, 1f);
                        tempText.text = costs[_id].ToString("n0");
                        GC.INS.AddXp(-costs[_id] / 4);
                    }
                    else
                    {
                        GC.INS.AddXp(costs[_id] / 500);
                        tempText.text = "-" + costs[_id].ToString("n0");
                    }
                }
                else
                {
                    ob.Purchased();
                    ob = null;
                }
               
                if (costs[_id] < 0)
                    GC.INS.SetStars(-costs[_id] / 4);
                else
                    GC.INS.SetStars(costs[_id] / 70);
                    
                if (_id == 29)
                {
                    contentSr.enabled = false;
                    Instantiate(clown, content.transform);
                }
                if (_id == 30)
                {
                    contentSr.enabled = false;
                    Instantiate(guitarrist, content.transform);
                }
            }
            else
            {
                if (Mathf.Abs(transform.parent.position.x - startPos) > 0.1f)
                {
                    TextMeshPro tempText = Instantiate(minusText, transform.parent).GetComponentInChildren<TextMeshPro>();
                    tempText.transform.parent.position = screenPoint;
                    tempText.text = "-100";
                    GC.INS.PurchaseMute(100);
                    GC.INS.ModifyOutside(number, new Outside(_id, transform.parent.position.x));
                }
                else
                {
                    transform.parent.position = new Vector3(startPos, -0.1f, transform.parent.position.z);
                }
               
            }
        }
        else if (bought)
        {
            transform.parent.position = new Vector3(startPos, -0.1f, transform.parent.position.z);
            content.SetActive(true);
            preview.SetActive(false);
            if (trashing)
            {
                GC.INS.DeleteOutside(number);
                if (costs[_id] > 0)
                    GC.INS.AddCoins((int)(costs[_id] * .4f));
                else
                    GC.INS.AddCoins((int)(costs[_id] * -.4f));
                Destroy(transform.parent.gameObject);


                if (costs[_id] < 0)
                {
                    GC.INS.SetStars(costs[_id] / 4);
                }
                else
                {
                    GC.INS.SetStars(-costs[_id] / 70);
                }
            }
        }
        else
        {
            Destroy(transform.parent.gameObject);
        }
        buying = false;

    }


    public bool Trashing()
    {
        if (!dragging)
            return false;
        preview.SetActive(false);
        SC.INS.PlaySound(0, 14, 0);
        trashing = true;
        return bought;
    }
    public void TrashingExit()
    {
        preview.SetActive(true);
        trashing = false;
    }



    public static int[] costs =
    {
		//When negative, its gems
		3000, 5000,5000,-75,10000,
        -100,12500,12500,20000,-50,
        -50,12000,25000,-75,25000,
        30000,-250,30000,-125,10000,
        //20
        -150, -150, 30000,50000,-200,
        -200, -175, 50000,50000,-300,
        -300, -175, 50000,50000,12500,
    
    };
}

public class Outside
{
    int _id;
    float _positionX;
    public Outside(int id, float posX)
    {
        _id = id;
        _positionX = posX;
    }

    public int id
    {
        get { return _id; }
    }
    public float positionX
    {
        get { return _positionX; }
        set { _positionX = value; }
    }

}
