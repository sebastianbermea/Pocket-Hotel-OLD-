using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SC : MonoBehaviour
{
    public static SC INS { get; private set; }
    public AudioSource hotelAd, gameAd, musicAud;
    public AudioClip roomPop, cosSleep, cosAngry, cosHappy, cosThrow, cosHit, coinClip, doorClip, waterClip, powerClip, dust, dustC, phone, keyLost, keyf, phoneA, phoneH;
    public AudioClip tap, error, purc, transition, trashC, puff, itemF, levelUp, pop2, prestige;
    bool doorSound;
    [HideInInspector]
    public bool music, sound;

    private void Awake()
    {
        if (INS == null)
        {
            INS = this;
        }
    }
    private void Start()
    {
        music = (PlayerPrefs.GetInt("Music", 1) == 1);
        ChangeMusic(music);
        sound = (PlayerPrefs.GetInt("Sound", 1) == 1);
        ChangeSound(sound);
    }
    public void ChangeMusic(bool on)
    {
        if (on)
        {
            musicAud.Play();
        }
        else
        {
            musicAud.Stop();
        }
    }
    public void ChangeSound(bool on)
    {
        if (on)
        {
            hotelAd.volume = 0.8f;
            gameAd.volume = 0.6f;
        }
        else
        {
            hotelAd.volume = 0f;
            gameAd.volume = 0;
        }
    }
    public void PlaySound(int type, int id, float p)
    {
        switch (type)
        {
            case 0:
                switch (id)
                {
                    case 0:
                        hotelAd.PlayOneShot(tap);
                        break;
                    case 1:
                        if (!GC.INS.whileYouAway.activeInHierarchy)
                            hotelAd.PlayOneShot(waterClip);
                        break;
                    case 2:
                        if (!GC.INS.whileYouAway.activeInHierarchy)
                            hotelAd.PlayOneShot(powerClip);
                        break;
                    case 3:
                        if (!GC.INS.whileYouAway.activeInHierarchy)
                            hotelAd.PlayOneShot(dust);
                        break;
                    case 4:
                        hotelAd.PlayOneShot(dustC);
                        break;
                    case 5:
                        if (!GC.INS.whileYouAway.activeInHierarchy)
                            hotelAd.PlayOneShot(phone);
                        break;
                    case 6:
                        if (!GC.INS.whileYouAway.activeInHierarchy)
                            hotelAd.PlayOneShot(keyLost);
                        break;
                    case 7:
                        hotelAd.PlayOneShot(keyf);
                        break;
                    case 8:
                        if (!GC.INS.whileYouAway.activeInHierarchy)
                            hotelAd.PlayOneShot(phoneA);
                        break;
                    case 9:
                        if (!GC.INS.whileYouAway.activeInHierarchy)
                            hotelAd.PlayOneShot(phoneH);
                        break;
                    case 10:
                        hotelAd.PlayOneShot(error);
                        break;
                    case 11:
                        hotelAd.PlayOneShot(roomPop);
                        break;
                    case 12:
                        hotelAd.PlayOneShot(purc);
                        break;
                    case 13:
                        hotelAd.PlayOneShot(transition);
                        break;
                    case 14:
                        hotelAd.PlayOneShot(trashC);
                        break;
                    case 15:
                        hotelAd.PlayOneShot(itemF);
                        break;
                    case 16:
                        hotelAd.PlayOneShot(levelUp);
                        break;
                    case 17:
                        hotelAd.PlayOneShot(pop2);
                        break;
                    case 18:
                        hotelAd.PlayOneShot(prestige);
                        break;
                }
                break;
            case 1:
                gameAd.pitch = (p!=0)? p: 1;
                if (GC.INS.loadingPanel.activeInHierarchy)
                    return;
                switch (id)
                {
                    case 0:
                        gameAd.PlayOneShot(cosSleep);
                        break;
                    case 1:
                        gameAd.PlayOneShot(cosAngry);
                        break;
                    case 2:
                        gameAd.PlayOneShot(cosHappy);
                        break;
                    case 3:
                        gameAd.PlayOneShot(cosThrow);
                        break;
                    case 4:
                        gameAd.PlayOneShot(cosHit);
                        break;
                    case 5:
                        gameAd.PlayOneShot(coinClip);
                        break;
                    case 6:
                        if (doorSound)
                            return;
                        doorSound = true;
                        gameAd.PlayOneShot(doorClip);
                        Invoke("DoorSound", .6f);
                        break;
                    case 7:
                        gameAd.PlayOneShot(puff);
                        break;
                }
                break;
               
        }
       
    }
    void DoorSound()
    {
        doorSound = false;
    }
    public void StopSound()
    {
        hotelAd.Stop();
    }
}
