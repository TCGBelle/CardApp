using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Cardviewer : MonoBehaviour
{
    //Annabelle McQuade
    //key u = unity i = int = s = string etc
    public SpriteRenderer[] uCardRenders = new SpriteRenderer[80];
    private List<Texture2D> uCardSpriteList = new List<Texture2D>();
    private List<string> sCardUrlList = new List<string>();
    private int iReloadSpriteCount;

    [SerializeField]
    private GameObject uCheck;
    [SerializeField]
    private Color uGreen;

    void Start()
    {
        GatherCards(PersistantManagerScript.Instance.sPassedUrl); // start searching for cards in passed deck list
        iReloadSpriteCount = 0;
    }
    private void Update()
    {
        if (uCardSpriteList.Count > 70+iReloadSpriteCount) //once 70 cards have been found start loading sprites
        {
            iReloadSpriteCount++; // if more sprites found allows for reload till we have them all
            uCheck.GetComponent<Renderer>().material.color = uGreen; //indicating web scraping finnished to user
            DisplayCards();
        }
    }
    private void DisplayCards()
    {
            Sprite uSprite;
            for (int x = 0; x < uCardSpriteList.Count; x++)
            {
                uSprite = Sprite.Create(uCardSpriteList[x], new Rect(0, 0, uCardSpriteList[x].width, uCardSpriteList[x].height), new Vector2(0.5f, 0.5f));
                uCardRenders[x].sprite = uSprite; // for every card load the correct sprite
            }
    }
    void GatherCards(string url)
    {
        GetSite(url, (string error) =>
            {
                //if cant connect tell me why
                Debug.Log("i failed:" + error);
            }, (string htmlCode) =>
            {
                //if connected
                string sTextToFindMain;
                string sTextToFindSub;
                char cNoOfCards;
                int iNoOfCards = 1;

                while (htmlCode.IndexOf("<div class=\"mb-4 col-12") != -1) // look for all table classes
                {

                    sTextToFindMain = "<div class=\"mb-4 col-12";
                    //seperate everything in the table class from rest of html
                    htmlCode = htmlCode.Substring(htmlCode.IndexOf(sTextToFindMain) + sTextToFindMain.Length);
                    sTextToFindSub = "<td"; //look for table line
                    while (htmlCode.IndexOf("<td") != -1)
                    {
                        htmlCode = htmlCode.Substring(htmlCode.IndexOf(sTextToFindSub) + sTextToFindSub.Length); // seperate line from rest of code
                        cNoOfCards = htmlCode[1];
                        iNoOfCards = (int)Char.GetNumericValue(cNoOfCards); // get the number of cards used in deck list e.g 3xWartune Herald we want the 3
                        string imageUrl = htmlCode.Substring(htmlCode.IndexOf("href=\"")+ 6, htmlCode.IndexOf("\" target=")-htmlCode.IndexOf("href=\"")-6); // seperate the link
                        if (imageUrl != "")
                        {
                            Debug.Log(imageUrl);
                            GetTexture(imageUrl, iNoOfCards, (string error) => //connect to site storing the picture
                            {
                                // if cant download image tell me why
                                Debug.Log("Failed to download card image");
                                Debug.Log("Error: " + error);
                            }, (Texture2D texture) =>
                            {
                                // downloaded image and url saved to lists for later use
                                    Debug.Log("Cards Downloaded: " + uCardSpriteList.Count);
                            });
                        }
                    }
                }
            });
    }
    public void GetSite(string url, Action<string> onError, Action<string> onSuccess)
    {
        //we have to do this a diffrent way to card scrapping as we are connecting to multiple sites at the same time now
        //UnityWebRequest Getter in diffrent class for multiple calls later
        MyWebRequests.Get(url, onError, onSuccess);
    }

    public void GetTexture(string url, int noOfCards, Action<string> onError, Action<Texture2D> onSuccess)
    {
        //we have to do this a diffrent way to card scrapping as we are connecting to multiple sites at the same time now
        //UnityWebRequest Getter in diffrent class for multiple calls later
        StartCoroutine(GetTextureCoroutine(url,noOfCards, onError, onSuccess));
    }

    public IEnumerator GetTextureCoroutine(string url, int noOfCards, Action<string> onError, Action<Texture2D> onSuccess)
    {
        // has to be in this class to add texture to our list = to the number of the card
        using (UnityWebRequest unityWebRequest = UnityWebRequestTexture.GetTexture(url))
        {
            yield return unityWebRequest.SendWebRequest();

            if (unityWebRequest.isNetworkError || unityWebRequest.isHttpError)
            {
                onError(unityWebRequest.error);
            }
            else
            {
                DownloadHandlerTexture downloadHandlerTexture = unityWebRequest.downloadHandler as DownloadHandlerTexture;
                sCardUrlList.Add(url);
                for (int x = 0; x < noOfCards; x++)
                {
                    uCardSpriteList.Add(downloadHandlerTexture.texture);
                }
                onSuccess(downloadHandlerTexture.texture);
            }
        }
    }
}
