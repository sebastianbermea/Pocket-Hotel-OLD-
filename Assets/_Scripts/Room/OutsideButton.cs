using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OutsideButton : MonoBehaviour
{
    public int id, level, inv;
    public Text coinText;
    public Image[] extra;
    private List<Gift> gifts = new List<Gift>();

    public void ButtonClick()
    {
        if (level <= GC.INS.level || gifts.Count > 0)
        {
            if (gifts.Count > 0)
                GC.INS.OutsideGift(this);
            else
                GC.INS.BuyOutside(id);
        }
        else
        {
            GC.INS.errorM.Error(3);
        }


    }
    private void Start()
    {
        if (OutsideO.costs[id] > 0)
        {
            coinText.text = OutsideO.costs[id].ToString("n0");
        }
        else
        {
            int temp = OutsideO.costs[id] * -1;
            coinText.text = temp.ToString("n0");
        }
        extra[0].gameObject.SetActive(false);
        gifts = new List<Gift>();
        for (int i = 0; i < GC.INS.gift.outsideGifts.Count; i++)
        {
            if (GC.INS.gift.outsideGifts[i].id == id)
            {
                GC.INS.gift.outsideGifts[i].seen = true;
                GC.INS.gift.roomGiftDoots[5].SetActive(false);
                gifts.Add(GC.INS.gift.outsideGifts[i]);
            }
        }
        if (gifts.Count > 0)
        {
            extra[0].gameObject.SetActive(true);
            extra[0].GetComponentInChildren<Text>().text = gifts.Count.ToString();
        }
    }
    public void Purchased()
    {
        GC.INS.gift.outsideGifts.Remove(gifts[0]);
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
        if (OutsideO.costs[id] > 0)
        {
            if (GC.INS.coins >= OutsideO.costs[id])
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
            if (GC.INS.gems >= OutsideO.costs[id] * -1)
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
            for (int i = 0; i < GC.INS.gift.outsideGifts.Count; i++)
            {
                if (GC.INS.gift.outsideGifts[i].id == id)
                {
                    GC.INS.gift.outsideGifts[i].seen = true;
                    GC.INS.gift.roomGiftDoots[5].SetActive(false);
                    gifts.Add(GC.INS.gift.outsideGifts[i]);
                    GC.INS.gift.newGift = false;
                }
            }
            if (gifts.Count > 0)
            {
                extra[0].gameObject.SetActive(true);
                extra[0].GetComponentInChildren<Text>().text = gifts.Count.ToString();
            }
        }
        
    }
}
