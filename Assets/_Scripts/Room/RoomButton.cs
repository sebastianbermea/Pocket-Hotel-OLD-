using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomButton : MonoBehaviour
{
    public int id, level, type;
    public int blocks;
    public Text coinText;
    public Image[] extra;
    private List<Gift> gifts = new List<Gift>();
    bool giftset;
    int giftN;
    public void ButtonClick()
    {
        if (!GC.INS.visit)
        {
            if (blocks + GC.INS.blocks <= GC.INS.blocksPermited)
            {
                if(level <= GC.INS.level || gifts.Count>0)
                {
                    if (gifts.Count > 0)
                        GC.INS.GiftRoom(this);
                    else
                        GC.INS.BuyRoom(id);
                }
                else
                    GC.INS.errorM.Error(3);

            }
            else
                    GC.INS.errorM.Error(6);
               
            
        }
        else
        {
            if (level <= GC.INS.level || gifts.Count>0)
            {
                if (gifts.Count > 0)
                    VC.INS.GiftRoom(this);
                else
                    VC.INS.BuyRoom(id);
            }
            else
                VC.INS.errorM.Error(3);
        }
        
    }
    private void Start()
    {
        if (Room.costs[id] > 0)
        {
            coinText.text = Room.costs[id].ToString("n0");
        }
        else
        {
            int temp = Room.costs[id] * -1;
            coinText.text = temp.ToString("n0");
        }
        extra[0].gameObject.SetActive(false);
        gifts = new List<Gift>();
        for (int i = 0; i < GC.INS.gift.roomgifts.Count; i++)
        {
            if (GC.INS.gift.roomgifts[i].id == id)
            {
                GC.INS.gift.roomgifts[i].seen = true;
                GC.INS.gift.roomGiftDoots[type].SetActive(false);
                gifts.Add(GC.INS.gift.roomgifts[i]);
            }
        }
        if (gifts.Count > 0)
        {
            extra[0].gameObject.SetActive(true);
            extra[0].GetComponentInChildren<Text>().text = gifts.Count.ToString();
            extra[2].gameObject.SetActive(false);
        }
        GC.INS.gift.roomBtns[id] = this;
    }
    public void Purchased()
    {
        GC.INS.gift.roomgifts.Remove(gifts[0]);
        gifts.RemoveAt(0);
        if (gifts.Count > 0)
        {
            extra[0].gameObject.SetActive(true);
            extra[0].GetComponentInChildren<Text>().text = gifts.Count.ToString();
        }
        else
        {
            extra[0].gameObject.SetActive(false);
        }
    }
    private void OnEnable()
    {
        if (Room.costs[id] > 0)
        {
            if (GC.INS.coins>= Room.costs[id])
            {
                coinText.color = new Color(1, 1, .38f);
            }
            else
            {
                coinText.color = new Color(0.8f, 0.8f, 0.8f);
            }

        }
        else
        {
            if (GC.INS.gems >= Room.costs[id]*-1)
            {
                coinText.color = new Color(0.3f, 0.7f, 1);
            }
            else
            {
                coinText.color = new Color(0.8f, 0.8f, 0.8f);
            }
        }
       
        if (level > GC.INS.level && gifts.Count==0)
        {
            extra[2].gameObject.SetActive(true);
            extra[2].GetComponentInChildren<Text>().text = level.ToString();
        }
        else
            extra[2].gameObject.SetActive(false);
        if (blocks + GC.INS.blocks > GC.INS.blocksPermited && !GC.INS.visit)
        {
            extra[1].gameObject.SetActive(true);
        }else
            extra[1].gameObject.SetActive(false);
        if (giftset)
        {
            GC.INS.gift.roomgifts[giftN].seen = true;
            GC.INS.gift.roomGiftDoots[type].SetActive(false);
            GC.INS.gift.newGift = false;
            giftset = false;
        }
    }
    public void SetGifts(Gift gift, int giftNumber)
    {
        gifts.Add(gift);
        extra[0].gameObject.SetActive(true);
        extra[0].GetComponentInChildren<Text>().text = gifts.Count.ToString();
        giftset = true;
        giftN = giftNumber;
        extra[2].gameObject.SetActive(false);
    }
}
