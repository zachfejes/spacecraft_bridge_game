using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Torpedo : MonoBehaviour
{

    private float fuseTime = 3.0f;
    private float fuseStartTime;
    private bool effectActive = false;


    // Use this for initialization
    void Awake()
    {
        fuseStartTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if (!effectActive && Time.time - fuseStartTime >= fuseTime)
        {
            Activate();
        }
    }

    public void EffectActive(bool value) {
        effectActive = value;
    }

    public bool EffectActive() {
        return(effectActive);
    }

    public void FuseStartTime(float value) {
        fuseStartTime = value;
    }

    public float FuseStartTime() {
        return(fuseStartTime);
    }

    public void FuseTime(float value) {
        fuseTime = value;
    }

    public float FuseTime() {
        return(fuseTime);
    }

    /// <summary>
    /// This method is meant to be overwritten by child classes
    /// </summary>
    public void Activate()
    {
        effectActive = true;
    }

}
