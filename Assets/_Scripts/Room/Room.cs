using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class Room : MonoBehaviour
{
    //Componentes necesarios
    BoxCollider2D bc;
    Rigidbody2D rig;
    Animator anim;
    Camera mainCam;

    public GameObject content, checkerObj, particle, editingP, minusText;
    public SpriteRenderer outline;
    public Color particleColor;
    GameObject checker;

    [HideInInspector]
    public bool placed, trashing;

    public bool canDecorate, isRecepcion, hasStaff;

    bool dragging, firstTry, placing, occupied, editing, reDrag;
    public bool bought, fix;

    float speed, holdCountDown;

    Vector3 screenPoint, startPos, camTempPos;

    //Propiedades del cuarto

    public int _wallId, _bedId, _floorId;
    public int id, type;
    public int roomTime = 20, roomCoins = 1;
    [HideInInspector]
    public int number;
    public SlotController slotController;
    [HideInInspector]
    public List<int> slotsIds;
    public GameObject costumerCountObj;
    public TextMeshPro rateTxt;
    SpriteRenderer[] slotsSr;
    int costumerCount;

    [Header("Decorations")]
    public GameObject decorationsP;
    public GameObject decorationsEditing, decorationBar, decorationBarWhite;
    public GameObject bedObj, backsObj, floorsObj;
    public SpriteRenderer decBarW;
    SpriteRenderer bedSr;
    //Sin decoracion BackSpr-- FrontSprt
    SpriteRenderer[] backsSr, floorsSr;
    List<GameObject> decorationsList = new List<GameObject>();
    List<float[]> decorations = new List<float[]>();
    List<Character> characters = new List<Character>();
    bool tapped = false;
    [HideInInspector]
    public Vector2 roomSize;
    public Staff[] staff;

    int blocks;
    public GameObject lightP;
    RoomButton rb;
    bool gifting, gift, gifted;
    int addedStars;
    bool overlaped;
    private void Awake()
    {
        //Obtener componentes
        bc = GetComponent<BoxCollider2D>();
        rig = GetComponent<Rigidbody2D>();
        anim = GetComponentInParent<Animator>();
        mainCam = Camera.main;

        speed = 25;


        //Obtener checker de hijos
        checker = transform.GetChild(0).gameObject;

        if (bedObj)
            bedSr = bedObj.GetComponent<SpriteRenderer>();
        backsSr = backsObj.GetComponentsInChildren<SpriteRenderer>();
        floorsSr = floorsObj.GetComponentsInChildren<SpriteRenderer>();
        if (isRecepcion)
        {
            Destroy(rig);
            bc.size = Vector2.one;
            Destroy(checker);
            gameObject.tag = "Room";
            placed = true;
            anim.enabled = false;
        }
        if (costumerCountObj)
            slotsSr = costumerCountObj.GetComponentsInChildren<SpriteRenderer>();

        roomSize = transform.localScale;
        blocks = (int)(roomSize.x * roomSize.y);
        if (rateTxt)
            rateTxt.text = (3600 / roomTime * roomCoins * slotController.slots.Count).ToString() + "/hour";
    }
    void SetContent(bool x)
    {
        slotController.gameObject.SetActive(x);
        if (bedObj)
        {
            if (x)
                bedSr.color = new Color(1, 1, 1, 1);
            else
                bedSr.color = new Color(1, 1, 1, .3f);
        }
        if (x)
            outline.color = new Color(1, 1, 1, 1);
        else
            outline.color = new Color(1, 1, 1, .3f);
        for (int i = 0; i < backsSr.Length; i++)
        {
            if (x)
                backsSr[i].color = new Color(1, 1, 1, 1);
            else
                backsSr[i].color = new Color(1, 1, 1, .3f);
        }
        for (int i = 0; i < floorsSr.Length; i++)
        {
            if (x)
                floorsSr[i].color = new Color(1, 1, 1, 1);
            else
                floorsSr[i].color = new Color(1, 1, 1, .3f);
        }
        if (hasStaff)
        {
            for (int i = 0; i < staff.Length; i++)
            {
                staff[i].transform.parent.gameObject.SetActive(x);
            }
        }
    }

    public void Purchased()
    {

        //Ajustar posision
        if (transform.localScale.x % 2 == 0)
            transform.localPosition = new Vector3(transform.localPosition.x + 0.5f, transform.localPosition.y, 0);
        if (transform.localScale.y % 2 == 0)
            transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y + 0.5f, 0);

        content.transform.localPosition = transform.localPosition;
        //Mostrar habitacion previa para colocar y ocultar la habitacion real
        SetContent(false);

        //Ni comprado ni puesto
        bought = false;
        placed = false;

        //Para que no colsione con si mismo
        gameObject.tag = "Untagged";


        screenPoint = mainCam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0));
        screenPoint.z = 0;
        transform.parent.position = screenPoint;

        //Drag despues de compra
        BeginDrag();
    }
    public void SetGift(RoomButton rb)
    {
        this.rb = rb;
        Destroy(rig);
        bc.size = Vector2.one;
        Destroy(checker);
        gameObject.tag = "Room";
        anim.enabled = false;
        gift = true;
        Purchased();
    }
    public void GiftPurchased(RoomButton rb)
    {
        this.rb = rb;
        Purchased();
    }
    void BeginDrag()
    {
        dragging = true;
        if (!placed)
        {
            //Para checar colisiones con otros cuartos
            bc.size = new Vector2(.95f + (1f / transform.localScale.x), .95f + (1f / transform.localScale.y));
            firstTry = true;

        }
        else
        {
            holdCountDown = 2;
        }
    }

    void FixedUpdate()
    {
        if (dragging)
        {

            if (!placed)
            {

                screenPoint = mainCam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0));
                screenPoint.z = 0;
                if (bought && !bc.enabled)
                {

                    if (((Vector2)startPos - (Vector2)screenPoint).magnitude > .5f)
                    {
                        bc.enabled = true;

                    }
                    else
                    {
                        placing = false;
                    }
                }

                if (placing && !occupied)
                {
                    if (!firstTry)
                    {
                        firstTry = true;
                    }
                    transform.parent.position = Vector3.MoveTowards(transform.parent.position, SnapToGrid(screenPoint), Time.fixedDeltaTime * speed);
                }
                else
                {
                    if (firstTry && bc.enabled)
                    {

                        firstTry = false;
                        bc.enabled = false;
                        bc.enabled = true;

                    }
                    transform.parent.position = Vector3.MoveTowards(transform.parent.position, screenPoint, Time.fixedDeltaTime * (speed + (transform.parent.position - screenPoint).magnitude * 15));
                }
            }
            else if (!GC.INS.CheckUI())
            {
                if (holdCountDown > 0)
                {
                    holdCountDown -= 1 * Time.fixedDeltaTime;
                }
                else
                {
                    if ((camTempPos - mainCam.transform.position).magnitude < 0.3f)
                        ReDrag();

                }

            }

        }

        if (editing)
            if (((Vector2)transform.position - (Vector2)mainCam.transform.position).magnitude > 0.3f)
                EndEditing();
    }



    private void Update()
    {
        if (dragging)
        {
            if (Input.GetMouseButtonUp(0))
            {
                EndDrag();
            }
        }
    }
    private Vector3 SnapToGrid(Vector3 dragPos)
    {
        dragPos.x = Mathf.Round(dragPos.x);
        dragPos.y = (Mathf.Round(dragPos.y));
        placing = true;
        return dragPos;
    }

    void ReDrag()
    {

        if (isRecepcion)
        {
            GC.INS.errorM.Error(4);
            holdCountDown = 1.8f;
            return;
        }

        if (placed && !GC.INS.isDragging && !fix)
        {
            gifted = false;
            bc.enabled = false;
            reDrag = true;
            placed = false;
            rig = gameObject.AddComponent<Rigidbody2D>() as Rigidbody2D;
            rig.isKinematic = true;
            BeginDrag();
            GC.INS.BeginDrag(this);
            checker = Instantiate(checkerObj, transform);
            gameObject.tag = "Untagged";
            SetContent(false);
            startPos = transform.parent.position;
            anim.enabled = true;
            firstTry = false;
            transform.parent.position = screenPoint;
            for (int i = 0; i < slotsIds.Count; i++)
            {
                GC.INS.RemoveSlot(slotsIds[i]);
            }
        }
        else
        {
            holdCountDown = 1.7f;
        }

    }
    void SetRoom()
    {
        Invoke("ResetTime", .24f);
        Destroy(rig);
        bc.size = Vector2.one;
        Invoke("DestroyChecker", .2f);
        gameObject.tag = "Room";
        placed = true;
        Vector3 temp = transform.parent.position;
        temp.x = Mathf.Round(temp.x);
        temp.y = Mathf.Round(temp.y);
        //Acomodar con maximo de 200 hacia arriba
        temp.z = 200 - temp.y;
        transform.parent.position = temp;
        SetContent(true);
        //Reset slots ignoring with costumer
        if (bought)
        {
            Costumer[] tempCostumers = content.GetComponentsInChildren<Costumer>();
            if (tempCostumers.Length > 0)
            {
                List<int> ignoreIds = new List<int>();
                for (int i = 0; i < tempCostumers.Length; i++)
                {
                    if (tempCostumers[i].slot != null)
                        ignoreIds.Add(tempCostumers[i].slot.id);

                }
                for (int i = 0; i < slotsIds.Count; i++)
                {
                    if (!ignoreIds.Contains(slotsIds[i]))
                    {
                        GC.INS.slotsID.Add(slotsIds[i]);
                    }

                }
            }
            else
            {
                for (int i = 0; i < slotsIds.Count; i++)
                {
                    GC.INS.slotsID.Add(slotsIds[i]);
                }
            }

        }
    }
    void ResetTime()
    {
        Vector3 temp = transform.parent.position;
        temp.z = 200 - temp.y;
        transform.parent.position = temp;
        bc.enabled = true;
        anim.enabled = false;
    }
    void EndDrag()
    {
        dragging = false;
        if (!placed)
        {
            if (gift)
            {
                if (gifting)
                {
                    if (rb == null)
                        VC.INS.AddGift(new Gift(0, type, id, false), costs[id]);
                    else
                    {
                        VC.INS.AddGift(new Gift(0, type, id, true), 0);
                        rb.Purchased();
                        rb = null;
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
            if (placing && !occupied && !trashing)
            {
                anim.SetTrigger("Place");
                var main = Instantiate(particle, transform.parent).GetComponent<ParticleSystem>().main;
                main.startColor = particleColor;
                Instantiate(particle, transform.parent);
                SetRoom();
                SC.INS.PlaySound(0, 11, 0);
                if (bought)
                {
                    if ((Vector2)transform.parent.position != (Vector2)startPos && GC.INS.coins >= (100 * (GC.INS.replacedRoomsCount + 1)))
                    {
                        TextMeshPro tempText = Instantiate(minusText, transform.parent).GetComponentInChildren<TextMeshPro>();
                        tempText.transform.parent.position = screenPoint;
                        tempText.text = "-" + (100 * (GC.INS.replacedRoomsCount + 1)).ToString("n0");
                        GC.INS.PurchaseMute(100 * (GC.INS.replacedRoomsCount + 1));
                        GC.INS.replacedRoomsCount++;
                        //Change if can decorate
                        if (canDecorate)
                        {
                            //Debug.Log("Modify: " + (decorations == null));
                            GC.INS.ModifyRoom(number, new RoomC(
                                id,
                                (int)transform.parent.position.x,
                                (int)transform.parent.position.y,
                                _wallId, _bedId, _floorId, decorations
                                ));
                        }
                        else if (hasStaff)
                        {
                            //Debug.Log("Modify: " + (characters == null));
                            GC.INS.ModifyRoom(number, new RoomC(
                                id,
                                (int)transform.parent.position.x,
                                (int)transform.parent.position.y,
                                characters
                                ));
                        }
                        else
                        {
                            GC.INS.ModifyRoom(number, new RoomC(
                                id,
                                (int)transform.parent.position.x,
                                (int)transform.parent.position.y
                                ));
                        }
                    }
                    else
                    {
                        if (GC.INS.coins < (100 * (GC.INS.replacedRoomsCount + 1)))
                        {
                            GC.INS.errorM.Error(0);
                        }
                        transform.parent.position = startPos;
                    }



                }
                else //New
                {
                    if (costs[id] < 0)
                        GC.INS.AddXp(-costs[id] / 5);
                    else
                        GC.INS.AddXp(costs[id] / 2500);
                    slotController.Create(5, roomCoins, roomTime, this);
                    if (costs[id] < 0)
                        GC.INS.SetStars((int)(-costs[id] * 1.5f));
                    else
                        GC.INS.SetStars(costs[id] / 150);

                    if (slotsIds.Count > 0)
                    {
                        if (costs[id] < 0)
                            GC.INS.SetStars(-costs[id] / 2);
                        else
                            GC.INS.SetStars(costs[id] / 500);
                    }

                    if (canDecorate)
                    {
                        GC.INS.SetStars(-80 + (costs[id] / 500));
                        number = GC.INS.AddRoom(new RoomC(
                             id,
                            (int)transform.parent.position.x,
                            (int)transform.parent.position.y,
                            _wallId, _bedId, _floorId, decorations
                            ), this);
                        addedStars = 24;
                    }
                    else if (hasStaff)
                    {
                        for (int i = 0; i < staff.Length; i++)
                        {
                            //Outfit id 
                            characters.Add(staff[i].RandomGenerator(this, i));
                        }
                        number = GC.INS.AddRoom(new RoomC(
                             id,
                            (int)transform.parent.position.x,
                            (int)transform.parent.position.y,
                            characters
                            ), this);
                    }
                    else
                    {
                        number = GC.INS.AddRoom(new RoomC(
                             id,
                            (int)transform.parent.position.x,
                            (int)transform.parent.position.y
                            ), this);
                    }
                    GC.INS.EndDrag();

                    if (id == 13 || id == 15 || id == 16 || id == 24 || id == 25)
                    {
                        GC.INS.SetStars(costs[id] / 100);
                    }
                    bought = true;
                    GC.INS.SetBlocks(blocks);
                    if (rb == null)
                    {
                        GC.INS.PurchaseMute(costs[id]);
                        GC.INS.dm.AddTask(8, 1);
                        TextMeshPro tempText = Instantiate(minusText, transform.parent).GetComponentInChildren<TextMeshPro>();
                        tempText.transform.parent.position = screenPoint;
                        if (costs[id] < 0)
                        {
                            tempText.color = new Color(0.5f, 0.80f, 1f);
                            tempText.text = costs[id].ToString("n0");
                        }
                        else
                        {
                            tempText.text = "-" + costs[id].ToString("n0");
                        }
                        if (GC.INS.tutoOn)
                        {
                            if (id == 0 && GC.INS.tuto.current == 4)
                                GC.INS.tuto.AddRoom();
                            else if (id == 13 && GC.INS.tuto.current == 15)
                                GC.INS.tuto.Next();
                            else if (id == 7 && GC.INS.tuto.current == 20)
                                GC.INS.tuto.Next();
                        }
                    }
                    else
                    {
                        rb.Purchased();
                        gifted = true;
                        rb = null;
                    }

                    if (!canDecorate)
                        SetRecomendations();


                }

            }
            else if (bought)
            {
                if (trashing)
                {
                    GC.INS.blockX2 = true;
                    if (costs[id] > 0)
                        GC.INS.AddCoins((int)(costs[id] * .4f));
                    else
                        GC.INS.AddGems((int)(costs[id] * -.4f));
                    DestroyRoom();

                }
                else
                {
                    SetRoom();
                    transform.parent.position = startPos;
                }

            }
            else
            {
                placed = false;
                Destroy(transform.parent.gameObject);

                if (GC.INS.tutoOn)
                {
                    if (id == 13 && GC.INS.tuto.current == 15)
                        GC.INS.tuto.blocks[6].SetActive(false);
                    else if (id == 7 && GC.INS.tuto.current == 20)
                        GC.INS.tuto.blocks[8].SetActive(false);
                }
            }
        }
        if (GC.INS.isDragging)
        {
            GC.INS.EndDrag();
        }

    }

    public void Occupied(bool oc)
    {
        occupied = oc;
        if (!firstTry && !oc)
        {
            firstTry = true;
        }
    }

    #region MouseEvents

    void OnMouseDown()
    {
        if (EventSystem.current.IsPointerOverGameObject())
            return;
        camTempPos = mainCam.transform.position;
        BeginDrag();
        holdCountDown = 1.7f;
    }
    private void OnMouseUpAsButton()
    {
        if (camTempPos == mainCam.transform.position && placed && tapped && !GC.INS.shopping)
        {
            mainCam.transform.parent.GetComponentInChildren<CameraController>().RoomFocus(transform.localScale.x - 1, transform.position);
            editingP.SetActive(true);
            ChangeCostumers();
            if (canDecorate)
                decorationsEditing.SetActive(true);
            Invoke("CheckEditing", 1.2f);
            if (canDecorate)
            {
                for (int i = 0; i < decorationsList.Count; i++)
                {
                    decorationsList[i].transform.GetChild(0).gameObject.SetActive(true);
                }
            }
        }
        tapped = true;
        Invoke("ResetTapped", .5f);
    }
    void OnMouseUp()
    {
        if (dragging)
            EndDrag();
    }
    void CheckEditing()
    {
        if (((Vector2)transform.position - (Vector2)mainCam.transform.position).magnitude < 0.3f)
        {
            editing = true;
        }
        else
            EndEditing();

    }

    void ResetTapped()
    {
        tapped = false;
    }
    #endregion

    void ChangeCostumers()
    {
        if (costumerCountObj)
        {
            for (int i = 0; i < slotsSr.Length; i++)
            {
                if (i < costumerCount)
                    slotsSr[i].color = new Color(1, 1, 1, 1);
                else
                    slotsSr[i].color = new Color(.1f, .1f, .1f, 1);

            }
        }
    }
    void EndEditing()
    {
        if (editing && canDecorate)
        {
            for (int i = 0; i < decorationsList.Count; i++)
            {
                decorationsList[i].transform.GetChild(0).gameObject.SetActive(false);
            }
        }
        editing = false;
        editingP.SetActive(editing);
        decorationsEditing.SetActive(false);
    }
    public bool Trashing()
    {
        if (!dragging)
            return false;
        if (bought)
            slotController.gameObject.SetActive(false);
        content.SetActive(false);
        placing = false;
        trashing = true;
        SC.INS.PlaySound(0, 14, 0);
        return bought;
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
    public void TrashingExit()
    {
        if (!dragging)
            return;
        if (bought)
        {
            slotController.gameObject.SetActive(false);
        }
        content.SetActive(true);
        trashing = false;
    }
    public void DecorationEditing(bool x)
    {
        if (!IsInvoking("DecorationEditingOff") && !editingP.activeInHierarchy && isActiveAndEnabled)
            decorationsEditing.SetActive(x);
    }
    public void SetCostumer()
    {
        costumerCount++;
        if (editing)
            ChangeCostumers();
    }
    public void ByeCostumer()
    {
        costumerCount--;
        if (editing)
            ChangeCostumers();
    }
    void SetRecomendations()
    {
        if (id == 7)
            GC.INS.haveGym = true;
        else if (id == 8)
            GC.INS.haveRestaurant = true;
        else if (id == 9)
            GC.INS.haveCinema = true;
    }

    public void Create(RoomC room)
    {
        //Ajustar posision
        if (transform.localScale.x % 2 == 0)
            transform.localPosition = new Vector3(transform.localPosition.x + 0.5f, transform.localPosition.y, 0);
        if (transform.localScale.y % 2 == 0)
            transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y + 0.5f, 0);

        content.transform.localPosition = transform.localPosition;
        transform.parent.parent = GC.INS.roomsArrange.transform;
        transform.parent.position = new Vector3(room.positionX, room.positionY, 200 - room.positionY);


        if (canDecorate)
        {
            _wallId = room.wallType;
            _bedId = room.bedType;
            _floorId = room.floorType;
            decorations = room.decorations ?? new List<float[]>();
            SetDecorations(room.wallType, room.bedType, room.floorType, room.decorations);
            if (isRecepcion)
            {
                characters = room.characters;
                SetStaff();
                number = GC.INS.AddRoom(new RoomC(
                         id,
                        (int)transform.parent.position.x,
                        (int)transform.parent.position.y,
                        _wallId, _bedId, _floorId, decorations, characters
                        ), this);
            }
            else
            {
                number = GC.INS.AddRoom(new RoomC(
                         id,
                        (int)transform.parent.position.x,
                        (int)transform.parent.position.y,
                        _wallId, _bedId, _floorId, decorations
                        ), this);
            }

        }
        else if (hasStaff)
        {
            characters = room.characters;
            SetStaff();
            number = GC.INS.AddRoom(new RoomC(
                             id,
                            (int)transform.parent.position.x,
                            (int)transform.parent.position.y,
                            characters), this);
        }
        else
        {
            number = GC.INS.AddRoom(new RoomC(
                             id,
                            (int)transform.parent.position.x,
                            (int)transform.parent.position.y
                            ), this);
        }
        if (!isRecepcion)
            slotController.Create(5, roomCoins, roomTime, this);

        Destroy(rig);
        bought = true;
        bc.size = Vector2.one;
        Destroy(checker);
        gameObject.tag = "Room";
        placed = true;
        Vector3 temp = transform.parent.position;
        temp.x = Mathf.Round(temp.x);
        temp.y = Mathf.Round(temp.y);
        //Acomodar con maximo de 200 hacia arriba
        temp.z = 200 - temp.y;
        transform.parent.position = temp;
        SetContent(true);
        ResetTime();
        if (!isRecepcion)
            GC.INS.SetBlocks(blocks);

        if (!canDecorate)
            SetRecomendations();
        dragging = false;
    }
    void DestroyChecker()
    {
        Destroy(checker);
    }
    void DestroyRoom()
    {
        GC.INS.DeleteRoom(number, this);
        placed = false;
        for (int i = 0; i < slotsIds.Count; i++)
        {
            GC.INS.RemoveSlot(slotsIds[i]);
        }
        GC.INS.SetBlocks(-blocks);
        if (costs[id] < 0)
            GC.INS.SetStars((int)(costs[id] * 1.5f));
        else
            GC.INS.SetStars(-costs[id] / 150);

        if (slotsIds.Count > 0)
        {
            if (costs[id] < 0)
                GC.INS.SetStars(costs[id] / 2);
            else
                GC.INS.SetStars(-costs[id] / 500);
        }
        if (canDecorate)
            GC.INS.SetStars(80 - (costs[id] / 500));
        if (addedStars > 240)
            GC.INS.SetStars(-240);
        else
            GC.INS.SetStars(-addedStars);
        Destroy(transform.parent.gameObject);
    }
    public void CreateVisit(RoomC room)
    {
        //Ajustar posision
        if (transform.localScale.x % 2 == 0)
            transform.localPosition = new Vector3(transform.localPosition.x + 0.5f, transform.localPosition.y, 0);
        if (transform.localScale.y % 2 == 0)
            transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y + 0.5f, 0);

        content.transform.localPosition = transform.localPosition;
        transform.parent.parent = VC.INS.roomsArrange.transform;
        transform.parent.position = new Vector3(room.positionX, room.positionY, 200 - room.positionY);

        if (!isRecepcion)
            slotController.Create(5, roomCoins, roomTime, this);

        if (canDecorate)
        {
            _wallId = room.wallType;
            _bedId = room.bedType;
            _floorId = room.floorType;
            decorations = room.decorations ?? new List<float[]>();
            SetDecorations(room.wallType, room.bedType, room.floorType, room.decorations);
            if (isRecepcion)
            {
                characters = room.characters;
                SetStaff();
                number = VC.INS.AddRoom(new RoomC(
                         id,
                        (int)transform.parent.position.x,
                        (int)transform.parent.position.y,
                        _wallId, _bedId, _floorId, decorations, characters
                        ), this);
            }
            else
            {
                number = VC.INS.AddRoom(new RoomC(
                         id,
                        (int)transform.parent.position.x,
                        (int)transform.parent.position.y,
                        _wallId, _bedId, _floorId, decorations
                        ), this);
            }

        }
        else if (hasStaff)
        {
            characters = room.characters;
            SetStaff();
            number = VC.INS.AddRoom(new RoomC(
                             id,
                            (int)transform.parent.position.x,
                            (int)transform.parent.position.y,
                            characters), this);
        }
        else
        {
            number = VC.INS.AddRoom(new RoomC(
                             id,
                            (int)transform.parent.position.x,
                            (int)transform.parent.position.y
                            ), this);
        }

        Destroy(rig);
        bought = true;
        bc.size = Vector2.one;
        Destroy(checker);
        bc.enabled = false;


        gameObject.tag = "Room";
        placed = true;
        Vector3 temp = transform.parent.position;
        temp.x = Mathf.Round(temp.x);
        temp.y = Mathf.Round(temp.y);
        //Acomodar con maximo de 200 hacia arriba
        temp.z = 200 - temp.y;
        transform.parent.position = temp;
        SetContent(true);
        dragging = false;
    }

    void SetStaff()
    {
        if (characters == null)
        {
            Debug.LogError("ERROR STAFF!!!!!!!!!!!!!!!!!!!!!!!!!\n\n Room Id: " + id);
        }
        for (int i = 0; i < characters.Count; i++)
        {
            staff[i].SetCharacterStart(characters[i], this, i);
        }
    }
    public void ChangeStaff(Character c, int number)
    {
        characters[number] = c;
        if (isRecepcion)
        {
            GC.INS.ModifyRoom(number, new RoomC(
                            this.id,
                            (int)transform.parent.position.x,
                            (int)transform.parent.position.y,
                            _wallId, _bedId, _floorId, decorations, characters
                            ));
        }
        else
        {
            GC.INS.ModifyRoom(this.number, new RoomC(
                            id,
                            (int)transform.parent.position.x,
                            (int)transform.parent.position.y,
                            characters
                            ));
        }

    }
    public List<Character> RecepcionCreate()
    {
        characters.Add(staff[0].RandomGenerator(this, 0));
        return characters;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Room")
        {
            placing = true;

            if (placed)
            {
                Debug.LogError("Overlap: " + name);
            }
        }

    }
    private void OnTriggerExit2D(Collider2D collision)
    {

        if (collision.gameObject.tag == "Room")
        {
            placing = false;
        }/*else if(collision.gameObject.tag == "Decoration")
        {
			Debug.Log(collision.gameObject== null);
			OnDecoratingOff();
        }*/

    }
    #region Decorating
    int tempType;

    Sprite tempSprite;
    public void SetDecorations(int wallId, int bedId, int floorId, List<float[]> decorationsL)
    {
        if (wallId > 0)
        {
            for (int i = 0; i < backsSr.Length; i++)
            {
                backsSr[i].sprite = SM.INS.GetRoomObject(0, wallId);
            }
        }
        if (bedId > 0)
        {
            bedSr.sprite = SM.INS.GetRoomObject(1, bedId);
        }
        if (floorId > 0)
        {
            for (int i = 0; i < floorsSr.Length; i++)
            {
                floorsSr[i].sprite = SM.INS.GetRoomObject(2, floorId);
            }
        }

        //Null safety
        if (decorationsL == null)
            decorationsL = new List<float[]>();

        for (int i = 0; i < decorationsL.Count; i++)
        {
            decorationsList.Add(GC.INS.SetDecorationsToRoom(
                    new float[4] { decorationsL[i][0], decorationsL[i][1], decorationsL[i][2], decorationsL[i][3] }, decorationsP.transform.gameObject, this, i));
        }

        //Add added stars

        //Wall
        if (Decoration.costs[0, wallId] > 0)
            addedStars = Decoration.costs[0, wallId] / 83;
        else
            addedStars = -Decoration.costs[0, wallId] * 4;

        //Bed
        if (Decoration.costs[1, bedId] > 0)
            addedStars += Decoration.costs[1, bedId] / 83;
        else
            addedStars -= Decoration.costs[1, bedId] * 4;

        //Floor
        if (Decoration.costs[2, floorId] > 0)
            addedStars += Decoration.costs[2, floorId] / 83;
        else
            addedStars -= Decoration.costs[2, floorId] * 4;

        //Objects
        for (int i = 0; i < decorationsL.Count; i++)
        {
            if (Decoration.costs[(int)decorationsL[i][1], (int)decorationsL[i][0]] > 0)
                addedStars += Decoration.costs[(int)decorationsL[i][1], (int)decorationsL[i][0]] / 83;
            else
                addedStars -= Decoration.costs[(int)decorationsL[i][1], (int)decorationsL[i][0]] * 4;
        }

        //Maximo de estrellas agregadas por un cuarto decorado 240
        if (addedStars > 240)
            decorationBar.transform.localScale = new Vector3(.8f, decorationBar.transform.localScale.y);
        else
            decorationBar.transform.localScale = new Vector3(addedStars * .1f / 30f, decorationBar.transform.localScale.y);

        //Debug.Log("Set Decoration added stars: " + addedStars);
    }
    int tempStars = 0;
    public void OnDecorating(int type, Sprite sprite, int id)
    {
        if (canDecorate)
        {
            //Check if adding
            int oldId = 0;
            tempType = type;
            if (sprite != null)
            {
                switch (type)
                {
                    case 0:
                        oldId = _wallId;
                        tempSprite = backsSr[0].sprite;
                        for (int i = 0; i < backsSr.Length; i++)
                        {
                            backsSr[i].sprite = sprite;
                        }
                        break;
                    case 1:
                        oldId = _bedId;
                        tempSprite = bedSr.sprite;
                        bedSr.sprite = sprite;
                        break;

                    case 2:
                        oldId = _floorId;
                        tempSprite = floorsSr[0].sprite;
                        for (int i = 0; i < floorsSr.Length; i++)
                        {
                            floorsSr[i].sprite = sprite;
                        }
                        break;
                }

                tempStars = ((Decoration.costs[0, id] > 0) ? Decoration.costs[0, id] / 83 : Decoration.costs[0, id] * -4) //New Added stars
                - ((Decoration.costs[0, oldId] > 0) ? Decoration.costs[0, oldId] / 83 : Decoration.costs[0, oldId] * -4); //Old added stars

                if (tempStars >= 0)
                {
                    if (addedStars < 240)
                    {
                        if (addedStars + tempStars >= 240)
                        {
                            decorationBarWhite.transform.localScale = new Vector3(.8f, decorationBarWhite.transform.localScale.y);
                        }
                        else
                        {
                            decorationBarWhite.transform.localScale = new Vector3((addedStars + tempStars) * .1f / 30f, decorationBarWhite.transform.localScale.y);
                        }
                    }
                }
                else
                {
                    if (addedStars + tempStars < 240)
                    {
                        decBarW.color = new Color(1, 0.5f, 0.5f);
                        if (addedStars < 240)
                            decorationBarWhite.transform.localScale = new Vector3(addedStars * .1f / 30f, decorationBarWhite.transform.localScale.y);
                        else
                            decorationBarWhite.transform.localScale = new Vector3(.8f, decorationBarWhite.transform.localScale.y);

                        decorationBar.transform.localScale = new Vector3((addedStars + tempStars) * .1f / 30f, decorationBar.transform.localScale.y);
                    }
                    else
                    {
                        decorationBarWhite.transform.localScale = new Vector3(.8f, decorationBarWhite.transform.localScale.y);
                        decorationBar.transform.localScale = new Vector3(.8f, decorationBar.transform.localScale.y);
                    }
                    
                }
            }
            else
            {
                if (addedStars < 240)
                {
                    if (Decoration.costs[tempType, id] > 0)
                        tempStars = Decoration.costs[tempType, id] / 83;
                    else
                        tempStars = Decoration.costs[tempType, id] * -4;

                    if (addedStars + tempStars >= 240)
                    {
                        decorationBarWhite.transform.localScale = new Vector3(.8f, decorationBarWhite.transform.localScale.y);
                    }
                    else
                    {
                        decorationBarWhite.transform.localScale = new Vector3((addedStars + tempStars) * .1f / 30f, decorationBarWhite.transform.localScale.y);
                    }
                }
            }
        }
    }
    public void OnDecoratingOff()
    {
        if (!canDecorate)
            return;
        if (tempStars < 0)
        {
            if (addedStars >= 240)
                decorationBar.transform.localScale = new Vector3(.8f, decorationBar.transform.localScale.y);
            else
                decorationBar.transform.localScale = new Vector3(addedStars * .1f / 30f, decorationBarWhite.transform.localScale.y);
            decBarW.color = new Color(1, 1, 1);
        }
        if (addedStars >= 240)
        {
            decorationBarWhite.transform.localScale = new Vector3(.8f, decorationBarWhite.transform.localScale.y);
        }
        else
        {
            decorationBarWhite.transform.localScale = new Vector3(addedStars * .1f / 30f, decorationBarWhite.transform.localScale.y);
        }
        if (tempSprite != null)
        {

            switch (tempType)
            {
                case 0:
                    for (int i = 0; i < backsSr.Length; i++)
                    {
                        backsSr[i].sprite = tempSprite;
                    }
                    break;
                case 1:
                    bedSr.sprite = tempSprite;
                    break;
                case 2:
                    for (int i = 0; i < floorsSr.Length; i++)
                    {
                        floorsSr[i].sprite = tempSprite;
                    }
                    break;
            }
        }
        tempStars = 0;
    }
    public int SetDecoration(GameObject obj, int id, Sprite sprite)
    {
        decorationsEditing.SetActive(true);
        int oldId = 0;
        GC.INS.dm.AddTask(9, 1);
        Invoke("DecorationEditingOff", 3);
        if (GC.INS.tutoOn && GC.INS.tuto.current == 12)
        {
            GC.INS.tuto.Next();
        }
        if (tempSprite != null)
        {
            switch (tempType)
            {
                case 0:
                    GC.INS.dm.AddTask(10, 1);
                    oldId = _wallId;
                    _wallId = id;
                    for (int i = 0; i < backsSr.Length; i++)
                    {
                        backsSr[i].sprite = sprite;
                    }
                    break;
                case 1:
                    GC.INS.dm.AddTask(12, 1);
                    oldId = _bedId;
                    _bedId = id;
                    bedSr.sprite = sprite;
                    break;
                case 2:
                    GC.INS.dm.AddTask(11, 1);
                    oldId = _floorId;
                    _floorId = id;
                    for (int i = 0; i < floorsSr.Length; i++)
                    {
                        floorsSr[i].sprite = sprite;
                    }
                    break;
            }

            tempStars = ((Decoration.costs[tempType, id] > 0) ? Decoration.costs[tempType, id] / 83 : Decoration.costs[tempType, id] * -4) //New Added stars
                - ((Decoration.costs[tempType, oldId] > 0) ? Decoration.costs[tempType, oldId] / 83 : Decoration.costs[tempType, oldId] * -4); //Old added stars

            if (tempStars < 0)
            {
                decBarW.color = new Color(1, 1, 1);
            }
            Destroy(obj);
        }
        else
        {
            GC.INS.dm.AddTask(10 + tempType, 1);
            decorationsList.Add(obj);
            if (decorationsP)
                obj.transform.parent = decorationsP.transform;
            decorations.Add(new float[] { id, tempType, obj.transform.localPosition.x, obj.transform.localPosition.y });

            if (Decoration.costs[tempType, id] > 0)
                tempStars = Decoration.costs[tempType, id] / 83;
            else
                tempStars = Decoration.costs[tempType, id] * -4;

        }
        if (addedStars < 240)
        {
            //Debug.Log("Added stars: " + addedStars + "  temp stars:" + tempStars);
            if (addedStars + tempStars > 240)
                GC.INS.SetStars((240 - addedStars) * 2); //Maximo de cuarto 240
            else
                GC.INS.SetStars(tempStars * 2); //SetStars

            addedStars += tempStars;
            if (addedStars >= 240)
            {
                decorationBar.transform.localScale = new Vector3(.8f, decorationBar.transform.localScale.y);
                decorationBarWhite.transform.localScale = new Vector3(.8f, decorationBarWhite.transform.localScale.y);
            }
            else
            {
                decorationBar.transform.localScale = new Vector3(addedStars * .1f / 30f, decorationBar.transform.localScale.y);
                decorationBarWhite.transform.localScale = new Vector3(addedStars * .1f / 30f, decorationBarWhite.transform.localScale.y);
            }
        }
        

        tempSprite = null;
        if (isRecepcion)
        {
            GC.INS.ModifyRoom(number, new RoomC(
                            this.id,
                            (int)transform.parent.position.x,
                            (int)transform.parent.position.y,
                            _wallId, _bedId, _floorId, decorations, characters
                            ));
        }
        else
        {
            GC.INS.ModifyRoom(number, new RoomC(
                            this.id,
                            (int)transform.parent.position.x,
                            (int)transform.parent.position.y,
                            _wallId, _bedId, _floorId, decorations
                            ));
        }
        return decorationsList.Count - 1;
    }

    public void ModifyDecoration(int n, int id, int type, GameObject obj)
    {
        decorationsList[n] = obj;
        decorations[n] = new float[] { id, type, obj.transform.localPosition.x, obj.transform.localPosition.y };
        if (isRecepcion)
        {
            GC.INS.ModifyRoom(number, new RoomC(
                            this.id,
                            (int)transform.parent.position.x,
                            (int)transform.parent.position.y,
                            _wallId, _bedId, _floorId, decorations, characters
                            ));
        }
        else
        {
            GC.INS.ModifyRoom(number, new RoomC(
                            this.id,
                            (int)transform.parent.position.x,
                            (int)transform.parent.position.y,
                            _wallId, _bedId, _floorId, decorations
                            ));
        }
    }

    public void RemoveDecoration(GameObject obj, int id, int type)
    {
        int index = decorationsList.IndexOf(obj);
        decorationsList.RemoveAt(index);
        for (int i = 0; i < decorationsList.Count; i++)
        {
            decorationsList[i].GetComponentInChildren<Decoration>().SetNumber(i);
        }
        decorations.RemoveAt(index);

        if (Decoration.costs[type, id] > 0)
            tempStars = Decoration.costs[type, id] / 83;
        else
            tempStars = Decoration.costs[type, id] * -4;

        //Maximo de estrellas agregadas por un cuarto decorado 240
        if (addedStars - tempStars < 240)
        {
            if (addedStars > 240)
            {
                GC.INS.SetStars((240 - (addedStars-tempStars)) * -2);
            }
            else
            {
                GC.INS.SetStars(tempStars * -2);
            }
        }
        Debug.Log("Removed: Added stars: " + addedStars + "  temp stars:" + tempStars);
        addedStars -= tempStars;

        if (addedStars > 240)
            decorationBar.transform.localScale = new Vector3(.8f, decorationBar.transform.localScale.y);
        else
            decorationBar.transform.localScale = new Vector3(addedStars * .1f / 30f, decorationBar.transform.localScale.y);

        decorationBarWhite.transform.localScale = decorationBar.transform.localScale;
        
        if (isRecepcion)
        {
            GC.INS.ModifyRoom(number, new RoomC(
                            this.id,
                            (int)transform.parent.position.x,
                            (int)transform.parent.position.y,
                            _wallId, _bedId, _floorId, decorations, characters
                            ));
        }
        else
        {
            GC.INS.ModifyRoom(number, new RoomC(
                            this.id,
                            (int)transform.parent.position.x,
                            (int)transform.parent.position.y,
                            _wallId, _bedId, _floorId, decorations
                            ));
        }
    }

    void DecorationEditingOff()
    {
        if (!GC.INS.decorating && !editingP.activeInHierarchy)
            decorationsEditing.SetActive(false);
    }
    #endregion

    public void OverLap()
    {
        if (overlaped)
            return;
        overlaped = true;
        Debug.Log("Room overlap: " + id);
        SC.INS.StopSound();
        if (reDrag)
        {
            transform.parent.position = startPos;
            if (canDecorate)
            {
                //Debug.Log("Modify: " + (decorations == null));
                GC.INS.ModifyRoom(number, new RoomC(
                    id,
                    (int)transform.parent.position.x,
                    (int)transform.parent.position.y,
                    _wallId, _bedId, _floorId, decorations
                    ));
            }
            else if (hasStaff)
            {
                //Debug.Log("Modify: " + (characters == null));
                GC.INS.ModifyRoom(number, new RoomC(
                    id,
                    (int)transform.parent.position.x,
                    (int)transform.parent.position.y,
                    characters
                    ));
            }
            else
            {
                GC.INS.ModifyRoom(number, new RoomC(
                    id,
                    (int)transform.parent.position.x,
                    (int)transform.parent.position.y
                    ));
            }
            Debug.Log("Back where you belong");
            return;
        }
        if (!gifted)
        {
            if (costs[id] > 0)
                GC.INS.coins += costs[id];
            else
                GC.INS.gems -= costs[id];

            GC.INS.coinsText.text = GC.INS.coins.ToString("n0");
            GC.INS.gemsText.text = GC.INS.gems.ToString("n0");
        }
        else
        {
            GC.INS.gift.AddGiftWOCard(new Dictionary<string, object>
                {
                    { "id",id},
                    { "type", 0},
                    { "subtype", type},
                });
        }
        DestroyRoom();
    }

    public void SetWork(bool work, bool now)
    {
        if (!work)
        {
            lightP.SetActive(true);
            if (now)
            {
                staff[0].transform.parent.GetComponentInChildren<Animator>().SetTrigger("Walk");
                staff[0].transform.parent.GetComponentInChildren<Animator>().SetTrigger("Sleep");
            }
            else
            {
                lightP.GetComponent<Animator>().Play("Dark", -1, 0);
                staff[0].transform.parent.GetComponentInChildren<Animator>().Play("Sleeping", -1, 0);
            }
        }
        else
        {
            lightP.SetActive(false);
            staff[0].transform.parent.GetComponentInChildren<Animator>().SetTrigger("Iddle");
        }
    }
    public static int[] costs =
    {
		//When negative, its gems
		3000, 5500,18000,-75,40000,
        -150,-250,5000,20000,30000,
        38500,60000,75000,5000,37500,
		//15
		12500,15000,3500,12500,-125,
        25000, 32000, 22500,10000,10000,
        15000,38000,65000,120000, -450,
		//30
		-500,-250,-150,-200,45000,
        150000,30000,42000,-250,-275,
        -300,-350,-400,90000,75000,
		//45
		-450,175000,-300,-200,-300,
        75000,-250,-300,-300,200,

    };
}


public class RoomC
{
    int _id, _positionX, _positionY, _wallId, _bedId, _floorId;
    List<float[]> _decorations;
    List<Character> _characters;
    bool _hasDecorations, _hasStaff;
    public RoomC(int id, int positionX, int positionY, int wallType, int bedType, int floorType, List<float[]> decorations)
    {
        _id = id;
        _positionX = positionX;
        _positionY = positionY;
        _wallId = wallType;
        _bedId = bedType;
        _floorId = floorType;
        _decorations = decorations;
        _hasDecorations = true;
    }
    public RoomC(int id, int positionX, int positionY, List<Character> characters)
    {
        _id = id;
        _positionX = positionX;
        _positionY = positionY;
        _wallId = wallType;
        _bedId = bedType;
        _floorId = floorType;
        _characters = characters;
        _hasStaff = true;
    }
    public RoomC(int id, int positionX, int positionY)
    {
        _id = id;
        _positionX = positionX;
        _positionY = positionY;
        _decorations = null;
    }
    public RoomC(int id, int positionX, int positionY, int wallType, int bedType, int floorType, List<float[]> decorations, List<Character> characters)
    {
        _id = id;
        _positionX = positionX;
        _positionY = positionY;
        _wallId = wallType;
        _bedId = bedType;
        _floorId = floorType;
        _decorations = decorations;
        _hasDecorations = true;
        _characters = characters;
        _hasStaff = true;
    }
    /*public RoomC(int id, int positionX, int positionY, int wallType, int bedType, int floorType, List<object> decorations)
	{
		_id = id;
		_positionX = positionX;
		_positionY = positionY;
		_wallId = wallType;
		_bedId = bedType;
		_floorId = floorType;
		_decorations = new List<float[]>();
		for (int i = 0; i < decorations.Count; i++)
		{
			Dictionary<string, object> tempDic = decorations[i] as Dictionary<string, object>;
			float[] tempFloat = new float[4];
			tempFloat[0] = Convert.ToSingle(tempDic["id"]);
			tempFloat[1] = Convert.ToSingle(tempDic["type"]);
			tempFloat[2] = Convert.ToSingle(tempDic["posX"]);
			tempFloat[3] = Convert.ToSingle(tempDic["posY"]);
			_decorations.Add(tempFloat);

		}
		_hasDecorations = true;
	}*/
    public static List<float[]> TransformDecorationToList(List<object> decorations)
    {
        List<float[]> tempList = new List<float[]>();
        for (int i = 0; i < decorations.Count; i++)
        {
            Dictionary<string, object> tempDic = decorations[i] as Dictionary<string, object>;
            float[] tempFloat = new float[4];
            tempFloat[0] = Convert.ToSingle(tempDic["id"]);
            tempFloat[1] = Convert.ToSingle(tempDic["type"]);
            tempFloat[2] = Convert.ToSingle(tempDic["posX"]);
            tempFloat[3] = Convert.ToSingle(tempDic["posY"]);
            tempList.Add(tempFloat);
        }
        return tempList;
    }
    public static List<Character> TransformToCharacterList(List<object> characterList)
    {
        List<Character> tempList = new List<Character>();

        for (int i = 0; i < characterList.Count; i++)
        {
            Dictionary<string, object> tempDic = characterList[i] as Dictionary<string, object>;
            Character tempChar = new Character
                (Convert.ToInt32(tempDic["id"]),
                Convert.ToInt32(tempDic["outfitId"]),
                Convert.ToInt32(tempDic["hairId"]),
                tempDic["name"].ToString(),
                Convert.ToInt32(tempDic["hairColor"]),
                Convert.ToInt32(tempDic["eyeColor"]),
                Convert.ToInt32(tempDic["glassColor"]),
                Convert.ToInt32(tempDic["skinColor"]),
                Convert.ToInt32(tempDic["extraId"]),
                Convert.ToInt32(tempDic["extraColor"]),
                Convert.ToInt32(tempDic["glassId"]),
                Convert.ToInt32(tempDic["glassColorId"]),
                Convert.ToInt32(tempDic["mouthId"]),
                Convert.ToInt32(tempDic["eyesId"]),
                Convert.ToBoolean(tempDic["isFriend"])
                );
            tempList.Add(tempChar);
        }

        return tempList;
    }
    public int id
    {
        get { return _id; }
    }
    public int positionX
    {
        get { return _positionX; }
    }
    public int positionY
    {
        get { return _positionY; }
    }
    public int wallType
    {
        get { return _wallId; }
    }
    public int bedType
    {
        get { return _bedId; }
    }

    public int floorType
    {
        get { return _floorId; }
    }
    public List<float[]> decorations
    {
        get { return _decorations; }
    }
    public List<Character> characters
    {
        get { return _characters; }
    }
    public List<object> decorationsAsMap
    {
        get
        {
            List<object> tempObj = new List<object>();
            for (int i = 0; i < _decorations.Count; i++)
            {
                Dictionary<string, float> tempMap = new Dictionary<string, float>
                {
                    { "id", _decorations[i][0]},
                    { "type",  _decorations[i][1]},
                    { "posX",  _decorations[i][2]},
                    { "posY",  _decorations[i][3]},

                };
                tempObj.Add(tempMap);
            }
            return tempObj;
        }
    }
    public List<object> characterAsMap
    {
        get
        {
            List<object> tempObj = new List<object>();
            for (int i = 0; i < _characters.Count; i++)
            {
                Dictionary<string, object> tempMap = new Dictionary<string, object>
                {
                    { "id", _characters[i].id},
                    { "outfitId", _characters[i].outfitId},
                    { "hairId", _characters[i].hairId},
                    { "name", _characters[i].name},
                    { "hairColor", _characters[i].hairColor},
                    { "eyeColor", _characters[i].eyeColor},
                    { "glassColor", _characters[i].glassColor},
                    { "skinColor", _characters[i].skinColor},
                    { "extraId", _characters[i].extraId},
                    { "extraColor", _characters[i].extraColor},
                    { "glassId", _characters[i].glassId},
                    { "glassColorId", _characters[i].glassColorId},
                    { "mouthId", _characters[i].mouthId},
                    { "eyesId", _characters[i].eyesId},
                    { "isFriend", _characters[i].isFriend},
                };
                tempObj.Add(tempMap);
            }
            return tempObj;
        }
    }
    public bool hasDecorations
    {
        get { return _hasDecorations; }
    }
    public bool hasStaff
    {
        get { return _hasStaff; }
    }
}

