using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSet : MonoBehaviour
{
    public Image[] headParts, outfit, body, eyes;
    public Image mouth;
    public Text sname;
    public void SetCharacter(Character character)
    {
        //Head
        Sprite[] hairs = SM.INS.Hairs();
        headParts[0].sprite = hairs[character.hairId];
        headParts[0].color = GC.INS.hairC[character.hairColor];
        eyes[0].sprite = SM.INS.eyes[character.eyesId * 2];
        eyes[1].sprite = SM.INS.eyes[character.eyesId * 2 + 1];
        eyes[1].color = GC.INS.eyesC[character.eyeColor];
        mouth.sprite = SM.INS.mouths[character.mouthId];
        mouth.SetNativeSize();
        //Extra
        headParts[1].sprite = SM.INS.beards[character.extraId];
        if (character.extraId < 9)
        {
            headParts[1].color = GC.INS.hairC[character.extraColor];
        }
        else
        {
            headParts[1].color = Color.white;
        }

        //Glasses
        headParts[2].sprite = SM.INS.glasses[character.glassId * 2];
        headParts[2].color = GC.INS.armazonColor[character.glassColorId];
        if (character.glassId > 0 && character.glassColor > 0)
        {
            headParts[3].sprite = SM.INS.glasses[character.glassId * 2 + 1];
            headParts[4].sprite = SM.INS.glasses[character.glassId * 2 + 1];
            headParts[3].color = GC.INS.glassColor[character.glassColor];
            headParts[4].color = GC.INS.glassColor[character.glassColor];
        }

        //SkinColor
        Sprite[] tempBody = SM.INS.Bodys(character.skinColor);
        body[0].sprite = tempBody[0];
        body[1].sprite = tempBody[1];
        body[2].sprite = tempBody[1];
        body[3].sprite = tempBody[2];
        body[4].sprite = tempBody[3];
        body[5].sprite = tempBody[4];

        sname.text = character.name;

        //Arrange Head
        headParts[0].SetNativeSize();
        Vector2 size = headParts[0].GetComponent<RectTransform>().sizeDelta;
        size *= headParts[0].GetComponent<Image>().pixelsPerUnit;
        Vector2 pixelPivot = headParts[0].GetComponent<Image>().sprite.pivot;
        Vector2 percentPivot = new Vector2(pixelPivot.x / size.x, pixelPivot.y / size.y);
        headParts[0].GetComponent<RectTransform>().pivot = percentPivot;
        //Arrange extra
        headParts[1].SetNativeSize();
        size = headParts[1].GetComponent<RectTransform>().sizeDelta;
        size *= headParts[1].GetComponent<Image>().pixelsPerUnit;
        pixelPivot = headParts[1].GetComponent<Image>().sprite.pivot;
        percentPivot = new Vector2(pixelPivot.x / size.x, pixelPivot.y / size.y);
        headParts[1].GetComponent<RectTransform>().pivot = percentPivot;

        if (character.glassId > 0)
        {
            //Arrange glasses
            headParts[2].SetNativeSize();
            size = headParts[2].GetComponent<RectTransform>().sizeDelta;
            size *= headParts[2].GetComponent<Image>().pixelsPerUnit;
            pixelPivot = headParts[2].GetComponent<Image>().sprite.pivot;
            percentPivot = new Vector2(pixelPivot.x / size.x, pixelPivot.y / size.y);
            headParts[2].GetComponent<RectTransform>().pivot = percentPivot;
            //Arrange glass 1
            headParts[3].SetNativeSize();
            size = headParts[3].GetComponent<RectTransform>().sizeDelta;
            size *= headParts[3].GetComponent<Image>().pixelsPerUnit;
            pixelPivot = headParts[3].GetComponent<Image>().sprite.pivot;
            percentPivot = new Vector2(pixelPivot.x / size.x, pixelPivot.y / size.y);
            headParts[3].GetComponent<RectTransform>().pivot = percentPivot;
            //Arrange glass 2
            headParts[4].SetNativeSize();
            size = headParts[4].GetComponent<RectTransform>().sizeDelta;
            size *= headParts[4].GetComponent<Image>().pixelsPerUnit;
            pixelPivot = headParts[4].GetComponent<Image>().sprite.pivot;
            percentPivot = new Vector2(pixelPivot.x / size.x, pixelPivot.y / size.y);
            headParts[4].GetComponent<RectTransform>().pivot = percentPivot;
        }

        Sprite[] tempOutfit = SM.INS.GetOutfit(character.outfitId);
        for (int i = 0; i < tempOutfit.Length; i++)
            outfit[i].sprite = tempOutfit[i];

        for (int i = 0; i < outfit.Length; i++)
        {
            outfit[i].SetNativeSize();
            size = outfit[i].GetComponent<RectTransform>().sizeDelta;
            size *= outfit[i].GetComponent<Image>().pixelsPerUnit;
            pixelPivot = outfit[i].GetComponent<Image>().sprite.pivot;
            percentPivot = new Vector2(pixelPivot.x / size.x, pixelPivot.y / size.y);
            outfit[i].GetComponent<RectTransform>().pivot = percentPivot;
        }
    }
    public void SetOutfit(int x)
    {
        Sprite[] tempOutfit = SM.INS.GetOutfit(x);
        for (int i = 0; i < tempOutfit.Length; i++)
            outfit[i].sprite = tempOutfit[i];

        for (int i = 0; i < outfit.Length; i++)
        {
            outfit[i].SetNativeSize();
            Vector2 size = outfit[i].GetComponent<RectTransform>().sizeDelta;
            size *= outfit[i].GetComponent<Image>().pixelsPerUnit;
            Vector2 pixelPivot = outfit[i].GetComponent<Image>().sprite.pivot;
            Vector2 percentPivot = new Vector2(pixelPivot.x / size.x, pixelPivot.y / size.y);
            outfit[i].GetComponent<RectTransform>().pivot = percentPivot;
        }
    }
    public void SetMouth(int x)
    {
        Unactive();
        mouth.gameObject.SetActive(true);
        mouth.sprite = SM.INS.mouths[x];
        mouth.SetNativeSize();
    }
    public void SetExtra(int x)
    {
        Unactive();
        headParts[1].gameObject.SetActive(true);
        headParts[1].sprite = SM.INS.beards[x];
        headParts[1].SetNativeSize();
        Vector2 size = headParts[1].GetComponent<RectTransform>().sizeDelta;
        size *= headParts[1].GetComponent<Image>().pixelsPerUnit;
        Vector2 pixelPivot = headParts[1].GetComponent<Image>().sprite.pivot;
        Vector2 percentPivot = new Vector2(pixelPivot.x / size.x, pixelPivot.y / size.y);
        headParts[1].GetComponent<RectTransform>().pivot = percentPivot;
        if (x < 9)
        {
            headParts[1].color = GC.INS.hairC[GC.INS.player.extraColor];
        }
    }
    public void SetEyes(int x)
    {
        Unactive();
        eyes[0].gameObject.SetActive(true);
        eyes[1].gameObject.SetActive(true);
        eyes[0].sprite = SM.INS.eyes[x * 2];
        eyes[1].sprite = SM.INS.eyes[x * 2 + 1];
        eyes[1].color = GC.INS.eyesC[GC.INS.player.eyeColor];
    }
    public void SetGlasses(int x)
    {
        Unactive();
        headParts[2].gameObject.SetActive(true);
        headParts[3].gameObject.SetActive(true);
        headParts[4].gameObject.SetActive(true);

        headParts[2].sprite = SM.INS.glasses[x * 2];
        headParts[2].color = GC.INS.armazonColor[GC.INS.player.glassColorId];
        if (x > 0 && GC.INS.player.glassColor > 0)
        {
            headParts[3].sprite = SM.INS.glasses[x * 2 + 1];
            headParts[4].sprite = SM.INS.glasses[x * 2 + 1];
            headParts[3].color = GC.INS.glassColor[GC.INS.player.glassColor];
            headParts[4].color = GC.INS.glassColor[GC.INS.player.glassColor];
        }
    }
    public void SetHair(int x)
    {
        Unactive();
        headParts[0].gameObject.SetActive(true);
        Sprite[] hairs = SM.INS.Hairs();
        headParts[0].sprite = hairs[x];
        headParts[0].color = GC.INS.hairC[GC.INS.player.hairColor];

        headParts[0].SetNativeSize();
        Vector2 size = headParts[0].GetComponent<RectTransform>().sizeDelta;
        size *= headParts[0].GetComponent<Image>().pixelsPerUnit;
        Vector2 pixelPivot = headParts[0].GetComponent<Image>().sprite.pivot;
        Vector2 percentPivot = new Vector2(pixelPivot.x / size.x, pixelPivot.y / size.y);
        headParts[0].GetComponent<RectTransform>().pivot = percentPivot;
    }
    void Unactive()
    {
        eyes[0].gameObject.SetActive(false);
        eyes[1].gameObject.SetActive(false);
        headParts[0].gameObject.SetActive(false);
        headParts[1].gameObject.SetActive(false);
        mouth.gameObject.SetActive(false);
        headParts[2].gameObject.SetActive(false);
        headParts[3].gameObject.SetActive(false);
        headParts[4].gameObject.SetActive(false);
    }
}
