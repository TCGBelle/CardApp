using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Net;
using UnityEngine.Networking;


public class CardScraping : MonoBehaviour
{
    //Annabelle McQuade
    /*The purpose of this code is a proof of concept
     this code can load any deck list from fabtcg.com
    it can then get another link from inside that html
    it saves that link into a list
    it gose to that link and saves an image into another list
    it uses that saved image to make a sprite
    it cycles through all the cards on the list to show you the complete deck list
    this shows that i will be able to code a system that can get the url for multiple deck lists.
    go to the deck lists using those url
    copy the deck lists and other information
    copy the cards images to make ui easier to read
    and then display it
    UI will be refined to allow users to look through multiple decks.
    Project is also now set up correctly as unity default settings needed changed and libararys installed
    it dose this by getting the html code uning UnityWebRequest libarary
    searching for the tables in which the cards are stored*/
    private List<Texture2D> cardSpriteList = new List<Texture2D>();
    private List<string> cardUrlList = new List<string>();
    private int frameCount = 0;
    private int listCount = 0;
    private List<string> FABDepositoryURL = new List<string>();
    private List<string> decklistDepository = new List<string>();
    // private List<string> PlayerNames = new List<string>;
    // private List<string> WinningDate = new List<string>;
    private bool decksFinnished = false;
    private bool waiting = false;

    // Start is called before the first frame update
    void Start()
    {
        decksFinnished = false;
        StartCoroutine(GatherDecksTwo());
        //GetWinningLinks();
        //GatherDecks();
        //DownloadData();
        //loop url list
        if(KMPStringSearch("glade", "bangladesh")[0] >0)// target from button, url[loop]
        {

        }
        //if found > 0
        //change scene and load deck
       
        KMPStringSearch("ABABCABAB", "ABABDABACDABABCABAB");
    }

    // Update is called once per frame
    void Update()
    {
        if (decksFinnished == true)
        {
            frameCount++;
            if (frameCount >= 400)
            {
                if (listCount < cardSpriteList.Count)
                {
                    listCount++;
                    // make new sprite from texture 2d
                    this.gameObject.GetComponent<SpriteRenderer>().sprite = Sprite.Create(cardSpriteList[listCount], new Rect(0, 0, cardSpriteList[listCount].width, cardSpriteList[listCount].height), new Vector2(0, 0));
                }
                else
                {
                    listCount = 0;
                }
                frameCount = 0;
            }
        }
    }

    private void DownloadData(string url)
    {
        // can be replaced with any fabtcg.com/decklists link
        //string url = "https://fabtcg.com/decklists/pietro-gerletti-kano-deck-italy-national-championship-271121/";
        Debug.Log("Downloading Decks...");
        GetSite(url, (string error) =>
        {
            // if cant reach the url tell me why
            Debug.Log("Could not contact fabtcg.com");
            Debug.Log("Error: " + error);
        }, (string htmlCode) =>
        {
            Debug.Log("fabtcg deck downloaded");
            string textToFindMain;
            string textToFindSub;

            //if format is CC keep it

            while (htmlCode.IndexOf("<div class=\"mb-4 col-12") != -1) // look for all table classes
            {

                textToFindMain = "<div class=\"mb-4 col-12";
                //seperate everything in the table class from rest of html
                htmlCode = htmlCode.Substring(htmlCode.IndexOf(textToFindMain) + textToFindMain.Length);
                Debug.Log("Ive reached checkpoint 2: " + htmlCode);
                textToFindSub = "href=\"";
                //look for all links in the table classes
                while (htmlCode.IndexOf("href=\"https://storage.googleapis.com/fabmaster/media/images/") != -1)
                {
                    //seperate the link from other html
                    htmlCode = htmlCode.Substring(htmlCode.IndexOf(textToFindSub) + textToFindSub.Length);
                    string imageUrl = htmlCode.Substring(0, htmlCode.IndexOf("\""));
                    //download image
                    GetTexture(imageUrl, (string error) =>
                    {
                        // if cant download image tell me why
                        Debug.Log("Failed to download card image");
                        Debug.Log("Error: " + error);
                    }, (Texture2D texture) =>
                    {
                        // downloaded image and url saved to lists for later use
                        cardSpriteList.Add(texture);
                        cardUrlList.Add(imageUrl);
                        Debug.Log("Cards Downloaded: " + cardSpriteList.Count);
                    });
                }
            }
            Debug.Log(cardUrlList);
        });
    }

