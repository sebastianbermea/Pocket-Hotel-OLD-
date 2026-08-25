using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OutfitButton : MonoBehaviour
{
    public int id, level;
    public Text coinText, wagesText;
    public Image[] extra;
    private List<Gift> gifts = new List<Gift>();
    int type = 1;
    public void ButtonClick()
    {
        if (!GC.INS.visit)
        {
            if (level <= GC.INS.level || gifts.Count > 0)
            {
                if (gifts.Count > 0)
                    GC.INS.GiftOutfit(this);
                else
                    GC.INS.BuyOufit(id);
            }
            else
                GC.INS.errorM.Error(3);
        }
        else
        {
            if (level <= GC.INS.level || gifts.Count > 0)
            {
                if (gifts.Count > 0)
                    VC.INS.GiftOutfit(this);
                else
                    VC.INS.BuyOufit(id);
            }
            else
                VC.INS.errorM.Error(3);
        }
       
       
    }
    public void Purchased()
    {
        GC.INS.gift.staffGifts.Remove(gifts[0]);
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
    private void Start()
    {
        wagesText.text = Staff.oWages[id].ToString();
        if (StaffOutfit.costs[id] > 0)
        {
            coinText.text = StaffOutfit.costs[id].ToString("n0");
        }
        else
        {
            int temp = StaffOutfit.costs[id] * -1;
            coinText.text = temp.ToString("n0");
        }
        extra[0].gameObject.SetActive(false);
        gifts = new List<Gift>();
        for (int i = 0; i < GC.INS.gift.staffGifts.Count; i++)
        {
            if (GC.INS.gift.staffGifts[i].subtype == type)
            {
                if (GC.INS.gift.staffGifts[i].id == id)
                {
                    gifts.Add(GC.INS.gift.staffGifts[i]);
                    GC.INS.gift.staffGiftsDots[1].SetActive(false);
                    GC.INS.gift.staffGifts[i].seen = true;
                }
            }
        }
        if (gifts.Count > 0)
        {
            extra[0].gameObject.SetActive(true);
            extra[0].GetComponentInChildren<Text>().text = gifts.Count.ToString();
            extra[1].gameObject.SetActive(false);
        }
    }
    private void OnEnable()
    {
        if (StaffOutfit.costs[id] > 0)
        {
            if (GC.INS.coins >= StaffOutfit.costs[id])
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
            if (GC.INS.gems >= StaffOutfit.costs[id] * -1)
            {
                coinText.color = new Color(0.6f, 0.85f, 1);
            }
            else
            {
                coinText.color = new Color(0.8f, 0.8f, 0.8f);
            }
        }
        if (level > GC.INS.level)
        {
            extra[1].gameObject.SetActive(true);
            extra[1].GetComponentInChildren<Text>().text = level.ToString();
        }
        else
        {
            extra[1].gameObject.SetActive(false);
        }

        if (GC.INS.gift.newGift)
        {
            gifts = new List<Gift>();
            for (int i = 0; i < GC.INS.gift.staffGifts.Count; i++)
            {
                if (GC.INS.gift.staffGifts[i].subtype == type)
                {
                    if (GC.INS.gift.staffGifts[i].id == id)
                    {
                        GC.INS.gift.staffGifts[i].seen = true;
                        GC.INS.gift.staffGiftsDots[1].SetActive(false);
                        gifts.Add(GC.INS.gift.staffGifts[i]);
                        GC.INS.gift.newGift = false;
                    }
                }
            }
            
        }
        if (gifts.Count > 0)
        {
            extra[0].gameObject.SetActive(true);
            extra[0].GetComponentInChildren<Text>().text = gifts.Count.ToString();
            extra[1].gameObject.SetActive(false);
        }
    }
}
