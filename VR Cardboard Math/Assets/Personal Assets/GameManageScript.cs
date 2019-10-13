using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManageScript : MonoBehaviour
{

    // Materials for selecting objects
    public Material highlightMaterialStar;
    public Material defaultMaterialStar;
    public Material highlightMaterialUI;
    public Material defaultMaterialUI;

    //Where the first star spawns
    public Vector3 initialStarPos = new Vector3(5f, 2f, 5f);

    // Amount of stars spawned per question
    public int newstars = 5;
    // GameObject prefab for spawning stars
    public GameObject star;
    // GameObject prefab for testing if space is valid
    public GameObject feelerStar;
    // GameObject prefab for constellation lines
    public GameObject constellationLine;
    // Question numbers
    private int num1;
    private int num2;
    private bool testWaitbool = false;
    // Spacial bounds where stars are allowed to spawn
    float yUpBound = 7;
    float yLowBound = 1;
    float xBound = 9;
    float zBound = 9;

    //bounds of multiplication
    int multLowBound = 1;
    int multHighBound = 12;

    private GameObject solutionStar;
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
    private int gameState = 0;
    private int questionsPerGame = 15;
    private int answeredQuestions = 0;
    private int wrongAnswersPerGame = 3;
    private int wrongAnswers = 0;
    
    private Transform prevSelectedObjectTransform;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        DeselectObject();
        SelectObject();    
    }

    private void GameEnd(bool gameWon)
    {
        if (gameWon)
        {
            Debug.Log("Game Won!");
            FinalImageCreationScript fScript = this.GetComponent<FinalImageCreationScript>();
            fScript.moveToFinalImage(15, completedStars);
        }
        else
        {
            Debug.Log("Game Lost :(");
        }
    }

    // Handles selecting a star
    public void SelectStar(GameObject selectedObj)
    {
        StarScript sScript = (StarScript)selectedObj.GetComponent("StarScript");
        // Selected star is the solution
        if (sScript.isSolution)
        {
            
            // remove false answers
            foreach (GameObject star in nonSolutionStars)
            {
                Destroy(star);
            }
            nonSolutionStars = new List<GameObject>();
            this.Retire(questionStar);
            completedStars.Add(selectedObj);

            // transition answer star to question star
            selectedObj.tag = "Untagged";
            questionStar = selectedObj;
            sScript.isSolution = false;

            // increment number of correctly answered questions and check if game won
            answeredQuestions = answeredQuestions + 1;
            if (answeredQuestions == questionsPerGame)
            {
                this.GameEnd(true);
                this.Retire(questionStar);
            }
            //if game not one, create a new question and spawn new stars
            else
            {
                num1 = (int)Random.Range(multLowBound, multHighBound);
                num2 = (int)Random.Range(multLowBound, multHighBound);
                string newVal = num1.ToString() + " x " + num2.ToString();
                sScript.ChangeTextTo(newVal);
                SpawnStars();
            }

        }

        // selected star was not the correct answer
        else
        {
            // removese the star
            sScript.Remove();
            this.nonSolutionStars.Remove(selectedObj);

            // increments the number of wrongly guessed stars
            wrongAnswers = wrongAnswers + 1;
            
            // end game if too many incorrect answers
            if (wrongAnswers == wrongAnswersPerGame)
            {
                this.GameEnd(false);
            }
        }
    }

    private void Retire(GameObject star)
    {
        StarScript sScript = (StarScript)star.GetComponent("StarScript");
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
                solutionStar = newstar;

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
                string val = this.CreateFalseSolution(i);
                

                sScript.ChangeTextTo(val);

            }

            setStarPos(sScript);
        }


    }

    // moves star to random location where it does not overlap with an object
    // may pass through objects to get to position
    private void setStarPos(StarScript sScript)
    {
        StartCoroutine(feelAndPlace(sScript));    
    
            
    }

    IEnumerator feelAndPlace(StarScript sScript)
    {
        bool isOverlapping = true;
        float xPos = 1f;
        float yPos = 1f;
        float zPos = 1f;
        while (isOverlapping)
        {
            xPos = Random.Range(-xBound, xBound);
            yPos = Random.Range(yLowBound, yUpBound);
            zPos = Random.Range(-zBound, zBound);
            Vector3 endPos = new Vector3(xPos, yPos, zPos);
            GameObject feeler = Instantiate(feelerStar);
            feeler.transform.position = endPos;
            yield return new WaitForSeconds(0.1f);
            if (!feeler.GetComponent<FeelerStarScript>().hasCollided)
            {
                isOverlapping = false;
            }
            testWaitbool = false;
            Destroy(feeler);
        }
        if (!isOverlapping)
        {
            Vector3 startPos = new Vector3(xPos, 0f, zPos);
            sScript.initiate(yPos, startPos, questionStar);
            sScript.touched = true;
        }
    }

    // creates a non conflicting false solution, index starts at 1, index must be <= HighBound - LowBound
    private string CreateFalseSolution(int index)
    {
        // 
        int netmove = (int) Mathf.Ceil(((float)index / 2f));
        if (num1 + netmove > multHighBound)
        {
            netmove = - (index - multHighBound - num1);
        }
        else if (num1 - netmove < multLowBound)
        {
            netmove = index + num1 - multLowBound;
        }
        else if (index % 2 == 0) //if index is even move left
        {
            netmove = -netmove;
        }

        int falseSolution = (num1 + netmove)*num2;
        
        return falseSolution.ToString();
    }

    

    // Initializes the game to the given gameState
    private void StartGame(int gameState)
    {
        if (gameState != 0)
        {

            GameObject startStar = Instantiate(star);
            this.questionStar = startStar;
            StarScript sScript = (StarScript)startStar.GetComponent("StarScript");
            sScript.initiate(initialStarPos.y, initialStarPos, questionStar);
            this.SelectStar(startStar);
            startStar.GetComponent<Renderer>().material = defaultMaterialStar;
            if (gameState == 1)
            {
               
            }
            else if (gameState == 2)
            {
            }
            else if (gameState == 3)
            {
            }
            else if (gameState == 4)
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

    
}
