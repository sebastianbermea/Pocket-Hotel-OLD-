using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IAP : MonoBehaviour
{
    public GameObject[] purchasedI, packs;

    private void Start()
    {
        for (int i = 0; i < purchasedI.Length; i++)
        {
            purchasedI[i].SetActive(GC.INS.iap[i]);
        }
    }
    public void OpenPackFromOther(int x)
    {
        GC.INS.OpenPhone(2);
        currentP = x;
        Invoke("OpenP", .75f);
    }
    int currentP;
    void OpenP()
    {
        SC.INS.PlaySound(0, 17, 0);
        packs[currentP].SetActive(true);
    }
    public void OpenPack(int x)
    {
        SC.INS.PlaySound(0, 17, 0);
        packs[x].SetActive(true);
    }
    public void Purchase75gems()
    {
        GC.INS.gems += 75;
        GC.INS.gemsText.text = GC.INS.gems.ToString("n0");
        GC.INS.ad.NormalAnim();
        GC.INS.SaveFromBtn();
    }
    public void Purchase200gems()
    {
        GC.INS.gems += 200;
        GC.INS.gemsText.text = GC.INS.gems.ToString("n0");
        GC.INS.ad.NormalAnim();
        GC.INS.SaveFromBtn();
    }
    public void Purchase500gems()
    {
        GC.INS.gems += 500;
        GC.INS.gemsText.text = GC.INS.gems.ToString("n0");
        GC.INS.ad.NormalAnim();
        GC.INS.SaveFromBtn();
    }
    public void Purchase2kgems()
    {
        GC.INS.gems += 2000;
        GC.INS.gemsText.text = GC.INS.gems.ToString("n0");
        GC.INS.ad.NormalAnim();
        GC.INS.SaveFromBtn();
    }
    public void Purchase5kgems()
    {
        GC.INS.gems += 5000;
        GC.INS.gemsText.text = GC.INS.gems.ToString("n0");
        GC.INS.ad.NormalAnim();
        GC.INS.SaveFromBtn();
    }
    public void Purchase10kgems()
    {
        GC.INS.gems += 10000;
        GC.INS.gemsText.text = GC.INS.gems.ToString("n0");
        GC.INS.ad.NormalAnim();
        GC.INS.SaveFromBtn();
    }
    public void Purchase15kcoins()
    {
        GC.INS.AddCoins(15000);
        GC.INS.ad.CoinAnim();
        GC.INS.SaveFromBtn();
    }
    public void Purchase40kcoins()
    {
        GC.INS.AddCoins(40000);
        GC.INS.ad.CoinAnim();
        GC.INS.SaveFromBtn();
    }
    public void Purchase100kcoins()
    {
        GC.INS.AddCoins(100000);
        GC.INS.ad.CoinAnim();
    }
    public void Purchase400kcoins()
    {
        GC.INS.AddCoins(400000);
        GC.INS.ad.CoinAnim();
        GC.INS.SaveFromBtn();
    }
    public void Purchase1mcoins()
    {
        GC.INS.AddCoins(1000000);
        GC.INS.ad.CoinAnim();
        GC.INS.SaveFromBtn();
    }
    public void Purchase2mcoins()
    {
        GC.INS.AddCoins(2000000);
        GC.INS.ad.CoinAnim();
        GC.INS.SaveFromBtn();
    }
    public void PurchaseX2Coins()
    {
        if (GC.INS.iap[0])
        {
            GC.INS.errorM.Error(11);
            return;
        }
        GC.INS.iap[0] = true;
        purchasedI[0].SetActive(true);
        GC.INS.ad.CoinAnim();
        GC.INS.x2Image.SetActive(true);
        GC.INS.boostFill.fillAmount = 1;
        GC.INS.boostTimeText.text = "∞";
        GC.INS.SaveFromBtn();
    }
    public void PurchaseX2Speed()
    {
        if (GC.INS.iap[1])
        {
            GC.INS.errorM.Error(11);
            return;
        }
        GC.INS.iap[1] = true;
        purchasedI[1].SetActive(true);
        GC.INS.ad.NormalAnim();
        GC.INS.SaveFromBtn();
        SceneManager.LoadScene("SampleScene");
    }
    public void PurchaseX2Shift()
    {
        if (GC.INS.iap[2])
        {
            GC.INS.errorM.Error(11);
            return;
        }
        GC.INS.iap[2] = true;
        purchasedI[2].SetActive(true);
        GC.INS.shiftX2I.SetActive(true);
        GC.INS.ad.NormalAnim();
        GC.INS.SaveFromBtn();
        SceneManager.LoadScene("SampleScene");
    }
    public void PurchaseStarterPack()
    {
        if (GC.INS.iap[3])
        {
            GC.INS.errorM.Error(11);
            return;
        }
        GC.INS.iap[3] = true;
        purchasedI[3].SetActive(true);
        GC.INS.gems += 100;
        GC.INS.gemsText.text = GC.INS.gems.ToString("n0");
        GC.INS.ad.NormalAnim();
        GC.INS.AddCoins(50000);
        GC.INS.gift.AddGift(new Dictionary<string, object>
                            {
                                { "id", 16},
                                { "type",2},
                                { "subtype", 0},
                     });
        GC.INS.gift.AddGift(new Dictionary<string, object>
                            {
                                { "id", 30},
                                { "type",0},
                                { "subtype", 3},
                     });
        GC.INS.SaveFromBtn();
    }
    public void PurchasePremiumPack()
    {
        if (GC.INS.iap[4])
        {
            GC.INS.errorM.Error(11);
            return;
        }
        GC.INS.iap[4] = true;
        purchasedI[4].SetActive(true);
        GC.INS.gems += 1000;
        GC.INS.gemsText.text = GC.INS.gems.ToString("n0");
        GC.INS.ad.NormalAnim();
        GC.INS.AddCoins(100000);
        GC.INS.SaveFromBtn();
    }
    public void PurchaseX2pack()
    {
        if (GC.INS.iap[5])
        {
            GC.INS.errorM.Error(11);
            return;
        }
        GC.INS.iap[5] = true;
        purchasedI[5].SetActive(true);
        GC.INS.iap[1] = true;
        purchasedI[1].SetActive(true);
        GC.INS.iap[2] = true;
        purchasedI[2].SetActive(true);
        GC.INS.gems += 1000;
        GC.INS.gemsText.text = GC.INS.gems.ToString("n0");
        GC.INS.ad.NormalAnim();
        GC.INS.AddCoins(75000);
        GC.INS.shiftX2I.SetActive(true);
        GC.INS.SaveFromBtn();
        SceneManager.LoadScene("SampleScene");
    }

}
