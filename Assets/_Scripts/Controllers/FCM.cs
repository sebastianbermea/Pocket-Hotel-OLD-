using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Messaging;
#if UNITY_IOS
using Unity.Notifications.iOS;
#endif
public class FCM : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }
    public void SetNoti()
    {
#if UNITY_ANDROID
        FirebaseMessaging.TokenReceived += TokenReceived;
        FirebaseMessaging.MessageReceived += MessageReceived;
        GC.INS.customized = true;
#endif
#if UNITY_IOS
        RequestAuthorization();
        FirebaseMessaging.TokenReceived += TokenReceived;
        FirebaseMessaging.MessageReceived += MessageReceived;
        GC.INS.customized = true;
#endif
    }
#if UNITY_IOS
    IEnumerator RequestAuthorization()
    {
        var authorizationOption = AuthorizationOption.Alert | AuthorizationOption.Badge;
        using (var req = new AuthorizationRequest(authorizationOption, true))
        {
            while (!req.IsFinished)
            {
                yield return null;
            };

            string res = "\n RequestAuthorization:";
            res += "\n finished: " + req.IsFinished;
            res += "\n granted :  " + req.Granted;
            res += "\n error:  " + req.Error;
            res += "\n deviceToken:  " + req.DeviceToken;
            Debug.Log(res);
      
        }
    }
#endif
    void MessageReceived(object sender, MessageReceivedEventArgs e)
    {
        Debug.Log("notification received " + e.Message);
    }
    void TokenReceived(object sender, TokenReceivedEventArgs e)
    {
        Debug.Log("Token received " + e.Token);
        GC.INS.rdb.SaveToken(e.Token);
    }
}
