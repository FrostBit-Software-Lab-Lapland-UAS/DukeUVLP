/******************************************************************************
 * File        : TaskController.cs
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

using System.Collections.Generic;
using UnityEngine;


public enum SelectedTaskList {
    Full,
    Pressure,
    Filter,
    Messy
}

public class TaskController : MonoBehaviour
{

    // --- --- --- Variables --- --- ---

    public static TaskController current;
    //public Language language;
    [HideInInspector] public SelectedTaskList selectedTaskList;


    [HideInInspector] public List<Task> tasks = new List<Task>();
    public List<Task> allTasks = new List<Task>();
    public List<Task> pressureTasks = new List<Task>();
    public List<Task> filterTasks = new List<Task>();
    public List<Task> messyTasks = new List<Task>();
    [HideInInspector] public Task currentTask;
    [SerializeField] private Transform currentPlayerPosRotRef;
    public Transform CurrentPlayerPosRotRef { get { return currentPlayerPosRotRef; } set { currentPlayerPosRotRef = value; } }

    public int currentTaskIndex = 0;




    // --- --- --- Methods --- --- ---

    // --- MonoBehaviour ---

    /// <summary>
    /// Check the presence of SceneInfoManager and spawn a new one if there are none.
    /// Begin the first Task on the selected list.
    /// </summary>
    void Start()
    {
        current = this;
        currentTaskIndex = 0;
        if (null == SceneInfoManager.Current) {
            Debug.Log("Instantiating a new SceneInfoManager!");
            Instantiate(Resources.Load("SceneInfoManager", typeof(GameObject)) as GameObject);
        }

        SetTasks();
        currentTask = tasks[0];
        tasks[0].BeginTask(this);
        
    }

    private void OnEnable ()
    {
        SceneInfoManager.OnLangChange += LanguageChanged;
    }
    private void OnDisable ()
    {
        SceneInfoManager.OnLangChange -= LanguageChanged;
    }




    // --- TaskController ---

    /// <summary>
    /// Progress to the next Task on the list. Return to Main Menu if there are no more Tasks (regardless of what the last Task's SubtaskStory's Retangular Button says).
    /// </summary>
    /// <param name="_task"></param>
    public void NextTask(Task _task)
    {
        currentTaskIndex++;
        
        int taskindex = tasks.IndexOf(_task);
        if (taskindex < tasks.Count - 1) {
            currentTask = tasks[taskindex + 1];
            Debug.Log("Current Task Index = " + currentTaskIndex + ". Attempting to start Task named " + currentTask.taskName + ".");
            tasks[taskindex + 1].BeginTask(this);
        } else {
            if(SceneInfoManager.Current != null) {
                SceneInfoManager.Current.GoToScene(0);
            }
        }
    }  

    private void LanguageChanged (Language _lang)
    {
        if(currentTask != null) {
            if (currentTask.currentSubtask != null) {
                currentTask.currentSubtask.UpdateStoryText();
            }
        }
    }

    /// <summary>
    /// Select a list of Tasks the user must complete in this scene.
    /// </summary>
    public void SetTasks ()
    {
        if(SceneInfoManager.Current != null) {
            selectedTaskList = SceneInfoManager.Current.nibeFullSelectedTask;
        }

        switch (selectedTaskList) {

            case SelectedTaskList.Pressure:
                tasks = new List<Task>(pressureTasks);
                break;

            case SelectedTaskList.Filter:
                tasks = new List<Task>(filterTasks);
                break;


            case SelectedTaskList.Messy:
                tasks = new List<Task>(messyTasks);
                break;

            case SelectedTaskList.Full:
            default:
                tasks = new List<Task>(allTasks);
                break;
        }
    }



}
