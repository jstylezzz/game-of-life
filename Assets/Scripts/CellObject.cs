/*
 * Copyright (c) Jari Senhorst. All rights reserved.  
 * 
 * Licensed under the MIT License. See LICENSE file in the project root for full license information.  
 * 
 * 
 * This class handles the spawning of the cell prefabs, and passes important data to the create gridnode
 * 
 */

using UnityEngine;
using System.Collections;

public class CellObject
{
    private GridNode m_gnode; //Our cell's GridNode
    public GridNode GetGridNode { get { return m_gnode; } } //Property for our cell's GridNode

    /// <summary>
    /// Constructor for CellObject
    /// </summary>
    /// <param name="pos">Position in the grid at which to place the cell</param>
    /// <param name="cellPrefab">The prefab to spawn in</param>
    /// <param name="s">The sprite to use for the prefab</param>
    public CellObject(Vector2 pos, GameObject cellPrefab, Sprite[] s)
    {
        m_gnode = new GridNode(GameObject.Instantiate(cellPrefab), s);
        m_gnode.ObjectInstance.transform.position = pos;
    }
}
