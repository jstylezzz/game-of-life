/*
 * Copyright (c) Jari Senhorst. All rights reserved.  
 * 
 * Licensed under the MIT License. See LICENSE file in the project root for full license information.  
 * 
 * 
 * This class is the essential heart of the game. It handles the grid and input made to it. The game simulation is also handled here.
 * 
 */

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class GridManager : MonoBehaviour
{
    [SerializeField]
    private int m_gridSize; //The squared size of the grid

    [SerializeField]
    private GameObject m_cellPrefab; //The prefab for cells

    [SerializeField]
    private Sprite[] m_cellSprites; //An array of the dead and alive cell sprites (0 dead, 1 alive)

    [SerializeField]
    private float m_simDelay; //The delay in seconds between each simulation iteration

    [SerializeField]
    private float m_zoomSpeed; //The zoom speed for the camera

    [SerializeField]
    private float m_camSpeed; //The movement speed for the camera

    private UIHandler m_uih; //UI handler instance
    private bool m_gameReady = false; //Bool for if the game is ready to go
    private List<GridNode> m_cellList = new List<GridNode>(); //A list of all the grid nodes
    private bool m_gameRunning = false; //Bool for whether the simulation is running or not
    private int m_itemsLoaded; //Int for keeping track of how many grid cells have been loaded during first generation
    private int m_secsLeft; //Int for the amount of seconds left before the generator is finished
    private int m_secsPassed = 0; //Int for the amount of seconds passed since grid generation started

	/// <summary>
    /// Script entry point
    /// </summary>
	private void Start () 
	{
        m_uih = GameObject.FindObjectOfType<UIHandler>();
        StartCoroutine(CalculateLoadTime());
        StartCoroutine(GenerateGrid(0));
	}

    /// <summary>
    /// A separate routine for calculating the time needed to complete grid generation
    /// </summary>
    private IEnumerator CalculateLoadTime()
    {

        if (m_itemsLoaded > 0)
        {
            m_secsPassed++;
            int total = m_gridSize * m_gridSize;
            int left = total - m_itemsLoaded;
            m_secsLeft = left / (m_itemsLoaded / m_secsPassed);
          
        }
        yield return new WaitForSeconds(1);
        if (m_gameReady == false) StartCoroutine(CalculateLoadTime());
        
    }

    /// <summary>
    /// The grid generator in a separate routine
    /// </summary>
    /// <param name="progress">The progress from which to start the generating process</param>
    private IEnumerator GenerateGrid(int progress)
    {
        int tprog = 0;
        while(tprog <= 10 && progress < (m_gridSize * m_gridSize))
        {
            Vector2 pos = Vector2.zero;

            if (progress >= m_gridSize) //When second grid row is reached
            {
                float cy = progress / m_gridSize;
                float y = (float)Math.Round(cy, 0, MidpointRounding.AwayFromZero) * 0.01f;
                float x = ((progress - (progress / m_gridSize)) - ((m_gridSize-1)) * y / 0.01f) * 0.01f;
                pos = new Vector2(x, y);
            }
            else
            {
                pos = new Vector2(progress * 0.01f, 0f);
            }

            m_cellList.Add(new CellObject(pos, m_cellPrefab, m_cellSprites).GetGridNode);
            tprog++;
            progress++;
           
            m_itemsLoaded++;
            m_uih.UpdateLoadingStatus(progress, m_gridSize * m_gridSize, m_secsLeft);
        }
        yield return new WaitForSeconds(0.005f);

        if (progress != (m_gridSize * m_gridSize)) StartCoroutine(GenerateGrid(progress));
        else
        {
            CenterCamera();
            ResolveNeighbors();
        }
    }

    /// <summary>
    /// Resolves the neighboring cells for each individual cell
    /// </summary>
    private void ResolveNeighbors()
    {

        for (int x = 0; x < m_gridSize; x++)
        {
            for (int y = 0; y < m_gridSize; y++)
            {
                //Current array index of the cell
                int cidx = x + (m_gridSize * y);

                //Variables for our cell array indexes
                int topleft = 0;
                int top = 0;
                int topright = 0;

                int left = 0;
                int right = 0;

                int bottomleft = 0;
                int bottom = 0;
                int bottomright = 0;

                //First cell of a row
                if (x == 0)
                {
                    //If we're not on the top row -- Top
                    if(y != m_gridSize-1) top = cidx + m_gridSize;

                    //If we're not on the toprow -- TopRight
                    if (y != m_gridSize-1) topright = cidx + m_gridSize + 1;

                    //Right
                    right = cidx + 1;
             
                    //If we're not on the bottom row -- BottomRight
                    if(y != 0) bottomright = cidx - m_gridSize + 1;

                    //If we're not on the bottom row -- Bottom
                    if (y != 0) bottom = cidx - m_gridSize;
                }
                else if (x == m_gridSize-1) //Last cell of a row
                {
                    //If we're not on the top row -- Top
                    if (y != m_gridSize - 1) top = cidx + m_gridSize;

                    //If we're not on the toprow -- TopLeft
                    if (y != m_gridSize - 1) topleft = cidx + m_gridSize - 1;

                    //Left
                    left = cidx - 1;

                    //If we're not on the bottom row -- BottomLeft
                    if (y != 0) bottomleft = cidx - m_gridSize - 1;

                    //If we're not on the bottom row -- Bottom
                    if (y != 0) bottom = cidx - m_gridSize;
                }
                else //All cells inbetween the above
                {
                    //If we're not on the top row -- Top
                    if (y != m_gridSize - 1) top = cidx + m_gridSize;

                    //If we're not on the right outer edge-- TopRight
                    if (y != m_gridSize - 1) topright = cidx + m_gridSize + 1;

                    //Right
                    right = cidx + 1;

                    //If we're not on the bottom row -- BottomRight
                    if (y != 0) bottomright = cidx - m_gridSize + 1;

                    //If we're not on the bottom row -- Bottom
                    if (y != 0) bottom = cidx - m_gridSize;

                    //If we're not on the top row
                    if (y != m_gridSize - 1) topleft = cidx + m_gridSize - 1;

                    //If we're not on the bottom row
                    if (y != 0) bottomleft = cidx - m_gridSize - 1;

                    //Left
                    left = cidx - 1;
                }

                //Add neighbor cells where needed
                if (top != 0) m_cellList[cidx].AddNeighbor(GridNode.NeighborPos.Top, m_cellList[top]);
                if (topleft != 0) m_cellList[cidx].AddNeighbor(GridNode.NeighborPos.TopLeft, m_cellList[topleft]);
                if (topright != 0) m_cellList[cidx].AddNeighbor(GridNode.NeighborPos.TopRight, m_cellList[topright]);

                if (left != 0) m_cellList[cidx].AddNeighbor(GridNode.NeighborPos.Left, m_cellList[left]);
                if (right != 0) m_cellList[cidx].AddNeighbor(GridNode.NeighborPos.Right, m_cellList[right]);

                if (bottom != 0) m_cellList[cidx].AddNeighbor(GridNode.NeighborPos.Bottom, m_cellList[bottom]);
                if (bottomleft != 0) m_cellList[cidx].AddNeighbor(GridNode.NeighborPos.BottomLeft, m_cellList[bottomleft]);
                if (bottomright != 0) m_cellList[cidx].AddNeighbor(GridNode.NeighborPos.BottomRight, m_cellList[bottomright]);

            }
        }

        m_gameReady = true;
        m_uih.OnGamePause();
    }

    /// <summary>
    /// Centers the camera on the grid
    /// </summary>
    private void CenterCamera()
    {
        int centeridx = Mathf.RoundToInt((m_gridSize * m_gridSize) / 2);
  
        Vector2 p = m_cellList[m_gridSize/2].ObjectInstance.transform.position;
        Camera.main.transform.position = new Vector3(p.x, p.y + ((0.01f * m_gridSize) / 2), -10);
    }
	
	/// <summary>
    /// Script main loop
    /// </summary>
	private void Update () 
	{
		if(m_gameReady == true)
        {
            if (Input.GetMouseButtonDown(0) && m_gameRunning == false)
            {
                Ray r = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit2D rhit = Physics2D.Raycast(r.origin, r.direction);
                if(rhit)
                {
                    GridNode n = m_cellList.Find(x => x.ObjectInstance.transform.position == rhit.collider.transform.position);

                    if (n.CurrentCellState == GridNode.CellState.Dead) n.SetCellState(GridNode.CellState.Alive);
                    else n.SetCellState(GridNode.CellState.Dead);
                }
            }
            if(Input.GetKeyDown(KeyCode.Space))
            {
                if (m_gameRunning)
                {
                    m_gameRunning = false;
                    m_uih.OnGamePause();
                }
                else
                {
                    m_gameRunning = true;
                    m_uih.OnGamePlay();
                    StartCoroutine(RunSimulation());
                }
            }

            float dscroll = Input.GetAxis("Mouse ScrollWheel");
            if(dscroll > 0 || dscroll < 0)
            {
                float csize = Mathf.Clamp(Camera.main.orthographicSize + m_zoomSpeed * (dscroll * -1), 0.01f, 100);
                Camera.main.orthographicSize = csize;
            }

            Vector3 cPos = Camera.main.transform.position;
            if (Input.GetKey(KeyCode.W)) Camera.main.transform.position = new Vector3(cPos.x, cPos.y+m_camSpeed, cPos.z);
            else if (Input.GetKey(KeyCode.S)) Camera.main.transform.position = new Vector3(cPos.x, cPos.y - m_camSpeed, cPos.z);

            if (Input.GetKey(KeyCode.A)) Camera.main.transform.position = new Vector3(cPos.x - m_camSpeed, cPos.y, cPos.z);
            else if (Input.GetKey(KeyCode.D)) Camera.main.transform.position = new Vector3(cPos.x + m_camSpeed, cPos.y, cPos.z);
        }
	}

    /// <summary>
    /// A separate routine that executes the simulation iterations
    /// </summary>
    private IEnumerator RunSimulation()
    {
        List<GridNode> nodesToKill = new List<GridNode>();
        List<GridNode> nodesToMake = new List<GridNode>();

        foreach(GridNode n in m_cellList)
        {
            GridNode.CellState[] cellStates = new GridNode.CellState[8];

            cellStates[0] = n.GetNeighborCellState(GridNode.NeighborPos.TopLeft);
            cellStates[1] = n.GetNeighborCellState(GridNode.NeighborPos.Top);
            cellStates[2] = n.GetNeighborCellState(GridNode.NeighborPos.TopRight);

            cellStates[3] = n.GetNeighborCellState(GridNode.NeighborPos.Left);
            cellStates[4] = n.GetNeighborCellState(GridNode.NeighborPos.Right);

            cellStates[5] = n.GetNeighborCellState(GridNode.NeighborPos.BottomRight);
            cellStates[6] = n.GetNeighborCellState(GridNode.NeighborPos.Bottom);
            cellStates[7] = n.GetNeighborCellState(GridNode.NeighborPos.BottomLeft);

            int activeNeighbors = 0;
            for (int i = 0; i < 8; i++) { if (cellStates[i] == GridNode.CellState.Alive) activeNeighbors++; }


            if (activeNeighbors < 2) nodesToKill.Add(n); //Any live cell with fewer than two live neighbours dies, as if caused by underpopulation.
            else if (activeNeighbors > 3) nodesToKill.Add(n); //Any live cell with more than three live neighbours dies, as if by overpopulation.
            else if (activeNeighbors == 3) nodesToMake.Add(n); //Any dead cell with exactly three live neighbours becomes a live cell, as if by reproduction.
        }

        foreach(GridNode ntk in nodesToKill) { ntk.SetCellState(GridNode.CellState.Dead); }
        foreach (GridNode ntm in nodesToMake) { ntm.SetCellState(GridNode.CellState.Alive); }
        yield return new WaitForSeconds(m_simDelay);
        if(m_gameRunning) StartCoroutine(RunSimulation());
    }
}
