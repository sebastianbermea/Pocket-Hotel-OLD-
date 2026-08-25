using Facebook.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FaceB : MonoBehaviour
{
    [HideInInspector]
    public List<string> faceIds, friendsIds;
    public GameObject facebookBtn;
    public GameObject friendBtn, userBtn, facebokCanvas;
    [HideInInspector]
    public string faceId;
    bool logf;
    public GameObject shareBtn;

    private void Awake()
    {


    }
    private void Start()
    {
        Debug.Log("FACebook logged: " + FB.IsLoggedIn);
        if (!FB.IsLoggedIn)
            return;
        facebookBtn.SetActive(false);
        if (faceIds != null)
            CheckFriendsButtons();
        SearchFriends();

    }
    public bool IsConnected()
    {
        return FB.IsLoggedIn;
    }
    private void InitCallback()
    {
        //Debug.Log("Callback");
        if (FB.IsInitialized)
        {
            // Signal an app activation App Event
            FB.ActivateApp();
            // Continue with Facebook SDK
            // ...
            if (logf)
            {
                logf = false;
                LoginFacebook();
            }
        }
        else
        {
            Debug.Log("Failed to Initialize the Facebook SDK");
        }
    }

    private void OnHideUnity(bool isGameShown)
    {
        if (!isGameShown)
        {
            // Pause the game - we will need to hide
            Time.timeScale = 0;
        }
        else
        {
            // Resume the game - we're getting focus again
            Time.timeScale = 1;
        }
    }

    public void SetList(List<object> facebookList, string faceId)
    {
        if (!FB.IsInitialized)
        {
            //Debug.Log("Init");
            // Initialize the Facebook SDK
            FB.Init(InitCallback, OnHideUnity);
        }
        else
        {
            // Already initialized, signal an app activation App Event
            FB.ActivateApp();
        }
        if (FB.IsLoggedIn)
            facebookBtn.SetActive(false);

        if (facebookList != null)
        {
            faceIds = new List<string>();
            friendsIds = new List<string>();
            for (int i = 0; i < facebookList.Count; i++)
            {
                Dictionary<string, object> tempDic = facebookList[i] as Dictionary<string, object>;
                friendsIds.Add(tempDic["id"].ToString());
                faceIds.Add(tempDic["faceId"].ToString());

            }
        }
        this.faceId = faceId;

    }
    public List<object> TransformFriendsToList()
    {
        List<object> tempList = new List<object>();
        for (int i = 0; i < friendsIds.Count; i++)
        {
            Dictionary<string, string> tempDic = new Dictionary<string, string>()
            {
                  { "id", friendsIds[i]},
                  { "faceId", faceIds[i]},

            };

            tempList.Add(tempDic);
        }

        return tempList;
    }
    public void LoginFacebook()
    {
        var perms = new List<string>() { "public_profile","gaming_profile", "email", "user_friends" };
        if (!FB.IsInitialized)
        {
            Debug.Log("Init");
            logf = true;
            // Initialize the Facebook SDK
            FB.Init(InitCallback, OnHideUnity);
        }
        else
            FB.LogInWithReadPermissions(perms, AuthCallback);
    }
    public void Share()
    {
        FB.ShareLink(new System.Uri("https://www.youtube.com/watch?v=ZZfulYCt6xc"), "Pocket Hotel",
            "Build the hotel of your dreams", new System.Uri("https://www.youtube.com/watch?v=ZZfulYCt6xc"),
            delegate (IShareResult result)
            {
                Debug.Log(result.RawResult);
                if (!string.IsNullOrEmpty(result.RawResult) && !result.Cancelled)
                {
                    GC.INS.lastFShare = System.DateTime.UtcNow;
                    shareBtn.SetActive(false);
                    GC.INS.dm.CloseLaptop();
                    int attempts = 0;
                    int type = 0, id = 0;
                    while (attempts < 4)
                    {
                        attempts++;
                        type = Random.Range(0, 12);
                        switch (type)
                        {
                            case 0:
                                id = Random.Range(1, 8);
                                break;
                            case 1:
                                id = Random.Range(1, 30);
                                break;
                            case 2:
                                id = Random.Range(1, 8);
                                break;
                            case 3:
                                id = Random.Range(1, 16);
                                break;
                            case 4:
                                id = Random.Range(1, 16);
                                break;
                            case 5:
                                id = Random.Range(1, 8);
                                break;
                            case 6:
                                id = Random.Range(1, 12);
                                break;
                            case 7:
                                id = Random.Range(1, 4);
                                break;
                            case 8:
                                id = Random.Range(1, 8);
                                break;
                            case 9:
                                id = Random.Range(1, 10);
                                break;
                            case 10:
                                id = Random.Range(1, 28);
                                break;
                            case 11:
                                id = Random.Range(1, 16);
                                break;
                        }
                        if (!GC.INS.customPurchased[type][id])
                        {
                            attempts = 5;
                        }
                    }
                    GC.INS.gift.AddGift(new Dictionary<string, object>
                    {
                        { "id",id},
                        { "type", 5},
                        { "subtype", type},
                    });
                }
            });
    }

    public void InviteFriends()
    {
        FB.AppRequest(
                "Come play this great game!",
                null, null, null, null, null, null,
                delegate (IAppRequestResult result)
                {
                    Debug.Log(result.RawResult);
                    if (!string.IsNullOrEmpty(result.RawResult) && !result.Cancelled && result.To != null)
                    {
                        Debug.Log(result.To);

                        Debug.Log("Temp Array");
                        string[] tempArray = result.To as string[];
                        for (int i = 0; i < tempArray.Length; i++)
                        {
                            if (string.IsNullOrEmpty(tempArray[i]))
                                continue;
                            GC.INS.dm.CheckFacebookInvite(tempArray[i]);
                            Debug.Log(tempArray[i]);
                            GC.INS.dm.AddTask(19, 1);
                        }

                    }
                }
            );
    }
    private void AuthCallback(ILoginResult result)
    {
        if (FB.IsLoggedIn)
        {
            // AccessToken class will have session details
            var aToken = Facebook.Unity.AccessToken.CurrentAccessToken;
            // Print current access token's User ID
            Debug.Log(aToken.UserId);
            // Print current access token's granted permissions
            foreach (string perm in aToken.Permissions)
            {
                Debug.Log(perm);
            }
            CheckFriendsButtons();
            SearchFriends();
            facebookBtn.SetActive(false);
            faceId = aToken.UserId;
        }
        else
        {
            Debug.Log("User cancelled login");
        }
    }
    public void CheckFriendsButtons()
    {
        for (int i = 0; i < friendsIds.Count; i++)
        {
            if (FRC.INS.friendsIds.Contains(friendsIds[i]))
            {
                GameObject tempB = Instantiate(friendBtn, facebokCanvas.transform);
                Friend fr = tempB.GetComponent<Friend>();
                Dictionary<string, object> data = FRC.INS.friendList[FRC.INS.friendsIds.IndexOf(friendsIds[i])];
                fr.Create(data["id"].ToString(), data["name"].ToString(), data["title"].ToString(), data["stars"].ToString(),
                    FRC.INS.friendsIds.IndexOf(data["id"].ToString()), FRC.INS.SetCharacter(data["character"] as Dictionary<string, object>));

            }
            else
            {
                Fire.INS.GetFriendFacebookId(faceIds[i], this);
            }
        }
    }
    public void SearchFriends()
    {
        string query = "me/friends";
        List<string> stList = new List<string>();
        FB.API(query, HttpMethod.GET,  result =>
        {
            Debug.Log("RAW" + result.RawResult);
            Dictionary<string, object> tempDic = (Dictionary<string, object>)Facebook.MiniJSON.Json.Deserialize(result.RawResult);
            List<object> friendList = (List<object>)tempDic["data"];

            Debug.Log("Face Friends: " + friendList.Count);
            foreach (object dic in friendList)
            {
                string tempId = ((Dictionary<string, object>)dic)["id"].ToString();
                stList.Add(tempId);
            }
            for (int i = 0; i < stList.Count; i++)
            {
                if (!faceIds.Contains(stList[i]))
                {
                    Fire.INS.GetFriendFacebookId(stList[i], this);
                }
            }
        });
    }
    public void AddToList(Dictionary<string, object> data)
    {
        if (data == null) return;
        if (!data.ContainsKey("id")) return;

        GC.INS.customized = true;

        friendsIds.Add(data["id"].ToString());
        faceIds.Add(data["faceId"].ToString());
        if (!FRC.INS.friendsIds.Contains(data["id"].ToString()))
        {
            GameObject temp = Instantiate(userBtn, facebokCanvas.transform);
            if (data.ContainsKey("requestList"))
                temp.GetComponent<UserB>().Create(data["id"].ToString(), data["requestList"] as List<object>,
                    data["name"].ToString(), data["title"].ToString(), data["stars"].ToString(), (data["friendList"] as List<object>).Count,
                    FRC.INS.SetCharacter(data["character"] as Dictionary<string, object>), (bool)data["noti"]);
            else
                temp.GetComponent<UserB>().Create(data["id"].ToString(), null,
                    data["name"].ToString(), data["title"].ToString(), data["stars"].ToString(),
                    (data["friendList"] as List<object>).Count, FRC.INS.SetCharacter(data["character"] as Dictionary<string, object>), (bool)data["noti"]);
        }
        else
        {
            GameObject tempB = Instantiate(friendBtn, facebokCanvas.transform);
            Friend fr = tempB.GetComponent<Friend>();
            fr.Create(data["id"].ToString(), data["name"].ToString(), data["title"].ToString(), data["stars"].ToString(),
                FRC.INS.friendsIds.IndexOf(data["id"].ToString()), FRC.INS.SetCharacter(data["character"] as Dictionary<string, object>));

        }
    }
}
