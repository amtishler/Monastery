using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForegroundParallaxController : MonoBehaviour
{
    public GameObject tile;
    Vector2 tileSize;
    public GameObject player;
    Vector2 centerChunk = new Vector2(0,0);
    Vector2 offsetPlayerPosition = new Vector2(0,0);
    public GameObject tileParent;

    // Start is called before the first frame update
    void Start()
    {
        Vector3 bounds = tile.GetComponent<SpriteRenderer>().bounds.size;
        tileSize = new Vector2(bounds.x, bounds.y);
        //Debug.Log("tile size " + tileSize);

        tileParent.transform.position = Vector3.zero;
        for(int x = -1; x <= 1; x++) {
            for(int y = -1; y <= 1; y++) {
                Vector3 tilePos = new Vector3(x * tileSize.x, y * tileSize.y, 0);
                GameObject newTile = Instantiate(tile, tilePos, Quaternion.identity);
                newTile.transform.parent = tileParent.transform;
            }
        }
        tile.GetComponent<SpriteRenderer>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        offsetPlayerPosition = player.transform.position;
        //offsetPlayerPosition.x -= (tileSize.x / 2);
        //offsetPlayerPosition.y += (tileSize.y / 2);
        centerChunk.x = Mathf.Round(offsetPlayerPosition.x / tileSize.x);
        centerChunk.y = Mathf.Round(offsetPlayerPosition.y / tileSize.y);

        tileParent.transform.position = centerChunk * tileSize;
    }
}
