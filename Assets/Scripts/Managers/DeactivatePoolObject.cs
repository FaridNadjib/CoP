using UnityEngine;

/// <summary>
/// Will deactivate poolobject after dalay.
/// </summary>
public class DeactivatePoolObject : MonoBehaviour
{
    [SerializeField] private string poolName;
    [SerializeField] private float deactivationTime;
    private float timer;

    private void OnEnable()
    {
        timer = 0.0f;
    }

    // Update is called once per frame
    private void Update()
    {
        if (timer < deactivationTime)
            timer += Time.deltaTime;
        else
            ObjectPool.Instance.AddToPool(this.gameObject, poolName);
    }
}