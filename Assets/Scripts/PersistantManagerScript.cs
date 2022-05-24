using UnityEngine;

public class PersistantManagerScript : MonoBehaviour
{
    public static PersistantManagerScript Instance { get; private set; }

    public string sPassedUrl;
    public string sTarget;
    public bool bButtonPressed = false;
    public bool bContinuable = false; 

    //Game Instance/Global Variables the storing of varaibles between scenes and scripts. 
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
