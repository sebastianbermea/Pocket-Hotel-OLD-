using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaffBtns : MonoBehaviour
{

    [SerializeField]
    Staff staff;
    [SerializeField]
    bool rest;

    private void OnMouseDown()
    {
        if (staff)
        {
            staff.Rest(rest);
        }
    }
}
