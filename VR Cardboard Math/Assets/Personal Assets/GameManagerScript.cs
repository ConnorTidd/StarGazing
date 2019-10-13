using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
public class CameraRaycaster : MonoBehaviour
{

    // Materials for selecting objects
    public Material highlightMaterialStar;
    public Material defaultMaterialStar;
    public Material highlightMaterialUI;
    public Material defaultMaterialUI;

    //Where the first star spawns
    public Vector3 initialStarPos = new Vector3(5f,2f,5f);

    // Amount of stars spawned per question
    public int newstars = 5;
    // GameObject prefab for spawning stars
    public GameObject star;
    // Question numbers
    private int num1;
    private int num2;

    // Spacial bounds where stars are allowed to spawn
    float yUpBound = 7;
    float yLowBound = 1;
    float xBound = 9;
    float zBound = 9;
    
    private Transform solutionStarTransform;
    private GameObject questionStar;
    private List<GameObject> nonSolutionStars = new List<GameObject>();
    private List<GameObject> completedStars = new List<GameObject>();
    private string starTag = "Star";
    private string UIStartMenuTag = "UIStartMenu";
    /*represents whether the game is:
      0  No state
      1  Addition
      2  Subtraction
      3  Multiplication
      4  Division
      5  Addition and Subtraction
      6  Multiplication and Division
      6  Addition, Subtraction, Multiplication, and Division*/
  /*  private int gameState = 0;
    private int questionsPerGame = 5;
    private int answeredQuestions = 0;
    private int wrongAnswersPerGame = 3;
    private int wrongAnswers = 0;

    //represents all answers for a game;
    private List<List<int>> answerGrid;

    private Transform prevSelectedObjectTransform;
    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < newstars; i++)
        {

        }
    }

    // Update is called once per frame
    void Update()
    {
        DeselectObject();
        SelectObject();
        
        DrawConstellationLines();
        
    }

    private void GameEnd(bool gameWon)
    {
        if(gameWon)
        {
            Debug.Log("Game Won!");
        }
        else
        {
            Debug.Log("Game Lost :(");
        }
    }

    // Handles selecting a star
    public void SelectStar(GameObject selectedObj)
    {
        StarScript sScript = (StarScript) selectedObj.GetComponent("StarScript");
        if (sScript.isSolution)
        {
            answeredQuestions = answeredQuestions + 1;
            foreach (GameObject star in nonSolutionStars)
            {
                Destroy(star);
            }
            nonSolutionStars = new List<GameObject>();

            completedStars.Insert(0, questionStar);
            this.Retire(questionStar);
            
            questionStar = selectedObj;
            sScript.isSolution = false;
           



            if (answeredQuestions == questionsPerGame)
            {
                this.GameEnd(true);
                this.Retire(questionStar);
            } else
            {
                num1 = (int)Random.Range(1f, 12f);
                num2 = (int)Random.Range(1f, 12f);
                string newVal = num1.ToString() + " x " + num2.ToString();
                sScript.ChangeTextTo(newVal);
                SpawnStars();
            }
            
        } else
        {
            sScript.Remove();
            
            this.nonSolutionStars.Remove(selectedObj);
            wrongAnswers = wrongAnswers + 1;
            if(wrongAnswers == wrongAnswersPerGame)
            {
                this.GameEnd(false);
            }
        }
    }

    private void Retire(GameObject star)
    {
        StarScript sScript = (StarScript) star.GetComponent("StarScript");
        sScript.Finish();
    }

    //Spawns 'newstars' amount of star objects at random x z positions with y = 0, and then rises them up to a random y position
    //random positions in range of declared bounds
    public void SpawnStars()
    {

        //creates new stars at random positions with 1st being a solution, and others not
        for (int i = 0; i < newstars; i++)
        {
            GameObject newstar = Instantiate(star);

            StarScript sScript = (StarScript)newstar.GetComponent("StarScript");
            if (i == 0)
            {
                // keep track of the solution star
                solutionStarTransform = newstar.transform;

                // initialize star to be a solution
                sScript.isSolution = true;
                string val = (num1 * num2).ToString();
                sScript.ChangeTextTo(val);
            }
            else
            {
                // keep track of non solutions
                nonSolutionStars.Add(newstar);

                // initialize star to be a non conflicting wrong answer
                sScript.isSolution = false;
                string val = "";
                if(num2 == 0 && num1 == 0)
                {
                    val = ((i + num1) * (i+num2)).ToString();
                } else if(num2 == 0)
                {
                    val = (( num1) * (i + num2)).ToString();
                }
                {
                    val = ((num1 + i) * num2).ToString();
                }
                
                sScript.ChangeTextTo(val);

            }

            // move star to random location
            Vector3 newpos = new Vector3(Random.Range(-xBound, xBound), 0f, Random.Range(-zBound, zBound));
            float height = Random.Range(yLowBound, yUpBound);
            sScript.initiate(height, newpos);
        }


    }


    // Draws the constellation lines from question star to potential answer stars
    private void DrawConstellationLines()
    {
        // if game started
        if(gameState != 0 && solutionStarTransform != null)
        {
            LineRenderer lineRenderer = this.transform.GetComponentInChildren<LineRenderer>();
            if (lineRenderer != null)
            {
                int posind = 0;
                // allocates the correct amount of lines needed to draw the constellation
                lineRenderer.positionCount = 0;

                //draws a line from the question star to each of the non solution stars
                foreach (GameObject star in nonSolutionStars)
                {

                    lineRenderer.positionCount = lineRenderer.positionCount + 3;
                    lineRenderer.SetPosition(posind, questionStar.transform.position);
                    lineRenderer.SetPosition(posind + 1, star.transform.position);
                    lineRenderer.SetPosition(posind + 2,  questionStar.transform.position);
                    /*
                    lineRenderer.SetPosition(posind, this.linePositionAdjusted(star.transform.position, questionStar.transform.position));
                    lineRenderer.SetPosition(posind + 1, this.linePositionAdjusted(questionStar.transform.position, star.transform.position));
                    lineRenderer.SetPosition(posind + 2, this.linePositionAdjusted(star.transform.position, questionStar.transform.position));
                    *//*
                    posind = posind + 3;
                }


                lineRenderer.positionCount = lineRenderer.positionCount + 3;
                //draws a line from the question star to the solution star
                lineRenderer.SetPosition(posind, questionStar.transform.position);
                lineRenderer.SetPosition(posind + 1, solutionStarTransform.position);
                lineRenderer.SetPosition(posind + 2, questionStar.transform.position);

                posind = posind + 3;
                //draws the lines for previous answered stars
                foreach (GameObject completedStar in completedStars)
                {

                    lineRenderer.positionCount = lineRenderer.positionCount + 1;
                    lineRenderer.SetPosition(posind, completedStar.transform.position);
                    posind = posind + 1;
                }
            }
        }
        
    }

    private Vector3 linePositionAdjusted(Vector3 startStar, Vector3 endStar)
    {
        Vector3 direction = (startStar - endStar).normalized;
        float starHalfSize = (this.star.GetComponent<Renderer>().bounds.size.x)/2f;
        Vector3 newPos = endStar + (direction * starHalfSize);
        
        return newPos;
    }

    // Initializes the game to the given gameState
    private void StartGame(int gameState)
    {
        if(gameState != 0)
        {

            GameObject startStar = Instantiate(star);
            this.questionStar = startStar;
            StarScript sScript = (StarScript)startStar.GetComponent("StarScript");
            sScript.initiate(initialStarPos.y, initialStarPos);
            this.SelectStar(startStar);
            startStar.GetComponent<Renderer>().material = defaultMaterialStar;
            if(gameState == 1)
            {
                this.ConstructAnswerGrid(0, 10, 0, 10, (x, y) => x + y);
            }
            else if (gameState == 2)
            {
            }
            else if (gameState == 3)
            {
                this.ConstructAnswerGrid(1, 12, 1, 12, (x, y) => x * y);
            } else if (gameState == 4)
            {
            }
            //star.transform.position = initialStarPos;
        }
    }

    // selects an object if selectable and handles clicks on that object
    private void SelectObject()
    {
        //Initialize ray for object detection
        Ray mray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        var ray = Camera.main.transform.forward;
        RaycastHit hit;


        // if camera is pointing at an object
        if (Physics.Raycast(mray, out hit))
        {

            var selection = hit.transform;
            // if the object is a star object
            if (selection.CompareTag(starTag))
            {

                var selectionRenderer = selection.GetComponent<Renderer>();

                // if the renderer exists, highlight object
                if (selectionRenderer != null)
                {
                    selectionRenderer.material = highlightMaterialStar;
                }

                // select the star that the play is looking at
                if (Input.GetMouseButtonDown(0))
                {
                    StarScript script = (StarScript)selection.gameObject.GetComponent("StarScript");
                    if (script != null)
                    {
                        script.touch();
                        SelectStar(selection.gameObject);
                    }
                }

                // remember transform to deselect
                prevSelectedObjectTransform = selection;
            }

            // if the object is a UI start menu button
            else if (selection.CompareTag(UIStartMenuTag))
            {
                var selectionRenderer = selection.GetComponent<Renderer>();

                // if selection renderer exists, highlight object
                if (selectionRenderer != null)
                {
                    selectionRenderer.material = highlightMaterialUI;
                }

                // starts the game based off of the button selected
                if (Input.GetMouseButtonDown(0))
                {
                    UIStartScript script = (UIStartScript)selection.gameObject.GetComponent("UIStartScript");
                    if (script != null)
                    {
                        this.gameState = script.gameStateTrigger;
                        script.Select();
                        this.StartGame(gameState);
                    }
                }

                // remember transform to deselect
                prevSelectedObjectTransform = selection;
            }

        }
    }

    //resets previously selected objects to default material
    private void DeselectObject()
    {
        if (prevSelectedObjectTransform != null)
        {
            var selectionRenderer1 = prevSelectedObjectTransform.GetComponent<Renderer>();

            //resets object to respective default material based off of tag
            if (selectionRenderer1.CompareTag(starTag))
            {
                selectionRenderer1.material = defaultMaterialStar;
            }
            else if (selectionRenderer1.CompareTag(UIStartMenuTag))
            {
                selectionRenderer1.material = defaultMaterialUI;
            }

            prevSelectedObjectTransform = null;
        }
    }

    //creates an answer grid for the gamestate
    private void ConstructAnswerGrid(int xLow, int xHigh, int yLow, int yHigh, System.Func<int,int,int> operation)
    {
        /*
        answerGrid = new List<List<int>>();
        answerGrid.Capacity = yHigh;
        for (int y = yLow; y <= yHigh; y++)
        {
            answerGrid[y].Capacity = xHigh;
            for (int x = xLow; x <= xHigh; x++)
            {
                answerGrid[y][x] = operation(y, x);
            }
        }
        */
    //}
//}
