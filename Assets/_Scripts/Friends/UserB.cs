using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UserB : MonoBehaviour
{
    string uid;
    public Text uname, title, stars;
    public Image statusI;
    public Sprite addSp, waitSp;
    Button btn;
    bool addAcces, waitingResponse, wait, noti;
    int friendsCount;
    public CharacterSet character;
    //List to add your friend request and send new list of requests
    List<object> currentfriendList;
    public void Create(string uid, List<object> currentfriendList, string username, string title, string stars, int friendsCount, Character chara, bool noti)
    {
        this.uid = uid;
        this.noti = noti;
        uname.text = username;
        this.stars.text = stars;
        this.title.text = title;
        if (currentfriendList != null)
            this.currentfriendList = currentfriendList;
        btn = GetComponent<Button>();
        btn.onClick.AddListener(AddFriend);
        addAcces = true;
        List<string> waitList = FRC.INS.waitRequestList;
        List<Dictionary<string, object>> friendList = FRC.INS.friendList;
        List<Dictionary<string, object>> requestList = FRC.INS.requestList;
        this.friendsCount = friendsCount;
        character.SetCharacter(chara);
        //Checks if friend request already sent
        for (int i = 0; i < waitList.Count; i++)
        {
            if (waitList[i] == uid)
            {
                addAcces = false;
                waitingResponse = true;
                wait = true;
            }
               
        }

        //Checks if is already a friend
        for (int i = 0; i < friendList.Count; i++)
        {
            if (friendList[i]["id"].ToString() == uid)
                addAcces = false;
        }

        //Checks if you have a request from him
        for (int i = 0; i < requestList.Count; i++)
        {
            if (requestList[i]["id"].ToString() == uid)
            {
                addAcces = false;
                waitingResponse = true;
            }

        }

        //Debug.Log("Access: " + uid + "  :" + addAcces);
        if (!waitingResponse)
            statusI.gameObject.SetActive(addAcces);
        else
            statusI.sprite = waitSp;
        btn.interactable = addAcces;
        if (wait)
            btn.interactable = true;
    }
    
    public void AddFriend()
    {
        if(wait)
        {
            FRC.INS.OpenCancel(uid, transform.position, this);
            return;
        }
        if (!addAcces)
            return;
        if (friendsCount > 23)
        {
            GC.INS.errorM.Error(7);
            return;
        }
        if (FRC.INS.waitRequestList.Count > 9)
        {
            GC.INS.errorM.Error(9);
            return;
        }
        if (currentfriendList!=null && currentfriendList.Count > 9)
        {
            GC.INS.errorM.Error(8);
            return;
        }
        addAcces = false;
        statusI.sprite = waitSp;
        btn.interactable = addAcces;
        FRC.INS.SendFriendRequest(uid, currentfriendList, noti);
    }
    public void CancelRequest()
    {
        Debug.Log("Canceled");
        addAcces = true;
        statusI.sprite = addSp;
        wait = false;
    }
    
}
