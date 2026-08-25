using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class VC : MonoBehaviour
{

    #region Shop
    public GameObject buyingCanvas, shopPanel;
    public GameObject[] shopsPanels;
    List<List<GameObject>> roomsShopLists = new List<List<GameObject>>();
    List<List<GameObject>> decorationsShopLists = new List<List<GameObject>>();
    List<List<GameObject>> hotelShopLists = new List<List<GameObject>>();
    List<List<GameObject>> staffShopLists = new List<List<GameObject>>();
    List<GameObject> roomsPanels = new List<GameObject>();
    List<GameObject> decorationsPanels = new List<GameObject>();
    List<GameObject> hotelPanels = new List<GameObject>();
    List<GameObject> staffPanels = new List<GameObject>();
    List<Animator> roomsShopButtons = new List<Animator>();
    List<Animator> decorationsShopButtons = new List<Animator>();
    List<Animator> hotelShopButtons = new List<Animator>();
    List<Animator> staffShopButtons = new List<Animator>();
    public Text shopCountText;
    public GameObject prevListButton, nextListButton;
    int listNumber, shopPanelActive, subShopPanelActive;
    public GameObject lockedShop, unlockedShop;
    bool lockShop;
    #endregion
    List<RoomC> rooms = new List<RoomC>();
    List<Room> roomControllers = new List<Room>();
    [HideInInspector]
    public List<Staff> janitors = new List<Staff>();
    [HideInInspector]
    public List<Staff> plumbers = new List<Staff>();
    [HideInInspector]
    public List<Staff> electicists = new List<Staff>();
    [HideInInspector]
    public List<Staff> officinist = new List<Staff>();
    [HideInInspector]
    public List<Staff> keyBuilder = new List<Staff>();
    [HideInInspector]
    public List<int> dust = new List<int>();
    [HideInInspector]
    public List<int> pipe = new List<int>();
    [HideInInspector]
    public List<int> electricity = new List<int>();
    [HideInInspector]
    public List<int> complaint = new List<int>();
    [HideInInspector]
    public List<int> key = new List<int>();

    List<Slot> slots = new List<Slot>();
    [HideInInspector]
    public List<int> slotsID = new List<int>();

    public Text hotelTitle, usernameText;
    public GameObject loadingPanel, roomsArrange, costumersArrange, minusText, shopButtons, giftButton, gift, coinBag, itemG, tipJar;
    int starsCount;
    float stars;
    public Image starsBar;
    public static VC INS { get; private set; }
    bool work = true, gifting;
    float timeToSpawn, spawnStartTime = 40;
    public Text coinText, gemText, giftText, giftCoinsText, giftGemsText;
    public Image giftI;
    public Sprite giftEmpty, giftFill;

    List<Gift> giftList = new List<Gift>(), tempGiftList = new List<Gift>();
    int tempGiftCoins, tempGiftGems;
    public ErrorM errorM;
    string username;
    List<object> friendGiftList;
    public GameObject requestJobBtn;
    public JobReq jobr;
    int backId;
    public bool[] iap;
    public GameObject tutoPanel, pointer;
    public Text tutoText;
    public Animator tutoAnim;
    int blocksId, blocksPermited;
    public GameObject outsideO, parkingObj, sendGiftBtn, firstGiftAnim, homeBtn;
    public Image starsI, medalI;
    int prestige;
    ParkingO parking;
    public bool userVisit;
    bool noti, firstGift;
    public CharacterSet playerCharacter;

    public void SetUserVisit()
    {
        userVisit = true;
        Invoke("ResetUserVisist", 25);
    }
    void ResetUserVisist()
    {

    }
    private void Awake()
    {
        if (INS == null)
            INS = this;
        else
        {
            Debug.Log("Duplicated VC");
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        iap = new bool[8];
        for (int i = 0; i < GC.INS.canvas.Length; i++)
            GC.INS.canvas[i].SetActive(false);
        loadingPanel.SetActive(true);
        if (GC.INS.tutoOn)
        {
            giftButton.SetActive(false);
            homeBtn.SetActive(false);
            requestJobBtn.SetActive(false);
        }

        Fire.INS.GetFriendData(FRC.INS.visitId);
        GetShopLists();
        SetCreditCard();
        GC.INS.dm.AddTask(7, 1);
        GC.INS.AddXp(1);
        firstGift = PlayerPrefs.GetInt("firstGift") == 0;
    }

    void GetShopLists()
    {
        for (int i = 0; i < shopsPanels[0].transform.childCount; i++)
        {
            GameObject tempGameObject = shopsPanels[0].transform.GetChild(i).gameObject;
            if (tempGameObject.name == "PaperI")
                continue;
            if (tempGameObject.GetComponent<Button>() == null && tempGameObject.GetComponent<Text>() == null)
            {
                roomsPanels.Add(tempGameObject);
                List<GameObject> tempList = new List<GameObject>();
                for (int j = 0; j < tempGameObject.transform.childCount; j++)
                {
                    tempList.Add(tempGameObject.transform.GetChild(j).gameObject);
                }
                roomsShopLists.Add(tempList);
            }
            else
            {
                roomsShopButtons.Add(tempGameObject.GetComponent<Animator>());
            }
        }
        for (int i = 0; i < shopsPanels[3].transform.childCount; i++)
        {
            GameObject tempGameObject = shopsPanels[3].transform.GetChild(i).gameObject;
            if (tempGameObject.name == "PaperI")
                continue;
            if (tempGameObject.GetComponent<Button>() == null)
            {
                staffPanels.Add(tempGameObject);
                List<GameObject> tempList = new List<GameObject>();
                for (int j = 0; j < tempGameObject.transform.childCount; j++)
                {
                    tempList.Add(tempGameObject.transform.GetChild(j).gameObject);
                }
                staffShopLists.Add(tempList);
            }
            else
            {
                staffShopButtons.Add(tempGameObject.GetComponent<Animator>());
            }
        }
        for (int i = 0; i < shopsPanels[1].transform.childCount; i++)
        {
            GameObject tempGameObject = shopsPanels[1].transform.GetChild(i).gameObject;
            if (tempGameObject.name == "PaperI")
                continue;
            if (tempGameObject.GetComponent<Button>() == null)
            {
                decorationsPanels.Add(tempGameObject);
                List<GameObject> tempList = new List<GameObject>();
                for (int j = 0; j < tempGameObject.transform.childCount; j++)
                {
                    tempList.Add(tempGameObject.transform.GetChild(j).gameObject);
                }
                decorationsShopLists.Add(tempList);
            }
            else
            {
                decorationsShopButtons.Add(tempGameObject.GetComponent<Animator>());
            }
        }
        for (int i = 0; i < shopsPanels[2].transform.childCount; i++)
        {
            GameObject tempGameObject = shopsPanels[2].transform.GetChild(i).gameObject;
            if (tempGameObject.name == "PaperI")
                continue;
            if (tempGameObject.GetComponent<Button>() == null)
            {
                hotelPanels.Add(tempGameObject);
                List<GameObject> tempList = new List<GameObject>();
                for (int j = 0; j < tempGameObject.transform.childCount; j++)
                {
                    tempList.Add(tempGameObject.transform.GetChild(j).gameObject);
                }
                hotelShopLists.Add(tempList);
            }
            else
            {
                hotelShopButtons.Add(tempGameObject.GetComponent<Animator>());
            }
        }
    }
    public void Bye()
    {
        if (gifting)
        {
            CancelGifts();
        }
        FRC.INS.Bye();
        GC.INS.backController.ChangeB(GC.INS.backId);
        SceneManager.UnloadSceneAsync("Scene2");
    }
    public void EndDrag()
    {
        GC.INS.isDragging = false;
        buyingCanvas.SetActive(true);
        shopPanel.SetActive(lockShop);
        OpenSubShop(subShopPanelActive);
        if (giftList.Count > 0) giftI.sprite = giftEmpty;
    }
    public void BeginDrag()
    {
        GC.INS.isDragging = true;
        buyingCanvas.SetActive(false);
        giftI.sprite = giftFill;
    }

    public void OpenShop(int x)
    {
        for (int i = 0; i < shopsPanels.Length; i++)
            shopsPanels[i].SetActive(false);
        shopsPanels[x].SetActive(true);
        shopPanel.SetActive(true);
        shopPanelActive = x;
        OpenSubShop(0);
        SC.INS.PlaySound(0, 13, 0);
    }
    public void OpenSubShop(int x)
    {
        listNumber = 0;
        subShopPanelActive = x;
        ChangeSubList(0);
        switch (shopPanelActive)
        {
            case 0:
                for (int i = 0; i < roomsPanels.Count; i++)
                {
                    if (i == x)
                        continue;
                    roomsPanels[i].SetActive(false);
                    roomsShopButtons[i].transform.SetAsFirstSibling();
                    roomsShopButtons[i].SetBool("Open", false);
                }
                roomsPanels[x].SetActive(true);
                roomsShopButtons[x].transform.SetSiblingIndex(8);
                roomsShopButtons[x].SetBool("Open", true);
                break;
            case 1:
                for (int i = 0; i < decorationsPanels.Count; i++)
                {
                    if (i == x)
                        continue;
                    decorationsPanels[i].SetActive(false);
                    decorationsShopButtons[i].transform.SetAsFirstSibling();
                    decorationsShopButtons[i].SetBool("Open", false);
                }
                decorationsPanels[x].SetActive(true);
                decorationsShopButtons[x].transform.SetSiblingIndex(8);
                decorationsShopButtons[x].SetBool("Open", true);
                break;
            case 2:
                for (int i = 0; i < hotelPanels.Count; i++)
                {
                    if (i == x)
                        continue;
                    hotelPanels[i].SetActive(false);
                    hotelShopButtons[i].transform.SetAsFirstSibling();
                    hotelShopButtons[i].SetBool("Open", false);
                }
                hotelPanels[x].SetActive(true);
                hotelShopButtons[x].transform.SetSiblingIndex(2);
                hotelShopButtons[x].SetBool("Open", true);
                break;
            case 3:
                for (int i = 0; i < staffPanels.Count; i++)
                {
                    if (i == x)
                        continue;
                    staffPanels[i].SetActive(false);
                    staffShopButtons[i].transform.SetAsFirstSibling();
                    staffShopButtons[i].SetBool("Open", false);
                }
                staffPanels[x].SetActive(true);
                staffShopButtons[x].transform.SetSiblingIndex(4);
                staffShopButtons[x].SetBool("Open", true);
                break;
        }
    }
    public void ChangeSubList(int x)
    {
        listNumber += x;
        if (listNumber <= 0)
        {
            prevListButton.SetActive(false);
            listNumber = 0;
        }
        else
        {
            prevListButton.SetActive(true);
        }
        int t = 0;
        switch (shopPanelActive)
        {
            case 0:
                t = roomsShopLists[subShopPanelActive].Count;
                if (t > 1)
                    shopCountText.text = (listNumber + 1) + "/" + t;
                else
                    shopCountText.text = "";
                for (int i = 0; i < roomsShopLists[subShopPanelActive].Count; i++)
                    roomsShopLists[subShopPanelActive][i].SetActive(false);
                roomsShopLists[subShopPanelActive][listNumber].SetActive(true);

                if (listNumber >= roomsShopLists[subShopPanelActive].Count - 1)
                {
                    nextListButton.SetActive(false);
                    listNumber = roomsShopLists[subShopPanelActive].Count - 1;
                }
                else
                {
                    nextListButton.SetActive(true);
                }
                break;
            case 1:
                t = decorationsShopLists[subShopPanelActive].Count;
                if (t > 1)
                    shopCountText.text = (listNumber + 1) + "/" + t;
                else
                    shopCountText.text = "";
                for (int i = 0; i < decorationsShopLists[subShopPanelActive].Count; i++)
                    decorationsShopLists[subShopPanelActive][i].SetActive(false);
                decorationsShopLists[subShopPanelActive][listNumber].SetActive(true);

                if (listNumber >= decorationsShopLists[subShopPanelActive].Count - 1)
                {
                    nextListButton.SetActive(false);
                    listNumber = decorationsShopLists[subShopPanelActive].Count - 1;
                }
                else
                {
                    nextListButton.SetActive(true);
                }
                break;
            case 2:
                t = hotelShopLists[subShopPanelActive].Count;
                if (t > 1)
                    shopCountText.text = (listNumber + 1) + "/" + t;
                else
                    shopCountText.text = "";
                for (int i = 0; i < hotelShopLists[subShopPanelActive].Count; i++)
                    hotelShopLists[subShopPanelActive][i].SetActive(false);
                hotelShopLists[subShopPanelActive][listNumber].SetActive(true);

                if (listNumber >= hotelShopLists[subShopPanelActive].Count - 1)
                {
                    nextListButton.SetActive(false);
                    listNumber = hotelShopLists[subShopPanelActive].Count - 1;
                }
                else
                {
                    nextListButton.SetActive(true);
                }
                break;
            case 3:
                t = staffShopLists[subShopPanelActive].Count;
                if (t > 1)
                    shopCountText.text = (listNumber + 1) + "/" + t;
                else
                    shopCountText.text = "";
                for (int i = 0; i < staffShopLists[subShopPanelActive].Count; i++)
                    staffShopLists[subShopPanelActive][i].SetActive(false);
                staffShopLists[subShopPanelActive][listNumber].SetActive(true);

                if (listNumber >= staffShopLists[subShopPanelActive].Count - 1)
                {
                    nextListButton.SetActive(false);
                    listNumber = staffShopLists[subShopPanelActive].Count - 1;
                }
                else
                {
                    nextListButton.SetActive(true);
                }
                break;

        }
    }

    public void LockShop()
    {
        lockShop = !lockShop;
        lockedShop.SetActive(lockShop);
        unlockedShop.SetActive(!lockShop);
    }
    public void CloseShop()
    {
        lockShop = false;
        lockedShop.SetActive(lockShop);
        unlockedShop.SetActive(!lockShop);
        shopPanel.SetActive(lockShop);
        SC.INS.PlaySound(0, 13, 0);
    }
    public void SetHotel(Dictionary<string, object> data)
    {

        if (data != null && data.ContainsKey("title"))
        {
            starsCount = Convert.ToInt32(data["stars"]);
            SetStars(0);
            hotelTitle.text = data["title"].ToString();
            usernameText.text = data["name"].ToString();
            TransformListToRoom(data["rooms"] as List<object>);
            backId = Convert.ToInt32(data["backId"]);
            GC.INS.backController.ChangeB(backId);
            /* logTime = Fire.INS.ParseTime(data["time"]);
             shiftStart = Fire.INS.ParseTime(data["shiftStart"]);
             Fire.INS.SetShiftStart(data["shiftStart"]);
             shiftTime = Convert.ToInt32(data["shift"]);
             shiftType = Convert.ToInt32(data["shiftType"]);
             SetShift();*/
            if (data.ContainsKey("iap"))
            {
                List<object> temp = data["iap"] as List<object>;
                for (int i = 0; i < temp.Count; i++)
                    iap[i] = (bool)temp[i];
            }
            SetCostumersStart();
            blocksId = Convert.ToInt32(data["blocksId"]);
            blocksPermited = GC.blocksPer[blocksId];
            SetBlocks();
            if (data.ContainsKey("outsides"))
            {
                TransformListToOutside(data["outsides"] as List<object>);
            }
            if (data.ContainsKey("prestige"))
            {
                Debug.Log("Contains prestige");
                prestige = Convert.ToInt32(data["prestige"]);
                if (prestige > 0)
                {
                    medalI.gameObject.SetActive(true);
                    starsI.color = GC.INS.p.starColors[prestige - 1];
                    medalI.sprite = GC.INS.p.medals[prestige - 1];
                    if (data.ContainsKey("parkinglevels"))
                    {
                        SetParking(data["parkinglevels"] as List<object>);
                    }
                }
            }
        }
        else
        {
            Bye();
        }
        if (!GC.INS.tutoOn)
        {
            if ((DateTime.UtcNow - GC.INS.lastFriendVisit[FRC.INS.visitNumber]).Days >= 1)
            {
                coinBag.SetActive(true);
            }
            Fire.INS.GetForeignDataVisit(FRC.INS.visitId);
        }
        else
        {
            tutoPanel.SetActive(true);
            coinBag.SetActive(true);
            SetAnim();
            tutoText.text = GC.INS.t.GetText(107);
            Character player = new Character(18, 28, 4, "Friend", 4, 3, 1, 0, 12, 2, 0, 0, 0, 0, false);
            playerCharacter.SetCharacter(player);
        }

        Invoke("LoadingOff", 1);
        loadingPanel.GetComponent<Animator>().SetTrigger("Finish");
    }
    public void SetBlocks()
    {
        GC.INS.limitL = -3 - blocksId * 0.35f + ((blocksId > 7) ? (blocksId - 7) * 0.2f : 0);
        GC.INS.limitR = 7f + blocksId * 0.5f + (blocksId * blocksId * 0.03f) - ((blocksId > 7) ? (blocksId - 7) * 0.5f : 0);
        GC.INS.limitY = 20 + blocksId * 1.5f + (blocksId * blocksId * 0.03f);
        //Debug.Log(limitL + "   " + limitR + "   " + limitY);
    }
    void SetParking(List<object> levels)
    {
        parking = Instantiate(parkingObj, new Vector3(-3.5f, 0.8f, 0), Quaternion.identity, roomsArrange.transform).GetComponent<ParkingO>();
        if (levels != null)
        {
            List<int[]> tempList = new List<int[]>();
            int floor = 0;
            for (int i = 0; i < levels.Count; i++)
            {
                Dictionary<string, object> tempDic = levels[i] as Dictionary<string, object>;
                int[] tempInt = new int[3];
                tempInt[0] = Convert.ToInt32(tempDic["up"]);
                tempInt[1] = Convert.ToInt32(tempDic["space"]);
                tempInt[2] = Convert.ToInt32(tempDic["valet"]);
                if (tempInt[0] > 0)
                    floor = i + 1;
                tempList.Add(tempInt);
            }
            if (floor > 0)
                parking.SetVisitUp(floor, tempList[floor - 1]);
        }
    }
    void TransformListToOutside(List<object> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            if (list[i] != null)
            {
                Outside tempOut;
                Dictionary<string, object> tempDic = list[i] as Dictionary<string, object>;

                tempOut = new Outside(
                  Convert.ToInt32(tempDic["id"]),
                  Convert.ToSingle(tempDic["posX"])
                );

                GameObject temp = Instantiate(outsideO, Vector3.zero, transform.rotation);
                temp.GetComponentInChildren<OutsideO>().SetObject(tempOut);
            }
        }
    }
    public void TutoNext()
    {
        tutoText.text = GC.INS.t.GetText(108);
        SC.INS.PlaySound(1, 1, 1);
        homeBtn.SetActive(true);
        pointer.SetActive(true);
    }
    void SetAnim()
    {
        tutoAnim.SetTrigger("3");
        SC.INS.PlaySound(1, 1, 1);
    }
    public void SetUser(Dictionary<string, object> friendData)
    {
        if (friendData != null)
        {
            FRC.INS.friendList[FRC.INS.visitNumber] = new Dictionary<string, object>
                    {
                        {"id" , friendData["id"].ToString() },
                        {"title", friendData["title"].ToString()},
                        {"name" , friendData["name"].ToString() },
                        {"stars" , friendData["stars"].ToString()},
                        {"character" , friendData["character"]}
                    };

            if (friendData.ContainsKey("gifts"))
            {
                friendGiftList = friendData["gifts"] as List<object>;
            }
            if (friendData.ContainsKey("noti"))
                noti = (bool)friendData["noti"];
            username = friendData["name"].ToString();
            if (friendData.ContainsKey("character"))
            {
                SetPlayer(friendData["character"] as Dictionary<string, object>);
            }
            if (friendData.ContainsKey("jobApp"))
            {
                if (requestJobBtn.activeInHierarchy && friendData["jobApp"].ToString() == Fire.INS.GetCurrentUser().UserId)
                {
                    requestJobBtn.SetActive(false);
                }
            }
        }
    }
    void SetPlayer(Dictionary<string, object> tempDic)
    {
        Character player = new Character
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
        playerCharacter.SetCharacter(player);
    }
    void TransformListToRoom(List<object> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            if (list[i] != null)
            {
                RoomC tempRoom;
                Dictionary<string, object> tempDic = list[i] as Dictionary<string, object>;
                if (Convert.ToInt32(tempDic["id"]) == 23)
                {
                    tempRoom = new RoomC(
                       Convert.ToInt32(tempDic["id"]),
                       Convert.ToInt32(tempDic["posX"]),
                       Convert.ToInt32(tempDic["posY"]),
                       Convert.ToInt32(tempDic["wallT"]),
                       Convert.ToInt32(tempDic["bedT"]),
                       Convert.ToInt32(tempDic["floorT"]),
                       RoomC.TransformDecorationToList(tempDic["decorations"] as List<object>),
                       RoomC.TransformToCharacterList(tempDic["staff"] as List<object>)
                   );
                }
                else if (tempDic.ContainsKey("decorations"))
                {
                    tempRoom = new RoomC(
                        Convert.ToInt32(tempDic["id"]),
                        Convert.ToInt32(tempDic["posX"]),
                        Convert.ToInt32(tempDic["posY"]),
                        Convert.ToInt32(tempDic["wallT"]),
                        Convert.ToInt32(tempDic["bedT"]),
                        Convert.ToInt32(tempDic["floorT"]),
                        RoomC.TransformDecorationToList(tempDic["decorations"] as List<object>)
                    );
                }
                else if (tempDic.ContainsKey("staff"))
                {
                    tempRoom = new RoomC(
                        Convert.ToInt32(tempDic["id"]),
                        Convert.ToInt32(tempDic["posX"]),
                        Convert.ToInt32(tempDic["posY"]),
                        RoomC.TransformToCharacterList(tempDic["staff"] as List<object>)
                   );
                }
                else
                {
                    tempRoom = new RoomC(
                        Convert.ToInt32(tempDic["id"]),
                        Convert.ToInt32(tempDic["posX"]),
                        Convert.ToInt32(tempDic["posY"])
                   );
                }
                GameObject temp = Instantiate(GC.INS.roomsObj[tempRoom.id], Vector3.zero, transform.rotation);
                temp.GetComponentInChildren<Room>().CreateVisit(tempRoom);
            }
        }
    }

    public void SetStars(int x)
    {
        starsCount += x;
        int tempCount = starsCount;
        if (tempCount <= GC.starsXp[0])
        {
            stars = (tempCount * 1f) / (GC.starsXp[0] * 1f);
        }
        if (tempCount > GC.starsXp[0] && tempCount <= GC.starsXp[1])
        {
            stars = 1 + (tempCount * 1f - GC.starsXp[0]) / (GC.starsXp[1] * 1f - GC.starsXp[0]);
        }
        if (tempCount > GC.starsXp[1] && tempCount <= GC.starsXp[2])
        {
            stars = 2 + (tempCount * 1f - GC.starsXp[1]) / (GC.starsXp[2] * 1f - GC.starsXp[1]);
        }
        if (tempCount > GC.starsXp[2] && tempCount <= GC.starsXp[3])
        {
            stars = 3 + (tempCount * 1f - GC.starsXp[2]) / (GC.starsXp[3] * 1f - GC.starsXp[2]);
        }
        if (tempCount > GC.starsXp[3] && tempCount <= GC.starsXp[4])
        {
            stars = 4 + (tempCount * 1f - GC.starsXp[3]) / (GC.starsXp[4] * 1f - GC.starsXp[3]);
        }
        if (tempCount > GC.starsXp[4])
            stars = 5;

        starsBar.fillAmount = stars / 5;

    }

    public int AddRoom(RoomC room, Room roomController)
    {
        roomControllers.Add(roomController);
        rooms.Add(room);
        int roomTime = roomController.roomTime;
        int roomCoins = roomController.roomCoins;
        float ave = (roomCoins * 1f) / (roomTime * 1f);
        return rooms.Count - 1;
    }
    private void FixedUpdate()
    {
        if (timeToSpawn <= 0)
        {
            InatantiateCostumer();
            timeToSpawn = spawnStartTime / (slotsID.Count + 3) - (stars * 5) / (slotsID.Count + 3);
        }
        else
        {
            timeToSpawn -= Time.fixedDeltaTime;
        }
    }

    public void InatantiateCostumer()
    {
        bool going = false;
        float r = UnityEngine.Random.Range(0f, 1f);
        float calc = 0.7f - 1f / (slotsID.Count / 3f + 4f) + stars * .06f;
        //Debug.Log(r + "   " + calc);
        if (r < calc)
            going = true;
        if (!work && UnityEngine.Random.Range(0, 3) == 0)
            going = false;

        Costumer temp = Instantiate(GC.INS.costumer, costumersArrange.transform).GetComponentInChildren<Costumer>();
        temp.Create(going, GC.INS.limitL, GC.INS.limitR);
    }
    public Slot SetCostumer()
    {
        Slot temp = null;
        if (slotsID.Count > 0)
        {
            int index = UnityEngine.Random.Range(0, slotsID.Count);
            temp = slots[slotsID[index]];
            slotsID.RemoveAt(index);
            roomControllers[temp.roomId].SetCostumer();
        }

        return temp;
    }
    void SetCostumersStart()
    {
        if (work)
        {
            float timeCheck = 1;

            float starsCheck = stars / 5f;
            float percent = timeCheck * .4f + starsCheck * .2f + UnityEngine.Random.Range(0.1f, 0.4f);
            if (percent > 1)
                percent = 1;
            int quant = (int)(slotsID.Count * percent);
            //Debug.Log("porciento: " + percent + "  Cantidad:  " + quant);
            for (int i = 0; i < quant; i++)
            {
                Costumer temp = Instantiate(GC.INS.costumer, costumersArrange.transform).GetComponentInChildren<Costumer>();
                temp.SetFromStart(50, -5, 14);
            }
            quant = (int)(slotsID.Count * percent * UnityEngine.Random.Range(0.1f, 0.4f));
            //Debug.Log(quant);
            for (int i = 0; i < quant; i++)
            {
                Costumer temp = Instantiate(GC.INS.costumer, costumersArrange.transform).GetComponentInChildren<Costumer>();
                temp.SetMaintenance();
            }
        }

        for (int i = 0; i < slotsID.Count / 3 + 1; i++)
        {
            Costumer temp = Instantiate(GC.INS.costumer, costumersArrange.transform).GetComponentInChildren<Costumer>();
            temp.CreateRandom(GC.INS.limitL, GC.INS.limitR);
        }
    }
    public void AddCoins(int x, Vector2 pos)
    {
        SC.INS.PlaySound(1, 5, 0);
        if (x < 1)
            return;
        TextMeshPro tempText = Instantiate(minusText, transform.parent).GetComponentInChildren<TextMeshPro>();
        tempText.transform.parent.position = pos;
        tempText.text = "+" + x;
        GC.INS.coins += x;
        SetCreditCard();
    }

    public void AddDust(int id)
    {
        dust.Add(id);
        for (int i = 0; i < janitors.Count; i++)
            janitors[i].GetToWork(slots[id]);
    }

    public void RemoveDust(Slot slot)
    {
        dust.Remove(slot.id);
        ResetSlot(slot);
    }
    public void AddJanitor(Staff s)
    {
        janitors.Add(s);
        if (dust.Count > 0)
        {
            for (int i = 0; i < janitors.Count; i++)
                for (int j = 0; j < dust.Count; j++)
                    janitors[i].GetToWork(slots[dust[j]]);
        }
    }
    public void AddPipe(int id)
    {
        pipe.Add(id);
        for (int i = 0; i < plumbers.Count; i++)
            plumbers[i].GetToWork(slots[id]);
    }
    public void RemovePipe(Slot slot)
    {
        pipe.Remove(slot.id);
        ResetSlot(slot);
    }
    public void AddPlumber(Staff s)
    {
        plumbers.Add(s);
        if (pipe.Count > 0)
        {
            for (int i = 0; i < plumbers.Count; i++)
                for (int j = 0; j < pipe.Count; j++)
                    plumbers[i].GetToWork(slots[pipe[j]]);
        }
    }
    public void AddElect(int id)
    {
        electricity.Add(id);
        for (int i = 0; i < electicists.Count; i++)
            electicists[i].GetToWork(slots[id]);
    }
    public void RemoveElect(Slot slot)
    {
        electricity.Remove(slot.id);
        ResetSlot(slot);
    }
    public void AddElectric(Staff s)
    {
        electicists.Add(s);
        if (electricity.Count > 0)
        {
            for (int i = 0; i < electicists.Count; i++)
                for (int j = 0; j < electricity.Count; j++)
                    electicists[i].GetToWork(slots[electricity[j]]);
        }
    }
    public void AddComplaint(int id)
    {
        complaint.Add(id);
        for (int i = 0; i < officinist.Count; i++)
            officinist[i].GetToWork(slots[id]);
    }
    public void RemoveComplaint(Slot slot)
    {
        complaint.Remove(slot.id);
        ResetSlot(slot);
    }
    public void AddOfficier(Staff s)
    {
        officinist.Add(s);
        if (complaint.Count > 0)
        {
            for (int i = 0; i < officinist.Count; i++)
                for (int j = 0; j < complaint.Count; j++)
                    officinist[i].GetToWork(slots[complaint[j]]);
        }
    }
    public void AddKeyLoss(int id)
    {
        key.Add(id);
        for (int i = 0; i < keyBuilder.Count; i++)
            keyBuilder[i].GetToWork(slots[id]);
    }
    public void RemoveKey(Slot slot)
    {
        key.Remove(slot.id);
        ResetSlot(slot);
    }
    public void AddKeyBuilder(Staff s)
    {
        keyBuilder.Add(s);
        if (key.Count > 0)
        {
            for (int i = 0; i < keyBuilder.Count; i++)
                for (int j = 0; j < key.Count; j++)
                    keyBuilder[i].GetToWork(slots[key[j]]);
        }
    }
    public int AddSlot(Slot slot)
    {
        slot.id = slots.Count;
        slotsID.Add(slot.id);
        slots.Add(slot);
        return slot.id;
    }
    public void RemoveSlot(int id)
    {
        slotsID.Remove(id);
    }
    public void ResetSlot(Slot slot)
    {
        slotsID.Add(slot.id);
        roomControllers[slot.roomId].ByeCostumer();
    }
    void LoadingOff()
    {
        loadingPanel.SetActive(false);
    }
    public void SetGift()
    {
        gifting = true;
        giftList = new List<Gift>();
        gifting = false;
        shopButtons.SetActive(true);
        giftButton.SetActive(false);
        gift.SetActive(true);
        giftText.text = giftList.Count.ToString();
        giftGemsText.text = tempGiftGems.ToString();
        giftCoinsText.text = tempGiftCoins.ToString();
        giftI.transform.localScale = new Vector2(1f, 1f);
        giftI.sprite = giftFill;
        if (firstGift)
            firstGiftAnim.SetActive(true);
    }
    #region BuyGift
    Room currentRoom;
    Decoration currentDecoration;
    StaffHire currentStaffHire;
    StaffOutfit currentStaffOutfit;
    OutsideO currentOutside;
    ItemGift currentItemGift;

    public void BuyRoom(int id)
    {
        if (GC.INS.coins >= Room.costs[id] && Room.costs[id] > 0 || GC.INS.gems >= (Room.costs[id] * -1) && Room.costs[id] < 0)
        {
            BeginDrag();
            GameObject temp = Instantiate(GC.INS.roomsObj[id], roomsArrange.transform);
            currentRoom = temp.GetComponentInChildren<Room>();
            currentRoom.SetGift(null);
        }
        else
        {
            if (Room.costs[id] > 0)
                errorM.Error(0);
            else
                errorM.Error(1);
        }

    }
    public void GiftRoom(RoomButton rb)
    {
        BeginDrag();
        GameObject temp = Instantiate(GC.INS.roomsObj[rb.id], roomsArrange.transform);
        currentRoom = temp.GetComponentInChildren<Room>();
        currentRoom.SetGift(rb);
    }
    public void BuyDecoration(int id, int type)
    {
        if (GC.INS.coins >= Decoration.costs[type, id] && Decoration.costs[type, id] >= 0 || GC.INS.gems >= (Decoration.costs[type, id] * -1) && Decoration.costs[type, id] < 0)
        {
            BeginDrag();
            GameObject temp = Instantiate(GC.INS.decorationsObj[type], roomsArrange.transform);
            currentDecoration = temp.GetComponentInChildren<Decoration>();
            currentDecoration.SetGift(null, id);
        }
        else
        {
            if (Decoration.costs[type, id] >= 0)
                errorM.Error(0);
            else
                errorM.Error(1);
        }

    }
    public void GiftDecoration(DecorationButton db)
    {
        BeginDrag();
        GameObject temp = Instantiate(GC.INS.decorationsObj[db.type], roomsArrange.transform);
        currentDecoration = temp.GetComponentInChildren<Decoration>();
        currentDecoration.SetGift(db, db.id);
    }
    public void BuyOufit(int id)
    {
        if (GC.INS.coins >= StaffOutfit.costs[id] && StaffOutfit.costs[id] >= 0 || GC.INS.gems >= (StaffOutfit.costs[id] * -1) && StaffOutfit.costs[id] < 0)
        {
            BeginDrag();
            GameObject temp = Instantiate(GC.INS.staffOufit, roomsArrange.transform);
            currentStaffOutfit = temp.GetComponentInChildren<StaffOutfit>();
            currentStaffOutfit.SetGift(id, null);

        }
        else
        {
            if (StaffOutfit.costs[id] >= 0)
                errorM.Error(0);
            else
                errorM.Error(1);
        }

    }
    public void GiftOutfit(OutfitButton ob)
    {
        BeginDrag();
        GameObject temp = Instantiate(GC.INS.staffOufit, roomsArrange.transform);
        currentStaffOutfit = temp.GetComponentInChildren<StaffOutfit>();
        currentStaffOutfit.SetGift(ob.id, ob);
    }

    public void BuyCharacter(Character character)
    {
        if (character.id < 20)
        {
            if (GC.INS.coins >= Staff.costs[character.id] && Staff.costs[character.id] >= 0 || GC.INS.gems >= (Staff.costs[character.id] * -1) && Staff.costs[character.id] < 0)
            {
                BeginDrag();
                GameObject temp = Instantiate(GC.INS.staffHire, transform.position, transform.rotation);
                currentStaffHire = temp.GetComponentInChildren<StaffHire>();
                currentStaffHire.SetGift(character, null);
            }
            else
            {
                if (Staff.costs[character.id] >= 0)
                    errorM.Error(0);
                else
                    errorM.Error(1);
            }
        }

    }
    public void StaffGift(StaffBtn sb, Character character)
    {
        BeginDrag();
        GameObject temp = Instantiate(GC.INS.staffHire, transform.position, transform.rotation);
        currentStaffHire = temp.GetComponentInChildren<StaffHire>();
        currentStaffHire.SetGift(character, sb);
    }
    public void BuyOutside(int id)
    {
        if (GC.INS.coins >= OutsideO.costs[id] && OutsideO.costs[id] > 0 || GC.INS.gems >= (OutsideO.costs[id] * -1) && OutsideO.costs[id] < 0)
        {
            BeginDrag();
            GameObject temp = Instantiate(GC.INS.outsideO, roomsArrange.transform);
            currentOutside = temp.GetComponentInChildren<OutsideO>();
            currentOutside.SetGift(id, null);
        }
        else
        {
            if (OutsideO.costs[id] > 0)
                errorM.Error(0);
            else
                errorM.Error(1);
        }

    }
    public void OutsideGift(OutsideButton ob)
    {
        BeginDrag();
        GameObject temp = Instantiate(GC.INS.outsideO, roomsArrange.transform);
        currentOutside = temp.GetComponentInChildren<OutsideO>();
        currentOutside.SetGift(ob.id, ob);
    }

    public void BuyItem(int id, int cost, ItemButton ib)
    {
        BeginDrag();
        GameObject temp = Instantiate(itemG, roomsArrange.transform);
        currentItemGift = temp.GetComponentInChildren<ItemGift>();
        currentItemGift.SetGift(id, cost, ib);
    }
    #endregion
    public void SetCreditCard()
    {
        coinText.text = GC.INS.coins.ToString("n0");
        gemText.text = GC.INS.gems.ToString("n0");
    }
    public void Gifting()
    {
        if (!GC.INS.isDragging)
            return;
        giftI.sprite = giftEmpty;
        giftI.transform.localScale = new Vector2(1.2f, 1.2f);
        giftText.text = (giftList.Count + 1).ToString();
        if (currentRoom)
        {
            currentRoom.Gifting();
        }
        else if (currentDecoration)
        {
            currentDecoration.Gifting();
        }
        else if (currentStaffHire != null)
        {
            currentStaffHire.Gifting();
        }
        else if (currentStaffOutfit)
        {
            currentStaffOutfit.Gifting();
        }
        else if (currentOutside)
        {
            currentOutside.Gifting();
        }
        else if (currentItemGift)
        {
            currentItemGift.Gifting();
        }
    }
    public void GifttingExit()
    {
        if (!GC.INS.isDragging)
            return;
        if (!IsInvoking("GiftingEx"))
        {
            Invoke("GiftingEx", .05f);
        }

    }
    void GiftingEx()
    {
        if (addedGift)
            return;
        giftI.transform.localScale = new Vector2(1f, 1f);
        giftText.text = giftList.Count.ToString();
        if (currentRoom)
        {
            currentRoom.GiftingExit();
        }
        else if (currentDecoration)
        {
            currentDecoration.GiftingExit();
        }
        else if (currentStaffHire != null)
        {
            currentStaffHire.GiftingExit();
        }
        else if (currentStaffOutfit)
        {
            currentStaffOutfit.GiftingExit();
        }
        else if (currentOutside)
        {
            currentOutside.GiftingExit();
        }
        else if (currentItemGift)
        {
            currentItemGift.GiftingExit();
        }
        giftI.sprite = giftFill;
    }
    bool addedGift;
    public void AddGift(Gift gift, int cost)
    {
        addedGift = true;
        Invoke("AddedFalse", .1f);
        giftI.sprite = giftEmpty;
        giftI.transform.localScale = new Vector2(1f, 1f);
        giftList.Add(gift);
        if (cost > 0)
        {
            GC.INS.coins -= cost;
            tempGiftCoins += cost;
            giftCoinsText.text = tempGiftCoins.ToString();
        }
        if (cost < 0)
        {
            GC.INS.gems += cost;
            tempGiftGems -= cost;
            giftGemsText.text = tempGiftGems.ToString();
        }
        if (cost == 0)
        {
            tempGiftList.Add(gift);
        }
        SetCreditCard();
        sendGiftBtn.SetActive(true);
    }
    void AddedFalse()
    {
        addedGift = false;
    }
    public void SendGifts()
    {
        shopButtons.SetActive(false);
        gift.GetComponent<Animator>().SetTrigger("Send");
        SC.INS.PlaySound(0, 13, 0);
        Invoke("GiftActive", .5f);
        foreach (Gift gift in giftList)
        {
            friendGiftList.Add(new Dictionary<string, object>
            {
                { "id", gift.id },
                { "type", gift.type },
                { "subtype", gift.subtype },
                { "seen", false},
                {"name", GC.INS.username},
                {"added", false },
                {"character", GC.INS.characterAsMap}
            });

        }
        Dictionary<string, object> newDataToSend = new Dictionary<string, object>
        {
             { "gifts", friendGiftList },
        };
        if (noti)
            GC.INS.rdb.SendGift(FRC.INS.visitId);
        Fire.INS.MergeDataFirestore(newDataToSend, FRC.INS.visitId);
        GC.INS.AddXp(tempGiftCoins / 1500 - tempGiftGems * 2);
        tempGiftCoins = 0;
        tempGiftGems = 0;
        GC.INS.dm.AddTask(28, giftList.Count);
        tempGiftList = new List<Gift>();
        giftList = new List<Gift>();
        gifting = false;
        friendGiftList = new List<object>();
        GC.INS.coinsText.text = GC.INS.coins.ToString("n0");
        GC.INS.gemsText.text = GC.INS.gems.ToString("n0");
        GC.INS.pg.Achivements(5, 0);
        if (firstGift)
            PlayerPrefs.SetInt("firstGift", 1);
    }
    void GiftActive()
    {
        SC.INS.PlaySound(0, 12, 0);
        gift.SetActive(false);
    }
    public void CancelGifts()
    {
        shopButtons.SetActive(false);
        giftButton.SetActive(true);
        gift.SetActive(false);
        GC.INS.coins += tempGiftCoins;
        GC.INS.gems += tempGiftGems;
        tempGiftCoins = 0;
        tempGiftGems = 0;
        for (int i = 0; i < tempGiftList.Count; i++)
        {
            GC.INS.gift.ReturnGift(tempGiftList[i]);
        }
        tempGiftList = new List<Gift>();
        giftList = new List<Gift>();
        gifting = false;
        SetCreditCard();
        sendGiftBtn.SetActive(false);
    }

    private void OnApplicationPause(bool pause)
    {
        if (GC.INS.tutoOn)
            return;
        if (pause)
        {
            GC.INS.SaveFromBtn();
        }
    }
    private void OnApplicationQuit()
    {
        if (GC.INS.tutoOn)
            return;
        CancelGifts();
        GC.INS.SaveFromBtn();
    }

    public void CheckStaffName(string uname)
    {
        if (uname == GC.INS.username)
        {
            requestJobBtn.SetActive(false);
            if (coinBag.activeInHierarchy)
            {
                tipJar.SetActive(true);
            }
        }
    }
    public void SetRequest()
    {
        jobr.JobAppFriend();
    }
    public void RequestJob()
    {
        Dictionary<string, object> newDataToSend = new Dictionary<string, object>
        {
             { "jobApp", Fire.INS.GetCurrentUser().UserId},
        };
        Fire.INS.MergeDataFirestore(newDataToSend, FRC.INS.visitId);
        requestJobBtn.SetActive(false);
    }
}