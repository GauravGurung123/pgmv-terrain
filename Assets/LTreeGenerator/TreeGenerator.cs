using LTreeGenerator;

using UnityEngine;
using System.Collections.Generic;

public class TreeGenerator : MonoBehaviour
{
    private string tree;

    [SerializeField] private string axiom;
    [SerializeField] private int iterations;
    Stack<TransformInfoHelper> stack = new Stack<TransformInfoHelper>();
    private TransformInfoHelper helper;
    [SerializeField] private float length;
    [SerializeField] private float angle;
    private List<List<Vector3>> LineList = new List<List<Vector3>>();
    void ExpandTreeString()
    {
        string expandTree;

        for (int i = 0; i < iterations; i++)
        {
            expandTree = "";
            foreach (char j in tree)
            {
                expandTree += j switch
                {
                    'F' => "FF",
                    'B' => "[lFB][rFB]",
                     _  => j.ToString()
                };   
            }
            tree = expandTree;
             Debug.Log("Tree at iteration "+ i + " is " + tree);
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        tree = axiom;
        Debug.Log("Starting tree" + tree);
        ExpandTreeString();
        CreateMesh();
    }

    void OnDrawGizmos()
    {
        foreach (List<Vector3> line in LineList)
        {
            Gizmos.DrawLine(line[0], line[1]);
        }
    }

    void CreateMesh()
    {
        Vector3 initialPosition;

        foreach (char i in tree)
        {
            switch (i)
            {
                case 'F':
                    initialPosition = transform.position;
                    transform.Translate(Vector3.up * length); 
                    LineList.Add(new List<Vector3>() { initialPosition, transform.position });
                    initialPosition = transform.position;

                    break;
                case 'B':
                     // We can add some randomness to the length of the branches to make it look more natural
                    break;
                case '[':
                    stack.Push(new TransformInfoHelper()
                    {
                        position = transform.position,
                        rotation = transform.rotation,
                    });
                    break;
                case ']':
                    TransformInfoHelper helper = stack.Pop();
                    transform.position = helper.position;
                    transform.rotation = helper.rotation;
                    break;
                case 'l':
                    transform.Rotate(Vector3.back, angle);
                    break;
                case 'r':
                    transform.Rotate(Vector3.forward, angle);
                    break;
                
            }
        }
    }
}
