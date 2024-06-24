using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class PlacingTurret : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        var renderers = GetComponentsInChildren<MeshRenderer>();
        foreach (var renderer in renderers)
        {
            StartCoroutine(FadeTurret(renderer));
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator FadeTurret(MeshRenderer renderer)
    {
        while (GameManager.instance.placingTurret)
        {
            renderer.enabled = true;
            yield return new WaitForSeconds(0.33f);
            renderer.enabled = false;
            yield return new WaitForSeconds(0.33f);
        }
        renderer.enabled = true;
    }
}
