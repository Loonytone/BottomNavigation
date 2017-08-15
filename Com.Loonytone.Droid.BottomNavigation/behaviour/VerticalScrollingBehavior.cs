

using Context = Android.Content.Context;
using CoordinatorLayout = Android.Support.Design.Widget.CoordinatorLayout;
using Android.Views;
using Android.Util;
using Object = Java.Lang.Object;

namespace Com.Loonytone.Droid.BottomNavigation.behaviour
{

	/// <summary>
	/// Class description
	/// 
	/// @author ashokvarma
	/// @version 1.0
	/// @since 25 Mar 2016
	/// </summary>
	public abstract class VerticalScrollingBehavior<V> : CoordinatorLayout.Behavior where V : View
	{

		private int mTotalDyUnconsumed = -1;
		private int mTotalDyConsumed = -1;
		private int mTotalDy = -1;

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @ScrollDirection private int mScrollDirection = ScrollDirection.SCROLL_NONE;
		private ScrollDirection mScrollDirection = ScrollDirection.SCROLL_NONE;
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @ScrollDirection private int mPreScrollDirection = ScrollDirection.SCROLL_NONE;
		private ScrollDirection mPreScrollDirection = ScrollDirection.SCROLL_NONE;
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @ScrollDirection private int mConsumedScrollDirection = ScrollDirection.SCROLL_NONE;
		private ScrollDirection mConsumedScrollDirection = ScrollDirection.SCROLL_NONE;

		public VerticalScrollingBehavior(Context context, IAttributeSet attrs) : base(context, attrs)
		{
		}

		public VerticalScrollingBehavior() : base()
		{
		}


		/// <returns> Scroll direction: SCROLL_DIRECTION_UP, CROLL_DIRECTION_DOWN, SCROLL_NONE </returns>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @ScrollDirection public int getScrollDirection()
		public virtual ScrollDirection ScrollDirection
		{
			get
			{
				return mScrollDirection;
			}
		}

		/// <returns> ConsumedScroll direction: SCROLL_DIRECTION_UP, CROLL_DIRECTION_DOWN, SCROLL_NONE </returns>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @ScrollDirection public int getConsumedScrollDirection()
		public virtual ScrollDirection ConsumedScrollDirection
		{
			get
			{
				return mConsumedScrollDirection;
			}
		}


		/// <returns> PreScroll direction: SCROLL_DIRECTION_UP, SCROLL_DIRECTION_DOWN, SCROLL_NONE </returns>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @ScrollDirection public int getPreScrollDirection()
		public virtual ScrollDirection PreScrollDirection
		{
			get
			{
				return mPreScrollDirection;
			}
		}

		public override bool OnStartNestedScroll(CoordinatorLayout coordinatorLayout, Object child, View directTargetChild, View target, int nestedScrollAxes)
		{
			return (nestedScrollAxes & (int)ScrollAxis.Vertical) != 0;
		}

	//    @Override
	//    public void onNestedScrollAccepted(CoordinatorLayout coordinatorLayout, V child, View directTargetChild, View target, int nestedScrollAxes) {
	//        super.onNestedScrollAccepted(coordinatorLayout, child, directTargetChild, target, nestedScrollAxes);
	//    }
	//
	//    @Override
	//    public void onStopNestedScroll(CoordinatorLayout coordinatorLayout, V child, View target) {
	//        super.onStopNestedScroll(coordinatorLayout, child, target);
	//    }

