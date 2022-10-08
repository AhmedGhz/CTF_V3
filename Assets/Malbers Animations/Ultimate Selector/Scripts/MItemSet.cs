using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MalbersAnimations.Selector
{
    [CreateAssetMenu(menuName = "Malbers Animations/Ultimate Selector/Items Set")]
    public class MItemSet : ScriptableObject {
        public List<MItem> Set;
    }
}