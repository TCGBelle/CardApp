using UnityEngine;
using UnityEngine.UI;

public class CardViewerButtonManager : MonoBehaviour
{
    public Button uBackButton;
    public Button uFabtcgButton;
    public SceneManagerScript sceneManager;
    // Start is called before the first frame update
    void Start()
    {
        //button pressed go to coresponding function
        uBackButton.onClick.AddListener(() => BackButtonClicked());
        uFabtcgButton.onClick.AddListener(() => FABButtonClicked());
    }
    void BackButtonClicked()
    {
        sceneManager.GoToTierList(); // go back
    }
    void FABButtonClicked()
    {
        Application.OpenURL(PersistantManagerScript.Instance.sPassedUrl); // open deck list in browser (incase cards didnt load right)
    }
}
