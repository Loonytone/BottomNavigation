using System.Collections.Generic;
using Context = Android.Content.Context;
using TypedArray = Android.Content.Res.TypedArray;
using Color = Android.Graphics.Color;
using Build = Android.OS.Build;
using CoordinatorLayout = Android.Support.Design.Widget.CoordinatorLayout;
using FloatingActionButton = Android.Support.Design.Widget.FloatingActionButton;
using ContextCompat = Android.Support.V4.Content.ContextCompat;
using ViewCompat = Android.Support.V4.View.ViewCompat;
using ViewPropertyAnimatorCompat = Android.Support.V4.View.ViewPropertyAnimatorCompat;
using LinearOutSlowInInterpolator = Android.Support.V4.View.Animation.LinearOutSlowInInterpolator;
using Android.Util;
using Android.Views;
using Android.Views.Animations;
using Android.Widget;

using Com.Loonytone.Droid.BottomNavigation.behaviour;
using Com.Loonytone.Droid.BottomNavigation.utils;
using Android.OS;
using System.Threading;

namespace Com.Loonytone.Droid.BottomNavigation
{

    /// <summary>
    /// Class description : This class is used to draw the layout and this acts like a bridge between
    /// library and app, all details can be modified via this class.
    /// 
    /// @author ashokvarma
    /// @version 1.0 </summary>
    /// <seealso cref= FrameLayout </seealso>
    /// <seealso cref= <a href="https://www.google.com/design/spec/components/bottom-navigation.html">Google Bottom Navigation Component</a>
    /// @since 19 Mar 2016 </seealso>
    //JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
    //ORIGINAL LINE: @CoordinatorLayout.DefaultBehavior(BottomVerticalScrollBehavior.class) public class BottomNavigationBar extends android.widget.FrameLayout
    public class BottomNavigationBar : FrameLayout
	{



//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Mode private int mMode = MODE_DEFAULT;
		private Mode mMode = Mode.MODE_DEFAULT;
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @BackgroundStyle private int mBackgroundStyle = BACKGROUND_STYLE_DEFAULT;
		private BackgroundStyle mBackgroundStyle = BackgroundStyle.BACKGROUND_STYLE_DEFAULT;

		private static readonly IInterpolator INTERPOLATOR = new LinearOutSlowInInterpolator();
		private ViewPropertyAnimatorCompat mTranslationAnimator;

		private bool mScrollable = false;

		private const int MIN_SIZE = 3;
		private const int MAX_SIZE = 5;

		internal List<BottomNavigationItem> mBottomNavigationItems = new List<BottomNavigationItem>();
		internal List<BottomNavigationTab> mBottomNavigationTabs = new List<BottomNavigationTab>();

		private const int DEFAULT_SELECTED_POSITION = -1;
		private int mSelectedPosition = DEFAULT_SELECTED_POSITION;
		private int mFirstSelectedPosition = 0;
		private IOnTabSelectedListener mTabSelectedListener;

		private int mActiveColor;
		private int mInActiveColor;
		private int mBackgroundColor;

		private FrameLayout mBackgroundOverlay;
		private FrameLayout mContainer;
		private LinearLayout mTabContainer;

		private const int DEFAULT_ANIMATION_DURATION = 200;
		private int mAnimationDuration = DEFAULT_ANIMATION_DURATION;
		private int mRippleAnimationDuration = (int)(DEFAULT_ANIMATION_DURATION * 2.5);

		private float mElevation;

		private bool mAutoHideEnabled;
		private bool mIsHidden = false;

		///////////////////////////////////////////////////////////////////////////
		// View Default Constructors and Methods
		///////////////////////////////////////////////////////////////////////////

		public BottomNavigationBar(Context context) : this(context, null)
		{
		}

		public BottomNavigationBar(Context context, IAttributeSet attrs) : this(context, attrs, 0)
		{
		}

