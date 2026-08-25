using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FriendRequestB : MonoBehaviour
{
    int number;
    string id;
    public Text username;
    public CharacterSet character;
    public void Create(string id, int number, string username, Character chara)
    {
        this.username.text = username + GC.INS.t.GetText(143);
        this.id = id;
        this.number = number;
        character.SetCharacter(chara);
    }
    public void ResetNumber(int newNumber)
    {
        number = newNumber;
    }
    public void Respond(bool accepted)
    {
        //MC.INS.RespondRequest(number, id, accepted, temp);
    }
    public void Click()
    {
        FRC.INS.OpenResponse(id, transform.position, number);
    }
}
