using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterCustomize : MonoBehaviour
{
    public Image[] headParts, outfit, body, eyes;
    public Image mouth;
    Vector2 size, pixelPivot, percentPivot;
    private void Start()
    {
        //Head
        Sprite[] hairs = SM.INS.Hairs();
        headParts[0].sprite = hairs[GC.INS.player.hairId];
        headParts[0].color = GC.INS.hairC[GC.INS.player.hairColor];
        eyes[0].sprite = SM.INS.eyes[GC.INS.player.eyesId * 2];
        eyes[1].sprite = SM.INS.eyes[GC.INS.player.eyesId * 2 + 1];
        eyes[1].color = GC.INS.eyesC[GC.INS.player.eyeColor];
        mouth.sprite = SM.INS.mouths[GC.INS.player.mouthId];
        mouth.SetNativeSize();

        //Extra
        headParts[1].sprite = SM.INS.beards[GC.INS.player.extraId];
        if (GC.INS.player.extraId < 9)
        {
            headParts[1].color = GC.INS.hairC[GC.INS.player.extraColor];
        }
        else
        {
            headParts[1].color = Color.white;
        }

        //Glasses
        headParts[2].sprite = SM.INS.glasses[GC.INS.player.glassId * 2];
        headParts[2].color = GC.INS.armazonColor[GC.INS.player.glassColorId];
        if (GC.INS.player.glassId > 0 && GC.INS.player.glassColor > 0)
        {
            headParts[3].sprite = SM.INS.glasses[GC.INS.player.glassId * 2 + 1];
            headParts[4].sprite = SM.INS.glasses[GC.INS.player.glassId * 2 + 1];
            headParts[3].color = GC.INS.glassColor[GC.INS.player.glassColor];
            headParts[4].color = GC.INS.glassColor[GC.INS.player.glassColor];
        }

        //SkinColor
        Sprite[] tempBody = SM.INS.Bodys(GC.INS.player.skinColor);
        body[0].sprite = tempBody[0];
        body[1].sprite = tempBody[1];
        body[2].sprite = tempBody[1];
        body[3].sprite = tempBody[2];
        body[4].sprite = tempBody[3];
        body[5].sprite = tempBody[4];

        
        //Arrange Head
        headParts[0].SetNativeSize();
        size = headParts[0].GetComponent<RectTransform>().sizeDelta;
        size *= headParts[0].GetComponent<Image>().pixelsPerUnit;
        pixelPivot = headParts[0].GetComponent<Image>().sprite.pivot;
        percentPivot = new Vector2(pixelPivot.x / size.x, pixelPivot.y / size.y);
        headParts[0].GetComponent<RectTransform>().pivot = percentPivot;
        //Arrange extra
        headParts[1].SetNativeSize();
        size = headParts[1].GetComponent<RectTransform>().sizeDelta;
        size *= headParts[1].GetComponent<Image>().pixelsPerUnit;
        pixelPivot = headParts[1].GetComponent<Image>().sprite.pivot;
        percentPivot = new Vector2(pixelPivot.x / size.x, pixelPivot.y / size.y);
        headParts[1].GetComponent<RectTransform>().pivot = percentPivot;


        
       //Oufit
        Sprite[] tempOutfit = SM.INS.GetOutfit(GC.INS.player.outfitId);
        for (int i = 0; i < tempOutfit.Length; i++)
        {
            outfit[i].sprite = tempOutfit[i];
            outfit[i].SetNativeSize();
            size = outfit[i].GetComponent<RectTransform>().sizeDelta;
            size *= outfit[i].GetComponent<Image>().pixelsPerUnit;
            pixelPivot = outfit[i].GetComponent<Image>().sprite.pivot;
            percentPivot = new Vector2(pixelPivot.x / size.x, pixelPivot.y / size.y);
            outfit[i].GetComponent<RectTransform>().pivot = percentPivot;
        }
        
    }

    public void Customize(int type,int id)
    {

        switch (type)
        {
            case 0:
                //SkinColor
                GC.INS.player.skinColor = id;
                Sprite[] tempBody = SM.INS.Bodys(id);
                body[0].sprite = tempBody[0];
                body[1].sprite = tempBody[1];
                body[2].sprite = tempBody[1];
                body[3].sprite = tempBody[2];
                body[4].sprite = tempBody[3];
                body[5].sprite = tempBody[4];
                
                break;
            case 1:
                //Oufit
                GC.INS.player.outfitId = id;
                Sprite[] tempOutfit = SM.INS.GetOutfit(id);
                for (int i = 0; i < tempOutfit.Length; i++)
                {
                    outfit[i].sprite = tempOutfit[i];
                    outfit[i].SetNativeSize();
                    size = outfit[i].GetComponent<RectTransform>().sizeDelta;
                    size *= outfit[i].GetComponent<Image>().pixelsPerUnit;
                    pixelPivot = outfit[i].GetComponent<Image>().sprite.pivot;
                    percentPivot = new Vector2(pixelPivot.x / size.x, pixelPivot.y / size.y);
                    outfit[i].GetComponent<RectTransform>().pivot = percentPivot;
                }

                Debug.Log(GC.INS.player.outfitId);
                break;
            case 2:
                GC.INS.player.mouthId = id;
                mouth.sprite = SM.INS.mouths[id];
                mouth.SetNativeSize();
                break;
            case 3:
                GC.INS.player.extraId = id;
                headParts[1].sprite = SM.INS.beards[GC.INS.player.extraId];
                if (GC.INS.player.extraId < 9)
                {
                    headParts[1].color = GC.INS.hairC[GC.INS.player.extraColor];
                }
                else
                    headParts[1].color = Color.white;
                //Arrange extra
                headParts[1].SetNativeSize();
                size = headParts[1].GetComponent<RectTransform>().sizeDelta;
                size *= headParts[1].GetComponent<Image>().pixelsPerUnit;
                pixelPivot = headParts[1].GetComponent<Image>().sprite.pivot;
                percentPivot = new Vector2(pixelPivot.x / size.x, pixelPivot.y / size.y);
                headParts[1].GetComponent<RectTransform>().pivot = percentPivot;
                break;
            case 4:
                GC.INS.player.extraColor = id;
                if (GC.INS.player.extraId < 9)
                    headParts[1].color = GC.INS.hairC[GC.INS.player.extraColor];
                else
                    headParts[1].color = Color.white;
                break;
            case 5:
                GC.INS.player.eyesId = id;
                eyes[0].sprite = SM.INS.eyes[GC.INS.player.eyesId * 2];
                eyes[1].sprite = SM.INS.eyes[GC.INS.player.eyesId * 2 + 1];
                break;
            case 6:
                GC.INS.player.eyeColor = id;
                eyes[1].color = GC.INS.eyesC[GC.INS.player.eyeColor];
                break;
            case 7:
                GC.INS.player.glassId = id;
                headParts[2].sprite = SM.INS.glasses[GC.INS.player.glassId * 2];
                headParts[3].sprite = SM.INS.glasses[GC.INS.player.glassId * 2 + 1];
                headParts[4].sprite = SM.INS.glasses[GC.INS.player.glassId * 2 + 1];
                headParts[3].color = GC.INS.glassColor[GC.INS.player.glassColor];
                headParts[4].color = GC.INS.glassColor[GC.INS.player.glassColor];
                break;
            case 8:
                GC.INS.player.glassColorId = id;
                headParts[2].color = GC.INS.armazonColor[GC.INS.player.glassColorId];
                break;
            case 9:
                GC.INS.player.glassColor = id;
                headParts[3].color = GC.INS.glassColor[GC.INS.player.glassColor];
                headParts[4].color = GC.INS.glassColor[GC.INS.player.glassColor]; 
                break;
            case 10:
                GC.INS.player.hairId = id;
                Sprite[] hairs = SM.INS.Hairs();
                headParts[0].sprite = hairs[GC.INS.player.hairId];

                headParts[0].SetNativeSize();
                size = headParts[0].GetComponent<RectTransform>().sizeDelta;
                size *= headParts[0].GetComponent<Image>().pixelsPerUnit;
                pixelPivot = headParts[0].GetComponent<Image>().sprite.pivot;
                percentPivot = new Vector2(pixelPivot.x / size.x, pixelPivot.y / size.y);
                headParts[0].GetComponent<RectTransform>().pivot = percentPivot;
                break;
            case 11:
                GC.INS.player.hairColor = id;
                headParts[0].color = GC.INS.hairC[GC.INS.player.hairColor];
                break;
        }
        GC.INS.playerCharacter.SetCharacter(GC.INS.player);
    }
    
    public static int[,] costs =
    {
        //Skin
        {
            2500,
            5000,
            5000,
            10000,15000,15000,-250,-250,2000,3000,3000,3500,3500,5000,5000,5000,
            2000,2500,2500,2500,2500,3000,3000,4000,4000,4000,4000,5000,50000,50000,
        },
        //Outfit
         {
            500,2500,2500,4000,4000,
            5000,5000,7500,-25,8000,
            8000,9000,9500,10000,-50,
            15000,-75,10000,12500,-100,
            12500,-150,15000,-150,-200,
            16500,-225,-250,-300,-300,
        },
         //Mouth
         {
            1000,
            2500,
            4000,
            4000,10000,20000,-250,-250,2500,2500,3000,3000,3000,3000,3000,3000,
            2000,2500,2500,2500,2500,3000,3000,4000,4000,4000,4000,5000,50000,50000,
        },
         //Extra
         {
            2000,
            3000,
            3000,
            5000,-100,-100,10000,10000,25000,3000,4000,4000,10000,-100,-125,-125,
            -150,-150,2500,2500,2500,3000,3000,4000,4000,4000,4000,5000,50000,50000,
        },
         //Extra color
         {
            2000,
            2500,
            2500,
            5000,5000,10000,10000,10000,15000,15000,20000,-200,-200,-500,-500,5000,
            2000,2500,2500,2500,2500,3000,3000,4000,4000,4000,4000,5000,50000,50000,
        },
         //Eyes
         {
            2000,
            5000,
            8000,
            8000,12500,15000,-250,-250,3000,3000,4000,4000,4000,4000,5000,50000,
            2000,2500,2500,2500,2500,3000,3000,4000,4000,4000,4000,5000,50000,50000,
        },
          //Eyes Color
         {
            2000,
            3000,
            3000,
            5000,10000,10000,12500,12500,-200,-200,-300,-300,4000,4000,5000,50000,
            2000,2500,2500,2500,2500,3000,3000,4000,4000,4000,4000,5000,50000,50000,
        },
          //Glasses
         {
            2000,
            10000,
            15000,
            20000,2500,2500,2500,2500,3000,3000,4000,4000,4000,4000,5000,50000,
            2000,2500,2500,2500,2500,3000,3000,4000,4000,4000,4000,5000,50000,50000,
        },
          //GlassesColor
         {
            2000,
            2000,
            5000,
            5000,10000,10000,10000,10000,3000,3000,4000,4000,4000,4000,5000,50000,
            2000,2500,2500,2500,2500,3000,3000,4000,4000,4000,4000,5000,50000,50000,
        },
          //GlassColor
         {
            2000,
            25000,
            25000,
            45000,45000,-100,-100,-250,-250,-250,4000,4000,4000,4000,5000,50000,
            2000,2500,2500,2500,2500,3000,3000,4000,4000,4000,4000,5000,50000,50000,
        },
          //Hair
         {
            2000,
            5000,
            5000,
            7500,7500,7500,10000,15000,20000,20000,-100,-100,-250,-300,50000,50000,
            -350,5000,7500,12500,15000,15000,20000,20000,-100,-100,-250,-250,50000,50000,

        },
          //Hair color
         {
            2000,
            2500,
            5000,
            5000,10000,10000,15000,15000,20000,30000,40000,-250,-250,-500,-500,5000,
            2000,2500,2500,2500,2500,3000,3000,4000,4000,4000,4000,5000,50000,50000,
        },
         
    };
}
