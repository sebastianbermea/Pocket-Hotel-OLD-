using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InOutAnim : MonoBehaviour
{
    public RectTransform panel;
    public Vector3 endPos= new Vector3(35,13,0);
    Vector3 startPos;
    float timeOfTravel = .25f; //time after object reach a target place 
    float currentTime = 0; // actual floting time 
    float normalizedValue;
    private void Awake()
    {
        startPos = panel.anchoredPosition;
    }
    void OnEnable()
    {
        currentTime = 0;
        StartCoroutine(In());
    }

    void OnDisable()
    {
        StopCoroutine(Out());
        panel.anchoredPosition = startPos;
    }
    public void OutAnim()
    {
        if (!gameObject.activeInHierarchy)
            return;
        StopCoroutine(In());
        currentTime = 0;
        StartCoroutine(Out());
    }

    IEnumerator In()
    {

        while (currentTime <= timeOfTravel)
        {
            currentTime += Time.deltaTime;
            normalizedValue = currentTime / timeOfTravel; // we normalize our time 

            panel.anchoredPosition = Vector3.Lerp(startPos, endPos, normalizedValue);
            yield return null;
        }

    }
    IEnumerator Out()
    {

        while (currentTime <= timeOfTravel)
        {
            currentTime += Time.deltaTime;
            normalizedValue = currentTime / timeOfTravel; // we normalize our time 

            panel.anchoredPosition = Vector3.Lerp(endPos, startPos, normalizedValue);
            yield return null;
        }

    }
}
