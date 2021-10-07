using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CodeMonkey.Utils;

using brainflow;
using brainflow.math;

public class Window_Graph : MonoBehaviour
{
    [SerializeField] private Sprite circleSprite;
    private RectTransform graphContainer;

    private BoardShim board_shim = null;
    private int sampling_rate = 0;

    List<int> dt = new List<int>() { };

    public static double[,] data;
    
    private void Awake() {
        graphContainer = transform.Find("graphContainer").GetComponent<RectTransform>();
        //CreateCircle(new Vector2(200,100));

        List<int> valueList = new List <int>(){10,56,56,45,30,22,17,15,13,17,25,37,40,36,33};
        //ShowGraph(valueList);

        
    }
    void Start(){
        try
        {
            BoardShim.set_log_file("brainflow_log.txt");
            BoardShim.enable_dev_board_logger();
            //BoardShim.serial_port = "COM3"; //PUERTO

            BrainFlowInputParams input_params = new BrainFlowInputParams();
            //int board_id = (int)BoardIds.GANGLION_BOARD; //SYNTHETIC BOARD REPLACED
            int board_id = (int)BoardIds.SYNTHETIC_BOARD;
            //input_params.serial_port = "COM3"; //Puerto
            board_shim = new BoardShim(board_id, input_params);
        
            board_shim.prepare_session();
            
            board_shim.start_stream(450000, "file://brainflow_data.csv:w");
            
            sampling_rate = BoardShim.get_sampling_rate(board_id);
            Debug.Log("Brainflow streaming was started");
        }
        catch (BrainFlowException e)
        {
            Debug.Log(e);
        }
    }
    // Update is called once per frame
    void Update()
    {
        //JUGAR CON LA LIBRERIA TIME
        //TIME.DELTA.TIME
        //Implementar tarea en 2D
        if (board_shim == null)
        {
            return;
        }
        int number_of_data_points = sampling_rate * 4;
        //double[,] data = board_shim.get_current_board_data(number_of_data_points);
        data = board_shim.get_current_board_data(number_of_data_points);
        // check https://brainflow.readthedocs.io/en/stable/index.html for api ref and more code samples
        
        Debug.Log("Num elements: " + data.GetLength(1));
        
        for (int i=0;i<data.GetLength(0);i++){
            dt.Add((int)data[1,i]);    
        }

        //print(dt.GetLength());
        ShowGraph(dt);
        dt.Clear();
        
        //graphContainer = transform.Find("graphContainer").GetComponent<RectTransform>();
        //print(data.GetLength(1));

    }
    // you need to call release_session and ensure that all resources correctly released
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
        float yMaximum = 600f;
        float xSize = 30f;

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











/*
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CodeMonkey.Utils;

public class Window_Graph : MonoBehaviour
{
    [SerializeField] private Sprite circleSprite;
    private RectTransform graphContainer;

    private void Awake() {
        graphContainer = transform.Find("graphContainer").GetComponent<RectTransform>();
        //CreateCircle(new Vector2(200,100));

        List<int> valueList = new List <int>(){10,56,56,45,30,22,17,15,13,17,25,37,40,36,33};
        ShowGraph(valueList);
    }

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
        float yMaximum = 90f;
        float xSize = 20f;

        GameObject lastCircleGameObject = null;

        for(int i = 0; i < valueList.Count;i++){
            float xPosition = xSize + i*xSize;
            float yPosition = (valueList[i] /yMaximum) * graphHeight;
            
            GameObject circleGameObject = CreateCircle(new Vector2(xPosition,yPosition));
            if(lastCircleGameObject!=null){
                CreateDotConnection(lastCircleGameObject.GetComponent<RectTransform>().anchoredPosition,circleGameObject.GetComponent<RectTransform>().anchoredPosition);
            }
            lastCircleGameObject = circleGameObject;
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
    }
}
*/