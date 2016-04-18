using UnityEngine;
using System.Collections;

public class AlarmManager : MonoBehaviour
{

    public static AlarmManager Get()
    {
        return GameObject.Find("Alarm Manager").GetComponent<AlarmManager>();
    }

    public bool isAlarmSounding;

    // The last global sighting of the player.
    [HideInInspector]
    public Vector3 lastPlayerPosition = new Vector3(1000f, 1000f, 1000f);

    // The default position if the player is not in sight.
    [HideInInspector]
    public Vector3 resetPosition = new Vector3(1000f, 1000f, 1000f);

    // Reference to the AudioSources of the megaphones.					                 
    AudioSource[] alarms;

    void Awake()
    {
        // Find an array of the siren gameobjects.
        GameObject[] alarmGameObjects = GameObject.FindGameObjectsWithTag(Tags.siren);

        // Set the sirens array to have the same number of elements as there are gameobjects.
        alarms = new AudioSource[alarmGameObjects.Length];

        // For all the sirens allocate the audio source of the gameobjects.
        for (int i = 0; i < alarms.Length; i++)
        {
            alarms[i] = alarmGameObjects[i].GetComponent<AudioSource>();
        }

        isAlarmSounding = false;
    }

    void Start()
    {
        // For all the sirens allocate the audio source of the gameobjects.
        for (int i = 0; i < alarms.Length; i++)
        {
            AudioManager.instance.ApplyDefaultAudioSourceSettings(alarms[i]);
        }
        InvokeRepeating("CheckSoundOcclusion", 1f, 1f);
    }

    void CheckSoundOcclusion()
    {
        // For all the sirens allocate the audio source of the gameobjects.
        for (int i = 0; i < alarms.Length; i++)
        {
            AudioManager.instance.CheckDistanceAndOcclusionToListener(alarms[i]);

        }
    }

    public void TurnOnAlarm(Vector3 playerSpottedLocation)
    {

        // Update the location where the player was spotted.
        this.lastPlayerPosition = playerSpottedLocation;

        // Play sounds if not already playing.
        if (!isAlarmSounding)
        {
            isAlarmSounding = true;

            for (int i = 0; i < alarms.Length; i++)
            {
                AudioManager.instance.PlaySound(alarms[i], alarms[i].clip, 1f, 10f, true);
            }
        }
    }

    public void TurnOffAlarm()
    {

        // Rest the player location to the reset position.
        this.lastPlayerPosition = this.resetPosition;

        // Stop the alarm sounds.
        if (isAlarmSounding)
        {
            isAlarmSounding = false;

            for (int i = 0; i < alarms.Length; i++)
            {
                alarms[i].Stop();
            }

        }
    }
}