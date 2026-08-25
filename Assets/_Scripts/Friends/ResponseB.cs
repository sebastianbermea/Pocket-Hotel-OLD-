using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResponseB : MonoBehaviour
{
    int number;
    string id;
    public Text uname;
    bool isGiftNot;
    public CharacterSet character;
    public void Create(string id, int number, string uname, Character chara)
    {
        //Debug.Log("Number " + number + "  id: " + id);
        this.id = id;
        this.number = number;
        this.uname.text = uname + GC.INS.t.GetText(144);
        character.SetCharacter(chara);
    }
    public void CreateGift(int number, string uname, Character chara)
    {
        this.number = number;
        this.uname.text = uname + GC.INS.t.GetText(145);
        isGiftNot = true;
        character.SetCharacter(chara);
    }
    public void ResetNumber(int newNumber)
    {
        number = newNumber;
    }
    public void Respond()
    {
        if (isGiftNot)
            FRC.INS.DeleteGiftNotification(number);
        else
            FRC.INS.DeleteNotification(number);
    }
}
