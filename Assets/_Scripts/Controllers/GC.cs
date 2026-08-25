using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GC : MonoBehaviour
{
    public static GC INS { get; private set; }
    List<RoomC> rooms = new List<RoomC>();
    List<RoomC> deletedRooms = new List<RoomC>();
    List<Room> roomControllers = new List<Room>();
    List<Outside> outsideObjects = new List<Outside>();
    List<Outside> deletedOutside = new List<Outside>();
    List<Slot> slots = new List<Slot>();
    [HideInInspector]
    public List<int> slotsID = new List<int>();

    public Character player;

    [HideInInspector]
    public bool isDragging;

    public GameObject[] roomsObj, decorationsObj;
    [Space(20)]
    public GameObject roomsArrange;
    public GameObject buyingCanvas, editingCanvas, shopPanel;

    #region Shop
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

    [Space(20)]
    public GameObject costumer, cosumterPlusM;
    public GameObject costumersArrange, staffHire;
    public GameObject staffOufit;

    #region Maintanence
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
    [HideInInspector]
    //public List<Slot> slotsOnMaintanance = new List<Slot>();
    #endregion

    public float stars = 0f;
    int starsCount;
    public Image starsBar;
    float timeToSpawn, spawnStartTime = 40, timeToSpawn2;

    public Text coinsText, addedCoinsText, gemsText, xpLeftText;
    int currentType, xp;
    [HideInInspector]
    public int level, coins, gems;
    Animator coinsAnim;

    [HideInInspector]
    public bool decorating, shopping;
    [Header("Level")]
    public TextMeshProUGUI levelText;
    public Image levelBar, trashIm;
    public Sprite trashsp1, trashsp2;
    Animator levelAnim;

    public Text shiftTimer;
    public Animator shiftAnim;
    public GameObject shiftPanel;
    public GameObject[] shiftCheck, shiftLock, canvas;
    public Text[] shiftCostText;
    public Text wagesText;
    int wages;
    public GameObject phone, phoneNot;
    public GameObject[] phonePanels;

    [HideInInspector]
    public int blocksPermited = 12, blocks, blocksId;
    public Text blocksUsed;
    //Recomendations
    [HideInInspector]
    public bool haveGym, haveRestaurant, haveCinema;

    public InputField hotelTitleInput, usernameInput;
    public Text warningTH, warningTU;
    public GameObject changeHotelPanel, loadingPanel;
    [HideInInspector]
    public string username;
    string hotelTitle;
    public Text title, levelTCus, friendsTCus;
    int secondsDif, coinsSave;
    int shiftType, shiftTime, shiftTimeLog;
    DateTime shiftStart, logTime, lastSave;
    float shift, coinsAverage, parkingCoinsAverage;
    [HideInInspector]
    public float totalAverage;
    [HideInInspector]
    public bool work;
    public Text coinOut;

    #region customize
    [HideInInspector]
    public List<bool[]> customPurchased;
    [HideInInspector]
    public bool customized;
    public Color[] hairC, eyesC, glassColor, armazonColor;
    public GameObject customizePanel;
    public GameObject[] custCont;
    public Image[] custBtn;
    public ScrollRect customPanel;
    public Image[] currentBtnCus = new Image[12];
    public InputField customTitle, customUser;
    public GameObject editTBtn, okTBtn, editUBtn, okUBtn;
    public Text editTcost, editUcost, starsText, rateText;
    #endregion
    public CloudsC backController;
    [HideInInspector]
    public int backId;
    [HideInInspector]
    public float limitL = -4, limitR = 12, limitY = 40;
    [HideInInspector]
    public bool visit;

    public ErrorM errorM;
    public GameObject outsideO;
    public GFC gift;
    public DailyMisions dm;
    DateTime pauseTime, lastAd;
    public GameObject levelPanel, levelBlocksOb;
    public Text[] levelTexts;
    public TextMeshProUGUI levelUpText;

    [HideInInspector]
    public List<DateTime> lastFriendVisit = new List<DateTime>();
    public Tutorial tuto;
    public bool tutoOn;
    public GiftCard gc;
    public GameObject whileYouAway;
    [HideInInspector]
    public List<int> staffFriendIDList = new List<int>();
    public JobReq jobReq;
    public GameObject expandUI, phoneConnectionCheck, phoneNoInternet, doubleShiftP;

    public AdController ad;
    public GameObject zeppeling;
    [HideInInspector]
    public Costumer costumerReward;
    [HideInInspector]
    public Zeppeling currentZep;
    public GameObject x2boostP, x2Image;
    bool coinBoost;
    float boostTime;
    public Image boostFill;
    public Text boostTimeText;
    bool doubleShift;
    [HideInInspector]
    public bool[] iap = new bool[8];
    public bool[] plusVisit = new bool[3];
    public bool[] codes = new bool[50];
    public GameObject shiftX2I;
    [HideInInspector]
    public int roomFix, giftCount, dailyMCount, prestige;
    public PG pg;
    public Prestige p;
    public int idiom;
    public Translate t;
    [HideInInspector]
    public DateTime lastFShare;
    public GameObject invitesBtn, shareBtn;
    public FaceB f;
    public AppReview appReview;
    [HideInInspector]
    public bool noti;
    public FCM notifications;
    public RDB rdb;
    [HideInInspector]
    public int replacedRoomsCount;
    //public GameObject coppa;
    public CameraController camC;
    public CharacterSet playerCharacter;

    private void Awake()
    {
        if (INS == null)
            INS = this;
        else
        {
            Debug.Log("Duplicated GC");
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        iap = new bool[8];
        lastAd = DateTime.UtcNow.AddSeconds(-15);
        coinsAnim = coinsText.transform.parent.GetComponent<Animator>();
        levelAnim = levelBar.transform.parent.GetComponent<Animator>();
        lastSave = DateTime.UtcNow;
        limitL = -4;
        limitR = 12;
        roomFix = PlayerPrefs.GetInt("roomFix");
        giftCount = PlayerPrefs.GetInt("giftCount");
        dailyMCount = PlayerPrefs.GetInt("dailyMCount");

        plusVisit = new bool[3];
        codes = new bool[50];

        GetShopLists();
        loadingPanel.SetActive(true);
        Fire.INS.GetData();

        timeToSpawn2 = 4;
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
    #region shift
    public void StartShift(int id)
    {
        if (shift > 0)
            return;
        if (level > 1 && (id * id) > level || tutoOn && id != 1)
        {
            errorM.Error(3);
            return;
        }
        if (tutoOn && tuto.current == 7 && id == 1)
        {
            tuto.Next();
        }
        doubleShift = false;
        shift = SwitchShift(id);
        if (shift == 0)
        {
            return;
        }
        shiftType = id;
        shiftTime = (int)shift;
        shiftStart = Fire.INS.GetTimeInFormat();
        work = true;
        roomControllers[0].SetWork(work, true);
        if (wages > 0)
            Purchase((int)(shiftCosts[id] + shiftCosts[id] * ((wages * 1f) / 100f)));
        else
            Purchase((int)(shiftCosts[id]));
        AddXp(id * id);
        shiftAnim.SetTrigger("Start");

        if ((DateTime.UtcNow - lastAd).TotalSeconds > 10 && !iap[2] && !tutoOn)
        {
            doubleShiftP.SetActive(true);
            Invoke("SetDoubleOff", 10f);
        }
        else
        {
            ShiftPanel(false);
        }
        if (noti)
        {
            rdb.SetShiftNotification(GetShift(id));
        }
    }
    void SetDoubleOff()
    {
        doubleShiftP.SetActive(false);
    }
    public void SetWages(int x)
    {
        wages += x;
    }
    int GetShift(int id)
    {
        int temp = 0;

        switch (id)
        {
            case 1:
                temp = 3600;
                break;
            case 2:
                temp = 10800;
                break;
            case 3:
                temp = 21600;
                break;
            case 4:
                temp = 43200;
                break;
            case 5:
                temp = 86400;
                break;
            case 6:
                temp = 86400 * 2;
                break;
            case 7:
                temp = 86400 * 4;
                break;
        }
        if (doubleShift || iap[2])
            temp *= 2;

        return temp;
    }
    int SwitchShift(int id)
    {
        if (id < 1)
        {
            return 0;
        }

        if ((int)(shiftCosts[id] + shiftCosts[id] * ((wages * 1f) / 100f)) > coins)
        {
            errorM.Error(0);
            return 0;
        }

        shiftCheck[id - 1].SetActive(true);
        return GetShift(id);
    }
    int[] shiftCosts = { 50000, 12, 50, 120, 250, 800, 4000, 18000 };
    private void Update()
    {
        if (shiftTime > 0 && work)
        {
            shiftTime = (int)(shift - ((DateTime.UtcNow - shiftStart).TotalSeconds));
            int time = shiftTime;
            string timer = ((int)time / 3600).ToString("00") + ":";
            time -= ((int)time / 3600) * 3600;
            timer += "" + ((int)time / 60).ToString("00") + ":";
            timer += "" + ((int)time % 60).ToString("00");
            shiftTimer.text = timer;
        }
        else if (work)
        {
            shiftCheck[shiftType - 1].SetActive(false);
            work = false;
            shiftType = 0;
            roomControllers[0].SetWork(work, true);
            shift = 0;
            Debug.Log("Finish Shift");
            shiftAnim.SetTrigger("End");
            shiftTimer.text = "00:00:00";
        }
        if (boostTime > 0 && !iap[0])
        {
            if (!coinBoost)
            {
                x2Image.SetActive(true);
            }
            coinBoost = true;
            boostTime -= Time.deltaTime;
            int time = (int)boostTime;
            string timer = ((int)time / 3600).ToString("00") + ":";
            time -= ((int)time / 3600) * 3600;
            timer += "" + ((int)time / 60).ToString("00") + ":";
            timer += "" + ((int)time % 60).ToString("00");
            boostTimeText.text = timer;
            boostFill.fillAmount = ((boostTime * 1f) / 7200f);
        }
        else if (coinBoost)
        {
            EndBoost();
        }
    }

    public void ShiftPanel(bool active)
    {
        if (tutoOn && tuto.current < 6)
            return;
        if (CheckUI() && active)
            return;
        SC.INS.PlaySound(0, 13, 0);
        if (active)
        {
            shiftPanel.SetActive(true);
            if (tutoOn && tuto.current == 6)
            {
                tuto.Next();
            }
            if (wages > 0)
            {
                for (int i = 0; i < shiftCostText.Length; i++)
                {
                    if (((i + 1) * (i + 1)) <= level || i == 0)
                    {
                        shiftLock[i].SetActive(false);
                        shiftCostText[i].gameObject.SetActive(!work);
                        shiftCostText[i].text = ((int)(shiftCosts[i + 1] + shiftCosts[i + 1] * ((wages * 1f) / 100f))).ToString();
                    }
                    else
                    {
                        shiftCostText[i].gameObject.SetActive(false);
                        shiftLock[i].SetActive(true);
                    }
                }
                wagesText.text = "%" + wages;
            }
            else
            {
                wagesText.text = "%0";
                for (int i = 0; i < shiftCostText.Length; i++)
                {
                    if (((i + 1) * (i + 1)) <= level)
                    {
                        shiftLock[i].SetActive(false);
                        shiftCostText[i].gameObject.SetActive(!work);
                        shiftCostText[i].text = (shiftCosts[i + 1]).ToString();
                    }
                    else
                    {
                        shiftCostText[i].gameObject.SetActive(false);
                        shiftLock[i].SetActive(true);
                    }
                }
            }

        }
        else
        {
            if (tutoOn && tuto.current == 6 && work)
            {
                tuto.Next();
            }
            if (IsInvoking("ShiftPanelOff"))
                return;
            shiftPanel.GetComponentInChildren<InOutAnim>().OutAnim();

            Invoke("ShiftPanelOff", .3f);
        }
    }
    void ShiftPanelOff()
    {
        shiftPanel.SetActive(false);
    }
    void SetShift()
    {
        //Debug.Log(logTime);
        secondsDif = (int)((DateTime.UtcNow - logTime).TotalSeconds);
        shiftTimeLog = shiftTime;
        if (shiftType > 0 && shiftTime - secondsDif > 0)
        {
            work = true;
            //Debug.Log("Working...");
            roomControllers[0].SetWork(work, false);
            if (shiftTime - secondsDif < 360)
                rdb.CancelNotification();
        }
        else
        {
            shiftType = 0;
            work = false;
            secondsDif = shiftTime;
            shiftTime = 0;
            roomControllers[0].SetWork(work, false);
            shiftTimer.text = "00:00:00";
            shiftAnim.SetTrigger("End");
            doubleShift = false;
        }

        shift = SwitchShift(shiftType);
    }
    #endregion
    #region Phone
    int currentPhoneP;
    public void OpenPhone(int x)
    {
        if (CheckUI())
            return;
        if (tutoOn)
        {
            if (tuto.current != 17 || x != 0)
                return;
        }
        if (x == 1)
        {
            phoneNoInternet.SetActive(false);
            phone.SetActive(true);
            for (int i = 0; i < phonePanels.Length; i++)
                phonePanels[i].SetActive(false);
            currentPhoneP = x;
            CheckConnectivity();
            SC.INS.PlaySound(0, 13, 0);
            phonePanels[currentPhoneP].SetActive(true);
            return;
        }
        if (phoneConnectionCheck.activeInHierarchy)
            return;
        phoneNoInternet.SetActive(false);
        phone.SetActive(true);
        phoneConnectionCheck.SetActive(true);
        for (int i = 0; i < phonePanels.Length; i++)
            phonePanels[i].SetActive(false);
        currentPhoneP = x;
        CheckConnectivity();
        SC.INS.PlaySound(0, 13, 0);
        if (tutoOn && x == 0 && !phoneNoInternet.activeInHierarchy && tuto.current == 17)
        {
            FRC.INS.InstantiateTutoFriend();
            tuto.OpenFriends();
        }
    }
    public void CheckConnectivity()
    {
        StartCoroutine(checkInternet((isConnected) =>
        {
            if (isConnected)
            {
                phonePanels[currentPhoneP].SetActive(true);
                if (currentPhoneP == 0)
                    phoneNot.SetActive(false);
                if (currentPhoneP == 3)
                    dm.dot.SetActive(false);
            }
            else
            {
                if (currentPhoneP != 1)
                    phoneNoInternet.SetActive(true);

            }
            phoneConnectionCheck.SetActive(false);
        }));
    }
    IEnumerator checkInternet(Action<bool> action)
    {
        UnityWebRequest www = new UnityWebRequest("https://www.google.com/");
        yield return www.SendWebRequest();
        if (www.error != null)
        {
            action(false);
        }
        else
        {
            action(true);
        }
    }
    public void ClosePhone()
    {

        phone.GetComponentInChildren<InOutAnim>().OutAnim();
        Invoke("PhoneActiveFalse", .3f);
        if (tutoOn && currentPhoneP == 0 && phoneNoInternet.activeInHierarchy && tuto.current == 17)
        {
            tuto.Next();
        }
    }
    void PhoneActiveFalse()
    {
        phone.SetActive(false);
    }
    #endregion
    #region rooms
    public void BuyRoom(int id)
    {
        if (coins >= Room.costs[id] && Room.costs[id] > 0 || gems >= (Room.costs[id] * -1) && Room.costs[id] < 0)
        {
            BeginDrag();
            GameObject temp = Instantiate(roomsObj[id], roomsArrange.transform);
            currentRoom = temp.GetComponentInChildren<Room>();
            currentRoom.Purchased();
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
        GameObject temp = Instantiate(roomsObj[rb.id], roomsArrange.transform);
        currentRoom = temp.GetComponentInChildren<Room>();
        currentRoom.GiftPurchased(rb);
    }

    public void BuyDecoration(int id)
    {
        if (coins >= Decoration.costs[currentType, id] && Decoration.costs[currentType, id] >= 0 || gems >= (Decoration.costs[currentType, id] * -1) && Decoration.costs[currentType, id] < 0)
        {
            BeginDrag();
            GameObject temp = Instantiate(decorationsObj[currentType], roomsArrange.transform);
            currentDecoration = temp.GetComponentInChildren<Decoration>();
            currentDecoration.Purchased(SM.INS.GetRoomObject(currentType, id), id);

        }
        else
        {
            if (Decoration.costs[currentType, id] >= 0)
                errorM.Error(0);
            else
                errorM.Error(1);
        }

    }
    public void BuyDecoration(int id, int type)
    {
        if (coins >= Decoration.costs[type, id] && Decoration.costs[type, id] >= 0 || gems >= (Decoration.costs[type, id] * -1) && Decoration.costs[type, id] < 0)
        {
            BeginDrag();
            GameObject temp = Instantiate(decorationsObj[type], roomsArrange.transform);
            currentDecoration = temp.GetComponentInChildren<Decoration>();
            currentDecoration.Purchased(SM.INS.GetRoomObject(type, id), id);

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
        GameObject temp = Instantiate(decorationsObj[db.type], roomsArrange.transform);
        currentDecoration = temp.GetComponentInChildren<Decoration>();
        currentDecoration.GiftPurchased(SM.INS.GetRoomObject(db.type, db.id), db.id, db);
    }
    public void BuyOufit(int id)
    {
        if (coins >= StaffOutfit.costs[id] && StaffOutfit.costs[id] >= 0 || gems >= (StaffOutfit.costs[id] * -1) && StaffOutfit.costs[id] < 0)
        {
            BeginDrag();
            GameObject temp = Instantiate(staffOufit, roomsArrange.transform);
            currentStaffOutfit = temp.GetComponentInChildren<StaffOutfit>();
            currentStaffOutfit.Purchased(id);

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
        GameObject temp = Instantiate(staffOufit, roomsArrange.transform);
        currentStaffOutfit = temp.GetComponentInChildren<StaffOutfit>();
        currentStaffOutfit.Gift(ob);
    }
    public void BuyCharacter(Character character)
    {
        if (character.id < 20)
        {
            if (coins >= Staff.costs[character.id] && Staff.costs[character.id] >= 0 || gems >= (Staff.costs[character.id] * -1) && Staff.costs[character.id] < 0)
            {
                BeginDrag();
                GameObject temp = Instantiate(staffHire, transform.position, transform.rotation);
                currentStaffHire = temp.GetComponentInChildren<StaffHire>();
                currentStaffHire.Purchased(character);
            }
            else
            {
                if (Staff.costs[character.id] >= 0)
                    errorM.Error(0);
                else
                    errorM.Error(1);
            }
        }
        else
        {
            if (coins >= 500)
            {
                BeginDrag();
                GameObject temp = Instantiate(staffHire, transform.position, transform.rotation);
                currentStaffHire = temp.GetComponentInChildren<StaffHire>();
                currentStaffHire.Purchased(character);
            }
            else
            {
                errorM.Error(0);
            }
        }

    }
    public void StaffGift(StaffBtn sb)
    {
        BeginDrag();
        GameObject temp = Instantiate(staffHire, transform.position, transform.rotation);
        currentStaffHire = temp.GetComponentInChildren<StaffHire>();
        currentStaffHire.GiftStaff(sb);
    }
    public void BuyOutside(int id)
    {
        if (coins >= OutsideO.costs[id] && OutsideO.costs[id] > 0 || gems >= (OutsideO.costs[id] * -1) && OutsideO.costs[id] < 0)
        {
            BeginDrag();
            GameObject temp = Instantiate(outsideO, roomsArrange.transform);
            currentOutside = temp.GetComponentInChildren<OutsideO>();
            currentOutside.Purchased(id);
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
        GameObject temp = Instantiate(outsideO, roomsArrange.transform);
        currentOutside = temp.GetComponentInChildren<OutsideO>();
        currentOutside.Gift(ob);
    }
    public void SetType(int type)
    {
        currentType = type;
    }
    public void BeginDrag()
    {
        isDragging = true;
        buyingCanvas.SetActive(false);
        editingCanvas.SetActive(true);
        if (tutoOn)
        {
            tuto.PointerOff();
        }
    }
    // Cuando editamos la posicion
    public void BeginDrag(Room room)
    {
        isDragging = true;
        buyingCanvas.SetActive(false);
        editingCanvas.SetActive(true);
        currentRoom = room;

    }
    public void BeginDrag(Decoration decoration)
    {
        isDragging = true;
        buyingCanvas.SetActive(false);
        editingCanvas.SetActive(true);
        currentDecoration = decoration;
    }
    public void BeginDrag(OutsideO outsideO)
    {
        isDragging = true;
        buyingCanvas.SetActive(false);
        editingCanvas.SetActive(true);
        currentOutside = outsideO;

    }
    public void EndDrag()
    {
        isDragging = false;
        if (visit)
            return;
        buyingCanvas.SetActive(true);
        editingCanvas.SetActive(false);
        shopPanel.SetActive(lockShop);
        if (!lockShop)
        {
            shopping = false;
            if (shopPanelActive == 1)
                CloseDecoratingPanel();
        }
        else
        {
            OpenSubShop(subShopPanelActive);
        }

        currentRoom = null;
        currentDecoration = null;
        currentStaffHire = null;
        currentStaffOutfit = null;
        currentOutside = null;
    }
    public int AddRoom(RoomC room, Room roomController)
    {
        roomControllers.Add(roomController);
        rooms.Add(room);
        int roomTime = roomController.roomTime;
        int roomCoins = roomController.roomCoins;
        int slots = roomController.slotController.slots.Count;
        float ave = (roomCoins * 1f * slots) / (roomTime * 1f);
        if (!loadingPanel.activeInHierarchy)
        {
            if (((totalAverage + ave) * 3600) > 1000 && totalAverage * 3600 < 1000)
            {
                pg.Achivements(2, 0);
            }
            if (((totalAverage + ave) * 3600) > 2000 && totalAverage * 3600 < 2000)
            {
                pg.Achivements(2, 1);
            }
            if (((totalAverage + ave) * 3600) > 5000 && totalAverage * 3600 < 5000)
            {
                pg.Achivements(2, 2);
            }
        }
        //Debug.Log("Room Time: " + roomTime+ "  Room Coins: " + roomCoins+ "  Slots Counts: " + slots + "  ave:" +ave);
        coinsAverage += ave;
        totalAverage = coinsAverage + parkingCoinsAverage;
        if ((int)(totalAverage * 3600) > 1800 * (prestige + 1))
            p.CheckPrestige((int)(totalAverage * 3600));
        rateText.text = ((int)(totalAverage * 3600)).ToString() + "/" + t.GetText(26);
        return rooms.Count - 1;
    }
    public void ModifyRoom(int number, RoomC room)
    {
        //Debug.Log(room.characters);
        //Debug.Log(rooms.Count);
        rooms[number] = room;
    }
    public void DeleteRoom(int number, Room controller)
    {
        int roomTime = controller.roomTime;
        int roomCoins = controller.roomCoins;
        float ave = (roomCoins * 1f) / (roomTime * 1f);
        coinsAverage -= ave;
        rateText.text = ((int)(coinsAverage * 3600)).ToString() + "/" + t.GetText(26);
        totalAverage = coinsAverage + parkingCoinsAverage;
        deletedRooms.Add(rooms[number]);
    }
    public int AddOutside(Outside outside)
    {
        outsideObjects.Add(outside);
        return outsideObjects.Count - 1;
    }
    public void ModifyOutside(int number, Outside outside)
    {
        outsideObjects[number] = outside;
    }
    public void DeleteOutside(int number)
    {
        deletedOutside.Add(outsideObjects[number]);
    }
    Room currentRoom;
    Decoration currentDecoration;
    StaffHire currentStaffHire;
    StaffOutfit currentStaffOutfit;
    OutsideO currentOutside;
    public void Purchase(int x)
    {
        SC.INS.PlaySound(0, 12, 0);
        if (x > 0)
        {
            dm.AddTask(20, x);
            coins -= x;
            coinsAnim.Play("CoinTextMinus", -1, 0);
            coinsText.text = coins.ToString("n0");
        }
        else
        {
            gems += x;
            gemsText.GetComponentInParent<Animator>().Play("CoinTextMinus", -1, 0);
            gemsText.text = gems.ToString("n0");
        }
    }
    public void PurchaseMute(int x)
    {
        if (x > 0)
        {
            coins -= x;
            dm.AddTask(20, x);
            coinsAnim.Play("CoinTextMinus", -1, 0);
            coinsText.text = coins.ToString("n0");
        }
        else
        {
            gems += x;
            gemsText.GetComponentInParent<Animator>().Play("CoinTextMinus", -1, 0);
            gemsText.text = gems.ToString("n0");
        }


    }
    public void Trashing()
    {
        trashIm.sprite = trashsp2;
        trashIm.transform.localScale = Vector3.one * 1.2f;
        if (currentRoom != null)
        {
            if (currentRoom.Trashing())
            {
                int cost = Room.costs[currentRoom.id];
                if (cost > 0)
                {
                    addedCoinsText.color = new Color(1, 1, .38f);
                }
                else
                {
                    cost *= -1;
                    addedCoinsText.color = new Color(0.6f, 0.85f, 1);
                }
                addedCoinsText.text = "+" + (int)(cost * .4f);
                int tempSetPos = (int)Mathf.Floor(Mathf.Log10(coins) + 1);
                tempSetPos = Mathf.Clamp(tempSetPos, 0, 7);
                Vector2 tempPos = addedCoinsText.rectTransform.anchoredPosition;
                tempPos.x = 30 + tempSetPos * 12.5f;
                addedCoinsText.gameObject.SetActive(true);
            }

        }
        else if (currentDecoration != null)
        {
            currentDecoration.Trashing();
        }
        else if (currentStaffHire)
        {
            currentStaffHire.Trashing();
        }
        else if (currentStaffOutfit)
        {
            currentStaffOutfit.Trashing();
        }
        else if (currentOutside)
        {
            if (currentOutside.Trashing())
            {
                int cost = OutsideO.costs[currentOutside._id];
                if (cost > 0)
                {
                    addedCoinsText.color = new Color(1, 1, .38f);
                }
                else
                {
                    cost *= -1;
                    addedCoinsText.color = new Color(0.6f, 0.85f, 1);
                }
                addedCoinsText.text = "+" + (int)(cost * .4f);
                int tempSetPos = (int)Mathf.Floor(Mathf.Log10(coins) + 1);
                tempSetPos = Mathf.Clamp(tempSetPos, 0, 7);
                Vector2 tempPos = addedCoinsText.rectTransform.anchoredPosition;
                tempPos.x = 30 + tempSetPos * 12.5f;
                addedCoinsText.gameObject.SetActive(true);
            }
        }

    }
    public void TrashingExit()
    {
        if (currentRoom != null)
        {
            currentRoom.TrashingExit();
            addedCoinsText.gameObject.SetActive(false);
        }
        else if (currentDecoration != null)
        {
            currentDecoration.TrashingExit();
        }
        else if (currentStaffHire != null)
        {
            currentStaffHire.TrashingExit();
        }
        else if (currentStaffOutfit)
        {
            currentStaffOutfit.TrashingExit();
        }
        else if (currentOutside)
        {
            currentOutside.TrashingExit();
        }
        trashIm.sprite = trashsp1;
        trashIm.transform.localScale = Vector3.one;
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
    #region Maintenance
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
    #endregion
    #endregion

    private void FixedUpdate()
    {
        if (timeToSpawn <= 0)
        {
            InstantiateCostumer();
            timeToSpawn = spawnStartTime / (slotsID.Count + 3) - (stars * 5) / (slotsID.Count + 3) + (UnityEngine.Random.Range(0.01f, 0.25f) * ((slotsID.Count * 1f) / 12f)) + UnityEngine.Random.Range(-0.1f, 0.3f);
            if (timeToSpawn > 8)
            {
                timeToSpawn = (UnityEngine.Random.Range(1f, 2.5f));
            }
            if (!work)
                timeToSpawn *= 2;
        }
        else
        {
            timeToSpawn -= Time.fixedDeltaTime;
        }
        if (!work)
            return;
        if (timeToSpawn2 <= 0)
        {
            InatantiateCostumerNot();
            timeToSpawn2 = level / 3 + UnityEngine.Random.Range(6, 12) + slotsID.Count / 6;
        }
        else
        {
            timeToSpawn2 -= Time.fixedDeltaTime;
        }
    }
    void CheckInternetAd()
    {
        StartCoroutine(checkInternet((isConnected) =>
        {
            if (isConnected)
            {

                if (ad.type == 1 || UnityEngine.Random.Range(0, 4) == 0 || slotsID.Count < 5)
                {
                    Costumer temp = Instantiate(cosumterPlusM, costumersArrange.transform).GetComponentInChildren<Costumer>();
                    temp.Create(false, limitL, limitR);
                    costumerReward = temp;
                    ad.SetReward(0);
                }
                else
                {
                    Zeppeling temp = Instantiate(zeppeling, costumersArrange.transform).GetComponentInChildren<Zeppeling>();
                    temp.Create(limitL, limitR, limitY);
                    currentZep = temp;
                    ad.SetReward(1);
                }
            }

        }));
    }

    void InatantiateCostumerNot()
    {
        if (UnityEngine.Random.Range(0, 2) == 0 && costumerReward == null && currentZep == null && (DateTime.UtcNow - lastAd).TotalSeconds > (90 + UnityEngine.Random.Range(0, 25)) && level > 2)
        {
            CheckInternetAd();
        }
        else
        {
            Costumer temp = Instantiate(costumer, costumersArrange.transform).GetComponentInChildren<Costumer>();
            temp.Create(false, limitL, limitR);
        }

    }
    public void InstantiateCostumerGoing()
    {
        Costumer temp = Instantiate(costumer, costumersArrange.transform).GetComponentInChildren<Costumer>();
        temp.Create(true, limitL, limitR);
    }
    public void InstantiateCostumerZeppeling(Vector2 pos)
    {
        int count = UnityEngine.Random.Range(5, 7) + slotsID.Count / 6;
        SC.INS.PlaySound(1, 3, 1);
        SC.INS.PlaySound(0, 15, 0);
        for (int i = 0; i < count; i++)
        {
            Costumer temp = Instantiate(costumer, costumersArrange.transform).GetComponentInChildren<Costumer>();
            temp.Create(true, limitL, limitR);
            temp.Zeppeling(pos);
        }
    }
    public void InstantiateCostumer()
    {
        bool going = false;
        float r = UnityEngine.Random.Range(0f, 1.8f);
        float calc = 0.7f - 1f / (slotsID.Count / 3f + 4f) + stars * .06f;
        //Debug.Log(r + "   " + calc);
        if (r < calc)
            going = true;
        if (!work && UnityEngine.Random.Range(0, 3) == 0)
            going = false;

        Costumer temp = Instantiate(costumer, costumersArrange.transform).GetComponentInChildren<Costumer>();
        temp.Create(going, limitL, limitR);
    }
    public Slot SetCostumer(bool fromStart)
    {
        Slot temp = null;
        if (slotsID.Count > 0)
        {
            int index = UnityEngine.Random.Range(0, slotsID.Count);
            temp = slots[slotsID[index]];
            slotsID.RemoveAt(index);
            roomControllers[temp.roomId].SetCostumer();
            if (fromStart)
                AddXp(UnityEngine.Random.Range(0, temp.coins + 1));
            else
                AddXp(temp.coins);
        }

        return temp;
    }

    public void OpenShop(int x)
    {
        if (tutoOn)
        {
            if (tuto.current >= 6 && tuto.current < 11)
                return;
            if (tuto.current > 10 && tuto.current < 13 && x != 1)
                return;
            if (tuto.current == 13 || tuto.current == 14 || tuto.current == 16 || tuto.current == 17)
                return;
            if (tuto.current == 15 && x != 0)
                return;
            if (tuto.current == 18 && x != 2 && level == 2)
                return;
            if (tuto.current >= 19 && x != 0)
                return;
        }
        if (CheckUI())
            return;
        for (int i = 0; i < shopsPanels.Length; i++)
            shopsPanels[i].SetActive(false);
        shopsPanels[x].SetActive(true);
        if (shopPanel.activeInHierarchy && shopPanelActive == 1)
            CloseDecoratingPanel();
        shopPanel.SetActive(true);
        shopPanelActive = x;
        gift.DotActive(x);
        if (x == 1)
        {
            decorating = true;
            for (int i = 0; i < roomControllers.Count; i++)
            {
                if (roomControllers[i])
                    roomControllers[i].DecorationEditing(true);
            }
        }
        if (tutoOn)
        {
            if (tuto.current == 13)
            {
                CloseShop();
                return;
            }

            if (tuto.current == 2)
                tuto.OpenShop();
            else if (tuto.current == 11 && x == 1)
                tuto.Next();
            else if (tuto.current == 15 && x == 0)
            {
                tuto.OpenShopFix();
            }
            else if (tuto.current == 18 && x == 2)
            {
                tuto.OpenShopExpand();
            }
            else if (tuto.current == 19 && x == 0)
                tuto.Next();
        }

        OpenSubShop(0);
        SC.INS.PlaySound(0, 13, 0);
        shopping = true;
    }
    public void OpenSubShop(int x)
    {
        if (tutoOn)
        {
            if (shopPanelActive == 1 && tuto.current > 10 && tuto.current < 13 && x != 0)
                return;
            if (shopPanelActive == 0 && tuto.current == 15 && x != 4)
                return;
            if (shopPanelActive == 0 && tuto.current == 20 && x != 3)
                return;
        }
        listNumber = 0;
        subShopPanelActive = x;
        ChangeSubList(0);
        expandUI.SetActive(false);
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
                if (tutoOn)
                {
                    if (x == 4 && tuto.current == 15)
                        tuto.OpenSubShopFix();
                    else if (x == 3 && tuto.current == 20)
                        tuto.OpenSubShopGym();
                }
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
                if (x == 0)
                {
                    expandUI.SetActive(true);
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
    void CloseDecoratingPanel()
    {
        decorating = false;
        for (int i = 0; i < roomControllers.Count; i++)
        {
            if (roomControllers[i])
                roomControllers[i].DecorationEditing(false);
        }
    }
    public void LockShop()
    {
        if (tutoOn && tuto.current == 3)
        {
            tuto.LockShop();
        }
        lockShop = !lockShop;
        lockedShop.SetActive(lockShop);
        unlockedShop.SetActive(!lockShop);
    }
    public void CloseShop()
    {
        expandUI.SetActive(false);
        lockShop = false;
        lockedShop.SetActive(lockShop);
        unlockedShop.SetActive(!lockShop);
        shopPanel.GetComponent<Animator>()?.SetTrigger("Out");
        Invoke("SetShopOff", .5f);
        if (shopPanelActive == 1)
            CloseDecoratingPanel();
        SC.INS.PlaySound(0, 13, 0);
        shopping = false;
        if (tutoOn && shopPanelActive == 0 && tuto.current == 5)
        {
            tuto.Next();
        }
    }
    void SetShopOff()
    {
        shopPanel.SetActive(false);
    }
    public bool blockX2;
    public void AddCoins(int x)
    {
        if (!blockX2)
        {
            if (coinBoost || iap[0])
                x *= 2;
        }

        coins += x;
        addedCoinsText.text = "+" + x;
        addedCoinsText.gameObject.SetActive(true);
        int tempSetPos = (int)Mathf.Floor(Mathf.Log10(coins) + 1);
        tempSetPos = Mathf.Clamp(tempSetPos, 0, 7);
        Vector2 tempPos = addedCoinsText.rectTransform.anchoredPosition;
        tempPos.x = 30 + tempSetPos * 12.5f;
        addedCoinsText.rectTransform.anchoredPosition = tempPos;
        if (x > 0)
            SC.INS.PlaySound(1, 5, 0);
        coinsAnim.Play("CoinTextPlus", -1, 0);
        if (IsInvoking("SetCoins"))
            CancelInvoke("SetCoins");

        Invoke("SetCoins", .25f);
        blockX2 = false;
    }
    public void AddGems(int x)
    {
        gems += x;
        gemsText.text = gems.ToString("n0");
        addedCoinsText.gameObject.SetActive(false);
        if (x > 0)
            SC.INS.PlaySound(1, 5, 0);
    }
    void SetCoins()
    {
        addedCoinsText.gameObject.SetActive(false);
        coinsText.text = coins.ToString("n0");
    }

    public void AddXp(int x)
    {
        xp += x;
        if (level >= 64)
        {
            level = 64;
            levelBar.fillAmount = 1;
            xpLeftText.text = "";
            return;
        }
        levelBar.fillAmount = (xp * 1f - levelxp[level] * 1f) / (levelxp[level + 1] * 1f - levelxp[level] * 1f) * .99f;
        if (!visit)
            levelAnim.Play("LevelBarPlus", -1, 0);
        if (xp >= levelxp[level + 1])
        {
            if (tutoOn && tuto.current > 10 && tuto.current < 18 || !gameObject.activeInHierarchy)
                return;
            level++;
            levelText.text = level.ToString();
            levelTCus.text = level.ToString();
            levelBar.fillAmount = (xp * 1f - levelxp[level] * 1f) / (levelxp[level + 1] * 1f - levelxp[level] * 1f) * .99f;
            if (!whileYouAway.activeInHierarchy)
                LevelUp();
            else
                Invoke("LevelUp", 1.5f);
        }
        xpLeftText.text = "" + (levelxp[level + 1] - xp);
    }


    void LevelUp()
    {
        pg.Achivements(0, 0);
        levelPanel.SetActive(true);
        int tempCoins = 0;
        if (Mathf.Abs(coinsSave - coins) > 100 && !tutoOn)
        {
            SaveFromBtn();
        }
        levelBlocksOb.SetActive(false);
        SC.INS.PlaySound(0, 16, 0);
        levelUpText.text = level.ToString();
        switch (level)
        {
            case 1:
                tempCoins = 10000;
                levelTexts[0].text = t.GetText(1) + "Double, Janitor" + t.GetText(4) + "Janitor" + t.GetText(25);
                levelTexts[3].text = t.GetText(2) + levelxp[level + 1].ToString() + t.GetText(3) + "Triple, Twin, Gym, Gym" + t.GetText(25) + t.GetText(4) + t.GetText(5);
                break;
            case 2:
                tempCoins = 15000;
                levelTexts[0].text = t.GetText(1) + "Triple, Twin, Gym" + t.GetText(4) + "Gym" + t.GetText(25);
                levelTexts[3].text = t.GetText(2) + levelxp[level + 1].ToString() + t.GetText(3) + "Quad, Prison Cell, Restaurant, Cinema, Cheff Outfit, Advertisament" + t.GetText(4) + "Street Light";
                levelBlocksOb.SetActive(true);
                levelTexts[1].text = t.GetText(5);
                break;
            case 3:
                tempCoins = 5000;
                if (!noti)
                    dm.SetNoti();
                levelTexts[0].text = t.GetText(1) + "Quad, Prison Cell, Restaurant, Cinema, Cheff Outfit, Advertisament" + t.GetText(4) + "Street Light";
                levelTexts[3].text = t.GetText(2) + levelxp[level + 1].ToString() + t.GetText(3) + "Window, Arcade, Office, Comfort, Suit Outfit, Vip Chain" + t.GetText(4) + t.GetText(6);
                break;
            case 4:
                appReview.AskReview();
                tempCoins = 7500;
                levelTexts[0].text = t.GetText(1) + "Window, Arcade, Office, Comfort, Office" + t.GetText(25) + t.GetText(4) + "Vip Chain";
                levelTexts[3].text = t.GetText(2) + levelxp[level + 1].ToString() + t.GetText(3) + "Junior-Suite, France, Mexico, Bar" + t.GetText(4) + "Waiter" + t.GetText(25);
                levelBlocksOb.SetActive(true);
                levelTexts[1].text = t.GetText(6);
                break;
            case 5:
                tempCoins = 10000;
                levelTexts[0].text = t.GetText(1) + "Junior-Suite, France, Mexico, Bar" + t.GetText(4) + "Waiter" + t.GetText(25);
                levelTexts[3].text = t.GetText(2) + levelxp[level + 1].ToString() + t.GetText(3) + "Globe, Old Prison, Cheese, Plumber, Plumber" + t.GetText(25) + t.GetText(4) + "Lights";
                break;
            case 6:
                appReview.AskReview();
                tempCoins = 10000;
                levelTexts[0].text = t.GetText(1) + "Globe, Old Prison, Cheese, Plumber, Plumber" + t.GetText(25) + t.GetText(4) + "Lights";
                levelTexts[3].text = t.GetText(2) + levelxp[level + 1].ToString() + t.GetText(3) + "Fancy, Laundry, Key Builder, Locksmith" + t.GetText(25) + t.GetText(4) + t.GetText(7);
                break;
            case 7:
                tempCoins = 12500;
                levelTexts[0].text = t.GetText(1) + "Fancy, Laundry, Key Builder" + t.GetText(4) + "Locksmith" + t.GetText(25);
                levelTexts[3].text = t.GetText(2) + levelxp[level + 1].ToString() + t.GetText(3) + "Witch Room, Cozy Modern, Parking" + t.GetText(4) + "Chill" + t.GetText(25);
                levelBlocksOb.SetActive(true);
                levelTexts[1].text = t.GetText(7);
                break;
            case 8:
                tempCoins = 15000;
                levelTexts[0].text = t.GetText(1) + "Witch Room, Cozy Modern, Parking" + t.GetText(4) + "Chill" + t.GetText(25);
                levelTexts[3].text = t.GetText(2) + levelxp[level + 1].ToString() + t.GetText(3) + "Pirate Room, Asian, Party Lounge, Clown, Guitarrist" + t.GetText(4) + "Joe" + t.GetText(25);
                break;
            case 9:
                if (!noti)
                    dm.SetNoti();
                tempCoins = 15000;
                levelTexts[0].text = t.GetText(1) + "Pirate Room, Asian, Party Lounge, Clown, Guitarrist" + t.GetText(4) + "Joe" + t.GetText(25);
                levelTexts[3].text = t.GetText(2) + levelxp[level + 1].ToString() + t.GetText(3) + "Electrician, Bohemian, Master-Suite, Shop, Electrician" + t.GetText(25) + t.GetText(4) + t.GetText(8);
                break;
            case 10:
                appReview.AskReview();
                tempCoins = 17500;
                levelTexts[0].text = t.GetText(1) + "Electrician, Bohemian, Master-Suite, Shop" + t.GetText(4) + "Electrician" + t.GetText(25);
                levelTexts[3].text = t.GetText(2) + levelxp[level + 1].ToString() + t.GetText(3) + "Flipped Room, Luxury Room" + t.GetText(4) + "Hoodie, Penguin" + t.GetText(4) + "Hot Dog" + t.GetText(25);
                levelBlocksOb.SetActive(true);
                levelTexts[1].text = t.GetText(8);
                break;
            case 11:
                tempCoins = 20000;
                levelTexts[0].text = t.GetText(1) + "Flipped Room, Luxury Room" + t.GetText(4) + "Hoddie, Penguin" + t.GetText(4) + "HotDog" + t.GetText(25);
                levelTexts[3].text = t.GetText(2) + levelxp[level + 1].ToString() + t.GetText(3) + "Kid Room, Future Room, Bench, Red Bench, Totem" + t.GetText(25) + t.GetText(4) + t.GetText(9);
                break;
            case 12:
                tempCoins = 20000;
                levelTexts[0].text = t.GetText(1) + "Kid Room, Future Room, Bench, Red Bench" + t.GetText(4) + "Totem" + t.GetText(25);
                levelTexts[3].text = t.GetText(2) + levelxp[level + 1].ToString() + t.GetText(3) + "Cowboy Room, Foodtrucks, Trash Can, Fireplug, AU Uniform" + t.GetText(4) + "Peaky" + t.GetText(25);
                levelBlocksOb.SetActive(true);
                levelTexts[1].text = t.GetText(9);
                break;
            case 13:
                tempCoins = 20000;
                levelTexts[0].text = t.GetText(1) + "Cowboy Room, Foodtrucks, Trash Can, Fireplug, AU Uniform" + t.GetText(4) + "Peaky" + t.GetText(25);
                levelTexts[3].text = t.GetText(2) + levelxp[level + 1].ToString() + t.GetText(3) + "Modern, Palace" + t.GetText(4) + "Trees";
                break;
            case 14:
                tempCoins = 20000;
                levelTexts[0].text = t.GetText(1) + "Modern, Palace" + t.GetText(4) + "Trees";
                levelTexts[3].text = t.GetText(2) + levelxp[level + 1].ToString() + t.GetText(3) + "Viking Room, Disco, Payphone" + t.GetText(4) + t.GetText(10);
                break;
            case 15:
                tempCoins = 22500;
                levelTexts[0].text = t.GetText(1) + "Viking Room, Disco" + t.GetText(4) + "Payphone";
                levelTexts[3].text = t.GetText(2) + levelxp[level + 1].ToString() + t.GetText(3) + "Spaceship, Play Room, Red Trash, Traffic Lights";
                levelBlocksOb.SetActive(true);
                levelTexts[1].text = t.GetText(10);
                break;
            case 16:
                tempCoins = 25000;
                levelTexts[0].text = t.GetText(1) + "Spaceship, Play Room, Red Trash" + t.GetText(4) + "Traffic Lights";
                levelTexts[3].text = t.GetText(2) + levelxp[level + 1].ToString() + t.GetText(3) + "Blue VIP, Swiming Pool, Walk, Palm" + t.GetText(4) + "Life Guard";

                break;
            case 17:
                tempCoins = 25000;
                levelTexts[0].text = t.GetText(1) + "Blue VIP, Swiming Pool, Walk, Palm" + t.GetText(4) + "Life Guard";
                levelTexts[3].text = t.GetText(2) + levelxp[level + 1].ToString() + t.GetText(3) + "Casino, Fences, Chill" + t.GetText(25) + t.GetText(4) + t.GetText(11);
                break;
            case 18:
                tempCoins = 25000;
                levelTexts[0].text = t.GetText(1) + "Casino, Fences" + t.GetText(4) + "Chill" + t.GetText(25);
                levelTexts[3].text = t.GetText(2) + levelxp[level + 1].ToString() + t.GetText(3) + "Chinese Food, Small Tree" + t.GetText(4) + "Pine";
                levelBlocksOb.SetActive(true);
                levelTexts[1].text = t.GetText(11);
                break;
            case 19:
                tempCoins = 25000;
                levelTexts[0].text = t.GetText(1) + "Chinese Food, Small Tree" + t.GetText(4) + "Pine";
                levelTexts[3].text = t.GetText(2) + levelxp[level + 1].ToString() + t.GetText(3) + "Museum, Handrail, Swing" + t.GetText(4) + "Joe" + t.GetText(25);
                break;
            case 20:
                tempCoins = 27500;
                levelTexts[0].text = t.GetText(1) + "Museum, Handrail, Swing" + t.GetText(4) + "Joe" + t.GetText(25);
                levelTexts[3].text = t.GetText(2) + levelxp[level + 1].ToString() + t.GetText(3) + "Fountain";

                break;
            case 21:
                tempCoins = 30000;
                levelTexts[0].text = t.GetText(1) + "Fountain";
                levelTexts[3].text = t.GetText(2) + levelxp[level + 1].ToString() + t.GetText(3) + "Pocket Hotel Sign, Hoodie" + t.GetText(4) + t.GetText(12);
                break;
            case 22:
                tempCoins = 30000;
                levelTexts[0].text = t.GetText(1) + "Pocket Hotel Sign" + t.GetText(4) + "Hoodie";
                levelTexts[3].text = t.GetText(2) + levelxp[level + 1].ToString() + t.GetText(3) + "Popcorn, Hot Dogs" + t.GetText(4) + "Totem" + t.GetText(25);
                levelBlocksOb.SetActive(true);
                levelTexts[1].text = t.GetText(12);
                break;
            case 23:
                tempCoins = 30000;
                levelTexts[0].text = t.GetText(1) + "Popcorn, Hot Dogs" + t.GetText(4) + "Totem" + t.GetText(25);
                levelTexts[3].text = t.GetText(2) + levelxp[level + 1].ToString() + t.GetText(3) + "Taxi, Police Car" + t.GetText(4) + "Jacket" + t.GetText(4) + t.GetText(13);
                break;
            case 24:
                tempCoins = 30000;
                levelTexts[0].text = t.GetText(1) + "Taxi, Police Car" + t.GetText(4) + "Jacket";
                levelTexts[3].text = t.GetText(2) + levelxp[level + 1].ToString() + t.GetText(3) + "Suit";
                levelBlocksOb.SetActive(true);
                levelTexts[1].text = t.GetText(13);
                break;
            case 25:
                tempCoins = 30000;
                levelTexts[0].gameObject.SetActive(false);
                levelTexts[3].text = t.GetText(2) + levelxp[level + 1].ToString() + t.GetText(3) + "Shirt, Skirt" + t.GetText(4) + t.GetText(14);
                break;
            case 26:
                tempCoins = 30000;
                levelTexts[0].text = t.GetText(1) + "Shirt" + t.GetText(4) + "Skirt";
                levelTexts[3].text = t.GetText(2) + levelxp[level + 1].ToString() + t.GetText(3) + "Minishorts" + t.GetText(4) + "Police" + t.GetText(25);
                levelBlocksOb.SetActive(true);
                levelTexts[1].text = t.GetText(14);
                break;
            case 27:
                dm.SetNoti();
                tempCoins = 32500;
                levelTexts[0].text = t.GetText(1) + "Minishorts" + t.GetText(4) + "Police" + t.GetText(25);
                levelTexts[3].text = t.GetText(2) + levelxp[level + 1].ToString() + t.GetText(3) + "Blue" + t.GetText(4) + "Clown" + t.GetText(25) + t.GetText(4) + t.GetText(15);
                break;
            case 28:
                tempCoins = 35000;
                levelTexts[0].text = t.GetText(1) + "Blue" + t.GetText(4) + "Clown" + t.GetText(25);
                levelTexts[3].text = t.GetText(2) + levelxp[level + 1].ToString() + "";
                levelBlocksOb.SetActive(true);
                levelTexts[1].text = t.GetText(15);
                break;
            case 29:
                tempCoins = 35000;
                levelTexts[0].gameObject.SetActive(false);
                levelTexts[3].text = t.GetText(2) + levelxp[level + 1].ToString() + t.GetText(3) + "Woodcutter" + t.GetText(4) + "Swim" + t.GetText(25) + t.GetText(4) + t.GetText(16);
                break;
            case 30:
                appReview.AskReview();
                tempCoins = 35000;
                levelTexts[0].text = t.GetText(1) + "Woodcutter" + t.GetText(4) + "Swim" + t.GetText(25);
                levelTexts[3].text = t.GetText(2) + levelxp[level + 1].ToString() + t.GetText(3) + "White Suit" + t.GetText(4) + "Pink" + t.GetText(25);
                levelBlocksOb.SetActive(true);
                levelTexts[1].text = t.GetText(16);
                break;
            case 31:
                tempCoins = 35000;
                levelTexts[0].text = t.GetText(1) + "White Suit" + t.GetText(4) + "Pink" + t.GetText(25);
                levelTexts[3].text = t.GetText(2) + levelxp[level + 1].ToString() + t.GetText(3) + t.GetText(17);
                break;
            case 32:
                tempCoins = 40000;
                levelTexts[0].gameObject.SetActive(false);
                levelTexts[3].text = t.GetText(2) + levelxp[level + 1].ToString() + t.GetText(24);
                levelBlocksOb.SetActive(true);
                levelTexts[1].text = t.GetText(17);
                break;
            case 33:
                tempCoins = 40000;
                levelTexts[0].gameObject.SetActive(false);
                levelTexts[3].text = t.GetText(2) + levelxp[level + 1].ToString() + t.GetText(24);
                break;
            case 34:
                tempCoins = 40000;
                levelTexts[0].gameObject.SetActive(false);
                levelTexts[3].text = t.GetText(2) + levelxp[level + 1].ToString() + t.GetText(24);
                break;
            case 35:
                tempCoins = 40000;
                levelTexts[0].gameObject.SetActive(false);
                levelTexts[3].text = t.GetText(2) + levelxp[level + 1].ToString() + t.GetText(3) + t.GetText(18);
                break;
            case 36:
                tempCoins = 40000;
                levelTexts[0].gameObject.SetActive(false);
                levelTexts[3].text = t.GetText(2) + levelxp[level + 1].ToString() + t.GetText(24);
                levelBlocksOb.SetActive(true);
                levelTexts[1].text = t.GetText(18);
                break;
            case 37:
                tempCoins = 40000;
                levelTexts[0].gameObject.SetActive(false);
                levelTexts[3].text = t.GetText(2) + levelxp[level + 1].ToString() + t.GetText(24);
                break;
            case 38:
                tempCoins = 40000;
                levelTexts[0].gameObject.SetActive(false);
                levelTexts[3].text = t.GetText(2) + levelxp[level + 1].ToString() + t.GetText(24);
                break;
            case 39:
                tempCoins = 40000;
                levelTexts[0].gameObject.SetActive(false);
                levelTexts[3].text = t.GetText(2) + levelxp[level + 1].ToString() + t.GetText(3) + t.GetText(19);
                break;
            case 40:
                tempCoins = 40000;
                levelTexts[0].gameObject.SetActive(false);
                levelTexts[3].text = t.GetText(2) + levelxp[level + 1].ToString() + t.GetText(24);
                levelBlocksOb.SetActive(true);
                levelTexts[1].text = t.GetText(19);
                break;
            case 41:
                tempCoins = 45000;
                levelTexts[0].gameObject.SetActive(false);
                levelTexts[3].text = t.GetText(2) + levelxp[level + 1].ToString() + t.GetText(24);
                break;
            case 42:
                tempCoins = 45000;
                levelTexts[0].gameObject.SetActive(false);
                levelTexts[3].text = t.GetText(2) + levelxp[level + 1].ToString() + t.GetText(24);
                break;
            case 43:
                tempCoins = 45000;
                levelTexts[0].gameObject.SetActive(false);
                levelTexts[3].text = t.GetText(2) + levelxp[level + 1].ToString() + t.GetText(3) + t.GetText(20);
                break;
            case 44:
                tempCoins = 45000;
                levelTexts[0].gameObject.SetActive(false);
                levelTexts[3].text = t.GetText(2) + levelxp[level + 1].ToString() + t.GetText(24);
                levelBlocksOb.SetActive(true);
                levelTexts[1].text = t.GetText(20);
                break;
            case 45:
                tempCoins = 45000;
                levelTexts[0].gameObject.SetActive(false);
                levelTexts[3].text = t.GetText(2) + levelxp[level + 1].ToString() + t.GetText(24);
                break;
            case 46:
                tempCoins = 45000;
                levelTexts[0].gameObject.SetActive(false);
                levelTexts[3].text = t.GetText(2) + levelxp[level + 1].ToString() + t.GetText(24);
                break;
            case 47:
                tempCoins = 45000;
                levelTexts[0].gameObject.SetActive(false);
                levelTexts[3].text = t.GetText(2) + levelxp[level + 1].ToString() + t.GetText(3) + t.GetText(21);
                break;
            case 48:
                tempCoins = 45000;
                levelTexts[0].gameObject.SetActive(false);
                levelTexts[3].text = t.GetText(2) + levelxp[level + 1].ToString() + t.GetText(24);
                levelBlocksOb.SetActive(true);
                levelTexts[1].text = t.GetText(21);
                break;
            case 49:
                tempCoins = 45000;
                levelTexts[0].gameObject.SetActive(false);
                levelTexts[3].text = t.GetText(2) + levelxp[level + 1].ToString() + t.GetText(24);
                break;
            case 50:
                tempCoins = 45000;
                levelTexts[0].gameObject.SetActive(false);
                levelTexts[3].text = t.GetText(2) + levelxp[level + 1].ToString() + t.GetText(24);
                break;
            case 51:
                tempCoins = 45000;
                levelTexts[0].gameObject.SetActive(false);
                levelTexts[3].text = t.GetText(2) + levelxp[level + 1].ToString() + t.GetText(3) + t.GetText(22);
                break;
            case 52:
                appReview.AskReview();
                tempCoins = 50000;
                levelTexts[0].gameObject.SetActive(false);
                levelTexts[3].text = t.GetText(2) + levelxp[level + 1].ToString() + t.GetText(24);
                levelBlocksOb.SetActive(true);
                levelTexts[1].text = t.GetText(22);
                break;
            case 53:
                tempCoins = 50000;
                levelTexts[0].gameObject.SetActive(false);
                levelTexts[3].text = t.GetText(2) + levelxp[level + 1].ToString() + t.GetText(24);
                break;
            case 54:
                tempCoins = 50000;
                levelTexts[0].gameObject.SetActive(false);
                levelTexts[3].text = t.GetText(2) + levelxp[level + 1].ToString() + t.GetText(24);
                break;
            case 55:
                tempCoins = 50000;
                levelTexts[0].gameObject.SetActive(false);
                levelTexts[3].text = t.GetText(2) + levelxp[level + 1].ToString() + t.GetText(3) + t.GetText(23);
                break;
            case 56:
                tempCoins = 50000;
                levelTexts[0].gameObject.SetActive(false);
                levelTexts[3].text = t.GetText(2) + levelxp[level + 1].ToString() + t.GetText(24);
                levelBlocksOb.SetActive(true);
                levelTexts[1].text = t.GetText(23);
                break;
            case 57:
                tempCoins = 50000;
                levelTexts[0].gameObject.SetActive(false);
                levelTexts[3].text = t.GetText(2) + levelxp[level + 1].ToString() + t.GetText(24);
                break;
            case 58:
                tempCoins = 50000;
                levelTexts[0].gameObject.SetActive(false);
                levelTexts[3].text = t.GetText(2) + levelxp[level + 1].ToString() + t.GetText(24);
                break;
            case 59:
                tempCoins = 50000;
                levelTexts[0].gameObject.SetActive(false);
                levelTexts[3].text = t.GetText(2) + levelxp[level + 1].ToString() + t.GetText(24);
                break;
            case 60:
                tempCoins = 50000;
                levelTexts[0].gameObject.SetActive(false);
                levelTexts[3].text = t.GetText(2) + levelxp[level + 1].ToString() + t.GetText(24);
                break;
            case 61:
                tempCoins = 50000;
                levelTexts[0].gameObject.SetActive(false);
                levelTexts[3].text = t.GetText(2) + levelxp[level + 1].ToString() + t.GetText(24);
                break;
            case 62:
                tempCoins = 50000;
                levelTexts[0].gameObject.SetActive(false);
                levelTexts[3].text = t.GetText(2) + levelxp[level + 1].ToString() + t.GetText(24);
                break;
            case 63:
                tempCoins = 50000;
                levelTexts[0].gameObject.SetActive(false);
                levelTexts[3].text = t.GetText(2) + levelxp[level + 1].ToString() + t.GetText(24);
                break;
            case 64:
                tempCoins = 50000;
                levelTexts[0].gameObject.SetActive(false);
                levelTexts[3].gameObject.SetActive(false);
                break;

        }
        coins += tempCoins;
        SetCoins();
        levelTexts[2].text = tempCoins.ToString("n0");
    }
    public void SetStars(int x)
    {
        starsCount += x;
        if (starsCount < 0)
            starsCount = 0;
        int tempCount = starsCount;

        if (tempCount <= starsXp[0])
        {
            stars = (tempCount * 1f) / (starsXp[0] * 1f);
        }
        if (tempCount > starsXp[0] && tempCount <= starsXp[1])
        {
            stars = 1 + (tempCount * 1f - starsXp[0]) / (starsXp[1] * 1f - starsXp[0]);
        }
        if (tempCount > starsXp[1] && tempCount <= starsXp[2])
        {
            stars = 2 + (tempCount * 1f - starsXp[1]) / (starsXp[2] * 1f - starsXp[1]);
        }
        if (tempCount > starsXp[2] && tempCount <= starsXp[3])
        {
            if (stars == 2)
            {
                pg.Achivements(1, 0);
            }
            stars = 3 + (tempCount * 1f - starsXp[2]) / (starsXp[3] * 1f - starsXp[2]);
        }
        if (tempCount > starsXp[3] && tempCount <= starsXp[4])
        {
            if (stars == 3)
            {
                pg.Achivements(1, 1);
            }
            stars = 4 + (tempCount * 1f - starsXp[3]) / (starsXp[4] * 1f - starsXp[3]);
        }
        if (tempCount > starsXp[4])
        {
            if (stars == 4)
            {
                pg.Achivements(1, 2);
            }
            stars = 5;
        }

        starsBar.fillAmount = stars / 5;
        starsText.text = stars.ToString("F2");
    }
    public void SetBlocks(int x)
    {
        blocks += x;
        blocksUsed.text = blocks + "/" + blocksPermited + t.GetText(0);
        limitL = -3 - blocksId * 0.35f + ((blocksId > 7) ? (blocksId - 7) * 0.2f : 0);
        limitR = 7f + blocksId * 0.5f + (blocksId * blocksId * 0.03f) - ((blocksId > 7) ? (blocksId - 7) * 0.5f : 0);
        limitY = 20 + blocksId * 1.5f + (blocksId * blocksId * 0.03f);
        camC.sizeBoundMax = 10 + blocksId * .25f;
        //Debug.Log(limitL + "   " + limitR + "   " + limitY);
    }
    public void PurchaseBlocks(int id, int cost)
    {
        if (tutoOn && id == 1 && tuto.current == 18)
        {
            tuto.PurchaseExpand();
        }
        blocksId = id;
        blocksPermited = blocksPer[id];
        Purchase(cost);
        SetBlocks(0);
        CloseShop();
    }
    public static int[] blocksPer =
    {
        12,24,28,34,42,
        52,64,76,92,116,
        128,144,164,176,194,
        210,224,256,272,312
    };
    public static int[] levelxp =
    {
        //Prestige 16,24,32,40,48,56,64

        0, 5, 14, 80,
        250, 550, 900, 1400,
        3000, 5000, 8000, 10000,
        13000, 17500, 24000, 30000,
        //16
        38000, 55000/*1*/, 75000, 100000,
        125000, 150000, 175000, 200000,
        230000, 300000/*2*/, 380000, 460000,
        520000, 600000, 680000, 760000,
        //32
        840000, 1000000/*3*/, 1150000, 1300000,
        1500000, 1650000, 1800000, 2000000,
        2200000, 2500000/*4*/, 2850000, 3200000,
        3750000, 4400000, 5200000, 6000000,
        //48
        6800000, 8000000/*5*/, 9000000, 10000000,
        12000000, 15000000, 17500000, 20000000,
        22500000, 27500000/*6*/, 35000000, 42500000,
        50000000, 60000000, 70000000, 80000000, 100000000/*7*/, 2000000000

    };
    public static int[] starsXp =
    {
        650,
        750,
        1600,
        8000,
        30000,

    };
    public void SetCustContent(int x)
    {
        for (int i = 0; i < custBtn.Length; i++)
        {
            if (x != i)
            {
                custBtn[i].color = new Color(0.64f, 0.95f, 0.97f);
                custCont[i].SetActive(false);
            }
        }
        gift.customizeGiftDots[x].SetActive(false);
        custBtn[x].color = new Color(1f, 0.6f, 0.6f);
        custCont[x].SetActive(true);
        customPanel.content = custCont[x].GetComponent<RectTransform>();
    }
    public void OpenCustomization()
    {
        if (CheckUI())
            return;
        if (coins >= 5000)
        {
            editTcost.color = new Color(1, 1, .38f);
            editUcost.color = new Color(1, 1, .38f);
        }
        else
        {
            editTcost.color = new Color(0.8f, 0.8f, 0.8f);
            editUcost.color = new Color(0.8f, 0.8f, 0.8f);
        }
        SC.INS.PlaySound(0, 13, 0);
        customizePanel.SetActive(true);
        gift.giftsDots[4].SetActive(false);
    }
    public void CloseCustomization()
    {
        Invoke("SetCustPanelOut", .25f);
        SC.INS.PlaySound(0, 13, 0);
        customizePanel.GetComponentInChildren<InOutAnim>().OutAnim();
        if (customized)
        {
            customized = false;
            SaveUserData();
        }
    }
    void SetCustPanelOut()
    {
        customTitle.text = hotelTitle;
        customUser.text = username;
        customizePanel.SetActive(false);
    }
    public void SetHotel(Dictionary<string, object> data)
    {
        RestartLists();
        if (data != null && data.ContainsKey("title"))
        {
            idiom = PlayerPrefs.GetInt("Idiom");
            coins = Convert.ToInt32(data["coins"]);
            gems = Convert.ToInt32(data["gems"]);
            starsCount = Convert.ToInt32(data["stars"]);
            SetStars(0);
            xp = Convert.ToInt32(data["xp"]);
            level = Convert.ToInt32(data["level"]);
            AddXp(0);
            blocksId = Convert.ToInt32(data["blocksId"]);
            blocksPermited = blocksPer[blocksId];
            SetBlocks(0);
            title.text = data["title"].ToString();
            hotelTitle = title.text;
            customTitle.text = hotelTitle;
            username = data["name"].ToString();
            customUser.text = username;

            if (data.ContainsKey("prestige"))
            {
                prestige = Convert.ToInt32(data["prestige"]);
                p.SetPrestige(prestige);
                if (prestige > 0)
                {
                    if (data.ContainsKey("parkinglevels"))
                        p.SetParking(data["parkinglevels"] as List<object>);
                    else
                        p.SetParking(null);

                    //Set Harder Stars
                    for (int i = 0; i < starsXp.Length; i++)
                        starsXp[i] += (int)(starsXp[i] * (prestige * .4f));

                    SetStars(0);
                }
            }
            TransformListToRoom(data["rooms"] as List<object>);
            logTime = Fire.INS.ParseTime(data["time"]);
            if (data.ContainsKey("iap"))
            {
                List<object> temp = data["iap"] as List<object>;
                for (int i = 0; i < temp.Count; i++)
                    iap[i] = (bool)temp[i];

                if (iap[0])
                {
                    x2Image.SetActive(true);
                    boostFill.fillAmount = 1;
                    boostTimeText.text = "∞";
                }
                if (iap[2])
                {
                    shiftX2I.SetActive(true);
                }
            }
            if (data.ContainsKey("boostTime"))
            {
                boostTime = Convert.ToInt32(data["boostTime"]);
            }
            if (data.ContainsKey("doubleShift"))
            {
                doubleShift = (bool)data["doubleShift"];
            }
            shiftStart = Fire.INS.ParseTime(data["shiftStart"]);
            Fire.INS.SetShiftStart(data["shiftStart"]);
            shiftTime = Convert.ToInt32(data["shift"]);
            shiftType = Convert.ToInt32(data["shiftType"]);
            backId = Convert.ToInt32(data["backId"]);
            backController.SetBack(backId);
            SetShift();
            SetCoinsOut();
            SetCostumersStart();
            dm.tutoRewards = (bool)data["tutoRewards"];
            if (dm.tutoRewards)
            {
                dm.SetTutoRewardsList(data["tutoRewardsL"] as List<object>);
            }
            if (data.ContainsKey("outsides"))
            {
                TransformListToOutside(data["outsides"] as List<object>);
            }
            if (data.ContainsKey("itemsList"))
            {
                List<object> temp = data["itemsList"] as List<object>;
                for (int i = 0; i < temp.Count; i++)
                    backController.itemsList.Add(Convert.ToInt32(temp[i]));
            }
            if (data.ContainsKey("backsUnlocked"))
            {
                List<object> temp = data["backsUnlocked"] as List<object>;
                for (int i = 0; i < temp.Count; i++)
                    backController.backsUnlocked[i] = Convert.ToBoolean(temp[i]);
            }
            if (data.ContainsKey("gifts"))
            {
                gift.TransformListToGift(data["gifts"] as List<object>);
            }
            if (data.ContainsKey("lastFriendVisit"))
            {
                List<object> tempList = data["lastFriendVisit"] as List<object>;
                for (int i = 0; i < tempList.Count; i++)
                {
                    lastFriendVisit.Add(Fire.INS.ParseTime(tempList[i]));
                }
            }
            if (data.ContainsKey("dailyMisions") && data.ContainsKey("dailyTime") && data.ContainsKey("currentDay"))
            {
                dm.Load(Fire.INS.ParseTime(data["dailyTime"]), data["dailyMisions"] as List<object>, Convert.ToInt32(data["currentDay"]));
            }
            else
            {
                //Debug.Log("NoContains Daily");
                dm.Load(DateTime.UtcNow, null, 0);
            }
            if (data.ContainsKey("codes"))
            {
                List<object> temp = data["plusVisit"] as List<object>;
                for (int i = 0; i < temp.Count; i++)
                    plusVisit[i] = Convert.ToBoolean(temp[i]);
            }
            if (data.ContainsKey("codes"))
            {
                List<object> temp = data["codes"] as List<object>;
                for (int i = 0; i < temp.Count; i++)
                    codes[i] = Convert.ToBoolean(temp[i]);
            }
            if (data.ContainsKey("invitesList"))
            {
                dm.SetFacebookList(data["invitesList"] as List<object>);
            }
            if (data.ContainsKey("lastFShare"))
            {
                lastFShare = Fire.INS.ParseTime(data["lastFShare"]);
                if ((DateTime.UtcNow - lastFShare).TotalDays >= 3)
                {
                    shareBtn.SetActive(true);
                    shareBtn.transform.GetChild(0).GetComponent<Animator>().SetTrigger("In");
                    lastFShare = DateTime.UtcNow.AddHours(-64);

                }
            }
            if (level > 3)
            {
                invitesBtn.SetActive(true);
                if (level < 5)
                    invitesBtn.transform.GetChild(0).GetComponent<Animator>().SetTrigger("In");
            }

        }
        else
        {
            switch (Application.systemLanguage)
            {
                case SystemLanguage.English:
                    idiom = 0;
                    break;
                case SystemLanguage.Spanish:
                    idiom = 1;
                    break;
            }
            PlayerPrefs.SetInt("Idiom", idiom);

            coins = 30000;
            gems = 100;
            starsCount = 700;
            tutoOn = true;
            tuto.StartTutorial();
            //changeHotelPanel.SetActive(true);
            string userN = Fire.INS.GetCurrentUser().DisplayName;
            usernameInput.text = userN;
            string tempHotelTitle = "";
            if (userN.Length > 7)
            {
                tempHotelTitle = userN.Substring(0, 7) + "'s hotel";
            }
            else
            {
                tempHotelTitle = userN + "'s hotel";
            }
            hotelTitleInput.text = tempHotelTitle;
            GameObject temp = Instantiate(roomsObj[23], Vector3.zero, transform.rotation);
            List<Character> tempCList = temp.GetComponentInChildren<Room>().RecepcionCreate();
            RoomC tempRoom = new RoomC(23, 1, 0, 0, 0, 0, new List<float[]>(), tempCList);
            temp.GetComponentInChildren<Room>().Create(tempRoom);
            xp = 0;
            work = false;
            shiftTimer.text = "00:00:00";
            shiftAnim.SetTrigger("End");
            roomControllers[0].SetWork(work, false);
            SetStars(0);
            AddXp(0);
            blocksId = 0;
            SetBlocks(0);
            backId = 0;
            backController.backsUnlocked[0] = true;
            backController.SetBack(backId);
            lastFShare = DateTime.UtcNow;
            SetPlayer();
            noti = false;

        }
        coinsSave = coins;
        coinsText.text = coins.ToString("n0");
        gemsText.text = gems.ToString("n0");
        levelText.text = level.ToString();
        levelTCus.text = level.ToString();
        canvas[1].SetActive(true);
        Invoke("LoadingOff", 1);
        loadingPanel.GetComponentInChildren<Animator>().SetTrigger("Finish");
    }
    public void Prestige()
    {
        prestige++;
        if (prestige == 1)
        {
            pg.Achivements(7, 0);
        }
        else if (prestige == 2)
        {
            pg.Achivements(7, 1);
        }
        else if (prestige == 5)
        {
            pg.Achivements(7, 2);
        }
        coins = p.coinBonus;
        gems += p.gems[prestige - 1];
        starsCount = 700 + (int)(700 * prestige * .4f);
        Destroy(roomControllers[0].gameObject);
        rooms = new List<RoomC>();
        GameObject temp = Instantiate(roomsObj[23], Vector3.zero, transform.rotation);
        List<Character> tempCList = temp.GetComponentInChildren<Room>().RecepcionCreate();
        RoomC tempRoom = new RoomC(23, 1, 0, 0, 0, 0, new List<float[]>(), tempCList);
        temp.GetComponentInChildren<Room>().Create(tempRoom);
        roomControllers[0].SetWork(work, false);
        backId = 0;
        for (int i = 1; i < backController.backsUnlocked.Length; i++)
            backController.backsUnlocked[i] = false;
        backController.itemsList = new List<int>();
        outsideObjects = new List<Outside>();
        shiftTime = 0;
        shiftStart = DateTime.UtcNow.AddDays(-30);
        Fire.INS.prestige = true;
        SaveFromBtn();
        SceneManager.LoadScene("SampleScene");
    }
    void LoadingOff()
    {
        loadingPanel.SetActive(false);
        Fire.INS.SignInSocial();
    }
    void SetCostumersStart()
    {
        if (work)
        {
            float timeCheck = 1;
            if (secondsDif < 100)
            {
                timeCheck = secondsDif / 100f;
            }
            float starsCheck = stars / 5f;
            float percent = timeCheck * .4f + starsCheck * .2f + UnityEngine.Random.Range(0.1f, 0.4f);
            if (percent > 1)
                percent = 1;
            int quant = (int)(slotsID.Count * percent);
            //Debug.Log("porciento: " + percent+ "  Cantidad:  " + quant);
            for (int i = 0; i < quant; i++)
            {
                Costumer temp = Instantiate(costumer, costumersArrange.transform).GetComponentInChildren<Costumer>();
                temp.SetFromStart(secondsDif, -5, 14);
            }
            quant = (int)(slotsID.Count * percent * UnityEngine.Random.Range(0.1f, 0.4f));
            //Debug.Log(quant);
            for (int i = 0; i < quant; i++)
            {
                Costumer temp = Instantiate(costumer, costumersArrange.transform).GetComponentInChildren<Costumer>();
                temp.SetMaintenance();
            }
        }
        else
        {
            if (shiftTimeLog > 0)
            {
                DateTime tempDate = logTime;
                tempDate.AddSeconds(shiftTimeLog);
                int tempTime = (int)((DateTime.UtcNow - tempDate).TotalSeconds);
                //Debug.Log(tempTime);
                if (tempTime < 100)
                {
                    float timeCheck = (100 - tempTime) / 100f;

                    float starsCheck = stars / 5f;
                    float percent = timeCheck * .5f + starsCheck * .2f + UnityEngine.Random.Range(-0.1f, 0.2f);
                    if (percent > 1)
                        percent = 1;
                    int quant = (int)(slotsID.Count * percent);
                    Debug.Log("porciento: " + percent + "  Cantidad:  " + quant);
                    for (int i = 0; i < quant; i++)
                    {
                        Costumer temp = Instantiate(costumer, costumersArrange.transform).GetComponentInChildren<Costumer>();
                        temp.SetFromStart(secondsDif, -5, 14);
                    }
                }
            }
        }

        for (int i = 0; i < slotsID.Count / 4 + 1; i++)
        {
            Costumer temp = Instantiate(costumer, costumersArrange.transform).GetComponentInChildren<Costumer>();
            temp.CreateRandom(limitL, limitR);
        }
    }
    int currentAddedCoins;
    void SetCoinsOut()
    {
        if (secondsDif < 2)
            return;
        float extraFixRooms = ((janitors.Count < 3) ? janitors.Count : 3 + ((plumbers.Count < 3) ? plumbers.Count : 3) + ((electicists.Count < 3) ? electicists.Count : 3)
            + ((officinist.Count < 3) ? officinist.Count : 3) + ((keyBuilder.Count < 3) ? keyBuilder.Count : 3)) * .006f;
        float starsCheck = stars / 5f;
        float percent = .35f + starsCheck * .3f + UnityEngine.Random.Range(-.1f, .2f) + extraFixRooms;
        if (Fire.INS.offPer > 0)
        {
            //Debug.Log("Off percentage: " + Fire.INS.offPer);
            percent += (Fire.INS.offPer * .3f);
        }
        int addCoins = (int)(coinsAverage * secondsDif * percent);
        int addXp = addCoins / 5 - 5;
        if (addXp < 0) addXp = 0;
        SC.INS.PlaySound(0, 13, 0);
        if (iap[0])
        {
            addCoins *= 2;
        }
        else
        {
            if (boostTime > 0)
            {
                if (boostTime - secondsDif < 0)
                {
                    addCoins += (int)(coinsAverage * boostTime * percent);
                    boostTime = 0;
                }
                else
                {
                    boostTime -= secondsDif;
                    addCoins *= 2;
                }
            }
            else
            {
                boostTimeText.text = "0:00";
                boostFill.fillAmount = 0;
            }
        }
        if (iap[1])
        {
            addCoins += (int)(coinsAverage * secondsDif * percent * .7f);
        }
        if (prestige > 0)
        {
            addCoins += (int)(parkingCoinsAverage * percent * secondsDif);
        }
        coinOut.text = addCoins.ToString();
        whileYouAway.SetActive(true);
        AddCoins(addCoins);
        AddXp(addXp);
        currentAddedCoins = addCoins;
    }

    public void DoubleShift()
    {
        Debug.Log("Double shift");
        doubleShift = true;
        shift = SwitchShift(shiftType);
        shiftTime = (int)shift;
        rdb.SetShiftNotification(shiftTime);
        ad.NormalAnim();
    }
    public void ShowVideo(int x)
    {
        if (x == 4 && iap[0] || x == 2 && iap[2])
        {
            errorM.Error(12);
            return;
        }
        lastAd = DateTime.UtcNow;
        if (x == 2)
        {
            if (IsInvoking("SetDoubleOff"))
                CancelInvoke("SetDoubleOff");
            doubleShiftP.SetActive(false);
            ShiftPanelOff();
        }
        dm.AddTask(25, 1);
        if (iap[4])
            Fire.INS.Reward(x);
        else
            Fire.INS.ShowVideoReward(x);
    }

    public void DoubleCoinsAway()
    {
        ad.CoinAnim();
        AddCoins(currentAddedCoins);
        currentAddedCoins = 0;
        whileYouAway.SetActive(false);
    }
    public void SetLastAd()
    {
        lastAd = DateTime.UtcNow;
    }
    public void AdBooster()
    {
        boostTime += 600;
        if (boostTime > 7200)
            boostTime = 7200;
    }
    void TransformListToRoom(List<object> list)
    {
        if (list == null || list.Count == 0)
        {
            GameObject temp = Instantiate(roomsObj[23], Vector3.zero, transform.rotation);
            List<Character> tempCList = temp.GetComponentInChildren<Room>().RecepcionCreate();
            RoomC tempRoom = new RoomC(23, 1, 0, 0, 0, 0, new List<float[]>(), tempCList);
            temp.GetComponentInChildren<Room>().Create(tempRoom);
            roomControllers[0].SetWork(work, false);
            return;
        }
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
                GameObject temp = Instantiate(roomsObj[tempRoom.id], Vector3.zero, transform.rotation);
                temp.GetComponentInChildren<Room>().Create(tempRoom);
            }
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
    public bool ChangeHotelName()
    {
        if (hotelTitleInput.text.Length < 6)
        {
            warningTH.text = t.GetText(106);
            warningTH.color = new Color(1, 0.5f, 0.5f);
            return false;
        }
        if (usernameInput.text.Length < 6)
        {
            warningTU.text = t.GetText(106);
            warningTU.color = new Color(1, 0.5f, 0.5f);
            warningTH.color = new Color(0.83f, 0.76f, 0.76f);
            return false;
        }
        hotelTitle = hotelTitleInput.text;
        title.text = hotelTitle;
        changeHotelPanel.SetActive(false);
        username = usernameInput.text;

        player.name = username;
        customTitle.text = hotelTitle;
        customUser.text = username;
        return true;
    }
    void Save()
    {
        if (rooms.Count < 1)
            return;
        Debug.Log("Save");
        Dictionary<string, object> data = new Dictionary<string, object>
        {
            { "title", hotelTitle},
            { "name", username},
            { "stars", starsCount },
            { "coins", coins },
            { "gems", gems },
            {"xp", xp},
            {"level", level},
            {"prestige", prestige},
            {"blocksId", blocksId},
            {"backId", backId},
            { "rooms", TransformRoomToList(rooms)},
            { "time", Fire.INS.GetTime()},
            { "shift", shiftTime},
            { "shiftStart", Fire.INS.ShiftStart()},
            { "shiftType", shiftType},
            { "itemsList", backController.itemsList},
            { "backsUnlocked", backController.backsUnlocked},
            { "outsides",TransformOutsideToList(outsideObjects)},
            {"gifts", gift.TransformGiftToList() },
            {"lastFriendVisit", lastFriendVisit},
            {"dailyMisions", dm.TransformMisionsToList()},
            {"dailyTime", dm.lastday},
            {"currentDay", dm.currentDay},
            {"tutoRewards", dm.tutoRewards},
            {"tutoRewardsL", dm.tutoRewardsClaim},
            {"boostTime",boostTime },
            {"doubleShift",doubleShift },
            {"iap", iap},
            {"plusVisit", plusVisit},
            {"codes", codes},
            {"invitesList", dm.invitedIds},
            {"lastFShare", lastFShare},

        };
        if (prestige > 0)
        {
            if (p.par)
            {
                data.Add("parkinglevels", p.par.TransformLevelsToList());
            }
        }
        PlayerPrefs.SetInt("roomFix", roomFix);
        PlayerPrefs.SetInt("giftCount", giftCount);
        PlayerPrefs.SetInt("dailyMCount", dailyMCount);
        if (data["rooms"] == null)
        {
            Debug.LogError("SAVE ERROR");
            return;
        }
        Fire.INS.SaveDataFirestore(data);
        coinsSave = coins;
    }
    public void SaveUserData()
    {
        //Debug.Log("-------PP: " + FRC.INS.pp);
        //Debug.Log("SAVED USER DATA!!!");
        if (characterAsMap == null)
        {
            SetPlayer();
        }
        Dictionary<string, object> data = new Dictionary<string, object>
        {
            { "id", Fire.INS.GetCurrentUser().UserId},
            { "title", hotelTitle},
            { "name", username},
            { "stars", stars.ToString("F2") },
            {"character", characterAsMap},
            {"purchased", GetPurchased()},
            {"friendList", FRC.INS.friendList},
            {"gifts", FRC.INS.giftList},
            {"jobApp", ""},
            {"faceId", FRC.INS.facebook.faceId},
            {"facebookList", FRC.INS.facebook.TransformFriendsToList()},
            {"noti", noti }
        };
        Fire.INS.MergeDataFirestore(data);
        if (Fire.INS.firstTime)
        {
            Debug.Log("First time listen");
            FRC.INS.Listen();
        }
        customized = false;
    }
    Dictionary<string, object> GetPurchased()
    {
        Dictionary<string, object> tempDic = new Dictionary<string, object>
        {
             { "skinColor", customPurchased[0]},
             { "outfit", customPurchased[1]},
             { "mouthId", customPurchased[2]},
             { "extraId", customPurchased[3]},
             { "extraColor", customPurchased[4]},
             { "eyesId", customPurchased[5]},
             { "eyeColor", customPurchased[6]},
             { "glassId", customPurchased[7]},
             { "glassColorId", customPurchased[8]},
             { "glassColor", customPurchased[9]},
             { "hairId", customPurchased[10]},
             { "hairColor", customPurchased[11]},
        };

        return tempDic;
    }
    public void SetPurchased(Dictionary<string, object> tempDic)
    {
        SetPurchasedList(0, tempDic["skinColor"] as List<object>);
        SetPurchasedList(1, tempDic["outfit"] as List<object>);
        SetPurchasedList(2, tempDic["mouthId"] as List<object>);
        SetPurchasedList(3, tempDic["extraId"] as List<object>);
        SetPurchasedList(4, tempDic["extraColor"] as List<object>);
        SetPurchasedList(5, tempDic["eyesId"] as List<object>);
        SetPurchasedList(6, tempDic["eyeColor"] as List<object>);
        SetPurchasedList(7, tempDic["glassId"] as List<object>);
        SetPurchasedList(8, tempDic["glassColorId"] as List<object>);
        SetPurchasedList(9, tempDic["glassColor"] as List<object>);
        SetPurchasedList(10, tempDic["hairId"] as List<object>);
        SetPurchasedList(11, tempDic["hairColor"] as List<object>);
    }
    void SetPurchasedList(int id, List<object> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            if (i == 0)
                customPurchased[id][i] = true;
            else
                customPurchased[id][i] = Convert.ToBoolean(list[i]);
        }
    }

    void SetPlayer()
    {
        player = new Character(100, 0, 0, "username", 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, false);
        for (int i = 0; i < customPurchased.Count; i++)
            customPurchased[i][0] = true;

        playerCharacter.SetCharacter(player);
    }
    public Dictionary<string, object> characterAsMap
    {
        get
        {
            if (player == null)
            {
                Debug.Log("EERRRR");
            }

            Dictionary<string, object> tempMap = new Dictionary<string, object>
            {
                 { "id", player.id},
                 { "outfitId", player.outfitId},
                 { "hairId", player.hairId},
                 { "name", player.name},
                 { "hairColor", player.hairColor},
                 { "eyeColor", player.eyeColor},
                 { "glassColor", player.glassColor},
                 { "skinColor", player.skinColor},
                 { "extraId", player.extraId},
                 { "extraColor", player.extraColor},
                 { "glassId", player.glassId},
                 { "glassColorId", player.glassColorId},
                 { "mouthId", player.mouthId},
                 { "eyesId", player.eyesId},
                 { "isFriend", player.isFriend},
            };

            return tempMap;
        }

    }

    public void SetPlayer(Dictionary<string, object> tempDic)
    {
        player = new Character
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
    object TransformRoomToList(List<RoomC> roomsL)
    {
        for (int i = 0; i < deletedRooms.Count; i++)
        {
            rooms.Remove(deletedRooms[i]);
        }
        List<object> newList = new List<object>();
        for (int i = 0; i < roomsL.Count; i++)
        {
            if (rooms[i].id == 23)
            {
                Dictionary<string, object> temp = new Dictionary<string, object>
                {
                    { "id", rooms[i].id},
                    { "posX", roomsL[i].positionX },
                    { "posY", roomsL[i].positionY },
                    { "wallT", roomsL[i].wallType },
                    { "bedT", roomsL[i].bedType },
                    { "floorT", roomsL[i].floorType },
                    { "decorations", roomsL[i].decorationsAsMap },
                    { "staff", roomsL[i].characterAsMap},
                };
                newList.Add(temp);
            }
            else if (rooms[i].hasDecorations)
            {
                Dictionary<string, object> temp = new Dictionary<string, object>
                {
                    { "id", rooms[i].id},
                    { "posX", roomsL[i].positionX },
                    { "posY", roomsL[i].positionY },
                    { "wallT", roomsL[i].wallType },
                    { "bedT", roomsL[i].bedType },
                    { "floorT", roomsL[i].floorType },
                    { "decorations", roomsL[i].decorationsAsMap },
                };
                /*foreach (KeyValuePair<string, object> pair in temp)
                {
                    Debug.Log(String.Format("{0}: {1}", pair.Key, pair.Value));
                }*/
                newList.Add(temp);
            }
            else if (rooms[i].hasStaff)
            {
                Dictionary<string, object> temp = new Dictionary<string, object>
                {
                    { "id", rooms[i].id},
                    { "posX", roomsL[i].positionX },
                    { "posY", roomsL[i].positionY },
                    { "staff", roomsL[i].characterAsMap},
                };
                newList.Add(temp);
            }
            else
            {
                Dictionary<string, object> temp = new Dictionary<string, object>
                {
                    { "id", rooms[i].id},
                    { "posX", roomsL[i].positionX },
                    { "posY", roomsL[i].positionY },
                };
                newList.Add(temp);
            }


        }

        return newList.ToArray();
    }
    object TransformOutsideToList(List<Outside> outsideL)
    {
        for (int i = 0; i < deletedOutside.Count; i++)
        {
            outsideObjects.Remove(deletedOutside[i]);
        }
        List<object> newList = new List<object>();
        for (int i = 0; i < outsideL.Count; i++)
        {

            Dictionary<string, object> temp = new Dictionary<string, object>
                {
                    { "id", outsideL[i].id},
                    { "posX", outsideL[i].positionX },
                };
            newList.Add(temp);

        }
        return newList.ToArray();
    }
    public GameObject SetDecorationsToRoom(float[] values, GameObject parent, Room room, int number)
    {
        GameObject temp = Instantiate(decorationsObj[(int)values[1]], Vector3.zero, transform.rotation);
        temp.GetComponentInChildren<Decoration>().SetObject((int)values[0], values[2], values[3], parent, room, number);
        return temp;
    }
    public void PurchaseChangeText(int x)
    {
        if (coins >= 5000)
        {
            if (x == 0)
            {
                editTBtn.SetActive(false);
                okTBtn.SetActive(true);
                customTitle.interactable = true;
                customTitle.ActivateInputField();
            }
            else
            {
                editUBtn.SetActive(false);
                okUBtn.SetActive(true);
                customUser.interactable = true;
                customUser.ActivateInputField();
            }

        }
        else
        {
            errorM.Error(0);
        }
    }
    public void SetHotelTitle(int x)
    {
        if (x == 0)
        {
            if (customTitle.text.Length > 5 && coins >= 5000)
            {
                editTBtn.SetActive(true);
                okTBtn.SetActive(false);
                Purchase(5000);
                hotelTitle = customTitle.text;
                title.text = hotelTitle;
                customized = true;
            }
            else
            {
                customTitle.text = hotelTitle;
                if (coins >= 5000)
                    errorM.Error(0);
                else
                    errorM.Error(2);
            }
        }
        else
        {
            if (customUser.text.Length > 5 && coins >= 5000)
            {
                editUBtn.SetActive(true);
                okUBtn.SetActive(false);
                Purchase(5000);
                username = customUser.text;
                customized = true;
            }
            else
            {
                customUser.text = username;
                if (coins >= 5000)
                    errorM.Error(0);
                else
                    errorM.Error(2);
            }
        }

    }
    public void CheckRecepcion()
    {
        if (!work)
        {
            roomControllers[0].SetWork(work, false);
        }
    }
    private void OnApplicationPause(bool pause)
    {
        if (username.Length < 6 || tutoOn)
            return;
        if (pause)
        {
            if (customized)
            {
                customized = false;
                SaveUserData();
            }
            if ((DateTime.UtcNow - lastSave).TotalSeconds > 7)
            {
                lastSave = DateTime.UtcNow;
                Save();
            }
            else if (Mathf.Abs(coins - coinsSave) > 2500)
            {
                Debug.Log("Save from coins");
                lastSave = DateTime.UtcNow;
                Save();
            }
            pauseTime = DateTime.UtcNow;
        }
        else
        {
            if ((DateTime.UtcNow - pauseTime).TotalSeconds > 150 || shiftTime < 0)
            {
                ReloadScene();
            }
            else
            {
                if (boostTime > 0)
                {
                    boostTime -= (float)(DateTime.UtcNow - pauseTime).TotalSeconds;
                    if (boostTime < 0)
                    {
                        EndBoost();
                    }
                }
            }
        }
    }
    public void ReloadScene()
    {
        Fire.INS.offPer = (slotsID.Count / slots.Count);
        rooms = new List<RoomC>();
        SceneManager.LoadScene("SampleScene");
    }
    void EndBoost()
    {
        if (!coinBoost)
            return;
        boostTime = 0;
        coinBoost = false;
        if (iap[0])
        {
            x2Image.SetActive(true);
            boostFill.fillAmount = 1;
            boostTimeText.text = "∞";
        }
        else
        {
            x2Image.SetActive(false);
            boostFill.fillAmount = 0;
            boostTimeText.text = "0:00";
        }
    }
    public void SaveFromBtn()
    {
        if (username.Length < 6)
            return;
        //Debug.Log((DateTime.UtcNow - lastSave).TotalSeconds);
        if ((DateTime.UtcNow - lastSave).TotalSeconds > 1)
        {
            lastSave = DateTime.UtcNow;
            Save();
            SaveUserData();
        }
        else if (Mathf.Abs(coins - coinsSave) > 2500)
        {
            Debug.Log("Save from coins");
            lastSave = DateTime.UtcNow;
            Save();
        }

    }
    private void OnApplicationQuit()
    {
        if (username.Length < 6 || tutoOn)
            return;
        if ((DateTime.UtcNow - lastSave).TotalSeconds > 8)
        {
            lastSave = DateTime.UtcNow;
            Save();
        }
        if (customized)
            SaveUserData();
        else if (UnityEngine.Random.Range(0, 9) == 0)
            SaveUserData();
    }
    public void LogOut()
    {
        Fire.INS.LogOut();
    }

    public void SetX2Panel()
    {
        if (CheckUI())
            return;
        x2boostP.SetActive(true);
        SC.INS.PlaySound(0, 17, 0);
    }
    public void SetParkingCoinsAverage(float ave)
    {
        parkingCoinsAverage = ave;
        totalAverage = coinsAverage + parkingCoinsAverage;
    }


    public bool CheckUI()
    {
        if (phone.activeInHierarchy)
            return true;

        if (shopPanel.activeInHierarchy)
            return true;

        if (shiftPanel.activeInHierarchy)
            return true;

        if (customizePanel.activeInHierarchy)
            return true;

        if (x2boostP.activeInHierarchy)
            return true;

        if (prestige > 1)
        {
            if (p.parkingCan && p.parkingCan.activeInHierarchy)
                return true;
        }
        if (levelPanel.activeInHierarchy)
        {
            return true;
        }
        return false;
    }

    void RestartLists()
    {
        rooms = new List<RoomC>();
        deletedRooms = new List<RoomC>();
        for (int i = 0; i < roomControllers.Count; i++)
            Destroy(roomControllers[i].gameObject);
        roomControllers = new List<Room>();
        outsideObjects = new List<Outside>();
        deletedOutside = new List<Outside>();
        slots = new List<Slot>();
        lastFriendVisit = new List<DateTime>();
        staffFriendIDList = new List<int>();
    }

    public void ResetCustomPurchases()
    {
        customPurchased = new List<bool[]>();
        //Skin
        customPurchased.Add(new bool[8]);
        //Outfit
        customPurchased.Add(new bool[50]);
        //Mouth
        customPurchased.Add(new bool[8]);
        //Extra
        customPurchased.Add(new bool[24]);
        //Extra Color
        customPurchased.Add(new bool[16]);
        //Eyes
        customPurchased.Add(new bool[8]);
        //Eyes color
        customPurchased.Add(new bool[12]);
        //Glasses
        customPurchased.Add(new bool[4]);
        //Glasses color
        customPurchased.Add(new bool[8]);
        //Glass color
        customPurchased.Add(new bool[10]);
        //Hair
        customPurchased.Add(new bool[32]);
        //Hair color
        customPurchased.Add(new bool[16]);
    }
}


