using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ParkingO : MonoBehaviour
{
    Parking p;
    [HideInInspector]
    public int[] floorST = { 180, 200, 220, 240, 260 };
    [HideInInspector]
    public float[] floorT = { 180, 200, 220, 240, 260 };
    [HideInInspector]
    public int[] startValetT;
    [HideInInspector]
    public float[] valetTime = { 60, 75, 80, 85, 90 };

    public TextMeshPro floorN, upN, spaceN, valetN;
    public GameObject minusText;
    public void Set(Parking pa)
    {
        startValetT = new int[]{ 70, 75, 80, 85, 90 };
        p = pa;
    }
    private void FixedUpdate()
    {
        if (!p || !GC.INS.work || GC.INS.visit)
            return;
        if (p.floor > 0)
        {
            FloorUpdate(0);
            if (p.floor > 1)
            {
                FloorUpdate(1);
                if (p.floor > 2)
                {
                    FloorUpdate(2);
                    if (p.floor > 3)
                    {
                        FloorUpdate(3);
                        if (p.floor > 4)
                        {
                            FloorUpdate(4);

                        }
                    }
                }
            }
        }
    }
    void FloorUpdate(int x)
    {
        if (floorT[x] <= 0)
        {
            floorT[x] = floorST[x];
            AddCoins((x+1) * p.floorLevels[x][0]);
        }
        else
        {
            floorT[x] -= Time.fixedDeltaTime;
        }
        if (valetTime[x] <= 0)
        {
            if (!p.tipReady[x])
            {
                p.tipReady[x] = true;
                p.valetI[x].color = new Color(0.5f, 1f, 0.5f);
                p.valetTimesT[x].text = "";
                p.valetFill[x].fillAmount = 1;
            }
        }
        else
        {
            valetTime[x] -= Time.fixedDeltaTime;
            if (p.gameObject.activeInHierarchy)
            {
                p.valetFill[x].fillAmount = (1 - (valetTime[x] / startValetT[x]));
                p.valetTimesT[x].text = (int)valetTime[x] + "s";
            }

        }
    }
    public void UpgradeSet()
    {
        if (p.floor < 1)
            return;
        floorN.text = p.floor.ToString();
        upN.text = p.floorLevels[p.floor-1][0].ToString();
        spaceN.text = p.floorLevels[p.floor-1][1].ToString();
        valetN.text = p.floorLevels[p.floor-1][2].ToString();
    }
    public void SetVisitUp(int floor, int[] levels)
    {
        if (floor < 1)
            return;
        floorN.text = floor.ToString();
        upN.text = levels[0].ToString();
        spaceN.text = levels[1].ToString();
        valetN.text = levels[2].ToString();
    }
    void AddCoins(int c)
    {
        GC.INS.AddCoins(c);
        TextMeshPro tempText = Instantiate(minusText, transform).GetComponentInChildren<TextMeshPro>();
        tempText.text = "+" + c;
    }
}
