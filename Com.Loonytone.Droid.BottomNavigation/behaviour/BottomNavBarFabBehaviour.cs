using System;
using System.Collections.Generic;

using CoordinatorLayout = Android.Support.Design.Widget.CoordinatorLayout;
using FloatingActionButton = Android.Support.Design.Widget.FloatingActionButton;
using Snackbar = Android.Support.Design.Widget.Snackbar;
using ViewCompat = Android.Support.V4.View.ViewCompat;
using ViewPropertyAnimatorCompat = Android.Support.V4.View.ViewPropertyAnimatorCompat;
using FastOutSlowInInterpolator = Android.Support.V4.View.Animation.FastOutSlowInInterpolator;
using Android.Views;
using Android.Views.Animations;
using Object = Java.Lang.Object;

namespace Com.Loonytone.Droid.BottomNavigation.behaviour
{
    /// <summary>
    /// Class description
    /// 
    /// @author ashokvarma
    /// @version 1.0
    /// @since 06 Jun 2016  FloatingActionButton
    /// </summary>
    public class BottomNavBarFabBehaviour : CoordinatorLayout.Behavior
    {

        internal ViewPropertyAnimatorCompat mFabTranslationYAnimator;
        //    @BottomNavigationBar.FabBehaviour
        //    private int mFabBehaviour;

        internal static readonly IInterpolator FAST_OUT_SLOW_IN_INTERPOLATOR = new FastOutSlowInInterpolator();

        ///////////////////////////////////////////////////////////////////////////
        // Constructor
        ///////////////////////////////////////////////////////////////////////////

        //    public BottomNavBarFabBehaviour() {
        //        mFabBehaviour = BottomNavigationBar.FAB_BEHAVIOUR_TRANSLATE_AND_STICK;
        //    }
        ///////////////////////////////////////////////////////////////////////////
        // Dependencies setup
        ///////////////////////////////////////////////////////////////////////////

        public override bool LayoutDependsOn(CoordinatorLayout parent, Object child, View dependency)
        {
            return isDependent(dependency) || base.LayoutDependsOn(parent, child, dependency);
        }

        public override bool OnLayoutChild(CoordinatorLayout parent, Object child, int layoutDirection)
        {
            // First let the parent lay it out
            parent.OnLayoutChild((FloatingActionButton)child, layoutDirection);

            updateFabTranslationForBottomNavigationBar(parent, (FloatingActionButton)child, null);

            return base.OnLayoutChild(parent, child, layoutDirection);
        }

        public override bool OnDependentViewChanged(CoordinatorLayout parent, Object child, View dependency)
        {
            if (isDependent(dependency))
            {
                updateFabTranslationForBottomNavigationBar(parent, (FloatingActionButton)child, dependency);
                return false;
            }

            return base.OnDependentViewChanged(parent, child, dependency);
        }

        public override void OnDependentViewRemoved(CoordinatorLayout parent, Object child, View dependency)
        {
            if (isDependent(dependency))
            {
                updateFabTranslationForBottomNavigationBar(parent, (FloatingActionButton)child, dependency);
            }
        }

        private bool isDependent(View dependency)
        {
            return dependency is BottomNavigationBar || dependency is Snackbar.SnackbarLayout;
        }

        ///////////////////////////////////////////////////////////////////////////
        // Animating Fab based on Changes
        ///////////////////////////////////////////////////////////////////////////

        private void updateFabTranslationForBottomNavigationBar(CoordinatorLayout parent, FloatingActionButton fab, View dependency)
        {

            float snackBarTranslation = getFabTranslationYForSnackBar(parent, fab);
            float[] bottomBarParameters = getFabTranslationYForBottomNavigationBar(parent, fab);
            float bottomBarTranslation = bottomBarParameters[0];
            float bottomBarHeight = bottomBarParameters[1];

            float targetTransY = 0;
            if (snackBarTranslation >= bottomBarTranslation)
            {
                // when snackBar is below BottomBar in translation present.
                targetTransY = bottomBarTranslation;
            }
            else
            {
                targetTransY = snackBarTranslation;
            }

            //        if (mFabBehaviour == BottomNavigationBar.FAB_BEHAVIOUR_DISAPPEAR) {
            //            if (targetTransY == 0) {
            //                fab.hide();
            //            } else {
            //                fab.show();
            //            }
            //        }

            //JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
            //ORIGINAL LINE: final float currentTransY = Android.Support.V4.View.ViewCompat.getTranslationY(fab);
            float currentTransY = ViewCompat.GetTranslationY(fab);

            // Make sure that any current animation is cancelled
            ensureOrCancelAnimator(fab);


            if (fab.IsShown && Math.Abs(currentTransY - targetTransY) > (fab.Height * 0.667f))
            {
                // If the FAB will be travelling by more than 2/3 of it's height, let's animate it instead
                mFabTranslationYAnimator.TranslationY(targetTransY).Start();
            }
            else
            {
                // Now update the translation Y
                ViewCompat.SetTranslationY(fab, targetTransY);
            }
        }

        ///////////////////////////////////////////////////////////////////////////
        // Fab Translation due to SnackBar and Due to BottomBar
        ///////////////////////////////////////////////////////////////////////////

        private float[] getFabTranslationYForBottomNavigationBar(CoordinatorLayout parent, FloatingActionButton fab)
        {
            float minOffset = 0;
            float viewHeight = 0;
            //JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
            //ORIGINAL LINE: final java.util.List<Android.View.View> dependencies = parent.getDependencies(fab);
            IList<View> dependencies = parent.GetDependencies(fab);
            for (int i = 0, z = dependencies.Count; i < z; i++)
            {
                //JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
                //ORIGINAL LINE: final Android.View.View view = dependencies.get(i);
                View view = dependencies[i];
                if (view is BottomNavigationBar)
                {
                    viewHeight = view.Height;
                    minOffset = Math.Min(minOffset, ViewCompat.GetTranslationY(view) - viewHeight);
                }
            }
            float[] returnValues = new float[] { minOffset, viewHeight };

            return returnValues;
        }

        private float getFabTranslationYForSnackBar(CoordinatorLayout parent, FloatingActionButton fab)
        {
            float minOffset = 0;
            //JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
            //ORIGINAL LINE: final java.util.List<Android.View.View> dependencies = parent.getDependencies(fab);
            IList<View> dependencies = parent.GetDependencies(fab);
            for (int i = 0, z = dependencies.Count; i < z; i++)
            {
                //JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
                //ORIGINAL LINE: final Android.View.View view = dependencies.get(i);
                View view = dependencies[i];
                if (view is Snackbar.SnackbarLayout && parent.DoViewsOverlap(fab, view))
                {
                    minOffset = Math.Min(minOffset, ViewCompat.GetTranslationY(view) - view.Height);
                }
            }

            return minOffset;
        }

        //    public void setmFabBehaviour(int mFabBehaviour) {
        //        this.mFabBehaviour = mFabBehaviour;
        //    }

        ///////////////////////////////////////////////////////////////////////////
        // Animator Initializer
        ///////////////////////////////////////////////////////////////////////////

        private void ensureOrCancelAnimator(FloatingActionButton fab)
        {
            if (mFabTranslationYAnimator == null)
            {
                mFabTranslationYAnimator = ViewCompat.Animate(fab);
                mFabTranslationYAnimator.SetDuration(400);
                mFabTranslationYAnimator.SetInterpolator(FAST_OUT_SLOW_IN_INTERPOLATOR);
            }
            else
            {
                mFabTranslationYAnimator.Cancel();
            }
        }
    }

}