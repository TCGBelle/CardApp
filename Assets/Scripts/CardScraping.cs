using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;


public class CardScraping : MonoBehaviour
{
    //Annabelle McQuade
    //key u = unity i = int = s = string etc
    private List<string> sDecklistDepository = new List<string>();
    [SerializeField]
    private SceneManagerScript uSceneSwapper;
    [SerializeField]
    private GameObject uCheck;
    [SerializeField]
    private Color uGreen;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(GatherDecks());// Coroutine/threads to not be waiting on websites loading
    }

    // Update is called once per frame
    void Update()
    {
        if (PersistantManagerScript.Instance.bButtonPressed == true) //if button is pressed
        {
            for (int x = 0; x < sDecklistDepository.Count; x++)
            {
                if(NaiveSearch(PersistantManagerScript.Instance.sTarget, sDecklistDepository[x]) == true) // search all saved deck lists for one with name we want
                {
                    PersistantManagerScript.Instance.sPassedUrl = sDecklistDepository[x]; //pass the deck to other scene
                    PersistantManagerScript.Instance.bButtonPressed = false;
                    PersistantManagerScript.Instance.sTarget = null; // reset varaibles for if we return
                    uSceneSwapper.GoToDeckView(); // change scene
                    break;
                }
            }
        }
    }

    private IEnumerator GatherDecks()
    {
        List<string> sTempDecklUrlList = new List<string>();
        string sBaseSite;
        string sFullSite;
        int iPageNo;
        bool bLastPage;
        bool bFoundFirstDeck;
        string sOldFirstDeck = "";
        string sNewFirstDeck = "";
        sBaseSite = "https://fabtcg.com/decklists/?query=CC&page=";
        iPageNo = 0;
        bLastPage = false;
        bool bDidIFUp = false;
        while (bLastPage == false && bDidIFUp == false) // while not on the last page
        {
            bDidIFUp = true;
            bFoundFirstDeck = false;
            iPageNo++;
            sFullSite = sBaseSite + iPageNo.ToString(); //go to page= number of loops
            // go to the url if cant tell me why.
            using (UnityWebRequest unityWebRequest = UnityWebRequest.Get(sFullSite)) // connect to website
            {
                yield return unityWebRequest.SendWebRequest();//go do something else while we wait on connection
                if (unityWebRequest.result == UnityWebRequest.Result.ConnectionError || unityWebRequest.result == UnityWebRequest.Result.ProtocolError)
                {
                    //if connection failed tell me why
                    string error = unityWebRequest.error;
                    Debug.Log("erorr in coroutine");
                    Debug.Log("Could not contact fabtcg.com");
                    Debug.Log("Error: " + error);
                }
                else
                {
                    //if connection sucessed
                    string sHtmlCode = unityWebRequest.downloadHandler.text;
                    bDidIFUp = false;
                    Debug.Log("fabtcg reached: " + sFullSite);
                    string sTextToFindMain;
                    string sTextToFindSub;
                    while (sHtmlCode.IndexOf("<div class=\"block-table") != -1) // look for all table classes
                    {
                        sTextToFindMain = "<div class=\"block-table";
                        //seperate everything in the table class from rest of html
                        sHtmlCode = sHtmlCode.Substring(sHtmlCode.IndexOf(sTextToFindMain) + sTextToFindMain.Length);
                        sTextToFindSub = "href=\"https://fabtcg.com/decklists/";
                        while (sHtmlCode.IndexOf("href=\"https://fabtcg.com/decklists/") != -1) // find all the links in the table
                        {
                            sHtmlCode = sHtmlCode.Substring(sHtmlCode.IndexOf(sTextToFindSub) -29 + sTextToFindSub.Length);//dont know why i have to add and subtract the same number just dosnet wanna work with out it
                            string sDecklistUrl = sHtmlCode.Substring(0, sHtmlCode.IndexOf("\"")); //copy the link text into seperate varaible
                            if (sDecklistUrl != "")
                            {
                                if (bFoundFirstDeck == false) // if we havent found the first deck on the page
                                {
                                    sNewFirstDeck = sDecklistUrl;
                                    bFoundFirstDeck = true; // we now have
                                }
                                sTempDecklUrlList.Add(sDecklistUrl); // add the list to our temporary storage
                            }

                        }
                        for (int x = 0; x < sTempDecklUrlList.Count; x++)
                        {
                            if (sOldFirstDeck == sTempDecklUrlList[x]) // if our first deck lists matches the first deck list from previous page
                            {
                                bLastPage = true; // we are at the last page
                            }
                            else
                            {
                                sDecklistDepository.Add(sTempDecklUrlList[x]); // else add all decks to our master lists
                            }
                        }
                        sTempDecklUrlList.Clear(); // empty our temporary storage for connecting to a new page
                        sOldFirstDeck = sNewFirstDeck; // make this pages first deck the old pages first deck
                    }
                }
            }
        }
        PersistantManagerScript.Instance.bContinuable = true;
        uCheck.GetComponent<Renderer>().material.color = uGreen; // indicating getting decklists is done
    }

    public bool NaiveSearch(string target, string text) // basic string search cause our text is not that long
    {
        int n = text.Length;
        int m = target.Length;
        int i;
        int j;
        for (i = 0; i < n; i++)
        {
            for (j = 0; j < m && i + j < n; j++)
            {
                if (text[i + j] != target[j]) break;
            }
            if (j == m)
            {
                return true;
            }
        }
        return false;
    }
}
