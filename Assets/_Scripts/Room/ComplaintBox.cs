using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComplaintBox : MonoBehaviour
{
    public Complain com;
    bool delayed;
    private void OnMouseUp()
    {
        if (delayed)
            return;

        delayed = true;
        if(com.Click())
            SC.INS.PlaySound(0, 10, 0);
        else
            SC.INS.PlaySound(0, 7, 0);

        com.playerWork = true;
        Invoke("Delayed", .6f);
    }

    void Delayed()
    {
        delayed = false;
    }
}