		public override void OnNestedScroll(CoordinatorLayout coordinatorLayout, Object child, View target, int dxConsumed, int dyConsumed, int dxUnconsumed, int dyUnconsumed)
		{
			base.OnNestedScroll(coordinatorLayout, child, target, dxConsumed, dyConsumed, dxUnconsumed, dyUnconsumed);
			if (dyUnconsumed > 0 && mTotalDyUnconsumed < 0)
			{
				mTotalDyUnconsumed = 0;
				mScrollDirection = ScrollDirection.SCROLL_DIRECTION_UP;
				onNestedVerticalScrollUnconsumed(coordinatorLayout, (V)child, mScrollDirection, dyConsumed, mTotalDyUnconsumed);
			}
			else if (dyUnconsumed < 0 && mTotalDyUnconsumed > 0)
			{
				mTotalDyUnconsumed = 0;
				mScrollDirection = ScrollDirection.SCROLL_DIRECTION_DOWN;
				onNestedVerticalScrollUnconsumed(coordinatorLayout, (V)child, mScrollDirection, dyConsumed, mTotalDyUnconsumed);
			}
			mTotalDyUnconsumed += dyUnconsumed;

			if (dyConsumed > 0 && mTotalDyConsumed < 0)
			{
				mTotalDyConsumed = 0;
				mConsumedScrollDirection = ScrollDirection.SCROLL_DIRECTION_UP;
				onNestedVerticalScrollConsumed(coordinatorLayout, (V)child, mConsumedScrollDirection, dyConsumed, mTotalDyConsumed);
			}
			else if (dyConsumed < 0 && mTotalDyConsumed > 0)
			{
				mTotalDyConsumed = 0;
				mConsumedScrollDirection = ScrollDirection.SCROLL_DIRECTION_DOWN;
				onNestedVerticalScrollConsumed(coordinatorLayout, (V)child, mConsumedScrollDirection, dyConsumed, mTotalDyConsumed);
			}
			mTotalDyConsumed += dyConsumed;
		}

		public override void OnNestedPreScroll(CoordinatorLayout coordinatorLayout, Object child, View target, int dx, int dy, int[] consumed)
		{
			base.OnNestedPreScroll(coordinatorLayout, child, target, dx, dy, consumed);
			if (dy > 0 && mTotalDy < 0)
			{
				mTotalDy = 0;
				mPreScrollDirection = ScrollDirection.SCROLL_DIRECTION_UP;
				onNestedVerticalPreScroll(coordinatorLayout, (V)child, target, dx, dy, consumed, mPreScrollDirection);
			}
			else if (dy < 0 && mTotalDy > 0)
			{
				mTotalDy = 0;
				mPreScrollDirection = ScrollDirection.SCROLL_DIRECTION_DOWN;
				onNestedVerticalPreScroll(coordinatorLayout, (V)child, target, dx, dy, consumed, mPreScrollDirection);
			}
			mTotalDy += dy;
		}


		public override bool OnNestedFling(CoordinatorLayout coordinatorLayout, Object child, View target, float velocityX, float velocityY, bool consumed)
		{
			base.OnNestedFling(coordinatorLayout, child, target, velocityX, velocityY, consumed);
			return onNestedDirectionFling(coordinatorLayout, (V)child, target, velocityX, velocityY, consumed, velocityY > 0 ? ScrollDirection.SCROLL_DIRECTION_UP : ScrollDirection.SCROLL_DIRECTION_DOWN);
		}

		/// <param name="coordinatorLayout"> the CoordinatorLayout parent of the view this Behavior is
		///                          associated with </param>
		/// <param name="child">             the child view of the CoordinatorLayout this Behavior is associated with </param>
		/// <param name="scrollDirection">   Direction of the scroll: SCROLL_DIRECTION_UP, SCROLL_DIRECTION_DOWN </param>
		/// <param name="currentOverScroll"> Unconsumed value, negative or positive based on the direction; </param>
		/// <param name="totalScroll">       Cumulative value for current direction (Unconsumed) </param>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: public abstract void onNestedVerticalScrollUnconsumed(Android.Support.Design.Widget.CoordinatorLayout coordinatorLayout, V child, @ScrollDirection int scrollDirection, int currentOverScroll, int totalScroll);
		public abstract void onNestedVerticalScrollUnconsumed(CoordinatorLayout coordinatorLayout, V child, ScrollDirection scrollDirection, int currentOverScroll, int totalScroll);

