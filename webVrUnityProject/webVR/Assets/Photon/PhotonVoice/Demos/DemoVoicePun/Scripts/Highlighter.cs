﻿//// ----------------------------------------------------------------------------
//// <copyright file="Highlighter.cs" company="Exit Games GmbH">
//// Photon Voice Demo for PUN- Copyright (C) 2016 Exit Games GmbH
//// </copyright>
//// <summary>
//// Class that highlights the Photon Voice features by toggling isometric view 
//// icons for the two components Recorder and Speaker.
//// </summary>
//// <author>developer@photonengine.com</author>
//// ----------------------------------------------------------------------------

//#pragma warning disable 0649 // Field is never assigned to, and will always have its default value

//namespace ExitGames.Demos.DemoPunVoice
//{

//    using UnityEngine;
//    using UnityEngine.UI;
//	using Photon.Voice.Unity;
//	using Photon.Voice.PUN;

//    [RequireComponent(typeof(Canvas))]
//    public class Highlighter : MonoBehaviour
//    {
//        private Canvas canvas;

//        private PhotonVoiceView photonVoiceView;

//        [SerializeField]
//        private Image recorderSprite;

//        [SerializeField]
//        private Image speakerSprite;

//        [SerializeField]
//        private Text bufferLagText;

//        private bool showSpeakerLag;

//        private void OnEnable()
//        {
//            ChangePOV.CameraChanged += ChangePOV_CameraChanged;
//            VoiceDemoUI.DebugToggled += VoiceDemoUI_DebugToggled;
//        }

//        private void OnDisable()
//        {
//            ChangePOV.CameraChanged -= ChangePOV_CameraChanged;
//            VoiceDemoUI.DebugToggled -= VoiceDemoUI_DebugToggled;
//        }

//        private void VoiceDemoUI_DebugToggled(bool debugMode)
//        {
//            showSpeakerLag = debugMode;
//        }

//        private void ChangePOV_CameraChanged(Camera camera)
//        {
//            canvas.worldCamera = camera;
//        }

//        private void Awake()
//        {
//            canvas = GetComponent<Canvas>();
//            if (canvas != null && canvas.worldCamera == null) { canvas.worldCamera = Camera.main; }
//            photonVoiceView = GetComponentInParent<PhotonVoiceView>();
//        }


//        // Update is called once per frame
//        private void Update()
//        {
//            recorderSprite.enabled = photonVoiceView.IsRecording;
//            speakerSprite.enabled = photonVoiceView.IsSpeaking;
//            bufferLagText.enabled = showSpeakerLag && photonVoiceView.IsSpeaking;
//            if (bufferLagText.enabled)
//            {
//                bufferLagText.text = string.Format("{0}", photonVoiceView.SpeakerInUse.Lag);
//            }
//        }

//        private void LateUpdate()
//        {
//            if (canvas == null || canvas.worldCamera == null) { return; } // should not happen, throw error
//            transform.rotation = Quaternion.Euler(0f, canvas.worldCamera.transform.eulerAngles.y, 0f); //canvas.worldCamera.transform.rotation;
//        }
//    }
//}