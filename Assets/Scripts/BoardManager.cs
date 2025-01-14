using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Tilemaps;



public class BoardManager : MonoBehaviour
{

    public class CellData
    {
        public bool isPassable;
    }

    private CellData[,] m_boardData;

    private Tilemap tileMap;

    public int width;
    public int height;
    public Tile[] groundTiles;
    public Tile[] wallTiles;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        tileMap = GetComponentInChildren<Tilemap>();

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Tile tile;
                CellData cellData = new CellData();
                if (x == 0 || x == width - 1 || y == 0 || y == height - 1)
                {
                    tile = wallTiles[Random.Range(0, wallTiles.Length)];
                    cellData.isPassable = false;
                }
                else
                {
                    tile = groundTiles[Random.Range(0, groundTiles.Length)];
                    cellData.isPassable = true;
                }
                tileMap.SetTile(new Vector3Int(x, y, 0), tile);
                m_boardData[x, y] = cellData;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
