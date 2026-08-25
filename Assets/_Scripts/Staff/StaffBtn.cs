using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StaffBtn : MonoBehaviour
{
    public int id;
    public Image[] headParts, outfit, body, eyes;
    public Image mouth, invIm;
    public Text sname, coinText, wages;
    Character character;
    bool isFriend;
    private List<Gift> gifts = new List<Gift>();
    int type = 0;
    public GameObject coinsOb, hiredObj;
    bool friendHired;

    private void Start()
    {

        if (isFriend)
            return;
        character = Staff.staffList[id];

        SetCharacter();
        if (Staff.costs[id] > 0)
        {
            coinText.text = Staff.costs[id].ToString("n0");
        }
        else
        {
            int temp = Staff.costs[id] * -1;
            coinText.text = temp.ToString("n0");
        }
        invIm.gameObject.SetActive(false);
        gifts = new List<Gift>();
        for (int i = 0; i < GC.INS.gift.staffGifts.Count; i++)
        {
            if (GC.INS.gift.staffGifts[i].subtype == type)
            {
                if (GC.INS.gift.staffGifts[i].id == id)
                {
                    GC.INS.gift.staffGifts[i].seen = true;
                    GC.INS.gift.staffGiftsDots[0].SetActive(false);
                    gifts.Add(GC.INS.gift.staffGifts[i]);
                }
            }
        }
        if (gifts.Count > 0)
        {
            invIm.gameObject.SetActive(true);
            invIm.GetComponentInChildren<Text>().text = gifts.Count.ToString();
        }
    }
    public void Purchased()
    {
        GC.INS.gift.staffGifts.Remove(gifts[0]);
        gifts.RemoveAt(0);
        if (gifts.Count > 0)
        {
           invIm.gameObject.SetActive(true);
            invIm.GetComponentInChildren<Text>().text = gifts.Count.ToString();
        }
        else
        {
            invIm.gameObject.SetActive(false);
        }
    }
    public void Create(Character c)
    {
        character = c;
        id = character.id;
        isFriend = true;
        SetCharacter();
    }
    void SetCharacter()
    {
        if (character.id < 20)
        {
            wages.text = "+" + Staff.wages[character.id] +"%";
        }
        else
        {
            wages.text = "+5%";
        }
       //Head
       Sprite[] hairs = SM.INS.Hairs();
        headParts[0].sprite = hairs[character.hairId];
        headParts[0].color = GC.INS.hairC[character.hairColor];
        eyes[0].sprite = SM.INS.eyes[character.eyesId * 2];
        eyes[1].sprite = SM.INS.eyes[character.eyesId * 2 + 1];
        eyes[1].color = GC.INS.eyesC[character.eyeColor];
        mouth.sprite = SM.INS.mouths[character.mouthId];
        mouth.SetNativeSize();
        //Extra
        headParts[1].sprite = SM.INS.beards[character.extraId];
        if (character.extraId < 9)
        {
            headParts[1].color = GC.INS.hairC[character.extraColor];
        }

        //Glasses
        headParts[2].sprite = SM.INS.glasses[character.glassId * 2];
        headParts[2].color = GC.INS.armazonColor[character.glassColorId];
        if (character.glassId > 0 && character.glassColor > 0)
        {
            headParts[3].sprite = SM.INS.glasses[character.glassId * 2 + 1];
            headParts[4].sprite = SM.INS.glasses[character.glassId * 2 + 1];
            headParts[3].color = GC.INS.glassColor[character.glassColor];
            headParts[4].color = GC.INS.glassColor[character.glassColor];
        }

        //SkinColor
        Sprite[] tempBody = SM.INS.Bodys(character.skinColor);
        body[0].sprite = tempBody[0];
        body[1].sprite = tempBody[1];
        body[2].sprite = tempBody[1];
        body[3].sprite = tempBody[2];
        body[4].sprite = tempBody[3];
        body[5].sprite = tempBody[4];

        sname.text = character.name;

        //Arrange Head
        headParts[0].SetNativeSize();
        Vector2 size = headParts[0].GetComponent<RectTransform>().sizeDelta;
        size *= headParts[0].GetComponent<Image>().pixelsPerUnit;
        Vector2 pixelPivot = headParts[0].GetComponent<Image>().sprite.pivot;
        Vector2 percentPivot = new Vector2(pixelPivot.x / size.x, pixelPivot.y / size.y);
        headParts[0].GetComponent<RectTransform>().pivot = percentPivot;
        //Arrange extra
        headParts[1].SetNativeSize();
        size = headParts[1].GetComponent<RectTransform>().sizeDelta;
        size *= headParts[1].GetComponent<Image>().pixelsPerUnit;
        pixelPivot = headParts[1].GetComponent<Image>().sprite.pivot;
        percentPivot = new Vector2(pixelPivot.x / size.x, pixelPivot.y / size.y);
        headParts[1].GetComponent<RectTransform>().pivot = percentPivot;

        if (character.glassId > 0)
        {
            //Arrange glasses
            headParts[2].SetNativeSize();
            size = headParts[2].GetComponent<RectTransform>().sizeDelta;
            size *= headParts[2].GetComponent<Image>().pixelsPerUnit;
            pixelPivot = headParts[2].GetComponent<Image>().sprite.pivot;
            percentPivot = new Vector2(pixelPivot.x / size.x, pixelPivot.y / size.y);
            headParts[2].GetComponent<RectTransform>().pivot = percentPivot;
            //Arrange glass 1
            headParts[3].SetNativeSize();
            size = headParts[3].GetComponent<RectTransform>().sizeDelta;
            size *= headParts[3].GetComponent<Image>().pixelsPerUnit;
            pixelPivot = headParts[3].GetComponent<Image>().sprite.pivot;
            percentPivot = new Vector2(pixelPivot.x / size.x, pixelPivot.y / size.y);
            headParts[3].GetComponent<RectTransform>().pivot = percentPivot;
            //Arrange glass 2
            headParts[4].SetNativeSize();
            size = headParts[4].GetComponent<RectTransform>().sizeDelta;
            size *= headParts[4].GetComponent<Image>().pixelsPerUnit;
            pixelPivot = headParts[4].GetComponent<Image>().sprite.pivot;
            percentPivot = new Vector2(pixelPivot.x / size.x, pixelPivot.y / size.y);
            headParts[4].GetComponent<RectTransform>().pivot = percentPivot;
        }

        if (character.isFriend)
        {
            Sprite[] tempOutfit = SM.INS.GetOutfit(character.outfitId);
            for (int i = 0; i < tempOutfit.Length; i++)
                outfit[i].sprite = tempOutfit[i];
        }
        for (int i = 0; i < outfit.Length; i++)
        {
            outfit[i].SetNativeSize();
            size = outfit[i].GetComponent<RectTransform>().sizeDelta;
            size *= outfit[i].GetComponent<Image>().pixelsPerUnit;
            pixelPivot = outfit[i].GetComponent<Image>().sprite.pivot;
            percentPivot = new Vector2(pixelPivot.x / size.x, pixelPivot.y / size.y);
            outfit[i].GetComponent<RectTransform>().pivot = percentPivot;
        }
            
        coinText.text = "1,000";
    }
    public void Purchase()
    {
        if (!GC.INS.visit)
        {
            if (!isFriend)
            {
                if (gifts.Count > 0)
                    GC.INS.StaffGift(this);
                else
                    GC.INS.BuyCharacter(Staff.staffList[id]);
            }
            else
            {
                if(friendHired)
                {
                    GC.INS.errorM.Error(10);
                    return;
                }
                GC.INS.BuyCharacter(character);
            }
        } else if (!isFriend)
        {
            if (gifts.Count > 0)
                VC.INS.StaffGift(this, Staff.staffList[id]);
            else
                VC.INS.BuyCharacter(Staff.staffList[id]);
        }
            
    }

    private void OnEnable()
    {
        if (!isFriend)
        {
            if (Staff.costs[id] > 0)
            {
                if (GC.INS.coins >= Staff.costs[id])
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
                if (GC.INS.gems >= Staff.costs[id] * -1)
                {
                    coinText.color = new Color(0.3f, 0.7f, 1);
                }
                else
                {
                    coinText.color = new Color(0.8f, 0.8f, 0.8f);
                }
            }
        }
        else
        {
            if (GC.INS.coins >= 1000)
            {
                coinText.color = new Color(1, 1, .38f);
            }
            else
            {
                coinText.color = new Color(0.8f, 0.8f, 0.8f);
            }
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
                        GC.INS.gift.staffGiftsDots[0].SetActive(false);
                        gifts.Add(GC.INS.gift.staffGifts[i]);
                        GC.INS.gift.newGift = true;
                    }
                }
            }
           
        }
        if (gifts.Count > 0)
        {
            invIm.gameObject.SetActive(true);
            invIm.GetComponentInChildren<Text>().text = gifts.Count.ToString();
        }
        if (isFriend)
        {
            if (GC.INS.staffFriendIDList.Contains(character.id))
            {
                friendHired = true;
                coinsOb.SetActive(false);
                hiredObj.SetActive(true);
            }
            else
            {
                friendHired = false;
                coinsOb.SetActive(true);
                hiredObj.SetActive(false);
            }

        }
       
    }
}
