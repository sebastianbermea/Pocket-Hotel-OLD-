using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Staff : MonoBehaviour
{
    public Animator anim, eyesAnim;
    public SpriteRenderer[] headParts, eyes, outfit, body;
    public SpriteRenderer mouthSr;
    public TextMeshPro nameText;
    public GameObject tempStaff, content, minusText;
    public int outfitId, number;
    public int id, hasAnimation;
    public SlotType animType;
    bool isMen, random, go, working, onRoom, taskCompleted, returning;
    public Character character;
    Room room;
    List<Slot> goToSlots = new List<Slot>();
    Slot currentSlot;
    float destination, speed = 0.3f;
    StaffBtn sb;
    OutfitButton ob;
    Transform parentTransform;
    Vector3 startPos;

    public GameObject restBtn, workBtn,shadowP;
    bool resting;

    //Maintenance
    Dust currentDust;
    Pipe currentPipe;
    Electricity currentElec;
    Complain currentComp;
    Keyloss currentKey;


    // Start is called before the first frame update
    void Awake()
    {
        if (!GC.INS.visit)
        {
            switch (id)
            {
                case 1:
                    GC.INS.AddJanitor(this);
                    break;
                case 2:
                    GC.INS.AddPlumber(this);
                    break;
                case 3:
                    GC.INS.AddElectric(this);
                    break;
                case 4:
                    GC.INS.AddOfficier(this);
                    break;
                case 5:
                    GC.INS.AddKeyBuilder(this);
                    break;
            }

        }

    }
    private void Start()
    {
        if (!GC.INS.visit)
        {
            if (character.isFriend)
            {
                GC.INS.staffFriendIDList.Add(character.id);
            }
        }
        else
        {
            if (character.isFriend)
            {
                VC.INS.CheckStaffName(character.name);
            }
            if (restBtn)
            {
                restBtn.SetActive(false);
                workBtn.SetActive(false);
            }
        }
        parentTransform = transform.parent.parent;
        startPos = transform.parent.localPosition;
        anim.speed = .7f + Random.Range(0, 11) * .06f;
        eyesAnim.speed = .7f + Random.Range(0, 11) * .06f;
    }
    private void OnEnable()
    {
        switch (animType)
        {
            case SlotType.Seat:
                anim.Play("Seat", -1, 0);
                break;

        }
    }
    public Character RandomGenerator(Room room, int number)
    {
        isMen = (Random.Range(0, 5) < 3);
        tempStaff.SetActive(true);
        nameText.gameObject.SetActive(false);
        int _mouthId = Random.Range(0, 4);
        if (Random.Range(0, 5) == 0)
            _mouthId = Random.Range(0, 7);
        mouthSr.sprite = SM.INS.mouths[_mouthId];
        int _eyesColor = Random.Range(0, 7);
        int _eyesId = Random.Range(0, 3);
        if (Random.Range(0, 5) == 0)
        {
            _eyesId = Random.Range(0, SM.INS.eyes.Length / 2);
        }
        int _outfitId = outfitId;
        SetOutfit(outfitId);
        int _hairId;

        if (isMen)
            _hairId = Random.Range(0, 17);
        else
            _hairId = Random.Range(16, 29);

        int _hairColor;
        if (Random.Range(0, 8) != 0)
            _hairColor = Random.Range(0, 8);
        else
            _hairColor = Random.Range(0, GC.INS.hairC.Length);
        int extraId = 0;
        int _extraColor = 0;
        if (isMen && Random.Range(0, 4) == 0)
        {
            extraId = Random.Range(0, SM.INS.beards.Length);
            if (extraId < 9)
            {
                if (Random.Range(0, 2) == 0)
                    _extraColor = Random.Range(0, GC.INS.hairC.Length);
                else
                    _extraColor = _hairColor;
            }


        }
        int glassId = 0;
        int glassColorId = 0;
        if (Random.Range(0, 6) == 0)
        {
            glassId = Random.Range(0, 4);
            glassColorId = Random.Range(0, GC.INS.armazonColor.Length);

        }
        int _bodyNumber = 0;
        if (Random.Range(0, 3) != 0)
        {
            _bodyNumber = Random.Range(0, 6);
        }
        this.room = room;
        character = new Character(0, _outfitId, _hairId, "temp", _hairColor, _eyesColor, 0, _bodyNumber, extraId, _extraColor, glassId, glassColorId, _mouthId, _eyesId, false);
        random = true;
        SetCharacter(character);
        this.number = number;
        return character;
    }
    public void SetCharacter(Character c)
    {
        //Head
        Sprite[] hairs = SM.INS.Hairs();
        headParts[0].sprite = hairs[c.hairId];
        headParts[0].color = GC.INS.hairC[c.hairColor];
        eyes[1].color = GC.INS.eyesC[c.eyeColor];
        eyes[0].sprite = SM.INS.eyes[c.eyesId * 2];
        eyes[1].sprite = SM.INS.eyes[c.eyesId * 2 + 1];
        mouthSr.sprite = SM.INS.mouths[c.mouthId];
        //Extra
        headParts[1].sprite = SM.INS.beards[c.extraId];
        if (c.extraId < 9)
            headParts[1].color = GC.INS.hairC[c.hairColor];
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
        body[0].sprite = tempBody[0];
        body[1].sprite = tempBody[1];
        body[2].sprite = tempBody[1];
        body[3].sprite = tempBody[2];
        body[4].sprite = tempBody[3];
        body[5].sprite = tempBody[4];
        if (c.skinColor == 4)
            mouthSr.color = new Color(0.8f, 0.8f, 0.8f);

        /* //Outfit
         if (c.isFriend)
         {
             Sprite[] tempOutfit = SM.INS.GetOutfit(character.outfitId);
             for (int i = 0; i < tempOutfit.Length; i++)
                 outfit[i].sprite = tempOutfit[i];
         }
        */
        nameText.text = c.name;
    }
    public void SetCharacterStart(Character c, Room room, int number)
    {
        if (!GC.INS.visit)
        {
            if (c.isFriend)
            {
                if (FRC.INS.friendsC.Count > c.id - 20)
                {
                    Character temp = FRC.INS.friendsC[c.id - 20];
                    c.id = temp.id;
                    c.hairId = temp.hairId;
                    c.extraColor = temp.extraColor;
                    c.extraId = temp.extraId;
                    c.eyeColor = temp.eyeColor;
                    c.eyesId = temp.eyesId;
                    c.glassColor = temp.glassColor;
                    c.glassColorId = temp.glassColorId;
                    c.glassId = temp.glassId;
                    c.hairColor = temp.hairColor;
                    c.mouthId = temp.mouthId;
                    c.name = temp.name;
                    c.skinColor = temp.skinColor;
                }
                else
                {
                    FRC.INS.friendsStaff.Add(this);
                }

            }
            if (c.id < 20)
            {
                GC.INS.SetWages(wages[c.id]);
            }
            else
            {
                GC.INS.SetWages(5);
            }
        }

        //Head
        Sprite[] hairs = SM.INS.Hairs();
        headParts[0].sprite = hairs[c.hairId];
        headParts[0].color = GC.INS.hairC[c.hairColor];
        eyes[1].color = GC.INS.eyesC[c.eyeColor];
        eyes[0].sprite = SM.INS.eyes[c.eyesId * 2];
        eyes[1].sprite = SM.INS.eyes[c.eyesId * 2 + 1];
        mouthSr.sprite = SM.INS.mouths[c.mouthId];
        //Extra
        headParts[1].sprite = SM.INS.beards[c.extraId];
        headParts[1].color = GC.INS.hairC[c.hairColor];

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
        body[0].sprite = tempBody[0];
        body[1].sprite = tempBody[1];
        body[2].sprite = tempBody[1];
        body[3].sprite = tempBody[2];
        body[4].sprite = tempBody[3];
        body[5].sprite = tempBody[4];
        if (c.skinColor == 4)
            mouthSr.color = new Color(0.8f, 0.8f, 0.8f);

        SetOutfit(c.outfitId);
        if (!GC.INS.visit)
            GC.INS.SetWages(oWages[c.outfitId]);

        character = c;
        if (c.id != 0)
        {
            nameText.text = c.name;
            tempStaff.SetActive(false);
            nameText.gameObject.SetActive(true);
        }
        else
        {
            random = true;
            tempStaff.SetActive(true);
            nameText.gameObject.SetActive(false);
        }

        this.room = room;
        this.number = number;
    }
    public void SetNewCharacter(Character c)
    {
        if (random)
        {
            tempStaff.SetActive(false);
            nameText.gameObject.SetActive(true);
        }
        else if (c.id == 0)
        {
            tempStaff.SetActive(true);
            nameText.gameObject.SetActive(false);
        }
        SetCharacter(c);
    }
    public void PurchasedCharacter(Character c)
    {
        //   Debug.Log("Purchased Character");
        if (character.id < 20)
            GC.INS.SetWages(-wages[character.id]);
        else
            GC.INS.SetWages(-5);
        if (c.id < 20)
            GC.INS.SetWages(wages[c.id]);
        else
            GC.INS.SetWages(5);

        if (c.isFriend)
        {
            GC.INS.staffFriendIDList.Add(c.id);
        }
        if (character.isFriend)
        {
            GC.INS.staffFriendIDList.Remove(character.id);
        }
        c.outfitId = character.outfitId;
        SetNewCharacter(c);
        character = c;
        random = false;
        if (c.id == 0)
            random = true;
        int cost;
        if (c.id < 20)
        {
            cost = costs[character.id];
        }
        else
        {
            cost = 500;
        }
        GC.INS.Purchase(cost);

        GC.INS.dm.AddTask(27, 1);
        TextMeshPro tempText = Instantiate(minusText, transform.parent.parent.parent).GetComponentInChildren<TextMeshPro>();
        tempText.transform.parent.position = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 1));

        if (cost < 0)
        {
            tempText.color = new Color(0.5f, 0.80f, 1f);
            tempText.text = cost.ToString("n0");
            GC.INS.SetStars(cost / 5);
            GC.INS.AddXp(-costs[id] / 5);
        }
        else
        {
            GC.INS.SetStars(cost / 100);
            tempText.text = "-" + cost.ToString("n0");
            GC.INS.AddXp(costs[id] / 1000);
        }


        room.ChangeStaff(character, number);
    }
    public void PurchasedCharacter(Character c, StaffBtn sb)
    {
        SC.INS.PlaySound(0, 12, 0);
        if (character.id < 20)
            GC.INS.SetWages(-wages[character.id]);
        else
            GC.INS.SetWages(-5);
        if (c.id < 20)
            GC.INS.SetWages(wages[c.id]);
        else
            GC.INS.SetWages(5);

        c.outfitId = character.outfitId;
        SetNewCharacter(c);
        character = c;
        random = false;
        if (c.id == 0)
            random = true;
        int cost;
        if (c.id < 20)
        {
            cost = costs[character.id];
        }
        else
        {
            cost = 500;
        }

        if (cost < 0)
            GC.INS.SetStars(cost / 5);
        else
            GC.INS.SetStars(cost / 100);

        sb.Purchased();
        sb = null;
        room.ChangeStaff(character, number);
    }
    public void ResetCharacter()
    {
        if (random)
        {
            tempStaff.SetActive(true);
            nameText.gameObject.SetActive(false);
        }
        else if (character.id != 0)
        {
            tempStaff.SetActive(false);
            nameText.gameObject.SetActive(true);
        }
        SetCharacter(character);
    }
    public void SetOutfit(int id)
    {
        Sprite[] tempOutfit = SM.INS.GetOutfit(id);
        for (int i = 0; i < tempOutfit.Length; i++)
            outfit[i].sprite = tempOutfit[i];
    }
    public void ResetOutfit()
    {
        Sprite[] tempOutfit = SM.INS.GetOutfit(character.outfitId);
        for (int i = 0; i < tempOutfit.Length; i++)
            outfit[i].sprite = tempOutfit[i];
    }
    public void PurchasedOutfit(int id)
    {
        GC.INS.SetWages(-oWages[character.outfitId]);
        SetOutfit(id);
        character.outfitId = id;
        GC.INS.SetWages(oWages[character.outfitId]);
        //Debug.Log("Purchased Outfit");
        if (ob == null)
        {
            GC.INS.Purchase(StaffOutfit.costs[id]);
            TextMeshPro tempText = Instantiate(minusText, transform.parent.parent.parent).GetComponentInChildren<TextMeshPro>();
            tempText.transform.parent.position = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 1));
            GC.INS.dm.AddTask(18, 1);
            if (StaffOutfit.costs[id] < 0)
            {
                tempText.color = new Color(0.5f, 0.80f, 1f);
                tempText.text = StaffOutfit.costs[id].ToString("n0");
                GC.INS.AddXp(-costs[id] / 5);
            }
            else
            {
                tempText.text = "-" + StaffOutfit.costs[id].ToString("n0");
                GC.INS.AddXp(costs[id] / 1000);
            }
        }
        else
        {
            SC.INS.PlaySound(0, 12, 0);
            ob.Purchased();
            ob = null;
        }

        room.ChangeStaff(character, number);
    }

    public void GiftOutfit(OutfitButton obn)
    {
        ob = obn;
        PurchasedOutfit(ob.id);
    }
    public void GetToWork(Slot slot)
    {
        if (!GC.INS.visit)
        {
            switch (id)
            {
                case 1:
                    if (GC.INS.dust.Contains(slot.id))
                    {
                        Work(slot);
                    }
                    break;
                case 2:
                    if (GC.INS.pipe.Contains(slot.id))
                    {
                        Work(slot);
                    }
                    break;
                case 3:
                    if (GC.INS.electricity.Contains(slot.id))
                    {
                        Work(slot);
                    }
                    break;
                case 4:
                    if (GC.INS.complaint.Contains(slot.id))
                    {
                        Work(slot);
                    }
                    break;
                case 5:
                    if (GC.INS.key.Contains(slot.id))
                    {
                        Work(slot);
                    }
                    break;
            }

        }
        else
        {
            switch (id)
            {
                case 1:
                    if (VC.INS.dust.Contains(slot.id))
                    {
                        Work(slot);
                    }
                    break;
                case 2:
                    if (VC.INS.pipe.Contains(slot.id))
                    {
                        Work(slot);
                    }
                    break;
                case 3:
                    if (VC.INS.electricity.Contains(slot.id))
                    {
                        Work(slot);
                    }
                    break;
                case 4:
                    if (VC.INS.complaint.Contains(slot.id))
                    {
                        Work(slot);
                    }
                    break;
                case 5:
                    if (VC.INS.key.Contains(slot.id))
                    {
                        Work(slot);
                    }
                    break;
            }
        }

    }

    void Work(Slot slot)
    {
        if (!goToSlots.Contains(slot))
            goToSlots.Add(slot);
        if (!working && !resting)
        {
            working = true;
            onRoom = false;
            taskCompleted = false;
            returning = false;
            speed = .3f;
            currentSlot = slot;
            RemoveObject();

            if (IsInvoking("Go"))
            {
                CancelInvoke("Go");
            }

            Invoke("Go", 1f);
            destination = 0;
        }
    }
    void RemoveObject()
    {
        if (!GC.INS.visit)
        {
            switch (id)
            {
                case 1:
                    GC.INS.dust.Remove(currentSlot.id);
                    break;
                case 2:
                    GC.INS.pipe.Remove(currentSlot.id);
                    break;
                case 3:
                    GC.INS.electricity.Remove(currentSlot.id);
                    break;
                case 4:
                    GC.INS.complaint.Remove(currentSlot.id);
                    break;
                case 5:
                    GC.INS.key.Remove(currentSlot.id);
                    break;
            }
        }
        else
        {
            switch (id)
            {
                case 1:
                    VC.INS.dust.Remove(currentSlot.id);
                    break;
                case 2:
                    VC.INS.pipe.Remove(currentSlot.id);
                    break;
                case 3:
                    VC.INS.electricity.Remove(currentSlot.id);
                    break;
                case 4:
                    VC.INS.complaint.Remove(currentSlot.id);
                    break;
                case 5:
                    VC.INS.key.Remove(currentSlot.id);
                    break;
            }
        }
    }
    void ContinueWorking()
    {
        anim.ResetTrigger("Walk");
        currentSlot = goToSlots[0];

        RemoveObject();
        onRoom = false;
        taskCompleted = false;
        DestinationReached();
    }
    void Go()
    {
        if (IsInvoking("Go"))
        {
            Debug.Log("Error");
            return;
        }
        if (speed > 0)
            content.transform.localScale = new Vector3(0.35f, 0.35f, 1);
        else
            content.transform.localScale = new Vector3(-0.35f, 0.35f, 1);
        anim.SetTrigger("Walk");
        go = true;
    }
    private void FixedUpdate()
    {
        if (go)
        {
            if (Mathf.Abs(transform.parent.localPosition.x - destination) > 0.15f)
            {
                transform.parent.Translate(Vector2.right * speed * Time.fixedDeltaTime);
            }
            else
            {
                go = false;
                DestinationReached();
            }
        }
    }
    void DestinationReached()
    {
        if (!onRoom)
        {
            onRoom = true;
            transform.parent.position = currentSlot.spawnPos.position;
            transform.parent.parent = currentSlot.spawnPos;
            content.transform.localScale = new Vector3(-0.35f, 0.35f, 1);
            switch (id)
            {
                case 1:
                    destination = currentSlot.pos.x + 0.15f;
                    currentDust = currentSlot.spawnPos.GetComponentInChildren<Dust>();
                    break;
                case 2:
                    destination = 0.1f - currentSlot.spawnPos.transform.localPosition.x;
                    currentPipe = currentSlot.spawnPos.parent.GetComponentInChildren<Pipe>();
                    break;
                case 3:
                    destination = 0.1f - currentSlot.spawnPos.transform.localPosition.x;
                    currentElec = currentSlot.spawnPos.parent.GetComponentInChildren<Electricity>();
                    break;
                case 4:
                    destination = 0.1f - currentSlot.spawnPos.transform.localPosition.x;
                    currentComp = currentSlot.spawnPos.parent.GetComponentInChildren<Complain>();
                    break;
                case 5:
                    destination = 0.1f - currentSlot.spawnPos.transform.localPosition.x;
                    currentKey = currentSlot.spawnPos.parent.GetComponentInChildren<Keyloss>();
                    break;
            }

            speed = -0.5f;
            go = true;
        }
        else if (!returning)
        {
            if (!taskCompleted)
            {
                switch (id)
                {
                    case 1:
                        InvokeRepeating("Clean", 0, .5f);
                        break;
                    case 2:
                        if (currentPipe == null)
                        {
                            FinishPipe();
                        }
                        else
                            currentPipe.StartFix(this);
                        break;
                    case 3:

                        if (currentElec == null)
                            FinishPipe();
                        else
                            currentElec.StartFix(this);
                        break;
                    case 4:
                        if (currentComp == null)
                            FinishPipe();
                        else
                            currentComp.StartFix(this);
                        break;
                    case 5:

                        if (currentKey == null)
                            FinishPipe();
                        else
                            currentKey.StartFix(this);
                        break;
                }

                anim.SetTrigger("Fix");
            }
            else
            {
                WorkCompleted();
            }

        }
        else
        {
            content.transform.localScale = new Vector3(0.35f, 0.35f, 1);
            if (!resting)
            {
                anim.SetTrigger("Iddle");
                anim.ResetTrigger("Walk");
            }
            else
            {
                anim.SetTrigger("Sleep");
            }
            
        }
    }
    void Clean()
    {
        if (currentDust == null)
        {
            CancelInvoke("Clean");
            taskCompleted = true;
            speed = 0.4f;
            destination = 0;
            Invoke("Go", .8f);
            return;
        }
        if (currentDust.Cleaner())
        {
            CancelInvoke("Clean");
            taskCompleted = true;
            speed = 0.4f;
            destination = 0;
            Invoke("Go", .8f);
        }

    }
    void OnDestroy()
    {
        if (working && currentSlot != null)
        {
            GC.INS.ResetSlot(currentSlot);
        }
        switch (id)
        {
            case 1:
                GC.INS.janitors.Remove(this);
                break;
            case 2:
                GC.INS.plumbers.Remove(this);
                break;
            case 3:
                GC.INS.electicists.Remove(this);
                break;
            case 4:
                GC.INS.officinist.Remove(this);
                break;
            case 5:
                GC.INS.keyBuilder.Remove(this);
                break;
        }
        if (id != 0)
        {
            Destroy(gameObject);
        }
    }
    public void FinishPipe()
    {
        taskCompleted = true;
        speed = 0.4f;
        destination = 0;
        Invoke("Go", .5f);
    }
    void WorkCompleted()
    {
        if (!GC.INS.visit)
        {
            GC.INS.ResetSlot(currentSlot);
            goToSlots.RemoveAt(0);
            bool check = false;
            List<Slot> tempSlots = new List<Slot>();
            if (goToSlots.Count > 0)
            {
                for (int i = 0; i < goToSlots.Count; i++)
                {
                    switch (id)
                    {
                        case 1:
                            if (!GC.INS.dust.Contains(goToSlots[i].id))
                            {
                                tempSlots.Add(goToSlots[i]);
                            }
                            else
                            {
                                check = true;
                            }
                            break;
                        case 2:
                            if (!GC.INS.pipe.Contains(goToSlots[i].id))
                            {
                                tempSlots.Add(goToSlots[i]);
                            }
                            else
                            {
                                check = true;
                            }
                            break;
                        case 3:
                            if (!GC.INS.electricity.Contains(goToSlots[i].id))
                            {
                                tempSlots.Add(goToSlots[i]);
                            }
                            else
                            {
                                check = true;
                            }
                            break;
                        case 4:
                            if (!GC.INS.complaint.Contains(goToSlots[i].id))
                            {
                                tempSlots.Add(goToSlots[i]);
                            }
                            else
                            {
                                check = true;
                            }
                            break;
                        case 5:
                            if (!GC.INS.key.Contains(goToSlots[i].id))
                            {
                                tempSlots.Add(goToSlots[i]);
                            }
                            else
                            {
                                check = true;
                            }
                            break;
                    }


                }
            }


            for (int i = 0; i < tempSlots.Count; i++)
            {
                goToSlots.Remove(tempSlots[i]);
            }
            if (check && !resting)
            {
                ContinueWorking();
            }
            else
            {
                working = false;
                transform.parent.parent = parentTransform;
                transform.parent.position = parentTransform.position;
                currentSlot = null;
                returning = true;
                transform.parent.localPosition = new Vector3(transform.parent.localPosition.x - .05f, startPos.y, startPos.z);
                destination = startPos.x - .1f;
                speed = -.3f;
                content.transform.localScale = new Vector3(-0.35f, 0.35f, 1);
                go = true;

            }
        }
        else
        {
            VC.INS.ResetSlot(currentSlot);
            goToSlots.RemoveAt(0);
            bool check = false;
            List<Slot> tempSlots = new List<Slot>();
            if (goToSlots.Count > 0)
            {
                for (int i = 0; i < goToSlots.Count; i++)
                {
                    switch (id)
                    {
                        case 1:
                            if (!VC.INS.dust.Contains(goToSlots[i].id))
                            {
                                tempSlots.Add(goToSlots[i]);
                            }
                            else
                            {
                                check = true;
                            }
                            break;
                        case 2:
                            if (!VC.INS.pipe.Contains(goToSlots[i].id))
                            {
                                tempSlots.Add(goToSlots[i]);
                            }
                            else
                            {
                                check = true;
                            }
                            break;
                        case 3:
                            if (!VC.INS.electricity.Contains(goToSlots[i].id))
                            {
                                tempSlots.Add(goToSlots[i]);
                            }
                            else
                            {
                                check = true;
                            }
                            break;
                        case 4:
                            if (!VC.INS.complaint.Contains(goToSlots[i].id))
                            {
                                tempSlots.Add(goToSlots[i]);
                            }
                            else
                            {
                                check = true;
                            }
                            break;
                        case 5:
                            if (!VC.INS.key.Contains(goToSlots[i].id))
                            {
                                tempSlots.Add(goToSlots[i]);
                            }
                            else
                            {
                                check = true;
                            }
                            break;
                    }


                }
            }


            for (int i = 0; i < tempSlots.Count; i++)
            {
                goToSlots.Remove(tempSlots[i]);
            }
            if (check)
            {
                ContinueWorking();
            }
            else
            {
                working = false;
                transform.parent.parent = parentTransform;
                transform.parent.position = parentTransform.position;
                currentSlot = null;
                returning = true;
                transform.parent.localPosition = new Vector3(transform.parent.localPosition.x - .05f, startPos.y, startPos.z);
                destination = startPos.x - .1f;
                speed = -.3f;
                content.transform.localScale = new Vector3(-0.35f, 0.35f, 1);
                go = true;

            }
        }

    }

    void CheckFromResting()
    {
        bool check = false;
        List<Slot> tempSlots = new List<Slot>();
        if (goToSlots.Count > 0)
        {
            for (int i = 0; i < goToSlots.Count; i++)
            {
                switch (id)
                {
                    case 1:
                        if (!GC.INS.dust.Contains(goToSlots[i].id))
                        {
                            tempSlots.Add(goToSlots[i]);
                        }
                        else
                        {
                            check = true;
                        }
                        break;
                    case 2:
                        if (!GC.INS.pipe.Contains(goToSlots[i].id))
                        {
                            tempSlots.Add(goToSlots[i]);
                        }
                        else
                        {
                            check = true;
                        }
                        break;
                    case 3:
                        if (!GC.INS.electricity.Contains(goToSlots[i].id))
                        {
                            tempSlots.Add(goToSlots[i]);
                        }
                        else
                        {
                            check = true;
                        }
                        break;
                    case 4:
                        if (!GC.INS.complaint.Contains(goToSlots[i].id))
                        {
                            tempSlots.Add(goToSlots[i]);
                        }
                        else
                        {
                            check = true;
                        }
                        break;
                    case 5:
                        if (!GC.INS.key.Contains(goToSlots[i].id))
                        {
                            tempSlots.Add(goToSlots[i]);
                        }
                        else
                        {
                            check = true;
                        }
                        break;
                }


            }
        }
       
        for (int i = 0; i < tempSlots.Count; i++)
        {
            goToSlots.Remove(tempSlots[i]);
        }
        if (check)
        {
            GetToWork(goToSlots[0]);
        }
    }
    public static List<Character> staffList = new List<Character>()
    {
        new Character(0,0,0,"temp",0, 0,0,0,0,0,0,0,0,0, false),
        new Character(1,0,2,"Jonathan",1, 0,0,0,0,0,0,0,0,0, false),
        new Character(2,0,1,"Henry",6, 3,0,1,0,0, 0,0,1,0,false),
        new Character(3,0,21,"Monica",2, 2,0,0,0,0,0,0,0,1,false),
        new Character(4,0,7,"Lukas",4, 0,0,5,0,0,3,0,0,2, false),
        new Character(5,0,18,"Isabel",2, 4,0,0,9,0,0,0,2,0, false),
        new Character(6,0,17,"Emily",10,0,3,1,0,0,0,0,0,2, false),
        new Character(7,0,0,"Adrian",3, 0,1,4,4,0,0,0,0,0, false),
        new Character(8,0,22,"Mia",4, 0,1,3,10,0,0,0,0,0, false),
        new Character(9,0,3,"Gaston",10, 9,1,5,13,0,0,5,0,1, false),
        new Character(10,0,25,"Sophia",11,2,4,4,0,0,0,1,0,1, false),
        new Character(11,0,9,"Adam",14, 1,1,2,1,14,1,1,0,1, false),
        new Character(12,0,26,"Sarah",12, 6,0,0,0,0,0,0,5,4, false),
        new Character(13,0,10,"Raul",11, 0,1,0,0,2,0,0,0,7, false),
        new Character(14,0,13,"Cesar",15, 0,3,0,0,2,3,0,0,3, false),
        new Character(15,0,16,"Auron",4, 0,1,0,6,2,0,2,0,0, false),
        new Character(16,0,20,"Angela",10, 0,0,0,0,0,1,0,3,6, false),
        new Character(17,0,24,"Amanda",11, 0,5,1,0,2,3,3,7,2, false),
        new Character(18,0,4,"Roy",4, 3,1,0,12,2,0,0,0,0, false),
        new Character(19,0,0,"Joe",0, 0,1,6,0,2,0,0,0,1, false),
    };
    public static int[] wages =
    {
        30,25,25,25,25,
        25,20,20,17,17,
        15,15,13,13,11,
        11,10,10,8,5,5
    };
    public static int[] oWages =
   {
        0,0,0,0,0,
        0,0,0,0,0,
        -2,-2,-1,-1,-1,
        //15
        -2,-1,-1,-2,-2,
        -2,-3,-2,-3,-3,
        -2,-4,-4,-5,-5,

    };
    public static int[] costs =
    {
		//When negative, its gems
		250,1500,2000,2000,2500,
        2500,5000,5000,6500,6500,
        7500,7500,9000,9000,10000,
        10000,-10,-10,-20,-50,-50,

    };

    public void Rest(bool rest)
    {
        if (!restBtn)
            return;

        resting = rest;
        restBtn.SetActive(!rest);
        workBtn.SetActive(rest);
        if (!rest)
        {
            shadowP.SetActive(false);
            CheckFromResting();
            anim.Play("CIddle", -1, 0);
        }
        else
        {
            shadowP.SetActive(true);
            if (!working)
            {
                anim.SetTrigger("Walk");
                anim.SetTrigger("Sleep");
            }
           
        }
    }
}
public class Character
{
    bool _isFriend;
    int _id, _outfitId, _hairId, _skinColor, _extraId, _glassId, _glassColorId, _mouthId, _eyesId, _extraColor;
    string _name;
    int _hairColor, _eyeColor, _glassColor;
    public Character(int id, int outfitId, int hairId, string name, int hairColor, int eyeColor, int glassColor,
        int skinColor, int extraId, int extraColor, int glassId, int glassColorId, int mouthId, int eyesId, bool isFriend)
    {
        _id = id;
        _outfitId = outfitId;
        _hairColor = hairColor;
        _hairId = hairId;
        _name = name;
        _eyeColor = eyeColor;
        _skinColor = skinColor;
        _extraId = extraId;
        _extraColor = extraColor;
        _glassId = glassId;
        _glassColorId = glassColorId;
        _glassColor = glassColor;
        _mouthId = mouthId;
        _eyesId = eyesId;
        _isFriend = isFriend;
    }

