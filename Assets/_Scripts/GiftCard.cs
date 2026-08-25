using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GiftCard : MonoBehaviour
{
    public Image bigI, smallI, cardI, colorI;
    public Sprite[] roomsS, cardTypes;
    public GameObject characterObj, fromObj, carD1, carD2, outfitObj, colorObj, headObj;
    public Text giftType, fromText;
    public CharacterSet cSet, outSet, headSet;
    List<Gift> giftList=new List<Gift>();
    List<bool> giftFromFriend = new List<bool>();
    List<string> giftFriendName = new List<string>();
    public Color[] bodyColors;

    public bool showing;
    public Animator anim;
    public void Set(Gift gift, string uname, bool friend)
    {
        
        if (showing)
        {
            giftFromFriend.Add(friend);
            if (friend)
            {
                giftFriendName.Add(uname);
            }

            giftList.Add(gift);
            return;
        }
        gameObject.SetActive(true);
        characterObj.SetActive(false);
        smallI.gameObject.SetActive(false);
        bigI.gameObject.SetActive(false);
        outfitObj.SetActive(false);
        colorObj.SetActive(false);
        headObj.SetActive(false);
        cardI.sprite = cardTypes[Random.Range(0,3)];
        if (Random.Range(0, 3)==0)
        {
            carD1.SetActive(true);
        }
        if (Random.Range(0, 3) == 0)
            carD2.SetActive(true);
        SC.INS.PlaySound(0, 15, 0);

        showing = true;
        if (!friend)
        {
            fromObj.SetActive(false);
        }
        else
        {
            fromObj.SetActive(true);
            fromText.text = uname;
        }

        switch (gift.type)
        {
            case 0:
                giftType.text = GC.INS.t.GetText(125);
                bigI.gameObject.SetActive(true);
                bigI.sprite = roomsS[gift.id];
                break;
            case 1:
                giftType.text = GC.INS.t.GetText(126);
                smallI.gameObject.SetActive(true);
                smallI.sprite = SM.INS.GetRoomObject(gift.subtype, gift.id);
                break;
            case 2:
                giftType.text = GC.INS.t.GetText(127);
                bigI.gameObject.SetActive(true);
                bigI.sprite = SM.INS.outsideO[gift.id];
                break;
            case 3:
                giftType.text = GC.INS.t.GetText(128);
                characterObj.SetActive(true);
                cSet.SetCharacter(Staff.staffList[gift.id]);
                break;
            case 4:
                giftType.text = GC.INS.t.GetText(129);
                smallI.gameObject.SetActive(true);
                smallI.sprite = SM.INS.items[gift.id];
                break;
            case 5:
                giftType.text = GC.INS.t.GetText(130 + gift.subtype);
                switch (gift.subtype)
                {
                    case 0:
                        colorObj.SetActive(true);
                        colorI.color = bodyColors[gift.id];
                        break;
                    case 1:
                        outfitObj.SetActive(true);
                        outSet.SetOutfit(gift.id);
                        break;
                    case 2:
                        headObj.SetActive(true);
                        headSet.SetMouth(gift.id);
                        break;
                    case 3:
                        headObj.SetActive(true);
                        headSet.SetExtra(gift.id);
                        break;
                    case 4:
                        colorObj.SetActive(true);
                        colorI.color = GC.INS.hairC[gift.id];
                        break;
                    case 5:
                        headObj.SetActive(true);
                        headSet.SetEyes(gift.id);
                        break;
                    case 6:
                        colorObj.SetActive(true);
                        colorI.color = GC.INS.eyesC[gift.id];
                        break;
                    case 7:
                        headObj.SetActive(true);
                        headSet.SetGlasses(gift.id);
                        break;
                    case 8:
                        colorObj.SetActive(true);
                        colorI.color = GC.INS.armazonColor[gift.id];
                        break;
                    case 9:
                        colorObj.SetActive(true);
                        colorI.color = GC.INS.glassColor[gift.id];
                        break;
                    case 10:
                        headObj.SetActive(true);
                        headSet.SetHair(gift.id);
                        break;
                    case 11:
                        colorObj.SetActive(true);
                        colorI.color = GC.INS.hairC[gift.id];
                        break;
                }
                break;
      
        }
    }
   
    public void Close()
    {
        anim.SetTrigger("Off");
        showing = false;
        if (giftList.Count > 0)
        {
            Invoke("Next", .25f);
        }
        else
        {
            Invoke("Inactive", .25f);
        }
    }
    void Inactive()
    {
        gameObject.SetActive(false);
    }
    public void Next()
    {
        if (giftList.Count < 1)
            return;
        if (giftFromFriend[0])
        {
            Set(giftList[0],giftFriendName[0], true);
            giftFriendName.RemoveAt(0);
        }
        else
            Set(giftList[0], "", false);
        giftList.RemoveAt(0);
    }
}