    public void GetSite(string url, Action<string> onError, Action<string> onSuccess)
    {
        //UnityWebRequest Getter in diffrent class for multiple calls later
        MyWebRequests.Get(url, onError, onSuccess);
    }

    public void GetTexture(string url, Action<string> onError, Action<Texture2D> onSuccess)
    {
        //UnityWebRequest Getter in diffrent class for multiple calls later
        MyWebRequests.GetTexture(url, onError, onSuccess);
    }

    public bool ParseDeck(string searchterm)
    {
        //for deck in master list
        // searching terms
        /*string Azela = "Azela";
        string Bravo = "Bravo Showstoper";
        string Bravatar = "Bravo Star";
        string Briar = "Briar";
        string Chane = "Chane";
        string Dash = "Dash";
        string Dorinthia = "Dorinthia";
        string Kano = "Kano";
        string Katsu = "Katsu";
        string Levia = "Levia";
        string Lexi = "Lexi";
        string Oldhim = "Oldhim";
        string Prism = "Prism";
        string Rhinar = "Rhinar";
        string SerBoltyn = "Ser Boltyn";
        string Viserai = "Viserai";*/
        //analyze deck
        //loop through list
        for(int x = 0; x < decklistDepository.Count; x++)
        {
            // check if string matches string
            if (decklistDepository[x] == searchterm)
            {
                // start couroutine to see if format is CC if yes download images
                // once found newest deck list
                //load site
                StartCoroutine(GatherImages(decklistDepository[x]));
                // if format is CC
                // get numbers + links
                //change scene
                return true;
            }
        }
        return false;
    }

