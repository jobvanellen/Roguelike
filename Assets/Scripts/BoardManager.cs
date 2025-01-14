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
    private Grid m_Grid;

    public int Width;
    public int Height;
    public Tile[] GroundTiles;
    public Tile[] BlockingTiles;

    public PlayerController Player;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        m_Grid = GetComponentInChildren<Grid>();
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

        Player.Spawn(this, new Vector2Int(1,1));
    }

    public Vector3 CellToWorld(Vector2Int cell)
    {
        return m_Grid.GetCellCenterWorld(new Vector3Int(cell.x, cell.y, 0));
    }

    public CellData GetCellData(Vector2Int cell)
    {
        if(cell.x < 0 || cell.x >= Width || cell.y < 0 || cell.y >= Height)
        {
            return null;
        }
        return m_BoardData[cell.x, cell.y];
    }

}
