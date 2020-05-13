using UnityEngine;
using UnityEditor;
using System.IO;
using System;

[CustomEditor(typeof(RoomGenerator))]
public class RoomGeneratorEditor : Editor
{
    RoomGenerator gen;

    public static void DrawUILine(Color color, int thickness = 2, int padding = 8)
    {
        Rect r = EditorGUILayout.GetControlRect(GUILayout.Height(padding + thickness));
        r.height = thickness;
        r.y += padding;
        EditorGUI.DrawRect(r, color);
    }

    private void saveTilemapAsAsset(RoomGenerator gen, string filename)
    {
        GameObject thingy = gen.GetGameObject();
        thingy.name = filename;
        if (File.Exists(Application.dataPath + "/Resources/Rooms/" + filename + ".prefab"))
        {
            bool isOk = EditorUtility.DisplayDialog("Warning", "The file already exsist in the folder. Do you want to overwrite the file?", "YuRP", "PLEASE NO");
            if (isOk)
            {
                saveFile(thingy, filename);
            }
        }
        else
        {
            saveFile(thingy, filename);
        }


        DestroyImmediate(thingy);
    }

    private void saveFile(GameObject prefabToSave, string filename)
    {
        bool saved = PrefabUtility.SaveAsPrefabAsset(prefabToSave, "Assets/Resources/Rooms/" + filename + ".prefab");
        if (saved)
        {
            EditorUtility.DisplayDialog("Niceuh", "Saved the prefab! Pretty goood.", "Thanks");
        }
        else
        {
            EditorUtility.DisplayDialog("Wait....", "Something went wrong, dont ask me what.", "Thank you?");
        }
    }

    GUIStyle Header;
    GUIStyle SubHeader;
    GUIStyle SubSubHeader;
    GUIStyle error;
    private void InitializeCss()
    {
        Header = new GUIStyle();
        Header.fontSize = 15;
        Header.fontStyle = FontStyle.Bold;
        Header.alignment = TextAnchor.MiddleCenter;

        SubHeader = new GUIStyle();
        SubHeader.fontSize = 15;

        SubSubHeader = new GUIStyle();
        SubSubHeader.fontSize = 14;

        error = new GUIStyle();
        error.alignment = TextAnchor.UpperRight;
        error.normal.textColor = Color.red;
    }


    public override void OnInspectorGUI()
    {
        InitializeCss();

        gen = (RoomGenerator)target;

        GUILayout.Space(3f);
        GUILayout.Label("CUSTOM ROOM GENERATION", Header);
        GUILayout.Space(10f);

        if (GUILayout.Button("Clear drawing"))
        {
            DungeonManager.ClearAllTiles();
        }

        GUILayout.Space(10f);

        drawNewSection();
        GUILayout.Space(15f);

        drawEditingSection();

        GUILayout.Space(10f);

        DrawUILine(Color.grey);

        GUILayout.Space(10f);

        DrawDefaultInspector();

        GUILayout.Space(10f);

    }

    string newRoom = "";
    private void drawNewSection()
    {
        GUILayout.Label("New room", SubHeader);
        DrawUILine(Color.grey, 1, 0);
        GUILayout.Space(4f);

        newRoom = EditorGUILayout.TextField("room naam", newRoom);
        if (newRoom != "")
        {
            if (GUILayout.Button("Generate new room prefab"))
            {
                saveTilemapAsAsset(gen, newRoom);
            }
        }
        else
        {
            EditorGUILayout.LabelField("Voeg zon naampje in dan", error);
        }
    }

    Room roomObjectPrevious = null;
    Room roomObject = null;
    Room tempRoomObject = null;
    string PreviousRoomName;
    private void drawEditingSection()
    {
        GUILayout.Label("Editing room", SubHeader);
        DrawUILine(Color.grey, 1, 0);
        GUILayout.Space(4f);
        GUILayout.Label(" Select room to place", SubSubHeader);
        roomObject = EditorGUILayout.ObjectField("DELETES ALL CURRENTLY DRAWN", roomObject, typeof(Room), true) as Room;

        if (!roomObject)
        {
            EditorGUILayout.LabelField("Selecteer zon kamertje dan", error);
        }
        else if (roomObject == roomObjectPrevious)
        {
            if (GUILayout.Button("Save to Assets folder"))
            {
                saveTilemapAsAsset(gen, PreviousRoomName);
            }
        }
        else
        {
            DungeonManager.ClearAllTiles();

            tempRoomObject = (Room)Instantiate(roomObject, new Vector3(0, 0, 0), Quaternion.identity) as Room;
            PreviousRoomName = tempRoomObject.name.Remove(tempRoomObject.name.Length - 7); // nice and hardcoded substring for '(clone)'
            tempRoomObject.DrawRoom();

            DestroyImmediate(tempRoomObject.gameObject);
            roomObjectPrevious = roomObject;
        }
    }
}