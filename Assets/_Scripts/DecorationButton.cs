using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DecorationButton : MonoBehaviour
{
    public int type, id, level;
    public Text coinText;
    public Image[] extra;
    List<Gift> gifts = new List<Gift>();
    public void ButtonClick()
    {
        if (!GC.INS.visit)
        {
            if (level <= GC.INS.level || gifts.Count > 0)
            {
                if (gifts.Count > 0)
                    GC.INS.GiftDecoration(this);
                else
                    GC.INS.BuyDecoration(id, type);
            }
            else
                GC.INS.errorM.Error(3);
        }
        else
        {
            if (level <= GC.INS.level || gifts.Count > 0)
            {
                if (gifts.Count > 0)
                    VC.INS.GiftDecoration(this);
                else
                    VC.INS.BuyDecoration(id, type);
            }
            else
                VC.INS.errorM.Error(3);
        }
       
    }
    public void Purchased()
    {
        GC.INS.gift.decorationGifts.Remove(gifts[0]);
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
        if (Decoration.costs[type, id] > 0)
        {
            coinText.text = Decoration.costs[type, id].ToString("n0");
        }
        else
        {
            int temp = Decoration.costs[type, id] * -1;
            coinText.text = temp.ToString("n0");
        }
        extra[0].gameObject.SetActive(false);
        gifts = new List<Gift>();
        for (int i = 0; i < GC.INS.gift.decorationGifts.Count; i++)
        {
            if (GC.INS.gift.decorationGifts[i].subtype == type)
            {
                if (GC.INS.gift.decorationGifts[i].id == id)
                {
                    gifts.Add(GC.INS.gift.decorationGifts[i]);
                    GC.INS.gift.decorationGiftDots[type].SetActive(false);
                    GC.INS.gift.decorationGifts[i].seen = true;
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
        if (Decoration.costs[type, id] > 0)
        {
            if (GC.INS.coins >= Decoration.costs[type, id])
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
            if (GC.INS.gems >= Decoration.costs[type, id] * -1)
            {
                coinText.color = new Color(0.3f, 0.7f, 1);
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
            for (int i = 0; i < GC.INS.gift.decorationGifts.Count; i++)
            {
                if (GC.INS.gift.decorationGifts[i].subtype == type)
                {
                    if (GC.INS.gift.decorationGifts[i].id == id)
                    {
                        gifts.Add(GC.INS.gift.decorationGifts[i]);
                        GC.INS.gift.decorationGiftDots[type].SetActive(false);
                        GC.INS.gift.newGift = false;
                        GC.INS.gift.decorationGifts[i].seen = true;
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
