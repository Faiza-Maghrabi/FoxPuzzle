#if UNITY_EDITOR
using Fries.Interior_01.Utility.Inspector;
#endif

using Fries.Interior_01.Utility;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

#if UNITY_EDITOR
#endif

namespace Fries.Interior_01 {
    public class DeskLamp : TurnOnAble {
        [YureiButton("Turn On")] public UnityEvent onTurnOn;
        [YureiButton("Turn Off")] public UnityEvent onTurnOff;
        
        [SerializeField] private MeshRenderer lightBoxRenderer;
        
        public void turnOn() {
            lightBoxRenderer.material.EnableKeyword("_EMISSION");
        }

        public void turnOff() {
            lightBoxRenderer.material.DisableKeyword("_EMISSION");
        }
    }
    
    #if UNITY_EDITOR
    [CustomEditor(typeof(DeskLamp))]
    public class DeskLampInspector : YureiInspector { }
    #endif
}
