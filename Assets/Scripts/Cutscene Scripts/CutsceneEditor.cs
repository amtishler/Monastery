using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

// Cutscene Config
public class CutsceneConfig {
    public bool eventExpanded = true;
    public bool textBoxExpanded = true;
}

// Cutscene Editor
[CustomEditor(typeof(Cutscene))]
public class CutsceneEditor : Editor {

    private Cutscene cutscene;
    private List<CutsceneConfig> cutsceneConfigs = new List<CutsceneConfig>();
    private GUIStyle eventStyle = new GUIStyle(EditorStyles.foldout);
    private GUIStyle textBoxStyle = new GUIStyle(EditorStyles.foldout);
    private GUIStyle headerStyle = new GUIStyle(EditorStyles.label);


    // Updates our GUI with a fresh, custom interface
    public override void OnInspectorGUI() {
        EditorStyles.textField.wordWrap=true;
        SetStyle();

        cutscene = (Cutscene)target;
        SerializedObject serializedObject = new SerializedObject(cutscene);
        serializedObject.Update();
        SerializedProperty events = serializedObject.FindProperty("events");

        EditorGUILayout.LabelField("Cutscene Settings", headerStyle);
        EditorGUILayout.Separator();
        DrawSettings(cutscene);
        EditorGUILayout.Separator();
        EditorGUILayout.LabelField("Cutscene Events", headerStyle);
        EditorGUILayout.Separator();
        DrawEvents(events);

        // Buttons
        EditorGUILayout.BeginHorizontal();
        if(GUILayout.Button("Add Dialogue")) {
            CutsceneEvent newEvent = new CutsceneEvent();
            newEvent.type = "Dialogue";
            cutscene.events.Add(newEvent);
            cutsceneConfigs.Add(new CutsceneConfig());
        }
        if(GUILayout.Button("Add Camera Move")) {
            CutsceneEvent newEvent = new CutsceneEvent();
            newEvent.type = "Camera Move";
            cutscene.events.Add(newEvent);
            cutsceneConfigs.Add(new CutsceneConfig());
        }
        EditorGUILayout.EndHorizontal();

        serializedObject.ApplyModifiedProperties();
    }


    // Draws cutscene's settings
    private void DrawSettings(Cutscene cutscene) {
        EditorGUILayout.BeginVertical();
            cutscene.textScrollSpeed = EditorGUILayout.FloatField("Text Scroll Speed", cutscene.textScrollSpeed);
            cutscene.blackBarHeight = EditorGUILayout.FloatField("Black Bar Height", cutscene.blackBarHeight);
            cutscene.originPoint = EditorGUILayout.Vector3Field("Origin Point", cutscene.originPoint);
            cutscene.triggerRadius = EditorGUILayout.FloatField("triggerRadius", cutscene.triggerRadius);
        EditorGUILayout.EndVertical();
    }


    // Draws cutscene's events
    private void DrawEvents(SerializedProperty events) {

        int lengthToBeat = cutscene.events.ToArray().Length;
        while (cutsceneConfigs.ToArray().Length < lengthToBeat) cutsceneConfigs.Add(new CutsceneConfig());

        for(int i=0; i<cutscene.events.ToArray().Length; i++) {

            if (cutscene.events.ToArray().Length <= i) break;
            CutsceneEvent cutsceneEvent = cutscene.events[i];
            if (cutsceneEvent == null) {
                cutscene.events.RemoveAt(i);
                break;
            }

            if (cutsceneEvent.type == "Dialogue") DrawDialogue(cutsceneEvent, cutsceneConfigs[i], i);
            else if (cutsceneEvent.type == "Camera Move") DrawCameraMove(cutsceneEvent, cutsceneConfigs[i], i);

            EditorGUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();
                if(GUILayout.Button("Delete Event",EditorStyles.miniButtonRight)) {
                    cutscene.events.RemoveAt(i);
                    cutsceneConfigs.RemoveAt(i);
                    break;
                }
            EditorGUILayout.EndHorizontal();
        }    
    }


