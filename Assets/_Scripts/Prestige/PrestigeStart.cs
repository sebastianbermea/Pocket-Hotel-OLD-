using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PrestigeStart : MonoBehaviour
{
    public Image medal;

    // Start is called before the first frame update
    void Start()
    {
        medal.sprite = GC.INS.p.medals[GC.INS.prestige-1];
    }
    public void Click()
    {
        Destroy(gameObject);
    }
    
}
