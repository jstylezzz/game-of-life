using UnityEngine;
using System.Collections;

public class CellObject
{
    private GridNode m_gnode;
    public GridNode GetGridNode { get { return m_gnode; } }

    public CellObject(Vector2 pos, GameObject cellPrefab, Sprite[] s)
    {
        m_gnode = new GridNode(GameObject.Instantiate(cellPrefab), s);
        m_gnode.ObjectInstance.transform.position = pos;
    }
}
