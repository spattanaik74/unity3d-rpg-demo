using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.AI;

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

            yield return fader.FadeOut(fadeoutTime);
            yield return SceneManager.LoadSceneAsync(sceneToLoad);
            
            Portal otherportal = GetOtherPortal();
            UpdatePlayer(otherportal);

            yield return new WaitForSeconds(fadewaitTime);
            yield return fader.FadeIn(fadeinTime);

            Destroy(gameObject);
        }
        private void UpdatePlayer(Portal otherportal)
        {
            GameObject player = GameObject.FindWithTag("Player");
            player.GetComponent<NavMeshAgent>().Warp(otherportal.spawnpoint.position);
            player.transform.rotation = otherportal.spawnpoint.rotation;

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