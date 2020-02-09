using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleAutoDestroy : MonoBehaviour
{
    private ParticleSystem particle;
    private bool checkAlive = false;
    void Start()
    {
        particle = GetComponent<ParticleSystem>();
        StartAutoDestroy();
    }

    // Update is called once per frame
    void Update()
    {
        if (particle) {
            if (!particle.IsAlive () && checkAlive) {
                Destroy (gameObject);
            }
        } 
    }

    public void StartAutoDestroy()
    {
        this.gameObject.SetActive(true);
        checkAlive = true;
    }
}
