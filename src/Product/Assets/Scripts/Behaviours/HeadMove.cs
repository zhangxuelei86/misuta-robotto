﻿using AL;
using Assets;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.VR;

public class HeadMove : MonoBehaviour
{
    private string[] jointNames = new string[] { "HeadYaw", "HeadPitch" };
    private const float SPEED_FRACTION = 1;

    public Calibration calibration;
    private HeadTranslator headTranslator;
    private ALMotionProxy motionProxy;

    // Use this for initialization
    void Start()
    {
        headTranslator = new HeadTranslator();
        motionProxy = new ALMotionProxy(RobotConfiguration.ADRESS, RobotConfiguration.PORT);
        calibration.ToggleMode += SetEnabled;
    }

    void SetEnabled(bool b)
    {
        enabled = b;
    }

    // Update is called once per frame
    void Update()
    {
        float pitch = InputTracking.GetLocalRotation(VRNode.Head).eulerAngles.x; //get Vive pitch
        float yaw = InputTracking.GetLocalRotation(VRNode.Head).eulerAngles.y; //get Vive yaw

        pitch = headTranslator.TranslatePitch(pitch);
        yaw = headTranslator.TranslateYaw(yaw);

        motionProxy.SetAngles(jointNames, new float[] { -yaw, pitch }, SPEED_FRACTION); //Send angles to robot
    }
}
