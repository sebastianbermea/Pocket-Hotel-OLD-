using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AdController : MonoBehaviour
{
    public GameObject canvas, rewardAnim, note, button, moneyBtn, zepBtn, moneyP, zepP, rewardAnim2;
    public Text coinText;
    int coins;
    public int type = 0;
    public RectTransform plusRect;
    int times;
    public void SetReward(int x)
    {
        gameObject.SetActive(true);
        type = x;
        plusRect.anchoredPosition = new Vector2(43,150-GC.INS.prestige*50);
        switch (x)
        {
            case 0:
                if(times==0)
                    coins = (int)(Random.Range(750, 2500) + ((GC.INS.coins-10000)*0.03f)) + GC.INS.level*100;
                else if (times == 1)
                    coins = (int)(Random.Range(1000, 3000) + ((GC.INS.coins - 8000) * 0.03f)) + GC.INS.level * 100;
                else
                    coins = (int)(Random.Range(1500, 3500) + ((GC.INS.coins - 7000) * 0.03f)) + GC.INS.level * 100;
                coinText.text = coins.ToString("n0");
                moneyBtn.SetActive(true);
                zepBtn.SetActive(false);
                moneyP.SetActive(true);
                zepP.SetActive(false);
                break;
            case 1:
                moneyBtn.SetActive(false);
                zepBtn.SetActive(true);
                moneyP.SetActive(false);
                zepP.SetActive(true);
                break;
        }
    }
    public void SetNote()
    {
        button.SetActive(false);
        note.SetActive(true);
        SC.INS.PlaySound(0, 17, 0);
    }
    public void Claim()
    {
        GC.INS.ShowVideo(3);
    }
    public void Reward()
    {
        times = 0;
        if (type == 0)
            AddCoins();
        else
        {
            if (GC.INS.currentZep)
            {
                GC.INS.currentZep.Claim();
                GC.INS.currentZep = null;
                Close();
            }
        }
    }
    public void Close()
    {
        if (GC.INS.costumerReward)
        {
            times++;
            GC.INS.costumerReward.FinishVideoReward();
            GC.INS.costumerReward = null;
        }
        if (GC.INS.currentZep)
        {
            GC.INS.currentZep.Leave();
            GC.INS.currentZep = null;
        }
        GC.INS.SetLastAd();
        gameObject.SetActive(false);
        button.SetActive(true);
        note.SetActive(false);
    }
    public void AddCoins()
    {
        GC.INS.AddCoins(coins);
        CoinAnim();
        coins = 0;
        Close();
    }
    public void CoinAnim()
    {
        Instantiate(rewardAnim, canvas.transform);
        SC.INS.PlaySound(0, 15, 0);
    }
    public void NormalAnim()
    {
        Instantiate(rewardAnim2, canvas.transform);
        SC.INS.PlaySound(0, 15, 0);
    }
}
