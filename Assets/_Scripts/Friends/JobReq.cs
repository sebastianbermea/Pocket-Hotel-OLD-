using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JobReq : MonoBehaviour
{
    public Text username;
    public CharacterSet cset;
    public void JobApp(string uname, Character character)
    {
        username.text = uname;
        gameObject.SetActive(true);
        cset.SetCharacter(character);
        GC.INS.customized = true;
    }

    public void Hire()
    {
        GC.INS.OpenShop(3);
        GC.INS.OpenSubShop(1);
        Close();
    }
    public void Close()
    {
        FRC.INS.currentJobApp = "";
        GC.INS.customized = true;
        gameObject.SetActive(false);
    }
    public void JobAppFriend()
    {
        username.text = GC.INS.username;
        Debug.Log(GC.INS.player.outfitId);
        cset.SetCharacter(GC.INS.player);
        gameObject.SetActive(true);
    }
    public void CloseVC()
    {
        gameObject.SetActive(false);
    }
    public void Send()
    {
        VC.INS.RequestJob();
        CloseVC();
    }
}
