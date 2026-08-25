using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ErrorM : MonoBehaviour
{
    public Animator anim;
    public Text mess;
    public void Error(string message, int time)
    {
        if (transform.parent.gameObject.activeInHierarchy)
            return;
        transform.parent.gameObject.SetActive(true);
        mess.text = message;
        SC.INS.PlaySound(0, 10, 0);
        Invoke("OffAnim", time);
    }
    public void Error(int id)
    {
        if (transform.parent.gameObject.activeInHierarchy)
            return;
        transform.parent.gameObject.SetActive(true);
 
        mess.text = GC.INS.t.GetText(109 + id);
        SC.INS.PlaySound(0, 10, 0);
        Invoke("OffAnim", .8f);
    }
    void OffAnim()
    {
        anim.SetTrigger("Out");
        Invoke("Off", 0.4f);
    }
    void Off()
    {
        transform.parent.gameObject.SetActive(false);
    }
}
