using Firebase.Auth;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
#if UNITY_ANDROID
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using UnityEngine.SocialPlatforms;
#endif
using System.Threading.Tasks;
using Firebase;
using Firebase.Firestore;
using Firebase.Extensions;
using System;
using UnityEngine.Assertions;

using Yodo1.MAS;


public class Fire : MonoBehaviour
{
    FirebaseAuth auth;
    FirebaseUser user;
    FirebaseApp app;
    FirebaseFirestore fdb;

    [HideInInspector]
    public bool firstTime = false;
    public static Fire INS { get; private set; }
    [HideInInspector]
    public string username;
    Timestamp shiftStart;
    [HideInInspector]
    public float offPer;
    [HideInInspector]
    public bool prestige;

    void Awake()
    {
        if (INS == null)
        {
            DontDestroyOnLoad(gameObject);
            INS = this;
            InitializeFirebase();
        }
        else
        {
            Debug.LogWarning("Duplicated Firebase");
            Destroy(gameObject);
        }
    }
    private void OnDestroy()
    {
        if (auth != null)
        {
            auth.StateChanged -= AuthStateChanged;
            auth = null;
        }
    }

    void InitializeFirebase()
    {
        Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            var dependencyStatus = task.Result;
            if (dependencyStatus == Firebase.DependencyStatus.Available)
            {
                // Create and hold a reference to your FirebaseApp,
                // where app is a Firebase.FirebaseApp property of your application class.
                auth = FirebaseAuth.DefaultInstance;
                fdb = FirebaseFirestore.DefaultInstance;
                app = FirebaseApp.DefaultInstance;
                auth.StateChanged += AuthStateChanged;
                AuthStateChanged(this, null);

                // Set a flag here to indicate whether Firebase is ready to use by your app.
            }
            else
            {
                UnityEngine.Debug.LogError(System.String.Format(
                  "Could not resolve all Firebase dependencies: {0}", dependencyStatus));
                // Firebase Unity SDK is not safe to use here.
                //MenuController.INS.SetError(dependencyStatus.ToString());
            }
        });
#if UNITY_ANDROID
        PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder()
            .RequestServerAuthCode(false /* Don't force refresh */)
            .Build();

        PlayGamesPlatform.InitializeInstance(config);
        PlayGamesPlatform.Activate();
