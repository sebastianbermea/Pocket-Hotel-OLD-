using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Scene2 : MonoBehaviour
{
    public Transform obj;
    public Text txt;
    private void Start()
    {
        txt.text = FRC.INS.visitId;
    }
    private void Update()
    {
        obj.Translate(Vector2.right * .5f * Time.deltaTime);
        if (Input.GetKeyDown(KeyCode.R))
        {
            Debug.Log("R");
        }
    }
    public void Bye()
    {
        FRC.INS.Bye();
        SceneManager.UnloadSceneAsync("Scene2");
    }
}
