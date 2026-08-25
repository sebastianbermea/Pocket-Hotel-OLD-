using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checker : MonoBehaviour
{
    // Start is called before the first frame update
    Room room;

    void Awake()
    {
        room = transform.parent.GetComponent<Room>();
    }
   
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Room")
        {
            if (!room.placed)
            {
                room.Occupied(true);
            }
            else if(collision.gameObject!=room.gameObject)
            {
                room.OverLap();
                
            }
        }
        else if (collision.gameObject.tag == "Permited")
        {
            room.Occupied(true);
        }
    }
   
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Room")
            room.Occupied(false);
        else if (collision.gameObject.tag == "Permited")
        {
            room.Occupied(false);
        }
    }

}