    public int id
    {
        get { return _id; }
        set { _id = value; }
    }
    public int outfitId
    {
        get { return _outfitId; }
        set { _outfitId = value; }
    }
    public int hairId
    {
        get { return _hairId; }
        set { _hairId = value; }
    }
    public int extraId
    {
        get { return _extraId; }
        set { _extraId = value; }
    }
    public int extraColor
    {
        get { return _extraColor; }
        set { _extraColor = value; }
    }
    public int glassId
    {
        get { return _glassId; }
        set { _glassId = value; }
    }
    public int glassColorId
    {
        get { return _glassColorId; }
        set { _glassColorId = value; }
    }
    public int mouthId
    {
        get { return _mouthId; }
        set { _mouthId = value; }
    }
    public int eyesId
    {
        get { return _eyesId; }
        set { _eyesId = value; }
    }
    public string name
    {
        get { return _name; }
        set { _name = value; }
    }
    public int hairColor
    {
        get { return _hairColor; }
        set { _hairColor = value; }
    }
    public int eyeColor
    {
        get { return _eyeColor; }
        set { _eyeColor = value; }
    }
    public int glassColor
    {
        get { return _glassColor; }
        set { _glassColor = value; }
    }
    public int skinColor
    {
        get { return _skinColor; }
        set { _skinColor = value; }
    }
    public bool isFriend
    {
        get { return _isFriend; }
    }

}