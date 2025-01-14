using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Tilemaps;



public class BoardManager : MonoBehaviour
{

    public class CellData
    {
        public bool isPassable;
    }

    private CellData[,] m_BoardData;
    private Tilemap m_TileMap;

    public int Width;
    public int Height;
    public Tile[] GroundTiles;
    public Tile[] BlockingTiles;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        m_TileMap = GetComponentInChildren<Tilemap>();

        m_BoardData = new CellData[Width, Height];

        for (int y = 0; y < Height; ++y)
        {
            for (int x = 0; x < Width; ++x)
            {
                Tile tile;
                m_BoardData[x, y] = new CellData();

                if (x == 0 || y == 0 || x == Width - 1 || y == Height - 1)
                {
                    tile = BlockingTiles[Random.Range(0, BlockingTiles.Length)];
                    m_BoardData[x, y].isPassable = false;
                }
                else
                {
                    tile = GroundTiles[Random.Range(0, GroundTiles.Length)];
                    m_BoardData[x, y].isPassable = true;
                }

                m_TileMap.SetTile(new Vector3Int(x, y, 0), tile);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
