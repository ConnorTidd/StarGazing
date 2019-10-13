using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarScript : MonoBehaviour
{
    public bool touched = false;
    public bool isSolution = true;
    public Material removedMaterial;
    public Material retiredMaterial;
   
    public Vector3 targetPos = new Vector3(0f,7f,0f);
    private bool removed = false;
    public float minimizeFactor = 1f;
    public GameObject fromStar;
    // Start is called before the first frame update
    void Start()
    {
        
        
    }

    // Update is called once per frame
    void Update()
    {
        Shrink();
        Minimize();
        FaceTextTowardsPlayer();
        this.transform.position = Vector3.MoveTowards(this.transform.position, targetPos, .04f);
        setLinePositions();

        

    }
    


    public void touch()
    {
        touched = true;
        //SpawnStars();
    }

    public void initiate(float height, Vector3 pos, GameObject questionStar)
    {
        this.transform.position = pos;
        this.targetPos = new Vector3(pos.x, height, pos.z);
        this.fromStar = questionStar;
    }

    public void ChangeTextTo(string str)
    {
        TextMesh textMesh = GetComponentInChildren<TextMesh>();
        textMesh.text = str;
    }

    public void Finish()
    {
        this.ChangeTextTo("");
        this.isSolution = false;
        this.transform.tag = "Untagged";
        this.GetComponent<Renderer>().material = retiredMaterial;
    }
    public void Shrink()
    {
        if(this.removed)
        {
            if(this.transform.localScale.x > .1)
            {
                this.transform.localScale = this.transform.localScale - new Vector3(.005f, .005f, .005f);
            } else
            {
                Destroy(this.gameObject);
            }
        }
    }

    public void Remove()
    {
        this.removed = true;
        this.transform.tag = "Untagged";
        this.ChangeTextTo("");
        Renderer renderer =  this.transform.GetComponent<Renderer>();
        renderer.material = removedMaterial;
      
    }

    private void FaceTextTowardsPlayer()
    {


        Vector3 playerPos = new Vector3(0f, 1.74f, 0f);
        Vector3 thisPos = this.transform.position;

        Vector3 direction = (this.transform.position - playerPos).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        Transform textObj = this.gameObject.transform.GetChild(1);
        textObj.transform.position = thisPos;
        textObj.transform.rotation = Quaternion.Slerp(textObj.transform.rotation, lookRotation, 0.5f);
        textObj.transform.Translate(new Vector3(0f, 0f, -0.3f), Space.Self);
    }

    private void setLinePositions()
    {
        if(fromStar != null)
        {

            LineRenderer lineRenderer;
            Vector3 qStarPos;
            Vector3 oStarPos;

            lineRenderer = this.gameObject.GetComponentInChildren<LineRenderer>();
            qStarPos = fromStar.transform.position;
            oStarPos = this.transform.position;
            lineRenderer.SetPosition(0, LinePositionAdjusted(qStarPos, oStarPos));
            lineRenderer.SetPosition(1, LinePositionAdjusted(oStarPos, qStarPos));
        }
    }

    private Vector3 LinePositionAdjusted(Vector3 startStar, Vector3 endStar)
    {
        Vector3 direction = (startStar - endStar).normalized;
        float starHalfSize = (this.GetComponent<Renderer>().bounds.size.x) / 2f;
        Vector3 newPos = endStar + (direction * starHalfSize);

        return newPos;
    }

    public void Minimize()
    {
        if(this.transform.localScale.x > this.minimizeFactor)
        {
            this.transform.localScale = this.transform.localScale - new Vector3(.1f, .1f, .1f);
        }
    }


}
