using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using Accord.Statistics.Models.Markov;
using Accord.Statistics.Models.Markov.Learning;

public class PlayerMain : MonoBehaviour {
    int x_index = 0;
    int y_index = 0;
    int cx_index = 0;
    int cy_index = 0;
    public GameObject message;
    public GameObject controller;
    private Transform controllerTransform;
    private static int size = 20;
    int[] x_movements = new int[size];
    int[] y_movements = new int[size];
    int[] cx_movements = new int[size];
    int[] cy_movements = new int[size];
    string data = "";
    public float gestureThreshold = 0.01f;
    public bool isTraining = false;

    private HiddenMarkovClassifier classifier;

    // Use this for initialization
    void Start () {

        controllerTransform = controller.GetComponent<Transform>();


        // Declare some training data
        int[][] inputs = new int[][]
        {
            /* Head Nod Data */
            new int[] {5, 5, 6, 6, 6, 7, 7, 8, 9, 9, 8, 8, 7, 6, 6, 6, 5, 5, 5, 5, },
            new int[] {5, 6, 6, 6, 7, 8, 8, 8, 8, 8, 8, 7, 6, 6, 6, 5, 5, 4, 4, 5, },
            new int[] {5, 6, 6, 6, 7, 8, 8, 8, 8, 8, 8, 7, 6, 6, 6, 6, 6, 6, 6, 6, },
            new int[] {8, 8, 8, 9, 9, 9, 8, 8, 7, 7, 6, 6, 6, 6, 6, 6, 6, 6, 7, 7, },
            new int[] {8, 9, 9, 9, 9, 9, 9, 8, 7, 7, 7, 7, 7, 7, 7, 7, 8, 8, 9, 9, },
            new int[] {9, 9, 9, 9, 8, 8, 7, 7, 6, 6, 6, 6, 6, 6, 6, 7, 7, 8, 8, 8, },
            new int[] {8, 7, 7, 6, 6, 6, 5, 5, 5, 5, 5, 5, 6, 6, 6, 7, 7, 7, 7, 7, },
            new int[] {7, 6, 6, 6, 5, 4, 4, 4, 4, 4, 4, 5, 5, 6, 6, 7, 7, 8, 8, 8, },
            new int[] {7, 6, 6, 6, 5, 5, 5, 5, 5, 5, 6, 6, 7, 8, 8, 8, 8, 8, 8, 7, },
            new int[] {6, 6, 6, 5, 5, 4, 4, 4, 5, 5, 6, 6, 7, 7, 8, 8, 8, 8, 7, 7, },
            new int[] {6, 6, 6, 5, 5, 5, 5, 5, 6, 6, 6, 7, 7, 8, 8, 8, 7, 7, 6, 6, },
            new int[] {5, 5, 5, 5, 5, 6, 6, 6, 7, 7, 8, 8, 8, 8, 8, 7, 7, 6, 6, 6, },
            new int[] {5, 5, 5, 5, 5, 6, 6, 6, 7, 8, 8, 8, 8, 8, 7, 7, 6, 6, 6, 5, },
            new int[] {5, 5, 5, 5, 5, 6, 6, 7, 7, 8, 8, 8, 8, 8, 7, 7, 6, 6, 5, 5, },
            new int[] {3, 3, 3, 3, 3, 4, 5, 5, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 5, 5, },
            new int[] {4, 4, 4, 5, 5, 6, 6, 6, 7, 8, 8, 8, 8, 8, 7, 7, 6, 6, 6, 6, },
            new int[] {5, 5, 5, 6, 6, 6, 7, 7, 8, 8, 8, 8, 8, 7, 7, 6, 6, 6, 6, 6, },
            new int[] {6, 6, 6, 6, 7, 8, 8, 8, 9, 9, 8, 8, 8, 7, 6, 6, 6, 6, 6, 6, },
            new int[] {6, 6, 6, 7, 7, 8, 8, 8, 8, 8, 8, 7, 7, 6, 6, 6, 6, 6, 6, 6, },
            new int[] {7, 6, 6, 6, 5, 5, 4, 4, 4, 4, 4, 5, 5, 6, 6, 6, 7, 8, 8, 8, },
            new int[] {8, 7, 7, 6, 6, 6, 5, 5, 4, 4, 5, 5, 6, 6, 6, 7, 8, 8, 8, 8, },
            new int[] {8, 7, 6, 6, 6, 6, 5, 5, 5, 5, 6, 6, 6, 6, 7, 7, 8, 8, 8, 8, },
            new int[] {7, 7, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 7, 7, 8, 8, 8, 8, 8, 8, },
            new int[] {6, 6, 6, 6, 6, 6, 6, 6, 7, 7, 7, 8, 8, 8, 8, 8, 7, 7, 6, 6, },
            new int[] {6, 6, 6, 6, 7, 7, 8, 8, 8, 9, 9, 9, 8, 8, 8, 7, 7, 6, 6, 6, },
            new int[] {6, 7, 7, 8, 8, 9, 9, 9, 9, 9, 8, 8, 7, 7, 7, 6, 6, 6, 6, 7, },
            new int[] {8, 8, 8, 8, 8, 8, 7, 7, 6, 6, 6, 6, 5, 5, 5, 5, 6, 6, 6, 6, },
            new int[] {7, 7, 7, 7, 6, 6, 6, 5, 5, 4, 4, 3, 4, 4, 4, 5, 5, 6, 6, 6, },
            new int[] {6, 6, 6, 6, 5, 5, 4, 4, 4, 4, 5, 5, 6, 6, 6, 6, 6, 6, 6, 5, },
            new int[] {4, 4, 3, 3, 3, 3, 4, 5, 5, 6, 6, 6, 6, 6, 6, 6, 6, 5, 4, 4, },
            new int[] {3, 3, 4, 4, 5, 6, 6, 6, 6, 7, 7, 6, 6, 6, 6, 5, 5, 5, 5, 5, },
            new int[] {5, 6, 6, 6, 7, 7, 7, 7, 7, 7, 6, 6, 6, 6, 5, 5, 5, 6, 6, 6, },
            new int[] {6, 7, 7, 7, 8, 8, 8, 8, 8, 8, 7, 7, 6, 6, 6, 6, 5, 5, 5, 5, },
            new int[] {6, 6, 7, 7, 8, 8, 9, 9, 9, 9, 9, 8, 8, 7, 6, 6, 6, 6, 5, 5, },
            new int[] {5, 5, 6, 6, 7, 7, 8, 8, 8, 8, 8, 8, 8, 7, 7, 6, 6, 6, 6, 5, },
            new int[] {5, 5, 5, 6, 6, 6, 6, 7, 8, 8, 8, 8, 8, 8, 7, 7, 6, 6, 6, 6, },
            new int[] {5, 5, 6, 6, 6, 6, 7, 7, 8, 8, 8, 8, 8, 8, 7, 7, 6, 6, 6, 6, },
            new int[] {6, 6, 6, 7, 7, 8, 8, 9, 9, 9, 8, 8, 7, 7, 6, 6, 6, 5, 5, 5, },
            new int[] {5, 5, 5, 6, 6, 7, 7, 8, 8, 8, 8, 8, 8, 7, 7, 6, 6, 6, 5, 5, },

            /* Still */
            new int[] {6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, },
            new int[] {6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, },
            new int[] {2, 2, 2, 1, 1, 1, 1, 1, 1, 1, 1, 3, 3, 3, 3, 3, 3, 2, 2, 2, },
            new int[] {6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, },
            new int[] {2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, },
            new int[] {6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, },
            new int[] {6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, },
            new int[] {6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, },
            new int[] {6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, },
            new int[] {6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, },
            new int[] {6, 6, 6, 6, 6, 6, 6, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, },
            new int[] {7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, },
            new int[] {6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, },
            new int[] {6, 6, 6, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 6, },
            new int[] {6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, },
            new int[] {6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, },
            new int[] {6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, },
            new int[] {6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, },
            new int[] {6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, },
            new int[] {6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 5, 5, },
            new int[] {5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, },
            new int[] {5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, },
            new int[] {5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, },
            new int[] {5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, },
            new int[] {7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, },
            new int[] {7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, },
            new int[] {7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 6, 6, 6, 6, 6, },
            new int[] {6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, },
            new int[] {6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, },
            new int[] {5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, },
            new int[] {5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, },
            new int[] {5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 6, 6, 6, 6, 6, 6, 6, },
            new int[] {6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, },
            new int[] {6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, },
            new int[] {6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, },
            new int[] {6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, },
            new int[] {11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, },
            new int[] {6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, },
            new int[] {6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, }
        };

        int[] outputs = new int[]
        {
            1,1,1,1,1,1,1,1,1,1,
            1,1,1,1,1,1,1,1,1,1,
            1,1,1,1,1,1,1,1,1,1,
            1,1,1,1,1,1,1,1,1,
            0,0,0,0,0,0,0,0,0,0,
            0,0,0,0,0,0,0,0,0,0,
            0,0,0,0,0,0,0,0,0,0,
            0,0,0,0,0,0,0,0,0
        };


        // We are trying to predict two different classes
        int classes = 2;

        // Each sequence may have up to two symbols (0 or 1)
        int symbols = 13;

        // Nested models will have two states each
        int[] states = new int[] { 2, 2 };

        // Creates a new Hidden Markov Model Classifier with the given parameters
        classifier = new HiddenMarkovClassifier(classes, states, symbols);

        // Create a new learning algorithm to train the sequence classifier
        var teacher = new HiddenMarkovClassifierLearning(classifier,

            // Train each model until the log-likelihood changes less than 0.001
            modelIndex => new BaumWelchLearning(classifier.Models[modelIndex])
            {
                Tolerance = 0.001,
                MaxIterations = 0
            }
        );

        // Train the sequence classifier using the algorithm
        teacher.Learn(inputs, outputs);
    }
	
