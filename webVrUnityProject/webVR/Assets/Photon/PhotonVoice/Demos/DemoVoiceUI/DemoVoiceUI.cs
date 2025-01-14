//using System.Collections.Generic;
//using ExitGames.Client.Photon;
//using Photon.Realtime;
//using Photon.Voice.Unity;
//using Photon.Voice.Unity.UtilityScripts;
//using UnityEngine;
//using UnityEngine.UI;


//namespace Photon.Voice.DemoVoiceUI
//{
//    [RequireComponent(typeof(VoiceConnection))]
//    public class DemoVoiceUI : MonoBehaviour
//    {
//        [SerializeField]
//        private Text connectionStatusText;

//        [SerializeField]
//        private Text serverStatusText;

//        [SerializeField]
//        private Text roomStatusText;

//        [SerializeField]
//        private Text inputWarningText;

//		[SerializeField]
//        private Text packetLossWarningText;

//        [SerializeField]
//        private InputField localNicknameText;

//		[SerializeField]
//		private WebRtcAudioDsp voiceAudioPreprocessor;

//        [SerializeField]
//        private Toggle debugEchoToggle;

//        [SerializeField]
//        private Toggle reliableTransmissionToggle;

//        [SerializeField]
//        private GameObject webRtcDspGameObject;

//        [SerializeField]
//        private Toggle aecToggle;

//        [SerializeField]
//        private Toggle noiseSuppressionToggle;

//        [SerializeField]
//        private Toggle agcToggle;

//        [SerializeField]
//        private Toggle vadToggle;
        
//        [SerializeField]
//        private Toggle muteToggle;

//        public Transform RemoteVoicesPanel;

//        private VoiceConnection voiceConnection;

//        // this demo uses a Custom Property (as explained in the Realtime API), to sync if a player muted her microphone. that value needs a string key.
//        protected internal const string MutePropKey = "mute";

//        private Color warningColor = new Color(0.9f,0.5f,0f,1f);
//        private Color okColor = new Color(0.0f,0.6f,0.2f,1f);

//        private void Awake()
//        {
//            this.voiceConnection = this.GetComponent<VoiceConnection>();
//            this.InitToggles();
//        }

//        private void OnEnable()
//        {
//            this.voiceConnection.SpeakerLinked += this.OnSpeakerCreated;

//            if (this.localNicknameText != null)
//            {
//                string savedNick = PlayerPrefs.GetString("vNick");
//                if (!string.IsNullOrEmpty(savedNick))
//                {
//                    //Debug.LogFormat("Saved nick = {0}", savedNick);
//                    this.localNicknameText.text = savedNick;
//                    this.voiceConnection.Client.NickName = savedNick;
//                }
//            }
//        }

//        private void OnDisable()
//        {
//            this.voiceConnection.SpeakerLinked -= this.OnSpeakerCreated;
//        }

//        private void OnSpeakerCreated(Speaker speaker)
//        {
//            speaker.gameObject.transform.SetParent(this.RemoteVoicesPanel, false);
//        }

//        private void OnRemoteVoiceRemove(Speaker speaker)
//        {
//            Destroy(speaker.gameObject);
//        }


//        public void ToggleMute()
//        {
//            this.voiceConnection.PrimaryRecorder.TransmitEnabled = !this.muteToggle.isOn;
//            this.voiceConnection.Client.LocalPlayer.SetCustomProperties(new Hashtable { { MutePropKey, this.muteToggle.isOn } }); // transmit is used as opposite of mute...
//        }

//        public void ToggleDebugEcho()
//        {
//            this.voiceConnection.PrimaryRecorder.DebugEchoMode = this.debugEchoToggle.isOn;
//        }

//        public void ToggleReliable()
//        {
//            this.voiceConnection.PrimaryRecorder.ReliableMode = this.reliableTransmissionToggle.isOn;
//        }

//		public void ToggleAEC()
//		{
//			this.voiceAudioPreprocessor.AEC = this.aecToggle.isOn;
//		}
            
//		public void ToggleNoiseSuppression()
//		{
//			this.voiceAudioPreprocessor.NoiseSuppression = this.noiseSuppressionToggle.isOn;
//		}

//		public void ToggleAGC()
//		{
//			this.voiceAudioPreprocessor.AGC = this.agcToggle.isOn;
//		}

//		public void ToggleVAD()
//		{
//			this.voiceAudioPreprocessor.VAD = this.vadToggle.isOn;
//		}
        
//        /// <summary>Called by UI.</summary>
//        public void UpdateSyncedNickname(string nickname)
//        {
//            nickname = nickname.Trim();
//            if (string.IsNullOrEmpty(nickname))
//            {
//                return;
//            }

