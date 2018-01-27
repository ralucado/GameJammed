﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IAController : MonoBehaviour {

    private IAState state;
    private IndependentMovementController movement;
    private int unhappiness;
    private int hunger;
    private int urge;
    private int hungerStep;
    private int urgeStep;
    private Vector3 tablePosition;
    private float timer;
    private float foodTimer;
    private float bathroomTimer;


	// Use this for initialization
	void Start () {
        state = IAState.WAITING;
        hungerStep = 1;
        urgeStep = 1;
        tablePosition = GameObject.Find("ThirdPersonController").transform.position;
        timer = 0f;
        foodTimer = 0f;
        bathroomTimer = 0f;
        movement = GetComponent<IndependentMovementController>();

    }
	
	// Update is called once per frame
	void Update () {

        switch (state)
        {
            case IAState.WAITING:
                float luck = Random.Range(0f, 1f);
                if (timer >= 1)
                {
                    if (luck < 0.3)
                    {
                    hunger += 1;
                    }
                    else if (luck > 0.8)
                    {
                        urge += urgeStep;
                    }
                    if (hunger >= 3)
                    {
                        movement.GoToFood();
                        hunger = 0;
                        state = IAState.TO_FOOD;
                    }
                    else if (urge >= 3)
                    {
                        movement.GoToBathroom();
                        urge = 0;
                        state = IAState.TO_BATHROOM;
                    }
                    timer = 0f;
                }
                timer += Time.deltaTime;
                break;
            case IAState.TO_BATHROOM:
                break;
            case IAState.TO_FOOD:
                break;
            case IAState.TO_TABLE:
                movement.GoToTable(tablePosition);
                break;
            case IAState.FOOD:
                foodTimer += Time.deltaTime;
                if (foodTimer >= 2)
                {
                    state = IAState.TO_TABLE;
                }
                break;
            case IAState.BATHROOM:
                bathroomTimer += Time.deltaTime;
                if (bathroomTimer >= 2)
                {
                    state = IAState.TO_TABLE;
                }
                break;
        }
	}

    public void NotifyEndOfRoad()
    {
        switch (state)
        {
            case IAState.WAITING:
                break;
            case IAState.TO_BATHROOM:
                state = IAState.BATHROOM;
                bathroomTimer = 0f;
                break;
            case IAState.TO_FOOD:
                state = IAState.FOOD;
                foodTimer = 0f;
                break;
            case IAState.TO_TABLE:
                state = IAState.WAITING;
                break;
            case IAState.FOOD:
                break;
        }
    }




    public enum IAState
    {
        WAITING, TO_BATHROOM, TO_FOOD, TO_TABLE, BATHROOM, FOOD
    }
}