    // Draws the dialogue boxes for dialogue events
    private void DrawDialogue(CutsceneEvent cutsceneEvent, CutsceneConfig config, int i) {
        config.eventExpanded = EditorGUILayout.Foldout(config.eventExpanded, i.ToString() + ". Dialogue Event", eventStyle);
        if (!config.eventExpanded) return;
        
        EditorGUILayout.BeginVertical("box");
            EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Speaker",GUILayout.Width(100));
                cutsceneEvent.speaker = (GameObject)EditorGUILayout.ObjectField(cutsceneEvent.speaker, typeof(GameObject),true);
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Text Box Side",GUILayout.Width(100));
                cutsceneEvent.Rotation = EditorGUILayout.Popup(cutsceneEvent.Rotation, new string[] {"Top Left", "Top Right", "Bottom Left", "Bottom Right"});
            EditorGUILayout.EndHorizontal();
            config.textBoxExpanded = EditorGUILayout.Foldout(config.textBoxExpanded, "Text Boxes", textBoxStyle);
            if (config.textBoxExpanded) {
                EditorGUILayout.BeginVertical("BOX");
                    for(int j=0; j<cutsceneEvent.textBoxes.ToArray().Length; j++) {
                        string label = "Box " + j.ToString();
                        EditorGUILayout.BeginHorizontal();
                            EditorGUILayout.LabelField(label,GUILayout.Width(40));
                            cutsceneEvent.textBoxes[j] = EditorGUILayout.TextArea(cutsceneEvent.textBoxes[j],GUILayout.Height(40));
                            if(GUILayout.Button("Delete Box",EditorStyles.miniButtonRight,GUILayout.Width(100))) {
                                cutsceneEvent.textBoxes.RemoveAt(j);
                                break;
                            }
                        EditorGUILayout.EndHorizontal();
                    }
                    EditorGUILayout.BeginHorizontal();
                        GUILayout.FlexibleSpace();
                        if(GUILayout.Button("Add Box",EditorStyles.miniButtonRight,GUILayout.Width(100))) cutsceneEvent.textBoxes.Add("");
                    EditorGUILayout.EndHorizontal();
                EditorGUILayout.EndVertical();
            }
        EditorGUILayout.EndVertical();
    }


    // Draws camera move settings for camera move events
    private void DrawCameraMove(CutsceneEvent cutsceneEvent, CutsceneConfig config, int i) {
        config.eventExpanded = EditorGUILayout.Foldout(config.eventExpanded, i.ToString() + ". Camera Move Event", eventStyle);
        if (!config.eventExpanded) return;
        EditorGUILayout.BeginVertical("box");
            EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Transition Time",GUILayout.Width(100));
                cutsceneEvent.transitionTime = EditorGUILayout.FloatField(cutsceneEvent.transitionTime);
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Camera Size",GUILayout.Width(100));
                cutsceneEvent.cameraSize = EditorGUILayout.FloatField(cutsceneEvent.cameraSize);
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.BeginHorizontal();
                cutsceneEvent.anchorPoint = EditorGUILayout.Vector2Field("Anchor Point", cutsceneEvent.anchorPoint);
            EditorGUILayout.EndHorizontal();
        EditorGUILayout.EndVertical();
    }


    private void SetStyle() {
        headerStyle.fontStyle = FontStyle.Bold;
        headerStyle.fontSize = 14;
        // Color myStyleColor3 = Color.red;
        // headerStyle.normal.textColor = myStyleColor3;
        // headerStyle.onNormal.textColor = myStyleColor3;
        // headerStyle.hover.textColor = myStyleColor3;
        // headerStyle.onHover.textColor = myStyleColor3;
        // headerStyle.focused.textColor = myStyleColor3;
        // headerStyle.onFocused.textColor = myStyleColor3;
        // headerStyle.active.textColor = myStyleColor3;
        // headerStyle.onActive.textColor = myStyleColor3;

        eventStyle.fontStyle = FontStyle.Bold;
        eventStyle.fontSize = 13;
        // Color myStyleColor1 = Color.cyan;
        // eventStyle.normal.textColor = myStyleColor1;
        // eventStyle.onNormal.textColor = myStyleColor1;
        // eventStyle.hover.textColor = myStyleColor1;
        // eventStyle.onHover.textColor = myStyleColor1;
        // eventStyle.focused.textColor = myStyleColor1;
        // eventStyle.onFocused.textColor = myStyleColor1;
        // eventStyle.active.textColor = myStyleColor1;
        // eventStyle.onActive.textColor = myStyleColor1;

        // textBoxStyle.fontStyle = FontStyle.Bold;
        // textBoxStyle.fontSize = 12;
        // Color myStyleColor2 = Color.white;
        // textBoxStyle.normal.textColor = myStyleColor2;
        // textBoxStyle.onNormal.textColor = myStyleColor2;
        // textBoxStyle.hover.textColor = myStyleColor2;
        // textBoxStyle.onHover.textColor = myStyleColor2;
        // textBoxStyle.focused.textColor = myStyleColor2;
        // textBoxStyle.onFocused.textColor = myStyleColor2;
        // textBoxStyle.active.textColor = myStyleColor2;
        // textBoxStyle.onActive.textColor = myStyleColor2;
    }
}