		/// <param name="coordinatorLayout">   the CoordinatorLayout parent of the view this Behavior is
		///                            associated with </param>
		/// <param name="child">               the child view of the CoordinatorLayout this Behavior is associated with </param>
		/// <param name="scrollDirection">     Direction of the scroll: SCROLL_DIRECTION_UP, SCROLL_DIRECTION_DOWN </param>
		/// <param name="currentOverScroll">   Unconsumed value, negative or positive based on the direction; </param>
		/// <param name="totalConsumedScroll"> Cumulative value for current direction (Unconsumed) </param>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: public abstract void onNestedVerticalScrollConsumed(Android.Support.Design.Widget.CoordinatorLayout coordinatorLayout, V child, @ScrollDirection int scrollDirection, int currentOverScroll, int totalConsumedScroll);
		public abstract void onNestedVerticalScrollConsumed(CoordinatorLayout coordinatorLayout, V child, ScrollDirection scrollDirection, int currentOverScroll, int totalConsumedScroll);

		/// <param name="coordinatorLayout"> the CoordinatorLayout parent of the view this Behavior is
		///                          associated with </param>
		/// <param name="child">             the child view of the CoordinatorLayout this Behavior is associated with </param>
		/// <param name="target">            the descendant view of the CoordinatorLayout performing the nested scroll </param>
		/// <param name="dx">                the raw horizontal number of pixels that the user attempted to scroll </param>
		/// <param name="dy">                the raw vertical number of pixels that the user attempted to scroll </param>
		/// <param name="consumed">          out parameter. consumed[0] should be set to the distance of dx that
		///                          was consumed, consumed[1] should be set to the distance of dy that
		///                          was consumed </param>
		/// <param name="scrollDirection">   Direction of the scroll: SCROLL_DIRECTION_UP, SCROLL_DIRECTION_DOWN </param>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: public abstract void onNestedVerticalPreScroll(Android.Support.Design.Widget.CoordinatorLayout coordinatorLayout, V child, Android.View.View target, int dx, int dy, int[] consumed, @ScrollDirection int scrollDirection);
		public abstract void onNestedVerticalPreScroll(CoordinatorLayout coordinatorLayout, V child, View target, int dx, int dy, int[] consumed, ScrollDirection scrollDirection);

		/// <param name="coordinatorLayout"> the CoordinatorLayout parent of the view this Behavior is
		///                          associated with </param>
		/// <param name="child">             the child view of the CoordinatorLayout this Behavior is associated with </param>
		/// <param name="target">            the descendant view of the CoordinatorLayout performing the nested scroll </param>
		/// <param name="velocityX">         horizontal velocity of the attempted fling </param>
		/// <param name="velocityY">         vertical velocity of the attempted fling </param>
		/// <param name="consumed">          true if the nested child view consumed the fling </param>
		/// <param name="scrollDirection">   Direction of the scroll: SCROLL_DIRECTION_UP, SCROLL_DIRECTION_DOWN </param>
		/// <returns> true if the Behavior consumed the fling </returns>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: protected abstract boolean onNestedDirectionFling(Android.Support.Design.Widget.CoordinatorLayout coordinatorLayout, V child, Android.View.View target, float velocityX, float velocityY, boolean consumed, @ScrollDirection int scrollDirection);
		protected abstract bool onNestedDirectionFling(CoordinatorLayout coordinatorLayout, V child, View target, float velocityX, float velocityY, bool consumed, ScrollDirection scrollDirection);

	//    @Override
	//    public boolean onNestedPreFling(CoordinatorLayout coordinatorLayout, V child, View target, float velocityX, float velocityY) {
	//        return super.onNestedPreFling(coordinatorLayout, child, target, velocityX, velocityY);
	//    }
	//
	//    @Override
	//    public WindowInsetsCompat onApplyWindowInsets(CoordinatorLayout coordinatorLayout, V child, WindowInsetsCompat insets) {
	//
	//        return super.onApplyWindowInsets(coordinatorLayout, child, insets);
	//    }
	//
	//    @Override
	//    public Parcelable onSaveInstanceState(CoordinatorLayout parent, V child) {
	//        return super.onSaveInstanceState(parent, child);
	//    }

	}

    //JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
    //ORIGINAL LINE: @Retention(RetentionPolicy.SOURCE) @IntDef({ScrollDirection.SCROLL_DIRECTION_UP, ScrollDirection.SCROLL_DIRECTION_DOWN}) public class ScrollDirection extends System.Attribute
    public enum ScrollDirection
    {


        SCROLL_DIRECTION_UP = 1,
        SCROLL_DIRECTION_DOWN = -1,
        SCROLL_NONE = 0
    }

}