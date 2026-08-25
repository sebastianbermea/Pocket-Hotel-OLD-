using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResponseSheet : MonoBehaviour
{
    public void Response(bool ac)
    {
        FRC.INS.RespondRequest(ac);
        Destroy(transform.gameObject);
    }
}
