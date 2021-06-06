using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class SpawnObjectsWindow : EditorWindow
{
    string m_numObjects;
    string m_minX = "-10";
    string m_maxX = "10";
    string m_minY = "2";
    string m_maxY = "2";
    string m_minZ = "-10";
    string m_maxZ = "10";

    string m_minRotX = "0";
    string m_maxRotX = "0";
    string m_minRotY = "0";
    string m_maxRotY = "0";
    string m_minRotZ = "0";
    string m_maxRotZ = "0";

    bool m_spawnFromBox = false;
    int m_spawnIndex = 0;

    GameObject m_spawnCube;

    [MenuItem("Tools/Spawn Objects/Spawn Objects")]
    static void SpawnObjectsWindowInit()
    {
        EditorWindow window = GetWindow(typeof(SpawnObjectsWindow));
        window.Show();
    }

    void OnGUI()
    {
        GUILayout.Label("HOW MANY objects do you want to spawn?");
        m_numObjects = GUILayout.TextField(m_numObjects);

        GUILayout.Space(10);

        int numObjects = 0;
        if(!int.TryParse(m_numObjects, out numObjects))
        {
            return;
        }

        if(numObjects > 0)
        {
            GUILayout.Label("HOW do you want to spawn objects?");

            string[] selectionOptions = { "Enter Position Coordinates", "Select Spawn Cube" };
            m_spawnIndex = GUILayout.SelectionGrid(m_spawnIndex, selectionOptions, selectionOptions.Length - 1);
            m_spawnFromBox = m_spawnIndex == 1;

            if (!m_spawnFromBox)
            {
                SetupPosition();
            }
            else
            {
                if(!m_spawnCube)
                {
                    GUILayout.Label("SELECT Spawn Cube.");
                    m_spawnCube = Selection.activeObject as GameObject;

                    GUILayout.Space(10);
                }

                if(m_spawnCube)
                {
                    if (GUILayout.Button("Reset Spawn Cube"))
                    {
                        m_spawnCube = null;
                    }

                    SetupPositionFromCube();
                }
            }

            if (!m_spawnFromBox || m_spawnCube)
            {
                GUILayout.Space(10);

                SetupRotation();

                GUILayout.Space(10);

                GUILayout.Label("Select WHICH object do you want to spawn?");

                GameObject[] selectedGameObjects = Selection.gameObjects;
                if (selectedGameObjects.Length > 0)
                {
                    GUILayout.Label("Selected object: " + GetSelectedObjectNames(selectedGameObjects));

                    if (GUILayout.Button("Spawn Objects"))
                    {
                        SpawnObjects(selectedGameObjects);
                        Repaint();
                    }
                }
            }
        }
    }

    void SetupPositionFromCube()
    {
        BoxCollider boxCollider = m_spawnCube.GetComponent<BoxCollider>();
        if(boxCollider)
        {
            GUILayout.Label("Cube Position");

            m_minX = EditorGUILayout.TextField("Min X", boxCollider.bounds.min.x.ToString());
            m_maxX = EditorGUILayout.TextField("Max X", boxCollider.bounds.max.x.ToString());

            m_minY = EditorGUILayout.TextField("Min Y", boxCollider.bounds.min.y.ToString());
            m_maxY = EditorGUILayout.TextField("Max Y", boxCollider.bounds.max.y.ToString());

            m_minZ = EditorGUILayout.TextField("Min Z", boxCollider.bounds.min.z.ToString());
            m_maxZ = EditorGUILayout.TextField("Max Z", boxCollider.bounds.max.z.ToString());
        }
    }

    void SetupPosition()
    {
        GUILayout.Label("WHERE do you want to spawn objects?");

        m_minX = EditorGUILayout.TextField("Min X", m_minX);
        m_maxX = EditorGUILayout.TextField("Max X", m_maxX);

        m_minY = EditorGUILayout.TextField("Min Y", m_minY);
        m_maxY = EditorGUILayout.TextField("Max Y", m_maxY);

        m_minZ = EditorGUILayout.TextField("Min Z", m_minZ);
        m_maxZ = EditorGUILayout.TextField("Max Z", m_maxZ);
    }

    void SetupRotation()
    {
        GUILayout.Label("WHICH ROTATION do you want to give the objects?");
        m_minRotX = EditorGUILayout.TextField("Min Rot X", m_minRotX);
        m_maxRotX = EditorGUILayout.TextField("Max Rot X", m_maxRotX);

        m_minRotY = EditorGUILayout.TextField("Min Rot Y", m_minRotY);
        m_maxRotY = EditorGUILayout.TextField("Max Rot Y", m_maxRotY);

        m_minRotZ = EditorGUILayout.TextField("Min Rot Z", m_minRotZ);
        m_maxRotZ = EditorGUILayout.TextField("Max Rot Z", m_maxRotZ);
    }

    string GetSelectedObjectNames(GameObject[] selectedGameObjects)
    {
        string nameList = "";

        for (int index = 0; index < selectedGameObjects.Length; ++index)
        {
            nameList += selectedGameObjects[index].name;

            if (index < selectedGameObjects.Length - 1)
            {
                nameList += ",";
            }
            else
            {
                nameList += ".";
            }
        }

        return nameList;
    }

    void SpawnObjects(GameObject[] selectedGameObjects)
    {
        float posX;
        float posY;
        float posZ;

        float rotX;
        float rotY;
        float rotZ;

        int randomObj;

        GameObject parentObj = new GameObject();
        parentObj.name = "SpawnedObjectGroup";

        for (int index = 0; index < int.Parse(m_numObjects); ++index)
        {
            posX = Random.Range(float.Parse(m_minX), float.Parse(m_maxX));
            posY = Random.Range(float.Parse(m_minY), float.Parse(m_maxY));
            posZ = Random.Range(float.Parse(m_minZ), float.Parse(m_maxZ));

            rotX = Random.Range(float.Parse(m_minRotX), float.Parse(m_maxRotX));
            rotY = Random.Range(float.Parse(m_minRotX), float.Parse(m_maxRotX));
            rotZ = Random.Range(float.Parse(m_minRotZ), float.Parse(m_maxRotZ));

            randomObj = Random.Range(0, selectedGameObjects.Length);

            Vector3 pos = new Vector3(posX, posY, posZ);
            Ray ray = new Ray(pos, Vector3.down);
            RaycastHit rayHit;
            if (Physics.Raycast(ray, out rayHit, 10.0f))
            {
                GameObject spawnedObject = GameObject.Instantiate(selectedGameObjects[randomObj], rayHit.point, Quaternion.Euler(rotX, rotY, rotZ)) as GameObject;
                spawnedObject.transform.parent = parentObj.transform;
            }
        }
    }
}