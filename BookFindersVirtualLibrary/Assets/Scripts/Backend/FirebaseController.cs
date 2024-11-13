using System.Collections;
using System.Collections.Generic;
using Firebase.Messaging;
using UnityEngine;

public class FirebaseController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Firebase.Messaging.FirebaseMessaging.TokenReceived += OnTokenReceived;
        Firebase.Messaging.FirebaseMessaging.MessageReceived += OnMessageReceived;
    }
    
    public void OnTokenReceived(object sender, Firebase.Messaging.TokenReceivedEventArgs token)
    {
        Debug.Log("Received token: " + token.Token);
    }

    public void OnMessageReceived(object sender, Firebase.Messaging.MessageReceivedEventArgs e)
    {
        Debug.Log("Received message: " + e.Message.From);
    }
}
