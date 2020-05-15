using UnityEngine;
using UnityEditor;
using System.IO;
using System;
using Assets.Scripts.Rooms;
using UnityEngine.Tilemaps;

[CustomEditor(typeof(RoomGenerator))]
public class RoomGeneratorEditor : Editor
{
    RoomGenerator gen;

    GUIStyle Header;
    GUIStyle SubHeader;
    GUIStyle SubSubHeader;
    GUIStyle error;

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

    // Get all the tiles from the current tilemap and save them into a new file.
    private void saveTilemapAsAsset(string filename, bool showDialog = true)
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

    private void DrawNewSection()
    {
        GUILayout.Label("New room", SubHeader);
        DrawUILine(Color.grey, 1, 0);
        GUILayout.Space(4f);

        string newRoomName = "";
        newRoomName = EditorGUILayout.TextField("room naam", newRoomName);
        if (newRoomName != "")
        {
            if (GUILayout.Button("Generate new room prefab"))
            {
                saveTilemapAsAsset(newRoomName);
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
        DungeonManager.ClearAllTiles();

        room = (Room)Instantiate(room, new Vector3(0, 0, 0), Quaternion.identity) as Room;
        room.name = room.name.Remove(room.name.Length - 7); // nice and hardcoded substring for '(clone)'
        room.DrawRoom();

        foreach (Door door in room.doors)
        {
            Tile tile = null;
            switch (door.direction)
            {
                case Direction.Up:
                    tile = gen.doorU;
                    break;
                case Direction.Down:
                    tile = gen.doorD;
                    break;
                case Direction.Left:
                    tile = gen.doorLeft;
                    break;
                case Direction.Right:
                    tile = gen.doorR;
                    break;
            }
            DungeonManager.tilemap_walls.SetTile((door.position + room.position), tile);
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
                saveTilemapAsAsset(roomObject.name);
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
            string path = "Assets/Resources/Rooms";
            string pathWithoutAssets = "Rooms";
            DirectoryInfo dirInfo = new DirectoryInfo(path);
            FileInfo[] fileInf = dirInfo.GetFiles("*.prefab");

            //loop through directory loading the game object and checking if it has the component you want
            foreach (FileInfo fileInfo in fileInf)
            {
                string fullPath = fileInfo.FullName.Replace(@"\", "/");
                GameObject prefab = Resources.Load<GameObject>(pathWithoutAssets + "/" + RemoveFileExtension(fileInfo.Name));
                if (prefab == null) continue;

                Room room = prefab.GetComponent<Room>();
                if (room != null)
                {
                    DrawGameObjectToTilemap(room);
                    saveTilemapAsAsset(room.name, true);
                }
            }

            DungeonManager.ClearAllTiles();
        }
    }

    private string RemoveFileExtension(string fileName)
    {
        string filenameWithoutExt = "";
        int fileExtPos = fileName.LastIndexOf(".", StringComparison.Ordinal);

        if (fileExtPos >= 0)
            filenameWithoutExt = fileName.Substring(0, fileExtPos);

        return filenameWithoutExt;
    }
}