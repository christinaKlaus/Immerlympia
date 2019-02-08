using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
#if UNITY_EDITOR
using UnityEditor;
#endif

[CreateAssetMenu(menuName="Audio Events/Scheduled Audio Event")]
public class ScheduledAudioEvent : ScriptableObject
{

    // public float[] ClipDurations{
    //     get { return clipDurations; }
    // }

    // [SerializeField] AudioClip[] clips;
    // [SerializeField] bool[] looped;
    // [ReadOnly(false), SerializeField] private float[] clipDurations;
    
    struct AudioEnd {
        public AudioSource source;
        public double endTime;
    }

    public float startTime = 0f;
    public AudioMixerGroup output = null;
    [SerializeField] AudioClipSetting[] clipSet = null;

    public string schedule = "";

    public void ScheduleAudio(GameObject holder){
        if(output == null) return;
        float nextStartTime = 0f;
        AudioClip thisClip;
        schedule = "";
        List<AudioEnd> audioEnds = new List<AudioEnd>();

        AudioScheduleJanitor janitor = holder.AddComponent<AudioScheduleJanitor>();

        for(int i = 0; i < clipSet.Length; i++){
            thisClip = clipSet[i].clip;

            if(clipSet[i].looped){
                AudioSource thisSource = holder.AddComponent<AudioSource>();
                //source.playOnAwake = false;
                thisSource.outputAudioMixerGroup = output;
                thisSource.loop = true;
                thisSource.clip = thisClip;
                schedule += (startTime + nextStartTime) + ": " + thisClip.name + "[" + thisClip.length + "] (looped)\n";
                double startTimeActual = AudioSettings.dspTime + startTime + nextStartTime;
                thisSource.PlayScheduled(startTimeActual);
                break;
            } else {
                for(int r = 0; r < clipSet[i].repeatTimes; r++){
                    AudioSource thisSource = holder.AddComponent<AudioSource>();
                    //source.playOnAwake = false;
                    thisSource.outputAudioMixerGroup = output;
                    thisSource.loop = false;
                    thisSource.clip = thisClip;
                    schedule += (startTime + nextStartTime) + ": " + thisClip.name + "[" + thisClip.length + "] (" + (r + 1) + "/" + clipSet[i].repeatTimes + ")\n";
                    double startTimeActual = AudioSettings.dspTime + startTime + nextStartTime;
                    thisSource.PlayScheduled(startTimeActual);
                    nextStartTime += clipSet[i].clipDuration;
                    audioEnds.Add(new AudioEnd(){source = thisSource, endTime = startTimeActual + thisClip.length});
                }
            }
        }

        janitor.StartCoroutine(ScheduleJanitor(audioEnds.ToArray(), AudioSettings.dspTime + nextStartTime));
    }

    // void OnValidate(){
        // if(clipDurations.Length != clips.Length || looped.Length != clips.Length){
        //     clipDurations = new float[clips.Length];
        //     looped = new bool[clips.Length];
        // }
        // foreach(AudioClip clip in clips){

        // }
        // clipDuration = clip != null ? clip.length : 0f;
    // }

    IEnumerator ScheduleJanitor(AudioEnd[] audioEnds, double lastStartTime){
        // Debug.Log("janitor coroutine started: " + AudioSettings.dspTime + "->" + lastStartTime + "(TimeScale = " + Time.timeScale + ")", this);
        while(AudioSettings.dspTime < lastStartTime + 10f){
            // Debug.Log("Coroutine running");
            for(int i = 0; i < audioEnds.Length; i++){
                // Debug.Log("checking " + ae);
                if(audioEnds[i].endTime <= 0) continue;
                else if(AudioSettings.dspTime > audioEnds[i].endTime + 1f){
                    // Debug.Log("End time reached for " + audioEnds[i].source, audioEnds[i].source);
                    Destroy(audioEnds[i].source);
                    audioEnds[i].endTime = -1;
                }
                yield return null;
            }
            yield return null;
        }
    }

}

class AudioScheduleJanitor : MonoBehaviour {

}

[System.Serializable]
public class AudioClipSetting {
    public bool looped;
    public int repeatTimes;
    public float clipDuration;
    public AudioClip clip;
}

#if UNITY_EDITOR
[CustomPropertyDrawer(typeof(AudioClipSetting))]
public class AudioClipSettingDrawer : PropertyDrawer {
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label){
        // property.serializedObject.Update();
        EditorGUI.BeginProperty(position, label, property);

        float fullWidth = position.width;
        position.width = fullWidth * 0.1f;
        property.FindPropertyRelative("looped").boolValue = EditorGUI.ToggleLeft(position, GUIContent.none, property.FindPropertyRelative("looped").boolValue);
        if(!property.FindPropertyRelative("looped").boolValue){
            position.x += position.width;
            position.width = fullWidth * 0.15f;
            property.FindPropertyRelative("repeatTimes").intValue = Mathf.Clamp(EditorGUI.IntField(position, property.FindPropertyRelative("repeatTimes").intValue), 1, int.MaxValue);
        } else {
            position.x += fullWidth * 0.15f;
        }

        position.x += position.width;
        position.width = fullWidth * 0.25f;
        EditorGUI.LabelField(position, property.FindPropertyRelative("clipDuration").floatValue.ToString());

        position.x += position.width; 
        position.width = fullWidth * 0.5f;
        EditorGUI.BeginChangeCheck();
        EditorGUI.PropertyField(position, property.FindPropertyRelative("clip"), GUIContent.none);
        if(EditorGUI.EndChangeCheck()){
            property.FindPropertyRelative("clipDuration").floatValue = ((AudioClip)property.FindPropertyRelative("clip").objectReferenceValue).length;
        }

        EditorGUI.EndProperty();

        property.serializedObject.ApplyModifiedProperties();
    }
}

[CustomEditor(typeof(ScheduledAudioEvent))]
public class ScheduledAudioEventEditor : Editor {

    public override void OnInspectorGUI(){
        DrawDefaultInspector();
        ScheduledAudioEvent sae = serializedObject.targetObject as ScheduledAudioEvent;
        GUILayout.TextArea(sae.schedule);
        if(GUILayout.Button("Schedule Audio")){
            GameObject g = GameObject.Find("Scheduled Audio Event Test");
            if(g == null) g = new GameObject("Scheduled Audio Event Test");
            sae.ScheduleAudio(g);
        }
    }
}
#endif
