using UnityEngine;
using RPG.Combat;
using RPG.Movement;
using RPG.Core;



namespace RPG.Control
{

    public class PlayerMove : MonoBehaviour
    {

        //Cached reference
        Health health;

        private void Start() {
            health = GetComponent<Health>();
        }
   
        void Update()
        {
            if (health.IsDead()) return;

            if (InteractWithCombat()) return ;
            if (InteractWithMove()) return;

        }

        private bool InteractWithCombat()
        {
          RaycastHit[] hits = Physics.RaycastAll(GetMouseRay());
            foreach (RaycastHit hit in hits)
            {
                CombatTarget target =  hit.transform.GetComponent<CombatTarget>();
                if(target == null) { continue; }

                if (!GetComponent<Fighter>().CanAttacK(target.gameObject))
                {
                    continue;
                }

                if(Input.GetMouseButton(0))
                {
                   GetComponent<Fighter>().Attack(target.gameObject);
                }
                return true;
                
            }
            return false;
           
        }

        private bool InteractWithMove()
        {
            RaycastHit hit;
            bool hasHit = Physics.Raycast(GetMouseRay(), out hit);
            if (hasHit)
            {
                if(Input.GetMouseButton(0))
                {
                    GetComponent<Mover>().StartMoveAction(hit.point, 1f);
                }
                return true;
                
            }
            return false;
        }

        private static Ray GetMouseRay()
        {
            return Camera.main.ScreenPointToRay(Input.mousePosition);
        }
    }
}