    private IEnumerator GatherDecksTwo()
    {
        decksFinnished = false;
        List<string> tempDecklUrlList = new List<string>();
        string baseSite;
        string fullSite;
        int n;
        bool lastPage;
        bool foundFirstDeck;
        string oldFirstDeck = "";
        string newFirstDeck = "";
        baseSite = "https://fabtcg.com/decklists/?page=";
        n = 0;
        lastPage = false;
        bool didIFUp = false;
        while (lastPage == false && didIFUp == false)
        {
            didIFUp = true;
            foundFirstDeck = false;
            //i = 1 to n
            n++;
            fullSite = baseSite + n.ToString();
            Debug.Log(fullSite);
            // go to the url if cant tell me why.
            using (UnityWebRequest unityWebRequest = UnityWebRequest.Get(fullSite))
            {
                yield return unityWebRequest.SendWebRequest();
                if (unityWebRequest.result == UnityWebRequest.Result.ConnectionError || unityWebRequest.result == UnityWebRequest.Result.ProtocolError)
                {
                    string error = unityWebRequest.error;
                    Debug.Log("erorr in coroutine");
                    Debug.Log("Could not contact fabtcg.com");
                    Debug.Log("Error: " + error);
                }
                else
                {
                    string htmlCode = unityWebRequest.downloadHandler.text;
                    didIFUp = false;
                    Debug.Log("fabtcg reached: " + fullSite);
                    string textToFindMain;
                    string textToFindSub;
                    string temphtmlCode;
                    temphtmlCode = htmlCode;
                    while (htmlCode.IndexOf("<div class=\"block-table") != -1) // look for all table classes
                    {
                        //Debug.Log(htmlCode);
                        textToFindMain = "<div class=\"block-table";
                        //seperate everything in the table class from rest of html
                        htmlCode = htmlCode.Substring(htmlCode.IndexOf(textToFindMain) + textToFindMain.Length);
                        textToFindSub = "href=\"";
                        while (htmlCode.IndexOf("href=\"https://fabtcg.com/decklists/") != -1)
                        {
                            //Debug.Log(htmlCode);
                            htmlCode = htmlCode.Substring(htmlCode.IndexOf(textToFindSub) + textToFindSub.Length);
                            string decklistUrl = htmlCode.Substring(0, htmlCode.IndexOf("\""));
                            //Debug.Log(decklistUrl);
                            if (decklistUrl != "")
                            {
                                if (foundFirstDeck == false)
                                {
                                    newFirstDeck = decklistUrl;
                                    foundFirstDeck = true;
                                }
                                tempDecklUrlList.Add(decklistUrl);
                            }
                            //check if deck 1 is equal to stored deck
                            // get a temp list with all the deck list links on a page. 
                            //loop through the list to check if it equals the previous pages first
                            //if so return
                            //else add to main list
                            // make first deck equal to this pages first deck.

                        }
                        for (int x = 0; x < tempDecklUrlList.Count; x++)
                        {
                            if (oldFirstDeck == tempDecklUrlList[x])
                            {
                                lastPage = true;
                            }
                            else
                            {
                                decklistDepository.Add(tempDecklUrlList[x]);
                            }
                        }
                        tempDecklUrlList.Clear();
                        oldFirstDeck = newFirstDeck;
                    }
                }
            }
        }
        Debug.Log(decklistDepository);
    }
    private IEnumerator GatherImages(string address)
    {
        using (UnityWebRequest unityWebRequest = UnityWebRequest.Get(address))
        {
            yield return unityWebRequest.SendWebRequest();
            if (unityWebRequest.result == UnityWebRequest.Result.ConnectionError || unityWebRequest.result == UnityWebRequest.Result.ProtocolError)
            {
                string error = unityWebRequest.error;
                Debug.Log("erorr in coroutine");
                Debug.Log("Could not contact fabtcg.com");
                Debug.Log("Error: " + error);
            }
            else
            {
                string htmlCode = unityWebRequest.downloadHandler.text;
                Debug.Log("fabtcg reached: " + address);
                string textToFindMain;
                //text to find
            }
        }
    }

    public List<int> KMPStringSearch(string pattern, string text)
    {
        //Debug.Log(pattern);
        //Debug.Log(text);
        int TexLen = text.Length;
        int PatLen = pattern.Length;

        //if (TexLen < PatLen) return -1;
        //if (TexLen == PatLen && text == pattern) return 0;
        //if (PatLen == 0) return 0;

        int[] lpsArray = new int[PatLen];
        List<int> matchedIndex = new List<int>();

        GetNext(pattern, ref lpsArray);

        int i = 0, j = 0;
        while(i<TexLen)
        {
            if (text[i] == pattern[j])
            {
                i++;
                j++;
            }
            if (j == PatLen)
            {
                matchedIndex.Add(i - j);
                Debug.Log((i - j).ToString());
                j = lpsArray[j - 1];
            }
            else if(i<TexLen&& text[i] != pattern[j])
            {
                if (j != 0)
                {
                    j = lpsArray[j - 1];
                }
                else
                {
                    i++;
                }
            }
        }
        Debug.Log(matchedIndex);
        return matchedIndex;
    }

    void GetNext(string pattern, ref int[] lpsArray)
    {
        int M = pattern.Length;
        int len = 0;
        lpsArray[0] = 0;
        int i = 1;

        while (i < M)
        {
            if (pattern[i] == pattern[len])
            {
                len++;
                lpsArray[i] = len;
                i++;
            }
            else
            {
                if (len == 0)
                {
                    lpsArray[i] = 0;
                    i++;
                }
                else
                {
                    len = lpsArray[len - 1];
                }
            }
        }
    }

}
