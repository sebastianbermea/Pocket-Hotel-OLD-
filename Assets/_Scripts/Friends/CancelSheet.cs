using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CancelSheet : MonoBehaviour
{
    string id;
    UserB btn;
    public void Set(string id, UserB btn)
    {
        this.id = id;
        this.btn = btn;
    }
    public void Cancel(bool ac)
    {
        if (ac)
            FRC.INS.CancelRequest(id, btn);
        Destroy(transform.gameObject);
    }
}
