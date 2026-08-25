using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Friend : MonoBehaviour
{
    public Text uname, title, stars;
    string _id;
    int number;
    public GameObject coinBag;
    public CharacterSet character;
    public bool tutoFriend;
    private void Awake()
    {
        if (GC.INS.lastFriendVisit.Count <= number && !GC.INS.tutoOn)
        {
            //Debug.Log(GC.INS.lastFriendVisit.Count - 1);
            for (int i = GC.INS.lastFriendVisit.Count - 1; i < number; i++)
            {
                GC.INS.lastFriendVisit.Add(DateTime.Now.AddDays(-12));
            }
        }
    }
    private void Start()
    {
        if (tutoFriend)
        {
            character.SetCharacter(new Character(18, 28, 4, "Friend", 4, 3, 1, 0, 12, 2, 0, 0, 0, 0, false));
        }
    }
    public void Create(string id, string uname, string title, string stars, int number, Character character)
    {
        this.uname.text = uname;
        this.title.text = title;
        this.stars.text = stars;
        _id = id;
        this.number = number;
        this.character.SetCharacter(character);
    }
    private void OnEnable()
    {
        if (GC.INS.tutoOn)
        {
            coinBag.SetActive(true);
            return;
        }
        if ((DateTime.UtcNow - GC.INS.lastFriendVisit[number]).TotalDays >= 1)
        {
            coinBag.SetActive(true);
        }
        else
        {
            coinBag.SetActive(false);
        }
    }
    public void SetI()
    {
        if (FRC.INS.currentJobApp != "")
        {
            if(_id == FRC.INS.currentJobApp)
            {
                GC.INS.jobReq.JobApp(uname.text, FRC.INS.friendsC[number]);
            }
        }
    }
    public void Visit()
    {
        FRC.INS.Visit(_id,number);
    }
    public void VisitTuto()
    {
        GC.INS.tuto.VisitFriend();
        FRC.INS.Visit("qLsdYNGh7KbdcGY3T6ST6Oz4qSy1",0);
    }
}
