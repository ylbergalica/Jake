using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileDestroyer : MonoBehaviour
{
	Tilemap tilemap;
	Vector3Int tilePos;

	public Grid grid;

    // Start is called before the first frame update
    void Start()
    {
        tilemap = GetComponent<Tilemap>();
    }

	public void DestroyTile(Vector2 contact)
	{
		tilePos = grid.WorldToCell(contact);
		tilemap.SetTile(tilePos, null);
	}
}
