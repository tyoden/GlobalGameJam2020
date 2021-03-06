﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{

    public static GameController Instance { get; set; }

    [SerializeField] private Animator shipAnimator;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void GameOver()
    {
        //anim game over
        shipAnimator.SetTrigger("Fail");
    }

    public void Win()
    {
        //anim win
        shipAnimator.SetTrigger("Win");
    }
}
