using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BackButton : MonoBehaviour
{
    public int[] itemsId;
    List<int> itemsLeft = new List<int>();
    public GameObject[] itemsNeeded;
    public Animator[] itemsAnim; 
    public int id;
    public GameObject itemsC, check;
    CloudsC bc;
    bool init;
    // Start is called before the first frame update
    void Awake()
    {
        if(!init)
        Init();
    }
    private void Init()
    {
        init = true;
        bc = GC.INS.backController;
        if (bc.backsUnlocked[id])
        {
            itemsC.SetActive(false);
            check.SetActive(true);
            return;
        }
        itemsLeft.AddRange(itemsId);
        for (int i = 0; i < itemsId.Length; i++)
        {
            if (bc.itemsList.Contains(itemsId[i]))
            {
                if (itemsLeft.Contains(itemsId[i]))
                {
                    itemsLeft.Remove(itemsId[i]);
                    itemsNeeded[i].SetActive(true);
                }

            }
        }
    }
    public void Click()
    {
        if (bc.backsUnlocked[id])
        {
            bc.SetBack(id);
        }
        else
        {
            if (itemsLeft.Count <= 0)
            {
                bc.backsUnlocked[id] = true;
      
                for (int i = 0; i < itemsId.Length; i++)
                {
                    bc.itemsList.Remove(itemsId[i]);
                }
                itemsC.SetActive(false);
                check.SetActive(true);
                bc.SetBack(id);
                bc.RemovedItem();
            }
            else
            {
                GC.INS.errorM.Error(5);
            }
        }
        
    }
   
    public void RemovedItem()
    {
        for (int i = 0; i < itemsId.Length; i++)
        {
            if (itemsNeeded[i].activeInHierarchy)
            {
                if (!bc.itemsList.Contains(itemsId[i]))
                {
                    itemsLeft.Add(itemsId[i]);
                    itemsNeeded[i].SetActive(false);
                }
            }
        }
    }
    public void PickedItem(int id)
    {
        if (!init)
            Init();
        if (itemsLeft.Count <= 0 || bc.backsUnlocked[this.id])
            return;
        for (int i = 0; i < itemsId.Length; i++)
        {
            
            if (itemsId[i]==id)
            {
                if (itemsLeft.Contains(itemsId[i]))
                {
                    itemsLeft.Remove(itemsId[i]);
                    itemsNeeded[i].SetActive(true);
                    itemsAnim[i].enabled = true;
                }
            }
        }
    }

    private void OnEnable()
    {
        if (!bc || bc.backsUnlocked[id])
            return;
        Invoke("DisableAnim", 3f);
       
    }
    void DisableAnim()
    {
        for (int i = 0; i < itemsAnim.Length; i++)
        {
            if (itemsAnim[i].enabled)
            {
                itemsAnim[i].enabled = false;
            }
        }
    }
}
