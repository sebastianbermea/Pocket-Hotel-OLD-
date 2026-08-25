using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemButton : MonoBehaviour
{
    public int id, cost;
    public Text coinText;
    public Image invIm;
    int inv;
    public void ButtonClick()
    {
        if (!GC.INS.visit)
        {
            if (GC.INS.gems >= cost)
            {
                GC.INS.Purchase(-cost);
                GC.INS.backController.AddItem(id);
            }
            else
                GC.INS.errorM.Error(1);
        }
    }
    public void Gift()
    {
        if (GC.INS.gems >= cost)
        {
            if (inv > 0)
                VC.INS.BuyItem(id, cost, this);
            else
                VC.INS.BuyItem(id, cost, null);
        }
        else
            VC.INS.errorM.Error(1);
    }
    public void Add()
    {
        inv++;
        invIm.gameObject.SetActive(true);
        invIm.GetComponentInChildren<Text>().text = inv.ToString();
    }
    public void Gifted()
    {
        if (inv > 0)
        {
            Debug.Log("Remove");
            inv--;
            GC.INS.backController.RemoveItem(id);
        }
        
        if (inv > 0)
        {
            invIm.gameObject.SetActive(true);
            invIm.GetComponentInChildren<Text>().text = inv.ToString();
        }
        
    }
    private void Start()
    {
        coinText.text = cost.ToString("n0");
        inv = 0;
        invIm.gameObject.SetActive(false);
        for (int i = 0; i < GC.INS.backController.itemsList.Count; i++)
        {
            if (GC.INS.backController.itemsList[i] == id)
                inv++;
        }
        if (inv > 0)
        {
            invIm.gameObject.SetActive(true);
            invIm.GetComponentInChildren<Text>().text = inv.ToString();
        }

    }
    private void OnEnable()
    {
        if (GC.INS.gems >= cost)
        {
            coinText.color = new Color(0.3f, 0.7f, 1);
        }
        else
        {
            coinText.color = new Color(0.8f, 0.8f, 0.8f);
        }

    }
}
