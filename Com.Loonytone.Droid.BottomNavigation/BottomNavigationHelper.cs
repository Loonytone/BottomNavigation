
using Animator = Android.Animation.Animator;
using AnimatorListenerAdapter = Android.Animation.AnimatorListenerAdapter;
using ObjectAnimator = Android.Animation.ObjectAnimator;
using Context = Android.Content.Context;
using Drawable = Android.Graphics.Drawables.Drawable;
using GradientDrawable = Android.Graphics.Drawables.GradientDrawable;
using Build = Android.OS.Build;
using Android.Views;
using Android.Widget;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.OS;

namespace Com.Loonytone.Droid.BottomNavigation
{

	/// <summary>
	/// Class description : This is utils class specific for this library, most the common code goes here.
	/// 
	/// @author ashokvarma
	/// @version 1.0
	/// @since 19 Mar 2016
	/// </summary>
	internal class BottomNavigationHelper
	{

		private BottomNavigationHelper()
		{
		}

		/// <summary>
		/// Used to get Measurements for MODE_FIXED
		/// </summary>
		/// <param name="context">     to fetch measurements </param>
		/// <param name="screenWidth"> total screen width </param>
		/// <param name="noOfTabs">    no of bottom bar tabs </param>
		/// <param name="scrollable">  is bottom bar scrollable </param>
		/// <returns> width of each tab </returns>
		public static int[] getMeasurementsForFixedMode(Context context, int screenWidth, int noOfTabs, bool scrollable)
		{

			int[] result = new int[2];

			int minWidth = (int) context.Resources.GetDimension(Resource.Dimension.fixed_min_width_small_views);
			int maxWidth = (int) context.Resources.GetDimension(Resource.Dimension.fixed_min_width);

			int itemWidth = screenWidth / noOfTabs;

			if (itemWidth < minWidth && scrollable)
			{
				itemWidth = (int) context.Resources.GetDimension(Resource.Dimension.fixed_min_width);
			}
			else if (itemWidth > maxWidth)
			{
				itemWidth = maxWidth;
			}

			result[0] = itemWidth;

			return result;
		}

		/// <summary>
		/// Used to get Measurements for MODE_SHIFTING
		/// </summary>
		/// <param name="context">     to fetch measurements </param>
		/// <param name="screenWidth"> total screen width </param>
		/// <param name="noOfTabs">    no of bottom bar tabs </param>
		/// <param name="scrollable">  is bottom bar scrollable </param>
		/// <returns> min and max width of each tab </returns>
		public static int[] getMeasurementsForShiftingMode(Context context, int screenWidth, int noOfTabs, bool scrollable)
		{

			int[] result = new int[2];

			int minWidth = (int) context.Resources.GetDimension(Resource.Dimension.shifting_min_width_inactive);
			int maxWidth = (int) context.Resources.GetDimension(Resource.Dimension.shifting_max_width_inactive);

			double minPossibleWidth = minWidth * (noOfTabs + 0.5);
			double maxPossibleWidth = maxWidth * (noOfTabs + 0.75);
			int itemWidth;
			int itemActiveWidth;

			if (screenWidth < minPossibleWidth)
			{
				if (scrollable)
				{
					itemWidth = minWidth;
					itemActiveWidth = (int)(minWidth * 1.5);
				}
				else
				{
					itemWidth = (int)(screenWidth / (noOfTabs + 0.5));
					itemActiveWidth = (int)(itemWidth * 1.5);
				}
			}
			else if (screenWidth > maxPossibleWidth)
			{
				itemWidth = maxWidth;
				itemActiveWidth = (int)(itemWidth * 1.75);
			}
			else
			{
				double minPossibleWidth1 = minWidth * (noOfTabs + 0.625);
				double minPossibleWidth2 = minWidth * (noOfTabs + 0.75);
				itemWidth = (int)(screenWidth / (noOfTabs + 0.5));
				itemActiveWidth = (int)(itemWidth * 1.5);
				if (screenWidth > minPossibleWidth1)
				{
					itemWidth = (int)(screenWidth / (noOfTabs + 0.625));
					itemActiveWidth = (int)(itemWidth * 1.625);
					if (screenWidth > minPossibleWidth2)
					{
						itemWidth = (int)(screenWidth / (noOfTabs + 0.75));
						itemActiveWidth = (int)(itemWidth * 1.75);
					}
				}
			}

			result[0] = itemWidth;
			result[1] = itemActiveWidth;

			return result;
		}

		/// <summary>
		/// Used to get set data to the Tab views from navigation items
		/// </summary>
		/// <param name="bottomNavigationItem"> holds all the data </param>
		/// <param name="bottomNavigationTab">  view to which data need to be set </param>
		/// <param name="bottomNavigationBar">  view which holds all the tabs </param>
		public static void bindTabWithData(BottomNavigationItem bottomNavigationItem, BottomNavigationTab bottomNavigationTab, BottomNavigationBar bottomNavigationBar)
		{

			Context context = bottomNavigationBar.Context;

			bottomNavigationTab.Label = bottomNavigationItem.getTitle(context);
			bottomNavigationTab.Icon = bottomNavigationItem.getIcon(context);

			int activeColor = bottomNavigationItem.getActiveColor(context);
			int inActiveColor = bottomNavigationItem.getInActiveColor(context);

			if (activeColor != -1)
			{
				bottomNavigationTab.ActiveColor = activeColor;
			}
			else
			{
				bottomNavigationTab.ActiveColor = bottomNavigationBar.ActiveColor;
			}

			if (inActiveColor != -1)
			{
				bottomNavigationTab.InactiveColor = inActiveColor;
			}
			else
			{
				bottomNavigationTab.InactiveColor = bottomNavigationBar.InActiveColor;
			}

			if (bottomNavigationItem.InActiveIconAvailable)
			{
				Drawable inactiveDrawable = bottomNavigationItem.getInactiveIcon(context);
				if (inactiveDrawable != null)
				{
					bottomNavigationTab.InactiveIcon = inactiveDrawable;
				}
			}

			bottomNavigationTab.ItemBackgroundColor = bottomNavigationBar.BackgroundColor;

			setBadgeForTab(bottomNavigationItem.BadgeItem, bottomNavigationTab);
		}

