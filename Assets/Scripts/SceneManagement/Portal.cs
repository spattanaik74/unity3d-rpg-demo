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
            yield return SceneManager.LoadSceneAsync(sceneToLoad);
            
            Portal otherportal = GetOtherPortal();
            UpdatePlayer(otherportal);

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