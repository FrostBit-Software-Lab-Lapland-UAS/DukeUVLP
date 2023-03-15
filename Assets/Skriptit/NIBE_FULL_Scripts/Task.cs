/******************************************************************************
 * File        : Task.cs
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


/// <summary>
/// Task is a container for Subtask-instances. Tasks are driven by TaskController. 
/// </summary>
public class Task : MonoBehaviour
{

    // --- --- --- Variables --- --- ---

    TaskController taskController;
    public string taskName;
    public List<Subtask> subtasks = new List<Subtask>();
    [HideInInspector] public Subtask currentSubtask;




    // --- --- --- Methods --- --- ---


    /// <summary>
    /// Begin the Task by choosing the first Subtask on the list and starting it.
    /// </summary>
    /// <param name="_taskController"></param>
    public void BeginTask (TaskController _taskController)
    {
        taskController = _taskController;
        if (subtasks.Count == 0) {
            Debug.Log(taskName + " has zero subtasks!");
            taskController.NextTask(this);
        } else {
            StartSubtask(subtasks[0]);
        }      
    }

    /// <summary>
    /// Receive information from previous Subtask's completion and start the next one one the list.
    /// </summary>
    /// <param name="_st"></param>
    public void NextSubtask (Subtask _st)
    {
        int index = subtasks.IndexOf(_st);
        if (index < subtasks.Count-1)  {
            StartSubtask(subtasks[index + 1]);
        }  else {
            taskController.NextTask(this);
        }
    }

    /// <summary>
    /// Start a subtask by calling it's Begin-function.
    /// </summary>
    /// <param name="_st"></param>
    public void StartSubtask(Subtask _st)
    {
        currentSubtask = _st;
        _st.Begin(this);
    }
}
