using System.Collections.Generic;
using UnityEngine;

public class LSystemPlant3D : MonoBehaviour
{
    public string axiom = "X";

    public int iterations = 4;

    public float angle = 25f;

    public float length = 2f;

    public float width = 0.2f;

    private string currentSentence;

    private Dictionary<char, string> rules =
        new Dictionary<char, string>();

    struct TurtleState
    {
        public Vector3 position;
        public Quaternion rotation;
    }

    void Start()
    {
        rules.Add('X', "F[+X][-X]FX");
        rules.Add('F', "FF");

        Generate();
    }

    void Generate()
    {
        currentSentence = axiom;

        for (int i = 0; i < iterations; i++)
        {
            string nextSentence = "";

            foreach (char c in currentSentence)
            {
                if (rules.ContainsKey(c))
                {
                    nextSentence += rules[c];
                }
                else
                {
                    nextSentence += c.ToString();
                }
            }

            currentSentence = nextSentence;
        }

        DrawPlant();
    }

    void DrawPlant()
    {
        Stack<TurtleState> stack =
            new Stack<TurtleState>();

        Vector3 currentPosition = Vector3.zero;

        Quaternion currentRotation =
            Quaternion.identity;

        foreach (char c in currentSentence)
        {
            switch (c)
            {
                case 'F':

                    Vector3 newPosition =
                        currentPosition +
                        currentRotation * Vector3.up * length;

                    CreateBranch(
                        currentPosition,
                        newPosition);

                    currentPosition = newPosition;

                    break;

                case '+':

                    currentRotation *=
                        Quaternion.Euler(
                            0,
                            0,
                            angle);

                    break;

                case '-':

                    currentRotation *=
                        Quaternion.Euler(
                            0,
                            0,
                            -angle);

                    break;

                case '[':

                    stack.Push(new TurtleState
                    {
                        position = currentPosition,
                        rotation = currentRotation
                    });

                    break;

                case ']':

                    TurtleState state = stack.Pop();

                    currentPosition = state.position;

                    currentRotation = state.rotation;

                    break;
            }
        }
    }

    void CreateBranch(Vector3 start, Vector3 end)
    {
        GameObject branch =
            GameObject.CreatePrimitive(
                PrimitiveType.Cylinder);

        branch.transform.parent = transform;

        Vector3 direction = end - start;

        branch.transform.position =
            start + direction / 2f;

        branch.transform.up = direction;

        branch.transform.localScale =
            new Vector3(
                width,
                direction.magnitude / 2f,
                width);

        Renderer renderer =
            branch.GetComponent<Renderer>();

        renderer.material =
            new Material(
                Shader.Find("Universal Render Pipeline/Lit"));

        renderer.material.color =
            new Color(0.3f, 0.2f, 0.1f);
    }
}