using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FeelerStarScript : MonoBehaviour
{
    public bool hasCollided = false;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider collider)
    {
        Physics.IgnoreCollision(collider.GetComponent<Collider>(), this.GetComponent<Collider>(), true);
        this.hasCollided = true;

        Physics.IgnoreCollision(collider.GetComponent<Collider>(), this.GetComponent<Collider>(), false);
    }
}
