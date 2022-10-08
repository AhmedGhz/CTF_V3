using UnityEngine;

namespace SnazzlebotTools.ENPCHealthBars
{
    public class DemoButtonActions : MonoBehaviour
    {
        public void DealDamage()
        {
            foreach (var healthBar in FindObjectsOfType<ENPCHealthBar>())
            {
                healthBar.Value -= 10;
            }
        }

        public void AddHealth()
        {
            foreach (var healthBar in FindObjectsOfType<ENPCHealthBar>())
            {
                healthBar.Value += 10;
            }
        }
    }
}