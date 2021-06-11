using UnityEngine;

/// <summary>
/// Class to move a crystal popup.
/// </summary>
public class FloatingCrystal : MonoBehaviour
{
    private SpriteRenderer rend;
    [SerializeField] private float maxLiveTime;
    private float timer;
    [SerializeField] private float speed;
    private Vector3 pos = new Vector3();
    private Color oldColor;
    private Color invisibleColor;

    [SerializeField] private Crystal item;

    // Start is called before the first frame update
    private void Start()
    {
        rend = GetComponent<SpriteRenderer>();
        oldColor = rend.material.color;
        invisibleColor.a = 0f;
    }

    private void OnEnable()
    {
        transform.localPosition = Vector3.zero;
        pos = transform.localPosition;
        timer = 0;
    }

    // Update is called once per frame
    private void Update()
    {
        if (timer < maxLiveTime)
        {
            timer += Time.deltaTime;
            pos.y += speed * Time.deltaTime;
            transform.localPosition = pos;

            rend.material.color = Color.Lerp(oldColor, invisibleColor, (timer / maxLiveTime) * 0.8f);
        }
        else
        {
            ObjectPool.Instance.AddToPool(gameObject, item.ToString());
        }
    }
}