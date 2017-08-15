using Context = Android.Content.Context;
using IAttributeSet = Android.Util.IAttributeSet;
using Android.Views;
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
    internal class FixedBottomNavigationTab : BottomNavigationTab
	{

		internal float labelScale;

		public FixedBottomNavigationTab(Context context) : base(context)
		{
		}

		public FixedBottomNavigationTab(Context context, IAttributeSet attrs) : base(context, attrs)
		{
		}

		public FixedBottomNavigationTab(Context context, IAttributeSet attrs, int defStyleAttr) : base(context, attrs, defStyleAttr)
		{
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @TargetApi(android.os.BuildVersionCodes.LOLLIPOP) public FixedBottomNavigationTab(Android.Content.Context context, Android.Util.IAttributeSet attrs, int defStyleAttr, int defStyleRes)
		public FixedBottomNavigationTab(Context context, IAttributeSet attrs, int defStyleAttr, int defStyleRes) : base(context, attrs, defStyleAttr, defStyleRes)
		{
		}

		internal override void init()
		{
			paddingTopActive = (int) Resources.GetDimension(Resource.Dimension.fixed_height_top_padding_active);
			paddingTopInActive = (int) Resources.GetDimension(Resource.Dimension.fixed_height_top_padding_inactive);

			LayoutInflater inflater = LayoutInflater.From(Context);
			View view = inflater.Inflate(Resource.Layout.fixed_bottom_navigation_item, this, true);
			containerView = view.FindViewById(Resource.Id.fixed_bottom_navigation_container);
			labelView = (TextView) view.FindViewById(Resource.Id.fixed_bottom_navigation_title);
			iconView = (ImageView) view.FindViewById(Resource.Id.fixed_bottom_navigation_icon);
			badgeView = (TextView) view.FindViewById(Resource.Id.fixed_bottom_navigation_badge);

			labelScale = Resources.GetDimension(Resource.Dimension.fixed_label_inactive) / Resources.GetDimension(Resource.Dimension.fixed_label_active);

			base.init();
		}

		public override void Select(bool setActiveColor, int animationDuration)
		{
			labelView.Animate().ScaleX(1).ScaleY(1).SetDuration(animationDuration).Start();
	//        labelView.setTextSize(TypedValue.COMPLEX_UNIT_PX, getResources().GetDimension(Resource.Dimension.fixed_label_active));
			base.Select(setActiveColor, animationDuration);
		}

		public override void unSelect(bool setActiveColor, int animationDuration)
		{
			labelView.Animate().ScaleX(labelScale).ScaleY(labelScale).SetDuration(animationDuration).Start();
	//        labelView.setTextSize(TypedValue.COMPLEX_UNIT_PX, getResources().GetDimension(Resource.Dimension.fixed_label_inactive));
			base.unSelect(setActiveColor, animationDuration);
		}

	//    @Override
	//    public void initialise(boolean setActiveColor) {
	//        super.initialise(setActiveColor);
	//    }
	}

}