using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class is the object pool controller. It stores pools of objects and returns the next one which can be set to true by the receiver. 
/// In case a bigger pool is needed it instantiates new objects.
/// </summary>
public class ObjectPool : MonoBehaviour
{
    #region Fields
    public Dictionary<string, Queue<GameObject>> poolDictionary;
    Dictionary<string, GameObject> prefabDictionary;

    [SerializeField] int poolGrowRate;


    [Header("Emoji related prefabs:")]
    [Header("Prefabs the objectpools are made of:")]
    [SerializeField] GameObject angryPrefab;
    [SerializeField] GameObject attentionPrefab;
    [SerializeField] GameObject goodMoodPrefab;
    [SerializeField] GameObject killPrefab;
    [SerializeField] GameObject lovePrefab;
    [SerializeField] GameObject sleepyPrefab;
    [SerializeField] GameObject talkingPrefab;
    [SerializeField] GameObject waitingPrefab;

    [Header("Crystal related Prefabs:")]
    [SerializeField] GameObject goldPrefab;
    [SerializeField] GameObject attackPrefab;
    [SerializeField] GameObject alignmentPrefab;
    [SerializeField] GameObject firePrefab;
    [SerializeField] GameObject icePrefab;
    [SerializeField] GameObject lightningPrefab;
    [SerializeField] GameObject windPrefab;
    [SerializeField] GameObject destructionPrefab;
    [SerializeField] GameObject holyPrefab;
    [SerializeField] GameObject hunterPrefab;
    [SerializeField] GameObject seadragonPrefab;

    [Header("Attacks related prefabs:")]
    [SerializeField] GameObject textPopup;

    // Pools for every poolable object in game.
    // Emoji related pools.
    Queue<GameObject> angryPool = new Queue<GameObject>();
    Queue<GameObject> attentionPool = new Queue<GameObject>();
    Queue<GameObject> goodMoodPool = new Queue<GameObject>();
    Queue<GameObject> killPool = new Queue<GameObject>();
    Queue<GameObject> lovePool = new Queue<GameObject>();
    Queue<GameObject> sleepyPool = new Queue<GameObject>();
    Queue<GameObject> talkingPool = new Queue<GameObject>();
    Queue<GameObject> waitingPool = new Queue<GameObject>();

    Queue<GameObject> goldPool = new Queue<GameObject>();
    Queue<GameObject> attackPool = new Queue<GameObject>();
    Queue<GameObject> alignmentPool = new Queue<GameObject>();
    Queue<GameObject> firePool = new Queue<GameObject>();
    Queue<GameObject> icePool = new Queue<GameObject>();
    Queue<GameObject> lightningPool = new Queue<GameObject>();
    Queue<GameObject> windPool = new Queue<GameObject>();
    Queue<GameObject> destructionPool = new Queue<GameObject>();
    Queue<GameObject> holyPool = new Queue<GameObject>();
    Queue<GameObject> hunterPool = new Queue<GameObject>();
    Queue<GameObject> seadragonPool = new Queue<GameObject>();

    Queue<GameObject> textPopupPool = new Queue<GameObject>();

    // Battleeffectpools.
    [SerializeField] GameObject[] battleEffectPrefabs;
    Queue<GameObject>[] battleEffectPools;

    // UI particles
    [SerializeField] GameObject UIPart1Prefab;
    Queue<GameObject> UIPart1Pool = new Queue<GameObject>();
    [SerializeField] GameObject UIPart2Prefab;
    Queue<GameObject> UIPart2Pool = new Queue<GameObject>();
    #endregion