		public BottomNavigationBar(Context context, IAttributeSet attrs, int defStyleAttr) : base(context, attrs, defStyleAttr)
		{
			parseAttrs(context, attrs);
			init();
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @TargetApi(android.os.BuildVersionCodes.LOLLIPOP) public BottomNavigationBar(Android.Content.Context context, Android.Util.IAttributeSet attrs, int defStyleAttr, int defStyleRes)
		public BottomNavigationBar(Context context, IAttributeSet attrs, int defStyleAttr, int defStyleRes) : base(context, attrs, defStyleAttr, defStyleRes)
		{
			parseAttrs(context, attrs);
			init();
		}

		/// <summary>
		/// This method initiates the bottomNavigationBar properties,
		/// Tries to get them form XML if not preset sets them to their default values.
		/// </summary>
		/// <param name="context"> context of the bottomNavigationBar </param>
		/// <param name="attrs">   attributes mentioned in the layout XML by user </param>
		private void parseAttrs(Context context, IAttributeSet attrs)
		{
			if (attrs != null)
			{
				TypedArray typedArray = context.Theme.ObtainStyledAttributes(attrs, Resource.Styleable.BottomNavigationBar, 0, 0);

				mActiveColor = typedArray.GetColor(Resource.Styleable.BottomNavigationBar_bnbActiveColor, Utils.fetchContextColor(context, Resource.Attribute.colorAccent));
				mInActiveColor = typedArray.GetColor(Resource.Styleable.BottomNavigationBar_bnbInactiveColor, Color.LightGray);
				mBackgroundColor = typedArray.GetColor(Resource.Styleable.BottomNavigationBar_bnbBackgroundColor, Color.White);
				mAutoHideEnabled = typedArray.GetBoolean(Resource.Styleable.BottomNavigationBar_bnbAutoHideEnabled, true);
				mElevation = typedArray.GetDimension(Resource.Styleable.BottomNavigationBar_bnbElevation, Resources.GetDimension(Resource.Dimension.bottom_navigation_elevation));

				setAnimationDuration(typedArray.GetInt(Resource.Styleable.BottomNavigationBar_bnbAnimationDuration, DEFAULT_ANIMATION_DURATION));

                Mode mode = (Mode)typedArray.GetInt(Resource.Styleable.BottomNavigationBar_bnbMode, (int)Mode.MODE_DEFAULT);
                switch (mode)
				{
					case Mode.MODE_FIXED:
						mMode = Mode.MODE_FIXED;
						break;

					case Mode.MODE_SHIFTING:
						mMode = Mode.MODE_SHIFTING;
						break;

					case Mode.MODE_DEFAULT:
					default:
						mMode = Mode.MODE_DEFAULT;
						break;
				}

                BackgroundStyle style = (BackgroundStyle)typedArray.GetInt(Resource.Styleable.BottomNavigationBar_bnbBackgroundStyle, (int)BackgroundStyle.BACKGROUND_STYLE_DEFAULT);

                switch (style)
				{
					case BackgroundStyle.BACKGROUND_STYLE_STATIC:
						mBackgroundStyle = BackgroundStyle.BACKGROUND_STYLE_STATIC;
						break;

					case BackgroundStyle.BACKGROUND_STYLE_RIPPLE:
						mBackgroundStyle = BackgroundStyle.BACKGROUND_STYLE_RIPPLE;
						break;

					case BackgroundStyle.BACKGROUND_STYLE_DEFAULT:
					default:
						mBackgroundStyle = BackgroundStyle.BACKGROUND_STYLE_DEFAULT;
						break;
				}

				typedArray.Recycle();
			}
			else
			{
				mActiveColor = Utils.fetchContextColor(context, Resource.Attribute.colorAccent);
				mInActiveColor = Color.LightGray;
				mBackgroundColor = Color.White;
				mElevation = Resources.GetDimension(Resource.Dimension.bottom_navigation_elevation);
			}
		}

		/// <summary>
		/// This method initiates the bottomNavigationBar and handles layout related values
		/// </summary>
		private void init()
		{

	//        MarginLayoutParams marginParams = new ViewGroup.MarginLayoutParams(new ViewGroup.LayoutParams(ViewGroup.LayoutParams.MatchParent, (int) getContext().getResources().GetDimension(Resource.Dimension.bottom_navigation_padded_height)));
	//        marginParams.setMargins(0, (int) getContext().getResources().GetDimension(Resource.Dimension.bottom_navigation_top_margin_correction), 0, 0);

			LayoutParameters = new ViewGroup.LayoutParams(new ViewGroup.LayoutParams(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.WrapContent));

			LayoutInflater inflater = LayoutInflater.From(Context);
			View parentView = inflater.Inflate(Resource.Layout.bottom_navigation_bar_container, this, true);
			mBackgroundOverlay = (FrameLayout) parentView.FindViewById(Resource.Id.bottom_navigation_bar_overLay);
			mContainer = (FrameLayout) parentView.FindViewById(Resource.Id.bottom_navigation_bar_container);
			mTabContainer = (LinearLayout) parentView.FindViewById(Resource.Id.bottom_navigation_bar_item_container);

			if (Build.VERSION.SdkInt >= BuildVersionCodes.Lollipop)
			{
				this.OutlineProvider = ViewOutlineProvider.Bounds;
			}
			else
			{
				//to do
			}

			ViewCompat.SetElevation(this, mElevation);
			SetClipToPadding(false);
		}

	//    @Override
	//    protected void onMeasure(int widthMeasureSpec, int heightMeasureSpec) {
	//        super.onMeasure(widthMeasureSpec, heightMeasureSpec);
	//    }

		///////////////////////////////////////////////////////////////////////////
		// View Data Setter methods, Called before Initialize method
		///////////////////////////////////////////////////////////////////////////

		/// <summary>
		/// Used to add a new tab.
		/// </summary>
		/// <param name="item"> bottom navigation tab details </param>
		/// <returns> this, to allow builder pattern </returns>
		public virtual BottomNavigationBar AddItem(BottomNavigationItem item)
		{
			mBottomNavigationItems.Add(item);
			return this;
		}

		/// <summary>
		/// Used to remove a tab.
		/// you should call initialise() after this to see the results effected.
		/// </summary>
		/// <param name="item"> bottom navigation tab details </param>
		/// <returns> this, to allow builder pattern </returns>
		public virtual BottomNavigationBar removeItem(BottomNavigationItem item)
		{
			mBottomNavigationItems.Remove(item);
			return this;
		}

		/// <param name="mode"> any of the three Modes supported by library </param>
		/// <returns> this, to allow builder pattern </returns>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: public BottomNavigationBar setMode(@Mode int mode)
		public virtual BottomNavigationBar SetMode(Mode mode)
		{
			this.mMode = mode;
			return this;
		}

		/// <param name="backgroundStyle"> any of the three Background Styles supported by library </param>
		/// <returns> this, to allow builder pattern </returns>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: public BottomNavigationBar setBackgroundStyle(@BackgroundStyle int backgroundStyle)
		public virtual BottomNavigationBar SetBackgroundStyle(BackgroundStyle backgroundStyle)
		{
			this.mBackgroundStyle = backgroundStyle;
			return this;
		}

		/// <param name="activeColor"> res code for the default active color </param>
		/// <returns> this, to allow builder pattern </returns>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: public BottomNavigationBar setActiveColor(@ColorRes int activeColor)
		public virtual BottomNavigationBar setActiveColor(int activeColor)
		{
			this.mActiveColor = ContextCompat.GetColor(Context, activeColor);
			return this;
		}

		/// <param name="activeColorCode"> color code in string format for the default active color </param>
		/// <returns> this, to allow builder pattern </returns>
		public virtual BottomNavigationBar setActiveColor(string activeColorCode)
		{
			this.mActiveColor = Color.ParseColor(activeColorCode);
			return this;
		}

		/// <param name="inActiveColor"> res code for the default in-active color </param>
		/// <returns> this, to allow builder pattern </returns>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: public BottomNavigationBar setInActiveColor(@ColorRes int inActiveColor)
		public virtual BottomNavigationBar setInActiveColor(int inActiveColor)
		{
			this.mInActiveColor = ContextCompat.GetColor(Context, inActiveColor);
			return this;
		}

		/// <param name="inActiveColorCode"> color code in string format for the default in-active color </param>
		/// <returns> this, to allow builder pattern </returns>
		public virtual BottomNavigationBar setInActiveColor(string inActiveColorCode)
		{
			this.mInActiveColor = Color.ParseColor(inActiveColorCode);
			return this;
		}

		/// <param name="backgroundColor"> res code for the default background color </param>
		/// <returns> this, to allow builder pattern </returns>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: public BottomNavigationBar setBarBackgroundColor(@ColorRes int backgroundColor)
		public virtual BottomNavigationBar setBarBackgroundColor(int backgroundColor)
		{
			this.mBackgroundColor = ContextCompat.GetColor(Context, backgroundColor);
			return this;
		}

		/// <param name="backgroundColorCode"> color code in string format for the default background color </param>
		/// <returns> this, to allow builder pattern </returns>
		public virtual BottomNavigationBar setBarBackgroundColor(string backgroundColorCode)
		{
			this.mBackgroundColor = Color.ParseColor(backgroundColorCode);
			return this;
		}

		/// <param name="firstSelectedPosition"> position of tab that needs to be selected by default </param>
		/// <returns> this, to allow builder pattern </returns>
		public virtual BottomNavigationBar SetFirstSelectedPosition(int firstSelectedPosition)
		{
			this.mFirstSelectedPosition = firstSelectedPosition;
			return this;
		}

		/// <summary>
		/// will be public once all bugs are ressolved.
		/// </summary>
		private BottomNavigationBar setScrollable(bool scrollable)
		{
			mScrollable = scrollable;
			return this;
		}

		///////////////////////////////////////////////////////////////////////////
		// Initialise Method
		///////////////////////////////////////////////////////////////////////////

		/// <summary>
		/// This method should be called at the end of all customisation method.
		/// This method will take all changes in to consideration and redraws tabs.
		/// </summary>
		public virtual void Initialise()
		{
			mSelectedPosition = DEFAULT_SELECTED_POSITION;
			mBottomNavigationTabs.Clear();

			if (mBottomNavigationItems.Count > 0)
			{
				mTabContainer.RemoveAllViews();
				if (mMode == Mode.MODE_DEFAULT)
				{
					if (mBottomNavigationItems.Count <= MIN_SIZE)
					{
						mMode = Mode.MODE_FIXED;
					}
					else
					{
						mMode = Mode.MODE_SHIFTING;
					}
				}
				if (mBackgroundStyle == BackgroundStyle.BACKGROUND_STYLE_DEFAULT)
				{
					if (mMode == Mode.MODE_FIXED)
					{
						mBackgroundStyle = BackgroundStyle.BACKGROUND_STYLE_STATIC;
					}
					else
					{
						mBackgroundStyle = BackgroundStyle.BACKGROUND_STYLE_RIPPLE;
					}
				}

				if (mBackgroundStyle == BackgroundStyle.BACKGROUND_STYLE_STATIC)
				{
					mBackgroundOverlay.Visibility = ViewStates.Gone;
					mContainer.SetBackgroundColor(new Color(mBackgroundColor));
				}

				int screenWidth = Utils.getScreenWidth(Context);

				if (mMode == Mode.MODE_FIXED)
				{

					int[] widths = BottomNavigationHelper.getMeasurementsForFixedMode(Context, screenWidth, mBottomNavigationItems.Count, mScrollable);
					int itemWidth = widths[0];

					foreach (BottomNavigationItem currentItem in mBottomNavigationItems)
					{
						FixedBottomNavigationTab bottomNavigationTab = new FixedBottomNavigationTab(Context);
						setUpTab(bottomNavigationTab, currentItem, itemWidth, itemWidth);
					}

				}
				else if (mMode == Mode.MODE_SHIFTING)
				{

					int[] widths = BottomNavigationHelper.getMeasurementsForShiftingMode(Context, screenWidth, mBottomNavigationItems.Count, mScrollable);

					int itemWidth = widths[0];
					int itemActiveWidth = widths[1];

					foreach (BottomNavigationItem currentItem in mBottomNavigationItems)
					{
						ShiftingBottomNavigationTab bottomNavigationTab = new ShiftingBottomNavigationTab(Context);
						setUpTab(bottomNavigationTab, currentItem, itemWidth, itemActiveWidth);
					}
				}

				if (mBottomNavigationTabs.Count > mFirstSelectedPosition)
				{
					selectTabInternal(mFirstSelectedPosition, true, false, false);
				}
				else if (mBottomNavigationTabs.Count > 0)
				{
					selectTabInternal(0, true, false, false);
				}
			}
		}

		////////////////////////////////////////////////////////////////////////////////////////////////
		// Anytime Setter methods that can be called irrespective of whether we call initialise or not
		////////////////////////////////////////////////////////////////////////////////////////////////

		/// <param name="tabSelectedListener"> callback listener for tabs </param>
		/// <returns> this, to allow builder pattern </returns>
		public virtual BottomNavigationBar SetTabSelectedListener(IOnTabSelectedListener tabSelectedListener)
		{
			this.mTabSelectedListener = tabSelectedListener;
			return this;
		}

		/// <summary>
		/// ripple animation will be 2.5 times this animation duration.
		/// </summary>
		/// <param name="animationDuration"> animation duration for tab animations </param>
		/// <returns> this, to allow builder pattern </returns>
		public virtual BottomNavigationBar setAnimationDuration(int animationDuration)
		{
			this.mAnimationDuration = animationDuration;
			this.mRippleAnimationDuration = (int)(animationDuration * 2.5);
			return this;
		}

		/// <summary>
		/// Clears all stored data and this helps to re-initialise tabs from scratch
		/// </summary>
		public virtual void ClearAll()
		{
			mTabContainer.RemoveAllViews();
			mBottomNavigationTabs.Clear();
			mBottomNavigationItems.Clear();
			mBackgroundOverlay.Visibility = ViewStates.Gone;
			mContainer.SetBackgroundColor(Color.Transparent);
			mSelectedPosition = DEFAULT_SELECTED_POSITION;
		}

		///////////////////////////////////////////////////////////////////////////
		// Setter methods that should called only after initialise is called
		///////////////////////////////////////////////////////////////////////////

		/// <summary>
		/// Should be called only after initialization of BottomBar(i.e after calling initialize method)
		/// </summary>
		/// <param name="newPosition"> to select a tab after bottom navigation bar is initialised </param>
		public virtual void selectTab(int newPosition)
		{
			selectTab(newPosition, true);
		}

		/// <summary>
		/// Should be called only after initialization of BottomBar(i.e after calling initialize method)
		/// </summary>
		/// <param name="newPosition">  to select a tab after bottom navigation bar is initialised </param>
		/// <param name="callListener"> should this change call listener callbacks </param>
		public virtual void selectTab(int newPosition, bool callListener)
		{
			selectTabInternal(newPosition, false, callListener, callListener);
		}

		///////////////////////////////////////////////////////////////////////////
		// Internal Methods of the class
		///////////////////////////////////////////////////////////////////////////

		/// <summary>
		/// Internal method to setup tabs
		/// </summary>
		/// <param name="bottomNavigationTab"> Tab item </param>
		/// <param name="currentItem">         data structure for tab item </param>
		/// <param name="itemWidth">           tab item in-active width </param>
		/// <param name="itemActiveWidth">     tab item active width </param>
		private void setUpTab(BottomNavigationTab bottomNavigationTab, BottomNavigationItem currentItem, int itemWidth, int itemActiveWidth)
		{
			bottomNavigationTab.InactiveWidth = itemWidth;
			bottomNavigationTab.ActiveWidth = itemActiveWidth;
			bottomNavigationTab.Position = mBottomNavigationItems.IndexOf(currentItem);

			bottomNavigationTab.SetOnClickListener(new OnClickListenerAnonymousInnerClass(this));

			mBottomNavigationTabs.Add(bottomNavigationTab);

			BottomNavigationHelper.bindTabWithData(currentItem, bottomNavigationTab, this);

			bottomNavigationTab.initialise(mBackgroundStyle == BackgroundStyle.BACKGROUND_STYLE_STATIC);

			mTabContainer.AddView(bottomNavigationTab);
		}

		private class OnClickListenerAnonymousInnerClass :Java.Lang.Object, IOnClickListener
		{
			private readonly BottomNavigationBar outerInstance;

			public OnClickListenerAnonymousInnerClass(BottomNavigationBar outerInstance)
			{
				this.outerInstance = outerInstance;
			}

			public void OnClick(View v)
			{
				BottomNavigationTab bottomNavigationTabView = (BottomNavigationTab) v;
				outerInstance.selectTabInternal(bottomNavigationTabView.Position, false, true, false);
			}
		}

		/// <summary>
		/// Internal Method to select a tab
		/// </summary>
		/// <param name="newPosition">     to select a tab after bottom navigation bar is initialised </param>
		/// <param name="firstTab">        if firstTab the no ripple animation will be done </param>
		/// <param name="callListener">    is listener callbacks enabled for this change </param>
		/// <param name="forcedSelection"> if bottom navigation bar forced to select tab (in this case call on selected irrespective of previous state </param>
		private void selectTabInternal(int newPosition, bool firstTab, bool callListener, bool forcedSelection)
		{
			int oldPosition = mSelectedPosition;
			if (mSelectedPosition != newPosition)
			{
				if (mBackgroundStyle == BackgroundStyle.BACKGROUND_STYLE_STATIC)
				{
					if (mSelectedPosition != -1)
					{
						mBottomNavigationTabs[mSelectedPosition].unSelect(true, mAnimationDuration);
					}
					mBottomNavigationTabs[newPosition].Select(true, mAnimationDuration);
				}
				else if (mBackgroundStyle == BackgroundStyle.BACKGROUND_STYLE_RIPPLE)
				{
					if (mSelectedPosition != -1)
					{
						mBottomNavigationTabs[mSelectedPosition].unSelect(false, mAnimationDuration);
					}
					mBottomNavigationTabs[newPosition].Select(false, mAnimationDuration);

//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final BottomNavigationTab clickedView = mBottomNavigationTabs.get(newPosition);
					BottomNavigationTab clickedView = mBottomNavigationTabs[newPosition];
					if (firstTab)
					{
						// Running a ripple on the opening app won't be good so on firstTab this won't run.
						mContainer.SetBackgroundColor(new Color(clickedView.ActiveColor));
						mBackgroundOverlay.Visibility = ViewStates.Gone;
					}
					else
					{
						mBackgroundOverlay.Post(() =>
					{
//                            try {
						BottomNavigationHelper.setBackgroundWithRipple(clickedView, mContainer, mBackgroundOverlay, clickedView.ActiveColor, mRippleAnimationDuration);
//                            } catch (Exception e) {
//                                mContainer.setBackgroundColor(clickedView.getActiveColor());
//                                mBackgroundOverlay.setVisibility(ViewStates.Gone);
//                            }
					});
					}
				}
				mSelectedPosition = newPosition;
			}

            ThreadPool.QueueUserWorkItem(o=> {
                if (callListener)
                {
                    sendListenerCall(oldPosition, newPosition, forcedSelection);
                }
            });

		}

        /// <summary>
        /// 根据position设置菜单项。
        /// </summary>
        /// <param name="position"></param>
		public void SetTabCurrent(int position)
        {
            selectTabInternal(position, false, false, false);
        }
        
        /// <summary>
		/// Internal method used to send callbacks to listener
		/// </summary>
		/// <param name="oldPosition">     old selected tab position, -1 if this is first call </param>
		/// <param name="newPosition">     newly selected tab position </param>
		/// <param name="forcedSelection"> if bottom navigation bar forced to select tab (in this case call on selected irrespective of previous state </param>
		private void sendListenerCall(int oldPosition, int newPosition, bool forcedSelection)
		{
			if (mTabSelectedListener != null)
			{
	//                && oldPosition != -1) {
				if (forcedSelection)
				{
					mTabSelectedListener.OnTabSelected(newPosition);
				}
				else
				{
					if (oldPosition == newPosition)
					{
						mTabSelectedListener.OnTabReselected(newPosition);
					}
					else
					{
						mTabSelectedListener.OnTabSelected(newPosition);
						if (oldPosition != -1)
						{
							mTabSelectedListener.OnTabUnselected(oldPosition);
						}
					}
				}
			}
		}

		///////////////////////////////////////////////////////////////////////////
		// Animating methods
		///////////////////////////////////////////////////////////////////////////

		/// <summary>
		/// show BottomNavigationBar if it is hidden and hide if it is shown
		/// </summary>
		public virtual void Toggle()
		{
			toggle(true);
		}

		/// <summary>
		/// show BottomNavigationBar if it is hidden and hide if it is shown
		/// </summary>
		/// <param name="animate"> is animation enabled for toggle </param>
		public virtual void toggle(bool animate)
		{
			if (mIsHidden)
			{
				show(animate);
			}
			else
			{
				hide(animate);
			}
		}

		/// <summary>
		/// hide with animation
		/// </summary>
		public virtual void hide()
		{
			hide(true);
		}

		/// <param name="animate"> is animation enabled for hide </param>
		public virtual void hide(bool animate)
		{
			mIsHidden = true;
			setTranslationY(this.Height, animate);
		}

		/// <summary>
		/// show with animation
		/// </summary>
		public virtual void show()
		{
			show(true);
		}

		/// <param name="animate"> is animation enabled for show </param>
		public virtual void show(bool animate)
		{
			mIsHidden = false;
			setTranslationY(0, animate);
		}

		/// <param name="offset">  offset needs to be set </param>
		/// <param name="animate"> is animation enabled for translation </param>
		private void setTranslationY(int offset, bool animate)
		{
			if (animate)
			{
				animateOffset(offset);
			}
			else
			{
				if (mTranslationAnimator != null)
				{
					mTranslationAnimator.Cancel();
				}
				this.TranslationY = offset;
			}
		}

		/// <summary>
		/// Internal Method
		/// <para>
		/// used to set animation and
		/// takes care of cancelling current animation
		/// and sets duration and interpolator for animation
		/// 
		/// </para>
		/// </summary>
		/// <param name="offset"> translation offset that needs to set with animation </param>
//JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
//ORIGINAL LINE: private void animateOffset(final int offset)
		private void animateOffset(int offset)
		{
			if (mTranslationAnimator == null)
			{
				mTranslationAnimator = ViewCompat.Animate(this);
				mTranslationAnimator.SetDuration(mRippleAnimationDuration);
				mTranslationAnimator.SetInterpolator(INTERPOLATOR);
			}
			else
			{
				mTranslationAnimator.Cancel();
			}
			mTranslationAnimator.TranslationY(offset).Start();
		}

		public virtual bool Hidden
		{
			get
			{
				return mIsHidden;
			}
		}

		///////////////////////////////////////////////////////////////////////////
		// Behaviour Handing Handling
		///////////////////////////////////////////////////////////////////////////

		public virtual bool AutoHideEnabled
		{
			get
			{
				return mAutoHideEnabled;
			}
			set
			{
				this.mAutoHideEnabled = value;
			}
		}


		public virtual FloatingActionButton Fab
		{
			set
			{
				ViewGroup.LayoutParams layoutParams = value.LayoutParameters;
				if (layoutParams != null && layoutParams is CoordinatorLayout.LayoutParams)
				{
					CoordinatorLayout.LayoutParams coLayoutParams = (CoordinatorLayout.LayoutParams) layoutParams;
					BottomNavBarFabBehaviour bottomNavBarFabBehaviour = new BottomNavBarFabBehaviour();
					coLayoutParams.Behavior = bottomNavBarFabBehaviour;
				}
			}
		}

		// scheduled for next
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: private void setFab(Android.Support.Design.Widget.FloatingActionButton fab, @FabBehaviour int fabBehaviour)
		private void setFab(FloatingActionButton fab, int fabBehaviour)
		{
			ViewGroup.LayoutParams layoutParams = fab.LayoutParameters;
			if (layoutParams != null && layoutParams is CoordinatorLayout.LayoutParams)
			{
				CoordinatorLayout.LayoutParams coLayoutParams = (CoordinatorLayout.LayoutParams) layoutParams;
				BottomNavBarFabBehaviour bottomNavBarFabBehaviour = new BottomNavBarFabBehaviour();
				coLayoutParams.Behavior = bottomNavBarFabBehaviour;
			}
		}


		///////////////////////////////////////////////////////////////////////////
		// Getters
		///////////////////////////////////////////////////////////////////////////

		/// <returns> activeColor </returns>
		public virtual int ActiveColor
		{
			get
			{
				return mActiveColor;
			}
		}

		/// <returns> in-active color </returns>
		public virtual int InActiveColor
		{
			get
			{
				return mInActiveColor;
			}
		}

		/// <returns> background color </returns>
		public virtual int BackgroundColor
		{
			get
			{
				return mBackgroundColor;
			}
		}

		/// <returns> current selected position </returns>
		public virtual int CurrentSelectedPosition
		{
			get
			{
				return mSelectedPosition;
			}
		}

		/// <returns> animation duration </returns>
		public virtual int AnimationDuration
		{
			get
			{
				return mAnimationDuration;
			}
		}

		///////////////////////////////////////////////////////////////////////////
		// Listener interfaces
		///////////////////////////////////////////////////////////////////////////

		/// <summary>
		/// Callback interface invoked when a tab's selection state changes.
		/// </summary>
		public interface IOnTabSelectedListener
		{

			/// <summary>
			/// Called when a tab enters the selected state.
			/// </summary>
			/// <param name="position"> The position of the tab that was selected </param>
			void OnTabSelected(int position);

			/// <summary>
			/// Called when a tab exits the selected state.
			/// </summary>
			/// <param name="position"> The position of the tab that was unselected </param>
			void OnTabUnselected(int position);

			/// <summary>
			/// Called when a tab that is already selected is chosen again by the user. Some applications
			/// may use this action to return to the top level of a category.
			/// </summary>
			/// <param name="position"> The position of the tab that was reselected. </param>
			void OnTabReselected(int position);
		}

		/// <summary>
		/// Simple implementation of the OnTabSelectedListener interface with stub implementations of each method.
		/// Extend this if you do not intend to override every method of OnTabSelectedListener.
		/// </summary>
		public class SimpleOnTabSelectedListener : IOnTabSelectedListener
		{
			public virtual void OnTabSelected(int position)
			{
			}

			public virtual void OnTabUnselected(int position)
			{
			}

			public virtual void OnTabReselected(int position)
			{
			}
		}
	}

}