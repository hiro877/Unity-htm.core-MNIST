using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;


public class EventPlane : MonoBehaviour
{
    // Timeline
    private GameObject _child;
    public PlayableDirector director;

    // Fungus
    public Fungus.Flowchart flowchart = null;

    // Collision Param
    public int collisionedEventCount = 0;
    private bool isCollisioned = false;

    // Start is called before the first frame update
    void Start()
    {
        // 子オブジェクトのTimeLineを探す
        _child = this.transform.Find ("Timeline").gameObject;
        director = _child.GetComponent<PlayableDirector>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        
        //EventPlaneが衝突したオブジェクトがSD_unitychan_humanoidだった場合
        if(collision.gameObject.name=="SD_unitychan_humanoid"){
            Debug.Log("EventPlane is collisioned");
            if (this.collisionedEventCount == 0){
                // Start Timeline
                director.Play();
                this.isCollisioned = true;
                this.collisionedEventCount += 1;
            }
            // else {
            //     smallCubeDirector.StartFungus("black_box_talk_from_second");
            // }
        }
    }

}
