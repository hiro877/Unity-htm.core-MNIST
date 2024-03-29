﻿using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class SmallCubeDirector : MonoBehaviour
{
    // Fungus
    public Fungus.Flowchart flowchart = null;

    // Display Test Image
    public GameObject csvReaderPlane;
    private List<List<byte>> csvReaderPlaneImages;
    private string csvReaderPlane_imeges_path = "test_images30";
    private bool testDisplayFlag = false;

    // MNIST Image
    private const int IMG_WIDTH = 28;
    private const int IMG_HEIGHT = 28;
    private List<List<byte>> test_images;
    private string test_images_path  = "test_images";

    // Generate Objects
    public GameObject questionBoard;

	void Awake() {
        csvReaderPlane = GameObject.FindGameObjectWithTag("csvReaderPlane");
        questionBoard = GameObject.CreatePrimitive(PrimitiveType.Plane);
	}

    void Start()
    {
        csvReaderPlaneImages = read_mnist_images(csvReaderPlane_imeges_path);
        test_images  = read_mnist_images(test_images_path);
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey("space")){
            Debug.Log("Pushed Tab");
            if (!testDisplayFlag){
                List<byte> img = csvReaderPlaneImages[0];
                List<List<byte>> img_byte2D = convert_data(img, true);
                AttachImage(csvReaderPlane, img_byte2D, IMG_WIDTH, IMG_HEIGHT);
                testDisplayFlag = true;
            }

        }
        
    }

    public void StartFungus(string message){
        flowchart.SendFungusMessage(message);
    }
    public void StartFungusFromEventPlane(){
        flowchart.SendFungusMessage("start_question");
    }

    List<List<byte>> read_mnist_images(string file_path)
    {
        TextAsset csv = Resources.Load(file_path) as TextAsset;
        StringReader reader = new StringReader(csv.text);
        List<List<byte>> data = new List<List<byte>>();

        while(reader.Peek() != -1)
        {
            string[] str_line = reader.ReadLine().Split(',');
            List<byte> line = new List<byte>();
            foreach(string str in str_line)
            {
                line.Add(byte.Parse(str));
            }
            data.Add(line);
        }
        return data;
    }

    List<List<byte>> convert_data( List<byte> data_array,
                                    bool is_texture_axis=false)
    {
        List<List<byte>> d = new List<List<byte>>();
        for (int i = 0; i < IMG_HEIGHT; i++) d.Add(new List<byte>());
        
        for(int y = 0; y < IMG_HEIGHT; y++)
        {
            for(int x = 0; x < IMG_WIDTH; x++)
            {
                if (is_texture_axis)
                {
                    d[IMG_HEIGHT - y -1].Add(data_array[y * IMG_HEIGHT + x]);
                }
                else
                {
                    d[y].Add(data_array[y * IMG_HEIGHT + x]);
                }
            }
        }
        return d;
    }

    public void AttachImage(GameObject target, List<List<byte>> byte2D,
                            int width, int height){
        Texture2D attach_texture = new Texture2D(width, height);

        for(int y = 0; y < height; y++)
        {
            for(int x = 0; x < width; x++)
            {
                attach_texture.SetPixel(x, y, new Color(byte2D[y][x], byte2D[y][x], byte2D[y][x]));

            }
        }
        attach_texture.Apply();
        target.GetComponent<MeshRenderer>().material.mainTexture = attach_texture;
    }

    public void GenerateQuestionBoard(){

        // 位置調整を行う
        questionBoard.transform.position = new Vector3(2.3f, 1.5f, -30.0f);

        questionBoard.transform.Rotate(new Vector3(90, 0, 0));

        // フワッと徐々にオブジェクトを出現させる
        int test_num = Random.Range(2000,3000);
        List<byte> img = test_images[test_num];
        List<List<byte>> img_byte2D = convert_data(img, true);
        AttachImage(questionBoard, img_byte2D, IMG_WIDTH, IMG_HEIGHT);
        StartCoroutine(AppearGradually(questionBoard));
    }

    IEnumerator AppearGradually(GameObject target)
    {   
        // 繰り返し回数
        int loopcount = 10;

        // 更新間隔
        float waitsecond = 0.05f;

        // スケール設定
        // オフセット値
        float offsetScale = 0.15f / loopcount;
        // 更新値
        float updateScale = 0;

        // オブジェクトの有効化（初期位置）
        target.transform.localScale = new Vector3(0.0f, 0.0f, 0.0f);

        for (int loop = 0; loop < loopcount; loop++)
        {
            // スケール更新
            updateScale = updateScale + offsetScale;
            target.transform.localScale = new Vector3(updateScale, updateScale, updateScale);
            yield return new WaitForSeconds(waitsecond);
        }
        // 最終スケール
        target.transform.localScale = new Vector3(0.15f, 0.15f, 0.15f);

        yield return new WaitForSeconds(3f);
    }

}