#endif
    }
    private void Start()
    {
        //MobileAds.Initialize(initStatus => {
        //    Debug.Log("Initialized MobileAds");
        //    InitAds();
        //});

        InitAds();
    }
    public void SignInWithSocial()
    {
#if UNITY_EDITOR
        MC.INS.CheckInternet();
#endif
        // Sign In and Get a server auth code.
        Social.localUser.Authenticate((bool success, string result) =>
        {
            Debug.Log("Success" + success);
            Debug.Log("!!!!Result: " + result);
            if (!success)
            {
                //Debug.LogWarning("SignInOnClick: Failed to Sign into Play Games Services.");
                MC.INS.CheckInternet();
                return;
            }
#if UNITY_ANDROID
            string authCode = PlayGamesPlatform.Instance.GetServerAuthCode();
            if (string.IsNullOrEmpty(authCode))
            {
                //Debug.LogError("SignInOnClick: Signed into Play Games Services but failed to get the server auth code.");
                MC.INS.CheckInternet();
                return;
            }
            //Debug.LogFormat("SignInOnClick: Auth code is: {0}", authCode);

            // Use Server Auth Code to make a credential
            Credential credential = PlayGamesAuthProvider.GetCredential(authCode);

#elif UNITY_IOS || !UNITY_EDITOR
            var credentialFuture = GameCenterAuthProvider.GetCredentialAsync();
            var retUserFuture = credentialFuture.ContinueWith(credentialTask =>
            {
                if (credentialTask.IsFaulted)
                    throw credentialTask.Exception;
                if (!credentialTask.IsCompleted)
                    Debug.Log("Failed to sign in");

                var credential = credentialTask.Result;
#endif

                // Sign In to Firebase with the credential
                auth.SignInWithCredentialAsync(credential).ContinueWith(task =>
            {
                if (task.IsCanceled)
                {
                    Debug.LogError("SignInOnClick was canceled.");
                    MC.INS.CheckInternet();
                    return;
                }
                if (task.IsFaulted)
                {
                    MC.INS.CheckInternet();
                    Debug.LogError("SignInOnClick encountered an error: " + task.Exception);
                    return;
                }

                FirebaseUser newUser = task.Result;
                username = newUser.DisplayName;
                Debug.LogFormat("SignInOnClick: User signed in successfully: {0} ({1})", newUser.DisplayName, newUser.UserId);
            });
            });
#if UNITY_IOS
            UpdateUser();
        });
#endif
    }
    public void SignInSocial()
    {
        Social.localUser.Authenticate((bool success, string result) =>
        {
            Debug.Log("!!!!Result Sign In Social: " + result);
            if (!success)
            {
                //Debug.LogError("SignInOnClick: Failed to Sign into Play Games Services.");
                return;
            }
        });
    }
    void AuthStateChanged(object sender, System.EventArgs eventArgs)
    {
        Debug.Log("Auth state changed");
        if (auth.CurrentUser != user)
        {
            bool signedIn = user != auth.CurrentUser && auth.CurrentUser != null;
            Debug.Log("Signed in");
            if (!signedIn && user != null)
            {
                if (SceneManager.GetActiveScene().name != "Login")
                    SceneManager.LoadScene("Login");
            }
            user = auth.CurrentUser;
            if (signedIn)
            {
                Debug.Log("user: " + user.UserId);
                MC.INS.signedIn = true;
            }
        }
        else
        {
            if (eventArgs != null)
            {
                SignInWithSocial();
            }
        }
    }

    public void SignInAnon()
    {
        auth.SignInAnonymouslyAsync().ContinueWith(task =>
        {
            if (task.IsCanceled)
            {
                Debug.LogError("SignInAnonymouslyAsync was canceled.");
                return;
            }
            if (task.IsFaulted)
            {
                Debug.LogError("SignInAnonymouslyAsync encountered an error: " + task.Exception);
                return;
            }
            username = "";
            user = task.Result;
        });
    }
    public void RegisterEmailPassword(string email, string password, string username)
    {
        auth.CreateUserWithEmailAndPasswordAsync(email, password).ContinueWith(task =>
        {
            if (task.IsCanceled)
            {
                Debug.LogError("CreateUserWithEmailAndPasswordAsync was canceled.");
                return;
            }
            if (task.IsFaulted)
            {
                Debug.Log("CreateUserWithEmailAndPasswordAsync encountered an error: " + task.Exception);
                MC.INS.error = task.Exception.Flatten().InnerExceptions[0].Message;
                return;
            }
            if (task.IsCompleted)
            {
                this.username = username;
                UpdateUser();
                firstTime = true;
            }

            // Firebase user has been created.
            user = task.Result;
            Debug.LogFormat("Firebase user created successfully: {0} ({1})",
                user.DisplayName, user.UserId);
        });
    }

    public void SignInEmailPassword(string email, string password)
    {
        auth.SignInWithEmailAndPasswordAsync(email, password).ContinueWith(task =>
        {
            if (task.IsCanceled)
            {
                Debug.LogError("SignInWithEmailAndPasswordAsync was canceled.");
                return;
            }
            if (task.IsFaulted)
            {
                Debug.Log("SignInWithEmailAndPasswordAsync encountered an error: " + task.Exception);
                MC.INS.error = task.Exception.Flatten().InnerExceptions[0].Message;
                return;
            }

            FirebaseUser user = task.Result;
            Debug.LogFormat("User signed in successfully: {0} ({1})",
                user.DisplayName, user.UserId);
        });
    }
    public void SignInWithFacebook(string accessToken)
    {
        Credential credential = FacebookAuthProvider.GetCredential(accessToken);
        auth.SignInWithCredentialAsync(credential).ContinueWith(task =>
        {
            if (task.IsCanceled)
            {
                Debug.LogError("SignInWithCredentialAsync was canceled.");
                return;
            }
            if (task.IsFaulted)
            {
                Debug.LogError("SignInWithCredentialAsync encountered an error: " + task.Exception);
                MC.INS.error = task.Exception.Flatten().InnerExceptions[0].Message;
                return;
            }

            FirebaseUser newUser = task.Result;
            Debug.LogFormat("User signed in successfully: {0} ({1})",
                newUser.DisplayName, newUser.UserId);
        });
    }
    public FirebaseUser GetCurrentUser()
    {
        return auth.CurrentUser;
    }
    public void LogOut()
    {
        Debug.Log("login out");
        auth.SignOut();
    }
    public void UpdateUser()
    {
        if (user != null)
        {
            UserProfile profile = new UserProfile
            {
                DisplayName = username,
                //PhotoUrl = new System.Uri("https://example.com/jane-q-user/profile.jpg"),
            };
            user.UpdateUserProfileAsync(profile).ContinueWith(task =>
            {
                if (task.IsCanceled)
                {
                    Debug.LogError("UpdateUserProfileAsync was canceled.");
                    return;
                }
                if (task.IsFaulted)
                {
                    Debug.LogError("UpdateUserProfileAsync encountered an error: " + task.Exception);
                    return;
                }

                Debug.Log("User profile updated successfully.");
            });
        }
    }

    public void SaveDataFirestore(Dictionary<string, object> data)
    {
        DocumentReference docRef = fdb.Collection("hotels").Document(user.UserId);
        docRef.SetAsync(data).ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted)
            {
                Debug.LogError("SaveDataFireastore encountered an error: " + task.Exception);
                return;
            }
            // Debug.Log("Added data to the firestore database.");
        });

    }
    /*public void SaveUserDataFirestore(Dictionary<string, object> data)
    {
        DocumentReference docRef = fdb.Collection("users").Document(user.UserId);
        docRef.SetAsync(data).ContinueWithOnMainThread(task => {
            if (task.IsFaulted)
            {
                Debug.LogError("SaveDataFireastore encountered an error: " + task.Exception);
                return;
            }
            Debug.Log("Added data to the firestore database.");
        });

    }*/
    public void MergeDataFirestore(Dictionary<string, object> data)
    {
        DocumentReference docRef = fdb.Collection("users").Document(user.UserId);

        docRef.SetAsync(data, SetOptions.MergeAll).ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted)
            {
                Debug.LogError("SaveDataFireastore encountered an error: " + task.Exception);
                return;
            }

            //Debug.Log("Added data to the firestore database.");
        });

    }
    public void MergeDataFirestore(Dictionary<string, object> data, string id)
    {
        DocumentReference docRef = fdb.Collection("users").Document(id);
        docRef.SetAsync(data, SetOptions.MergeAll).ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted || task.IsCanceled)
            {
                Debug.LogError("SaveDataFireastore encountered an error: " + task.Exception);
                return;
            }

            //Debug.Log("Added data to the firestore database.");
        });

    }
    bool gettingData;
    public void GetData()
    {
        if (gettingData) return;
        gettingData = true;
        DocumentReference docRef = fdb.Collection("hotels").Document(user.UserId);
        docRef.GetSnapshotAsync().ContinueWithOnMainThread(task =>
        {
            Assert.IsNull(task.Exception);
            DocumentSnapshot snapshot = task.Result;
            if (snapshot.Exists)
            {
                //Debug.Log(String.Format("Document data for {0} document:", snapshot.Id));
                Dictionary<string, object> data = snapshot.ToDictionary();
                /*foreach (KeyValuePair<string, object> pair in data)
                {
                    Debug.Log(String.Format("{0}: {1}", pair.Key, pair.Value));
                }*/
                GC.INS.SetHotel(data);
            }
            else
            {
                GC.INS.SetHotel(null);
            }
            gettingData = false;
        });

    }
    public void GetFriendData(string id)
    {

        DocumentReference docRef = fdb.Collection("hotels").Document(id);
        docRef.GetSnapshotAsync().ContinueWithOnMainThread(task =>
        {

            DocumentSnapshot snapshot = task.Result;
            if (snapshot.Exists)
            {
                Dictionary<string, object> data = snapshot.ToDictionary();
                VC.INS.SetHotel(data);
            }
            else
            {
                Debug.Log("Snapshot does not exists");
                VC.INS.SetHotel(null);
            }
        });
    }
    /*
    public void GetDataUserData()
    {
        DocumentReference docRef = fdb.Collection("users").Document(user.UserId);
        docRef.GetSnapshotAsync().ContinueWithOnMainThread(task =>
        {
            DocumentSnapshot snapshot = task.Result;
            if (snapshot.Exists)
            {
                Dictionary<string, object> data = snapshot.ToDictionary();
                GC.INS.SetHotel(data);
            }
            else
            {
                GC.INS.SetHotel(null);
            }
        });

    }
    */

    public void ListenToData()
    {
        DocumentReference docRef = fdb.Collection("users").Document(auth.CurrentUser.UserId);
        docRef.Listen(snapshot =>
        {
            if (snapshot.Exists)
            {
                //Debug.Log("Callback received document snapshot.");
                if (GC.INS != null)
                    FRC.INS.SetData(snapshot.ToDictionary());
            }
            else
            {
                //Debug.Log("Snapshot does not exist");
                if (GC.INS != null)
                    FRC.INS.SetData(null);
            }

        });
    }
    public void GetForeingDataCancel(string id, UserB btn)
    {
        DocumentReference docRef = fdb.Collection("users").Document(id);
        docRef.GetSnapshotAsync().ContinueWithOnMainThread(task =>
        {
            DocumentSnapshot snapshot = task.Result;
            if (snapshot.Exists)
            {
                Dictionary<string, object> data = snapshot.ToDictionary();
                FRC.INS.CancelRequestFunc(data, id, btn);
            }
            else
            {
                FRC.INS.CancelRequestFunc(null, id, btn);
            }
        });
    }
    /*public void GetForeingData(string id, UserB btn)
    {
        DocumentReference docRef = fdb.Collection("users").Document(id);
        docRef.GetSnapshotAsync().ContinueWithOnMainThread(task =>
        {
            DocumentSnapshot snapshot = task.Result;
            if (snapshot.Exists)
            {
                Dictionary<string, object> data = snapshot.ToDictionary();
                FRC.INS.CancelRequestFunc(data, id, btn);
            }
            else
            {
                FRC.INS.CancelRequestFunc(null, id, btn);
            }
        });
    }
    */
    public void GetForeignDataRequest(string id, int i)
    {
        //Debug.Log("Getting data... ");
        //Debug.Log("Fireabaese connected" + fdb != null);
        DocumentReference doc = fdb.Collection("users").Document(id);
        doc.GetSnapshotAsync().ContinueWithOnMainThread(task =>
        {
            DocumentSnapshot snapshot = task.Result;
            if (snapshot.Exists)
            {
                Dictionary<string, object> data = snapshot.ToDictionary();
                FRC.INS.SetRequestFriend(data, i);
            }
            else
            {
                FRC.INS.SetRequestFriend(null, i);
            }
        });
    }
    public void GetForeignDataActualize(string id, int i)
    {
        //Debug.Log("Fireabaese connected" + fdb != null);
        DocumentReference doc = fdb.Collection("users").Document(id);
        doc.GetSnapshotAsync().ContinueWithOnMainThread(task =>
        {
            DocumentSnapshot snapshot = task.Result;
            if (snapshot.Exists)
            {
                Dictionary<string, object> data = snapshot.ToDictionary();
                FRC.INS.ActualizeFriendData(data, i);
            }
            else
            {
                FRC.INS.ActualizeFriendData(null, i);
            }
        });
    }
    public void GetForeignDataRespond(string id, bool acc)
    {
        //Debug.Log("Fireabaese connected" + fdb != null);
        DocumentReference doc = fdb.Collection("users").Document(id);
        doc.GetSnapshotAsync().ContinueWithOnMainThread(task =>
        {
            DocumentSnapshot snapshot = task.Result;
            if (snapshot.Exists)
            {
                Dictionary<string, object> data = snapshot.ToDictionary();
                FRC.INS.SetDataRespond(data, acc);
            }
            else
            {
                FRC.INS.SetDataRespond(null, acc);
            }
        });
    }
    public void GetForeignDataVisit(string id)
    {
        //Debug.Log("Fireabaese connected" + fdb != null);
        DocumentReference doc = fdb.Collection("users").Document(id);
        doc.GetSnapshotAsync().ContinueWithOnMainThread(task =>
        {
            DocumentSnapshot snapshot = task.Result;
            if (snapshot.Exists)
            {
                Dictionary<string, object> data = snapshot.ToDictionary();
                VC.INS.SetUser(data);
            }
            else
            {
                VC.INS.SetUser(null);
            }
        });
    }
    //Firstap
    DocumentSnapshot lastSnap;
    public void GetFriendList(string friendquery)
    {
        List<Dictionary<string, object>> data = new List<Dictionary<string, object>>();
        CollectionReference citiesRef = fdb.Collection("users");
        Query query = citiesRef.WhereGreaterThanOrEqualTo("name", friendquery).WhereLessThanOrEqualTo("name", friendquery + '\uf8ff').Limit(5);
        query.GetSnapshotAsync().ContinueWithOnMainThread((querySnapshotTask) =>
        {
            int i = 0;
            foreach (DocumentSnapshot documentSnapshot in querySnapshotTask.Result.Documents)
            {
                i++;
                data.Add(documentSnapshot.ToDictionary());
                if (i == 5)
                    lastSnap = documentSnapshot;
            }
            FRC.INS.SetFriends(data);
            //Debug.Log("Query " + i );
        });

    }
    public void GetFriendOneMore(string friendquery)
    {
        List<Dictionary<string, object>> data = new List<Dictionary<string, object>>();
        CollectionReference citiesRef = fdb.Collection("users");
        Query query = citiesRef.WhereGreaterThanOrEqualTo("name", friendquery).WhereLessThanOrEqualTo("name", friendquery + '\uf8ff').Limit(1).StartAfter(lastSnap);
        query.GetSnapshotAsync().ContinueWithOnMainThread((querySnapshotTask) =>
        {
            int i = 0;
            foreach (DocumentSnapshot documentSnapshot in querySnapshotTask.Result.Documents)
            {
                i++;
                data.Add(documentSnapshot.ToDictionary());
                if (i == 1)
                    lastSnap = documentSnapshot;
            }
            FRC.INS.SetFriendOneMore(data);
        });

    }
    /*
    public async Task<List<Dictionary<string, object>>> GetFriendList(string friendquery)
    {
        List<Dictionary<string, object>> data = new List<Dictionary<string, object>>();
        CollectionReference citiesRef = fdb.Collection("users");
        Query query = citiesRef.WhereGreaterThanOrEqualTo("name", friendquery).WhereLessThanOrEqualTo("name", friendquery + '\uf8ff').Limit(5);
        await query.GetSnapshotAsync().ContinueWithOnMainThread((querySnapshotTask) =>
        {
            int i = 0;
            foreach (DocumentSnapshot documentSnapshot in querySnapshotTask.Result.Documents)
            {
                i++;
                data.Add(documentSnapshot.ToDictionary());
                if (i == 5)
                    lastSnap = documentSnapshot;
            }
            //Debug.Log("Query " + i );
        });
        return data;
    }
    */
    //Second and more snaps
    public void GetMoreFriendList(string friendquery)
    {
        List<Dictionary<string, object>> data = new List<Dictionary<string, object>>();
        CollectionReference citiesRef = fdb.Collection("users");
        Query query = citiesRef.WhereGreaterThanOrEqualTo("name", friendquery).WhereLessThanOrEqualTo("name", friendquery + '\uf8ff').Limit(5).StartAfter(lastSnap);
        query.GetSnapshotAsync().ContinueWithOnMainThread((querySnapshotTask) =>
        {
            int i = 0;
            foreach (DocumentSnapshot documentSnapshot in querySnapshotTask.Result.Documents)
            {
                i++;
                data.Add(documentSnapshot.ToDictionary());
                if (i == 5)
                    lastSnap = documentSnapshot;
            }
            FRC.INS.SetFriendsMore(data);
        });
    }

    /* public async Task<List<Dictionary<string, object>>> GetOneMoreFriendList(string friendquery)
     {
         List<Dictionary<string, object>> data = new List<Dictionary<string, object>>();
         CollectionReference citiesRef = fdb.Collection("users");
         Query query = citiesRef.WhereGreaterThanOrEqualTo("name", friendquery).WhereLessThanOrEqualTo("name", friendquery + '\uf8ff').Limit(1).StartAfter(lastSnap);
         await query.GetSnapshotAsync().ContinueWithOnMainThread((querySnapshotTask) =>
         {
             int i = 0;
             foreach (DocumentSnapshot documentSnapshot in querySnapshotTask.Result.Documents)
             {
                 i++;
                 data.Add(documentSnapshot.ToDictionary());
                 if (i == 1)
                     lastSnap = documentSnapshot;
             }
             Debug.Log(i);
         });
         return data;
     }

     */

    public void GetFriendFacebookId(string fid, FaceB faceB)
    {
        Debug.Log("Facebook friend");
        List<Dictionary<string, object>> data = new List<Dictionary<string, object>>();
        CollectionReference users = fdb.Collection("users");
        Query query = users.WhereEqualTo("faceId", fid).Limit(1);
        query.GetSnapshotAsync().ContinueWithOnMainThread((querySnapshotTask) =>
        {
            foreach (DocumentSnapshot documentSnapshot in querySnapshotTask.Result.Documents)
            {
                data.Add(documentSnapshot.ToDictionary());
            }
        });
        Debug.Log("Dsta Cound:" + data.Count);
        if (data.Count > 0)
        {
            Debug.Log(data[0]["id"]);
            faceB.AddToList(data[0]);
        }
        else
            faceB.AddToList(null);
    }

    #region Time
    public object GetTime()
    {
        return Timestamp.GetCurrentTimestamp();
    }
    public DateTime GetTimeInFormat()
    {
        shiftStart = Timestamp.GetCurrentTimestamp();
        return ((Timestamp)GetTime()).ToDateTime().ToUniversalTime();
    }
    public DateTime ParseTime(object time)
    {
        return ((Timestamp)time).ToDateTime().ToUniversalTime();
    }
    public void SetShiftStart(object time)
    {
        shiftStart = (Timestamp)time;
    }
    public object ShiftStart()
    {
        return shiftStart;
    }
    public int ParseSeconds(object time)
    {
        return (int)(DateTime.UtcNow - ((Timestamp)time).ToDateTime().ToUniversalTime()).TotalSeconds;
    }

    #endregion

    #region ADS

    //#if UNITY_ANDROID
    //    string doubleCoinsAwayId = "ca-app-pub-5973360501846047/1582508974";
    //    string doubleTodayRewardId = "ca-app-pub-5973360501846047/8077611046";
    //    string doubleShiftId = "ca-app-pub-5973360501846047/4667837318";
    //    string inGameAdId = "ca-app-pub-5973360501846047/3330387685";
    //    string boosterAdId = "ca-app-pub-5973360501846047/5467759579";
    //#else
    //    string doubleCoinsAwayId = "ca-app-pub-5973360501846047/7959365005";
    //    string doubleTodayRewardId = "ca-app-pub-5973360501846047/2034414045";
    //    string doubleShiftId = "ca-app-pub-5973360501846047/3772707983";
    //    string inGameAdId = "ca-app-pub-5973360501846047/2433035143";
    //    string boosterAdId = "ca-app-pub-5973360501846047/8887597003";
    //#endif
    //    int rewardT;
    //    private RewardedAd doubleCoinsAd;
    //    private RewardedAd doubleTodayRewardAd;
    //    private RewardedAd doubleShiftAd;
    //    private RewardedAd inGameAd;
    //    private RewardedAd boosterAd;


    //    void InitAds()
    //    {
    //        doubleCoinsAd = CreateAndLoadRewardedAd(doubleCoinsAwayId);
    //        doubleTodayRewardAd = CreateAndLoadRewardedAd(doubleTodayRewardId);
    //        doubleShiftAd = CreateAndLoadRewardedAd(doubleShiftId);
    //        inGameAd = CreateAndLoadRewardedAd(inGameAdId);
    //        boosterAd = CreateAndLoadRewardedAd(boosterAdId);
    //    }

    //    public RewardedAd CreateAndLoadRewardedAd(string adUnitId)
    //    {
    //        RewardedAd rewardedAd = new RewardedAd(adUnitId);

    //        rewardedAd.OnUserEarnedReward += HandleUserEarnedReward;
    //        rewardedAd.OnAdClosed += HandleRewardedAdClosed;

    //        // Create an empty ad request.
    //        AdRequest request = new AdRequest.Builder().Build();
    //        // Load the rewarded ad with the request.
    //        rewardedAd.LoadAd(request);
    //        return rewardedAd;
    //    }

    //    public void ShowVideoReward(int x)
    //    {
    //        rewardT = x;
    //        switch (rewardT)
    //        {
    //            case 0:
    //                if (doubleCoinsAd.IsLoaded())
    //                    doubleCoinsAd.Show();
    //                break;
    //            case 1:
    //                if (doubleTodayRewardAd.IsLoaded())
    //                    doubleTodayRewardAd.Show();
    //                break;
    //            case 2:
    //                if (doubleShiftAd.IsLoaded())
    //                    doubleShiftAd.Show();
    //                break;
    //            case 3:
    //                if (inGameAd.IsLoaded())
    //                    inGameAd.Show();
    //                break;
    //            case 4:
    //                if (boosterAd.IsLoaded())
    //                    boosterAd.Show();
    //                break;
    //        }
    //    }

    //    public void HandleRewardedAdClosed(object sender, EventArgs args)
    //    {
    //        Debug.Log(sender);
    //        MonoBehaviour.print("HandleRewardedAdClosed event received");
    //        switch (rewardT)
    //        {
    //            case 0:
    //                doubleCoinsAd = CreateAndLoadRewardedAd(doubleCoinsAwayId);
    //                break;
    //            case 1:
    //                doubleTodayRewardAd = CreateAndLoadRewardedAd(doubleTodayRewardId);
    //                break;
    //            case 2:
    //                doubleShiftAd = CreateAndLoadRewardedAd(doubleShiftId);
    //                break;
    //            case 3:
    //                inGameAd = CreateAndLoadRewardedAd(inGameAdId);
    //                break;
    //            case 4:
    //                boosterAd = CreateAndLoadRewardedAd(boosterAdId);
    //                break;
    //        }
    //    }

    //    public void HandleUserEarnedReward(object sender, Reward args)
    //    {
    //        Reward();
    //    }




    void InitAds()
    {
        Yodo1AdBuildConfig config =
        new Yodo1AdBuildConfig().enableUserPrivacyDialog(true);
        Yodo1U3dMas.SetAdBuildConfig(config);

        Yodo1U3dMas.InitializeSdk();

        Yodo1U3dMasCallback.OnSdkInitializedEvent += (success, error) =>
        {
            Debug.Log("[Yodo1 Mas] OnSdkInitializedEvent, success:" + success + ", error: " + error.ToString());
            if (success)
            {
                Debug.Log("[Yodo1 Mas] The initialization has succeeded");
            }
            else
            {
                Debug.Log("[Yodo1 Mas] The initialization has failed");
            }
        };
        InitializeRewardedAds();
    }

    private void InitializeRewardedAds()
    {
        // Add Events
        Yodo1U3dMasCallback.Rewarded.OnAdOpenedEvent += OnRewardedAdOpenedEvent;
        Yodo1U3dMasCallback.Rewarded.OnAdClosedEvent += OnRewardedAdClosedEvent;
        Yodo1U3dMasCallback.Rewarded.OnAdReceivedRewardEvent += OnAdReceivedRewardEvent;
        Yodo1U3dMasCallback.Rewarded.OnAdErrorEvent += OnRewardedAdErorEvent;
    }

    private void OnRewardedAdOpenedEvent()
    {
        Debug.Log("[Yodo1 Mas] Rewarded ad opened");
    }

    private void OnRewardedAdClosedEvent()
    {
        Debug.Log("[Yodo1 Mas] Rewarded ad closed");
    }

    private void OnAdReceivedRewardEvent()
    {
        Debug.Log("[Yodo1 Mas] Rewarded ad received reward");
        Reward();
    }

    private void OnRewardedAdErorEvent(Yodo1U3dAdError adError)
    {
        Debug.Log("[Yodo1 Mas] Rewarded ad error - " + adError.ToString());
    }

    public void ShowVideoReward(int x)
    {
        rewardT = x;
        bool isLoaded = Yodo1U3dMas.IsRewardedAdLoaded();
        if (!isLoaded) return;

        switch (rewardT)
        {
            case 0:
                Yodo1U3dMas.ShowRewardedAd("Double Coins Away");
                break;
            case 1:
                Yodo1U3dMas.ShowRewardedAd("Doubke today reward");
                break;
            case 2:
                Yodo1U3dMas.ShowRewardedAd("Double shift");
                break;
            case 3:
                Yodo1U3dMas.ShowRewardedAd("In Game ads");
                break;
            case 4:
                Yodo1U3dMas.ShowRewardedAd("Booster");
                break;
        }
    }

    int rewardT;
    //void Closed()
    //{
    //    switch (rewardT)
    //    {
    //        case 3:
    //            GC.INS.ad.Close();
    //            break;
    //    }
    //}
    void Reward()
    {
        GC.INS.dm.RecomendNoAds();
        Reward(rewardT);
    }
    public void Reward(int x)
    {
        switch (x)
        {
            case 0:
                GC.INS.DoubleCoinsAway();
                break;
            case 1:
                GC.INS.dm.DoubleTodayReward();
                break;
            case 2:
                GC.INS.DoubleShift();
                break;
            case 3:
                GC.INS.ad.Reward();
                break;
            case 4:
                GC.INS.AdBooster();
                break;
        }
    }



    #endregion


}
