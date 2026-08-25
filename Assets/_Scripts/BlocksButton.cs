using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlocksButton : MonoBehaviour
{
    public int level, id, cost;
    int blocks;
    public GameObject check, lockI;
    public Text coinText, blocksText, levelT;
    public void Purchase()
    {
        if (level <= GC.INS.level && GC.INS.coins>=cost  && id > GC.INS.blocksId && (GC.INS.blocksId+1)==id)
        {
            GC.INS.PurchaseBlocks(id, cost);
            coinText.gameObject.SetActive(false);
            check.SetActive(true);
        }
        else
        {
            if (level > GC.INS.level)
            {
                GC.INS.errorM.Error(3);
            }
            if (GC.INS.coins < cost)
            {
                GC.INS.errorM.Error(0);
            }
            
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        blocks = GC.blocksPer[id];
        coinText.text = cost.ToString("n0");
        levelT.text = level.ToString();
        blocksText.text = blocks.ToString();
        if (blocks <= GC.INS.blocksPermited)
        {
            coinText.gameObject.SetActive(false);
            check.SetActive(true);
        }
        else
        {
            coinText.gameObject.SetActive(true);
            check.SetActive(false);
        }
    }
    private void OnEnable()
    {
        if (level > GC.INS.level)
        {
            lockI.SetActive(true);
        }
        else
        {
            lockI.SetActive(false);
        }
        if((GC.INS.blocksId + 1) < id && level <= GC.INS.level)
        {
            lockI.SetActive(true);
            levelT.enabled = false;
        }
       
        if (cost > 0)
        {
            if (GC.INS.coins >= cost)
            {
                coinText.color = new Color(1, 1, .38f);
            }
            else
            {
                coinText.color = new Color(0.8f, 0.8f, 0.8f);
            }

        }
        
    }
}
