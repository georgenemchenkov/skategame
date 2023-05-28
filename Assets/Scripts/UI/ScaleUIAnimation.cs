using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleUIAnimation : MonoBehaviour
{
    [SerializeField] private float duration = .25f;
    [SerializeField] private LeanTweenType tweenType;
    [SerializeField] private Vector3 scaleTo = Vector3.one;
    [SerializeField] private Vector3 initScale;
    [SerializeField] private bool customInitScale;

    private void Awake()
    {
        if(!customInitScale)
        initScale = transform.localScale;
    }

    private void OnEnable()
    {
        transform.localScale = Vector3.zero;
        LeanTween.scale(gameObject, scaleTo, duration).setEase(tweenType);
    }

    public void Hide(bool instant = false)
    {
        if (instant)
        {
            transform.localScale = Vector3.zero;
        }
        else
        {
            LeanTween.scale(gameObject, Vector3.zero, duration).setEase(tweenType).setOnComplete(OnHideComplete);
        }
    }

    public void Show()
    {
        LeanTween.scale(gameObject, initScale, duration).setEase(tweenType);
    }

    private void OnHideComplete()
    {
        gameObject.SetActive(false);
    }

    public void Close()
    {
        LeanTween.scale(gameObject, Vector3.zero, duration).setOnComplete(OnComplete).setEase(tweenType);
    }

    private void OnComplete()
    {
        Destroy(gameObject);
    }
}
