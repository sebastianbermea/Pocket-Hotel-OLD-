using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinBag : MonoBehaviour
{
    public GameObject sprte;
    public GameObject[] coins;
    int cointCount, addedCoins;
    public bool tip;
    // Start is called before the first frame update
    void Start()
    {
        if (GC.INS.tutoOn)
        {
            addedCoins = 1000;
            return;
        }else if ((DateTime.UtcNow - GC.INS.lastFriendVisit[FRC.INS.visitNumber]).TotalDays > 11)
        {
            addedCoins = 1000;
        }
        else
        {
            if(tip)
                addedCoins = 500 + UnityEngine.Random.Range(0,6)*100;
            else
                addedCoins = 500 + 100 * (int)GC.INS.stars;
        }
    }


    private void OnMouseDown()
    {
        if (!sprte.activeInHierarchy)
            return;
        sprte.SetActive(false);
        VC.INS.AddCoins(addedCoins, transform.position);
        InvokeRepeating("CoinActive",0f, .2f);
        if (GC.INS.tutoOn)
        {
            VC.INS.TutoNext();
            return;
        }
        GC.INS.lastFriendVisit[FRC.INS.visitNumber] = DateTime.UtcNow;
        GC.INS.AddXp(50);
    }
    void CoinActive()
    {
        coins[cointCount].SetActive(true);
        coins[cointCount].transform.parent = null;
        cointCount++;
        if (cointCount >= coins.Length)
        {
            CancelInvoke("CoinActive");
            Destroy(gameObject);
        }
    }
}
