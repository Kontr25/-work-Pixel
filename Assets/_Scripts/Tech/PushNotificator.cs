using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NotificationSamples;
using System;

public class PushNotificator : MonoBehaviour
{
    [SerializeField] private List<PushNotification> _notifications;

    [SerializeField] private GameNotificationsManager _notificationManager;

    public static PushNotificator Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;

            CreateChannel();

            foreach (var notification in _notifications)
            {
                CreateNotification(notification.Title, notification.MessageText, notification.DelayTime);
            }
        }

        DontDestroyOnLoad(this);
    }

    private void CreateChannel()
    {
        GameNotificationChannel channel = new GameNotificationChannel("testChannel", "TEST 1", "TEXT TEsT 1");
        _notificationManager.Initialize(channel);
    }

    private void CreateNotification(string title, string body, float time)
    {
        IGameNotification notification = _notificationManager.CreateNotification();

        if (notification != null)
        {
            notification.Title = title;
            notification.Body = body;
            notification.DeliveryTime = DateTime.Now.AddHours(time);
            notification.SmallIcon = "icon_0";
            notification.LargeIcon = "icon_1";

            _notificationManager.ScheduleNotification(notification);
        }
    }

    //public void Check()
    //{
    //    CreateNotification("lalala", "shishi shi shi", DateTime.Now.AddHours(2f));
    //}
}

[Serializable]
public class PushNotification
{
    [SerializeField] private string _title;
    [SerializeField] private string _messageText;
    [SerializeField] private float _delayTime;

    public string Title => _title;
    public string MessageText => _messageText;
    public float DelayTime => _delayTime;
}
