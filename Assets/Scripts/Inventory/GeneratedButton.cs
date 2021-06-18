using UnityEngine;

/// <summary>
/// This class will initialize the button ui for instantiated runtime buttons.
/// </summary>
public class GeneratedButton : MonoBehaviour
{
    // Index to add the related item from shop array.
    private int buttonIndex;

    public int ButtonIndex { get => buttonIndex; set => buttonIndex = value; }


    public void SpawnParticle(Transform pos)
    {
        //if(GetComponent<ButtonChampion>() != null || GetComponent<ButtonCrystal>() != null || GetComponent<ButtonEquipment>() != null || GetComponent<ButtonWeapon>() != null)
        //{
            GameObject tmp = ObjectPool.Instance.GetFromPool("UIPart2");
            tmp.transform.position = pos.position;
            tmp.SetActive(true);
        //}
        

    }
}