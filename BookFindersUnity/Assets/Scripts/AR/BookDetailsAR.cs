using Assets.Scripts.Virtual_Library_Scripts.OnscreenDialogs;
using BookFindersVirtualLibrary.Models;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Networking;
using UnityEngine.UI;

public class BookDetailsAR : MonoBehaviour, IPointerClickHandler
{
    public GameObject bookDetails;

    [SerializeField] TextMeshProUGUI textMeshName;
    [SerializeField] TextMeshProUGUI textMeshAuthor;
    [SerializeField] TextMeshProUGUI textMeshLocationCode;
    [SerializeField] TextMeshProUGUI textMeshDescription;
    [SerializeField] TextMeshProUGUI textMeshISBNs;
    [SerializeField] RawImage bookImage;

    // Start is called before the first frame update
    void Start()
    {
        bookDetails.SetActive(false);

        if (BookSearchTracking.SelectedBook == null)
        {
            return;
        }
        textMeshName.text = BookSearchTracking.SelectedBook.Name;
        textMeshAuthor.text = BookSearchTracking.SelectedBook.Author;
        textMeshLocationCode.text = BookSearchTracking.SelectedBook.LocationCode;
        textMeshDescription.text = BookSearchTracking.SelectedBook.Description;
        textMeshISBNs.text = string.Join(',', BookSearchTracking.SelectedBook.Isbns);

        if (BookSearchTracking.SelectedBook.ImageLink != "defaultBook.png")
        {
            StartCoroutine(DownloadAndSetImage(BookSearchTracking.SelectedBook.ImageLink, bookImage));
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        bookDetails.SetActive(true);
    }

    IEnumerator DownloadAndSetImage(string url, RawImage imageComponent)
    {
        using (UnityWebRequest request = UnityWebRequestTexture.GetTexture(url))
        {
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError("Error downloading image: " + request.error);
            }
            else
            {
                Texture2D texture = DownloadHandlerTexture.GetContent(request);
                imageComponent.texture = texture;
                //set the weight and the height of the image
                RectTransform rectTransform = imageComponent.GetComponent<RectTransform>();

                float imageScaleFactor = 900.0f / texture.height;
                rectTransform.sizeDelta = new Vector2((float)texture.width * imageScaleFactor, (float)texture.height * imageScaleFactor);
            }
        }
    }
}
