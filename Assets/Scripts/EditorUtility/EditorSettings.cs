using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class is used to manage all kind of gizmo settings for easyer setting up maps and triggers and stuff.
/// </summary>
[CreateAssetMenu(menuName = "Create EditorSettings")]
public class EditorSettings : ScriptableObject
{
    #region Fields
    [SerializeField] bool showJumpTriggers = true;
    [SerializeField] Color jumpTriggerColor;

    [SerializeField] bool showMoveableObjectTriggers = true;
    [SerializeField] Color moveableObjectTriggerColor;

    [SerializeField] bool showNpcTriggers = true;
    [SerializeField] Color npcTriggerColor;
    #endregion

    #region Properties
    public bool ShowJumpTriggers { get => showJumpTriggers; }
    public Color JumpTriggerColor { get => jumpTriggerColor; }
    public bool ShowMoveableObjectTriggers { get => showMoveableObjectTriggers; }
    public Color MoveableObjectTriggerColor { get => moveableObjectTriggerColor; }
    public bool ShowNpcTriggers { get => showNpcTriggers; }
    public Color NpcTriggerColor { get => npcTriggerColor; }
    #endregion

    //#region Singleton
    //public static EditorSettings Instance;

    //private void Awake()
    //{
    //    if (Instance == null)
    //        Instance = this;
    //    else
    //        Destroy(gameObject);

    //    DontDestroyOnLoad(this.gameObject);
    //}
    //#endregion
}
