using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Costumer : MonoBehaviour
{
    bool isGoing, onRoom = false, onPositon = false, onFinish = false, goingOut = false, notAvaible, checkAvaible, checkingOut;
    Vector3 destination;
    float speed = 1, tempSpeed, fallSpeed, fallSpeedX;
    float time, startTime, timeBarStartScale, timeToDrag;
    public GameObject timeBar, timer, coin, content, messageBox, thrower, coinObj;
    public Slot slot;
    public SpriteRenderer mouthSR, message;
    float limitX, limitXR;
    public Animator anim, eyesAnim;
    public SpriteRenderer[] outfit;
    public SpriteRenderer[] shirtParts;
    public SpriteRenderer[] pantsParts;
    public SpriteRenderer[] bodys;
    public SpriteRenderer[] headParts, eyes;
    public Color[] pantsColors;
    public Color[] hairColors;
    bool isMen, tip, dragging, falling, mouseDown, isOutfit;
    float voice;
    Vector2 screenPoint;
    Camera mainCam;
    Rigidbody2D rig;
    public GameObject[] services;
    System.DateTime roomTime;
    public TextMeshPro nameText;
    public GameObject item;
    bool hasName, tutorial;
    public bool vreward;
    int boost=1;
    public BoxCollider2D bc;

    public GameObject tipClicker;

    //public Color[] shirtColors;
    private void Awake()
    {
        if (!GC.INS.visit)
        {
            if (GC.INS.iap[1])
            {
                speed *= 1.5f;
                boost = 2;
            }
        }
        else
        {
            if (VC.INS.iap[1])
            {
                speed *= 1.5f;
                boost = 2;
            }
        }
        
    }
    private void Start()
    {
        rig = GetComponent<Rigidbody2D>();
        timeBarStartScale = timeBar.transform.localScale.x;
        mainCam = Camera.main;
        if (vreward)
            return;
        if (FRC.INS.friendList.Count > 0)
        {
            int tempC = FRC.INS.friendList.Count;
            if (tempC > 10)
                tempC = 10;
            if (!GC.INS.visit)
            {
                if (Random.Range(0, 28 - tempC + (GC.INS.slotsID.Count/3)) == 0)
                {
                    FriendVisist(FRC.INS.friendsC[Random.Range(0, FRC.INS.friendsC.Count)]);
                }
                else
                {
                    RandomGenerator();
                }
            }
            else
            {
                if(Random.Range(0,15)==0 && !VC.INS.userVisit)
                {
                    VC.INS.SetUserVisit();
                    FriendVisist(GC.INS.player);
                }
                else if(Random.Range(0,45)==0)
                    FriendVisist(FRC.INS.friendsC[Random.Range(0, FRC.INS.friendsC.Count)]);
                else
                    RandomGenerator();

            }

        }
        else
        {
            RandomGenerator();
        }

        if (!onRoom)
            anim.SetTrigger("Walk");
        anim.speed = .9f + Random.Range(0, 11) * .02f;
        eyesAnim.speed = .7f + Random.Range(0, 11) * .06f;
        
    }

    private void RandomGenerator()
    {
        isMen = (Random.Range(0, 5) < 3);
        if (isMen)
            voice = Random.Range(0.6f, 1.1f);
        else
            voice = Random.Range(0.9f, 1.5f);
        mouthSR.sprite = SM.INS.mouths[0];
        if (Random.Range(0, 2) == 0)
        {
            if (Random.Range(0, 5) != 0)
                mouthSR.sprite = SM.INS.mouths[Random.Range(0, 4)];
            else
                mouthSR.sprite = SM.INS.mouths[Random.Range(0, SM.INS.mouths.Length)];
        }

        eyes[1].color = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
        int eyesId = Random.Range(0, 3);
        if (Random.Range(0, 4) == 0)
        {
            eyesId = Random.Range(0, SM.INS.eyes.Length / 2);
        }
        eyes[0].sprite = SM.INS.eyes[eyesId * 2];
        eyes[1].sprite = SM.INS.eyes[eyesId * 2 + 1];
        if (Random.Range(0, 3) != 0)
        {
            int bodyNumber = Random.Range(0, 6);
            if (Random.Range(0, 4) == 0)
                bodyNumber = Random.Range(0, 8);
            Sprite[] tempBody = SM.INS.Bodys(bodyNumber);
            bodys[0].sprite = tempBody[0];
            bodys[1].sprite = tempBody[1];
            bodys[2].sprite = tempBody[1];
            bodys[3].sprite = tempBody[2];
            bodys[4].sprite = tempBody[3];
            bodys[5].sprite = tempBody[4];
            if (bodyNumber == 4)
                mouthSR.color = new Color(0.8f, 0.8f, 0.8f);

        }

        //Hair
        Sprite[] hairs = SM.INS.Hairs();
        if (isMen)
            headParts[0].sprite = hairs[Random.Range(0, 17)];
        else
            headParts[0].sprite = hairs[Random.Range(16, 29)];

        Color hairColor;
        if (Random.Range(0, 5) != 0)
        {
            hairColor = hairColors[Random.Range(0, hairColors.Length)];
        }
        else
        {
            hairColor = new Color(Random.Range(0f, .9f), Random.Range(0f, .9f), Random.Range(0f, .9f));
        }


        headParts[0].color = hairColor;
        int extId;
        if (isMen)
        {
            if (Random.Range(0, 3) == 0)
            {
                
                extId = Random.Range(0, SM.INS.beards.Length);
                headParts[1].sprite = SM.INS.beards[extId];
                if (extId < 9)
                {
                    headParts[1].color = hairColor;
                }

            }
        }
        else
        {
            if (Random.Range(0, 4) == 0)
            {
                extId = Random.Range(9, SM.INS.beards.Length);
                headParts[1].sprite = SM.INS.beards[extId];
            }
        }
        


        //Glasses
        if (Random.Range(0, 5) == 0)
        {
            int glassesType = Random.Range(1, 4);
            headParts[2].sprite = SM.INS.glasses[glassesType * 2];
            if (Random.Range(0, 2) == 0)
            {
                Color glassColor = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f), 0.9f);
                headParts[3].sprite = SM.INS.glasses[glassesType * 2 + 1];
                headParts[4].sprite = SM.INS.glasses[glassesType * 2 + 1];
                headParts[3].color = glassColor;
                headParts[4].color = glassColor;
                headParts[2].color = new Color(Random.Range(0f, .7f), Random.Range(0f, .7f), Random.Range(0f, .7f));
            }
            else
            {
                headParts[2].color = new Color(Random.Range(0f, .3f), Random.Range(0f, .3f), Random.Range(0f, .3f));
            }
        }

        //Regular outfit or special
        if (Random.Range(0, 10) != 0)
        {
            Sprite[] outfit = SM.INS.RegularOutfit();
            Color tempColor = new Color(Random.Range(0f, 0.85f), Random.Range(0f, 0.85f), Random.Range(0f, 0.85f));
            if (Random.Range(0, 12) == 0)
            {
                if (Random.Range(0, 3) != 0)
                    tempColor = new Color(1, 1, 1);
                else
                    tempColor = new Color(.1f, .1f, .1f);
            }
            for (int i = 0; i < 3; i++)
            {
                shirtParts[i].color = tempColor;
            }
            //Normal or V neck
            if (Random.Range(0, 3) != 0)
            {
                shirtParts[2].sprite = outfit[4];
            }
            else
            {
                shirtParts[2].sprite = outfit[5];
            }

            //Sleeve, large sleeve or sleeveless
            int sleeve = Random.Range(0, 7);
            if (sleeve < 3)
            {
                shirtParts[0].sprite = outfit[0];
                shirtParts[1].sprite = outfit[1];
            }
            else if (sleeve < 6)
            {
                shirtParts[0].sprite = outfit[2];
                shirtParts[1].sprite = outfit[3];
            }
            else
            {
                shirtParts[0].sprite = null;
                shirtParts[1].sprite = null;
            }
            //Extra
            if (Random.Range(0, 5) < 3)
            {
                if (Random.Range(0, 2) == 0)
                {
                    shirtParts[3].sprite = outfit[Random.Range(10, 17)];
                }
                else
                {
                    int extraType = Random.Range(0, 5);
                    if (extraType < 2)
                    {
                        shirtParts[3].sprite = outfit[Random.Range(6, 8)];
                        shirtParts[3].color = new Color(tempColor.r + 0.1f, tempColor.g + 0.1f, tempColor.b + 0.1f);
                    }
                    else if (extraType == 2)
                    {
                        shirtParts[3].sprite = outfit[8];
                        shirtParts[3].color = new Color(Random.Range(0f, 0.5f), Random.Range(0f, 0.5f), Random.Range(0f, 0.5f));
                    }
                    else
                    {
                        shirtParts[3].sprite = outfit[9];
                        shirtParts[3].color = tempColor;
                        if (Random.Range(0, 2) == 0)
                            shirtParts[2].color = Color.white;
                        else
                            shirtParts[2].color = new Color(0.25f, 0.25f, 0.25f);
                        shirtParts[2].sprite = outfit[5];
                    }

                }
            }

            //Pants
            Sprite[] pantsOutfit = SM.INS.pants;
            int tempPants = Random.Range(0, pantsColors.Length);
            for (int i = 0; i < 3; i++)
            {
                pantsParts[i].color = pantsColors[tempPants];
            }
            if (Random.Range(0, 5) == 0)
            {
                pantsParts[1].sprite = null;
                pantsParts[2].sprite = null;

            }
            pantsParts[0].sprite = pantsOutfit[0];
            if (!isMen)
            {
                pantsParts[0].sprite = pantsOutfit[7];
                if (Random.Range(0, 5) != 0)
                {
                    pantsParts[1].sprite = null;
                    pantsParts[2].sprite = null;

                }
                else
                {
                    pantsParts[1].color = pantsColors[2];
                    pantsParts[2].color = pantsColors[2];
                }
            }
            //Shoes
            Color shoeColor = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
            if (Random.Range(0, 5) == 0)
            {
                if (Random.Range(0, 2) == 0)
                    shoeColor = new Color(1, 1, 1);
                else
                    shoeColor = new Color(.1f, .1f, .1f);
            }
            pantsParts[3].color = shoeColor;
            pantsParts[4].color = shoeColor;
            if (Random.Range(0, 3) == 0)
            {
                int r = Random.Range(3, 7);
                pantsParts[5].sprite = pantsOutfit[r];
                pantsParts[6].sprite = pantsOutfit[r];
            }

        }
        else
        {
            shirtParts[3].enabled = false;
            pantsParts[0].enabled = false;
            pantsParts[3].enabled = false;
            pantsParts[4].enabled = false;
            pantsParts[5].enabled = false;
            pantsParts[6].enabled = false;
            isOutfit = true;
            Sprite[] tempoutfit = SM.INS.GetRandomOutfit();
            for (int i = 0; i < tempoutfit.Length; i++)
                outfit[i].sprite = tempoutfit[i];
        }
    }

    void FriendVisist(Character c)
    {
        //Head
        Sprite[] hairs = SM.INS.Hairs();
        headParts[0].sprite = hairs[c.hairId];
        headParts[0].color = GC.INS.hairC[c.hairColor];
        eyes[1].color = GC.INS.eyesC[c.eyeColor];
        eyes[0].sprite = SM.INS.eyes[c.eyesId * 2];
        eyes[1].sprite = SM.INS.eyes[c.eyesId * 2 + 1];
        mouthSR.sprite = SM.INS.mouths[c.mouthId];
        //Extra
        headParts[1].sprite = SM.INS.beards[c.extraId];
        if (c.extraId < 9)
            headParts[1].color = GC.INS.hairC[c.extraColor];
        else
            headParts[1].color = Color.white;

        //Glasses
        headParts[2].sprite = SM.INS.glasses[c.glassId * 2];
        headParts[2].color = GC.INS.armazonColor[c.glassColorId];
        if (c.glassId > 0 && c.glassColor > 0)
        {
            headParts[3].sprite = SM.INS.glasses[c.glassId * 2 + 1];
            headParts[4].sprite = SM.INS.glasses[c.glassId * 2 + 1];
            headParts[3].color = GC.INS.glassColor[c.glassColor];
            headParts[4].color = GC.INS.glassColor[c.glassColor];
        }

        //Body
        Sprite[] tempBody = SM.INS.Bodys(c.skinColor);
        bodys[0].sprite = tempBody[0];
        bodys[1].sprite = tempBody[1];
        bodys[2].sprite = tempBody[1];
        bodys[3].sprite = tempBody[2];
        bodys[4].sprite = tempBody[3];
        bodys[5].sprite = tempBody[4];
        if (c.skinColor == 4)
            mouthSR.color = new Color(0.8f, 0.8f, 0.8f);

        //Outfit
        Sprite[] tempOutfit = SM.INS.GetOutfit(c.outfitId);
        for (int i = 0; i < tempOutfit.Length; i++)
            outfit[i].sprite = tempOutfit[i];

        nameText.gameObject.SetActive(true);
        nameText.text = c.name;
        hasName = true;
        if(content.transform.localScale.x<0)
            nameText.transform.localScale = new Vector2(-1, 1);
    }

    public void Create(bool going, float limitXL, float limitXR)
    {
        isGoing = going;
        limitX = limitXL - 0.8f;
        this.limitXR = limitXR + 0.7f;
        if (vreward)
        {
            float y = Random.Range(-0.3f, -1f);
            transform.parent.position = new Vector3(limitX, y, y - (Random.Range(0, 10) * .01f));
            destination.x = Random.Range(0.5f,2.5f);
            destination.y = y;
            messageBox.SetActive(true);
            if (gameObject.activeInHierarchy)
                messageBox.GetComponent<Animator>().ResetTrigger("Off");
            return;
        }
        if (!isGoing)
        {
            float x;
            float y = Random.Range(-0.3f, -1f);
            if (Random.Range(0, 5) == 0)
                Invoke("Tip", Random.Range(2, 7));
            else if(GC.INS.level>3)
            {
                if (Random.Range(0, 180 + GC.INS.level) == 0)
                {
                    Instantiate(item, content.transform);
                }
            }
            if (Random.Range(0, 3) == 0)
            {
                x = limitX;
                limitX = x;
                destination.x = limitXR;

            }
            else
            {
                x = limitXR;
                destination.x = limitX;
                speed *= -1;
                content.transform.localScale = new Vector3(-0.35f, 0.35f, 1);
                if(hasName)
                    nameText.transform.localScale = new Vector2(-1, 1);
            }
            destination.y = y;
            transform.parent.position = new Vector3(x, y, y - (Random.Range(0, 10) * .01f));
        }
        else
        {
            transform.parent.position = new Vector3(limitX, -0.2f, (Random.Range(0, 49) * .002f));
            destination.x = 2f;
            destination.y = -0.2f;
        }

    }
    
    private void FixedUpdate()
    {
        if (dragging)
            return;
        if (vreward)
        {
            if (Mathf.Abs(transform.parent.position.x - destination.x) > 0.1f && !onPositon)
            {
                transform.parent.Translate(Vector2.right * speed * Time.fixedDeltaTime);
            }
            else if(!onPositon)
            {
                onPositon = true;
                anim.SetTrigger("Dance");
            }
            if (goingOut)
            {
                if (transform.parent.position.x > limitX)
                {
                    transform.parent.Translate(Vector2.right * speed * Time.fixedDeltaTime);
                }
                else
                {

                    DestoyCostumer();
                }
            }
            if (transform.parent.position.x > 25 || transform.parent.position.x < -20)
                DestoyCostumer();
            return;
        }
        if (destination.y < transform.parent.position.y && falling)
        {
            transform.parent.Translate(Vector2.down * fallSpeed * Time.fixedDeltaTime);
            transform.parent.Translate(Vector2.right * fallSpeedX * Time.fixedDeltaTime);
            fallSpeed += 20 * Time.fixedDeltaTime;
            return;
        }
        else if (falling)
        {
            if (isGoing)
            {
                transform.parent.position = new Vector3(transform.position.x, -0.2f, transform.parent.position.z);
            }
            falling = false;
            tempSpeed = speed;
            anim.SetTrigger("Ground");
            SC.INS.PlaySound(1, 4, voice);
            Invoke("WalkAgain", .9f);
            speed = 0;
            fallSpeedX = 0;
            transform.parent.position = new Vector3(transform.parent.position.x, transform.parent.position.y, transform.parent.position.y - (Random.Range(0, 10) * .01f));
        }
        if (Mathf.Abs(transform.parent.position.x - destination.x) > 0.1f && !onRoom && !goingOut && !notAvaible)
        {
            transform.parent.Translate(Vector2.right * speed * Time.fixedDeltaTime);
            if (transform.parent.position.x > 0.2f && isGoing && !checkAvaible && CheckAvaible())
                NotAvaible();
        }
        else if (!onRoom && !notAvaible)
        {
            if (isGoing)
                SetRoom();
            else
                DestoyCostumer();
        }
        else
        {
            if (slot != null)
            {

                if (Mathf.Abs(transform.parent.localPosition.x - slot.pos.x) > 0.05f && !onPositon)
                {
                    transform.parent.Translate(Vector2.right * speed * Time.fixedDeltaTime);
                }
                else if (!onPositon)
                {
                    SetActivity();
                }
                if (onPositon)
                {
                    if (time < startTime)
                    {
                        time = (float)(System.DateTime.UtcNow - roomTime).TotalSeconds * boost;
                        Vector3 temp = timeBar.transform.localScale;
                        temp.x = ((startTime - time) / startTime) * timeBarStartScale;
                        timeBar.transform.localScale = temp;
                    }
                    else
                    {
                        //Debug.Log(startTime - time);
                        if (startTime - time < -10)
                        {
                            int randomAdd = (int)(startTime - time) / -10;
                            if (Random.Range(0, randomAdd) != 0)
                            {
                                if (!GC.INS.visit)
                                    GC.INS.AddCoins(slot.coins);
                                else
                                    VC.INS.AddCoins(0, transform.parent.position);

                                if (Random.Range(0, 3) == 0 && slot.type == SlotType.Sleep)
                                    SetMainten();
                                else
                                    SetFromMiddle(randomAdd * Random.Range(1, 5));
                                return;
                            }
                            else
                            {
                                if (!GC.INS.visit)
                                {
                                    GC.INS.AddCoins(slot.coins);
                                    GC.INS.ResetSlot(slot);
                                }
                                else
                                {
                                    VC.INS.AddCoins(0, Vector2.zero);
                                    VC.INS.ResetSlot(slot);
                                }

                                DestoyCostumer();
                            }
                        }
                        if (!onFinish)
                            Bye();

                        if (checkingOut && !goingOut)
                        {
                            if (Mathf.Abs(transform.parent.position.x - slot.spawnPos.position.x) > 0.05f)
                            {
                                transform.parent.Translate(Vector2.right * -speed * Time.fixedDeltaTime);

                            }
                            else
                            {
                                Out();
                            }
                        }
                    }
                }

            }
            

        }
        if (goingOut)
        {
            if (transform.parent.position.x > limitX)
            {
                transform.parent.Translate(Vector2.right * speed * Time.fixedDeltaTime);
            }
            else
            {

                DestoyCostumer();
            }
        }
        if (transform.parent.position.x > 25 || transform.parent.position.x < -20)
            DestoyCostumer();
    }
    void SetActivity()
    {
        //Debug.Log(slot.type.ToString());
        anim.Play("CWalk", -1, 0);

        switch (slot.type)
        {
            case SlotType.Sleep:
                anim.SetTrigger("Sleep");
                Invoke("SoundSleep", .5f);
                break;
            case SlotType.Seat:
                anim.SetTrigger("Seat");
                break;
            case SlotType.Fix:
                anim.SetTrigger("Fix");
                break;
            case SlotType.Iddle:
                anim.SetTrigger("Iddle");
                break;
            case SlotType.Lift:
                anim.SetTrigger("Gym");
                break;
            case SlotType.Dance:
                anim.SetTrigger("Dance");
                break;
            case SlotType.Swim:
                anim.SetTrigger("Swim");
                timer.transform.parent = content.transform.GetChild(0).transform;
                timer.transform.localPosition = new Vector3(1.4f + Random.Range(0, 5) * .05f, -.2f, 0);
                PutSwimSuit();
                break;
        }
        if (slot.right)
        {
            content.transform.localScale = new Vector3(0.35f, 0.35f, 1);
            if (hasName)
                nameText.transform.localScale = new Vector2(1, 1);
        }
        Vector3 temp = transform.parent.localPosition;
        temp.z = slot.zPos;
        transform.parent.localPosition = temp;
        transform.parent.localScale = Vector3.one;
        timer.SetActive(true);
        //time = slot.time;
        startTime = slot.time;
        onPositon = true;
        roomTime = System.DateTime.UtcNow;
    }
    void SoundSleep()
    {
        if(GC.INS.slotsID.Count<10)
            SC.INS.PlaySound(1, 0, voice);
    }
    void SetRoom()
    {
        if (!onRoom)
        {
            if (!GC.INS.visit)
                slot = GC.INS.SetCostumer(false);
            else
                slot = VC.INS.SetCostumer();

            if (slot != null)
            {
                onRoom = true;
                transform.parent.position = slot.spawnPos.position;
                transform.parent.parent = slot.spawnPos;
                transform.parent.localPosition = new Vector3(transform.parent.localPosition.x, slot.pos.y, slot.pos.z);
                speed /= -2f;
                content.transform.localScale = new Vector3(-0.35f, 0.35f, 1);
                if (hasName)
                    nameText.transform.localScale = new Vector2(-1, 1);
                transform.parent.localScale = Vector3.one;
                SC.INS.PlaySound(1, 6, 0);
            }
            else
            {
                if (!notAvaible)
                    NotAvaible();
            }
        }
    }
    private void OnEnable()
    {
        if (slot != null && !onFinish && onPositon)
        {
            SetAnim();
            if (vreward)
            {
                FinishVideoReward();
            }
        }
        else if (slot == null)
        {
            anim.SetTrigger("Walk");
        }
        
    }
    public void TipClick()
    {
        if (IsInvoking("Tip"))
            CancelInvoke("Tip");
        if (vreward && !goingOut)
        {
            Invoke("MessagePop", .2f);
            messageBox.GetComponent<Animator>().SetTrigger("Pop");
            GC.INS.ad.SetNote();
            onPositon = true;
        }
        if (tip)
        {
            tip = false;
            if (!GC.INS.visit)
            {
                GC.INS.dm.AddTask(6, 1);
                if (GC.INS.level > 0)
                    GC.INS.AddXp(1);
                GC.INS.AddCoins(1);
            }
            else
                VC.INS.AddCoins(1, transform.parent.position);
            Invoke("MessagePop", .2f);
            messageBox.GetComponent<Animator>().SetTrigger("Pop");
            Instantiate(coinObj, new Vector3(transform.parent.position.x, transform.parent.position.y + 0.3f, 0), transform.rotation);
        }
        timeToDrag = 0.2f;
        mouseDown = true;
    }
    private void OnMouseDown()
    {
        if (IsInvoking("Tip"))
            CancelInvoke("Tip");
        if (vreward && !goingOut)
        {
            Invoke("MessagePop", .2f);
            messageBox.GetComponent<Animator>().SetTrigger("Pop");
            GC.INS.ad.SetNote();
            onPositon = true;
        }
        if (tip)
        {
            tip = false;
            if (!GC.INS.visit)
            {
                GC.INS.dm.AddTask(6, 1);
                if(GC.INS.level>0)
                    GC.INS.AddXp(1);
                GC.INS.AddCoins(1);
            }
            else
                VC.INS.AddCoins(1, transform.parent.position);
            Invoke("MessagePop", .2f);
            messageBox.GetComponent<Animator>().SetTrigger("Pop");
            Instantiate(coinObj, new Vector3(transform.parent.position.x, transform.parent.position.y + 0.3f, 0), transform.rotation);
        }
        timeToDrag = 0.2f;
        mouseDown = true;
    }
   public void FinishVideoReward()
   {
        if (goingOut)
            return;
        goingOut = true;
        speed *= -1;
        anim.Play("CIddle", -1, 0);
        anim.Play("CIddle", -1, 0);
        anim.SetTrigger("Walk");
        if (messageBox.activeInHierarchy)
        {
            MessagePop();
        }
        content.transform.localScale = new Vector3(-0.35f, 0.35f, 1);
    }
    
    private void OnMouseDrag()
    {
        if (onRoom || goingOut || notAvaible || !mouseDown || messageBox.activeInHierarchy || falling || speed == 0 || vreward)
            return;
        if (timeToDrag > 0)
        {
            timeToDrag -= Time.deltaTime;
            return;
        }
        if (!dragging)
        {
            anim.ResetTrigger("Walk");
            anim.SetTrigger("Drag");
            if (content.transform.localScale.x > 0)
                content.transform.localScale = new Vector3(0.7f, 0.7f, 1);
            else
                content.transform.localScale = new Vector3(-0.7f, 0.7f, 1);
            dragging = true;
            GC.INS.isDragging = true;
            thrower.SetActive(true);
            SC.INS.PlaySound(1, 1, voice);
        }
        screenPoint = mainCam.ScreenToWorldPoint(new Vector2(Input.mousePosition.x, Input.mousePosition.y));
        transform.parent.position = new Vector3(screenPoint.x, screenPoint.y, -2f);
    }
    private void OnMouseUp()
    {
        if (!dragging)
            return;
        dragging = false;


        if (content.transform.localScale.x > 0)
            content.transform.localScale = new Vector3(0.35f, 0.35f, 1);
        else
            content.transform.localScale = new Vector3(-0.35f, 0.35f, 1);
        if (transform.parent.position.x > -0.5f && transform.parent.position.x < 2.6 && transform.parent.position.y < 0.75f && transform.parent.position.y > -0.6f)
        {
            float tempPos = 1.85f;
            if (transform.parent.position.x < 1.7f)
            {
                tempPos = transform.parent.position.x;
            }
            transform.parent.position = new Vector3(tempPos, -.2f, (Random.Range(0, 49) * .002f));
            destination.x = 2f;
            destination.y = -0.2f;
            isGoing = true;
            anim.SetTrigger("Walk");
            if (speed < 0)
            {
                speed *= -1;
                content.transform.localScale = new Vector3(0.35f, 0.35f, 1);
                if (hasName)
                    nameText.transform.localScale = new Vector2(1, 1);
            }
            GC.INS.dm.AddTask(26, 1);
        }
        else
        {
            if (isGoing)
            {
                destination.x = limitXR;
                isGoing = false;
                destination.y = -0.5f;
            }
            if (transform.parent.position.y > -0.2f)
            {
                anim.SetTrigger("Falling");
                falling = true;
                fallSpeed = 5;
                Vector2 tempDistance = transform.parent.position - thrower.transform.position;
                float cameraFactor = mainCam.orthographicSize;
                fallSpeedX = tempDistance.x * 3f;
                fallSpeed = 6 + tempDistance.y * -8f;
                fallSpeed -= tempDistance.y * (5f-cameraFactor);

                if (fallSpeed < -22)
                    fallSpeed = -22;
                bc.enabled = false;
                bc.enabled = true;
                SC.INS.PlaySound(1, 3, voice);
                GC.INS.dm.AddTask(24, 1);
            }
            else
            {
                transform.parent.position = new Vector3(transform.parent.position.x, transform.parent.position.y, transform.parent.position.y - (Random.Range(0, 10) * .01f));
                anim.SetTrigger("Walk");
            }
        }
        thrower.SetActive(false);
        thrower.transform.parent = transform.parent;
        thrower.transform.localPosition = Vector2.zero;
        mouseDown = false;
        GC.INS.isDragging = false;
    }
    void Tip()
    {
        if (isGoing || dragging || falling || mouseDown)
            return;
        tip = true;
        tempSpeed = speed;
        anim.SetTrigger("Message");
        speed = 0;
        tipClicker.SetActive(true);
        messageBox.SetActive(true);
        if(gameObject.activeInHierarchy)
             messageBox.GetComponent<Animator>().ResetTrigger("Off");
        message.sprite = SM.INS.messages[1];
        Invoke("WalkAgain", 2f);
        SC.INS.PlaySound(1, 2, voice);
    }
    public void ForceTutorial()
    {
        tutorial = true;
        time = startTime + 1;
        Bye();
    }
    
    void MessagePop()
    {
        message.sprite = SM.INS.messages[0];
        messageBox.SetActive(false);
    }
    void WalkAgain()
    {
        anim.SetTrigger("Walk");
        speed = tempSpeed;
    }
    void Bye()
    {
        onFinish = true;
        timer.SetActive(false);
        transform.parent.localPosition = new Vector3(slot.pos.x, slot.pos.y, slot.pos.z+0.05f);
        anim.Play("CIddle", -1, 0);
        anim.SetTrigger("Message");
        if(tutorial && GC.INS.tuto.current==18 && GC.INS.level>1)
        {
            SC.INS.PlaySound(1, 2, voice);
            Recomendate();
            GC.INS.tuto.Next();
            tutorial = false;
        }
        if (Random.Range(0, 3) == 0 && !GC.INS.visit && !tutorial)
        {
            SC.INS.PlaySound(1, 2, voice);
            Recomendate();
        }
        Invoke("CheckOut", 2f);
    }
    public bool CheckAvaible()
    {
        checkAvaible = true;
        if (!GC.INS.visit)
            return (GC.INS.slotsID.Count == 0 || !GC.INS.work);
        else
            return (VC.INS.slotsID.Count == 0);
    }
    public void CheckOut()
    {
        if (!GC.INS.visit)
            GC.INS.AddCoins(slot.coins);
        else
            VC.INS.AddCoins(0, Vector2.zero);
        //Services
        if (tutorial)
        {
            if (slot.type == SlotType.Sleep)
            {
                Instantiate(services[0], transform.parent.parent).GetComponentInChildren<Dust>().Create(slot);
            }
        }
        else
        {
            if (slot.type == SlotType.Sleep && Random.Range(0, 2) == 0)
            {
                switch (Random.Range(0, 5))
                {
                    case 0:
                        Instantiate(services[0], transform.parent.parent).GetComponentInChildren<Dust>().Create(slot);
                        break;
                    case 1:
                        Instantiate(services[1], transform.parent.parent.parent).GetComponentInChildren<Pipe>().Create(slot);
                        break;
                    case 2:
                        Instantiate(services[2], transform.parent.parent.parent).GetComponentInChildren<Electricity>().Create(slot);
                        break;
                    case 3:
                        Instantiate(services[3], transform.parent.parent.parent).GetComponentInChildren<Complain>().Create(slot);
                        break;
                    case 4:
                        Instantiate(services[4], transform.parent.parent.parent).GetComponentInChildren<Keyloss>().Create(slot);
                        break;
                }
            }
            else
            {
                if (!GC.INS.visit)
                    GC.INS.ResetSlot(slot);
                else
                    VC.INS.ResetSlot(slot);
            }
        }
       
        if(slot.type == SlotType.Swim)
        {
            RemoveSwimSuit();
        }
        coin.SetActive(true);
        checkingOut = true;
        anim.SetTrigger("Walk");
        coin.transform.parent = transform.parent.parent;
        content.transform.localScale = new Vector3(0.35f, 0.35f, 1);
        if (hasName)
            nameText.transform.localScale = new Vector2(1, 1);
    }
    private void OnDisable()
    {
        if (checkingOut)
            DestoyCostumer();
    }
    public void DestoyCostumer()
    {
        Destroy(transform.parent.gameObject);
    }
    public void NotAvaible()
    {
        notAvaible = true;
        speed *= -1;
        anim.SetTrigger("Message");
        messageBox.SetActive(true);
        Invoke("Out", 2f);
        SC.INS.PlaySound(1, 1, voice);
    }
    public void Out()
    {
        anim.SetTrigger("Walk");
        if (checkingOut)
        {
            if (!GC.INS.visit)
                transform.parent.parent = GC.INS.costumersArrange.transform;
            else
                transform.parent.parent = VC.INS.costumersArrange.transform;
            transform.parent.position = new Vector3(2, -0.2f, (Random.Range(0, 20) * .005f));
            speed *= 2;
        }
        messageBox.SetActive(false);
        goingOut = true;
        content.transform.localScale = new Vector3(-0.35f, 0.35f, 1);
        if (hasName)
            nameText.transform.localScale = new Vector2(-1, 1);
    }
    public void OutRoom()
    {
        anim.SetTrigger("Walk");
    }
    void Recomendate()
    {
        if (GC.INS.level > 2 && !GC.INS.haveCinema)
        {
            messageBox.SetActive(true);
            message.sprite = SM.INS.messages[4];
        }
        if (GC.INS.level > 1 && !GC.INS.haveRestaurant)
        {
            messageBox.SetActive(true);
            message.sprite = SM.INS.messages[3];
        }
        if (GC.INS.level > 0 && !GC.INS.haveGym)
        {
            messageBox.SetActive(true);
            message.sprite = SM.INS.messages[2];
        }  
    }
    void SetAnim()
    {
        anim.ResetTrigger("Walk");
        switch (slot.type)
        {
            case SlotType.Sleep:
                anim.Play("Sleeping", -1, 0);
                break;
            case SlotType.Seat:
                anim.Play("Seat", -1, 0);
                break;
            case SlotType.Fix:
                anim.Play("Fixing", -1, 0);
                break;
            case SlotType.Iddle:
                anim.Play("CIddle", -1, 0);
                break;
            case SlotType.Run:
                anim.SetTrigger("Walk");
                break;
            case SlotType.Lift:
                anim.Play("Gym", -1, 0);
                break;
            case SlotType.Dance:
                anim.Play("Dance", -1, Random.Range(0, 0.95f));
                break;
            case SlotType.Swim:
                anim.Play("Swim", -1, Random.Range(0, 0.95f));
                timer.transform.parent = content.transform.GetChild(0).transform;
                timer.transform.localPosition = new Vector3(1.4f + Random.Range(0, 5) * .05f, -.2f, 0);
                PutSwimSuit();
                break;
        }
    }
    void SetFromMiddle(int time)
    {
        if (time > startTime)
            time = Random.Range(0, (int)startTime);
        onRoom = true;
        onPositon = true;
        System.DateTime tempDate = System.DateTime.UtcNow;
        roomTime = tempDate.AddSeconds(-time);
        this.time = 0;
        RandomGenerator();
    }
    public void SetMaintenance()
    {
        if (!GC.INS.visit)
        {
            slot = GC.INS.SetCostumer(true);
            if (slot == null)
            {
                DestoyCostumer();
                return;
            }
            if (slot.type != SlotType.Sleep)
            {
                GC.INS.ResetSlot(slot);
                DestoyCostumer();
                return;
            }
        }
        else
        {
            slot = VC.INS.SetCostumer();
            if (slot == null)
            {
                DestoyCostumer();
                return;
            }
            if (slot.type != SlotType.Sleep)
            {
                VC.INS.ResetSlot(slot);
                DestoyCostumer();
                return;
            }
        }

        transform.parent.parent = slot.spawnPos;
        content.transform.localScale = new Vector3(-0.35f, 0.35f, 1);
        if (hasName)
            nameText.transform.localScale = new Vector2(-1, 1);
        transform.parent.localScale = Vector3.one;
        Vector3 temp = transform.parent.localPosition;
        temp.z = 0.05f + Random.Range(0, 10) * 0.01f;
        transform.parent.localPosition = temp;
        SetMainten();

    }
    void SetMainten()
    {
        if (!GC.INS.visit)
        {
            switch (Random.Range(0, 5))
            {
                case 0:
                    if (GC.INS.janitors.Count > 0)
                        switch (Random.Range(1, 5))
                        {
                            case 1:
                                Instantiate(services[1], transform.parent.parent.parent).GetComponentInChildren<Pipe>().Create(slot);
                                break;
                            case 2:
                                Instantiate(services[2], transform.parent.parent.parent).GetComponentInChildren<Electricity>().Create(slot);
                                break;
                            case 3:
                                Instantiate(services[3], transform.parent.parent.parent).GetComponentInChildren<Complain>().Create(slot);
                                break;
                            case 4:
                                Instantiate(services[4], transform.parent.parent.parent).GetComponentInChildren<Keyloss>().Create(slot);
                                break;
                        }
                    else
                        Instantiate(services[0], transform.parent.parent).GetComponentInChildren<Dust>().Create(slot);
                    break;
                case 1:
                    if (GC.INS.plumbers.Count > 0)
                        switch (Random.Range(1, 5))
                        {
                            case 1:
                                Instantiate(services[0], transform.parent.parent).GetComponentInChildren<Dust>().Create(slot);
                                break;
                            case 2:
                                Instantiate(services[2], transform.parent.parent.parent).GetComponentInChildren<Electricity>().Create(slot);
                                break;
                            case 3:
                                Instantiate(services[3], transform.parent.parent.parent).GetComponentInChildren<Complain>().Create(slot);
                                break;
                            case 4:
                                Instantiate(services[4], transform.parent.parent.parent).GetComponentInChildren<Keyloss>().Create(slot);
                                break;
                        }
                    else
                        Instantiate(services[1], transform.parent.parent.parent).GetComponentInChildren<Pipe>().Create(slot);
                    break;
                case 2:
                    if (GC.INS.electicists.Count > 0)
                        switch (Random.Range(1, 5))
                        {
                            case 1:
                                Instantiate(services[0], transform.parent.parent).GetComponentInChildren<Dust>().Create(slot);
                                break;
                            case 2:
                                Instantiate(services[1], transform.parent.parent.parent).GetComponentInChildren<Pipe>().Create(slot);
                                break;
                            case 3:
                                Instantiate(services[3], transform.parent.parent.parent).GetComponentInChildren<Complain>().Create(slot);
                                break;
                            case 4:
                                Instantiate(services[4], transform.parent.parent.parent).GetComponentInChildren<Keyloss>().Create(slot);
                                break;
                        }
                    else
                        Instantiate(services[2], transform.parent.parent.parent).GetComponentInChildren<Electricity>().Create(slot);
                    break;
                case 3:
                    if (GC.INS.officinist.Count > 0)
                        switch (Random.Range(1, 5))
                        {
                            case 1:
                                Instantiate(services[0], transform.parent.parent).GetComponentInChildren<Dust>().Create(slot);
                                break;
                            case 2:
                                Instantiate(services[1], transform.parent.parent.parent).GetComponentInChildren<Pipe>().Create(slot);
                                break;
                            case 3:
                                Instantiate(services[2], transform.parent.parent.parent).GetComponentInChildren<Electricity>().Create(slot);
                                break;
                            case 4:
                                Instantiate(services[4], transform.parent.parent.parent).GetComponentInChildren<Keyloss>().Create(slot);
                                break;
                        }
                    else
                        Instantiate(services[3], transform.parent.parent.parent).GetComponentInChildren<Complain>().Create(slot);
                    break;
                case 4:
                    if (GC.INS.keyBuilder.Count > 0)
                        switch (Random.Range(1, 5))
                        {
                            case 1:
                                Instantiate(services[0], transform.parent.parent).GetComponentInChildren<Dust>().Create(slot);
                                break;
                            case 2:
                                Instantiate(services[1], transform.parent.parent.parent).GetComponentInChildren<Pipe>().Create(slot);
                                break;
                            case 3:
                                Instantiate(services[2], transform.parent.parent.parent).GetComponentInChildren<Electricity>().Create(slot);
                                break;
                            case 4:
                                Instantiate(services[3], transform.parent.parent.parent).GetComponentInChildren<Complain>().Create(slot);
                                break;
                        }
                    else
                    {
                        Instantiate(services[4], transform.parent.parent.parent).GetComponentInChildren<Keyloss>().Create(slot);
                    }
                    break;
            }
        }
        else
        {
            switch (Random.Range(0, 5))
            {
                case 0:
                    if (VC.INS.janitors.Count > 0)
                        switch (Random.Range(1, 5))
                        {
                            case 1:
                                Instantiate(services[1], transform.parent.parent.parent).GetComponentInChildren<Pipe>().Create(slot);
                                break;
                            case 2:
                                Instantiate(services[2], transform.parent.parent.parent).GetComponentInChildren<Electricity>().Create(slot);
                                break;
                            case 3:
                                Instantiate(services[3], transform.parent.parent.parent).GetComponentInChildren<Complain>().Create(slot);
                                break;
                            case 4:
                                Instantiate(services[4], transform.parent.parent.parent).GetComponentInChildren<Keyloss>().Create(slot);
                                break;
                        }
                    else
                        Instantiate(services[0], transform.parent.parent).GetComponentInChildren<Dust>().Create(slot);
                    break;
                case 1:
                    if (VC.INS.plumbers.Count > 0)
                        switch (Random.Range(1, 5))
                        {
                            case 1:
                                Instantiate(services[0], transform.parent.parent).GetComponentInChildren<Dust>().Create(slot);
                                break;
                            case 2:
                                Instantiate(services[2], transform.parent.parent.parent).GetComponentInChildren<Electricity>().Create(slot);
                                break;
                            case 3:
                                Instantiate(services[3], transform.parent.parent.parent).GetComponentInChildren<Complain>().Create(slot);
                                break;
                            case 4:
                                Instantiate(services[4], transform.parent.parent.parent).GetComponentInChildren<Keyloss>().Create(slot);
                                break;
                        }
                    else
                        Instantiate(services[1], transform.parent.parent.parent).GetComponentInChildren<Pipe>().Create(slot);
                    break;
                case 2:
                    if (VC.INS.electicists.Count > 0)
                        switch (Random.Range(1, 5))
                        {
                            case 1:
                                Instantiate(services[0], transform.parent.parent).GetComponentInChildren<Dust>().Create(slot);
                                break;
                            case 2:
                                Instantiate(services[1], transform.parent.parent.parent).GetComponentInChildren<Pipe>().Create(slot);
                                break;
                            case 3:
                                Instantiate(services[3], transform.parent.parent.parent).GetComponentInChildren<Complain>().Create(slot);
                                break;
                            case 4:
                                Instantiate(services[4], transform.parent.parent.parent).GetComponentInChildren<Keyloss>().Create(slot);
                                break;
                        }
                    else
                        Instantiate(services[2], transform.parent.parent.parent).GetComponentInChildren<Electricity>().Create(slot);
                    break;
                case 3:
                    if (VC.INS.officinist.Count > 0)
                        switch (Random.Range(1, 5))
                        {
                            case 1:
                                Instantiate(services[0], transform.parent.parent).GetComponentInChildren<Dust>().Create(slot);
                                break;
                            case 2:
                                Instantiate(services[1], transform.parent.parent.parent).GetComponentInChildren<Pipe>().Create(slot);
                                break;
                            case 3:
                                Instantiate(services[2], transform.parent.parent.parent).GetComponentInChildren<Electricity>().Create(slot);
                                break;
                            case 4:
                                Instantiate(services[4], transform.parent.parent.parent).GetComponentInChildren<Keyloss>().Create(slot);
                                break;
                        }
                    else
                        Instantiate(services[3], transform.parent.parent.parent).GetComponentInChildren<Complain>().Create(slot);
                    break;
                case 4:
                    if (VC.INS.keyBuilder.Count > 0)
                        switch (Random.Range(1, 5))
                        {
                            case 1:
                                Instantiate(services[0], transform.parent.parent).GetComponentInChildren<Dust>().Create(slot);
                                break;
                            case 2:
                                Instantiate(services[1], transform.parent.parent.parent).GetComponentInChildren<Pipe>().Create(slot);
                                break;
                            case 3:
                                Instantiate(services[2], transform.parent.parent.parent).GetComponentInChildren<Electricity>().Create(slot);
                                break;
                            case 4:
                                Instantiate(services[3], transform.parent.parent.parent).GetComponentInChildren<Complain>().Create(slot);
                                break;
                        }
                    else
                    {
                        Instantiate(services[4], transform.parent.parent.parent).GetComponentInChildren<Keyloss>().Create(slot);
                    }
                    break;
            }
        }


        DestoyCostumer();
    }
    public void SetFromStart(int time, float limitXL, float limitXR)
    {
        if (!GC.INS.visit)
            slot = GC.INS.SetCostumer(true);
        else
            slot = VC.INS.SetCostumer();

        if (slot == null)
        {
            DestoyCostumer();
            return;
        }
        eyes[0].GetComponentInParent<Animator>().speed = Random.Range(0.7f, 1.3f);
        limitX = limitXL-0.8f;
        this.limitXR = limitXR+0.5f;
        onRoom = true;
        transform.parent.parent = slot.spawnPos;
        content.transform.localScale = new Vector3(-0.35f, 0.35f, 1);
        transform.parent.localScale = Vector3.one;
    
        speed /= -2f;
        if (Random.Range(0, 10) != 0)
        {
            onPositon = true;
            if (slot.right)
            {
                content.transform.localScale = new Vector3(0.35f, 0.35f, 1);
                if (hasName)
                    nameText.transform.localScale = new Vector2(-1, 1);
            }
            transform.parent.localPosition = slot.pos;
            Vector3 temp = transform.parent.localPosition;
            temp.z = slot.zPos;
            transform.parent.localPosition = temp;
            timer.SetActive(true);
            startTime = slot.time;
            int minusTime;
            if (time < 100)
            {
                minusTime = Random.Range(0, time);
            }
            else
            {
                minusTime = Random.Range(0, slot.time);
            }
            System.DateTime tempDate = System.DateTime.UtcNow;
            roomTime = tempDate.AddSeconds(-minusTime);
            SetAnim();
        }
        else
        {
            anim.SetTrigger("Walk");
            transform.parent.position = slot.spawnPos.position;
            transform.parent.localPosition = new Vector3(transform.parent.localPosition.x, slot.pos.y,slot.pos.z);

        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (falling && collision.gameObject.tag == "Cloud")
            collision.gameObject.GetComponent<Cloud>().Puff();
    }
    private void OnApplicationPause(bool pause)
    {
        if (!onRoom)
        {
            if (pause)
            {
                roomTime = System.DateTime.UtcNow;
            }
            else
            {
                float time = (float)(System.DateTime.UtcNow - roomTime).TotalSeconds;
                if (time > 5)
                {
                    if (isGoing)
                    {
                        if (!GC.INS.visit)
                            slot = GC.INS.SetCostumer(false);
                        else
                            slot = VC.INS.SetCostumer();
                        if (slot == null)
                        {
                            DestoyCostumer();
                            return;
                        }
                        onRoom = true;
                        transform.parent.parent = slot.spawnPos;
                        content.transform.localScale = new Vector3(-0.35f, 0.35f, 1);
                        if (hasName)
                            nameText.transform.localScale = new Vector2(-1, 1);
                        transform.parent.localScale = Vector3.one;
                        speed /= -2f;
                        transform.parent.localPosition = slot.pos;
                    
                        timer.SetActive(true);
                        startTime = slot.time;
                        onPositon = true;
                        System.DateTime tempDate = System.DateTime.UtcNow;
                        roomTime = tempDate.AddSeconds(-time);
                        this.time = 0;
                        SetAnim();
                        //SetFromMiddle((int)time);
                    }
                    else
                    {
                        DestoyCostumer();
                    }

                }
            }
        }


    }
    public void CreateRandom(float limitXL, float limitXR)
    {
        eyes[0].GetComponentInParent<Animator>().speed = Random.Range(0.7f, 1.3f);
        limitX = limitXL - 0.8f;
        this.limitXR = limitXR+0.6f;
        float x;
        float y = Random.Range(-0.3f, -1f);
        if (Random.Range(0, 8) == 0)
            Invoke("Tip", Random.Range(2, 7));
        if (Random.Range(0, 3) == 0)
        {
            x = limitXL-0.8f;
            limitX = x;
            destination.x = limitXR;
        }
        else
        {
            x = limitXR;
            destination.x = limitX;
            speed *= -1;
            content.transform.localScale = new Vector3(-0.35f, 0.35f, 1);
            if (hasName)
                nameText.transform.localScale = new Vector2(-1, 1);
        }
        destination.y = y;
        transform.parent.position = new Vector3(Random.Range(limitX, limitXR), y, y - (Random.Range(0, 10) * .01f));

    }
    #region swimSuit
    void PutSwimSuit()
    {
        if (isOutfit)
        {
            outfit[0].enabled = false;
            outfit[1].enabled = false;
            outfit[2].enabled = false;
            outfit[5].enabled = false;
        }
        if (isMen)
        {
            for (int i = 0; i < shirtParts.Length; i++)
            {
                shirtParts[i].enabled = false;
            }
            for (int i = 1; i < pantsParts.Length; i++)
            {
                pantsParts[i].enabled = false;
            }
        }
        else
        {

            shirtParts[0].enabled = false;
            shirtParts[1].enabled = false;
            shirtParts[3].enabled = false;
            for (int i = 0; i < pantsParts.Length; i++)
            {
                pantsParts[i].enabled = false;
            }
        }
    }
    void RemoveSwimSuit()
    {
        if (isOutfit)
        {
            outfit[0].enabled = true;
            outfit[1].enabled = true;
            outfit[2].enabled = true;
            outfit[5].enabled = true;
        }
        if (isMen)
        {
            for (int i = 0; i < shirtParts.Length; i++)
            {
                shirtParts[i].enabled = true;
            }
            for (int i = 1; i < pantsParts.Length; i++)
            {
                pantsParts[i].enabled = true;
            }
        }
        else
        {

            shirtParts[0].enabled = true;
            shirtParts[1].enabled = true;
            shirtParts[3].enabled = true;
            for (int i = 0; i < pantsParts.Length; i++)
            {
                pantsParts[i].enabled = true;
            }
        }
    }
    #endregion

    public void Zeppeling(Vector2 pos)
    {
        transform.parent.position = new Vector3(pos.x, pos.y, (Random.Range(0, 49) * .002f));
        anim.SetTrigger("Drag");
        anim.SetTrigger("Falling");
        falling = true;
        fallSpeedX = Random.Range(-1.5f,1.5f);
        fallSpeed = 3 + Random.Range(0.5f,3f) * -6f;
        destination.y = -0.1f;
    }
}
