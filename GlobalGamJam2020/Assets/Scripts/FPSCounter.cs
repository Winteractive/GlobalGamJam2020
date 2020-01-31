using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class FPSCounter : MonoBehaviour
{
    float frameCount = 0f;
    float dt = 0.0f;
    
    public float fps = 0.0f;
    public float updateRate = 4.0f;  // 4 updates per sec.


    public Text text;
    public Text minFpsText;
    public Text maxFpsText;
    public static float minFPS;
    public static float maxFPS;

    const string MINFPSTEXT = "Min FPS: ";
    const string MAXFPSTEXT = "Max FPS: ";

    
    public int rowDataCount;
    private void Start()
    {

        CreateTextDisplay();
        maxFpsText.text = "Max FPS: ";
        minFpsText.text = "Min FPS: ";
        ResetMinMax();
        
    }
    public void Update()
    {
        frameCount++;
        dt += Time.deltaTime;
        
        if (dt > 1.0f / updateRate)
        {
            fps = frameCount / dt;
            frameCount = 0f;
            dt -= 1.0f / updateRate;
            text.text = fps + " FPS";
            if (fps > maxFPS)
            {
                maxFPS = fps;
                maxFpsText.text = MAXFPSTEXT + maxFPS;
            }
            if (fps < minFPS)
            {
                minFPS = fps;
                minFpsText.text = MINFPSTEXT + minFPS;
            }
            sumOfFps += fps;
            numberOfFPSObserved++;
            AvrageFPS = sumOfFps / numberOfFPSObserved;
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            ResetMinMax();
            ResetDisplay();
        }
        
    }


    static float sumOfFps = 0;
    static float numberOfFPSObserved = 0;

    public static float AvrageFPS;
    public static float GetAvrageFPS()
    {
        AvrageFPS = sumOfFps / numberOfFPSObserved;
        return sumOfFps / numberOfFPSObserved;
    }
    public static void ResetAvrageFPS()
    {
        sumOfFps = 0;
        numberOfFPSObserved = 0;
    }

    #region RESET VALUES FUNCTIONS
    private void ResetDisplay()
    {
        minFpsText.text = MINFPSTEXT + 0;
        maxFpsText.text = MAXFPSTEXT + 0;
    }

    public static void ResetMinMax()
    {
        maxFPS = Mathf.NegativeInfinity;
        minFPS = Mathf.Infinity;   
    }
    #endregion

    #region AUTO CREATING CANVAS AND TEXT FOR VALUES
    public void CreateTextDisplay()
    {
        GameObject gmCanvas = new GameObject();
        gmCanvas.name = "Testing Stats Display";
        Canvas canvas = gmCanvas.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        gmCanvas.AddComponent<CanvasScaler>();

        text = text ? text: CreateTextGameObject("FPS", gmCanvas.transform, new Vector3(0, 10, 0));
        minFpsText = minFpsText ? minFpsText : CreateTextGameObject("MIN FPS",gmCanvas.transform,new Vector3(0, 50,0));
        maxFpsText = maxFpsText ? maxFpsText : CreateTextGameObject("Max FPS", gmCanvas.transform,new Vector3(0, 90,0));
    }
    Text CreateTextGameObject(string name, Transform parent, Vector3 position)
    {
        Font defaultFont = Resources.GetBuiltinResource<Font>("Arial.ttf");
        GameObject gmText = new GameObject(name);
        gmText.transform.parent = parent;
        gmText.transform.localPosition = Vector3.zero;
        Text textComp = gmText.AddComponent<Text>();
        textComp.resizeTextForBestFit = true;
        textComp.font = defaultFont;

        RectTransform rectTransform = gmText.GetComponent<RectTransform>();
        rectTransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, position.x, 760);
        rectTransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Top, position.y, 40);

        return textComp;
    }
    #endregion
}