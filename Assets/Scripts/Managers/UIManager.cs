using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{

    [SerializeField] GameObject dialogScreen;
    [SerializeField] TextMeshProUGUI dialogTextField;

    #region Singleton
    public static UIManager Instance;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        DontDestroyOnLoad(this.gameObject);
    }
    #endregion


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShowDialogScreen(bool status)
    {
        dialogScreen.SetActive(status);
    }

    public TextMeshProUGUI GetDialogTextField()
    {
        return dialogTextField;
    }
}
