using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour
{
    //Welcome to Pocket Hotel, i will help you build a succesful hotel!
    //First, set your hotel name and username. To help you get started you will receive 50,000 coins!
    //Nice!, now lets build some rooms
    //To keep the shop open, click on the lock button.
    //Good, now drag 3 rooms and add them to your hotel!
    //Perfect!, click here to finish building

    //Your hotel is closed right now!, Click here to start a shift
    //Congratulations! Your hotel is open for business and will keep runing even after you log off
    //Keep your hotel running so you keep receiving money!, Well, see you soon.

    //You leveled up, huh? Apparently you're serious about this
    //Lets get some decoration so you can improve your stars
    //Drag into the room, you can decorate only in the rooms that do not black out
    //More stars means more visitors, more visitors means more money!

    //Sometimes you have to fix room issues, follow the tips to learn how!
    //You are the man, you figure it out! to help you fix, you can build staff rooms
    //Connect with facebook to get friends!
    //You can visit friends to get daily bonuses or hire them to reduce wages!, and of course brag.

    //Your hotel is getting small, here you can increase your hotel's maximum size
    //Oops, a guest would have liked to use a gym, but you didn't have one
    //You can find gyms here!
    //Incredible, you are ready to run your own hotel! I'll keep an eye on what you can do
    //Dont forget to support us on facebook to get special rewards! See ya!

    public Animator anim;
    public Text txt;
    public GameObject msgBox, hotelSet, msgCheck, sideButtons, arrowPointer, fingerPointer, blocksP;
    public GameObject[] shopButtons, pointerParents, blocks;
    public RectTransform pointer, shiftBtn;
    int currentPointer, roomCount;
    public int current;
    string currentAnim;
    public void StartTutorial()
    {
        transform.parent.gameObject.SetActive(true);
        txt.text = GetText(0);
        current = 0;
        currentAnim = "1";
        Invoke("SetAnim", 1.5f);
        sideButtons.SetActive(false);
        for (int i = 0; i < shopButtons.Length; i++)
            shopButtons[i].SetActive(false);
    }
    public void Next()
    {
        if (current >= 23)
            return;
        current++;
        txt.text = GetText(current);
        switch (current)
        {
            case 1:
                msgBox.SetActive(false);
                anim.SetTrigger("1o");
                currentAnim = "2";
                Invoke("SetAnim", 1f);
                hotelSet.SetActive(true);
                msgCheck.SetActive(false);
                Invoke("NextPointer", 1.5f);
                SC.INS.PlaySound(0, 13, 0);
                break;
            case 2:
                if (GC.INS.ChangeHotelName())
                {
                    msgBox.SetActive(false);
                    anim.SetTrigger("2o");
                    currentAnim = "3";
                    Invoke("SetAnim", 1f);
                    msgCheck.SetActive(false);
                    currentPointer = 3;
                    Invoke("NextPointer", 1.5f);
                    shopButtons[0].SetActive(true);
                }
                else
                {
                    current--;
                    txt.text = GetText(current);
                }
                pointer.gameObject.SetActive(false);
                break;
            case 3:
                currentPointer = 4;
                pointer.gameObject.SetActive(false);
                Invoke("NextPointer", .5f);
                SC.INS.PlaySound(1, 1, 1);
                blocks[0].SetActive(true);
                blocks[3].SetActive(false);
                break;
            case 4:
                currentPointer = 5;
                pointer.gameObject.SetActive(false);
                Invoke("NextPointer", .5f);
                SC.INS.PlaySound(1, 1, 1);
                blocks[0].SetActive(false);
                blocks[1].SetActive(true);
                blocks[3].SetActive(true);
                break;
            case 5:
                currentPointer = 6;
                pointer.gameObject.SetActive(false);
                Invoke("NextPointer", .5f);
                SC.INS.PlaySound(1, 1, 1);
                blocks[1].SetActive(false);
                blocks[2].SetActive(true);
                break;
            case 6:
                anim.SetTrigger("3o");
                currentPointer = 7;
                pointer.gameObject.SetActive(false);
                Invoke("NextPointer", 1.5f);
                currentAnim = "4";
                Invoke("SetAnim", 1f);
                msgBox.SetActive(false);
                blocks[2].SetActive(false);
              
                break;
            case 7:
                currentPointer = 8;
                pointer.gameObject.SetActive(false);
                Invoke("NextPointer", .5f);
                SC.INS.PlaySound(1, 1, 1);
                break;
            case 8:
                pointer.gameObject.SetActive(false);
                msgCheck.SetActive(true);
                SC.INS.PlaySound(1, 1, 1);
                break;
            case 9:
                SC.INS.PlaySound(1, 1, 1);
                GC.INS.InstantiateCostumerGoing();
                break;
            case 10:
                anim.SetTrigger("4o");
                msgCheck.SetActive(false);
                msgBox.SetActive(false);
                GC.INS.ClosePhone();
                GC.INS.CloseCustomization();
                GC.INS.CloseShop();
                break;
            case 11:
                shopButtons[1].SetActive(true);
                currentPointer = 9;
                
                pointer.gameObject.SetActive(false);
                Invoke("NextPointer", .5f);
                SC.INS.PlaySound(1, 1, 1);
                msgCheck.SetActive(false);
                break;
            case 12:
                currentPointer = 10;
                pointer.gameObject.SetActive(false);
                SC.INS.PlaySound(1, 1, 1);
                GC.INS.InstantiateCostumerGoing();
                blocks[4].SetActive(true);
                Invoke("NextPointer", .5f);
                break;
            case 13:
                pointer.gameObject.SetActive(false);
                SC.INS.PlaySound(1, 1, 1);
                msgCheck.SetActive(true);
                blocks[4].SetActive(false);
                break;
            case 14:
                msgCheck.SetActive(false);
                msgBox.SetActive(false);
                anim.SetTrigger("3o");
                currentAnim = "4";
                Invoke("SetAnim", 1f);
                GC.INS.roomsArrange.transform.GetComponentInChildren<Costumer>().ForceTutorial();
                Invoke("MSGDesactive", 6);
                break;
            case 15:
                if (IsInvoking("MSGDesactive"))
                    CancelInvoke("MSGDesactive");
                if (!msgBox.activeInHierarchy)
                    msgBox.SetActive(true);
                currentPointer = 3;
                Invoke("NextPointer", 1.5f);
                SC.INS.PlaySound(1, 1, 1);
                break;
            case 16:
                pointer.gameObject.SetActive(false);
                SC.INS.PlaySound(1, 1, 1);
                msgCheck.SetActive(true);
                sideButtons.SetActive(true);
                blocks[6].SetActive(false);
                break;
            case 17:
                SC.INS.PlaySound(1, 1, 1);
                currentPointer = 17;
                NextPointer();
                msgCheck.SetActive(false);
                blocks[3].SetActive(false);
                GC.INS.ClosePhone();
                GC.INS.CloseCustomization();
                GC.INS.CloseShop();
                break;
            case 18:
                msgCheck.SetActive(false);
                msgBox.SetActive(false);
                anim.SetTrigger("4o");
                pointer.gameObject.SetActive(false);
                GC.INS.ClosePhone();
                GC.INS.CloseCustomization();
                GC.INS.CloseShop();
                break;
            case 19:
                GC.INS.ClosePhone();
                GC.INS.CloseCustomization();
                GC.INS.CloseShop();

                currentAnim = "3";
                Invoke("SetAnim", 1.5f);
                currentPointer = 3;
                msgBox.SetActive(true);
                pointer.gameObject.SetActive(false);
                Invoke("NextPointer", 2f);
                SC.INS.PlaySound(1, 1, 1);
                break;
            case 20:
                currentPointer = 15;
                pointer.gameObject.SetActive(false);
                Invoke("NextPointer", .2f);
                SC.INS.PlaySound(1, 1, 1);
                blocks[7].SetActive(true);
                if (GC.INS.coins < 5000)
                    GC.INS.coins = 5000;
                break;
            case 21:
                blocksP.SetActive(false);
                msgCheck.SetActive(true);
                pointer.gameObject.SetActive(false);
                SC.INS.PlaySound(1, 1, 1);
                break;
            case 22:
                msgCheck.SetActive(true);
                SC.INS.PlaySound(1, 1, 1);
                break;
            case 23:
                msgBox.SetActive(false);
                anim.SetTrigger("3o");
                FinishTutorial();
                break;
        }
    }
    void FinishTutorial()
    {
        GC.INS.dm.WelcomeGift();
        GC.INS.tutoOn = false;
        GC.INS.SaveFromBtn();
        transform.parent.gameObject.SetActive(false);
    }
    public void ContinueTutorial()
    {
        if (!GC.INS.tutoOn && current >= 22)
            return;
        if (current > 4 && current < 11)
        {
            currentAnim = "3";
            Invoke("SetAnim", .5f);
            current = 10;
            msgCheck.SetActive(true);
        } else if (current >= 18)
        {
            currentAnim = "3";
            Invoke("SetAnim", .2f);
            currentPointer = 13;
            Invoke("NextPointer", 1.5f);
            sideButtons.SetActive(true);
            for (int i = 0; i < shopButtons.Length; i++)
                shopButtons[i].SetActive(true);
        }

    }

    void SetAnim()
    {
        SC.INS.PlaySound(1, 1, 1);
        anim.SetTrigger(currentAnim);
        msgBox.SetActive(true);
    }
    void MSGDesactive()
    {
        msgBox.SetActive(false);
    }
    string GetText(int x)
    {
        return GC.INS.t.GetText(x + 82);
       /* switch (x)
        {
            default:
                return "";
            case 0:
                return "Welcome to Pocket Hotel, i will help you build a succesful hotel!";
            case 1:
                return "First, set your hotel name and username";
            case 2:
                return "Nice!, now lets build some rooms";
            case 3:
                return "To keep the shop open, click on the lock button";
            case 4:
                return "Good, now drag 3 rooms and add them to your hotel!";
            case 5:
                return "Perfect!, click here to finish building";
            case 6:
                return "Your hotel is closed right now!, Click here to start a shift";
            case 7:
                return "You can hire friends or purchase staff outfits to reduce wages";
            case 8:
                return "Congratulations! Your hotel is open for business and will keep runing even after you log off";
            case 9:
                return "Keep your hotel running so you keep receiving money!, Well, see you soon.";
            case 10:
                return "You leveled up, huh? Apparently you're serious about this";
            case 11:
                return "Lets get some decoration so you can improve your stars";
            case 12:
                return "Drag into the room, you can decorate only in the rooms that do not black out";
            case 13:
                return "More stars means more visitors, more visitors means more money!";
            case 14:
                return "Sometimes you have to fix room issues, follow the tips to learn how!";
            case 15:
                return "You are the man, you figure it out!, to help you fix, you can build staff rooms";
            case 16:
                return "Connect with facebook to get friends!\n\n";
            case 17:
                return "You can visit friends to get daily bonuses or hire them to reduce wages!, and of course brag.";
            case 18:
                return "Your hotel is getting small, here you can increase your hotel's maximum size";
            case 19:
                return "Oops, a guest would have liked to use a gym, but you didn't have one";
            case 20:
                return "You can find gyms here!";
            case 21:
                return "Incredible, you are ready to run your own hotel! I'll keep an eye on what you can do";
            case 22:
                return "Dont forget to support us on facebook to get special rewards! See ya!";

        }*/
    }

    void NextPointer()
    {
        pointer.gameObject.SetActive(true);
        if (pointer.parent != transform)
            pointer.SetParent(transform);
        arrowPointer.SetActive(true);
        fingerPointer.SetActive(false);
        switch (currentPointer)
        {
            case 0:
                pointer.anchoredPosition = new Vector2(0, 300);
                pointer.eulerAngles = new Vector3(0, 0, 180);
                break;
            case 1:
                pointer.anchoredPosition = new Vector2(-50, 130);
                pointer.eulerAngles = new Vector3(0, 0, 180);
                break;
            case 2:
                pointer.anchoredPosition = new Vector2(50, -160);
                pointer.eulerAngles = new Vector3(0, 0, 270);
                break;
            case 3:
                pointer.SetParent(shopButtons[0].transform);
                pointer.anchoredPosition = new Vector2(0, 75);
                pointer.eulerAngles = new Vector3(0, 0, 180);
                break;
            case 4:
                pointer.SetParent(pointerParents[0].transform);
                pointer.anchoredPosition = new Vector2(-80, 0);
                pointer.eulerAngles = new Vector3(0, 0, 270);
                break;
            case 5:
                pointer.SetParent(pointerParents[1].transform);
                pointer.anchoredPosition = new Vector2(10, -30);
                arrowPointer.SetActive(false);
                fingerPointer.SetActive(true);
                pointer.eulerAngles = new Vector3(0, 0, 270);
                break;
            case 6:
                pointer.SetParent(pointerParents[2].transform);
                pointer.anchoredPosition = new Vector2(-80, 0);
                pointer.eulerAngles = new Vector3(0, 0, 270);
                break;
            case 7:
                pointer.SetParent(shiftBtn.transform);
                pointer.anchoredPosition = new Vector2(0, 75);
                pointer.eulerAngles = new Vector3(0, 0, 180);
                break;
            case 8:
                pointer.anchoredPosition = new Vector2(130, 170);
                pointer.eulerAngles = new Vector3(0, 0, 180);
                break;
            case 9:
                pointer.SetParent(shopButtons[1].transform);
                pointer.anchoredPosition = new Vector2(0, 75);
                pointer.eulerAngles = new Vector3(0, 0, 180);
                break;
            case 10:
                pointer.SetParent(pointerParents[3].transform);
                pointer.anchoredPosition = new Vector2(10, -30);
                pointer.eulerAngles = new Vector3(0, 0, 270);
                arrowPointer.SetActive(false);
                fingerPointer.SetActive(true);
                break;
            case 11:
                pointer.SetParent(pointerParents[4].transform);
                pointer.anchoredPosition = new Vector2(-90, 0);
                pointer.eulerAngles = new Vector3(0, 0, 270);
                break;
            case 12:
                pointer.SetParent(pointerParents[5].transform);
                pointer.anchoredPosition = new Vector2(10, -30);
                arrowPointer.SetActive(false);
                fingerPointer.SetActive(true);
                pointer.eulerAngles = new Vector3(0, 0, 270);
                break;
            case 13:
                pointer.SetParent(shopButtons[2].transform);
                pointer.anchoredPosition = new Vector2(0, 75);
                pointer.eulerAngles = new Vector3(0, 0, 180);
                break;
            case 14:
                pointer.SetParent(pointerParents[7].transform);
                pointer.anchoredPosition = new Vector2(-150, 0);
                pointer.eulerAngles = new Vector3(0, 0, 270);
                break;
            case 15:
                pointer.SetParent(pointerParents[8].transform);
                pointer.anchoredPosition = new Vector2(-60, 0);
                pointer.eulerAngles = new Vector3(0, 0, 270);
                break;
            case 16:
                pointer.SetParent(pointerParents[9].transform);
                pointer.anchoredPosition = new Vector2(10, -30);
                arrowPointer.SetActive(false);
                fingerPointer.SetActive(true);
                pointer.eulerAngles = new Vector3(0, 0, 270);
                break;
            case 17:
                pointer.SetParent(pointerParents[6].transform);
                pointer.anchoredPosition = new Vector2(80, 0);
                pointer.eulerAngles = new Vector3(0, 0, 90);
                break;
            case 18:
                pointer.SetParent(FRC.INS.tutobtn.transform);
                pointer.anchoredPosition = new Vector2(0, -80);
                pointer.eulerAngles = new Vector3(0, 0, 0);
                break;
        }
    }
    public void FinishHotel()
    {
        currentPointer = 1;
        NextPointer();
        txt.text = GC.INS.t.GetText(105);
    }
    public void FinishUsername()
    {
        currentPointer = 2;
        NextPointer();
    }
    public void OpenShop()
    {
        current = 2;
        Next();
    }
    public void LockShop()
    {
        current = 3;
        Next();
    }
    public void AddRoom()
    {
        roomCount++;
        pointer.gameObject.SetActive(true);
        if (roomCount >= 3)
        {
            current = 4;
            Next();
        }
    }
    public void PointerOff()
    {
        pointer.gameObject.SetActive(false);
    }
    public void OpenShopFix()
    {
        currentPointer = 11;
        pointer.gameObject.SetActive(false);
        blocks[5].SetActive(true);
        Invoke("NextPointer", .5f);
    }
    public void OpenSubShopFix()
    {
        currentPointer = 12;
        blocks[5].SetActive(false);
        blocks[6].SetActive(true);
        pointer.gameObject.SetActive(false);
        Invoke("NextPointer", .5f);
    }
    public void OpenShopExpand()
    {
        currentPointer = 14;
        
        pointer.gameObject.SetActive(false);
        Invoke("NextPointer", .5f);
    }
    public void PurchaseExpand()
    {
        msgCheck.SetActive(false);
        msgBox.SetActive(false);
        pointer.gameObject.SetActive(false);
        anim.SetTrigger("3o");
        Invoke("GetCharacterComplaint", 5);
    }
    void GetCharacterComplaint()
    {
        Costumer cost = GC.INS.roomsArrange.transform.GetComponentInChildren<Costumer>();
        if (cost)
        {
            cost.ForceTutorial();
        }
        else
        {
            Invoke("GetCharacterComplaint", 3);
        }
    }
    public void OpenSubShopGym()
    {
        currentPointer = 16;
        pointer.gameObject.SetActive(false);
        Invoke("NextPointer", .5f);
        blocks[7].SetActive(false);
        blocks[8].SetActive(true);
    }

    public void OpenFriends()
    {
        currentPointer = 18;
        pointer.gameObject.SetActive(false);
        Invoke("NextPointer", .5f);
    }
    public void VisitFriend()
    {
        pointer.SetParent(transform);
        FRC.INS.DestroyTutoBtn();
        Next();
    }
}
