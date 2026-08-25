using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tip : MonoBehaviour
{
    public int level;
    // Start is called before the first frame update
    void Start()
    {
        gameObject.SetActive((Random.Range(3,level))>=GC.INS.level);
    }

   
}
