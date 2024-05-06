using UnityEngine;
using System.Collections;

public class UnscaledParticleTime : MonoBehaviour {
	ParticleSystem m_particle;
	// Use this for initialization
	void Start () 
	{
		m_particle = this.GetComponent<ParticleSystem>();
	}
	
	// Update is called once per frame
	void Update ()
	{
		m_particle.Simulate(Time.unscaledDeltaTime, true, false);
	}
}
