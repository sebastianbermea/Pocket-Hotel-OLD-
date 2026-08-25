using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class Settings : MonoBehaviour
{
    //Sound and music switches
    public RectTransform handleTM, handleTS;
    public Color backActColor, handleActColor;
    Color backDColor, handleDColor;
    public Image backIM, handleIM;
    public Image backIS, handleIS;
    public Toggle toggleM, toggleS;
    Vector2 handlePosM, handlePosS;
    public GameObject[] plus;
    public GameObject redemCode, idiomPanel, moregamesP, credits;
    public TMP_InputField inpf;
    public Image[] idiomBtns;
    public VideoPlayer[] moreGamesV;
    public GameObject canvas, buyingCanvas, phoneCanvas;

    private void Start()
    {
        handlePosM = handleTM.anchoredPosition;
        backDColor = backIM.color;
        handleDColor = handleIM.color;
        if (SC.INS.music)
        {
            handleTM.anchoredPosition = handlePosM * -1;
            handleIM.color = handleActColor;
            backIM.color = backActColor;
            toggleM.isOn = true;
        }
        handlePosS = handleTS.anchoredPosition;
        if (SC.INS.sound)
        {
            handleTS.anchoredPosition = handlePosS * -1;
            handleIS.color = handleActColor;
            backIS.color = backActColor;
            toggleS.isOn = true;
        }
        toggleM.onValueChanged.AddListener(ChangeMusic);
        toggleS.onValueChanged.AddListener(ChangeSound);
        for(int i=0; i < GC.INS.plusVisit.Length; i++)
        {
            plus[i].SetActive(!GC.INS.plusVisit[i]);
        }

        idiomBtns[GC.INS.idiom].color = new Color(.7f,.7f,.7f);
    }
    public void CanvasOff()
    {
        canvas.SetActive(false);
        buyingCanvas.SetActive(false);
        phoneCanvas.SetActive(false);
    }
    void ChangeMusic(bool on)
    {
        Debug.Log(on);
        if (on)
        {
            handleTM.anchoredPosition = handlePosM * -1;
            handleIM.color = handleActColor;
            backIM.color = backActColor;
            PlayerPrefs.SetInt("Music", 1);
        }
        else
        {
            handleTM.anchoredPosition = handlePosM;
            handleIM.color = handleDColor;
            backIM.color = backDColor;
            PlayerPrefs.SetInt("Music", 0);
        }
        SC.INS.ChangeMusic(on);
        SC.INS.PlaySound(0,0,0);
    }
    void ChangeSound(bool on)
    {
        if (on)
        {
            handleTS.anchoredPosition = handlePosS * -1;
            handleIS.color = handleActColor;
            backIS.color = backActColor;
            PlayerPrefs.SetInt("Sound", 1);
            SC.INS.PlaySound(0, 0, 0);
        }
        else
        {
            handleTS.anchoredPosition = handlePosS;
            handleIS.color = handleDColor;
            backIS.color = backDColor;
            PlayerPrefs.SetInt("Sound", 0);
        }
        SC.INS.ChangeSound(on);
        
    }
    public void OpenUrl(int x)
    {
        string link;
        switch (x)
        {
            default:
                link = "https://play.google.com/store/apps/developer?id=Chino+Creator";
                break;
            case 0:
                link = "https://play.google.com/store/apps/developer?id=Chino+Creator";
                break;
            case 1:
                if (!GC.INS.plusVisit[0])
                {
                    GC.INS.plusVisit[0] = true;
                    plus[0].SetActive(false);
                    SC.INS.PlaySound(0, 15, 0);
                    GC.INS.gems += 10;
                    GC.INS.gemsText.text = GC.INS.gems.ToString("n0");
                }
                link = "https://www.facebook.com/ChinoCreate/";
                break;
            case 2:
                if (!GC.INS.plusVisit[1])
                {
                    GC.INS.plusVisit[1] = true;
                    plus[1].SetActive(false);
                    SC.INS.PlaySound(0, 15, 0);
                    GC.INS.gems += 10;
                    GC.INS.gemsText.text = GC.INS.gems.ToString("n0");
                }
                link = "https://twitter.com/Chino_Creator";
                break;
            case 3:
                if (!GC.INS.plusVisit[2])
                {
                    GC.INS.plusVisit[2] = true;
                    plus[2].SetActive(false);
                    GC.INS.gems += 10;
                    SC.INS.PlaySound(0, 15, 0);
                    GC.INS.gemsText.text = GC.INS.gems.ToString("n0");
                }
                link = "https://www.youtube.com/channel/UCLGIblCBxEwFxdWL94eRSvA";
                break;
            case 4:
                link = "mailto: sebasbermear @hotmail.com? subject = Email & body = from Unity";
                break;
            case 5:
                link = "https://tequilappgames.wordpress.com/";
                break;
        }
        Application.OpenURL(link);
    }
    public void OpenRedem()
    {
        redemCode.SetActive(true);
        SC.INS.PlaySound(0, 17, 0);
    }
    public void Redem()
    {
        switch (inpf.text)
        {
            default:
                GC.INS.errorM.Error(14);
                break;
            case "hola":
                if (GC.INS.codes[0])
                {
                    GC.INS.errorM.Error(15);
                    return;
                }
                GC.INS.codes[0] = true;
                GC.INS.gift.AddGift(new Dictionary<string, object>
                            {
                                { "id",0},
                                { "type",0},
                                { "subtype", 0},
                });
                break;
            case "ResetPlayerP":
                PlayerPrefs.DeleteAll();
                break;
            case "ValiHelp":
                GC.INS.AddGems(150);
                GC.INS.codes[1] = true;
                break;
        }
        redemCode.SetActive(false);
    }
    private void OnDestroy()
    {
        toggleM.onValueChanged.RemoveListener(ChangeMusic);
        toggleS.onValueChanged.RemoveListener(ChangeSound);
    }
    public void OpenIdiom()
    {
        idiomPanel.SetActive(true);
        SC.INS.PlaySound(0, 17, 0);
    }
    public void ChangeIdiom(int x)
    {
        if (GC.INS.idiom == x)
            return;
        PlayerPrefs.SetInt("Idiom", x);
        GC.INS.SaveFromBtn();
        GC.INS.ReloadScene();
    }
    public void OpenMoreGames()
    {
        moregamesP.SetActive(true);
        SC.INS.PlaySound(0, 17, 0);
        for (int i = 0; i < moreGamesV.Length; i++)
        {
            moreGamesV[i].enabled = true;
            moreGamesV[i].Play();
        }
    }
    public void CloseMoreGames()
    {
        moregamesP.SetActive(false);
        for (int i = 0; i < moreGamesV.Length; i++)
        {
            moreGamesV[i].enabled = false;
            moreGamesV[i].Stop();
        }
    }
    public void OpenGame(int x)
    {
        string link;
        switch (x)
        {
            default:
                link = "https://play.google.com/store/apps/details?id=com.ChinoCreator.TotemDefense";
                break;
            case 0:
                link = "https://play.google.com/store/apps/details?id=com.ChinoCreator.TotemDefense";
                break;
            case 1:
                link = "https://play.google.com/store/apps/details?id=com.ChinoCreator.Reload";
                break;
            case 2:
                link = "https://play.google.com/store/apps/details?id=com.ChinoCreator.Planets";
                break;
        }
        Application.OpenURL(link);
    }
    public void OpenCredits()
    {
        SC.INS.PlaySound(0, 17, 0);
        credits.SetActive(true);
    }
}
