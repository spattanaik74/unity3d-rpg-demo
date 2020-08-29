using UnityEngine;
using RPG.Saving;
using RPG.Stats;
using RPG.Core;
using GameDevTV.Utils;

namespace RPG.Resouces
{
    public class Health : MonoBehaviour, ISaveable
    {
        [SerializeField] float regenerationPercentage = 70;

        LazyValue<float> health;

        bool isDead = false;

        private void Awake() 
        {
            health = new LazyValue<float>(GetInitialHealth);
        }

        private float GetInitialHealth()
        {
            return GetComponent<BaseStats>().GetStat(Stat.Health);
        }

        private void Start() 
        {
            health.ForceInit();
        }

        private void OnEnable() {
            GetComponent<BaseStats>().onLevelup += RegeneratedHealth;
        }
        private void OnDisable() {
            GetComponent<BaseStats>().onLevelup -= RegeneratedHealth;

        }

        public bool IsDead()
        {
            return isDead;
        }

        public void Takedamage(GameObject instigator, float damage)
        {
            print(gameObject.name + "took damage: " + damage);

            health.value = Mathf.Max(health.value - damage , 0);
            print(health);
            if (health.value == 0)
            {
                Die();
                AwardExperience(instigator);
            }
        }

        public float GetHealthPoints()
        {
            return health.value;
        }

        public float GetMaxHealth()
        {
            return GetComponent<BaseStats>().GetStat(Stat.Health);
        }

        public float GetPercentage()
        {
            return 100 * (health.value / GetComponent<BaseStats>().GetStat(Stat.Health));
        }

        private void RegeneratedHealth()
        {
            float regenHealthpoints = GetComponent<BaseStats>().GetStat(Stat.Health) * (regenerationPercentage / 100);
            health.value = Mathf.Max(health.value, regenHealthpoints);

        }

        private void AwardExperience(GameObject instigator)
        {
            Experience experience = instigator.GetComponent<Experience>();
            if(experience == null) return;

            experience.GainExperience(GetComponent<BaseStats>().GetStat(Stat.ExperienceReward));
        }

        private void Die()
        {
            if(isDead ){ return; } 

            isDead = true;
            GetComponent<Animator>().SetTrigger("die");
            GetComponent<ActionScheduler>().CancelCurrentAction();
        }

        public object CaptureState()
        {
            return health.value;
        }

        public void RestoreState(object state)
        {
            health.value = (float)state;

            if (health.value == 0)
            {
                Die();
            }
        }
        

    }
}