using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.AI;
using RPG.Control;
using RPG.Saving;

namespace RPG.SceneManagement
{
    public class Portal : MonoBehaviour
    {
        enum DestinationIdentifier
        {
            A, B, C, D, E
        }

        [SerializeField] int sceneToLoad = -1;
        [SerializeField] Transform spawnpoint;
        [SerializeField] DestinationIdentifier destination;
        [SerializeField] float fadeinTime = 1f;
        [SerializeField] float fadeoutTime = 1f;
        [SerializeField] float fadewaitTime = 0.5f;

        private void OnTriggerEnter(Collider other) 
        {
            if(other.tag == "Player")
            {
                StartCoroutine(Transition());
            }
        }

        private IEnumerator Transition()
        {
            DontDestroyOnLoad(gameObject);
    
            Fader fader = FindObjectOfType<Fader>();

            Savingwrapper wrapper = FindObjectOfType<Savingwrapper>();
            PlayerMove playerController = GameObject.FindWithTag("Player").GetComponent<PlayerMove>();
            playerController.enabled = false;


            yield return fader.FadeOut(fadeoutTime);

            wrapper.Save();

            yield return SceneManager.LoadSceneAsync(sceneToLoad);
            PlayerMove newPlayerController = GameObject.FindWithTag("Player").GetComponent<PlayerMove>();
            newPlayerController.enabled = false;
            
            wrapper.Load();

            Portal otherportal = GetOtherPortal();
            UpdatePlayer(otherportal);

            wrapper.Save();

            yield return new WaitForSeconds(fadewaitTime);
            fader.FadeIn(fadeinTime);

            newPlayerController.enabled = true;

            Destroy(gameObject);
        }
        private void UpdatePlayer(Portal otherportal)
        {
            GameObject player = GameObject.FindWithTag("Player");
            player.GetComponent<NavMeshAgent>().enabled = false;
            player.GetComponent<NavMeshAgent>().Warp(otherportal.spawnpoint.position);
            player.transform.rotation = otherportal.spawnpoint.rotation;
            player.GetComponent<NavMeshAgent>().enabled = false;

        }

        private Portal GetOtherPortal()
        {
            foreach (Portal portal in FindObjectsOfType<Portal>())
            {
                if(portal == this) continue;
                if(portal.destination != destination) continue;

                return portal;
            }

            return null;
        }
    }
}