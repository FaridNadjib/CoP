using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TestGen : MonoBehaviour
{

    Tilemap map;
    TileBase tile;
    [SerializeField] TileBase te;
    
    // Start is called before the first frame update
    void Start()
    {
        map = GetComponent<Tilemap>();
        Debug.Log("CellBounds:" + map.cellBounds);
        Debug.Log("Size:" + map.size);
        Debug.Log("Origin:" + map.origin);

        tile = map.GetTile(new Vector3Int(0, 0, 1));
        //map.DeleteCells(new Vector3Int(0, 0, 1), new Vector3Int(1, 1, 1));
        map.SetTile(new Vector3Int(0, 0, 1), null);
        map.SetTile(new Vector3Int(3, 3, 1), te);
        map.SetTile(new Vector3Int(4, 4, 1), te);
        map.SetTile(new Vector3Int(2, 2, 1), te);
        map.SetTile(new Vector3Int(1, 1, 1), te);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
