using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CutBtn : MonoBehaviour
{
    public int type, id;
    public Text coinText;
    public CharacterCustomize character;
    public GameObject minusText, dot;
    Image colorI;
    private void Start()
    {
        colorI = GetComponent<Image>();
        if (GC.INS.customPurchased[type][id])
        {
            coinText.gameObject.SetActive(false);
            switch (type)
            {
                case 0:
                    if (GC.INS.player.skinColor == id)
                    {
                        colorI.color = new Color(0.6f, 1, 0.5f);
                        GC.INS.currentBtnCus[type] = colorI;
                    }
                    else
                    {
                        colorI.color = new Color(0.73f, 0.73f, 0.73f);
                    }
                    break;
                case 1:
                    if (GC.INS.player.outfitId == id)
                    {
                        colorI.color = new Color(0.6f, 1, 0.5f);
                        GC.INS.currentBtnCus[type] = colorI;
                    }
                    else
                    {
                        colorI.color = new Color(0.73f, 0.73f, 0.73f);
                    }
                    break;
                case 2:
                    if (GC.INS.player.mouthId == id)
                    {
                        colorI.color = new Color(0.6f, 1, 0.5f);
                        GC.INS.currentBtnCus[type] = colorI;
                    }
                    else
                    {
                        colorI.color = new Color(0.73f, 0.73f, 0.73f);
                    }
                    break;
                case 3:
                    if (GC.INS.player.extraId == id)
                    {
                        colorI.color = new Color(0.6f, 1, 0.5f);
                        GC.INS.currentBtnCus[type] = colorI;
                    }
                    else
                    {
                        colorI.color = new Color(0.73f, 0.73f, 0.73f);
                    }
                    break;
                case 4:
                    if (GC.INS.player.extraColor == id)
                    {
                        colorI.color = new Color(0.6f, 1, 0.5f);
                        GC.INS.currentBtnCus[type] = colorI;
                    }
                    else
                    {
                        colorI.color = new Color(0.73f, 0.73f, 0.73f);
                    }
                    break;
                case 5:
                    if (GC.INS.player.eyesId == id)
                    {
                        colorI.color = new Color(0.6f, 1, 0.5f);
                        GC.INS.currentBtnCus[type] = colorI;
                    }
                    else
                    {
                        colorI.color = new Color(0.73f, 0.73f, 0.73f);
                    }
                    break;
                case 6:
                    if (GC.INS.player.eyeColor == id)
                    {
                        colorI.color = new Color(0.6f, 1, 0.5f);
                        GC.INS.currentBtnCus[type] = colorI;
                    }
                    else
                    {
                        colorI.color = new Color(0.73f, 0.73f, 0.73f);
                    }
                    break;
                case 7:
                    if (GC.INS.player.glassId == id)
                    {
                        colorI.color = new Color(0.6f, 1, 0.5f);
                        GC.INS.currentBtnCus[type] = colorI;
                    }
                    else
                    {
                        colorI.color = new Color(0.73f, 0.73f, 0.73f);
                    }
                    break;
                case 8:
                    if (GC.INS.player.glassColorId == id)
                    {
                        colorI.color = new Color(0.6f, 1, 0.5f);
                        GC.INS.currentBtnCus[type] = colorI;
                    }
                    else
                    {
                        colorI.color = new Color(0.73f, 0.73f, 0.73f);
                    }
                    break;
                case 9:
                    if (GC.INS.player.glassColor == id)
                    {
                        colorI.color = new Color(0.6f, 1, 0.5f);
                        GC.INS.currentBtnCus[type] = colorI;
                    }
                    else
                    {
                        colorI.color = new Color(0.73f, 0.73f, 0.73f);
                    }
                    break;
                case 10:
                    if (GC.INS.player.hairId == id)
                    {
                        colorI.color = new Color(0.6f, 1, 0.5f);
                        GC.INS.currentBtnCus[type] = colorI;
                    }
                    else
                    {
                        colorI.color = new Color(0.73f, 0.73f, 0.73f);
                    }
                    break;
                case 11:
                    if (GC.INS.player.hairColor == id)
                    {
                        colorI.color = new Color(0.6f, 1, 0.5f);
                        GC.INS.currentBtnCus[type] = colorI;
                    }
                    else
                    {
                        colorI.color = new Color(0.73f, 0.73f, 0.73f);
                    }
                    break;
                

            }
        }
           
        if (CharacterCustomize.costs[type, id] > 0)
        {
            coinText.text = CharacterCustomize.costs[type, id].ToString("n0");
        }
        else
        {
            int temp = CharacterCustomize.costs[type, id] * -1;
            coinText.text = temp.ToString("n0");
        }
    }
    private void OnEnable()
    {
        if (CharacterCustomize.costs[type, id] > 0)
        {
            if (GC.INS.coins >= CharacterCustomize.costs[type, id])
            {
                coinText.color = new Color(1, 1, .38f);
            }
            else
            {
                coinText.color = new Color(0.8f, 0.8f, 0.8f);
            }

        }
        else
        {
            if (GC.INS.gems >= CharacterCustomize.costs[type, id] * -1)
            {
                coinText.color = new Color(0.3f, 0.7f, 1);
            }
            else
            {
                coinText.color = new Color(0.8f, 0.8f, 0.8f);
            }
        }
        if (GC.INS.gift.custGifts.Count > 0)
        {
            for(int i=0; i< GC.INS.gift.custGifts.Count; i++)
            {
                if (GC.INS.gift.custGifts[i].id == id && GC.INS.gift.custGifts[i].subtype == type)
                    dot.SetActive(true);
            }
        }
        if (GC.INS.customPurchased[type][id])
        {
            coinText.gameObject.SetActive(false);
        }
    }
    public void OnClick()
    {
        if (GC.INS.customPurchased[type][id])
        {
            character.Customize(type, id);
            GC.INS.customized = true;
            GC.INS.currentBtnCus[type].color = new Color(0.73f, 0.73f, 0.73f);
            colorI.color = new Color(0.6f, 1, 0.5f);
            GC.INS.currentBtnCus[type] = colorI;
        }
        else if (GC.INS.coins >= CharacterCustomize.costs[type, id] && CharacterCustomize.costs[type, id] >= 0 || 
            GC.INS.gems >= (CharacterCustomize.costs[type, id] * -1) && CharacterCustomize.costs[type, id] < 0)
        {
            GC.INS.currentBtnCus[type].color = new Color(0.73f, 0.73f, 0.73f);
            colorI.color = new Color(0.6f, 1, 0.5f);
            GC.INS.currentBtnCus[type] = colorI;
            character.Customize(type, id);
            GC.INS.Purchase(CharacterCustomize.costs[type, id]);
            GC.INS.dm.AddTask(21, 1);
            if(type==1)
                GC.INS.dm.AddTask(22, 1);

            if(type==0 || type == 4 || type == 6 || type == 8 || type == 9 || type == 11)
                GC.INS.dm.AddTask(23, 1);
            GC.INS.customPurchased[type][id] = true;
            GC.INS.customized = true;
            coinText.gameObject.SetActive(false);
            Text tempText = Instantiate(minusText, transform.parent.parent.parent).GetComponentInChildren<Text>();
            tempText.transform.parent.position = transform.position;
            if (CharacterCustomize.costs[type, id] < 0)
            {
                tempText.color = new Color(0.5f, 0.80f, 1f);
                tempText.text = CharacterCustomize.costs[type, id].ToString("n0");
                GC.INS.AddXp(-CharacterCustomize.costs[type, id]/5);
            }
            else
            {
                tempText.text = "-" + CharacterCustomize.costs[type, id].ToString("n0");
                GC.INS.AddXp(CharacterCustomize.costs[type, id] / 2000);
            }
           
        }
        else
        {
            if (CharacterCustomize.costs[type, id] > 0)
                GC.INS.errorM.Error(0);
            else
                GC.INS.errorM.Error(1);
        }
    }
}
