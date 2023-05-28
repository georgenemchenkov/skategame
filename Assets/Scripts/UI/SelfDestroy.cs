using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDestroy : MonoBehaviour
{
    public float duration;

    private void Start()
    {
        StartCoroutine(IEDestroy());
    }

    private IEnumerator IEDestroy()
    {
        yield return new WaitForSeconds(duration);
        Destroy(gameObject);
    }
}
