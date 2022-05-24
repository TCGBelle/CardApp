using UnityEngine;
using UnityEngine.UI;

public class ButtonPress : MonoBehaviour
{
    [SerializeField]
    private Button uBSOSButton, uPrismButton, uAzelaButton, uBSSButton, uBriarButton, uChaneButton, uDashButton, uDoriButton, uKanoButton,
        uKatsuButton, uLeviaButton, uLexiButton, uOldhimButton, uRhinarButton, uBoltButton, uVisButton, uSurveryButton;
    // Start is called before the first frame update
    private void Start()
    {
        //if button is pressed do appropriate function
        uBSOSButton.onClick.AddListener(() => ButtonClicked("bravo-star-of-the-show"));
        uPrismButton.onClick.AddListener(() => ButtonClicked("prism"));
        uBSSButton.onClick.AddListener(() => ButtonClicked("bravo-showstopper"));
        uBriarButton.onClick.AddListener(() => ButtonClicked("briar"));
        uChaneButton.onClick.AddListener(() => ButtonClicked("chane"));
        uDashButton.onClick.AddListener(() => ButtonClicked("dash"));
        uDoriButton.onClick.AddListener(() => ButtonClicked("dorinthea"));
        uKanoButton.onClick.AddListener(() => ButtonClicked("kano"));
        uKatsuButton.onClick.AddListener(() => ButtonClicked("katsu"));
        uLexiButton.onClick.AddListener(() => ButtonClicked("lexi"));
        uOldhimButton.onClick.AddListener(() => ButtonClicked("oldhim"));
        uRhinarButton.onClick.AddListener(() => ButtonClicked("rhinar"));
        uBoltButton.onClick.AddListener(() => ButtonClicked("boltyn"));
        uVisButton.onClick.AddListener(() => ButtonClicked("viserai"));
        uAzelaButton.onClick.AddListener(() => ButtonClicked("Azela"));
        uLeviaButton.onClick.AddListener(() => ButtonClicked("Levia"));
        uSurveryButton.onClick.AddListener(() => SurveyButtonClicked());
    }
    void ButtonClicked(string target)
    {
        if (PersistantManagerScript.Instance.bContinuable == true)
        {
            PersistantManagerScript.Instance.sTarget = target;
            PersistantManagerScript.Instance.bButtonPressed = true;
            //set varaibles to change scene
        }
    }
    void SurveyButtonClicked()
    {
        //open survey/tierlist maker
        Application.OpenURL("https://forms.gle/tAHj5ajidoecpg169");
    }
}
