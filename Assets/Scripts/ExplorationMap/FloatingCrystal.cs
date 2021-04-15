using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingCrystal : MonoBehaviour
{
    SpriteRenderer rend;
    [SerializeField] float maxLiveTime;
    float timer;
    [SerializeField] float speed;
    Vector3 pos = new Vector3();
    Color oldColor;
    Color invisibleColor;

    [SerializeField] Crystals item;

    // Start is called before the first frame update
    void Start()
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
    void Update()
    {
        if (timer < maxLiveTime)
        {
            timer += Time.deltaTime;
            pos.y += speed * Time.deltaTime;
            transform.localPosition = pos;


            rend.material.color = Color.Lerp(oldColor, invisibleColor, (timer / maxLiveTime)*0.8f);
        }
        else
        {
            ObjectPool.Instance.AddToPool(gameObject, item.ToString());
        }
    }
}
