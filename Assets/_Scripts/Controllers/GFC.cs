using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GFC : MonoBehaviour
{
    public List<Gift> gifts = new List<Gift>();
    public List<Gift> roomgifts = new List<Gift>();
    public List<Gift> outsideGifts = new List<Gift>();
    public List<Gift> decorationGifts = new List<Gift>();
    public List<Gift> staffGifts = new List<Gift>();
    public List<Gift> custGifts = new List<Gift>();
    public bool newGift;
    public int newGiftCount;
    public GameObject[] giftsDots, roomGiftDoots, decorationGiftDots, staffGiftsDots, customizeGiftDots;
    public GameObject itemDot;
    public RoomButton[] roomBtns=new RoomButton[80];
    public object TransformGiftToList()
    {
        List<Gift> newGifts = new List<Gift>();
        newGifts.AddRange(roomgifts);
        newGifts.AddRange(decorationGifts);
        newGifts.AddRange(outsideGifts);
        newGifts.AddRange(staffGifts);
        List<object> newList = new List<object>();
        for (int i = 0; i < newGifts.Count; i++)
        {

            Dictionary<string, object> temp = new Dictionary<string, object>
                {
                    { "id", newGifts[i].id},
                    { "type", newGifts[i].type },
                    { "subtype", newGifts[i].subtype },
                    { "seen", newGifts[i].seen },
                };
            newList.Add(temp);

        }
        return newList.ToArray();
    }
    public void TransformListToGift(List<object> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            if (list[i] != null)
            {
                Gift tempOut;
                Dictionary<string, object> tempDic = list[i] as Dictionary<string, object>;
                tempOut = new Gift(
                  Convert.ToInt32(tempDic["type"]),
                  Convert.ToInt32(tempDic["subtype"]),
                  Convert.ToInt32(tempDic["id"]),
                  Convert.ToBoolean(tempDic["seen"])
                );
                gifts.Add(tempOut);
                
                switch (tempOut.type)
                {
                    case 0:
                        roomgifts.Add(tempOut);
                        if (!tempOut.seen)
                        {
                            giftsDots[0].SetActive(true);
                            roomGiftDoots[tempOut.subtype].SetActive(true);
                        }
                        break;
                    case 1:
                        decorationGifts.Add(tempOut);
                        if (!tempOut.seen)
                        {
                            giftsDots[1].SetActive(true);
                            decorationGiftDots[tempOut.subtype].SetActive(true);
                        }
                        break;
                    case 2:
                        outsideGifts.Add(tempOut);

                        if (!tempOut.seen)
                        {
                            giftsDots[0].SetActive(true);
                            roomGiftDoots[5].SetActive(true);
                        }
                        break;
                    case 3:
                        staffGifts.Add(tempOut);
                        if (!tempOut.seen)
                        {
                            giftsDots[3].SetActive(true);
                            staffGiftsDots[tempOut.subtype].SetActive(true);
                        }
                        break;
                    case 4:
                        if (!tempOut.seen)
                        {
                            giftsDots[2].SetActive(true);
                            itemDot.SetActive(true);
                            GC.INS.backController.AddItem(tempOut.id);
                        }
                        break;
                    case 5:
                        custGifts.Add(tempOut);
                        giftsDots[4].SetActive(true);
                        switch (tempOut.subtype)
                        {
                            case 0:
                            case 1:
                                customizeGiftDots[0].SetActive(true);
                                break;
                            case 2:
                            case 3:
                            case 4:
                                customizeGiftDots[1].SetActive(true);
                                break;
                            case 5:
                            case 6:
                            case 7:
                            case 8:
                            case 9:
                                customizeGiftDots[2].SetActive(true);
                                break;
                            case 10:
                            case 11:
                                customizeGiftDots[3].SetActive(true);
                                break;
                        }
                        GC.INS.customPurchased[tempOut.subtype][tempOut.id] = true;
                        GC.INS.customized = true;
                        break;
                }
            }
        }

    }
    public void CheckGift()
    {
        itemDot.SetActive(false);
    }
    public void AddGiftWOCard(Dictionary<string, object> gift)
    {
        Gift tempOut;
        tempOut = new Gift(
                  Convert.ToInt32(gift["type"]),
                  Convert.ToInt32(gift["subtype"]),
                  Convert.ToInt32(gift["id"]),
                 false
                );
        gifts.Add(tempOut);
        switch (tempOut.type)
        {
            case 0:
                if (roomBtns[tempOut.id])
                {
                    roomBtns[tempOut.id].SetGifts(tempOut, roomgifts.Count);
                }
                roomgifts.Add(tempOut);
                break;
            case 1:
                decorationGifts.Add(tempOut);
                break;
            case 2:
                outsideGifts.Add(tempOut);
                break;
            case 3:
                staffGifts.Add(tempOut);

                break;
        }
    }
    public void AddGift(Dictionary<string, object> gift)
    {
        newGift = true;
        Gift tempOut;
        tempOut = new Gift(
                  Convert.ToInt32(gift["type"]),
                  Convert.ToInt32(gift["subtype"]),
                  Convert.ToInt32(gift["id"]),
                 false
                );
        gifts.Add(tempOut);
        GC.INS.gc.Set(tempOut, "", false);
        switch (tempOut.type)
        {
            case 0:
                if (roomBtns[tempOut.id])
                {
                    roomBtns[tempOut.id].SetGifts(tempOut, roomgifts.Count);
                }
                roomgifts.Add(tempOut);
                giftsDots[0].SetActive(true);
                roomGiftDoots[tempOut.subtype].SetActive(true);
                break;
            case 1:
                decorationGifts.Add(tempOut);
                giftsDots[1].SetActive(true);
                decorationGiftDots[tempOut.subtype].SetActive(true);
                break;
            case 2:
                outsideGifts.Add(tempOut);
                giftsDots[0].SetActive(true);
                roomGiftDoots[5].SetActive(true);
                break;
            case 3:
                staffGifts.Add(tempOut);
                giftsDots[3].SetActive(true);
                staffGiftsDots[0].SetActive(true);
                break;
            case 4:
                if (!tempOut.seen)
                {
                    giftsDots[2].SetActive(true);
                    itemDot.SetActive(true);
                    GC.INS.backController.AddItem(tempOut.id);
                }
                break;
            case 5:
                giftsDots[4].SetActive(true);
                switch (tempOut.subtype)
                {
                    case 0:
                    case 1:
                        customizeGiftDots[0].SetActive(true);
                        break;
                    case 2:
                    case 3:
                    case 4:
                        customizeGiftDots[1].SetActive(true);
                        break;
                    case 5:
                    case 6:
                    case 7:
                    case 8:
                    case 9:
                        customizeGiftDots[2].SetActive(true);
                        break;
                    case 10:
                    case 11:
                        customizeGiftDots[3].SetActive(true);
                        break;
                }
                GC.INS.customPurchased[tempOut.subtype][tempOut.id] = true;
                GC.INS.customized = true;
                break;
        }
    }
    public void SendGift()
    {
        AddGift(new Dictionary<string, object>
                {
                    { "id",3},
                    { "type", 5},
                    { "subtype", 1},
                });
        AddGift(new Dictionary<string, object>
                {
                    { "id",5},
                    { "type", 5},
                    { "subtype", 0},
                });
        AddGift(new Dictionary<string, object>
                {
                    { "id",5},
                    { "type", 5},
                    { "subtype", 2},
                });
        AddGift(new Dictionary<string, object>
                {
                    { "id",5},
                    { "type", 5},
                    { "subtype", 3},
                });
        AddGift(new Dictionary<string, object>
                {
                    { "id",3},
                    { "type", 5},
                    { "subtype", 4},
                });
        AddGift(new Dictionary<string, object>
                {
                    { "id",3},
                    { "type", 5},
                    { "subtype", 5},
                });
        AddGift(new Dictionary<string, object>
                {
                    { "id",4},
                    { "type", 5},
                    { "subtype", 5},
                });
        AddGift(new Dictionary<string, object>
                {
                    { "id",3},
                    { "type", 5},
                    { "subtype", 6},
                });
        AddGift(new Dictionary<string, object>
                {
                    { "id",3},
                    { "type", 5},
                    { "subtype",7},
                });
        AddGift(new Dictionary<string, object>
                {
                    { "id",3},
                    { "type", 5},
                    { "subtype", 8},
                });
        AddGift(new Dictionary<string, object>
                {
                    { "id",3},
                    { "type", 5},
                    { "subtype", 9},
                });
        AddGift(new Dictionary<string, object>
                {
                    { "id",3},
                    { "type", 5},
                    { "subtype", 10},
                });
        AddGift(new Dictionary<string, object>
                {
                    { "id",3},
                    { "type", 5},
                    { "subtype",11},
                });

    }
    public void AddGift(Dictionary<string, object> gift, string uname)
    {
        newGift = true;
        Gift tempOut;
        tempOut = new Gift(
                  Convert.ToInt32(gift["type"]),
                  Convert.ToInt32(gift["subtype"]),
                  Convert.ToInt32(gift["id"]),
                 false
                );
        gifts.Add(tempOut);
        GC.INS.gc.Set(tempOut, uname, true);
        switch (tempOut.type)
        {
            case 0:
                if (roomBtns[tempOut.id])
                {
                    roomBtns[tempOut.id].SetGifts(tempOut, roomgifts.Count);
                }
                roomgifts.Add(tempOut);
                giftsDots[0].SetActive(true);
                roomGiftDoots[tempOut.subtype].SetActive(true);
                break;
            case 1:
                decorationGifts.Add(tempOut);
                giftsDots[1].SetActive(true);
                decorationGiftDots[tempOut.subtype].SetActive(true);
                break;
            case 2:
                outsideGifts.Add(tempOut);
                giftsDots[0].SetActive(true);
                roomGiftDoots[5].SetActive(true);
                break;
            case 3:
                staffGifts.Add(tempOut);
                giftsDots[3].SetActive(true);
                staffGiftsDots[0].SetActive(true);
                break;
            case 4:
                if (!tempOut.seen)
                {
                    giftsDots[2].SetActive(true);
                    itemDot.SetActive(true);
                    GC.INS.backController.AddItem(tempOut.id);
                }
                break;
            case 5:
                giftsDots[4].SetActive(true);
                switch (tempOut.subtype)
                {
                    case 0:
                    case 1:
                        customizeGiftDots[0].SetActive(true);
                        break;
                    case 2:
                    case 3:
                    case 4:
                        customizeGiftDots[1].SetActive(true);
                        break;
                    case 5:
                    case 6:
                    case 7:
                    case 8:
                    case 9:
                        customizeGiftDots[2].SetActive(true);
                        break;
                    case 10:
                    case 11:
                        customizeGiftDots[3].SetActive(true);
                        break;
                }
                GC.INS.customPurchased[tempOut.subtype][tempOut.id] = true;
                GC.INS.customized = true;
                break;
        }
    }
    public void ReturnGift(Gift gift)
    {
        newGift = true;
        gifts.Add(gift);
        Debug.Log(gift.type);
        switch (gift.type)
        {
            case 0:
                roomgifts.Add(gift);
                break;
            case 1:
                decorationGifts.Add(gift);
                break;
            case 2:
                outsideGifts.Add(gift);
                break;
            case 3:
                staffGifts.Add(gift);
                break;
            case 4:
                GC.INS.backController.AddItem(gift.id);
                break;
        }
    }
    public void DotActive(int x)
    {
        giftsDots[x].SetActive(false);
    }
}

public class Gift
{
    int _id, _type, _subtype;
    bool _seen;

    public Gift(int type, int subtype, int id, bool seen)
    {
        _id = id;
        _type = type;
        _subtype = subtype;
        _seen = seen;
    }

    public int id
    {
        get { return _id; }
    }
    public int type
    {
        get { return _type; }
    }
    public int subtype
    {
        get { return _subtype; }
    }
    public bool seen
    {
        get { return _seen; }
        set { _seen = value; }
    }

}