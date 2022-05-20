using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class BackgroundManager : MonoBehaviour
{
    public Animator blackScreenAnimator;
    public Image blackScreen;
    public GameObject[] backgrounds;
    public GameObject currentBackground;
    public Transform backgroundPositionMarker;
    public static BackgroundManager instance;
    private bool transitioningIn, transitioningOut;
    private float transitionTime, transitionTimer;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Debug.LogWarning("There is more than one BackgroundManager");
        }
    }

    public void ChangeBackground(int index, float transitionTime)
    {
        if (currentBackground != null) Destroy(currentBackground);
        TransitionOut(transitionTime);
        currentBackground = Instantiate(backgrounds[index]);
        currentBackground.transform.position = backgroundPositionMarker.position;
    }

    private void TransitionOut(float transitionTime)
    {
        if (transitioningOut) return;
        this.transitionTime = transitionTime / 2;
        transitionTimer = this.transitionTime;
        //blackScreenAnimator.SetTrigger("TransitionOut");

        transitioningOut = true;
        blackScreen.raycastTarget = true;
    }


    private void TransitionIn()
    {
        if (transitioningIn) return;
        //blackScreenAnimator.SetTrigger("TransitionIn");
        transitioningOut = false;
        transitionTimer = transitionTime;
        transitioningIn = true;
        blackScreen.raycastTarget = false;
    }

    private void Update()
    {
        if (transitionTimer > 0)
        {
            transitionTimer -= Time.deltaTime;
            if (transitioningOut)
            {
                blackScreen.raycastTarget = true;
                blackScreen.color = new Color(blackScreen.color.r,
                    blackScreen.color.g, blackScreen.color.b, 1 / (transitionTime*2/transitionTimer));
            }
        }
        else if(transitioningIn)
        {
            blackScreen.raycastTarget = true;
            blackScreen.color = new Color(blackScreen.color.r,
                blackScreen.color.g, blackScreen.color.b, (transitionTimer / transitionTime / 2));
        }

        else
        {
            if(transitioningOut)
            {
                TransitionIn();
            }
            else if(transitioningIn)
            {
                transitioningIn = false;
            }
        }
    }
}
