using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using RPG.Saving;

namespace RPG.Cinematic

{
    public class CinematicTrigger : MonoBehaviour, ISaveable
    {
        bool alreadyTriggered = false;

        private void OnTriggerEnter(Collider other) 
        {
            if(!alreadyTriggered && other.gameObject.tag == "Player")
            {
                alreadyTriggered = true;
                GetComponent<PlayableDirector>().Play();
            }
            
        }
        public object CaptureState()
        {
            print("SaveTrigger");
            return alreadyTriggered;
        }

        //Will be call after the level loaded just after awake
        //Load Character HP from save file
        public void RestoreState(object state)
        {
            alreadyTriggered = (bool)state;
        }
    }

}