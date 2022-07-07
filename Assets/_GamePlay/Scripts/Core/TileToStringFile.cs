using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using StackMaker.Core;

public class TileToStringFile : MonoBehaviour
{
    [SerializeField]
    LayerMask stackMask;
    [SerializeField]
    string pathFolder = "Assets/_GamePlay/Resources/LevelData";
    [SerializeField]
    string nameMapData = "Test.txt";
    [SerializeField]
    private float mapWide = 10f;
    private float mapHeight = 2f;
    List<string> writeData;
    Dictionary<Vector2Int, AbstractStack> data;

    Vector2Int maxPosition = new Vector2Int(int.MinValue,int.MinValue);
    Vector2Int minPosition = new Vector2Int(int.MaxValue, int.MaxValue);
    private Vector3 MapShape
    {
        get
        {
            Vector3 shape = Vector3.one * mapWide;
            return new Vector3(shape.x, mapHeight, shape.z);
        }
    }
    private void ConstructDataFile()
    {
        writeData = new List<string>();
        for(int y = maxPosition.y; y >= minPosition.y; y--)
        {
            string lineData = "";
            for(int x = maxPosition.x; x >= minPosition.x; x--)
            {
                Vector2Int pos = new Vector2Int(x, y);
                if (!data.ContainsKey(pos))
                {
                    lineData += '0';
                }
                else
                {
                    if(data[pos] is AddStack)
                    {
                        if(data[pos] is NormalAddStack)
                        {
                            lineData += '+';
                        }
                        else if(data[pos] is CrossAddStack)
                        {
                            lineData += 'x';
                        }
                    }
                    else if(data[pos] is SubtractStack)
                    {
                        if(data[pos] is NormalSubtractStack)
                        {
                            lineData += '-';
                        }
                        else if(data[pos] is DesSubtractStack)
                        {
                            lineData += '=';
                        }
                    }
                    else if(data[pos] is StartStack)
                    {
                        lineData += 'S';
                    }
                }
            }
            lineData += '#';
            writeData.Add(lineData);
        }
    }
    public static Vector2Int GetPosition(Vector3 pos)
    {
        pos = pos / Level.TileWide;
        return new Vector2Int(Mathf.RoundToInt(pos.x), Mathf.RoundToInt(pos.y));
    }
    private void GetStackDataFromScene()
    {
        data = new Dictionary<Vector2Int, AbstractStack>();

        Collider[] stack = Physics.OverlapBox(transform.localPosition, MapShape, Quaternion.identity, stackMask);
        for (int i = 0; i < stack.Length; i++)
        {
            AbstractStack stackScript = stack[i].gameObject.GetComponent<AbstractStack>();
            Vector2Int pos = GetPosition(stack[i].transform.localPosition - Vector3.one * 0.5f);
            UpdateMaxMin(pos);

            if (stackScript != null)
            {
                data.Add(pos, stackScript);
            }
        }
    }
    private void UpdateMaxMin(Vector2Int pos)
    {
        if(pos.x > maxPosition.x)
        {
            maxPosition.x = pos.x;
        }

        if(pos.y > maxPosition.y)
        {
            maxPosition.y = pos.y;
        }

        if(pos.x < minPosition.x)
        {
            minPosition.x = pos.x;
        }

        if(pos.y < minPosition.y)
        {
            minPosition.y = pos.y;
        }
    }
    public void WriteAFile()
    {
        GetStackDataFromScene();
        ConstructDataFile();
        string path = pathFolder + '/' + nameMapData;
        //Write some text to the test.txt file       
        File.Create(path).Close();

        StreamWriter writer = new StreamWriter(path, true);
        for(int i = 0; i < writeData.Count; i++)
        {
            writer.WriteLine(writeData[i]);
        }
        writer.Close();
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawCube(transform.position, MapShape * 2);
    }
}
