using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Database;
using System;
#if UNITY_IOS
using Unity.Notifications.iOS;
#endif
#if UNITY_ANDROID
using Unity.Notifications.Android;
#endif
public class RDB : MonoBehaviour
{

    DatabaseReference referenece;
    // Start is called before the first frame update
    void Start()
    {
        try
        {
            referenece = FirebaseDatabase.DefaultInstance.RootReference;
        }
        catch (Exception e)
        {
            Debug.LogError(e);
        }

    }
    public void CancelNotification()
    {
#if UNITY_ANDROID
        AndroidNotificationCenter.CancelAllNotifications();
#endif
#if UNITY_IOS
        iOSNotificationCenter.RemoveAllScheduledNotifications();
        iOSNotificationCenter.RemoveAllDeliveredNotifications();
#endif
    }
    public void SetChannel()
    {
        if (GC.INS.noti)
        {
#if UNITY_ANDROID
            AndroidNotificationCenter.CancelAllDisplayedNotifications();
            var channel = new AndroidNotificationChannel()
            {
                Id = "channel_id",
                Name = "Default Channel",
                Importance = Importance.Default,
                Description = "Generic notifications",
            };

            AndroidNotificationCenter.RegisterNotificationChannel(channel);
#endif
        }
    }
    public void SetShiftNotification(int seconds)
    {
        if (!GC.INS.noti)
            return;
        CancelNotification();
#if UNITY_ANDROID
        var notification = new AndroidNotification();
        string title = "";
        string body = "";
        switch (GC.INS.idiom)
        {
            case 0:
                title = "🕰Your hotel's shift is over!!〽️";
                body = "Come back to keep getting money💸💸💸";
                break;
            case 1:
                title = "🕰¡¡Se Termino el turno de tu hotel!!〽️";
                body = "Regresa para seguir ganando dinero💸💸💸";
                break;
        }
        notification.Title = title;
        notification.Text = body;
        notification.FireTime = DateTime.Now.AddSeconds(seconds);

        AndroidNotificationCenter.SendNotification(notification, "channel_id");

        var notification2 = new AndroidNotification();

        switch (GC.INS.idiom)
        {
            case 0:
                title = "Your staff is missing you...😪";
                body = "Come back and put them to work!🏢";
                break;
            case 1:
                title = "Tu staff te extraña...😪";
                body = "¡Regresa y hazlos trabajar!🏢";
                break;
        }
        notification2.Title = title;
        notification2.Text = body;
        int days = 2;
        if (seconds > 40000)
        {
            days = 3;
        }
        else if (seconds > 90000)
        {
            days = 4;
        }
        notification2.FireTime = DateTime.Now.AddDays(days);
        AndroidNotificationCenter.SendNotification(notification2, "channel_id");
#endif
#if UNITY_IOS
        var timeTrigger = new iOSNotificationTimeIntervalTrigger()
        {
            TimeInterval = new TimeSpan(0, 0, seconds),
            Repeats = false
        };
        string title = "";
        string body = "";
        switch (GC.INS.idiom)
        {
            case 0:
                title = "🕰Your hotel's shift is over!!〽️";
                body = "Come back to keep getting money💸💸💸";
                break;
            case 1:
                title = "🕰¡¡Se Termino el turno de tu hotel!!〽️";
                body = "Regresa para seguir ganando dinero💸💸💸";
                break;
        }

        var notification = new iOSNotification()
        {
            // You can specify a custom identifier which can be used to manage the notification later.
            Identifier = "_notification_01",
            Title = title,
            Body = body,
            ShowInForeground = true,
            ForegroundPresentationOption = (PresentationOption.Alert | PresentationOption.Sound),
            CategoryIdentifier = "category_a",
            ThreadIdentifier = "thread1",
            Trigger = timeTrigger,
        };

        iOSNotificationCenter.ScheduleNotification(notification);

        switch (GC.INS.idiom)
        {
            case 0:
                title = "Your staff is missing you...😪";
                body = "Come back and put them to work!🏢";
                break;
            case 1:
                title = "Tu staff te extraña...😪";
                body = "¡Regresa y hazlos trabajar!🏢";
                break;
        }

        int days = 2;
        if (seconds > 40000)
        {
            days = 3;
        }
        else if (seconds > 90000)
        {
            days = 4;
        }
        var timeTrigger2 = new iOSNotificationTimeIntervalTrigger()
        {
            TimeInterval = new TimeSpan(days, 0, 0),
            Repeats = false
        };
        var notification2 = new iOSNotification()
        {
            // You can specify a custom identifier which can be used to manage the notification later.
            Identifier = "_notification_01",
            Title = title,
            Body = body,
            ShowInForeground = true,
            ForegroundPresentationOption = (PresentationOption.Alert | PresentationOption.Sound),
            CategoryIdentifier = "category_a",
            ThreadIdentifier = "thread1",
            Trigger = timeTrigger2,
        };

        iOSNotificationCenter.ScheduleNotification(notification2);
#endif
    }
    public void SaveToken(string token)
    {
        referenece.Child("Users").Child(Fire.INS.GetCurrentUser().UserId).SetValueAsync(new Dictionary<string, object>
                {
                    { "token",token},
                    { "idiom", GC.INS.idiom },
                });
    }
    public void SendFriendRequest(string uid)
    {
        referenece.Child("Users").Child(uid).UpdateChildrenAsync(new Dictionary<string, object>
                {
                    { "type",1},
                    { "lastFriend", GC.INS.username },
                });
    }
    public void AcceptFriendRequest(string uid)
    {
        referenece.Child("Users").Child(uid).UpdateChildrenAsync(new Dictionary<string, object>
                {
                    { "type",2},
                    { "lastFriend", GC.INS.username },
                });
    }
    public void SendGift(string uid)
    {
        referenece.Child("Users").Child(uid).UpdateChildrenAsync(new Dictionary<string, object>
                {
                    { "type",3},
                    { "lastFriend", GC.INS.username },
                });
    }
}
