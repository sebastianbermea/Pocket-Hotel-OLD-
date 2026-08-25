using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Prestige : MonoBehaviour
{
    public Image medal, medalTail;
    public Sprite[] medals, extraSprites, medalsTails;
    public Image starsI;
    public Color[] starColors;
    public GameObject parkingB, parking, prestigeB, prestigeStart, parkingCan, block;
    public Image medalImage, extraImage, purBtnI;
    public Text costText, bonusText, medalText, extraText, gemsText, prestigeNText, coinsBonusT, xpbonus;
    int cost;
    public TextMeshProUGUI levelT;
    public Parking par;
    public int coinBonus;
    bool animOpen;
    public void CheckPrestige(int coinsAv)
    {
        if (prestigeB.activeInHierarchy)
            return;
        int x = GC.INS.prestige;
        switch (x)
        {
            case 0:
                if (coinsAv > 1700)
                    prestigeB.SetActive(true);
                break;
            case 1:
                if (coinsAv > 5200)
                    prestigeB.SetActive(true);
                break;
            case 2:
                if (coinsAv > 8000)
                    prestigeB.SetActive(true);
                break;
            case 3:
                if (coinsAv > 15000)
                    prestigeB.SetActive(true);
                break;
            case 4:
                if (coinsAv > 20000)
                    prestigeB.SetActive(true);
                break;
        }
        if(prestigeB.activeInHierarchy && !animOpen)
        {
            animOpen = true;
            prestigeB.transform.GetChild(0).GetComponent<Animator>().SetTrigger("In");
        }
    }
    public void SetPrestige(int x)
    {
        Debug.Log("Set prestige");
        SetPanel();
        if (x > 0)
        {
            medal.sprite = medals[x-1];
            medal.gameObject.SetActive(true);
            medalTail.sprite = medalsTails[x -1];
            starsI.color = starColors[x-1];
            if (Fire.INS.prestige)
            {
                Fire.INS.prestige = false;
                Instantiate(prestigeStart, GC.INS.canvas[0].transform);
                SC.INS.PlaySound(0, 13, 1);
            }
        }
    }
    public void SetParking(List<object> levels)
    {
        parkingB.SetActive(true);
        parkingCan = Instantiate(parking, GC.INS.canvas[0].transform);
        parkingCan.transform.SetSiblingIndex(9);
        par = parkingCan.GetComponent<Parking>();
        if (levels != null)
        {
            List<int[]> tempList = new List<int[]>();
            for (int i = 0; i < levels.Count; i++)
            {
                Dictionary<string, object> tempDic = levels[i] as Dictionary<string, object>;
                int[] tempInt = new int[3];
                tempInt[0] = Convert.ToInt32(tempDic["up"]);
                tempInt[1] = Convert.ToInt32(tempDic["space"]);
                tempInt[2] = Convert.ToInt32(tempDic["valet"]);
                tempList.Add(tempInt);
            }
            par.Set(tempList);
            return;
        }
        par.Set(null);
        

    }
    public void OpenParkingPanel()
    {
        if(!GC.INS.CheckUI())
            parkingCan.SetActive(true);
    }
    public void Purchase()
    {
        if (cost > 0)
        {
            if (GC.INS.level>=GC.INS.prestige*8+16)
            {
                if (GC.INS.stars >= 4)
                {
                    if (GC.INS.coins >= cost)
                    {
                        GC.INS.Prestige();
                    }
                    else
                        GC.INS.errorM.Error(0);
                }else
                    GC.INS.errorM.Error(13);
            }
            else
                GC.INS.errorM.Error(3);


        }
    }
    public void OpenPanel()
    {
        if (cost > GC.INS.coins)
            purBtnI.color = new Color(0.75f,0.75f,0.8f);
        else
            purBtnI.color = new Color(0.1f,0.6f,1f);

        gameObject.SetActive(true);
        SC.INS.PlaySound(0, 17, 0);
        if (GC.INS.prestige > 0)
        {
            block.SetActive(true);
            purBtnI.gameObject.SetActive(false);

        }
        coinBonus = (int)(GC.INS.totalAverage * 200000);
        if (coinBonus > 100000 + GC.INS.prestige*50000)
        {
            coinBonus += (100000 + GC.INS.prestige * 50000 - coinBonus) / 2;
        }
        if (coinBonus > 200000 + GC.INS.prestige * 50000)
        {
            coinBonus += (int)((200000 + GC.INS.prestige * 50000 - coinBonus) / 1.25f);
        }
        
        coinsBonusT.text = "+"+coinBonus.ToString("n0") + GC.INS.t.GetText(66);
    }
    public void SetPanel()
    {
        int x = GC.INS.prestige;
        prestigeNText.text = (x+1).ToString();
        switch (x)
        {
            case 0:
                cost = 50000;
                gemsText.text = "600 GEMS";
                bonusText.text = "+20% FAST BONUS";
                extraText.text = "PARKING";
                break;
            case 1:
                cost = 100000;
                gemsText.text = "1250 GEMS";
                bonusText.text = "+40% FAST BONUS";
                extraText.text = "ROOM SERVICE";
                break;
            case 2:
                cost = 250000;
                gemsText.text = "2000 GEMS";
                bonusText.text = "+60% FAST BONUS";
                extraText.text = "2 HOTELS";
                break;
            case 3:
                cost = 500000;
                gemsText.text = "3000 GEMS";
                bonusText.text = "+80% FAST BONUS";
                extraText.text = "ACTIVITIES";
                break;
            case 4:
                cost = 1000000;
                gemsText.text = "5000 GEMS";
                bonusText.text = "+100% FAST BONUS";
                extraText.text = "HOTEL EMPIRE";
                break;
        }
        medalImage.sprite = medals[x];
        extraImage.sprite = extraSprites[x];
        medalText.text = "PRESTIGE " +x +" MEDAL";
        costText.text = cost.ToString("n0");
        levelT.text = (x *8 + 16).ToString();
        xpbonus.text = "xp" + GC.INS.t.GetText(142) + (x + 2).ToString();
    }
    public int[] gems =
    {
        600,1250,2000,3000,5000
    };
}
