using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Parking : MonoBehaviour
{
    public GameObject parking, addBtn, btnAnim;
    public Text addBtnText;
    public Text[] coinsT, valetT, timeT, floor1C, floor2C, floor3C, floor4C, floor5C, valetTimesT;
    public Image addIm;
    public Image[] floor1IC, floor2IC, floor3IC, floor4IC, floor5IC, valetFill, floor1Fill, floor2Fill, floor3Fill, floor4Fill, floor5Fill, valetI;
    List<Image[]> costBtn, floorsFills;
    List<Text[]> floorsCost;
    [HideInInspector]
    public int floor;
    public List<int[]> floorLevels;
    public GameObject[] floors;
    public GameObject minusText;
    bool changedAdd;
    List<bool[]> changedI;
    ParkingO p;
    
    [HideInInspector]
    public bool[] tipReady;
    float coinsAve;

    public List<object> TransformLevelsToList()
    {
        List<object> tempList = new List<object>();
        for(int i=0; i<floorLevels.Count; i++)
        {
            Dictionary<string, int> tempDic = new Dictionary<string, int>()
            {
                  { "up", floorLevels[i][0]},
                  { "space", floorLevels[i][1]},
                  { "valet", floorLevels[i][2]},
                 
            };
            tempList.Add(tempDic);
        }

        return tempList;
    }
    void SetArrays()
    {
        floorsCost = new List<Text[]>();
        floorsCost.Add(floor1C);
        floorsCost.Add(floor2C);
        floorsCost.Add(floor3C);
        floorsCost.Add(floor4C);
        floorsCost.Add(floor5C);

        costBtn = new List<Image[]>();
        costBtn.Add(floor1IC);
        costBtn.Add(floor2IC);
        costBtn.Add(floor3IC);
        costBtn.Add(floor4IC);
        costBtn.Add(floor5IC);

        floorsFills = new List<Image[]>();
        floorsFills.Add(floor1Fill);
        floorsFills.Add(floor2Fill);
        floorsFills.Add(floor3Fill);
        floorsFills.Add(floor4Fill);
        floorsFills.Add(floor5Fill);

        changedI = new List<bool[]>();
        changedI.Add(new bool[3]);
        changedI.Add(new bool[3]);
        changedI.Add(new bool[3]);
        changedI.Add(new bool[3]);
        changedI.Add(new bool[3]);

        tipReady = new bool[5];

    }
   public void Set(List<int[]> levels)
    {
        if (floorLevels != null)
            return;
        SetArrays();
        if (levels != null)
        {
            floorLevels = levels;
            for(int i=0; i<floorLevels.Count; i++)
            {
                if (floorLevels[i][0] > 0)
                {
                    floor = i+1;
                }
            }
            changedAdd = false;
            addIm.color = new Color(0.7f, 0.8f, 0.8f);
            if (floor < 5)
            {
                addBtnText.text = costUp[floor, 0].ToString("n0");
                if (!changedAdd && GC.INS.coins >= costUp[floor, 0])
                {
                    changedAdd = true;
                    addIm.color = new Color(0.4f, 0.8f, 1f);
                }
            }
            else 
            { 

                changedAdd = true;
                addBtn.SetActive(false);
            }
            
            p = Instantiate(parking, new Vector3(-3.5f, 0.8f, 0), Quaternion.identity, GC.INS.roomsArrange.transform).GetComponent<ParkingO>();
            p.Set(this);
            for (int i=0; i<floor; i++)
            {
                floors[i].SetActive(true);
                for (int j=0; j<3; j++)
                {
                    changedI[i][j] = false;
                    costBtn[i][j].color = new Color(0.7f, 0.8f, 0.8f);
                    if (floorLevels[i][j] >= 10)
                    {
                        costBtn[i][j].gameObject.SetActive(false);
                    }
                }
                //Floor Properties
                int tempCoins = (i + 1) * floorLevels[i][0];
                coinsT[i].text = (tempCoins).ToString();
                p.floorST[i] = 160 + i * 20 + 20 * floorLevels[i][0] - 15 * floorLevels[i][1];
                coinsAve += ((tempCoins*1f) / (p.floorST[i]*1f));
                p.floorT[i] = p.floorST[i];
                int minutes = Mathf.FloorToInt(p.floorST[i] / 60F);
                int seconds = Mathf.FloorToInt(p.floorST[i] - minutes * 60);
                string niceTime = string.Format("{0:0}:{1:00}", minutes, seconds);
                timeT[i].text = niceTime;


                ///Up
                floorsFills[i][0].fillAmount = floorLevels[i][0] * .1f;
                if (floorLevels[i][0] < 10)
                    floorsCost[i][0].text = costUp[i, floorLevels[i][0]].ToString("n0");

                //Space
                floorsFills[i][1].fillAmount = floorLevels[i][1] * .1f;
                if (floorLevels[i][1] < 10)
                    floorsCost[i][1].text = ((300 + i * 100) + (300 + i * 100) * floorLevels[i][1]).ToString("n0");

                //Valet
                p.startValetT[i] -= (i + 3) * floorLevels[i][2];
              
                p.valetTime[i] = Random.Range(p.startValetT[i], p.startValetT[i]/2);
                
                valetT[i].text = p.startValetT[i].ToString();
                floorsFills[i][2].fillAmount = floorLevels[i][2] * .1f;
                if (floorLevels[i][2] < 10)
                    floorsCost[i][2].text = costValet[i, floorLevels[i][2]].ToString("n0");
                p.UpgradeSet();
            }
            GC.INS.SetParkingCoinsAverage(coinsAve);
        }
        else
        {
            floorLevels = new List<int[]>();
            floorLevels.Add(new int[3]);
            floorLevels.Add(new int[3]);
            floorLevels.Add(new int[3]);
            floorLevels.Add(new int[3]);
            floorLevels.Add(new int[3]);
            p = Instantiate(parking, new Vector3(-3f, 0.8f, 0), Quaternion.identity).GetComponent<ParkingO>();
            p.Set(this);
        }

        

    }
   
    void CheckChangedI(int x)
    {
        if (!changedI[x][0] && floorLevels[x][0] < 10 && GC.INS.coins >= costUp[x, floorLevels[x][0]])
        {
            changedI[x][0] = true;
            costBtn[x][0].color = new Color(0.4f,0.8f,1f);
        }
        if (!changedI[x][1] && floorLevels[x][1] < 10 && GC.INS.coins >= ((300 + x * 100) + (300 + x * 100) * floorLevels[x][1]))
        {
            changedI[x][1] = true;
            costBtn[x][1].color = new Color(0.4f, 0.8f, 1f);
        }
        if (!changedI[x][2] && floorLevels[x][2] < 10 && GC.INS.coins >= costValet[x, floorLevels[x][2]])
        {
            changedI[x][2] = true;
            costBtn[x][2].color = new Color(0.4f, 0.8f, 1f);
        }
        if (!changedAdd && floor<4 && GC.INS.coins >= costUp[floor, 0])
        {
            changedAdd = true;
            addIm.color = new Color(0.4f, 0.8f, 1f);
        }
    }
    public void AddFloor()
    {
        if (floor < 5 && GC.INS.coins >= costUp[floor, 0])
        {
            GC.INS.Purchase(costUp[floor, 0]);
            floorLevels[floor][0] = 1;
            coinsT[floor].text = (floor + 1).ToString();
            p.floorST[floor] = 180 + floor * 20;
            p.floorT[floor] = p.floorST[floor];
            int minutes = Mathf.FloorToInt(p.floorST[floor] / 60F);
            int seconds = Mathf.FloorToInt(p.floorST[floor] - minutes * 60);
            string niceTime = string.Format("{0:0}:{1:00}", minutes, seconds);
            timeT[floor].text = niceTime;
            valetT[floor].text = p.startValetT[floor].ToString();
            floors[floor].SetActive(true);
            floorsFills[floor][0].fillAmount = .1f;
            floor++;
            changedAdd = false;
            addIm.color = new Color(0.7f, 0.8f, 0.8f);
            if (floor == 5)
            {
                changedAdd = true;
                addIm.gameObject.SetActive(false);
            }
            else
            {
                addBtnText.text = costUp[floor, 0].ToString("n0");
            }
            p.UpgradeSet();
            GC.INS.SetStars(100 * floor);
            GC.INS.AddXp(10 * floor);
        }
    }
    int ct;
    public void SetUpdate(int x)
    {
        ct=x;
    }
    public void Upgrade(int x)
    {
        if (floorLevels[x][ct] >= 10)
            return;
        SC.INS.PlaySound(0, 0, 0);
        if (ct == 0)
        {
            if (GC.INS.coins >= costUp[x, floorLevels[x][0]])
            {
                GC.INS.Purchase(costUp[x, floorLevels[x][0]]);
                floorLevels[x][0]++;
                coinsT[x].text = ((x + 1) * floorLevels[x][0]).ToString();
                p.floorST[x] = 160 + x * 20 + 20 * floorLevels[x][0] - 15 * floorLevels[x][1];
                int minutes = Mathf.FloorToInt(p.floorST[x] / 60F);
                int seconds = Mathf.FloorToInt(p.floorST[x] - minutes * 60);
                string niceTime = string.Format("{0:0}:{1:00}", minutes, seconds);
                timeT[x].text = niceTime;
                floorsFills[x][0].fillAmount = floorLevels[x][ct] * .1f;
                if(floorLevels[x][ct] <10)
                    floorsCost[x][ct].text = costUp[x, floorLevels[x][0]].ToString("n0");
                GC.INS.SetStars(5 * (x + 1));
                GC.INS.AddXp(1 * (x + 1));
            }
        }
        else if (ct == 1)
        {
            if (GC.INS.coins >= (300 + x * 100) + (300 + x * 100) * floorLevels[x][1])
            {
                GC.INS.Purchase((300 + x * 100) + (300 + x * 100) * floorLevels[x][1]);
                floorLevels[x][1]++;
                p.floorST[x] = 160 + x * 20 + 20 * floorLevels[x][0] - 15 * floorLevels[x][1];
                int minutes = Mathf.FloorToInt(p.floorST[x] / 60F);
                int seconds = Mathf.FloorToInt(p.floorST[x] - minutes * 60);
                string niceTime = string.Format("{0:0}:{1:00}", minutes, seconds);
                timeT[x].text = niceTime;
                floorsFills[x][1].fillAmount = floorLevels[x][ct] * .1f;
                if (floorLevels[x][ct] < 10)
                    floorsCost[x][ct].text = ((300 + x * 100) + (300 + x * 100) * floorLevels[x][1]).ToString("n0");
                GC.INS.SetStars(5 * (x + 1));
                GC.INS.AddXp(1 * (x + 1));
            }
        }
        else
        {
            if (GC.INS.coins >= costValet[x, floorLevels[x][2]])
            {
                GC.INS.Purchase(costValet[x, floorLevels[x][2]]);
                floorLevels[x][2]++;
                p.startValetT[x] -= (x + 3);
                p.valetTime[x] -= x + 3;
                valetT[x].text = p.startValetT[x].ToString();
                floorsFills[x][2].fillAmount = floorLevels[x][ct] * .1f;
                if (floorLevels[x][ct] < 10)
                    floorsCost[x][ct].text = costValet[x, floorLevels[x][2]].ToString("n0");
                GC.INS.SetStars(5 * (x+1));
                GC.INS.AddXp(1 * (x+1));
            }
        }
        if (floorLevels[x][ct] >= 10)
        {
            costBtn[x][ct].gameObject.SetActive(false);
        }
     
        costBtn[x][ct].color = new Color(0.7f, 0.8f, 0.8f);
        changedI[x][ct] = false;
        p.UpgradeSet();
    }
    public void ClaimTip(int x)
    {
        SC.INS.PlaySound(0, 0, 0);
        if (!tipReady[x])
            return;
        GC.INS.AddCoins((x+1) * floorLevels[x][0]);
        p.valetTime[x] = p.startValetT[x];
        tipReady[x] = false;
        valetI[x].color  = new Color(0.7f, 0.8f, 0.8f);
        Text tempText = Instantiate(minusText, transform.parent).GetComponentInChildren<Text>();
        tempText.transform.parent.position = new Vector3(valetI[x].transform.position.x + 10, valetI[x].transform.position.y, valetI[x].transform.position.z);
        tempText.text = "+" + ((x + 1) * floorLevels[x][0]).ToString("n0");
    }
    private void FixedUpdate()
    {
        if (floor > 0)
        {
            CheckChangedI(0);
            if (floor > 1)
            {
                CheckChangedI(1);
                if (floor > 2)
                {
                    CheckChangedI(2);
                    if (floor > 3)
                    {
                        CheckChangedI(3);
                        if (floor > 4)
                        {
                            CheckChangedI(4);
                        }
                    }
                }
            }
        }
    }
    

    int[,] costUp =
    {
        //Floor 1
        {5000, 2000, 2400, 3200, 4400,
        6000, 8000, 10400, 13200, 16400},

        //Floor 2
        {10000, 3000, 3600, 4800, 6600,
        9000, 12000, 15600, 19800, 24600},

        //Floor 3
        {20000, 5000, 5800, 7400, 9800,
        13000, 17000, 21800, 27400, 33800},

        //Floor 4
        {30000, 8000, 9000, 11000, 14000,
        18000, 23000, 29000, 36000, 44000},

        //Floor 5
        {50000, 10000, 11500, 14500, 19000,
        25000, 32500, 41500, 52000, 64000},

    };
    int[,] costValet =
    {
        //Floor 1
        {25, 50, 75, 100, 175,
        250, 500, 750, 1000, 1500},

         //Floor 2
        {50, 75, 100, 175,250, 
         500, 750, 1000, 1500, 2000},

         //Floor 3
        {100, 175,250, 500, 750, 
        1000, 1500,2000,2500,3000},

         //Floor 4
        {250, 500, 750, 1000, 1500,
        2000,2500,3000,4000,5000},

         //Floor 5
        {1000, 1500,2000,2500,3000,
        3500,4000,5000,6000,7500},

    };

}
