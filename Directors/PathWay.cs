using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathWay : MonoBehaviour
{
    public bool Go = false;
    public bool Stop = false;
    public bool TestNext = false;
    // Transforms to act as start and end markers for the journey.
    public List<Transform> Path;

    // Movement speed in units per second.
    public float speed = 1.0F;


    // Transforms to act as start and end markers for the journey.
    private Transform startMarker;
    private Transform endMarker;

    // Time when the movement started.
    private float startTime;

    // Total distance between the markers.
    private float journeyLength;

    //Index
    private int indexPath = 0;

    //Character target
    private Character target;


    private bool Enable = false;

    private bool goNext = false;

    private bool hasFinish = false;



    void Start()
    {
        target = GetComponent<Character>();
    }

    void StartMoving()
    {
        Enable = true;
        if (indexPath == 0)
        {
            Transform startWayPoint = Instantiate(Path[indexPath], transform.position, Quaternion.identity);
            Path.Insert(indexPath, startWayPoint.transform);
        }
        init();
    }

    void StopMoving()
    {
        Enable = false;
    }

    void GoNext()
    {
        goNext = true;
        StartMoving();
    }

    void Update()
    {
        if (Go)
        {
            Go = false;
            StartMoving();  
        }
        if (TestNext)
        {
            TestNext = false;
            GoNext();
        }
        if (Stop)
        {
            Stop = false;
            StopMoving();
        }
        if (Enable)
        {
            if (indexPath < Path.Count - 1)
            {
                MoveTowardPoint();
            }
            else
            {
                Enable = false;
            }
        }
        
    }

    void init()
    {
        if(indexPath < Path.Count - 1)
        {
            startMarker = Path[indexPath];
            endMarker = Path[indexPath + 1];

            target.SetLookAtTargetTransform(endMarker);

            // Keep a note of the time the movement started.
            startTime = Time.time;

            // Calculate the journey length.
            journeyLength = Vector3.Distance(startMarker.position, endMarker.position);
        }
        else
        {
            hasFinish = true;
        }
        
    }

    // Move to the target end position.
    void MoveTowardPoint()
    {

        // Distance moved equals elapsed time times speed..
        float distCovered = (Time.time - startTime) * speed;

        // Fraction of journey completed equals current distance divided by total distance.
        float fractionOfJourney = distCovered / journeyLength;
        
        if (transform.position == Path[indexPath+1].position)
        {
            indexPath++;
            init();
            distCovered = (Time.time - startTime) * speed;
            fractionOfJourney = distCovered / journeyLength;
            if (goNext)
            {
                Enable = false;
                goNext = false;
                return;
            }
            
        }

        // Set our position as a fraction of the distance between the markers.
        transform.position = Vector3.Lerp(startMarker.position, endMarker.position, fractionOfJourney);
    }
}
