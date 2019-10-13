using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIStartScript : MonoBehaviour
{
    public int gameStateTrigger = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Select()
    {
        if(this.gameStateTrigger != 0)
        {
            Destroy(this.transform.parent.gameObject);
        }
    }
}
