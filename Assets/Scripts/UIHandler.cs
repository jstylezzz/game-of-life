/*
 * Copyright (c) Jari Senhorst. All rights reserved.  
 * 
 * Licensed under the MIT License. See LICENSE file in the project root for full license information.  
 * 
 * 
 * This class handles all UI related things in the game.
 * 
 */

using UnityEngine;
using UnityEngine.UI;

public class UIHandler : MonoBehaviour
{
    [SerializeField]
    private Text m_statusText; //The text element on the UI containing the status info

    /// <summary>
    /// Updates the loading status in the UI
    /// </summary>
    /// <param name="current">How many items have been loaded</param>
    /// <param name="max">How many items have to be loaded</param>
    /// <param name="timeleft">Amount of time needed to complete loading</param>
    public void UpdateLoadingStatus(int current, int max, float timeleft)
    {
        m_statusText.text = "Game state: loading (" + current + " of " + max + ")\nTime left: " + Mathf.RoundToInt(timeleft) + " seconds";
    }

    /// <summary>
    /// Function that handles UI when the game pauses
    /// </summary>
    public void OnGamePause()
    {
        m_statusText.text = "Game state: editor mode";
    }

    /// <summary>
    /// Function that handles UI when the game resumes
    /// </summary>
    public void OnGamePlay()
    {
        m_statusText.text = "Game state: simulating";
    }
	
}
