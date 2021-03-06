﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class FogWeather : MonoBehaviour, IWeather
{

    [SerializeField] private ChildController wheelChild;
    [SerializeField] private ChildController rotorChild;
    [SerializeField] private PostProcessVolume postProcessVolume;

    private ShipController shipController;

    public void EndWeather()
    {
        AudioManager.Instance.FogSoundPlay(false);
        Debug.Log(this.name + " is end");
        if ((wheelChild.ProblemPregress >= 30 && wheelChild.ProblemPregress <= 60)
            && rotorChild.ProblemPregress >= 0 && rotorChild.ProblemPregress <= 30)
        {
            Debug.Log("All is good");
        }
        else
        {
            Debug.Log("Ship take a 1 damage");
            shipController.SubHPPoint();
        }
        StartCoroutine(ChangeValue(1, 0.6f, 0, false));
    }

    public void StartWeather()
    {
        Debug.Log(this.name + " is start");
        AudioManager.Instance.FogSoundPlay(true);
        StartCoroutine(ChangeValue(1, 0, 0.6f, true));
    }

    private IEnumerator ChangeValue(float time, float from, float to, bool state)
    {
        if (state)
        {
            postProcessVolume.gameObject.SetActive(true);
        }
        Vignette vignette;
        if (postProcessVolume.profile.TryGetSettings(out vignette))
        {
            var startTime = Time.time;
            var finishTime = Time.time + time;
            while (Time.time < finishTime)
            {
                var t = 1f / time * (Time.time - startTime);
                var l = Mathf.Lerp(from, to, t);
                vignette.intensity.value = l;
                yield return null;
            }
            vignette.intensity.value = to;
        }
        if (!state)
        {
            postProcessVolume.gameObject.SetActive(false);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        shipController = FindObjectOfType<ShipController>();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
