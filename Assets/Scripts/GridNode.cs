using UnityEngine;
using System.Collections;
using System.IO;
using System.Text;

public class GridNode
{
    public enum NeighborPos
    {
        TopLeft,
        Top,
        TopRight,
        Left,
        Right,
        BottomLeft,
        Bottom,
        BottomRight
    }

    public enum CellState
    {
        Dead,
        Alive
    }

    private GridNode[] m_neighborNodes = new GridNode[8];
    private GameObject m_objectInstance;
    public GameObject ObjectInstance
    {
        get { return m_objectInstance; }
    }

    private CellState m_cellState;
    public CellState CurrentCellState
    {
        get { return m_cellState; }
    }

    private Sprite[] m_cellSprites;

    public GridNode(GameObject oInstance, Sprite[] s)
    {
        m_cellState = CellState.Dead;
        m_objectInstance = oInstance;
        m_cellSprites = s;
    }

    public CellState GetNeighborCellState(NeighborPos n)
    {
        int idx = (int)n;
        if (m_neighborNodes[idx] == null) return CellState.Dead;
        else return m_neighborNodes[idx].CurrentCellState;
    }

    public void AddNeighbor(NeighborPos n, GridNode g)
    {
        int idx = (int)n;
        if (m_neighborNodes[idx] == null) m_neighborNodes[idx] = g;
        else Debug.LogError("A GridNode already exists for idx " + idx);
    }

    public void SetCellState(CellState c)
    {
        m_cellState = c;
        m_objectInstance.GetComponent<SpriteRenderer>().sprite = m_cellSprites[(int)c];
    }

    public void ToggleCellState()
    {
        if (m_cellState == CellState.Alive) m_cellState = CellState.Dead;
        else m_cellState = CellState.Alive;
        m_objectInstance.GetComponent<SpriteRenderer>().sprite = m_cellSprites[(int)m_cellState];
    }

    public GridNode GetNeighbor(NeighborPos n)
    {
        return m_neighborNodes[(int)n];
    }
}
