using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyFind : MonoBehaviour
{
    public Keyloss par;
    private void OnMouseUpAsButton()
    {
        SC.INS.PlaySound(0, 7, 0);
        par.playerFind = true;
        par.Found();
    }
}
