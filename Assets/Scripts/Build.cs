using System;
using System.Linq;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Build : MonoBehaviour
{
    public GameObject[] buildObjects;
    private int currObject;

    [Space(10)]
    public Text objectName;

    private List<Vector3> pos = new List<Vector3>();
    private List<int> ids = new List<int>();

    public static List<Vector3> staticPos = new List<Vector3>();
    public static List<int> staticIds = new List<int>();

    void Update()
    {
        BuildBlock();
        DestroyBlock();
        ChangeObject();

        staticPos = pos;
        staticIds = ids;
    }

    void ChangeObject()
    {
        if (Input.mouseScrollDelta.y > 0)
        {
            currObject++;
        }
        if (Input.mouseScrollDelta.y < 0)
        {
            currObject--;
        }

        currObject = Mathf.Clamp(currObject, 0, buildObjects.Length - 1);

        objectName.text = buildObjects[currObject].name;
    }

    void BuildBlock()
    {
        if (Input.GetMouseButtonDown(1))
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, 10))
            {
                if (hit.transform != transform)
                {
                    int x = Mathf.RoundToInt(hit.point.x);
                    int y = Mathf.RoundToInt(hit.point.y);
                    int z = Mathf.RoundToInt(hit.point.z);

                    Vector3 tempPos = new Vector3(x, y, z);

                    if (!pos.Contains(tempPos))
                    {
                        int rotX = Mathf.RoundToInt(transform.eulerAngles.x / 90) * 90;
                        int rotY = Mathf.RoundToInt(transform.eulerAngles.y / 90) * 90;
                        int rotZ = Mathf.RoundToInt(transform.eulerAngles.z / 90) * 90;

                        GameObject newBlock = Instantiate(buildObjects[currObject], new Vector3(x, y, z), Quaternion.Euler(new Vector3(rotX,rotY,rotZ)));                    

                        newBlock.transform.eulerAngles = new Vector3(rotX, rotY, rotZ);

                        pos.Add(tempPos);
                        ids.Add(currObject + 1);
                    }
                }
            }
        }
    }

    void DestroyBlock()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, 10))
            {
                if (pos.Contains(hit.transform.position))
                {
                    int index = pos.IndexOf(hit.transform.position);
                    pos.RemoveAt(index);
                    ids.RemoveAt(index);

                    Destroy(hit.transform.gameObject);
                }
            }
        }
    }

    public static int GetBlockID(Vector3 getPosition)
    {
        if (staticPos.Contains(getPosition))
        {
            int index = staticPos.IndexOf(getPosition);
            return staticIds[index];
        }
        else
        {
            return 0;
        }
    }
}
