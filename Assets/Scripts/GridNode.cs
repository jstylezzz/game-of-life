/*
 * Copyright (c) Jari Senhorst. All rights reserved.  
 * 
 * Licensed under the MIT License. See LICENSE file in the project root for full license information.  
 * 
 * 
 * This is the GridNode class. It holds all data a grid node needs.
 * 
 */

using UnityEngine;
using System.Collections;
using System.IO;
using System.Text;

public class GridNode
{
    /// <summary>
    /// The position of a neighbor relative to the this GridNode
    /// </summary>
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

    /// <summary>
    /// The existing cell states
    /// </summary>
    public enum CellState
    {
        Dead,
        Alive
    }

    /// <summary>
    /// A list of neighboring GridNodes (cells)
    /// </summary>
    private GridNode[] m_neighborNodes = new GridNode[8];

    /// <summary>
    /// The phyisical game object instance of this GridNode
    /// </summary>
    private GameObject m_objectInstance;
    public GameObject ObjectInstance
    {
        get { return m_objectInstance; }
    }

    /// <summary>
    /// This GridNode's state
    /// </summary>
    private CellState m_cellState;
    public CellState CurrentCellState
    {
        get { return m_cellState; }
    }

    /// <summary>
    /// The existing sprites for the cells
    /// </summary>
    private Sprite[] m_cellSprites;

    /// <summary>
    /// Constructor, instantiates a new GridNode
    /// </summary>
    /// <param name="oInstance">The physical gameobject of this node</param>
    /// <param name="s">The available sprites for this node (0 dead, 1 alive)</param>
    public GridNode(GameObject oInstance, Sprite[] s)
    {
        m_cellState = CellState.Dead;
        m_objectInstance = oInstance;
        m_cellSprites = s;
    }

    /// <summary>
    /// Gets the CellState of a neighboring node
    /// </summary>
    /// <param name="n">The neighboring node of which to get the CellState</param>
    /// <returns>The CellState of the neighboring node</returns>
    public CellState GetNeighborCellState(NeighborPos n)
    {
        int idx = (int)n;
        if (m_neighborNodes[idx] == null) return CellState.Dead;
        else return m_neighborNodes[idx].CurrentCellState;
    }

    /// <summary>
    /// Adds a neighbor to this node's collection
    /// </summary>
    /// <param name="n">The position of the neighbor relative to this one</param>
    /// <param name="g">The GridNode to add as neighbor</param>
    public void AddNeighbor(NeighborPos n, GridNode g)
    {
        int idx = (int)n;
        if (m_neighborNodes[idx] == null) m_neighborNodes[idx] = g;
        else Debug.LogError("A GridNode already exists for idx " + idx);
    }

    /// <summary>
    /// Sets the cellstate for this node
    /// </summary>
    /// <param name="c">The state to set this node to</param>
    public void SetCellState(CellState c)
    {
        m_cellState = c;
        m_objectInstance.GetComponent<SpriteRenderer>().sprite = m_cellSprites[(int)c];
    }

    /// <summary>
    /// Toggles the cellstate for this node. If it was alive, it will be dead. If it was dead, it will be alive.
    /// </summary>
    public void ToggleCellState()
    {
        if (m_cellState == CellState.Alive) m_cellState = CellState.Dead;
        else m_cellState = CellState.Alive;
        m_objectInstance.GetComponent<SpriteRenderer>().sprite = m_cellSprites[(int)m_cellState];
    }

    /// <summary>
    /// Gets the GridNode instance of a registered neighbor.
    /// </summary>
    /// <param name="n">The position of the neighbor relative to this node</param>
    /// <returns>The GridNode instance of the given neighbor, null if no neighbor is registered.</returns>
    public GridNode GetNeighbor(NeighborPos n)
    {
        return m_neighborNodes[(int)n];
    }
}