//            //Debug.LogFormat("UpdateSyncedNickname() name: {0}", nickname);
//            this.voiceConnection.Client.LocalPlayer.NickName = nickname;
//            PlayerPrefs.SetString("vNick", nickname);
//        }


//        /// <summary>Called by UI.</summary>
//        public void JoinOrCreateRoom(string roomname)
//        {
//            roomname = roomname.Trim();
//            ConnectAndJoin caj = this.GetComponent<ConnectAndJoin>();
//            if (caj == null) return;

//            //Debug.LogFormat("JoinOrCreateRoom() roomname: {0}", roomname);

//            if (string.IsNullOrEmpty(roomname))
//            {
//                caj.RoomName = string.Empty;
//                caj.RandomRoom = true;
//                //caj.HideRoom = false;
//            }
//            else
//            {
//                caj.RoomName = roomname;
//                caj.RandomRoom = false;
//                //caj.HideRoom = true;
//            }
//            this.voiceConnection.Client.OpLeaveRoom(false);
//        }

//        protected void Update()
//        {
//            #if UNITY_EDITOR
//            this.InitToggles(); // refresh UI in case changed from Unity Editor
//            #endif
//            this.connectionStatusText.text = this.voiceConnection.Client.State.ToString();
//            this.serverStatusText.text = string.Format("{0}/{1}", this.voiceConnection.Client.CloudRegion, this.voiceConnection.Client.CurrentServerAddress);
//            string playerDebugString = string.Empty;
//            if (this.voiceConnection.Client.InRoom)
//            {
//                Dictionary<int, Player>.ValueCollection temp = this.voiceConnection.Client.CurrentRoom.Players.Values;
//                if (temp.Count > 1)
//                {
//                    foreach (Player p in temp)
//                    {
//                        playerDebugString += p.ToStringFull();
//                    }
//                }
//            }
//            this.roomStatusText.text = this.voiceConnection.Client.CurrentRoom == null ? string.Empty : string.Format("{0} {1}", this.voiceConnection.Client.CurrentRoom.Name, playerDebugString);

//            if (this.voiceConnection.PrimaryRecorder.IsCurrentlyTransmitting)
//            {
//                var amplitude = this.voiceConnection.PrimaryRecorder.LevelMeter.CurrentAvgAmp;
//                if (amplitude > 1) {
//                    amplitude /= 32768;
//                }
//                if (amplitude > 0.1) {
//                    this.inputWarningText.text = "Input too loud!";
//                    this.inputWarningText.color = this.warningColor;
//                } else {
//                    this.inputWarningText.text = string.Empty;
//                } 
//            }

//            if (this.voiceConnection.FramesReceivedPerSecond > 0) {
//                this.packetLossWarningText.text = string.Format("{0:0.##}% Packet Loss", this.voiceConnection.FramesLostPercent);
//                this.packetLossWarningText.color = (this.voiceConnection.FramesLostPercent > 1) ? this.warningColor : this.okColor;
//            } else {
//                this.packetLossWarningText.text = "(no data)";
//            }
//        }

//        private void InitToggles()
//        {
//            if (this.voiceConnection != null && this.voiceConnection.PrimaryRecorder != null)
//            {
//                if (this.debugEchoToggle != null)
//                {
//                    this.debugEchoToggle.isOn = this.voiceConnection.PrimaryRecorder.DebugEchoMode;
//                }
//                if (this.reliableTransmissionToggle != null)
//                {
//                    this.reliableTransmissionToggle.isOn = this.voiceConnection.PrimaryRecorder.ReliableMode;
//                }
//            }
//            if (this.webRtcDspGameObject != null)
//            {
//                if (this.voiceAudioPreprocessor == null)
//                {
//                    this.webRtcDspGameObject.SetActive(false);
//                }
//                else
//                {
//                    #if UNITY_EDITOR_WIN || UNITY_EDITOR_OSX || !UNITY_EDITOR && (UNITY_IOS || UNITY_ANDROID || UNITY_STANDALONE_OSX || UNITY_STANDALONE_WIN)
//                    this.webRtcDspGameObject.SetActive(true);
//                    if (this.aecToggle != null)
//                    {
//                        this.aecToggle.isOn = this.voiceAudioPreprocessor.AEC;
//                    }
//                    if (this.noiseSuppressionToggle != null)
//                    {
//                        this.noiseSuppressionToggle.isOn = this.voiceAudioPreprocessor.NoiseSuppression;
//                    }
//                    if (this.agcToggle != null)
//                    {
//                        this.agcToggle.isOn = this.voiceAudioPreprocessor.AGC;
//                    }
//                    if (this.vadToggle != null)
//                    {
//                        this.vadToggle.isOn = this.voiceAudioPreprocessor.VAD;
//                    }
//                    #else
//                    this.webRtcDspGameObject.SetActive(false);
//                    #endif
//                }
                
//            }
            
//        }
//    }
//}