﻿using UnityEngine;
using System.Collections;

public class Ball : MonoBehaviour {

    public Vector3 velocity;
    public GameObject beamPrefab;
    public Timing[] beamTimings;

    Vector3 initialPosition, initialVelocity;

    int shotMusicalTime;
    int beamIndex;

    public void Initialize()
    {
        shotMusicalTime = 0;
        beamIndex = 0;
        transform.position = initialPosition;
        velocity = initialVelocity;
    }

	// Use this for initialization
	void Start () {
        initialPosition = transform.position;
        initialVelocity = velocity;
	}
	
	// Update is called once per frame
	void Update () {
        if( Music.CurrentSection.name.StartsWith( "Play" ) )
        {
            UpdateShot();

            if( shotMusicalTime > 0 )
            {
                if( Music.isJustChanged )
                {
                    --shotMusicalTime;
                }
            }
            else
            {
                UpdatePosition();
            }
        }
	}

    void UpdateShot()
    {
        if( beamIndex < beamTimings.Length && Music.isNowChanged
            && ( beamTimings[beamIndex].totalUnit - 4 == Music.Now.totalUnit ) )
        {
            Beam beam = (Instantiate( beamPrefab ) as GameObject).GetComponent<Beam>();
            beam.transform.parent = transform;
            beam.transform.localPosition = Vector3.zero;
            beam.Initialize( beamTimings[beamIndex] );
            ++beamIndex;
        }
    }

    void UpdatePosition()
    {
        transform.position += velocity * Time.deltaTime;
        if( transform.position.x <= -Field.instance.fieldLength || Field.instance.fieldLength <= transform.position.x )
        {
            velocity.x = Mathf.Abs( velocity.x ) * -Mathf.Sign( transform.position.x );
            Music.QuantizePlay( audio );
        }
        if( Field.instance.fieldLength <= transform.position.y )
        {
            velocity.y = Mathf.Abs( velocity.y ) * -Mathf.Sign( transform.position.y );
            Music.QuantizePlay( audio, 7 );
        }
        else if( transform.position.y <= -Field.instance.fieldLength )
        {
            Music.SeekToSection( "GameOver" );
        }
    }

    public void OnShot()
    {
        if( Music.CurrentSection.name == "Play2" )
        {
            shotMusicalTime = 2;
        }
        else
        {
            shotMusicalTime = 3;
        }
    }
}
