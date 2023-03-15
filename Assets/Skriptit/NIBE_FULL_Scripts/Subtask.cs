/******************************************************************************
 * File        : Subtask.cs
 * Version     : 1.0
 * Author      : Miika Puljujärvi (miika.puljujarvi@lapinamk.fi), Petteri Maljamäki (petteri.maljamaki@lapinamk.fi)
 * Copyright   : Lapland University of Applied Sciences
 * Licence     : MIT-Licence
 * 
 * Copyright (c) 2022 Lapland University of Applied Sciences
 *
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 *
 * The above copyright notice and this permission notice shall be included in all
 * copies or substantial portions of the Software.
 *
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
 * SOFTWARE.
 * 
 *****************************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum SwitchState { On, Off };



/// <summary>
/// Base class for Subtasks. 
/// Subtasks are responsible for gameplay logic. 
/// </summary>
/// 
public class Subtask : MonoBehaviour
{

    // --- --- --- Variables --- --- ---

    protected Task parentTask;
    protected GameObject player;
    protected Vector3 playerPosition;
    protected Quaternion playerRotation;
    protected bool isCurrentlyActiveSubtask = false;

    public string subtaskName;
    public Transform playerPosRotRef;
    public Transform uiPosRotRef;
    public List<SubtaskStory> stories = new List<SubtaskStory>();
    public int currentStoryIndex = 0;
    public List<HighlightPingPong> highlightedObjects = new List<HighlightPingPong>();




    // --- --- --- Methods --- --- ---

    // --- Subtask Setup ---

    /// <summary>
    /// Begin the execution of the Subtask by subscribing to Buttons and Events, and getting required references.
    /// </summary>
    /// <param name="_t"></param>
    public void Begin (Task _t)
    {
        Debug.Log("Beginning " + subtaskName);
        parentTask = _t;
        isCurrentlyActiveSubtask = true;
        currentStoryIndex = 0;
        GetReferences();


        // Subscribing to events and buttons used to be here.
        // They have been moved inside AttemptToMovePlayer to avoid user clicking a button repeatedly, thereby causing the following tasks / subtasks to be skipped.

        StartCoroutine(AttemptToMovePlayer(1f));
        OnBegin();
    }

    /// <summary>
    /// Done is called when the last SubtaskStory on the list has been shown and something prompts the switch to a new SubtaskStory.
    /// Button and Event listeners are unsubscribed and parentTask is asked to advance to a new Subtask. 
    /// </summary>
    protected void Done()
    {
        //SceneInfoManager.Current.SetPauseable(false, (name+" Done") );
        OnDone();
        isCurrentlyActiveSubtask = false;
        //TaskUI.current.PlayParticleEffects();
        UnsubscribeToButtons();
        UnsubscribeToEvents();
        parentTask.NextSubtask(this);

    }

    /// <summary>
    /// Get references to required components. 
    /// This should probably be set to virtual and player reference moved to Begin.
    /// </summary>
    protected void GetReferences ()
    {
        player = GameObject.Find("SteamVRObjects");
    }

    /// <summary>
    /// Overrideable method to gain access to the moment of the Subtask's start. 
    /// </summary>
    protected virtual void OnBegin ()
    {

    }

    /// <summary>
    /// Overrideable method to gain access to the moment of the Subtask's end. 
    /// </summary>
    protected virtual void OnDone ()
    {

    }

    /// <summary>
    ///  Overrideable method to gain access to the moment right after moving the player to another position. 
    /// </summary>
    protected virtual void OnAfterMoving ()
    {

    }




    // --- Story (text) ---

    /// <summary>
    /// Show the next SubtaskStory on the list. 
    /// If the previous one was last on the list, call Done instead to indicate the end of the Subtask.
    /// </summary>
    protected void ShowNextStory ()
    {
        if(null == stories) {
            ClearStory();
        } else {
            
            if (currentStoryIndex < stories.Count) {
                currentStoryIndex++;
                ShowStory(currentStoryIndex);
            } else {
                Done();
            }
        }
    }

    /// <summary>
    /// Show a specific SubtaskStory by using the index on the list.
    /// Usually called from Subtask when states (and therefore info texts) can change in either direction.
    /// </summary>
    /// <param name="_index"></param>
    protected void ShowStory (int _index)
    {
        //Debug.Log("showing text of " + stories[currentStoryIndex].name);
        Language lang = SceneInfoManager.Current.language;
        TaskUI.current.SetHeadlineText(stories[_index].GetHeadline(lang));
        TaskUI.current.SetMainText(stories[_index].GetText(lang));
        TaskUI.current.SetButtonVisibility(stories[_index].storyButtonConfiguration);
        TaskUI.current.SetPanelVisibility(stories[_index].showUI);
        TaskUI.current.SetRectangularButtonText(stories[_index].GetButtonText(lang));

        TaskUI.current.LookAtEnabled = stories[_index].lookAtPlayerEnabled;
    }

    /// <summary>
    /// Show a specific SubtaskStory.
    /// Usually called from Subtask when states (and therefore info texts) can change in either direction.
    /// </summary>
    protected void ShowStory (SubtaskStory _story)
    {
        Language lang = SceneInfoManager.Current.language;
        TaskUI.current.SetHeadlineText(_story.GetHeadline(lang));
        TaskUI.current.SetMainText(_story.GetText(lang));
        TaskUI.current.SetButtonVisibility(_story.storyButtonConfiguration);
        TaskUI.current.SetPanelVisibility(_story.showUI);
        TaskUI.current.SetRectangularButtonText(_story.GetButtonText(lang));

        TaskUI.current.LookAtEnabled = _story.lookAtPlayerEnabled;
    }

    /// <summary>
    /// Set the story information essentially to null.
    /// Unused unless the SubtaskStory list is null. (Delete?)
    /// </summary>
    protected void ClearStory ()
    {
        TaskUI.current.SetHeadlineText("");
        TaskUI.current.SetMainText("");
        TaskUI.current.SetButtonVisibility(SubtaskStoryButton.None);
        TaskUI.current.SetPanelVisibility(false);
    }

    /// <summary>
    /// Refresh the UI by applying the text to it. Called by TaskController when language gets updated.
    /// </summary>
    public void UpdateStoryText ()
    {
        ShowStory(currentStoryIndex);
    }




    // --- Position and Rotation Setup ---

    /// <summary>
    /// Check if player's positional and rotational reference point is the same as is defined in the Subtask. 
    /// If they are different, start the process of moving the player to a new location and rotation. 
    /// TransitionFader-class is used to fade the view into black during the teleportation.
    /// </summary>
    /// <param name="_duration"></param>
    /// <returns></returns>
    protected IEnumerator AttemptToMovePlayer(float _duration)
    {
        SceneInfoManager.Current.SetPauseable(false, (name + " AttemptToMove - Start"));
        

        if (TaskController.current.CurrentPlayerPosRotRef != playerPosRotRef) {
            TransitionFader.Current.FadeInBlack(_duration);
            yield return new WaitUntil(() => TransitionFader.Current.fadeInProgress == false);
            SetPlayerPosAndRot();
            TransitionFader.Current.FadeOutBlack(_duration);
        }
        SubscribeToButtons();
        SubscribeToEvents();
        SetUIPosAndRot();
        Debug.Log("CurrentStoryIndex=" + currentStoryIndex);
        ShowStory(currentStoryIndex);

        SceneInfoManager.Current.SetPauseable(true, (name + " AttemptToMove - End"));
        HighlightInteractableObjects();
        OnAfterMoving();
    }

    /// <summary>
    /// Teleport the player to the reference position and set the initial rotation.
    /// </summary>
    protected void SetPlayerPosAndRot()
    {
        Debug.Log("Setting Player position to " + playerPosRotRef.position);
        playerPosition = playerPosRotRef.position;
        playerRotation = playerPosRotRef.rotation;
        player.transform.position = playerPosition;
        player.transform.rotation = playerRotation;
        TaskController.current.CurrentPlayerPosRotRef = playerPosRotRef;
    }

    /// <summary>
    /// Send information to TaskUI and ask it to teleport the UI canvas to the reference position and set it's initial rotation.
    /// </summary>
    protected void SetUIPosAndRot ()
    {
        Vector3 pos = uiPosRotRef == null ? Vector3.zero + Vector3.up * 1.75f : uiPosRotRef.position;
        Quaternion rot = uiPosRotRef == null ? Quaternion.identity : uiPosRotRef.rotation;
        TaskUI.current.LookAtEnabled = stories[currentStoryIndex].lookAtPlayerEnabled;
        TaskUI.current.SetUIPositionAndRotation(pos,rot);
    }




    // --- Button Handling ---

    /// <summary>
    /// Subscribe to buttons of the TaskUI.
    /// </summary>
    protected void SubscribeToButtons ()
    {
        DebugUIController.OnAClicked += ShowNextStory;
        DebugUIController.OnCClicked += Done;

        TaskUI.OnArrowClick += ShowNextStory;
        TaskUI.OnRectangularClick += Done;
    }

    /// <summary>
    /// Remove the listeners of TaskUI's buttons. 
    /// </summary>
    protected void UnsubscribeToButtons ()
    {
        DebugUIController.OnAClicked -= ShowNextStory;
        DebugUIController.OnCClicked -= Done;

        TaskUI.OnArrowClick -= ShowNextStory;
        TaskUI.OnRectangularClick -= Done;
    }

    /// <summary>
    /// If the user leaves before completing the task, button and event listeners have to be unsubscribed without running through Done.
    /// </summary>
    public void ForceUnsubscribeToAll ()
    {
        UnsubscribeToButtons();
        UnsubscribeToEvents();
    }
    /// <summary>
    /// Overrideable method to allow individual defining of listeners for each Subtask.
    /// </summary>
    protected virtual void SubscribeToEvents()
    {

    }

    /// <summary>
    /// Overrideable method to allow individual defining of listeners for each Subtask.
    /// </summary>
    protected virtual void UnsubscribeToEvents()
    {

    }




    // --- Object Highlighting ---

    /// <summary>
    /// Show the user which objects can be interacted with. 
    /// </summary>
    protected void HighlightInteractableObjects ()
    {
        if(highlightedObjects.Count > 0) {
            foreach(HighlightPingPong hpp in highlightedObjects) {
                hpp.Play();
            }
        }
    }
}
