using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class DailyMisions : MonoBehaviour
{
    List<int[]> misions = new List<int[]>();
    List<DailyTile> tiles = new List<DailyTile>();
    public GameObject tileObj, dot, listport;
    public Image rewardsFill, btnI;
    public Text rewardsCount, addXpText;
    public DateTime lastday;
    bool[] completed = new bool[4];
    int completedCount;
    bool allCompleted, xpAdded;
    int addXp;
    public GameObject laptop, welcomeGift, tutoRewardsP, dailyMisionsP, dailyRewardsP;
    public Animator laptopAnim;
    [HideInInspector]
    public bool tutoRewards;
    int tutoCustomize;
    public Image[] fillTutoIm, tutoRewardsClaimI;
    public Text[] tutoText;
    [HideInInspector]
    public bool[] tutoRewardsClaim = new bool[4];
    bool addedFriend, dragedV;
    public Image dailyUnlockI, tutoRewardsFill;
    public Text tutoRewardsLeft;
    int tutoRewardsCount;
    bool tutoRewardsComplete;
    [HideInInspector]
    public int currentDay;
    public Image[] dailyImages, inviteImages;
    public GameObject dailyDecoration;
    public bool showing;

    public GameObject help;
    public VideoPlayer vp;
    public RenderTexture rt;
    public GameObject rewardSign, dailyButton, sign, inviteP, shareP, connectF, notiP, content, closeBtn;
    GameObject notiPanel;
    public GameObject[] iapR;
    List<int> iaps;
    [HideInInspector]
    public List<string> invitedIds;
    public GameObject[] tutoClaimButtons;

    // Start is called before the first frame update
    void Start()
    {
        if (misions != null)
            return;
        misions = new List<int[]>();
        misions.Add(new int[3]);
        misions.Add(new int[3]);
        misions.Add(new int[3]);
        misions.Add(new int[3]);
    }

    public void GenerateMisions()
    {
        misions = new List<int[]>();
        List<int> tempMis = new List<int>();
        int nmisions = UnityEngine.Random.Range(3, 5);
        for (int i = 0; i < nmisions; i++)
        {
            int tid;
            do
            {
                tid = UnityEngine.Random.Range(0, 29);
                if (UnityEngine.Random.Range(0, 2) == 0)
                {
                    if (UnityEngine.Random.Range(0, 2) == 0)
                    {
                        tid = UnityEngine.Random.Range(0, 7);
                    }
                    else if (UnityEngine.Random.Range(0, 2) == 0)
                    {
                        tid = UnityEngine.Random.Range(16, 18);
                    }
                    else
                        tid = UnityEngine.Random.Range(24, 27);

                }
            } while (tempMis.Contains(tid));
            
            int tempT = 0;
            switch (tid)
            {
                case 0:
                case 16:
                    tempT = UnityEngine.Random.Range(10, 21);
                    break;
                case 1:
                case 2:
                case 3:
                case 4:
                case 5:
                    tempT = UnityEngine.Random.Range(5, 11);
                    break;
                case 6:
                    tempT = UnityEngine.Random.Range(20, 31);
                    break;
                case 7:
                    tempT = UnityEngine.Random.Range(1, 4);
                    break;
                case 8:
                case 9:
                case 21:
                    tempT = UnityEngine.Random.Range(1, 6);
                    break;
                case 10:
                case 11:
                case 12:
                case 13:
                case 14:
                case 15:
                case 18:
                case 22:
                case 23:
                case 28:
                case 19:
                    tempT = UnityEngine.Random.Range(1, 3);
                    break;
                case 17:
                    tempT = UnityEngine.Random.Range(3, 6);
                    break;
                case 20:
                    tempT = UnityEngine.Random.Range(6, 16) * 1000;
                    break;
                case 24:
                    tempT = UnityEngine.Random.Range(20, 41);
                    break;
                case 25:
                    tempT = UnityEngine.Random.Range(2, 5);
                    break;
                case 26:
                    tempT = UnityEngine.Random.Range(15, 30);
                    break;
                case 27:
                    tempT = 1;
                    break;
                
            }
            if (GC.INS.level < 11)
            {
                tempT -= (tempT /3);
                if(GC.INS.level < 7)
                {
                    tempT -= (tempT / 2);
                    if (GC.INS.level < 5)
                        tempT -= (tempT / 5);
                }
            }
            //Si es spend coins
            if (tid == 20)
            {
                tempT /= 10;
                tempT *= 10;
                if (tempT <= 2500)
                {
                    tempT += UnityEngine.Random.Range(1, 5) * 500;
                }
            }
            if (tempT == 0)
                tempT = 1;
            int[] tempAdd = { tid, tempT, 0 };
            misions.Add(tempAdd);
            tempMis.Add(tid);
        }

    }

    public void SetTutoRewardsList(List<object> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            tutoRewardsClaim[i] = (bool)list[i];
            tutoClaimButtons[i].SetActive(!tutoRewardsClaim[i]);
        }
        if (GC.INS.level > 2 && !tutoRewardsClaim[0])
        {
            sign = Instantiate(rewardSign, dailyButton.transform);
        }
        SetTutoRewards();
    }

    public void Load(DateTime last, List<object> list, int currentDay)
    {
        if (GC.INS.level < 1)
            return;
        if (tutoRewards)
            return;

        this.currentDay = currentDay;
        lastday = last;

        int days = (DateTime.UtcNow.Date - lastday.Date).Days;

        if (days >= 1 && list != null)
        {
            showing = true;
            if (!GC.INS.gc.showing)
                laptop.SetActive(true);
            if (days > 1)
            {
                this.currentDay = 0;
            }
         
            SetTodayReward();
        }
        if (days >= 1 || list == null)
        {
            GenerateMisions();
            lastday = DateTime.UtcNow;
        }
        else
        {
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i] != null)
                {
                    Dictionary<string, object> tempDic = list[i] as Dictionary<string, object>;
                    int[] tempAdd = { Convert.ToInt32(tempDic["id"]), Convert.ToInt32(tempDic["goal"]), Convert.ToInt32(tempDic["left"]) };
                    misions.Add(tempAdd);
                    if (tempAdd[1] <= tempAdd[2])
                    {
                        completed[i] = true;
                        completedCount++;
                    }

                }
            }
            rewardsCount.text = completedCount + "/" + misions.Count;
            rewardsFill.fillAmount = (completedCount * 1f) / (misions.Count * 1f);
        }
        if (tiles.Count > 0)
        {
            for (int i = 0; i < tiles.Count; i++)
                Destroy(tiles[i].gameObject);
            tiles = new List<DailyTile>();
        }
        for (int i = 0; i < misions.Count; i++)
        {
            tiles.Add(Instantiate(tileObj, listport.transform).GetComponent<DailyTile>());
            tiles[i].Create(i, misions[i][0], misions[i][1], misions[i][2]);
        }
        if (completedCount >= misions.Count)
        {
            allCompleted = true;
            btnI.color = new Color(0.345f, 1f, 0.4f);
            rewardsFill.fillAmount = 1;
            rewardsCount.text = misions.Count + "/" + misions.Count;
        }
        else
        {
            rewardsCount.text = completedCount + "/" + misions.Count;
            rewardsFill.fillAmount = (completedCount * 1f) / (misions.Count * 1f);
            btnI.color = new Color(0.79f, 0.84f, 0.86f);
        }
        if(GC.INS.level<8)
            addXp = (GC.levelxp[GC.INS.level] - GC.levelxp[GC.INS.level - 1]) / 3;
        else
            addXp = (GC.levelxp[GC.INS.level] - GC.levelxp[GC.INS.level - 1]) / ((GC.INS.level-2)/2);

        if (GC.INS.level <= 3)
            addXp += 8;
        addXpText.text = "+" + addXp + "xp";
        iaps = new List<int>();
        if (!GC.INS.iap[3])
            iaps.Add(0);
        if (!GC.INS.iap[4])
            iaps.Add(1);
        if (!GC.INS.iap[5])
            iaps.Add(2);
    }



    public void SetTutoRewards()
    {
        dot.SetActive(true);
        if (FRC.INS.friendList.Count > 0)
        {
            AddFriendFirst();
        }
        tutoRewardsP.SetActive(true);
        dailyMisionsP.SetActive(false);
        tutoCustomize = PlayerPrefs.GetInt("tutoCustomize");
        tutoText[1].text = tutoCustomize + "/3";
        fillTutoIm[1].fillAmount = (tutoCustomize * 1f) / 3f;
        dailyUnlockI.color = new Color(0.79f, 0.84f, 0.86f);
        CheckTutoComplete();
        for (int i = 0; i < tutoRewardsClaimI.Length; i++)
        {
            if (tutoRewardsClaim[i])
            {
                tutoRewardsClaimI[i].color = new Color(0.79f, 0.84f, 0.86f);
                if (i == 1 || i == 3)
                {
                    tutoText[i - 1].text = "1/1";
                    fillTutoIm[i - 1].fillAmount = 1;
                }
            }
            else
            {
                if (i == 0 || i == 1 && dragedV || i == 2 && tutoCustomize >= 3 || i == 3 && addedFriend)
                    tutoRewardsClaimI[i].color = new Color(0.4f, 0.8f, 1f);
                else
                    tutoRewardsClaimI[i].color = new Color(0.79f, 0.84f, 0.86f);

            }
        }
    }

    public object TransformMisionsToList()
    {
        List<object> tempList = new List<object>();
        for (int i = 0; i < misions.Count; i++)
        {
            Dictionary<string, object> temp = new Dictionary<string, object>
                {
                    { "id", misions[i][0]},
                    { "goal", misions[i][1]},
                    { "left", misions[i][2]},
                };
            tempList.Add(temp);
        }
        return tempList.ToArray();
    }

    public void AddTask(int id, int quant)
    {
        if (tutoRewards)
        {
            if(id == 21 && tutoCustomize < 3)
            {
                if (tutoRewardsClaim[2])
                {
                    tutoRewardsClaimI[1].color = new Color(0.79f, 0.84f, 0.86f);
                    return;
                }
                tutoCustomize++;
                PlayerPrefs.SetInt("tutoCustomize", tutoCustomize);
                tutoText[1].text = tutoCustomize + "/3";
                fillTutoIm[1].fillAmount = (tutoCustomize * 1f) / 3f;
                dot.SetActive(true);
                if (tutoCustomize == 3)
                {
                    tutoRewardsClaimI[2].color = new Color(0.4f, 0.8f, 1f);
                }
            }else if (id == 26 && !dragedV)
            {
                if (tutoRewardsClaim[1])
                {
                    tutoRewardsClaimI[0].color = new Color(0.79f, 0.84f, 0.86f);
                    return;
                }
                dragedV = true;
                tutoText[0].text = "1/1";
                fillTutoIm[0].fillAmount = 1f;
                tutoRewardsClaimI[1].color = new Color(0.4f, 0.8f, 1f);
                dot.SetActive(true);
            }
             CheckTutoComplete();
           
            return;
        }
        if (allCompleted)
            return;
        for (int i = 0; i < misions.Count; i++)
        {
            if (misions[i][0] == id)
            {
                if (completed[i])
                    break;
                misions[i][2] += quant;
                tiles[i].AddTask(misions[i][2]);
                if (misions[i][2] >= misions[i][1])
                {

                    CompleteMision(i);
                }
                break;
            }
        }
    }

    void CompleteMision(int x)
    {
        if (allCompleted)
            return;
        completed[x] = true;
        completedCount++;
        dot.SetActive(true);
        if (completedCount >= misions.Count)
        {
            allCompleted = true;
            btnI.color = new Color(0.345f, 1f, 0.4f);
            rewardsCount.text = misions.Count + "/" + misions.Count;
            rewardsFill.fillAmount = 1;
        }
        else
        {
            rewardsCount.text = completedCount + "/" + misions.Count;
            rewardsFill.fillAmount = (completedCount * 1f) / (misions.Count * 1f);
            btnI.color = new Color(0.79f, 0.84f, 0.86f);
        }
    }
    public void CompleteAll()
    {
        if (!allCompleted || xpAdded)
            return;
        xpAdded = true;
        SC.INS.PlaySound(0, 15, 0);
        GC.INS.pg.Achivements(6, 0);
        GC.INS.AddXp(addXp);
        addXp = 0;
        btnI.transform.parent.gameObject.SetActive(false);
        rewardsFill.fillAmount = 1;
        misions[0][2] = misions[0][1] + 6;
    }
    public void XpAdded()
    {
        xpAdded = true;
        btnI.transform.parent.gameObject.SetActive(false);
    }
    public void CoinsAdded(int i)
    {
        misions[i][2] = misions[i][1] + 3;
    }

    public void WelcomeGift()
    {
        tutoRewards = true;
        laptop.SetActive(true);
        welcomeGift.SetActive(true);
        closeBtn.SetActive(false);
        dailyRewardsP.SetActive(false);
        inviteP.SetActive(false);
        shareP.SetActive(false);
        connectF.SetActive(false);
        SC.INS.PlaySound(0, 15, 0);
    }
    public void ChooseWelcomeGift(int x)
    {
        if (x == 0)
        {
            GC.INS.gift.AddGift(new Dictionary<string, object>
            {
                { "id", 0},
                { "type",0},
                { "subtype", 0},
            });
            GC.INS.gift.AddGift(new Dictionary<string, object>
            {
                { "id", 0},
                { "type",0},
                { "subtype", 0},
            });
            GC.INS.gift.AddGift(new Dictionary<string, object>
            {
                { "id", 1},
                { "type",0},
                { "subtype", 0},
            });

            GC.INS.gift.AddGift(new Dictionary<string, object>
            {
                { "id", 1},
                { "type",0},
                { "subtype", 0},
            });
        }
        else if (x == 1)
        {
            GC.INS.gift.AddGift(new Dictionary<string, object>
            {
                { "id", 8},
                { "type",0},
                { "subtype", 3},
            });

        }
        else
        {

            GC.INS.gift.AddGift(new Dictionary<string, object>
            {
                { "id", 2},
                { "type",0},
                { "subtype", 0},
            });
            GC.INS.gift.AddGift(new Dictionary<string, object>
            {
                { "id", 17},
                { "type",0},
                { "subtype", 1},
            });
        }
        closeBtn.SetActive(true);
        SetTutoRewards();
        CloseLaptop();
    }
    public void CloseLaptop()
    {
        if (IsInvoking("LaptopOff"))
            return;
        laptopAnim.SetTrigger("Off");
        Invoke("LaptopOff", .5f);
    }
    void LaptopOff()
    {
        laptop.SetActive(false);
        welcomeGift.SetActive(false);
        dailyRewardsP.SetActive(false);
        inviteP.SetActive(false);
        shareP.SetActive(false);
        connectF.SetActive(false);
        closeBtn.SetActive(true);
        if (notiPanel)
        {
            Destroy(notiPanel);
        }
        for (int i = 0; i < iapR.Length; i++)
            iapR[i].SetActive(false);
        GC.INS.gc.Next();
        showing = false;
    }
    public void ClaimTutoReward(int x)
    {
        if (tutoRewardsClaim[x])
            return;
       
        if (sign)
        {
            Destroy(sign);
        }
        if (x == 0)
        {
            GC.INS.AddCoins(500);
        }
        else if (x == 1)
        {
            if (!dragedV)
                return;
            GC.INS.AddCoins(1000);
        }
        else if (x == 2)
        {
            if (tutoCustomize < 3)
                return;
            GC.INS.AddCoins(12000);
        }
        else
        {
            if (!addedFriend)
                return;
            GC.INS.AddCoins(3000);
        }
        tutoClaimButtons[x].SetActive(false);
        tutoRewardsClaim[x] = true;
        CheckTutoComplete();
    }
    public void AddFriendFirst()
    {
        if (tutoRewardsClaim[3])
        {
            tutoRewardsClaimI[2].color = new Color(0.79f, 0.84f, 0.86f);
            return;
        }
        addedFriend = true;
        tutoText[2].text = "1/1";
        tutoRewardsClaimI[3].color = new Color(0.4f, 0.8f, 1f);
        dot.SetActive(true);
        fillTutoIm[2].fillAmount = 1;
        CheckTutoComplete();
    }
   
    void CheckTutoComplete()
    {
        tutoRewardsCount = 1;
        if (tutoCustomize >= 3 || tutoRewardsClaim[2])
            tutoRewardsCount++;
        if (addedFriend || tutoRewardsClaim[3] || FRC.INS.friendList.Count>0)
            tutoRewardsCount++;
        if (dragedV || tutoRewardsClaim[1])
            tutoRewardsCount++;
        tutoRewardsLeft.text = tutoRewardsCount + "/4";
        tutoRewardsFill.fillAmount = (tutoRewardsCount*1f) / 4f;
        if (tutoRewardsCount < 4)
            return;
        tutoRewardsComplete = true;

        dailyUnlockI.color = new Color(0.345f, 1f, 0.4f);
    }
    public void UnlockDaily()
    {
        if (!tutoRewardsComplete)
            return;
        tutoRewards = false;
        tutoRewardsP.SetActive(false);
        dailyMisionsP.SetActive(true);
        Load(DateTime.UtcNow, null, 0);
    }
    public void SetTodayReward()
    {
        dailyRewardsP.SetActive(true);
        welcomeGift.SetActive(false);
        closeBtn.SetActive(false);
        for (int i = 0; i < currentDay; i++)
        {
            dailyImages[i].color = new Color(0.6f, 0.6f, 0.7f);
        }
        dailyImages[currentDay].color = new Color(0, .65f, 1);
        dailyImages[currentDay].transform.localScale = new Vector3(1.1f, 1.1f, 1);
        dailyDecoration.SetActive(true);
        dailyDecoration.transform.SetParent(dailyImages[currentDay].transform);
        dailyDecoration.transform.localPosition = Vector3.zero;
        SC.INS.PlaySound(0, 15, 0);
        currentDay++;
    }
    public void SetLaptop()
    {
        laptop.SetActive(true);
    }
    public void ClaimTodayReward()
    {
        switch (currentDay-1)
        {
            case 0:
                GC.INS.AddCoins(1000);
                break;
            case 1:
                int rand = UnityEngine.Random.Range(0, 5);
                GC.INS.gift.AddGift(new Dictionary<string, object>
                            {
                                { "id", (rand>=1)?UnityEngine.Random.Range(0, 6):UnityEngine.Random.Range(2, 8)},
                                { "type",1},
                                { "subtype", (rand>=1)?rand+1:rand},
                });
                break;
            case 2:
                GC.INS.gift.AddGift(new Dictionary<string, object>
                            {
                                { "id", UnityEngine.Random.Range(2, 12)},
                                { "type",3},
                                { "subtype", 0},
                });
                break;
            case 3:
                int[] ent = {8,9,10,26};
                GC.INS.gift.AddGift(new Dictionary<string, object>
                            {
                                { "id", ent[UnityEngine.Random.Range(0, 4)]},
                                { "type",0},
                                { "subtype", 3},
                });
                break;
            case 4:
                if(UnityEngine.Random.Range(0, 2)==0)
                {
                    int[] star = { 14, 34, 36, 37 };
                    GC.INS.gift.AddGift(new Dictionary<string, object>
                            {
                                { "id", star[UnityEngine.Random.Range(0, 4)]},
                                { "type",0},
                                { "subtype", 2},
                     });
                }
                else
                {
                    int[] dec = { 17, 22, 20, 21};
                    GC.INS.gift.AddGift(new Dictionary<string, object>
                            {
                                { "id", dec[UnityEngine.Random.Range(0, 4)]},
                                { "type",0},
                                { "subtype", 1},
                     });
                }
                break;
            case 5:
                GC.INS.gems += 25;
                GC.INS.gemsText.text = GC.INS.gems.ToString("n0");
                currentDay = 0;
                break;
        }

        CloseLaptop();
        if (iaps.Count > 0 && UnityEngine.Random.Range(0, 2) == 0 && !IsInvoking("Recomend") && GC.INS.level>5)
            Invoke("Recomend", 3);
    }
    void Recomend()
    {
        laptop.SetActive(true);
        SC.INS.PlaySound(0, 15, 0);
        iapR[iaps[UnityEngine.Random.Range(0, iaps.Count)]].SetActive(true);
    }
    public void RecomendNoAds()
    {
        if (GC.INS.iap[4])
            return;
        if (UnityEngine.Random.Range(0, 5) == 0)
            Invoke("RecomendIAPNoads", 2);
    }
    void RecomendIAPNoads()
    {
        laptop.SetActive(true);
        SC.INS.PlaySound(0, 15, 0);
        iapR[1].SetActive(true);
    }
    public void DoubleTodayReward()
    {
        switch (currentDay - 1)
        {
            case 0:
                GC.INS.AddCoins(1000);
                GC.INS.ad.CoinAnim();
                break;
            case 1:
                int rand = UnityEngine.Random.Range(0, 5);
                GC.INS.gift.AddGift(new Dictionary<string, object>
                            {
                                { "id", (rand>=1)?UnityEngine.Random.Range(1, 6):UnityEngine.Random.Range(2, 8)},
                                { "type",1},
                                { "subtype", rand},
                });
                break;
            case 2:
                GC.INS.gift.AddGift(new Dictionary<string, object>
                            {
                                { "id", UnityEngine.Random.Range(2, 12)},
                                { "type",3},
                                { "subtype", 0},
                });
                break;
            case 3:
                int[] ent = { 8, 9, 10, 26 };
                GC.INS.gift.AddGift(new Dictionary<string, object>
                            {
                                { "id", ent[UnityEngine.Random.Range(0, 4)]},
                                { "type",0},
                                { "subtype", 3},
                });
                break;
            case 4:
                if (UnityEngine.Random.Range(0, 2) == 0)
                {
                    int[] star = { 14, 34, 36, 37 };
                    GC.INS.gift.AddGift(new Dictionary<string, object>
                            {
                                { "id", star[UnityEngine.Random.Range(0, 4)]},
                                { "type",0},
                                { "subtype", 2},
                     });
                }
                else
                {
                    int[] dec = { 17, 22, 20, 21 };
                    GC.INS.gift.AddGift(new Dictionary<string, object>
                            {
                                { "id", dec[UnityEngine.Random.Range(0, 4)]},
                                { "type",0},
                                { "subtype", 1},
                     });
                }
                break;
            case -1:
            case 5:
                GC.INS.gems += 25;
                GC.INS.gemsText.text = GC.INS.gems.ToString("n0");
                GC.INS.ad.NormalAnim();
                break;
        }
    }

    public void Help(int x)
    {
        string tempUrl = "";
        switch (x)
        {
            case 0:
                tempUrl = "https://firebasestorage.googleapis.com/v0/b/pocket-hotel.appspot.com/o/dailyHelp%2FMi%20video.mp4?alt=media&token=02317099-f3bf-4ccb-b4d8-3eca71f26589";
                break;
            case 1:
                tempUrl = "https://firebasestorage.googleapis.com/v0/b/pocket-hotel.appspot.com/o/dailyHelp%2FFix1.mp4?alt=media&token=edfc7618-2b4d-4144-85b8-2b3dc0229df1";
                break;
            case 2:
                tempUrl = "https://firebasestorage.googleapis.com/v0/b/pocket-hotel.appspot.com/o/dailyHelp%2FFix2.mp4?alt=media&token=324ff5f2-cb9d-4ad7-bedc-95e4973bd880";
                break;
            case 3:
                tempUrl = "https://firebasestorage.googleapis.com/v0/b/pocket-hotel.appspot.com/o/dailyHelp%2FFix3.mp4?alt=media&token=2ed92ad7-8b22-457e-aaaa-5eac61aca4f3";
                break;
            case 4:
                tempUrl = "https://firebasestorage.googleapis.com/v0/b/pocket-hotel.appspot.com/o/dailyHelp%2FFix4.mp4?alt=media&token=ebafee79-23d5-498f-aa20-636422f45191";
                break;
            case 5:
                tempUrl = "https://firebasestorage.googleapis.com/v0/b/pocket-hotel.appspot.com/o/dailyHelp%2FFix5.mp4?alt=media&token=70423a69-1747-4b1a-953b-4aab9d3c55ec";
                break;
            case 6:
                tempUrl = "https://firebasestorage.googleapis.com/v0/b/pocket-hotel.appspot.com/o/dailyHelp%2FTip.mp4?alt=media&token=f8028373-366a-4bc9-b3a6-51e03b4e6c05";
                break;
            case 7:
                tempUrl = "https://firebasestorage.googleapis.com/v0/b/pocket-hotel.appspot.com/o/dailyHelp%2FVisitFriend.mp4?alt=media&token=0da947c6-ae4f-4806-a4cc-21b48d569197";
                break;
            case 20:
            case 8:
                tempUrl = "https://firebasestorage.googleapis.com/v0/b/pocket-hotel.appspot.com/o/dailyHelp%2FPurchaseRooms.mp4?alt=media&token=ed3de73c-c4ed-4879-a9f6-d8719caa0215";
                break;
            case 9:
                tempUrl = "https://firebasestorage.googleapis.com/v0/b/pocket-hotel.appspot.com/o/dailyHelp%2FDecorate.mp4?alt=media&token=8b2c5e3b-e13f-4ca6-94ef-a09134a0d426";
                break;
            case 10:
                tempUrl = "https://firebasestorage.googleapis.com/v0/b/pocket-hotel.appspot.com/o/dailyHelp%2FDecorate1.mp4?alt=media&token=4aa46e70-aa85-4ec9-bf27-ca8a6c830986";
                break;
            case 11:
                tempUrl = "https://firebasestorage.googleapis.com/v0/b/pocket-hotel.appspot.com/o/dailyHelp%2FDecorate2.mp4?alt=media&token=b9bfb5a0-e9ac-45d0-858c-1aac007ad0d8";
                break;
            case 12:
                tempUrl = "https://firebasestorage.googleapis.com/v0/b/pocket-hotel.appspot.com/o/dailyHelp%2FDecorate3.mp4?alt=media&token=7b3b16f0-f00b-434d-8e6d-2981820e1e58";
                break;
            case 13:
                tempUrl = "https://firebasestorage.googleapis.com/v0/b/pocket-hotel.appspot.com/o/dailyHelp%2FDecorate4.mp4?alt=media&token=b298b968-2493-489e-ac7d-47c27f4b933d";
                break;
            case 14:
                tempUrl = "https://firebasestorage.googleapis.com/v0/b/pocket-hotel.appspot.com/o/dailyHelp%2FDecorate5.mp4?alt=media&token=d52ba4bb-8db9-4e80-b810-3bc7450c195a";
                break;
            case 15:
                tempUrl = "https://firebasestorage.googleapis.com/v0/b/pocket-hotel.appspot.com/o/dailyHelp%2FDecorate6.mp4?alt=media&token=8d218780-3eea-429c-b29a-bd4350ad5d99";
                break;
            case 16:
                tempUrl = "https://firebasestorage.googleapis.com/v0/b/pocket-hotel.appspot.com/o/dailyHelp%2FCloud.mp4?alt=media&token=761b975a-5b92-46bd-96fe-73904afb7787";
                break;
            case 17:
                tempUrl = "https://firebasestorage.googleapis.com/v0/b/pocket-hotel.appspot.com/o/dailyHelp%2FCloudR.mp4?alt=media&token=2ea5d3e7-2a8b-4d72-be5d-4fee688ce7c8";
                break;
            case 18:
                tempUrl = "https://firebasestorage.googleapis.com/v0/b/pocket-hotel.appspot.com/o/dailyHelp%2FStaffOutfit.mp4?alt=media&token=c69352dc-9648-4dbc-afaa-a2b10e63c337";
                break;
                //Invite Friends
            case 19:
                tempUrl = "https://firebasestorage.googleapis.com/v0/b/pocket-hotel.appspot.com/o/dailyHelp%2FInvite.mp4?alt=media&token=8a95469f-1ca8-433c-a75e-094fb5e8cbf7";
                break;
            case 21:
                tempUrl = "https://firebasestorage.googleapis.com/v0/b/pocket-hotel.appspot.com/o/dailyHelp%2FCustomize.mp4?alt=media&token=fcd457d2-b4fd-4997-9590-6376c832502e";
                break;
            case 22:
                tempUrl = "https://firebasestorage.googleapis.com/v0/b/pocket-hotel.appspot.com/o/dailyHelp%2FPurOutfit.mp4?alt=media&token=21a2d22f-645e-441e-bcc3-b5bc0f103ac5";
                break;
            case 23:
                tempUrl = "https://firebasestorage.googleapis.com/v0/b/pocket-hotel.appspot.com/o/dailyHelp%2FPurColor.mp4?alt=media&token=53ccc6ae-fdf9-4b3c-ba9f-987be3e2ab81";
                break;
            case 24:
                tempUrl = "https://firebasestorage.googleapis.com/v0/b/pocket-hotel.appspot.com/o/dailyHelp%2FThrow.mp4?alt=media&token=a2cc109e-a7ca-4d00-92aa-540926fc3bb4";
                break;
                //Bonus videos
            case 25:
                tempUrl = "https://firebasestorage.googleapis.com/v0/b/pocket-hotel.appspot.com/o/dailyHelp%2FBonusVideo.mp4?alt=media&token=5e3f355f-253d-4e9a-badf-d4827092befe";
                break;
            case 26:
                tempUrl = "https://firebasestorage.googleapis.com/v0/b/pocket-hotel.appspot.com/o/dailyHelp%2FDragAndVisit.mp4?alt=media&token=19680ade-36ae-47dd-9adf-94eccebdfc6e";
                break;
            case 27:
                tempUrl = "https://firebasestorage.googleapis.com/v0/b/pocket-hotel.appspot.com/o/dailyHelp%2FPurStaff.mp4?alt=media&token=97794e7c-e5d8-41e4-8e09-88b1e3de8c92";
                break;
            case 29:
                tempUrl = "https://firebasestorage.googleapis.com/v0/b/pocket-hotel.appspot.com/o/dailyHelp%2FAddFriend.mp4?alt=media&token=fefc7844-1697-4c48-a0f7-fc6570029127";
                break;
            case 30:
                tempUrl = "https://firebasestorage.googleapis.com/v0/b/pocket-hotel.appspot.com/o/dailyHelp%2FChangePP.mp4?alt=media&token=c1d31a05-3fb0-47f8-bc0a-b873de54b097";
                break;
            case 28:
                tempUrl = "https://firebasestorage.googleapis.com/v0/b/pocket-hotel.appspot.com/o/dailyHelp%2FGift.mp4?alt=media&token=1bea0b96-dab5-4b87-90c2-e579f9b5c91e";
                break;
        }
        help.SetActive(true);
        vp.enabled = true;
        vp.url = tempUrl;
        vp.Play();
    }
    public void CloseHelp()
    {
        vp.Stop();
        help.SetActive(false);
        vp.enabled = false;
        rt.Release();
    }
    public void DoubleReward()
    {
        ClaimTodayReward();
        Fire.INS.ShowVideoReward(1);
    }
    public static float[] rewards =
    {
        40,50,50,50,50,
        50,20,250,2000,400,
        500,500,500,500,500,
        500,20,100,500,1000,
        0.05f,2500,1000,200,25,
        250,25,1500,1500
    };

    public void SetFacebookList(List<object> list)
    {
        invitedIds = new List<string>();
        for (int i = 0; i < list.Count; i++)
        {
            inviteImages[i].color = new Color(0.4f,0.5f,1);
            invitedIds.Add(list[i].ToString());
        }

    }
    public void CheckFacebookInvite(string temp)
    {
        if (invitedIds.Contains(temp))
            return;
        inviteImages[invitedIds.Count].color = new Color(0.4f, 0.5f, 1);
        switch (invitedIds.Count)
        {
            case 0:
                GC.INS.AddCoins(1000);
                GC.INS.ad.CoinAnim();
                break;
            case 1:
                GC.INS.AddGems(10);
                GC.INS.ad.NormalAnim();
                break;
            case 2:
                GC.INS.gift.AddGift(new Dictionary<string, object>
                {
                    { "id",24},
                    { "type", 0},
                    { "subtype", 4},
                });
                break;
            case 3:
                GC.INS.AddCoins(2000);
                GC.INS.ad.CoinAnim();
                break;
            case 4:
                GC.INS.AddGems(50);
                GC.INS.ad.NormalAnim();
                break;
            case 5:
                GC.INS.gift.AddGift(new Dictionary<string, object>
                {
                    { "id",32},
                    { "type", 0},
                    { "subtype", 2},
                });
                break;
            case 6:
                GC.INS.AddGems(100);
                GC.INS.ad.NormalAnim();
                break;
            case 7:
                GC.INS.gift.AddGift(new Dictionary<string, object>
                {
                    { "id",44},
                    { "type", 0},
                    { "subtype", 2},
                });
                break;
            case 8:
                GC.INS.gift.AddGift(new Dictionary<string, object>
                {
                    { "id",41},
                    { "type", 0},
                    { "subtype", 2},
                });
                break;
            case 9:
                GC.INS.AddGems(250);
                GC.INS.ad.NormalAnim();
                break;
        }
        invitedIds.Add(temp);
    }
    public void OpenInvites()
    {
        if (laptop.activeInHierarchy)
            return;
        laptop.SetActive(true);
        if (GC.INS.f.IsConnected())
        {
            inviteP.SetActive(true);
        }
        else
        {
            connectF.SetActive(true);
        }

    }
    public void OpenShare()
    {
        if (laptop.activeInHierarchy)
            return;
        laptop.SetActive(true);
        if (GC.INS.f.IsConnected())
        {
            shareP.SetActive(true);
        }
        else
        {
            connectF.SetActive(true);
        }
        
    }
    public void SetNoti()
    {
        if (GC.INS.noti)
            return;
        Invoke("SetNotificationsLaptop", 4);
    }
    void SetNotificationsLaptop()
    {
        content.SetActive(true);
        notiPanel = Instantiate(notiP, content.transform);
        laptop.SetActive(true);
    }
}
