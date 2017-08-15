using Context = Android.Content.Context;
using IAttributeSet = Android.Util.IAttributeSet;
using Android.Views;
using Android.Views.Animations;
using Android.Widget;

namespace Com.Loonytone.Droid.BottomNavigation
{

    /// <summary>
    /// Class description
    /// 
    /// @author ashokvarma
    /// @version 1.0 </summary>
    /// <seealso cref= BottomNavigationTab
    /// @since 19 Mar 2016 </seealso>
    internal class ShiftingBottomNavigationTab : BottomNavigationTab
	{

		public ShiftingBottomNavigationTab(Context context) : base(context)
		{
		}

		public ShiftingBottomNavigationTab(Context context, IAttributeSet attrs) : base(context, attrs)
		{
		}

		public ShiftingBottomNavigationTab(Context context, IAttributeSet attrs, int defStyleAttr) : base(context, attrs, defStyleAttr)
		{
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @TargetApi(android.os.BuildVersionCodes.LOLLIPOP) public ShiftingBottomNavigationTab(Android.Content.Context context, Android.Util.IAttributeSet attrs, int defStyleAttr, int defStyleRes)
		public ShiftingBottomNavigationTab(Context context, IAttributeSet attrs, int defStyleAttr, int defStyleRes) : base(context, attrs, defStyleAttr, defStyleRes)
		{
		}

		internal override void init()
		{
			paddingTopActive = (int) Resources.GetDimension(Resource.Dimension.shifting_height_top_padding_active);
			paddingTopInActive = (int) Resources.GetDimension(Resource.Dimension.shifting_height_top_padding_inactive);

			LayoutInflater inflater = LayoutInflater.From(Context);
			View view = inflater.Inflate(Resource.Layout.shifting_bottom_navigation_item, this, true);
			containerView = view.FindViewById(Resource.Id.shifting_bottom_navigation_container);
			labelView = (TextView) view.FindViewById(Resource.Id.shifting_bottom_navigation_title);
			iconView = (ImageView) view.FindViewById(Resource.Id.shifting_bottom_navigation_icon);
			badgeView = (TextView) view.FindViewById(Resource.Id.shifting_bottom_navigation_badge);

			base.init();
		}

		public override void Select(bool setActiveColor, int animationDuration)
		{
			base.Select(setActiveColor, animationDuration);

			ResizeWidthAnimation anim = new ResizeWidthAnimation(this, this, mActiveWidth);
			anim.Duration = animationDuration;
			this.StartAnimation(anim);

			labelView.Animate().ScaleY(1).ScaleX(1).SetDuration(animationDuration).Start();
		}

		public override void unSelect(bool setActiveColor, int animationDuration)
		{
			base.unSelect(setActiveColor, animationDuration);

			ResizeWidthAnimation anim = new ResizeWidthAnimation(this, this, mInActiveWidth);
			anim.Duration = animationDuration;
			this.StartAnimation(anim);

			labelView.Animate().ScaleY(0).ScaleX(0).SetDuration(0).Start();
		}

	//    @Override
	//    public void initialise(boolean setActiveColor) {
	//        super.initialise(setActiveColor);
	//    }

		public class ResizeWidthAnimation : Animation
		{
			private readonly ShiftingBottomNavigationTab outerInstance;

			internal int mWidth;
			internal int mStartWidth;
			internal View mView;

			public ResizeWidthAnimation(ShiftingBottomNavigationTab outerInstance, View view, int width)
			{
				this.outerInstance = outerInstance;
				mView = view;
				mWidth = width;
				mStartWidth = view.Width;
			}

			protected override void ApplyTransformation(float interpolatedTime, Transformation t)
			{
				mView.LayoutParameters.Width = mStartWidth + (int)((mWidth - mStartWidth) * interpolatedTime);
				mView.RequestLayout();
			}

	//        @Override
	//        public void initialize(int width, int height, int parentWidth, int parentHeight) {
	//            super.initialize(width, height, parentWidth, parentHeight);
	//        }

			public override bool WillChangeBounds()
			{
				return true;
			}
		}

	}

}