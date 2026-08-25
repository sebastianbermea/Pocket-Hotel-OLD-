using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StaffHire : MonoBehaviour
{
    public GameObject content, tempSt;
    Vector3 screenPoint;
    Camera mainCam;
    bool dragging, trashing;
    Character character;
    public SpriteRenderer[] headParts, body, outfit, eyes;
    public SpriteRenderer mouthSr;
    public TextMeshPro nameText;
    Staff tempStaff;
    StaffBtn sb;
    bool gift, gifting;

    private void Awake()
    {
        mainCam = Camera.main;
    }
    public void Purchased(Character character)
    {
        BeginDrag();
        screenPoint = mainCam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0));
        screenPoint.z = 0;
        transform.parent.position = screenPoint;
        this.character = character;
        if (character.id == 0)
        {
            tempSt.SetActive(true);
            nameText.transform.gameObject.SetActive(false);
            RandomGenerator();
        }
        else
            SetCharacter(character);
    }
    public void GiftStaff(StaffBtn sbn)
    {
        sb = sbn;
        Purchased(Staff.staffList[sb.id]);
    }
    public void SetGift(Character character, StaffBtn sb)
    {
        this.sb = sb;
        gift = true;
        Rigidbody2D rig = GetComponent<Rigidbody2D>();
        Destroy(rig);
        Purchased(character);
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
    public void RandomGenerator()
    {
        bool isMen = (Random.Range(0, 5) < 3);
        nameText.gameObject.SetActive(false);
        int _mouthId = Random.Range(0, 7);
        mouthSr.sprite = SM.INS.mouths[_mouthId];
        int _eyesColor = Random.Range(0, 7);
        int _eyesId = 0;
        if (Random.Range(0, 4) == 0)
        {
            _eyesId = Random.Range(0, SM.INS.eyes.Length / 2);
        }
        int _hairId;

        if (isMen)
            _hairId = Random.Range(0, 17);
        else
            _hairId = Random.Range(16, 29);

        int _hairColor;
        if (Random.Range(0, 8) != 0)
            _hairColor = Random.Range(0, 8);
        else
            _hairColor = Random.Range(0, GC.INS.hairC.Length);
        int extraId = 0;
        int _extraColor=0;
        if (isMen && Random.Range(0, 4) == 0)
        {
            extraId = Random.Range(0, SM.INS.beards.Length);

            if (extraId < 9)
            {
                if (Random.Range(0, 2) == 0)
                    _extraColor = Random.Range(0, GC.INS.hairC.Length);
                else
                    _extraColor = _hairColor;
            }
        }
        int glassId = 0;
        int glassColorId = 0;
        if (Random.Range(0, 6) == 0)
        {
            glassId = Random.Range(0, 4);
            glassColorId = Random.Range(0, GC.INS.armazonColor.Length);

        }
        int _bodyNumber = 0;
        if (Random.Range(0, 3) != 0)
        {
            _bodyNumber = Random.Range(0, 6);
        }
        
        character = new Character(0, 0, _hairId, "temp", _hairColor, _eyesColor,0, _bodyNumber, extraId, _extraColor, glassId, glassColorId, _mouthId, _eyesId, false);
        SetCharacter(character);
    }
    public void SetCharacter(Character c)
    {
        //Head
        Sprite[] hairs = SM.INS.Hairs();
        headParts[0].sprite = hairs[c.hairId];
        headParts[0].color = GC.INS.hairC[c.hairColor];
        eyes[1].color = GC.INS.eyesC[c.eyeColor];
        eyes[0].sprite = SM.INS.eyes[c.eyesId * 2];
        eyes[1].sprite = SM.INS.eyes[c.eyesId * 2 + 1];
        mouthSr.sprite = SM.INS.mouths[c.mouthId];
        //Extra
        headParts[1].sprite = SM.INS.beards[c.extraId];
        headParts[1].color = GC.INS.hairC[c.hairColor];

        //Glasses
        headParts[2].sprite = SM.INS.glasses[c.glassId * 2];
        headParts[2].color = GC.INS.armazonColor[c.glassColorId];
        if (c.glassId > 0 && c.glassColor > 0)
        {
            headParts[3].sprite = SM.INS.glasses[c.glassId * 2 + 1];
            headParts[4].sprite = SM.INS.glasses[c.glassId * 2 + 1];
            headParts[3].color = GC.INS.glassColor[c.glassColor];
            headParts[4].color = GC.INS.glassColor[c.glassColor];
        }

        //Body
        Sprite[] tempBody = SM.INS.Bodys(c.skinColor);
        body[0].sprite = tempBody[0];
        body[1].sprite = tempBody[1];
        body[2].sprite = tempBody[1];
        body[3].sprite = tempBody[2];
        body[4].sprite = tempBody[3];
        body[5].sprite = tempBody[4];
        if (c.skinColor == 4)
            mouthSr.color = new Color(0.8f, 0.8f, 0.8f);

        //Outfit
        if (character.isFriend)
        {
            Sprite[] tempOutfit = SM.INS.GetOutfit(character.outfitId);
            for (int i = 0; i < tempOutfit.Length; i++)
                outfit[i].sprite = tempOutfit[i];
        }

        nameText.text = c.name;
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
                if (sb == null)
                    VC.INS.AddGift(new Gift(3, 0, character.id, false), Staff.costs[character.id]);
                else
                {
                    VC.INS.AddGift(new Gift(3, 0, character.id, true), 0);
                    sb.Purchased();
                    sb = null;
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
            if(!trashing && tempStaff.character.id != character.id)
            {
                if (sb == null)
                {
                  tempStaff.PurchasedCharacter(character);
                }
                else
                {
                    tempStaff.PurchasedCharacter(character, sb);
                    sb = null;
                }
            }
            else
                tempStaff.ResetCharacter();
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
                tempStaff.ResetCharacter();
            }
            tempStaff = collision.gameObject.GetComponent<Staff>();
            if (tempStaff && tempStaff.character.id != character.id)
            {
                content.SetActive(false);
                tempStaff.SetNewCharacter(character);
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
                temp.ResetCharacter();  
            }
        }
    }
}
