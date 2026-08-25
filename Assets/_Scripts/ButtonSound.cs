using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonSound : MonoBehaviour
{
    Button btn;
    // Start is called before the first frame update
    void Start()
    {
        btn = GetComponent<Button>();
        if(btn)
            btn.onClick.AddListener(Sound);
       
    }

    void Sound()
    {
        if(SC.INS)
            SC.INS.PlaySound(0, 0, 0);
    }
    private void OnDestroy()
    {
        if (btn)
            btn.onClick.RemoveListener(Sound);
    }
}
