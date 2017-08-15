using System.Collections.Generic;
using CoordinatorLayout = Android.Support.Design.Widget.CoordinatorLayout;
using Snackbar = Android.Support.Design.Widget.Snackbar;
using ViewCompat = Android.Support.V4.View.ViewCompat;
using FastOutSlowInInterpolator = Android.Support.V4.View.Animation.FastOutSlowInInterpolator;
using Android.Views;
using Android.Views.Animations;
using Java.Lang.Ref;
using Object = Java.Lang.Object;

namespace Com.Loonytone.Droid.BottomNavigation.behaviour
{

	/// <summary>
	/// Class description
	/// 
	/// @author ashokvarma
	/// @version 1.0 </summary>
	/// <seealso cref= VerticalScrollingBehavior
	/// @since 25 Mar 2016 </seealso>
	public class BottomVerticalScrollBehavior<V> : VerticalScrollingBehavior<V> where V : View
	{
		private static readonly IInterpolator INTERPOLATOR = new FastOutSlowInInterpolator();
		private int mBottomNavHeight;
		private WeakReference mViewRef;

		///////////////////////////////////////////////////////////////////////////
		// onBottomBar changes
		///////////////////////////////////////////////////////////////////////////
//JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
//ORIGINAL LINE: @Override public boolean onLayoutChild(Android.Support.Design.Widget.CoordinatorLayout parent, final V child, int layoutDirection)
		public override bool OnLayoutChild(CoordinatorLayout parent, Object child, int layoutDirection)
		{
			// First let the parent lay it out
			parent.OnLayoutChild((V)child, layoutDirection);
			if (child is BottomNavigationBar)
			{
				mViewRef = new WeakReference((BottomNavigationBar)child);
			}

			((V)child).Post(() =>
		{
			mBottomNavHeight = ((V)child).Height;
		});
			updateSnackBarPosition(parent, (V)child, getSnackBarInstance(parent, (V)child));

			return base.OnLayoutChild(parent, child, layoutDirection);
		}

		///////////////////////////////////////////////////////////////////////////
		// SnackBar Handling
		///////////////////////////////////////////////////////////////////////////
		public override bool LayoutDependsOn(CoordinatorLayout parent, Object child, View dependency)
		{
			return isDependent(dependency) || base.LayoutDependsOn(parent, child, dependency);
		}

		private bool isDependent(View dependency)
		{
			return dependency is Snackbar.SnackbarLayout;
		}

		public override bool OnDependentViewChanged(CoordinatorLayout parent, Object child, View dependency)
		{
			if (isDependent(dependency))
			{
				updateSnackBarPosition(parent, (V)child, dependency);
				return false;
			}

			return base.OnDependentViewChanged(parent, child, dependency);
		}

		private void updateSnackBarPosition(CoordinatorLayout parent, V child, View dependency)
		{
			updateSnackBarPosition(parent, child, dependency, ViewCompat.GetTranslationY(child) - child.Height);
		}

		private void updateSnackBarPosition(CoordinatorLayout parent, V child, View dependency, float translationY)
		{
			if (dependency != null && dependency is Snackbar.SnackbarLayout)
			{
				ViewCompat.Animate(dependency).SetInterpolator(INTERPOLATOR).SetDuration(80).SetStartDelay(0).TranslationY(translationY).Start();
			}
		}

		private Snackbar.SnackbarLayout getSnackBarInstance(CoordinatorLayout parent, V child)
		{
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final java.util.List<Android.View.View> dependencies = parent.getDependencies(child);
			IList<View> dependencies = parent.GetDependencies(child);
			for (int i = 0, z = dependencies.Count; i < z; i++)
			{
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final Android.View.View view = dependencies.get(i);
				View view = dependencies[i];
				if (view is Snackbar.SnackbarLayout)
				{
					return (Snackbar.SnackbarLayout) view;
				}
			}
			return null;
		}

        ///////////////////////////////////////////////////////////////////////////
        // Auto Hide Handling
        ///////////////////////////////////////////////////////////////////////////

        //JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
        //ORIGINAL LINE: @Override public void onNestedVerticalScrollUnconsumed(Android.Support.Design.Widget.CoordinatorLayout coordinatorLayout, V child, @ScrollDirection int scrollDirection, int currentOverScroll, int totalScroll)

        public override void onNestedVerticalScrollUnconsumed(CoordinatorLayout coordinatorLayout, V child, ScrollDirection scrollDirection, int currentOverScroll, int totalScroll)
		{
			// Empty body
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Override public void onNestedVerticalPreScroll(Android.Support.Design.Widget.CoordinatorLayout coordinatorLayout, V child, Android.View.View target, int dx, int dy, int[] consumed, @ScrollDirection int scrollDirection)
		public override void onNestedVerticalPreScroll(CoordinatorLayout coordinatorLayout, V child, View target, int dx, int dy, int[] consumed, ScrollDirection scrollDirection)
		{
	//        handleDirection(child, scrollDirection);
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Override protected boolean onNestedDirectionFling(Android.Support.Design.Widget.CoordinatorLayout coordinatorLayout, V child, Android.View.View target, float velocityX, float velocityY, boolean consumed, @ScrollDirection int scrollDirection)
		protected override bool onNestedDirectionFling(CoordinatorLayout coordinatorLayout, V child, View target, float velocityX, float velocityY, bool consumed, ScrollDirection scrollDirection)
		{
	//        if (consumed) {
	//            handleDirection(child, scrollDirection);
	//        }
			return consumed;
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Override public void onNestedVerticalScrollConsumed(Android.Support.Design.Widget.CoordinatorLayout coordinatorLayout, V child, @ScrollDirection int scrollDirection, int currentOverScroll, int totalConsumedScroll)
		public override void onNestedVerticalScrollConsumed(CoordinatorLayout coordinatorLayout, V child, ScrollDirection scrollDirection, int currentOverScroll, int totalConsumedScroll)
		{
			handleDirection(coordinatorLayout, child, scrollDirection);
		}

		private void handleDirection(CoordinatorLayout parent, V child, ScrollDirection scrollDirection)
		{
			BottomNavigationBar bottomNavigationBar = (BottomNavigationBar)mViewRef.Get();
			if (bottomNavigationBar != null && bottomNavigationBar.AutoHideEnabled)
			{
				if (scrollDirection == ScrollDirection.SCROLL_DIRECTION_DOWN && bottomNavigationBar.Hidden)
				{
					updateSnackBarPosition(parent, child, getSnackBarInstance(parent, child), -mBottomNavHeight);
					bottomNavigationBar.show();
				}
				else if (scrollDirection == ScrollDirection.SCROLL_DIRECTION_UP && !bottomNavigationBar.Hidden)
				{
					updateSnackBarPosition(parent, child, getSnackBarInstance(parent, child), 0);
					bottomNavigationBar.hide();
				}
			}
		}
	}

}