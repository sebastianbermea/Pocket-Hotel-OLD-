using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudsC : MonoBehaviour
{
    public GameObject cloud;
    float timeToSpawn;
    public Sprite[] back0, back1, back2, back3, back4, back5;
    public SpriteRenderer[] sr;
    [HideInInspector]
    public List<int> itemsList = new List<int>();
    [HideInInspector]
    public bool[] backsUnlocked=new bool[6];
    bool[] erer = { true, false, false, false, false, false };
    public BackButton[] buttons;
    public ItemButton[] itemButtons = new ItemButton[15];
    public Camera mainCam;
    public Color[] colors;

    private void Awake()
    {
        backsUnlocked = erer;
    }
  
    public void SetBack(int x)
    {
        ChangeB(x);
        GC.INS.SetStars(-(GC.INS.backId * GC.INS.backId) * 50);
        GC.INS.backId = x;
        GC.INS.SetStars(GC.INS.backId * GC.INS.backId * 50);
    }
    
    private void FixedUpdate()
    {
        if (timeToSpawn <= 0)
        {
            Instantiate(cloud, transform);
            timeToSpawn = Random.Range(5, 13);
        }
        else
        {
            timeToSpawn -= Time.fixedDeltaTime;
        }
    }

    public void AddItem(int id)
    {
        for(int i=0; i<buttons.Length; i++)
        {
            buttons[i].PickedItem(id);
        }
        itemButtons[id].Add();
        itemsList.Add(id);
    }
    public void RemoveItem(int id)
    {
        itemsList.Remove(id);
        RemovedItem();
    }
    public void RemovedItem()
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].RemovedItem();
        }
    }
    public void ChangeB(int x)
    {
        Sprite[] temp;

        switch (x)
        {
            case 0:
                temp = back0;
                mainCam.backgroundColor = colors[0];
                break;
            case 1:
                temp = back1;
                mainCam.backgroundColor = colors[1];
                break;
            case 2:
                temp = back2;
                mainCam.backgroundColor = colors[0];
                break;
            case 3:
                temp = back3;
                mainCam.backgroundColor = colors[1];
                break;
            case 4:
                temp = back4;
                mainCam.backgroundColor = colors[0];
                break;
            case 5:
                temp = back5;
                mainCam.backgroundColor = colors[2];
                break;
            default:
                temp = back0;
                break;
        }
        sr[0].sprite = temp[0];
        sr[1].sprite = temp[0];
        sr[2].sprite = temp[1];
        sr[3].sprite = temp[1];
        sr[4].sprite = temp[2];
        for (int i = 5; i < sr.Length; i++)
        {
            sr[i].sprite = temp[3];
        }
    }
}