    #region Singleton
    public static ObjectPool Instance;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        DontDestroyOnLoad(this.gameObject);
    }
    #endregion

    /// <summary>
    /// Initialize the pool dictionary and add all object pools to it.
    /// </summary>
    private void Start()
    {
        poolDictionary = new Dictionary<string, Queue<GameObject>>();
        prefabDictionary = new Dictionary<string, GameObject>();
        // Add all emoji related pools to the pool dictionary. And add the corresponding prefab to the prefab dictionary.
        poolDictionary.Add(Emotion.Angry.ToString(), angryPool);
        prefabDictionary.Add(Emotion.Angry.ToString(), angryPrefab);
        poolDictionary.Add(Emotion.Attention.ToString(), attentionPool);
        prefabDictionary.Add(Emotion.Attention.ToString(), attentionPrefab);
        poolDictionary.Add(Emotion.GoodMood.ToString(), goodMoodPool);
        prefabDictionary.Add(Emotion.GoodMood.ToString(), goodMoodPrefab);
        poolDictionary.Add(Emotion.Kill.ToString(), killPool);
        prefabDictionary.Add(Emotion.Kill.ToString(), killPrefab);
        poolDictionary.Add(Emotion.Love.ToString(), lovePool);
        prefabDictionary.Add(Emotion.Love.ToString(), lovePrefab);
        poolDictionary.Add(Emotion.Sleepy.ToString(), sleepyPool);
        prefabDictionary.Add(Emotion.Sleepy.ToString(), sleepyPrefab);
        poolDictionary.Add(Emotion.Talking.ToString(), talkingPool);
        prefabDictionary.Add(Emotion.Talking.ToString(), talkingPrefab);
        poolDictionary.Add(Emotion.Waiting.ToString(), waitingPool);
        prefabDictionary.Add(Emotion.Waiting.ToString(), waitingPrefab);

        // Add all crystal related pools to the pool dictionary. And add the corresponding prefab to the prefab dictionary.
        poolDictionary.Add(Crystal.Gold.ToString(), goldPool);
        prefabDictionary.Add(Crystal.Gold.ToString(), goldPrefab);
        poolDictionary.Add(Crystal.Attack.ToString(), attackPool);
        prefabDictionary.Add(Crystal.Attack.ToString(), attackPrefab);
        poolDictionary.Add(Crystal.Darkness.ToString(), alignmentPool);
        prefabDictionary.Add(Crystal.Darkness.ToString(), alignmentPrefab);
        poolDictionary.Add(Crystal.Fire.ToString(), firePool);
        prefabDictionary.Add(Crystal.Fire.ToString(), firePrefab);
        poolDictionary.Add(Crystal.Ice.ToString(), icePool);
        prefabDictionary.Add(Crystal.Ice.ToString(), icePrefab);
        poolDictionary.Add(Crystal.Lightning.ToString(), lightningPool);
        prefabDictionary.Add(Crystal.Lightning.ToString(), lightningPrefab);
        poolDictionary.Add(Crystal.Wind.ToString(), windPool);
        prefabDictionary.Add(Crystal.Wind.ToString(), windPrefab);
        poolDictionary.Add(Crystal.Destruction.ToString(), destructionPool);
        prefabDictionary.Add(Crystal.Destruction.ToString(), destructionPrefab);
        poolDictionary.Add(Crystal.Holy.ToString(), holyPool);
        prefabDictionary.Add(Crystal.Holy.ToString(), holyPrefab);
        poolDictionary.Add(Crystal.Hunter.ToString(), hunterPool);
        prefabDictionary.Add(Crystal.Hunter.ToString(), hunterPrefab);
        poolDictionary.Add(Crystal.Seadragon.ToString(), seadragonPool);
        prefabDictionary.Add(Crystal.Seadragon.ToString(), seadragonPrefab);

        // Add all attack related pools to the dictionary.
        poolDictionary.Add("textPopup", textPopupPool);
        prefabDictionary.Add("textPopup", textPopup);

        // UI particles.
        poolDictionary.Add("UIPart1", UIPart1Pool);
        prefabDictionary.Add("UIPart1", UIPart1Prefab);
        poolDictionary.Add("UIPart2", UIPart2Pool);
        prefabDictionary.Add("UIPart2", UIPart2Prefab);

        battleEffectPools = new Queue<GameObject>[battleEffectPrefabs.Length];
        for (int i = 0; i < battleEffectPools.Length; i++)
        {
            battleEffectPools[i] = new Queue<GameObject>();
        }

        // Add all battleEffects to the pool.
        for (int i = 0; i < battleEffectPools.Length; i++)
        {
            poolDictionary.Add(((BattleEffects)(i + 1)).ToString(), battleEffectPools[i]);
        }

        for (int i = 0; i < battleEffectPrefabs.Length; i++)
            prefabDictionary.Add(((BattleEffects)(i + 1)).ToString(), battleEffectPrefabs[i]);

    }

    /// <summary>
    /// If another method needs an object to "instantiate", get the object from the pool instead.
    /// </summary>
    /// <param name="type">The name of the pool you want an object from.</param>
    /// <returns>The GameObject the receiver can work with.</returns>
    public GameObject GetFromPool(string poolName)
    {
        if (poolDictionary.ContainsKey(poolName))
        {
            if (poolDictionary[poolName].Count == 0)
                GrowPool(poolName);
            return poolDictionary[poolName].Dequeue();
        }
        else
        {
            Debug.Log("Pool with tag: " + poolName + " doesnt exist.");
            return null;
        }
    }

    /// <summary>
    /// Instantiates more from the pool specific prefabs if needed.
    /// </summary>
    /// <param name="poolToGrow">The pool to instantiate more prefabs for.</param>
    private void GrowPool(string poolToGrow)
    {
        if (prefabDictionary.ContainsKey(poolToGrow))
        {
            for (int i = 0; i < poolGrowRate; i++)
            {
                var instanceToAdd = Instantiate(prefabDictionary[poolToGrow]);
                instanceToAdd.transform.SetParent(transform);
                AddToPool(instanceToAdd, poolToGrow);
            }
        }
    }

    /// <summary>
    /// Deactivates the already instantiated gameobject first. And then adds it to its corresponding pool.
    /// </summary>
    /// <param name="instance">The gameobject to add to the pool.</param>
    /// <param name="poolToAddTo">The pool to add the gameobject to.</param>
    public void AddToPool(GameObject instance, string poolToAddTo)
    {
        instance.SetActive(false);
        instance.transform.SetParent(transform);
        poolDictionary[poolToAddTo].Enqueue(instance);
    }
}
