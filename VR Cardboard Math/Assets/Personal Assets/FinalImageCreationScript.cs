using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalImageCreationScript : MonoBehaviour
{
    private Dictionary<string, List<Vector3>> images = new Dictionary<string, List<Vector3>>();
    private Dictionary<int, List<string>> pointsToCompatibleImage = new Dictionary<int, List<string>>();
    // Start is called before the first frame update
    void Start()
    {
        createDictionary();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void createDictionary()
    {
        images.Add("hummingbird", this.HummingBirdList15());
        List<string> pointImages15 = new List<string>();
        pointImages15.Add("hummingbird");
        pointsToCompatibleImage.Add(15, pointImages15);
    }

    private List<Vector3> HummingBirdList15()
    {
        List<Vector3> points = new List<Vector3>();
        points.Add(new Vector3(-0.26f,  10f, -2.7f));   // 1
        points.Add(new Vector3(-4.76f,  10f, -2.7f));   // 2
        points.Add(new Vector3(-10.91f, 10f,  0.45f));  // 3
        points.Add(new Vector3(-3.95f,  10f,  0.5f));   // 4
        points.Add(new Vector3(-0.19f,  10f, -1.77f));  // 5
        points.Add(new Vector3(-2.05f,  10f,  3.4f));   // 6
        points.Add(new Vector3(0.3f,    10f,  8.7f));   // 7
        points.Add(new Vector3(3f,      10f, -4.7f));   // 8
        points.Add(new Vector3(-7.3f,   10f, -8f));     // 9
        points.Add(new Vector3(-3.6f,   10f, -9.3f));   // 10
        points.Add(new Vector3(3.08f,   10f,  1.8f));   // 11
        points.Add(new Vector3(5.96f,   10f,  0.23f));  // 12
        points.Add(new Vector3(10.3f,   10f, -0.7f));   // 13
        points.Add(new Vector3(6.4f,    10f, -1.2f));   // 14
        points.Add(new Vector3(3.5f,    10f, -3.3f));   // 15
        return points;
    }

    public void moveToFinalImage(int amount, List<GameObject> stars)
    {
        List<string> imageKeys = this.pointsToCompatibleImage[amount];
        string imageToDisplay = imageKeys[Random.Range(0, imageKeys.Count - 1)];
        List<Vector3> imagePoints = this.alterImage(.4f, .4f, this.images[imageToDisplay]);
        for(int i = 0; i < imagePoints.Count; i++)
        {
            StarScript sScript = stars[i].GetComponent<StarScript>();
            sScript.targetPos = imagePoints[i];
            sScript.minimizeFactor = .5f;
        }
    }

    private List<Vector3> alterImage(float xFactor, float zFactor, List<Vector3> points)
    {
        List<Vector3> newImage = new List<Vector3>();
        foreach(Vector3 point in points)
        {
            newImage.Add(new Vector3(point.x * xFactor, point.y, point.z * zFactor));
        }
        return newImage;
    }
}
