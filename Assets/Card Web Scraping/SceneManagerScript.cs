using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneManagerScript : MonoBehaviour
{
    public string passedURL;
    private void Start()
    {
        passedURL = PersistantManagerScript.Instance.passedUrl;
    }
    public void GoToTierList()
    {
        SceneManager.LoadScene("SampleScene");
    }
    public void GoToDeckView()
    {
        SceneManager.LoadScene("Deck Viewer");
    }
}
