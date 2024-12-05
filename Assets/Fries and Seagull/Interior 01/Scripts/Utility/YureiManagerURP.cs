using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;
#if UNITY_EDITOR
using Fries.Interior_01.Utility.Inspector;
#endif

namespace Fries.Interior_01.Utility {
    public class YureiManagerURP : MonoBehaviour {
        public VolumeProfile volumeProfile;
        
        [Tooltip("Post Process effect including glowing will only show to these cameras")]
        public List<Camera> gameCameras = new();
        
        [YureiButton("Initialize")] [IgnoreInInspector]
        public Action initialize;

        private void Reset() {
            initialize = init;
        }

        private void init() {
            if (gameCameras == null || gameCameras.Count == 0) {
                Debug.LogError("Please provide at least 1 valid camera to Game Cameras field.");
                return;
            }
            
            foreach (var camera in gameCameras) {
                Volume volume = camera.GetComponent<Volume>();
                if (volume) volume.sharedProfile = volumeProfile;
                else {
                    volume = camera.gameObject.AddComponent<Volume>();
                    volume.sharedProfile = volumeProfile;
                }
            }
            Debug.Log($"[Yurei] Init post-processor settings for Universal Rendering Pipeline successfully.");
        }

    }
    
    #if UNITY_EDITOR
    [CustomEditor(typeof(YureiManagerURP))]
    public class YureiManagerURPInspector : YureiInspector { }
    #endif
}