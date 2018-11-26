using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelPortalEffect : MonoBehaviour
{

    public ParticleSystem portalPS;
    public List<Renderer> cubes;

    Color _cubeEmisColor = new Color(0, 0.6f, 0.05f);
    int _cubeIndex;

	void Start ()
    {
	    
	}
	
	void Update () {
	}

    public void UpdateGoal(int livesRemoved, int[] objetives)
    {
        if (livesRemoved >= objetives[objetives.Length - 1])
        {
            SetEmissionColor(_cubeEmisColor, cubes[cubes.Count - 1].material);
            return;
        }

        if(livesRemoved >= objetives[0] && livesRemoved < objetives[1])
            SetEmissionColor(_cubeEmisColor, cubes[0].material);
        else if(livesRemoved >= objetives[1] && livesRemoved < objetives[2])
            SetEmissionColor(_cubeEmisColor, cubes[1].material);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == LayerMask.NameToLayer("Minion"))
        {
            portalPS.Emit(10);
        }
    }

    void SetEmissionColor(Color c, Material mat)
    {
        mat.SetColor("_EmissionColor", c);
    }
}
