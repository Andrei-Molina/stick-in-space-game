using UnityEngine;

public class BackgroundTiler : MonoBehaviour
{
    public Transform player;
    public float recycleOffset = 10f;

    private Transform[] tiles;
    private float[] tileWidths;

    void Start()
    {
        int count = transform.childCount;
        tiles = new Transform[count];
        tileWidths = new float[count];

        for (int i = 0; i < count; i++)
        {
            tiles[i] = transform.GetChild(i);
            // Auto calculate width from renderer bounds
            var renderer = tiles[i].GetComponent<SpriteRenderer>();
            tileWidths[i] = renderer.bounds.size.x;
        }
    }

    void Update()
    {
        for (int i = 0; i < tiles.Length; i++)
        {
            float width = tileWidths[i];
            if (tiles[i].position.x + width < player.position.x - recycleOffset)
            {
                float rightMostX = GetRightMostTileX();
                tiles[i].position = new Vector3(rightMostX + width, tiles[i].position.y, tiles[i].position.z);
            }
        }
    }

    float GetRightMostTileX()
    {
        float maxX = float.MinValue;
        foreach (var tile in tiles)
        {
            if (tile.position.x > maxX)
                maxX = tile.position.x;
        }
        return maxX;
    }
}
