using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlotController : MonoBehaviour
{
    public List<Transform> slots;
    public List<SlotType> slotstypes;
    public List<bool> right;
    public List<float> after;
    
    public void Create(int xp, int coins, int time, Room room)
    {
        if (!GC.INS.visit)
        {
            for (int i = 0; i < slots.Count; i++)
            {
                room.slotsIds.Add(
                    GC.INS.AddSlot(new Slot(
                     0,
                     slots[i].localPosition,
                     transform,
                     slotstypes[i],
                     coins,
                     xp,
                     room.number,
                     time,
                     right[i],
                     ((after.Count>i) ? after[i]:slots[i].localPosition.z)
                     )));
            }
        }
        else
        {
            for (int i = 0; i < slots.Count; i++)
            {
                room.slotsIds.Add(
                    VC.INS.AddSlot(new Slot(
                     0,
                     slots[i].localPosition,
                     transform,
                     slotstypes[i],
                     coins,
                     xp,
                     room.number,
                     time,
                     right[i],
                     ((after.Count > i) ? after[i] : slots[i].localPosition.z)
                     )));
            }
        }
        
    }
}
public class Slot
{
    Vector3 _pos;
    Transform _spawnPos;
    int _id, _coins, _xp, _roomId, _time;
    bool _right;
    SlotType _type;
    float _zPosAfter;
    public Slot(int id, Vector3 pos, Transform spawnPos, SlotType type, int coins, int xp, int roomId, int time, bool right, float zPosAfter)
    {
        _id = id;
        _pos = pos;
        _type = type;
        _spawnPos = spawnPos;
        _coins = coins;
        _xp = xp;
        _roomId = roomId;
        _time = time;
        _right = right;
        _zPosAfter = zPosAfter;
    }
    public Vector3 pos
    {
        get { return _pos; }
    }
    public SlotType type
    {
        get { return _type; }
    }
    public Transform spawnPos
    {
        get { return _spawnPos; }
    }
    public int xp
    {
        get { return _xp; }
    }
    public float zPos
    {
        get { return _zPosAfter; }
    }
    public int coins
    {
        get { return _coins; }
    }
    public int roomId
    {
        get { return _roomId; }
    }
    public int time
    {
        get { return _time; }
    }
    public bool right
    {
        get { return _right; }
    }
    public int id
    {
        get { return _id; }
        set { _id = value; }
    }
}
public enum SlotType
{
    Sleep,
    Seat,
    Run,
    Lift,
    Fix,
    Swim,
    Iddle,
    Dance,
}