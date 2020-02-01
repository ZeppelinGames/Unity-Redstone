using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Power : MonoBehaviour
{
    public bool isSource;
    public bool powered;
    public int powerDelay=0;

    private bool delayed;

    [Space(10)]
    public Transform reciever;

    [Space(10)]
    public List<Vector3> inPositions = new List<Vector3>();
    public List<Vector3> outPositions = new List<Vector3>();

    [Space(10)]
    public Color poweredColor;
    public Color unpoweredColor;

    private void Start()
    {
        AdjustInOutputs();
        if (powerDelay > 0)
        {
            StartCoroutine(PoweredDelay());
        }
    }

    private void Update()
    {
        CheckAdjacent();

        if (reciever == null)
        {
            if (!isSource)
            {
                if (powerDelay <= 0)
                {
                    powered = false;
                    GetComponent<MeshRenderer>().material.color = unpoweredColor;
                }
            }
        }
        else
        {
            if (powerDelay <= 0)
            {
                powered = true;
                GetComponent<MeshRenderer>().material.color = poweredColor;
            }
        }
    }

    IEnumerator PoweredDelay()
    {
        while (powered)
        {
            GetComponent<MeshRenderer>().material.color = poweredColor;
            yield return new WaitForSeconds(powerDelay);
            powered = true;
            powered = false;
            GetComponent<MeshRenderer>().material.color = unpoweredColor;
            yield return new WaitForSeconds(powerDelay);
            powered = true;
        }
    }

    void AdjustInOutputs()
    {
        List<Vector3> newInPos = new List<Vector3>();
        foreach (Vector3 pos in inPositions)
        {
            newInPos.Add(transform.position + transform.TransformDirection(pos));
        }

        inPositions = newInPos;

        List<Vector3> newOutPos = new List<Vector3>();
        foreach (Vector3 pos in outPositions)
        {
            newOutPos.Add(transform.position + transform.TransformDirection(pos));
        }

        outPositions = newOutPos;
    }

    void CheckAdjacent()
    {
        List<Transform> nearby = new List<Transform>();
        foreach (Vector3 pos in inPositions)
        {
            Collider[] cols = Physics.OverlapBox(pos, new Vector3(0.25f, 0.25f, 0.25f));
            foreach (Collider col in cols)
            {
                if (col.transform != transform)
                {
                    if (col.GetComponent<Power>())
                    {
                        nearby.Add(col.transform);
                    }
                }
            }
        }

        Transform decidedReciever = null;
        foreach (Transform near in nearby)
        {
            bool correctPos = false;
            if (!near.GetComponent<Power>().isSource)
            {
                foreach (Vector3 pos in near.GetComponent<Power>().outPositions)
                {
                    if (transform.position == pos)
                    {
                        correctPos = true;
                    }
                }
            }
            else
            {
                correctPos = true;
            }

            if (correctPos)
            {
                if (near.GetComponent<Power>().reciever != transform)
                {
                    if (near.GetComponent<Power>().powered)
                    {
                        decidedReciever = near;
                    }
                }
            }
        }

        reciever = decidedReciever;
    }
}
