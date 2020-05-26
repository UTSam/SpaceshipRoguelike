using UnityEngine;
using UnityEditor;
using System.IO;
using System;
using Assets.Scripts.Rooms;
using UnityEngine.Tilemaps;

[CustomEditor(typeof(RoomGenerator))]
public class RoomGeneratorEditor : Editor
{

    GUIStyle Header;
    GUIStyle SubHeader;
    GUIStyle SubSubHeader;
    GUIStyle error;

    RoomGenerator gen;

    void OnEnable()
    {
        gen = GameObject.Find("RoomGenerator").GetComponent<RoomGenerator>();
    }

    public override void OnInspectorGUI()
    {
        InitializeCss();

        GUILayout.Space(3f);
        GUILayout.Label("CUSTOM ROOM GENERATION", Header);
        GUILayout.Space(10f);

        if (GUILayout.Button("Clear drawing"))
        {
            gen.tilemap.ClearAllTiles();
        }

        GUILayout.Space(10f);

        DrawNewSection();

        DrawEditingSection();

        DrawUpdateAllRoomsSection();

        DrawUILine(Color.grey);
        GUILayout.Space(10f);

        DrawDefaultInspector();

        GUILayout.Space(10f);
    }

    private static void DrawUILine(Color color, int thickness = 2, int padding = 8)
    {
        Rect r = EditorGUILayout.GetControlRect(GUILayout.Height(padding + thickness));
        r.height = thickness;
        r.y += padding;
        EditorGUI.DrawRect(r, color);
    }

    private void InitializeCss()
    {
        Header = new GUIStyle
        {
            fontSize = 15,
            fontStyle = FontStyle.Bold,
            alignment = TextAnchor.MiddleCenter
        };

        SubHeader = new GUIStyle
        {
            fontSize = 15
        };

        SubSubHeader = new GUIStyle
        {
            fontSize = 14
        };

        error = new GUIStyle
        {
            alignment = TextAnchor.UpperRight
        };
        error.normal.textColor = Color.red;
    }

    // Get all the tiles from the current tilemap and save them into a new file.
    private void SaveTilemapAsAsset(string filename, bool showDialog = true)
    {
        GameObject thingy = gen.GetGameObject();
        thingy.name = filename;

        if (File.Exists(Application.dataPath + "/Resources/Rooms/" + filename + ".prefab"))
        {
            if (!showDialog)
            {
                SaveFile(thingy, filename);
                return;
            }

            bool isOk = EditorUtility.DisplayDialog("Warning", "The file already exsist in the folder. Do you want to overwrite the file?", "YuRP", "PLEASE NO");
            if (isOk)
            {
                SaveFile(thingy, filename);
            }
        }
        else
        {
            SaveFile(thingy, filename);
        }


        DestroyImmediate(thingy);
    }

    private void SaveFile(GameObject prefabToSave, string filename)
    {
        bool saved = PrefabUtility.SaveAsPrefabAsset(prefabToSave, "Assets/Resources/Rooms/" + filename + ".prefab");
        if (saved)
        {
            Debug.Log("Saved the prefab! Pretty goood.");
        }
        else
        {
            Debug.LogError("SOMETHING WENT WRONG ERMGOD");
        }
    }

    string newRoomName;
    private void DrawNewSection()
    {
        GUILayout.Label("New room", SubHeader);
        DrawUILine(Color.grey, 1, 0);
        GUILayout.Space(4f);

        newRoomName = EditorGUILayout.TextField("room naam", newRoomName);
        if (newRoomName != "")
        {
            if (GUILayout.Button("Generate new room prefab"))
            {
                SaveTilemapAsAsset(newRoomName);
            }
        }
        else
        {
            EditorGUILayout.LabelField("Voeg zon naampje in dan", error);
        }

        GUILayout.Space(10f);
    }

    private void DrawGameObjectToTilemap(Room room)
    {
        gen.tilemap.ClearAllTiles();

        room = (Room)Instantiate(room, new Vector3(0, 0, 0), Quaternion.identity) as Room;
        room.name = room.name.Remove(room.name.Length - 7); // nice and hardcoded substring for '(clone)'
        room.DrawRoom(gen.tilemap);

        // Since the doors tiles are removed upon saving we need to extract it and display it again.
        foreach (Door door in room.doors)
        {
            Tile tile = null;
            switch (door.direction)
            {
                case Direction.Up:
                    tile = gen.doorUp;
                    break;
                case Direction.Down:
                    tile = gen.doorDown;
                    break;
                case Direction.Left:
                    tile = gen.doorLeft;
                    break;
                case Direction.Right:
                    tile = gen.doorRight;
                    break;
            }
            gen.tilemap.SetTile((door.position + room.position), tile);
        }

        DestroyImmediate(room.gameObject);
    }

    Room roomObject = null;
    Room roomObjectPrevious = null;
    private void DrawEditingSection()
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
                SaveTilemapAsAsset(roomObject.name);
            }
        }
        else
        {
            DrawGameObjectToTilemap(roomObject);
            roomObjectPrevious = roomObject;
        }

        GUILayout.Space(10f);
    }

    private void DrawUpdateAllRoomsSection()
    {
        GUILayout.Label("Update all rooms", SubHeader);
        DrawUILine(Color.grey, 1, 0);
        GUILayout.Space(4f);

        if (GUILayout.Button("Update all rooms"))
        {
            string resourceFolder = "Rooms";
            UnityEngine.Object[] rooms = Resources.LoadAll(resourceFolder, typeof(Room));

            //loop through directory loading the game object and checking if it has the component you want
            foreach (Room room in rooms)
            {
                DrawGameObjectToTilemap(room);
                SaveTilemapAsAsset(room.name, true);
            }

            gen.tilemap.ClearAllTiles();
        }
    }
}