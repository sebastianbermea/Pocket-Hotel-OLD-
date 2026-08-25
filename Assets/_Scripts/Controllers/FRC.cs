using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FRC : MonoBehaviour
{
    public static FRC INS { get; private set; }

    [HideInInspector]
    int searchCount, currentPort = 1;
    bool changesInImbox;

    //Friend requests notifications from other users
    public List<Dictionary<string, object>> requestList = new List<Dictionary<string, object>>();

    public List<Dictionary<string, object>> friendList = new List<Dictionary<string, object>>();
    List<Dictionary<string, object>> searchList = new List<Dictionary<string, object>>();

    public List<Dictionary<string, object>> giftList = new List<Dictionary<string, object>>();

    //User pending friend request to other users
    [HideInInspector]
    public List<string> waitRequestList = new List<string>();

    List<GameObject> notificationButtons = new List<GameObject>();
    List<GameObject> giftnotificationButtons = new List<GameObject>();
    public List<GameObject> friendButtons = new List<GameObject>();
    List<GameObject> searchButtons = new List<GameObject>();
    List<GameObject> tempSearchButtons = new List<GameObject>();

    public RectTransform[] ports;
    public Image[] pButtons;
    public ScrollRect scrollRect;
    public GameObject loadingObj, userButton, notificationPop, friendRequestButton, responseB, friendButton, responseSheet, cancelSheet, phone;
    public GameObject friendStaffBtn, whiteScreen;
    [SerializeField] Transform[] friendShopLists;
    bool searching, moreSearch;
    string currentT;
    [HideInInspector]
    public List<Character> friendsC = new List<Character>();
    [HideInInspector]
    public List<GameObject> friendsSataffBtns;
    [HideInInspector]
    public List<Staff> friendsStaff = new List<Staff>();
    [HideInInspector]
    public List<string> friendsIds = new List<string>();
    //Current response values
    int cNumber, friendAdded = 0;
    string cId;
    [HideInInspector]
    public string visitId;
    [HideInInspector]
    public int visitNumber;
    [HideInInspector]
    public string currentJobApp;
    public GameObject tutoFriend, tutobtn;
    public FaceB facebook;
    public JobReq jobReq;

    private void Awake()
    {
        if (INS == null)
        {
            INS = this;
        }
    }
    private void Start()
    {
        if (!Fire.INS.firstTime)
            Listen();
    }
    public void Listen()
    {
        //Debug.Log("Listen to data");
        Fire.INS.ListenToData();
    }
    public void EndEdit(string t)
    {
        currentT = t;
        GetFriends();
    }
    public void ScrollChanged(Vector2 v2)
    {
        if (currentPort == 2 && v2.y < -.3 && !searching && moreSearch)
        {
            searching = true;
            GetMoreList();
        }

    }
    public void ChangePort(int x)
    {
        if (currentPort == x)
            return;
        if (currentPort == 0)
        {
            CloseNotification();
        }
        ports[currentPort].gameObject.SetActive(false);
        pButtons[currentPort].color = new Color(0.77f, 0.77f, 0.77f, 1);
        currentPort = x;
        if (currentPort == 0)
        {
            OpenNotificationPanel();
        }
        ports[currentPort].gameObject.SetActive(true);
        pButtons[currentPort].color = new Color(0.5f, 0.8f, 1f, 1);
        scrollRect.content = ports[currentPort].GetComponent<RectTransform>();
    }
    public void OpenResponse(string uid, Vector2 pos, int number)
    {
        cId = uid;
        cNumber = number;
        GameObject temp = Instantiate(responseSheet, phone.transform);
        temp.transform.position = pos;
    }
    public void OpenCancel(string uid, Vector2 pos, UserB btn)
    {
        GameObject temp = Instantiate(cancelSheet, phone.transform);
        temp.transform.position = pos;
        temp.GetComponent<CancelSheet>().Set(uid, btn);
    }
    public void CancelRequest(string id, UserB btn)
    {
        if (waitRequestList.Contains(id))
        {
            waitRequestList.Remove(id);
            Dictionary<string, object> newDataToKeep = new Dictionary<string, object>
        {
             { "waitRequestList", waitRequestList },
        };
            Fire.INS.MergeDataFirestore(newDataToKeep);
        }

        Fire.INS.GetForeingDataCancel(id, btn);

    }
    public void CancelRequestFunc(Dictionary<string, object> foreignData, string id, UserB btn)
    {
        if (foreignData == null) return;
        List<object> friendRequest = (foreignData.ContainsKey("requestList")) ? foreignData["requestList"] as List<object> : new List<object>();

        int temp = friendRequest.Count;
        for (int i = 0; i < friendRequest.Count; i++)
        {
            Dictionary<string, object> tempDic = friendRequest[i] as Dictionary<string, object>;
            print(tempDic["id"].ToString());
            if (tempDic["id"].ToString() == Fire.INS.GetCurrentUser().UserId)
                temp = i;
        }
        if (temp > friendRequest.Count - 1)
            return;
        friendRequest.RemoveAt(temp);
        Dictionary<string, object> newDataToSend = new Dictionary<string, object>
        {
             { "requestList", friendRequest },
        };
        Fire.INS.MergeDataFirestore(newDataToSend, id);
        btn.CancelRequest();
    }
    public void InstantiateTutoFriend()
    {
        if (tutobtn == null)
            tutobtn = Instantiate(tutoFriend, ports[1]);
    }
    public void DestroyTutoBtn()
    {
        Destroy(tutobtn);
    }
    public void SetData(Dictionary<string, object> data)
    {
        GC.INS.ResetCustomPurchases();
        if (data != null)
        {
            if (data.ContainsKey("character"))
            {
                GC.INS.SetPlayer(data["character"] as Dictionary<string, object>);
            }
            if (data.ContainsKey("purchased"))
                GC.INS.SetPurchased(data["purchased"] as Dictionary<string, object>);

            if (data.ContainsKey("noti"))
            {
                GC.INS.noti = (bool)data["noti"];
                GC.INS.rdb.SetChannel();
            }
            if (data.ContainsKey("waitRequestList") && waitRequestList.Count == 0)
            {
                List<object> temp = data["waitRequestList"] as List<object>;
                for (int i = 0; i < temp.Count; i++)
                    waitRequestList.Add(temp[i] as string);
            }
            if (data.ContainsKey("jobApp"))
            {
                currentJobApp = data["jobApp"].ToString();
            }
            if (data.ContainsKey("requestList"))
            {
                List<object> temp = data["requestList"] as List<object>;
                //Checks only the new list items

                int previousCount = requestList.Count;
                if (previousCount > temp.Count)
                {
                    previousCount = 0;
                    requestList = new List<Dictionary<string, object>>();
                    foreach (GameObject btn in notificationButtons)
                    {
                        Destroy(btn);
                    }
                    notificationButtons = new List<GameObject>();
                }
                for (int i = previousCount; i < temp.Count; i++)
                {
                    Dictionary<string, object> tempDic = temp[i] as Dictionary<string, object>;

                    bool frepeated = false;
                    for (int j = 0; j < requestList.Count; j++)
                    {
                        if (requestList[j]["id"].ToString() == tempDic["id"].ToString())
                            frepeated = true;
                    }
                    if (frepeated)
                    {
                        Debug.Log("Repeated");
                        continue;
                    }

                    requestList.Add(tempDic);
                }

                bool newNotifi = false;

                for (int i = previousCount; i < requestList.Count; i++)
                {
                    //Checks if you have seen the notification or if it is rejected so there is no notification
                    if (!(bool)requestList[i]["seen"] && !(bool)requestList[i]["rejected"])
                        newNotifi = true;
                    if ((bool)requestList[i]["accepted"])
                    {

                        Debug.Log(requestList[i]["id"].ToString());
                        Fire.INS.GetForeignDataRequest(requestList[i]["id"].ToString(), i);
                        if (GC.INS && GC.INS.dm.tutoRewards)
                        {
                            GC.INS.dm.AddFriendFirst();
                        }
                        GC.INS.pg.Achivements(4, 0);
                        friendAdded++;
                    }
                    //If rejected then removes it from your list
                    else if ((bool)requestList[i]["rejected"])
                    {
                        for (int j = 0; j < waitRequestList.Count; j++)
                        {
                            if (requestList[i]["id"].ToString() == waitRequestList[j])
                                waitRequestList.RemoveAt(j);
                        }
                        requestList.RemoveAt(i);
                        Dictionary<string, object> newData = new Dictionary<string, object>
                    {
                            { "requestList", requestList },
                            { "friendList", friendList },
                            { "waitRequestList", waitRequestList },
                     };
                        Fire.INS.MergeDataFirestore(newData);
                    }
                    else
                    {
                        //If neither accepted or rejected then is a friend request for you
                        GameObject tempB = Instantiate(friendRequestButton, ports[0]);
                        notificationButtons.Add(tempB);
                        tempB.GetComponent<FriendRequestB>().Create(requestList[i]["id"].ToString(), i, requestList[i]["name"].ToString(), SetCharacter(requestList[i]["character"] as Dictionary<string, object>));

                    }

                }
                //Notification dot
                if (notificationPop)
                    notificationPop.SetActive(newNotifi);
                if (GC.INS)
                {
                    GC.INS.phoneNot.SetActive(newNotifi);
                }
            }
            //Add friends to friend list just when the game starts friendCount ==0 or if you add a friend at request
            if (data.ContainsKey("friendList") && friendList.Count == friendAdded)
            {
                // Debug.Log(friendList.Count);
                List<object> friendtemp = data["friendList"] as List<object>;
                for (int i = 0; i < friendtemp.Count; i++)
                {

                    Dictionary<string, object> newTemp = friendtemp[i] as Dictionary<string, object>;

                    bool frepeated = false;
                    for (int j = 0; j < friendList.Count; j++)
                    {
                        if (friendList[j]["id"].ToString() == newTemp["id"].ToString())
                            frepeated = true;
                    }
                    if (frepeated)
                    {
                        Debug.Log("Repeated");
                        continue;
                    }

                    if (UnityEngine.Random.Range(0, 3 + friendtemp.Count * 3) == 0)
                    {
                        string tempId = newTemp["id"].ToString();
                        Fire.INS.GetForeignDataActualize(tempId, i);
                        if (newTemp.ContainsKey("name"))
                        {
                            Debug.Log("ActualizeData: " + newTemp["name"].ToString());
                            if (GC.INS)
                                GC.INS.customized = true;
                        }
                        else
                        {
                            newTemp = friendtemp[i] as Dictionary<string, object>;
                        }

                    }
                    bool repeated = false;
                    for (int j = 0; j < friendList.Count; j++)
                    {
                        if (friendList[j]["id"].ToString() == newTemp["id"].ToString())
                            repeated = true;
                    }
                    if (repeated)
                    {
                        Debug.Log("Repeated");
                        continue;
                    }
                    if (i >= 24)
                        return;
                    friendsC.Add(SetCharacter(newTemp["character"] as Dictionary<string, object>));
                    if (!string.IsNullOrEmpty(currentJobApp) && currentJobApp == newTemp["id"].ToString())
                    {
                        jobReq.JobApp(newTemp["name"].ToString(), friendsC[friendsC.Count - 1]);
                        currentJobApp = "";
                    }
                    AddFriendWithPhoto(new Dictionary<string, object>
                    {
                        {"id" , newTemp["id"].ToString() },
                        {"title", newTemp["title"].ToString()},
                        {"name" , newTemp["name"].ToString() },
                        {"stars" , newTemp["stars"].ToString()},
                        {"character" , newTemp["character"]}
                    });

                    for (int j = 0; j < friendsStaff.Count; j++)
                    {
                        if (friendsC[i].id == friendsStaff[j].character.id)
                        {
                            Debug.Log("Match change.............");
                            friendsStaff[j].SetCharacter(friendsC[i]);
                        }
                    }
                }
                if (friendsSataffBtns != null)
                {
                    for (int i = 0; i < friendsSataffBtns.Count; i++)
                    {
                        Destroy(friendsSataffBtns[i]);
                    }
                }
                friendsSataffBtns = new List<GameObject>();
                for (int i = 0; i < friendsC.Count; i++)
                {
                    GameObject temp = Instantiate(friendStaffBtn, friendShopLists[i / 8]);
                    temp.GetComponent<StaffBtn>().Create(friendsC[i]);
                    friendsSataffBtns.Add(temp);
                }
                if (GC.INS)
                    GC.INS.friendsTCus.text = friendList.Count.ToString();

            }
            if (data.ContainsKey("gifts"))
            {
                bool added = false;
                if (data["gifts"] != null)
                {
                    List<object> giftTemp = data["gifts"] as List<object>;
                    for (int i = giftList.Count; i < giftTemp.Count; i++)
                    {
                        Dictionary<string, object> tempGift = giftTemp[i] as Dictionary<string, object>;
                        GameObject tempRes = Instantiate(responseB, ports[0]);
                        giftnotificationButtons.Add(tempRes);
                        tempRes.GetComponent<ResponseB>().CreateGift(i, tempGift["name"].ToString(), SetCharacter(tempGift["character"] as Dictionary<string, object>));

                        if (!(bool)tempGift["added"])
                        {
                            added = true;
                            tempGift["added"] = true;
                            GC.INS.gift.AddGift(tempGift, tempGift["name"].ToString());
                        }
                        giftList.Add(tempGift);
                    }
                    if (added)
                    {
                        if (GC.INS)
                        {
                            GC.INS.customized = true;
                        }
                        Invoke("CheckActive", .5f);

                    }
                }
                if (data.ContainsKey("facebookList"))
                {
                    facebook.SetList(data["facebookList"] as List<object>, data["faceId"].ToString());
                }
            }
        }

    }
    public void SetRequestFriend(Dictionary<string, object> foreignData, int i)
    {
        if (foreignData == null) return;
        //If accepted new friend is added and a respond norification
        Dictionary<string, object> newFriend = new Dictionary<string, object>
                        {
                             { "id", foreignData["id"].ToString()},
                             { "name", foreignData["name"].ToString()},
                             { "title", foreignData["title"].ToString()},
                             { "stars", foreignData["stars"].ToString()},
                             {"character" , foreignData["character"]}
                        };
        friendsC.Add(SetCharacter(foreignData["character"] as Dictionary<string, object>));
        AddFriendWithPhoto(newFriend);
        GameObject tempO = Instantiate(friendStaffBtn, friendShopLists[i / 8]);
        tempO.GetComponent<StaffBtn>().Create(friendsC[friendsC.Count - 1]);
        friendsSataffBtns.Add(tempO);


        GameObject tempRes = Instantiate(responseB, ports[0]);
        notificationButtons.Add(tempRes);
        tempRes.GetComponent<ResponseB>().Create(requestList[i]["id"].ToString(), i, requestList[i]["name"].ToString(), friendsC[friendsC.Count - 1]);
    }
    public void ActualizeFriendData(Dictionary<string, object> foreignData, int i)
    {
        if (foreignData == null) return;
        friendList[i] = new Dictionary<string, object>()
        {
            { "id", foreignData["id"].ToString()},
            { "name", foreignData["name"].ToString()},
            { "title", foreignData["title"].ToString()},
            { "stars", foreignData["stars"].ToString()},
            {"character" , foreignData["character"]}
        };
    }
    void CheckActive()
    {
        if (notificationPop)
        {
            notificationPop.SetActive(true);
        }
        if (GC.INS)
        {
            GC.INS.phoneNot.SetActive(true);

        }
    }
    public void PrintOut()
    {
        Debug.Log(friendsC[1].outfitId);
    }
    public Character SetCharacter(Dictionary<string, object> tempDic)
    {
        Character tempC = new Character
            (20 + friendsC.Count,
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
            true
            );
        return tempC;

    }

    #region Search


    public void GetFriends()
    {
        if (currentT.Length < 3)
            return;
        loadingObj.SetActive(true);
        for (int i = 0; i < searchButtons.Count; i++)
            Destroy(searchButtons[i]);
        searchButtons = new List<GameObject>();
        searchList = new List<Dictionary<string, object>>();
        searching = true;
        Fire.INS.GetFriendList(currentT);
    }
    public void SetFriends(List<Dictionary<string, object>> list)
    {
        searchList.AddRange(list);
        bool userRepeated = false;
        for (int i = 0; i < searchList.Count; i++)
        {
            if (searchList[i]["id"].ToString() == Fire.INS.GetCurrentUser().UserId)
            {
                searchList.RemoveAt(i);
                //Debug.Log("Removed: " + i);
                i--;
                userRepeated = true;
                continue;
            }


            GameObject temp = Instantiate(userButton, ports[2]);
            searchButtons.Add(temp);
            // temp.GetComponentInChildren<Text>().text = searchList[i]["username"].ToString();
            if (searchList[i].ContainsKey("requestList"))
                temp.GetComponent<UserB>().Create(searchList[i]["id"].ToString(), searchList[i]["requestList"] as List<object>, searchList[i]["name"].ToString(), searchList[i]["title"].ToString(),
                    searchList[i]["stars"].ToString(), (searchList[i]["friendList"] as List<object>).Count, SetCharacter(searchList[i]["character"] as Dictionary<string, object>), (bool)searchList[i]["noti"]);
            else
                temp.GetComponent<UserB>().Create(searchList[i]["id"].ToString(), null, searchList[i]["name"].ToString(), searchList[i]["title"].ToString(),
                    searchList[i]["stars"].ToString(), (searchList[i]["friendList"] as List<object>).Count, SetCharacter(searchList[i]["character"] as Dictionary<string, object>), (bool)searchList[i]["noti"]);

            tempSearchButtons.Add(temp);
            temp.SetActive(false);
            /* foreach (KeyValuePair<string, object> pair in searchList[i])
             {
                  Debug.Log(i +":  "+ pair.Key + " " + pair.Value);
             }*/
        }
        if (userRepeated && searchList.Count == 4)
        {
            Fire.INS.GetFriendOneMore(currentT);
            return;
        }
        //Debug.Log(searchList.Count);
        if (searchList.Count > 4)
        {
            moreSearch = true;
        }
        Invoke("EndSearch", .7f);
    }
    public void SetFriendOneMore(List<Dictionary<string, object>> list)
    {
        Debug.Log("UserRepeated");
        searchList.AddRange(list);
        GameObject temp = Instantiate(userButton, ports[2]);
        searchButtons.Add(temp);
        int i = searchList.Count - 1;
        // temp.GetComponentInChildren<Text>().text = searchList[i]["username"].ToString();
        if (searchList[i].ContainsKey("requestList"))
            temp.GetComponent<UserB>().Create(searchList[i]["id"].ToString(), searchList[i]["requestList"] as List<object>, searchList[i]["name"].ToString(), searchList[i]["title"].ToString(),
                searchList[i]["stars"].ToString(), (searchList[i]["friendList"] as List<object>).Count, SetCharacter(searchList[i]["character"] as Dictionary<string, object>), (bool)searchList[i]["noti"]);
        else
            temp.GetComponent<UserB>().Create(searchList[i]["id"].ToString(), null,
                searchList[i]["name"].ToString(), searchList[i]["title"].ToString(), searchList[i]["stars"].ToString(),
                (searchList[i]["friendList"] as List<object>).Count, SetCharacter(searchList[i]["character"] as Dictionary<string, object>), (bool)searchList[i]["noti"]);

        tempSearchButtons.Add(temp);
        temp.SetActive(false);
        if (searchList.Count > 4)
        {
            moreSearch = true;
        }
        Invoke("EndSearch", .7f);
    }

    public void GetMoreList()
    {
        searching = true;
        loadingObj.SetActive(true);
        loadingObj.transform.SetAsLastSibling();
        Fire.INS.GetMoreFriendList(currentT);
    }
    public void SetFriendsMore(List<Dictionary<string, object>> list)
    {
        int searchListSize = searchList.Count;
        searchList.AddRange(list);
        // Debug.Log("More");
        if (searchListSize == searchList.Count)
        {
            Debug.Log("Last");
            loadingObj.SetActive(false);
            moreSearch = false;
            return;
        }
        searchCount++;

        for (int i = searchCount * 5; i < searchList.Count; i++)
        {
            if (searchList[i]["id"].ToString() == Fire.INS.GetCurrentUser().UserId)
            {
                searchList.RemoveAt(i);
                Debug.Log("Removed: " + i);
                i--;
                continue;
            }

            GameObject temp = Instantiate(userButton, ports[2]);
            searchButtons.Add(temp);

            if (searchList[i].ContainsKey("requestList"))
                temp.GetComponent<UserB>().Create(searchList[i]["id"].ToString(), searchList[i]["requestList"] as List<object>,
                    searchList[i]["name"].ToString(), searchList[i]["title"].ToString(), searchList[i]["stars"].ToString(),
                    (searchList[i]["friendList"] as List<object>).Count, SetCharacter(searchList[i]["character"] as Dictionary<string, object>), (bool)searchList[i]["noti"]);
            else
                temp.GetComponent<UserB>().Create(searchList[i]["id"].ToString(), null,
                    searchList[i]["name"].ToString(), searchList[i]["title"].ToString(), searchList[i]["stars"].ToString(),
                    (searchList[i]["friendList"] as List<object>).Count, SetCharacter(searchList[i]["character"] as Dictionary<string, object>), (bool)searchList[i]["noti"]);

            /*foreach (KeyValuePair<string, object> pair in searchList[i])
            {
                Debug.Log(i + ":  " + pair.Key + " " + pair.Value);
            }*/
            tempSearchButtons.Add(temp);
            temp.SetActive(false);
        }

        if (searchList.Count < (5 + searchCount * 5))
        {
            moreSearch = false;
        }
        Invoke("EndSearch", .7f);
    }
    void EndSearch()
    {
        foreach (GameObject game in tempSearchButtons)
        {
            game.SetActive(true);
        }
        tempSearchButtons = new List<GameObject>();
        loadingObj.SetActive(false);
        searching = false;
    }

    #endregion


    public void SendFriendRequest(string id, List<object> currentList, bool noti)
    {
        List<object> friendRequest = currentList ?? new List<object>();
        if (friendRequest.Count > 9)
        {
            GC.INS.errorM.Error(8);
            return;
        }
        if (waitRequestList.Count > 9)
        {
            GC.INS.errorM.Error(9);
            return;
        }
        Dictionary<string, object> request = new Dictionary<string, object>
        {
             { "id", Fire.INS.GetCurrentUser().UserId },
             { "name", GC.INS.username},
             { "accepted", false },
             { "rejected", false },
             { "seen", false },
             {"character", GC.INS.characterAsMap}
        };
        waitRequestList.Add(id);
        friendRequest.Add(request);
        Dictionary<string, object> newDataToSend = new Dictionary<string, object>
        {
             { "requestList", friendRequest },
        };
        Fire.INS.MergeDataFirestore(newDataToSend, id);
        Dictionary<string, object> newDataToKeep = new Dictionary<string, object>
        {
             { "waitRequestList", waitRequestList },
        };
        Fire.INS.MergeDataFirestore(newDataToKeep);
        if (noti)
            GC.INS.rdb.SendFriendRequest(id);
    }

    public void RespondRequest(bool accepted)
    {
        if (cId == "")
            return;

        if (friendsC.Count > 23 && accepted)
        {
            GC.INS.errorM.Error(7);
            return;
        }
        Fire.INS.GetForeignDataRespond(cId, accepted);
    }
    public void SetDataRespond(Dictionary<string, object> foreignData, bool accepted)
    {
        if (foreignData == null) return;
        Debug.Log("Cid:  " + cId);
        changesInImbox = true;
        List<object> friendRequest = (foreignData.ContainsKey("requestList")) ? foreignData["requestList"] as List<object> : new List<object>();
        Dictionary<string, object> request = new Dictionary<string, object>
        {
             { "id", Fire.INS.GetCurrentUser().UserId },
             { "name", GC.INS.username},
             { "accepted", accepted },
             { "rejected", (!accepted) },
             { "seen", false },
        };
        if (accepted)
        {
            Dictionary<string, object> newFriend = new Dictionary<string, object>
            {
                 { "id", foreignData["id"].ToString()},
                 { "name", foreignData["name"].ToString()},
                 { "title", foreignData["title"].ToString()},
                 { "stars", foreignData["stars"].ToString()},
                 { "character", foreignData["character"]},
            };
            friendsC.Add(SetCharacter(foreignData["character"] as Dictionary<string, object>));

            GameObject temp = Instantiate(friendStaffBtn, friendShopLists[(friendsC.Count - 1) / 8]);

            Debug.Log("FriendsC: " + friendsC.Count + "  shopList: " + friendShopLists.Length);
            temp.GetComponent<StaffBtn>().Create(friendsC[friendsC.Count - 1]);
            friendsSataffBtns.Add(temp);
            AddFriend(newFriend);
            if (GC.INS && GC.INS.dm.tutoRewards)
            {
                GC.INS.dm.AddFriendFirst();
            }
            GC.INS.pg.Achivements(4, 0);

        }
        friendRequest.Add(request);
        Dictionary<string, object> newDataToSend = new Dictionary<string, object>
        {
             { "requestList", friendRequest },
        };
        if ((bool)foreignData["noti"])
        {
            GC.INS.rdb.AcceptFriendRequest(cId);
        }
        Fire.INS.MergeDataFirestore(newDataToSend, cId);
        DeleteNotification(cNumber);
        cId = "";
    }
    void AddFriend(Dictionary<string, object> newFriend)
    {
        GameObject tempB = Instantiate(friendButton, ports[1]);
        friendButtons.Add(tempB);
        Friend fr = tempB.GetComponent<Friend>();
        fr.Create(newFriend["id"].ToString(), newFriend["name"].ToString(), newFriend["title"].ToString(), newFriend["stars"].ToString(), friendList.Count, friendsC[friendsC.Count - 1]);
        friendList.Add(newFriend);
        GC.INS.friendsTCus.text = friendList.Count.ToString();
    }
    void AddFriendWithPhoto(Dictionary<string, object> newFriend)
    {
        GameObject tempB = Instantiate(friendButton, ports[1]);
        /*GameObject tempB = Instantiate(friendButton, ports[3]);
        tempB.SetActive(false);*/
        friendButtons.Add(tempB);
        Friend fr = tempB.GetComponent<Friend>();
        fr.Create(newFriend["id"].ToString(), newFriend["name"].ToString(), newFriend["title"].ToString(), newFriend["stars"].ToString(), friendList.Count, friendsC[friendsC.Count - 1]);
        friendList.Add(newFriend);
        friendsIds.Add(newFriend["id"].ToString());
      
        //facebook.SetTestButton(tempB);
    }

    #region Notifications
    public void DeleteNotification(int number)
    {
        Debug.Log("DeletingNotification");
        for (int j = 0; j < waitRequestList.Count; j++)
        {
            if (requestList[number]["id"].ToString() == waitRequestList[j])
                waitRequestList.RemoveAt(j);
        }
        requestList.RemoveAt(number);
        Destroy(notificationButtons[number].gameObject);
        notificationButtons.RemoveAt(number);
        for (int i = 0; i < notificationButtons.Count; i++)
        {
            if (notificationButtons[i].GetComponent<FriendRequestB>())
            {
                notificationButtons[i].GetComponent<FriendRequestB>().ResetNumber(i);
            }
            else
            {
                notificationButtons[i].GetComponent<ResponseB>().ResetNumber(i);
            }
        }
        changesInImbox = true;
    }
    public void DeleteGiftNotification(int number)
    {
        giftList.RemoveAt(number);
        Destroy(giftnotificationButtons[number].gameObject);
        giftnotificationButtons.RemoveAt(number);
        for (int i = 0; i < giftnotificationButtons.Count; i++)
        {
            giftnotificationButtons[i].GetComponent<ResponseB>().ResetNumber(i);
        }
        changesInImbox = true;
        if (GC.INS)
        {
            GC.INS.gift.newGift = true;
        }
    }
    public void OpenNotificationPanel()
    {
        if (notificationPop.activeInHierarchy)
        {
            changesInImbox = true;
            for (int i = 0; i < requestList.Count; i++)
            {
                requestList[i]["seen"] = true;
            }
            notificationPop.SetActive(false);
        }

    }
    public void CloseNotification()
    {
        for (int i = 0; i < notificationButtons.Count; i++)
        {
            if (notificationButtons[i].GetComponent<ResponseB>())
            {
                notificationButtons[i].GetComponent<ResponseB>().Respond();
            }

        }
        if (changesInImbox)
        {
            Dictionary<string, object> newData = new Dictionary<string, object>
            {
                { "requestList", requestList },
                { "friendList", friendList },
                { "waitRequestList", waitRequestList },
                {"gifts", giftList }
            };
            Fire.INS.MergeDataFirestore(newData);
            changesInImbox = false;
        }
    }
    public void CheckClosePhone()
    {
        if (changesInImbox)
        {
            Dictionary<string, object> newData = new Dictionary<string, object>
            {
                { "requestList", requestList },
                { "friendList", friendList },
                { "waitRequestList", waitRequestList },
                {"gifts", giftList }
            };
            Fire.INS.MergeDataFirestore(newData);
            changesInImbox = false;
        }
    }
    #endregion


    public void Visit(string id, int number)
    {
        whiteScreen.SetActive(true);
        SceneManager.LoadScene("Scene2", LoadSceneMode.Additive);
        GC.INS.visit = true;
        GC.INS.roomsArrange.SetActive(false);
        GC.INS.costumersArrange.SetActive(false);
        GC.INS.gameObject.SetActive(false);
        visitId = id;

        visitNumber = number;
    }
    public void Bye()
    {
        GC.INS.roomsArrange.SetActive(true);
        GC.INS.visit = false;
        GC.INS.costumersArrange.SetActive(true);
        GC.INS.gameObject.SetActive(true);
        GC.INS.canvas[0].SetActive(true);
        GC.INS.canvas[1].SetActive(true);
        GC.INS.CheckRecepcion();
        whiteScreen.SetActive(false);
        GC.INS.AddCoins(0);
        GC.INS.SetBlocks(0);
    }
    public void LogInFacebook()
    {
        facebook.LoginFacebook();
    }
}
