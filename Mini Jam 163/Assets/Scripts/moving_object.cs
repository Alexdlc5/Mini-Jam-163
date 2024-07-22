using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class moving_object : MonoBehaviour
{
    public float speed;
    public Vector2[] points;
    private int index = 0;
    Vector2 current_target_point;

    private void Start()
    {
        current_target_point = points[index];
    }
    // Update is called once per frame
    void Update()
    {
        //change target point
        if ((Vector2) transform.position  == current_target_point) //reach point
        { 
            index++;
            if (index == points.Length)
            {
                index = 0;
            }
            current_target_point = points[index];
        }
        //(Update Position) speed divided by 10 to make number in editor easier to read/tweak
        transform.position = Vector2.MoveTowards(transform.position, current_target_point, speed / 10 * Time.deltaTime); 
        
    }
}
