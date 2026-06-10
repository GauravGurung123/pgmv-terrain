using System.Collections.Generic;
using UnityEngine;

public class LSystemPlant3D : MonoBehaviour
{
    [Header("L-System Settings")]
    [SerializeField] private string axiom = "F";
    [SerializeField] private string ruleF = "F[+F]F[-F]F";
    [SerializeField, Range(0, 6)] private int iterations = 4;

    [Header("Shape Settings")]
    [SerializeField] private float angle = 25f;
    [SerializeField] private float segmentLength = 0.8f;
    [SerializeField] private float branchRadius = 0.05f;
    [SerializeField] private float lengthMultiplierPerIteration = 0.75f;

    [Header("Visual Settings")]
    [SerializeField] private Material branchMaterial;
    [SerializeField] private Material leafMaterial;
    [SerializeField] private bool generateOnStart = true;

    private readonly List<GameObject> generatedObjects = new List<GameObject>();

    private struct TurtleState
    {
        public Vector3 position;
        public Quaternion rotation;
        public float length;
        public float radius;

        public TurtleState(Vector3 position, Quaternion rotation, float length, float radius)
        {
            this.position = position;
            this.rotation = rotation;
            this.length = length;
            this.radius = radius;
        }
    }

    private void Start()
    {
        if (generateOnStart)
        {
            Generate();
        }
    }

    [ContextMenu("Generate L-System Plant")]
    public void Generate()
    {
        ClearGeneratedObjects();

        string sentence = BuildSentence();

        Vector3 currentPosition = transform.position;
        Quaternion currentRotation = transform.rotation;
        float currentLength = segmentLength;
        float currentRadius = branchRadius;

        Stack<TurtleState> stack = new Stack<TurtleState>();

        foreach (char symbol in sentence)
        {
            switch (symbol)
            {
                case 'F':
                    Vector3 nextPosition = currentPosition + currentRotation * Vector3.up * currentLength;
                    CreateBranch(currentPosition, nextPosition, currentRadius);
                    currentPosition = nextPosition;
                    break;

                case '+':
                    currentRotation *= Quaternion.Euler(0f, 0f, angle);
                    break;

                case '-':
                    currentRotation *= Quaternion.Euler(0f, 0f, -angle);
                    break;

                case '&':
                    currentRotation *= Quaternion.Euler(angle, 0f, 0f);
                    break;

                case '^':
                    currentRotation *= Quaternion.Euler(-angle, 0f, 0f);
                    break;

                case '/':
                    currentRotation *= Quaternion.Euler(0f, angle, 0f);
                    break;

                case '\\':
                    currentRotation *= Quaternion.Euler(0f, -angle, 0f);
                    break;

                case '[':
                    stack.Push(new TurtleState(currentPosition, currentRotation, currentLength, currentRadius));
                    currentLength *= lengthMultiplierPerIteration;
                    currentRadius *= 0.75f;
                    break;

                case ']':
                    CreateLeaf(currentPosition);

                    if (stack.Count > 0)
                    {
                        TurtleState state = stack.Pop();
                        currentPosition = state.position;
                        currentRotation = state.rotation;
                        currentLength = state.length;
                        currentRadius = state.radius;
                    }
                    break;
            }
        }
    }

    private string BuildSentence()
    {
        string currentSentence = axiom;

        for (int i = 0; i < iterations; i++)
        {
            string nextSentence = "";

            foreach (char symbol in currentSentence)
            {
                if (symbol == 'F')
                {
                    nextSentence += ruleF;
                }
                else
                {
                    nextSentence += symbol;
                }
            }

            currentSentence = nextSentence;
        }

        return currentSentence;
    }

    private void CreateBranch(Vector3 startPosition, Vector3 endPosition, float radius)
    {
        Vector3 direction = endPosition - startPosition;
        Vector3 middlePoint = startPosition + direction * 0.5f;

        GameObject branch = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        branch.name = "Generated_Branch";
        branch.transform.SetParent(transform);
        branch.transform.position = middlePoint;
        branch.transform.up = direction.normalized;
        branch.transform.localScale = new Vector3(radius, direction.magnitude * 0.5f, radius);

        if (branchMaterial != null)
        {
            branch.GetComponent<Renderer>().material = branchMaterial;
        }

        generatedObjects.Add(branch);
    }

    private void CreateLeaf(Vector3 position)
    {
        GameObject leaf = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        leaf.name = "Generated_Leaf";
        leaf.transform.SetParent(transform);
        leaf.transform.position = position;
        leaf.transform.localScale = Vector3.one * 0.25f;

        if (leafMaterial != null)
        {
            leaf.GetComponent<Renderer>().material = leafMaterial;
        }

        generatedObjects.Add(leaf);
    }

    private void ClearGeneratedObjects()
    {
        foreach (GameObject generatedObject in generatedObjects)
        {
            if (generatedObject != null)
            {
                DestroyImmediate(generatedObject);
            }
        }

        generatedObjects.Clear();

        List<GameObject> oldChildren = new List<GameObject>();

        foreach (Transform child in transform)
        {
            if (child.name.StartsWith("Generated_"))
            {
                oldChildren.Add(child.gameObject);
            }
        }

        foreach (GameObject child in oldChildren)
        {
            DestroyImmediate(child);
        }
    }
}