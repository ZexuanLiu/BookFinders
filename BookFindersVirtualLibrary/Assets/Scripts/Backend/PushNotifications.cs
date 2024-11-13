using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PushNotifications : MonoBehaviour
{
    [SerializeField] private TMP_InputField txtPushNotification;
    [SerializeField] private Button btnSend;
    [SerializeField] private TMP_Text notification;
    
    // Start is called before the first frame update
    void Start()
    {
        btnSend.onClick.AddListener(ShowNotification);
    }

    void ShowNotification()
    {
        var input = txtPushNotification.text;
        notification.text = input;
    }
    
    
}
