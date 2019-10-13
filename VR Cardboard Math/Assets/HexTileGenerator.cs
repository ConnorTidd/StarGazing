using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexTileGenerator : MonoBehaviour
{
    public GameObject hexTilePrefab;
    public GameObject pentaTilePrefab;
    

    Vector3 hexsize = new Vector3();
    //important values of a dode angle in radians
    float phi = Mathf.Acos(-1 / Mathf.Sqrt(5));
    float phiSupplementary = Mathf.PI - Mathf.Acos(-1 / Mathf.Sqrt(5));
    float r = 0;
    float s = 0;
    
    List<GameObject> botpents = new List<GameObject>();
    List<GameObject> toppents = new List<GameObject>();
    public int layers = 1;

    // Start is called before the first frame update
    void Start()
    {
        CreateLeDode();
        ExpandPents();
        AddHexes();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //adds the hexagons
    void AddHexes()
    {
        //the number of hexagons in full rings
        float hexesinrings = 5f*Mathf.Pow(2f, (float)(layers - 1));

        //incremental tilt of hexes
        float b = phiSupplementary / (1 + layers);
        

        for (int i = 0; i < hexesinrings; i++)
        {
            GameObject hex1 = Instantiate(hexTilePrefab);
            hexsize = hex1.GetComponent<MeshCollider>().bounds.size;
            hex1.transform.parent = botpents[0].transform;
            hex1.transform.position = botpents[0].transform.position;
            hex1.transform.Rotate(0f, 0f, Mathf.Rad2Deg*b);
            hex1.transform.Translate((s) + ((hexsize.x / 2f) * Mathf.Cos(b)), (hexsize.x / 2f) * Mathf.Sin(b), 0f, Space.World);
            hex1.transform.RotateAround(botpents[0].transform.position, Vector3.up, 72 * i);
        }
        /*
        for (int i = 0; i < layers*5; i++)
        {
            GameObject hex1 = Instantiate(hexTilePrefab);
            hexsize = hex1.GetComponent<MeshCollider>().bounds.size;
            hex1.transform.parent = botpents[0].transform;
            hex1.transform.position = botpents[0].transform.position;
            hex1.transform.Rotate(0, 90, 0, Space.Self);
            hex1.transform.Rotate(0, 0, 121.717f, Space.World);
            hex1.transform.Translate(-.5f, 0, 0, Space.World);

        }*/
    }
    //pushes the pentagons out by an amount proportional to the hexes added`
    void ExpandPents()
    {
        float b = phiSupplementary / (1 + layers);
        GameObject hex1 = Instantiate(hexTilePrefab);
        //Expands based off the size of the hexagon
        Vector3 hexsize = hex1.GetComponent<MeshCollider>().bounds.size;
        Destroy(hex1);
        float upexpanddist = 0;
        float outexpanddist = 0;
        for (int i = 1; i < layers + 1; i++)
        {
            upexpanddist = upexpanddist + (hexsize.x * Mathf.Cos((Mathf.PI / 2) - (b*i)));
            outexpanddist = outexpanddist + (hexsize.x * Mathf.Sin((Mathf.PI / 2) - (b * i)));

        }
        Debug.Log(upexpanddist);
        botpents[0].transform.Translate(0f, upexpanddist, 0f,Space.Self);
        toppents[0].transform.Translate(0f, upexpanddist, 0f, Space.Self);
        for (int i = 1; i < 6; i++)
        {
            float angle = (i - 1) * Mathf.Deg2Rad * 72f;
            float distance = 1;
            botpents[i].transform.Translate(outexpanddist*Mathf.Cos(angle), 0f, outexpanddist * Mathf.Sin(-angle), Space.World);
        }
    }
    //Creates a hex tile map
    void CreateLeDode()
    {
        //
        //get this position
        Vector3 thispos = transform.position;

        //create an empty object to house half of the dode
        GameObject half = new GameObject();
        half.transform.parent = this.transform;
        half.transform.position = thispos;

        //create the base of the dode and get its size for calculations
        GameObject pent1 = Instantiate(pentaTilePrefab);
        botpents.Add(pent1);
        pent1.transform.parent = half.transform;
        pent1.transform.position = new Vector3(thispos.x, thispos.y, thispos.z);
        pent1.transform.Rotate(180f, 0f, 0f);
        Vector3 pentsize = pent1.GetComponent<MeshCollider>().bounds.size;

        
        //distance from center to bottom side
        s = pentsize.x*(1f-(1f/(1+Mathf.Cos(36f*Mathf.PI/180f))));
        //distance from center to top vertex
        r = pentsize.x / (1f + Mathf.Cos(36f * Mathf.PI / 180f));
        //size of an edge
        float edgeSize = 2f * r * Mathf.Cos(Mathf.PI*54/180);

        float xoffset = s * Mathf.Cos(phiSupplementary) + s;
        float yoffset = s * Mathf.Sin(phiSupplementary);




        
        for (int pentagon = 0; pentagon < 5; pentagon++)
        {
            GameObject pent = Instantiate(pentaTilePrefab);
            botpents.Add(pent);
            //put child on parent
            pent.transform.parent = half.transform;
            //offset
            pent.transform.position = new Vector3(thispos.x + xoffset, yoffset + thispos.y, thispos.z);
            //do tilt
            pent.transform.Rotate(0, 0, -116.5606f, Space.Self);
            //place at respective edge out of 5
            pent.transform.RotateAround(thispos, Vector3.up, 72*pentagon);

        }


        //copy the half dode
        GameObject half2 = Instantiate(half);
        for (int pentagon = 0; pentagon < 6; pentagon++)
        {
            toppents.Add(half2.transform.GetChild(pentagon).gameObject);
        }
        half2.transform.parent = this.transform;
        half2.transform.position = thispos;

        //flip and rotate to create top
        half2.transform.Rotate(180, 0, 0, Space.Self);

        float fauxHalfHeight = (1f / 2f) * pentsize.x * Mathf.Sin(phiSupplementary);
        float edgePointHeight = edgeSize * Mathf.Sin((Mathf.PI * 72f) / 180f);
        float yHeight = edgePointHeight * Mathf.Sin(phiSupplementary);
        half2.transform.RotateAround(thispos, Vector3.up, 180f);
        half2.transform.Translate(0f, -2f * fauxHalfHeight, 0f);
        half2.transform.Translate(0f, -yHeight, 0f);

        
    }
}
