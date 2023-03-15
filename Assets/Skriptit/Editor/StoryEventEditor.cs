using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;



[CustomEditor(typeof(StoryEvent))]
public class StoryEventEditor : Editor
{


    public override void OnInspectorGUI ()
    {
        DrawDefaultInspector();

        
        /*
        StoryEvent t = (StoryEvent)target;

        EditorGUILayout.LabelField("Setup Settings", EditorStyles.boldLabel);
        t.id = EditorGUILayout.IntField("ID", t.id);
        t.eventNameFin = EditorGUILayout.TextField("Event name (FIN)", t.eventNameFin);
        t.eventNameEng = EditorGUILayout.TextField("Event name (ENG)", t.eventNameEng);
        t.eventNameSwe = EditorGUILayout.TextField("Event name (SWE)", t.eventNameSwe);
        EditorGUILayout.Space(30f);


        EditorGUILayout.LabelField("Main Panel Settings", EditorStyles.boldLabel);
        t.showMainPanel = EditorGUILayout.Toggle("Show main panel?", t.showMainPanel);
        if(t.showMainPanel) {
            EditorGUILayout.LabelField("Label Text (FIN/ENG/SWE)");
            t.panelTextFin = EditorGUILayout.TextArea(t.panelTextFin);
            t.panelTextEng = EditorGUILayout.TextArea(t.panelTextEng);
            t.panelTextSwe = EditorGUILayout.TextArea(t.panelTextSwe);
            t.panelTextFontSize = EditorGUILayout.IntField("Font Size", t.panelTextFontSize);
        }
        EditorGUILayout.Space(30f);


        EditorGUILayout.LabelField("Arrow Button Settings", EditorStyles.boldLabel);
        t.nextArrowActive = EditorGUILayout.Toggle("Show NEXT Arrow?", t.nextArrowActive);
        if(t.nextArrowActive) {
            EditorGUILayout.LabelField("Next Arrow Info Text (FIN/ENG/SWE)");
            t.nextArrowInfoTextFin = EditorGUILayout.TextArea(t.nextArrowInfoTextFin, EditorStyles.textArea);
            t.nextArrowInfoTextEng = EditorGUILayout.TextArea(t.nextArrowInfoTextEng, EditorStyles.textArea);
            t.nextArrowInfoTextSwe = EditorGUILayout.TextArea(t.nextArrowInfoTextSwe, EditorStyles.textArea);
            t.arrowInfoTextFontSize = EditorGUILayout.IntField("Font Size", t.arrowInfoTextFontSize);
            EditorGUILayout.Space(10f);
        }
        t.prevArrowActive = EditorGUILayout.Toggle("Show PREV Arrow?", t.prevArrowActive);
        if (t.prevArrowActive) {
            EditorGUILayout.LabelField("Prev Arrow Info Text (FIN/ENG/SWE)");
            t.prevArrowInfoTextFin = EditorGUILayout.TextArea(t.prevArrowInfoTextFin, EditorStyles.textArea);
            t.prevArrowInfoTextEng = EditorGUILayout.TextArea(t.prevArrowInfoTextEng, EditorStyles.textArea);
            t.prevArrowInfoTextSwe = EditorGUILayout.TextArea(t.prevArrowInfoTextSwe, EditorStyles.textArea);
            t.arrowInfoTextFontSize = EditorGUILayout.IntField("Font Size", t.arrowInfoTextFontSize);
        }
        EditorGUILayout.Space(30f);


        EditorGUILayout.LabelField("Rectangular Button Settings", EditorStyles.boldLabel);
        t.rectangularButtonActive = EditorGUILayout.Toggle("Show rectangular button?", t.rectangularButtonActive);
        if (t.rectangularButtonActive) {
            EditorGUILayout.LabelField("Rectangular Arrow Info Text (FIN/ENG/SWE)");
            t.rectangularButtonTextFin = EditorGUILayout.TextArea(t.rectangularButtonTextFin, EditorStyles.textArea);
            t.rectangularButtonTextEng = EditorGUILayout.TextArea(t.rectangularButtonTextEng, EditorStyles.textArea);
            t.rectangularButtonTextSwe = EditorGUILayout.TextArea(t.rectangularButtonTextSwe, EditorStyles.textArea);
            t.rectangularButtonFontSize = EditorGUILayout.IntField("Font Size", t.rectangularButtonFontSize);
        }
        EditorGUILayout.Space(30f);


        EditorGUILayout.LabelField("Animations and Effects", EditorStyles.boldLabel);
        EditorGUILayout.LabelField("Animations");

        for (int i = 0; i < t.testingList.Count; i++)
        {
            t.testingList[i] = EditorGUILayout.TextField(t.testingList[i]);
        }

        */
    }
}