	// Update is called once per frame
	void Update () {
        int c_x = (int) ((controllerTransform.rotation.x * 10) + 11);
        if (c_x < 1) c_x = 1;
        if (c_x > 11) c_x = 11;
        Debug.Log("Controller X: "+c_x);

        int c_y = (int)((controllerTransform.rotation.y * 10) + 6);
        if (c_y < 0) c_y = 1;
        if (c_y > 9) c_y = 9;
        Debug.Log("Controller Y: " + c_y);

        int x = (int) (transform.rotation.x * 10) + 6;
        if (x < 2) x = 0;
        if (x > 10) x = 10;

        int y = (int) (transform.rotation.y * 10) + 6;
        if (y < 0) y = 0;
        if (y > 12) y = 12;

        
        if (!isTraining)
        {
            if (x_index < size)
            {
                x_movements[x_index++] = x;
            }

            if (y_index < size)
            {
                y_movements[y_index++] = y;
            }

            if (cx_index < size)
            {
                cx_movements[cx_index++] = c_x;
            }

            if (cy_index < size)
            {
                cy_movements[cy_index++] = c_y;
            }

            if (x_index >= size && y_index >= size && cx_index >= size && cy_index >= size)
            {
                x_index = 0;
                y_index = 0;
                cx_index = 0;
                cy_index = 0;
                Debug.Log(y_movements);
                int isVertical = classifier.Decide(x_movements);
                int isHorizontal = classifier.Decide(y_movements);
                int isControllerHor = classifier.Decide(cx_movements);
                int isControllerVert = classifier.Decide(cy_movements);
                Text canvas = message.GetComponent<Text>();
                string answer = "";
                if (isControllerHor == 1 && isControllerVert == 0)
                {
                    answer = "Hello!";
                }

                if (isControllerHor == 0 && isControllerVert == 1 && isVertical == 1)
                {
                    answer = "Come over!";
                } else if (isHorizontal == 1 && isVertical == 0)
                {
                    answer = "No";
                }
                else if (isVertical == 1 && isHorizontal == 0)
                {
                    answer = "Yes";
                }
                if (!answer.Equals(""))
                    canvas.text = answer;
            }
        } else
        {
            if (x_index < size)
            {
                x_movements[x_index++] = x;
            }
            else if (y_index < size)
            {
                y_movements[y_index++] = y;
            }
            else
            {
                x_index = 0;
                y_index = 0;
                string s = "{";
                foreach (int i in x_movements)
                {
                    s += i + ", ";
                }
                s += "}";
                data += s + '\n';
                Debug.Log(data);
            }
        }

	}
}
