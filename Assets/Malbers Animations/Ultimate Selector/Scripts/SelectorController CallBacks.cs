using UnityEngine;

namespace MalbersAnimations.Selector
{
    //──────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────
    //CALLBACKS
    //──────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────
    public partial class SelectorController
    {
        /// <summary>
        /// Change to the next or Previous Item Index
        /// </summary>
        /// <param name="next">true: Next, false: Previous</param>
        public void SelectNextItem(bool next)
        {
            if (next)
                IndexSelected++;
            else
            {
                if (IndexSelected == 0)
                    IndexSelected = Items.Count - 1;
                else
                    IndexSelected--;
            }
        }

        /// <summary> Select an item by its Index </summary>
        /// <param name="index"> the index </param>
        public void SelectNextItem(int index) => IndexSelected = index;

        /// <summary>
        /// Select an item by its MItem Script
        /// </summary>
        public void SelectNextItem(MItem next)
        {
            int index = Items.FindIndex(item => item == next);
            IndexSelected = index;
        }

        /// <summary>
        /// Select an item by its name
        /// </summary>
        public void SelectNextItem(string next)
        {
            int index = Items.FindIndex(item => item.name == next);
            IndexSelected = index;
        }

        /// <summary>
        /// Updates the Lock Material of the Items
        /// </summary>
        public virtual void UpdateLockItems()
        {
            if (!LockMaterial) return;          //Skip if there's no LockMaterial

            foreach (MItem item in Items)
            {
                if (item.Locked)
                {
                    item.SetLockMaterial(LockMaterial);
                }
            }
        }
        

        /// <summary>
        /// Plays an animation
        /// </summary>
        /// <param name="anim">the name of the animation to play</param>
        public virtual void _PlayAnimation(string anim)
        {
            if (IndexSelected == -1) return;    //Skip if the Selection is clear

            Animator itemAnimator = CurrentItem.GetComponentInChildren<Animator>();

            if (itemAnimator && !CurrentItem.Locked)
            {
                if (CurrentItem.CustomAnimation != string.Empty)// Check if the object has a custom animator
                { itemAnimator.CrossFade(CurrentItem.CustomAnimation, 0.1f); }

                else { itemAnimator.CrossFade(anim, 0.1f); }
            }
        } 

        /// <summary>Plays an animation on the previous item</summary>
        /// <param name="anim">the name of the animation to play</param>
        public virtual void _PlayAnimationPreviousItem(string anim)
        {
            if (PreviousIndex == -1) return;              //Skip if the Selection is clear
            if (PreviousIndex == IndexSelected) return;   //Skip if the there's no preview item

            Animator itemAnimator = PreviousItem.GetComponentInChildren<Animator>();

            if (itemAnimator && !PreviousItem.Locked)
            {
                itemAnimator.CrossFade(anim, 0.1f);
            }
        }

        /// <summary> Plays a transform Animation </summary>
        /// <param name="animTransform">The Animation Set to Play</param>
        public virtual void _PlayAnimationTransform(TransformAnimation animTransform)
        {
            if (IndexSelected == -1 || animTransform == null) return;    //Skip if the Selection is clear


            //This is used for Finishing the previous animation if a new  animation is being called
            if (LastAnimation && CurrentItem && LastAnimatedItem &&
              CurrentItem == LastAnimatedItem && isAnimating)
                EndAnimationTransform(LastAnimatedItem, LastAnimation);

            Stop_Coroutine(PlayTransformAnimationC, "Reset Anim: " + CurrentItem.name);
            PlayTransformAnimationC = PlayTransformAnimation(CurrentItem, animTransform);
            Start_Coroutine(PlayTransformAnimationC, "Play Anim: "+CurrentItem.name);

            LastAnimation = animTransform;              //Store the Last Animation
            LastAnimatedItem = CurrentItem;             //Store the last Item that was animated.
        }
    }
}