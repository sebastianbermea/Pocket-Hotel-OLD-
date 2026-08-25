using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DailyTile : MonoBehaviour
{
    int id, goal, left, number;
    public Text description, countText, coinsText;
    bool completed, added;
    public Image btnI, fill;
    public GameObject minusText;

    public void Create(int number, int id, int goal, int left)
    {
        this.id = id;
        this.goal = goal;
        this.left = left;
        this.number = number;
        added = false;
        description.text = GetText(id, goal);
        countText.text = left + "/" + goal;
        coinsText.text = ((int)(goal * DailyMisions.rewards[id])).ToString();
        if (left >= goal)
        {
            Completed();
            if (left > (goal + 2))
                added = true;
            if (number == 0 && left > (goal + 5))
                GC.INS.dm.XpAdded();
            btnI.gameObject.SetActive(!added);
        }
        else
        {
            fill.fillAmount = (left * 1f) / (goal * 1f);
            btnI.color = new Color(0.79f, 0.84f,0.86f);
        }
    }

    string GetText(int id, int goal)
    {
        string tempT = "";
        switch (id)
        {
            case 0:
                tempT = GC.INS.t.GetText(27) + goal + GC.INS.t.GetText(30);
                break;
            case 1:
                tempT = GC.INS.t.GetText(28) + goal + GC.INS.t.GetText(30);
                break;
            case 2:
                tempT = GC.INS.t.GetText(31) + goal + GC.INS.t.GetText(32);
                break;
            case 3:
                tempT = GC.INS.t.GetText(27) + goal + GC.INS.t.GetText(33);
                break;
            case 4:
                tempT = GC.INS.t.GetText(34) + goal + GC.INS.t.GetText(35);
                break;
            case 5:
                tempT = GC.INS.t.GetText(27) + goal + GC.INS.t.GetText(36);
                break;
            case 6:
                tempT = GC.INS.t.GetText(37) + goal + GC.INS.t.GetText(38);
                break;
            case 7:
                tempT = GC.INS.t.GetText(39) + goal + GC.INS.t.GetText((goal>1)?40:41);
                break;
            case 8:
                tempT = GC.INS.t.GetText(42) + goal + GC.INS.t.GetText((goal > 1) ? 30 : 29);
                break;
            case 9:
                tempT = GC.INS.t.GetText(42) + goal + GC.INS.t.GetText((goal > 1) ? 43 : 44);
                break;
            case 10:
                tempT = GC.INS.t.GetText(42) + goal + GC.INS.t.GetText((goal > 1) ? 45 : 46);
                break;
            case 11:
                tempT = GC.INS.t.GetText(42) + goal + GC.INS.t.GetText((goal > 1) ? 47 : 48);
                break;
            case 12:
                tempT = GC.INS.t.GetText(42) + goal + GC.INS.t.GetText((goal > 1) ? 49 : 50);
                break;
            case 13:
                tempT = GC.INS.t.GetText(42) + goal + GC.INS.t.GetText((goal > 1) ? 51 : 52);
                break;
            case 14:
                tempT = GC.INS.t.GetText(42) + goal + GC.INS.t.GetText((goal > 1) ? 53 : 54);
                break;
            case 15:
                tempT = GC.INS.t.GetText(42) + goal + GC.INS.t.GetText((goal > 1) ? 55 : 56);
                break;
            case 16:
                tempT = GC.INS.t.GetText(57) + goal + GC.INS.t.GetText(58);
                break;
            case 17:
                tempT = GC.INS.t.GetText(57) + goal + GC.INS.t.GetText(59);
                break;
            case 18:
                tempT = GC.INS.t.GetText(42) + goal + GC.INS.t.GetText((goal > 1) ? 60 : 61);
                break;
            case 19:
                tempT = GC.INS.t.GetText(64) + goal + GC.INS.t.GetText((goal > 1) ? 62 : 63);
                break;
            case 20:
                tempT = GC.INS.t.GetText(65) + goal + GC.INS.t.GetText(66);
                break;
            case 21:
                tempT = GC.INS.t.GetText(42) + goal + GC.INS.t.GetText(67);
                break;
            case 22:
                tempT = GC.INS.t.GetText(42) + goal + GC.INS.t.GetText((goal > 1) ? 68 : 69);
                break;
            case 23:
                tempT = GC.INS.t.GetText(42) + goal + GC.INS.t.GetText((goal > 1) ? 70 : 71);
                break;
            case 24:
                tempT = GC.INS.t.GetText(72) + goal + GC.INS.t.GetText(75);
                break;
            case 25:
                tempT = GC.INS.t.GetText(73) + goal + GC.INS.t.GetText(76);
                break;
            case 26:
                tempT = GC.INS.t.GetText(74) + goal + GC.INS.t.GetText(77);
                break;
            case 27:
                tempT = GC.INS.t.GetText(42) + goal + GC.INS.t.GetText(78);
                break;
            case 28:
                tempT = GC.INS.t.GetText(79) + goal + GC.INS.t.GetText((goal > 1) ? 80 : 81);
                break;
        }
        return tempT;
    }

    public void AddTask(int quant)
    {
        if (goal > quant)
        {
            fill.fillAmount = (quant * 1f) / (goal * 1f);
            countText.text = quant + "/" + goal;
        }
        else
            Completed();
    }
    public void Completed()
    {
        completed = true;
        countText.text = goal + "/" + goal;
        fill.fillAmount = 1;
        btnI.color = new Color(0.4f, 0.8f, 1f);
    }
    public void AddCoins()
    {
        if (!completed || added)
            return;
        added = true;
        GC.INS.AddCoins((int)(goal * DailyMisions.rewards[id]));
        btnI.gameObject.SetActive(false);
        GC.INS.dm.CoinsAdded(number);
        GC.INS.AddXp(4);
        Text tempText = Instantiate(minusText, transform.parent.parent.parent).GetComponentInChildren<Text>();
        tempText.transform.parent.position = btnI.transform.position;
        tempText.text = "+" + ((int)(goal * DailyMisions.rewards[id])).ToString("n0");
    }
    public void Help()
    {
        GC.INS.dm.Help(id);
    }
}
