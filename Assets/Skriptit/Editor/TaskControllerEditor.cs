using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.EditorTools;
using UnityEditorInternal;


/// <summary>
/// EditorScript for TaskController. 
/// This was meant to change the inspector to make handling Tasks and Subtasks more manageable.
/// Currently this script is unused and most of the code in OnInspectorGUI() is commented out.
/// </summary>
[CustomEditor(typeof(TaskController))]
public class TaskControllerEditor : Editor
{


    bool showDefaultInspector = true;




    private GUILayoutOption[] objectFieldOptions = new GUILayoutOption[] {
        GUILayout.Width(450),
        GUILayout.Height(20)
    };
    private GUILayoutOption[] taskButtonOptions = new GUILayoutOption[] {
        GUILayout.Width(70),
        GUILayout.Height(20)
    };
    private GUILayoutOption[] subtaskButtonOptions = new GUILayoutOption[] {
        GUILayout.Width(70),
        GUILayout.Height(20)
    };
    private GUILayoutOption[] storyButtonOptions = new GUILayoutOption[] {
        GUILayout.Width(70),
        GUILayout.Height(20)
    };
    private GUILayoutOption[] createButtonOptions = new GUILayoutOption[] {
        GUILayout.Width(120),
        GUILayout.Height(20)
    };
    private GUIStyle taskButtonStyle;
    private GUIStyle subtaskButtonStyle;
    private GUIStyle storyButtonStyle;

    private void OnEnable ()
    {

    }


    private void SetGUIStyles ()
    {
        //int buttonPadding = 5;
        taskButtonStyle = new GUIStyle(GUI.skin.button);
        subtaskButtonStyle = new GUIStyle(GUI.skin.button);
        storyButtonStyle = new GUIStyle(GUI.skin.button);

        Color taskColor = new Color(0.5f, 0f, 1f);
        taskButtonStyle.normal.textColor = Color.white;
        taskButtonStyle.fontSize = 14;
        taskButtonStyle.alignment = TextAnchor.MiddleCenter;
        taskButtonStyle.normal.background = SetTextureColor(60, 10, taskColor);


        Color subtaskColor = new Color(0f, 0.5f, 0.4f);
        subtaskButtonStyle.normal.textColor = Color.white;
        subtaskButtonStyle.fontSize = 14;
        subtaskButtonStyle.alignment = TextAnchor.MiddleCenter;
        subtaskButtonStyle.normal.background = SetTextureColor(60, 10, subtaskColor);



        Color storyColor = new Color(1f, 0.5f, 0f);
        storyButtonStyle.normal.textColor = Color.white;
        storyButtonStyle.fontSize = 14;
        storyButtonStyle.alignment = TextAnchor.MiddleCenter;
        storyButtonStyle.normal.background = SetTextureColor(60, 10, storyColor);

    }



    Texture2D SetTextureColor (int _x, int _y, Color _col)
    {
        Texture2D newTex = new Texture2D(_x,_y);
        for(int x = 0; x < _x; x++) {
            for(int y = 0; y < _y; y++) {
                newTex.SetPixel(x, y, _col);
            }
        }
        newTex.Apply(false, false);
        return newTex;
    }

    



    public override void OnInspectorGUI()
    {
        TaskController tc = (TaskController)target;

        if(GUILayout.Button("Toggle Default Inspector " + (showDefaultInspector ? "OFF":"ON"))) {
            showDefaultInspector = !showDefaultInspector;
            taskButtonStyle = new GUIStyle(GUI.skin.button);
            subtaskButtonStyle = new GUIStyle(GUI.skin.button);
            storyButtonStyle = new GUIStyle(GUI.skin.button);
            SetGUIStyles(); 
        }
        if(showDefaultInspector) {
            base.DrawDefaultInspector();
            return;
        }

        // COMMENT STARTS HERE
        /*

        

        EditorGUILayout.Space(10f);
        EditorGUILayout.HelpBox("Tasks are executed in the order listed below. Each task has a number of subtasks that are executed in order.", MessageType.Info);
        EditorGUILayout.Space(20f);

        for (int i = 0; i < tc.tasks.Count; i++) {
            EditorGUILayout.BeginHorizontal();
            Task task = tc.tasks[i];
            tc.tasks[i] = (Task)EditorGUILayout.ObjectField(tc.tasks[i], typeof(Task), true, objectFieldOptions);
            if (AddButtons(tc.tasks, i, taskButtonStyle, taskButtonOptions)) {
                return;
            }
            
            EditorGUILayout.EndHorizontal();   

            EditorGUI.indentLevel++;
            for (int j = 0; j < task.subtasks.Count; j++) {
                EditorGUILayout.BeginHorizontal();       
                Subtask subtask = task.subtasks[j];
                subtask = (Subtask)EditorGUILayout.ObjectField(subtask, typeof(Subtask), true, objectFieldOptions);
                if (AddButtons(task.subtasks, j, subtaskButtonStyle, subtaskButtonOptions)) {
                    return;
                }
                
                EditorGUILayout.EndHorizontal();

                EditorGUI.indentLevel++;
                for (int k = 0; k < subtask.stories.Count; k++) {
                    EditorGUILayout.BeginHorizontal();
                    subtask.stories[k] = (SubtaskStory)EditorGUILayout.ObjectField(subtask.stories[k], typeof(SubtaskStory), true, objectFieldOptions);
                    if (AddButtons(task.subtasks, k, storyButtonStyle, storyButtonOptions)) {
                        return;
                    }                  
                    EditorGUILayout.EndHorizontal();
                }
                if (GUILayout.Button("NEW", storyButtonStyle, storyButtonOptions)) { subtask.stories.Add(new SubtaskStory()); }
                EditorGUI.indentLevel--;
                EditorGUILayout.Space(5f);
            }
            if (GUILayout.Button("NEW", subtaskButtonStyle, subtaskButtonOptions)) { task.subtasks.Add(new Subtask()); }
            EditorGUI.indentLevel--;
            EditorGUILayout.Space(10f);
        }
        if (GUILayout.Button("NEW", taskButtonStyle, taskButtonOptions)) { tc.tasks.Add(new Task()); }

        */
        // COMMENT ENDS HERE

    }


    void ReorderList<T> (List<T> _list, int _indexToMove, bool _up) 
    {
        int newIndex = _up ? _indexToMove - 1 : _indexToMove + 1;
        newIndex = Mathf.Clamp(newIndex, 0, _list.Count - 1);
        List<T> tempList = _list;
        T obj = _list[_indexToMove];
        tempList.RemoveAt(_indexToMove);
        tempList.Insert(newIndex, obj);
    }



    bool AddButtons<T> (List<T> _listToEdit, int _indexToEdit, GUIStyle _style, GUILayoutOption[] _opt)
    {
        if (GUILayout.Button("UP", _style, _opt)) {
            ReorderList(_listToEdit, _indexToEdit, true);
        } else if (GUILayout.Button("DOWN", _style, _opt)) {
            ReorderList(_listToEdit, _indexToEdit, false);
        } else if (GUILayout.Button("REMOVE", _style, _opt)) {
            _listToEdit.RemoveAt(_indexToEdit);
            return true;
        }
        return false;
    }



    void OnInspectorUpdate()
    {
        Repaint();
    }



}
