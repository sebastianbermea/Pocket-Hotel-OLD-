using Facebook.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MC : MonoBehaviour
{
    public static MC INS { get; private set; }
    public InputField emailInputL, passInputL, usernameInput, emailS, passS, pass2S;
    public bool signedIn, pause;
    public GameObject load, phoneNoInternet, tequilaap;
    [SerializeField] GameObject loginP;
    public Text errorT;
    public string error = "";
    bool internetChecked;
    void Awake()
    {
        if (INS == null)
        {
            INS = this;
        }
        else
        {
            Debug.LogError("Duplicated MC");
            Destroy(gameObject);
        }
    }
    void Start()
    {
#if UNITY_IOS
        if (Application.isEditor)
        {
            loginP.SetActive(true);
            tequilaap.SetActive(false);
        }
#endif
    }
    public void SignInWithEmail()
    {
        Fire.INS.SignInEmailPassword(emailInputL.text, passInputL.text);
    }
    public void SignUpWithEmail()
    {
        if (pass2S.text == passS.text)
            Fire.INS.RegisterEmailPassword(emailS.text, passS.text, usernameInput.text);
        else
            error = "Passwords dont match";
    }
    public void Anon()
    {
        Fire.INS.SignInAnon();
    }
    private void Update()
    {
        if (signedIn)
        {
            SceneManager.LoadScene("SampleScene");
        }
        errorT.text = error;
    }

    public void CheckInternet()
    {
#if UNITY_ANDROID && !UNITY_EDITOR || UNITY_IOS && !UNITY_EDITOR
        CheckConnectivity();
#endif
    }
    void CheckConnectivity()
    {
        if (internetChecked)
            return;
        //Debug.Log("StartCheck");
        StartCoroutine(checkInternet((isConnected) =>
        {
            internetChecked = true;
            //Debug.Log("End Check: " + isConnected);
            if (isConnected)
            {
                Fire.INS.SignInWithSocial();
            }
            else
            {
                loginP.SetActive(false);
                phoneNoInternet.SetActive(true);
                tequilaap.SetActive(false);
            }
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
    private void OnApplicationPause(bool pause)
    {
        if (!pause)
        {
            CheckConnectivity();
            this.pause = true;
        }
    }


}
