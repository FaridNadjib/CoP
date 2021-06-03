using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChampionManager : MonoBehaviour
{

    [Header("ChampionBorders:")]
    [Tooltip("Have to me in the same order like Starsign enum.")]
    [SerializeField] Sprite[] borders;


    #region Singleton
    public static ChampionManager Instance;

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


    public Sprite GetChampionBorder(Starsign sign)
    {
        return borders[(int)sign];
    }

}