		/// <summary>
		/// Used to set badge for given tab
		/// </summary>
		/// <param name="badgeItem">           holds badge data </param>
		/// <param name="bottomNavigationTab"> bottom navigation tab to which badge needs to be attached </param>
		private static void setBadgeForTab(BadgeItem badgeItem, BottomNavigationTab bottomNavigationTab)
		{
			if (badgeItem != null)
			{

				Context context = bottomNavigationTab.Context;

				GradientDrawable shape = getBadgeDrawable(badgeItem, context);
				bottomNavigationTab.badgeView.SetBackgroundDrawable(shape);

				bottomNavigationTab.BadgeItem = badgeItem;
				badgeItem.SetTextView(bottomNavigationTab.badgeView);
				bottomNavigationTab.badgeView.Visibility = ViewStates.Visible;

				bottomNavigationTab.badgeView.SetTextColor(new Color(badgeItem.GetTextColor(context)));
				bottomNavigationTab.badgeView.Text = badgeItem.Text;


				FrameLayout.LayoutParams layoutParams = (FrameLayout.LayoutParams) bottomNavigationTab.badgeView.LayoutParameters;
				layoutParams.Gravity = badgeItem.Gravity;
				bottomNavigationTab.badgeView.LayoutParameters = layoutParams;

				if (badgeItem.Hidden)
				{
					// if hide is called before the initialisation of bottom-bar this will handle that
					// by hiding it.
					badgeItem.Hide();
				}
			}
		}

		internal static GradientDrawable getBadgeDrawable(BadgeItem badgeItem, Context context)
		{
			GradientDrawable shape = new GradientDrawable();
			shape.SetShape(ShapeType.Rectangle);
			shape.SetCornerRadius(context.Resources.GetDimensionPixelSize(Resource.Dimension.badge_corner_radius));
			shape.SetColor(new Color(badgeItem.GetBackgroundColor(context)));
			shape.SetStroke(badgeItem.BorderWidth, new Color(badgeItem.GetBorderColor(context)));
			return shape;
		}

		/// <summary>
		/// Used to set the ripple animation when a tab is selected
		/// </summary>
		/// <param name="clickedView">       the view that is clicked (to get dimens where ripple starts) </param>
		/// <param name="backgroundView">    temporary view to which final background color is set </param>
		/// <param name="bgOverlay">         temporary view which is animated to get ripple effect </param>
		/// <param name="newColor">          the new color i.e ripple color </param>
		/// <param name="animationDuration"> duration for which animation runs </param>
//JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
//ORIGINAL LINE: public static void setBackgroundWithRipple(Android.View.View clickedView, final Android.View.View backgroundView, final Android.View.View bgOverlay, final int newColor, int animationDuration)
		public static void setBackgroundWithRipple(View clickedView, View backgroundView, View bgOverlay, int newColor, int animationDuration)
		{
			int centerX = (int)(clickedView.GetX() + (clickedView.MeasuredWidth / 2));
			int centerY = clickedView.MeasuredHeight / 2;
			int finalRadius = backgroundView.Width;

			backgroundView.ClearAnimation();
			bgOverlay.ClearAnimation();

			Animator circularReveal;

			if (Build.VERSION.SdkInt >= BuildVersionCodes.Lollipop)
			{
				circularReveal = ViewAnimationUtils.CreateCircularReveal(bgOverlay, centerX, centerY, 0, finalRadius);
			}
			else
			{
				bgOverlay.Alpha = 0;
				circularReveal = ObjectAnimator.OfFloat(bgOverlay, "alpha", 0, 1);
			}

			circularReveal.SetDuration(animationDuration);
			circularReveal.AddListener(new AnimatorListenerAdapterAnonymousInnerClass(backgroundView, bgOverlay, newColor));

			bgOverlay.SetBackgroundColor(new Color(newColor));
			bgOverlay.Visibility = ViewStates.Visible;
			circularReveal.Start();
		}

		private class AnimatorListenerAdapterAnonymousInnerClass : AnimatorListenerAdapter
		{
			private View backgroundView;
			private View bgOverlay;
			private int newColor;

			public AnimatorListenerAdapterAnonymousInnerClass(View backgroundView, View bgOverlay, int newColor)
			{
				this.backgroundView = backgroundView;
				this.bgOverlay = bgOverlay;
				this.newColor = newColor;
			}

			public override void OnAnimationEnd(Animator animation)
			{
				onCancel();
			}

			public override void OnAnimationCancel(Animator animation)
			{
				onCancel();
			}

			private void onCancel()
			{
				backgroundView.SetBackgroundColor(new Color(newColor));
				bgOverlay.Visibility = ViewStates.Gone;
			}
		}
	}

}