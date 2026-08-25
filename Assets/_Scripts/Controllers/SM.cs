using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SM : MonoBehaviour
{
    public static SM INS { get; private set; }
    Sprite[] roomBacks, beds, floors, objs, upObjs, downObjs;
    public Sprite[] mouths;
    List<Sprite[]> outfits = new List<Sprite[]>();
    Sprite[] regularOutfit;
    public Sprite[] pants;
    Sprite[] hair;
    [HideInInspector]
    public Sprite[] beards, eyes, glasses, messages, items, outsideO;


    List<Sprite[]> bodys = new List<Sprite[]>();

    private void Awake()
    {
        if (INS == null)
            INS = this;
        else
            Destroy(gameObject);
        List<Sprite> list = new List<Sprite>();
        list.AddRange(Resources.LoadAll<Sprite>("RoomBack"));
        list.AddRange(Resources.LoadAll<Sprite>("RoomBack2"));
        roomBacks = list.ToArray();
        items = Resources.LoadAll<Sprite>("Items");
        beds = Resources.LoadAll<Sprite>("Beds");
        floors = Resources.LoadAll<Sprite>("Floors");
        objs = Resources.LoadAll<Sprite>("NormalO");
        upObjs = Resources.LoadAll<Sprite>("FloorO");
        downObjs = Resources.LoadAll<Sprite>("RoofO");
        mouths = Resources.LoadAll<Sprite>("Body/Mouths");
        outsideO = Resources.LoadAll<Sprite>("Outside");

        regularOutfit = Resources.LoadAll<Sprite>("Outfits/Many");
        pants = Resources.LoadAll<Sprite>("Outfits/Pants");
        hair = Resources.LoadAll<Sprite>("Body/Hair");
        glasses = Resources.LoadAll<Sprite>("Body/Glasses");
        messages = Resources.LoadAll<Sprite>("Messages");
        beards = Resources.LoadAll<Sprite>("Body/Beards");
        eyes = Resources.LoadAll<Sprite>("Body/eyes");
        outfits.Add(Resources.LoadAll<Sprite>("Outfits/Staff/Uniform"));
        outfits.Add(Resources.LoadAll<Sprite>("Outfits/Staff/Janitor"));
        outfits.Add(Resources.LoadAll<Sprite>("Outfits/Staff/GymOutfit"));
        outfits.Add(Resources.LoadAll<Sprite>("Outfits/Staff/Cheff"));
        outfits.Add(Resources.LoadAll<Sprite>("Outfits/Staff/Plumber"));
        outfits.Add(Resources.LoadAll<Sprite>("Outfits/Staff/Electrician"));
        outfits.Add(Resources.LoadAll<Sprite>("Outfits/Staff/LifeGuard"));
        outfits.Add(Resources.LoadAll<Sprite>("Outfits/Staff/Waiter"));
        outfits.Add(Resources.LoadAll<Sprite>("Outfits/Staff/Officinist"));
        outfits.Add(Resources.LoadAll<Sprite>("Outfits/Staff/KeyBuilder"));
        outfits.Add(Resources.LoadAll<Sprite>("Outfits/1"));
        outfits.Add(Resources.LoadAll<Sprite>("Outfits/Joe"));
        outfits.Add(Resources.LoadAll<Sprite>("Outfits/2"));
        outfits.Add(Resources.LoadAll<Sprite>("Outfits/3"));
        outfits.Add(Resources.LoadAll<Sprite>("Outfits/5"));
        outfits.Add(Resources.LoadAll<Sprite>("Outfits/Suit"));
        outfits.Add(Resources.LoadAll<Sprite>("Outfits/Shirt"));
        outfits.Add(Resources.LoadAll<Sprite>("Outfits/Skirt"));
        outfits.Add(Resources.LoadAll<Sprite>("Outfits/Minishorts"));
        outfits.Add(Resources.LoadAll<Sprite>("Outfits/Police"));
        outfits.Add(Resources.LoadAll<Sprite>("Outfits/W1"));
        outfits.Add(Resources.LoadAll<Sprite>("Outfits/Clown"));
        outfits.Add(Resources.LoadAll<Sprite>("Outfits/WoodCutter"));
        outfits.Add(Resources.LoadAll<Sprite>("Outfits/Swim"));
        outfits.Add(Resources.LoadAll<Sprite>("Outfits/WhiteSuit"));
        outfits.Add(Resources.LoadAll<Sprite>("Outfits/PinkDress"));
        outfits.Add(Resources.LoadAll<Sprite>("Outfits/Penguin"));
        outfits.Add(Resources.LoadAll<Sprite>("Outfits/HotDog"));
        outfits.Add(Resources.LoadAll<Sprite>("Outfits/UA"));
        outfits.Add(Resources.LoadAll<Sprite>("Outfits/PeakyBlinder"));
        bodys.Add(Resources.LoadAll<Sprite>("Body/Body"));
        bodys.Add(Resources.LoadAll<Sprite>("Body/Body1"));
        bodys.Add(Resources.LoadAll<Sprite>("Body/Body2"));
        bodys.Add(Resources.LoadAll<Sprite>("Body/Body3"));
        bodys.Add(Resources.LoadAll<Sprite>("Body/Body4"));
        bodys.Add(Resources.LoadAll<Sprite>("Body/Body5"));
        bodys.Add(Resources.LoadAll<Sprite>("Body/Body6"));
        bodys.Add(Resources.LoadAll<Sprite>("Body/Body7"));
    }

    public Sprite GetRoomObject(int type, int id)
    {

        switch (type)
        {
            case 0:
                if(roomBacks.Length>id)
                    return roomBacks[id];
                else
                    return roomBacks[roomBacks.Length-1];
            case 1:
                if (beds.Length > id)
                    return beds[id];
                else
                    return beds[beds.Length - 1];
            case 2:
                if (floors.Length > id)
                    return floors[id];
                else
                    return floors[floors.Length - 1];
            case 3:
                if (objs.Length > id)
                    return objs[id];
                else
                    return objs[objs.Length - 1];
            case 4:
                if (upObjs.Length > id)
                    return upObjs[id];
                else
                    return upObjs[upObjs.Length - 1];
            case 5:
                if (downObjs.Length > id)
                    return downObjs[id];
                else
                    return downObjs[downObjs.Length - 1];
            default:
                if (roomBacks.Length > id)
                    return roomBacks[id];
                else
                    return roomBacks[roomBacks.Length - 1];

        }

    }
   
    public Sprite[] GetRandomOutfit()
    {
        return outfits[Random.Range(0, outfits.Count)];
    }
    public Sprite[] GetOutfit(int x)
    {
        return outfits[x];
    }
    public Sprite[] RegularOutfit()
    {
        return regularOutfit;
    }
    
    public Sprite[] Bodys(int x)
    {
        return bodys[x];
    }
    public Sprite[] Hairs()
    {
        return hair;
    }
}

/*Concatenar sprites en uno par el id
    * List<int> list = new List<int>();
       list.AddRange(x);
       list.AddRange(y);
       int[] z = list.ToArray();
    * 
 */

