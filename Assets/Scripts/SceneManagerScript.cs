using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagerScript : MonoBehaviour
{
    public string sPassedURL;
    private void Start()
    {
        sPassedURL = PersistantManagerScript.Instance.sPassedUrl;
    }
    public void GoToTierList()
    {
        SceneManager.LoadScene("SampleScene"); // swap to tier list
    }
    public void GoToDeckView()
    {
        SceneManager.LoadScene("Deck Viewer");// go to deck viewer scene
    }
}
