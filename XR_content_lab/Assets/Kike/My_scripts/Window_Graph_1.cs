using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CodeMonkey.Utils;

/*
using brainflow;
using brainflow.math;
*/

public class Window_Graph_1 : MonoBehaviour
{
    [SerializeField] private Sprite circleSprite;
    private RectTransform graphContainer;

    //private BoardShim board_shim = null;

    private int sampling_rate = 0;


    //private float waitTime = 500.0f;
    //private float scrollBar = 50.0f;
    //private float timer = 0.0f;

    //private float waitTime = 500.0f;
    //private float scrollBar = 0.5f;
    //private float timer = 0.0f;

    private float waitTime = .1f;
    private float timer = 0.0f;
    //private float visualTime = 0.0f;
    //private int width, height;
    //private float value = 10.0f;
    private float scrollBar = 1.0f;
    //private float visualTime = 0.0f;

    List<int> dt_1 = new List<int>() { };

    private void Awake() {
        graphContainer = transform.Find("graphContainer").GetComponent<RectTransform>();
        //CreateCircle(new Vector2(200,100));

        List<int> valueList = new List <int>(){10,56,56,45,30,22,17,15,13,17,25,37,40,36,33};
        //ShowGraph(valueList);

        Time.timeScale = scrollBar;
    }
    /*
    void Start(){
        try
        {
            //BoardShim.set_log_file("brainflow_log.txt");
            //BoardShim.enable_dev_board_logger();
            
            BrainFlowInputParams input_params = new BrainFlowInputParams();
            int board_id = (int)BoardIds.SYNTHETIC_BOARD;
            board_shim = new BoardShim(board_id, input_params);
            board_shim.prepare_session();
            board_shim.start_stream(450000, "file://brainflow_data.csv:r");
            sampling_rate = BoardShim.get_sampling_rate(board_id);
            Debug.Log("Brainflow streaming was started");
            
        }
        catch (BrainFlowException e)
        {
            Debug.Log(e);
        }
    }
    */

    // Update is called once per frame
    void Update()
    {
        /*
        if (board_shim == null)
        {
            return;
        }
        */
        int number_of_data_points = sampling_rate * 4;
        double[,] data = Window_Graph.data;
        // check https://brainflow.readthedocs.io/en/stable/index.html for api ref and more code samples
        Debug.Log("Num elements: " + data.GetLength(1));
        
        for (int i=0;i<data.GetLength(0);i++){
            dt_1.Add((int)data[9,i]);    
        }

        //print(dt.GetLength());
        

        timer += Time.deltaTime;
        print("asdfasdfasdf"+timer);

        

        if (timer > waitTime)
        {
            //visualTime = timer;
            timer = timer - waitTime;
            Time.timeScale = scrollBar;
            ShowGraph(dt_1);
        }
        dt_1.Clear();
        
        
        

        

        
        
        //graphContainer = transform.Find("graphContainer").GetComponent<RectTransform>();
        //print(data.GetLength(1));

    }
    // you need to call release_session and ensure that all resources correctly released
    /*
    private void OnDestroy()
    {
        if (board_shim != null)
        {
            try
            {
                board_shim.release_session();
            }
            catch (BrainFlowException e)
            {
                Debug.Log(e);
            }
            Debug.Log("Brainflow streaming was stopped");
        }
    }
    */
    private GameObject CreateCircle(Vector2 anchoredPosition){

        GameObject gameObject = new GameObject("circle", typeof(Image));
        gameObject.transform.SetParent(graphContainer,false);
        gameObject.GetComponent<Image>().sprite = circleSprite;
        RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = anchoredPosition;
        rectTransform.sizeDelta = new Vector2(8,8);
        rectTransform.anchorMin = new Vector2(0,0);
        rectTransform.anchorMax = new Vector2(0,0);

        return gameObject;
    }

    private void ShowGraph(List<int> valueList){
        float graphHeight = graphContainer.sizeDelta.y;
        float yMaximum = 160f;
        float xSize = 13f;

        GameObject lastCircleGameObject = null;

        for(int i = 0; i < valueList.Count;i++){
            float xPosition = xSize + i*xSize;
            float yPosition = (valueList[i] /yMaximum) * graphHeight;
            
            GameObject circleGameObject = CreateCircle(new Vector2(xPosition,yPosition));
            if(lastCircleGameObject!=null){
                CreateDotConnection(lastCircleGameObject.GetComponent<RectTransform>().anchoredPosition,circleGameObject.GetComponent<RectTransform>().anchoredPosition);
            }
            lastCircleGameObject = circleGameObject;
            
            Destroy(circleGameObject,0.1f);
        }
       
    }

    private void CreateDotConnection(Vector2 dotPositionA,Vector2 dotPositionB){
        GameObject gameObject = new GameObject("dotConnection",typeof(Image));
        gameObject.transform.SetParent(graphContainer,false);
        gameObject.GetComponent<Image>().color = new Color(1,1,1, .5f);
        RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
        Vector2 dir = (dotPositionB-dotPositionA).normalized;
        float distance = Vector2.Distance(dotPositionA,dotPositionB);
        rectTransform.anchorMin = new Vector2(0,0);
        rectTransform.anchorMax = new Vector2(0,0);
        rectTransform.sizeDelta = new Vector2(distance,1f);
        rectTransform.anchoredPosition = dotPositionA + dir * distance *.5f;
        rectTransform.localEulerAngles = new Vector3(0,0,UtilsClass.GetAngleFromVectorFloat(dir));

        Destroy(gameObject,0.1f);
    }
}