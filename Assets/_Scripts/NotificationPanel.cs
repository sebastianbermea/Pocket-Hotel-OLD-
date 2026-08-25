using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotificationPanel : MonoBehaviour
{
    public void Set(bool t)
    {
        GC.INS.noti = t;
        if (t)
            GC.INS.notifications.SetNoti();
  
        GC.INS.dm.CloseLaptop();
    }
}
