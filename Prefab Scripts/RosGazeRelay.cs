﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RosGazeRelay : RosComponent {


    public double PublishingRate = 2;
    private RosPublisher<ros.geometry_msgs.PointStamped> FocusedPointPub;
    private RosPublisher<ros.geometry_msgs.PoseStamped> HeadPosePub;

	// Use this for initialization
	void Start () {
        Advertise("FocusedPointPub", "/hololens/headset/focused_point", PublishingRate, out FocusedPointPub);
        Advertise("HeadPosePub", "/hololens/headset/head_pose", PublishingRate, out HeadPosePub);
	}
	
	// Update is called once per frame
	void Update () {

        if (RosGazeManager.Instance.Focused)
        {
            ros.geometry_msgs.PointStamped fp = new ros.geometry_msgs.PointStamped();
            fp.header.frame_id = "/Unity";
            fp.point = new ros.geometry_msgs.Point(RosGazeManager.Instance.position);

            Publish(FocusedPointPub, fp);
        }


        ros.geometry_msgs.PoseStamped hp = new ros.geometry_msgs.PoseStamped();
        hp.header.frame_id = "/Unity";

        Quaternion headRot = Camera.main.transform.rotation;
        
        // Shifting head rotation because it was rotated in unity
        headRot *= Quaternion.Euler(0, -90, 0);
        hp.pose = new ros.geometry_msgs.Pose(new ros.geometry_msgs.Point(Camera.main.transform.position),
                                             new ros.geometry_msgs.Quaternion(headRot));

        Publish(HeadPosePub, hp);


	}
}
