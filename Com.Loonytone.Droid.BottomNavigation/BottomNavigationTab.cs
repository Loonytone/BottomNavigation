using ValueAnimator = Android.Animation.ValueAnimator;
using Context = Android.Content.Context;
using ColorStateList = Android.Content.Res.ColorStateList;
using Drawable = Android.Graphics.Drawables.Drawable;
using StateListDrawable = Android.Graphics.Drawables.StateListDrawable;
using DrawableCompat = Android.Support.V4.Graphics.Drawable.DrawableCompat;
using IAttributeSet = Android.Util.IAttributeSet;
using Android.Views;
using Android.Widget;
using Android.Graphics;

namespace Com.Loonytone.Droid.BottomNavigation
{

    /// <summary>
    /// Class description
    /// 
    /// @author ashokvarma
    /// @version 1.0 </summary>
    /// <seealso cref= FrameLayout
    /// @since 19 Mar 2016 </seealso>
    internal class BottomNavigationTab : FrameLayout
	{


		protected int paddingTopActive;
		protected int paddingTopInActive;

		protected int mPosition;
		protected int mActiveColor;
		protected int mInActiveColor;
		protected int mBackgroundColor;
		protected int mActiveWidth;
		protected int mInActiveWidth;

		protected Drawable mCompactIcon;
		protected Drawable mCompactInActiveIcon;
		protected bool isInActiveIconSet = false;
		protected string mLabel;

		protected BadgeItem badgeItem;

		internal bool isActive = false;

		internal View containerView;
		internal TextView labelView;
		internal ImageView iconView;
		internal TextView badgeView;

		public BottomNavigationTab(Context context) : base(context)
		{
			init();
		}

		public BottomNavigationTab(Context context, IAttributeSet attrs) : base(context, attrs)
		{
			init();
		}

		public BottomNavigationTab(Context context, IAttributeSet attrs, int defStyleAttr) : base(context, attrs, defStyleAttr)
		{
			init();
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @TargetApi(android.os.BuildVersionCodes.LOLLIPOP) public BottomNavigationTab(Android.Content.Context context, Android.Util.IAttributeSet attrs, int defStyleAttr, int defStyleRes)
		public BottomNavigationTab(Context context, IAttributeSet attrs, int defStyleAttr, int defStyleRes) : base(context, attrs, defStyleAttr, defStyleRes)
		{
			init();
		}

		internal virtual void init()
		{
			LayoutParameters = new ViewGroup.LayoutParams(ViewGroup.LayoutParams.WrapContent, ViewGroup.LayoutParams.MatchParent);
		}

		public virtual int ActiveWidth
		{
			set
			{
				mActiveWidth = value;
			}
		}

		public virtual int InactiveWidth
		{
			set
			{
				mInActiveWidth = value;
				ViewGroup.LayoutParams @params = LayoutParameters;
				@params.Width = mInActiveWidth;
				LayoutParameters = @params;
			}
		}

		public virtual Drawable Icon
		{
			set
			{
				mCompactIcon = DrawableCompat.Wrap(value);
			}
		}

		public virtual Drawable InactiveIcon
		{
			set
			{
				mCompactInActiveIcon = DrawableCompat.Wrap(value);
				isInActiveIconSet = true;
			}
		}

		public virtual string Label
		{
			set
			{
				mLabel = value;
				labelView.Text = value;
			}
		}

		public virtual int ActiveColor
		{
			set
			{
				mActiveColor = value;
			}
			get
			{
				return mActiveColor;
			}
		}


		public virtual int InactiveColor
		{
			set
			{
				mInActiveColor = value;
				labelView.SetTextColor(new Color(value));
			}
		}

		public virtual int ItemBackgroundColor
		{
			set
			{
				mBackgroundColor = value;
			}
		}

		public virtual int Position
		{
			set
			{
				mPosition = value;
			}
			get
			{
				return mPosition;
			}
		}

		public virtual BadgeItem BadgeItem
		{
			set
			{
				this.badgeItem = value;
			}
		}


		public virtual void Select(bool setActiveColor, int animationDuration)
		{
            isActive = true;

            ValueAnimator animator = ValueAnimator.OfInt(containerView.PaddingTop, paddingTopActive);
            animator.AddUpdateListener(new AnimatorUpdateListenerAnonymousInnerClass(this));
            animator.SetDuration(animationDuration);
            animator.Start();

            iconView.Selected = true;
            if (setActiveColor)
            {
                labelView.SetTextColor(new Color(mActiveColor));
            }
            else
            {
                labelView.SetTextColor(new Color(mBackgroundColor));
            }

            if (badgeItem != null)
            {
                badgeItem.Select();
            }

        }

		private class AnimatorUpdateListenerAnonymousInnerClass : Java.Lang.Object, ValueAnimator.IAnimatorUpdateListener
		{
			private readonly BottomNavigationTab outerInstance;

			public AnimatorUpdateListenerAnonymousInnerClass(BottomNavigationTab outerInstance)
			{
				this.outerInstance = outerInstance;
			}

			public void OnAnimationUpdate(ValueAnimator valueAnimator)
			{
				outerInstance.containerView.SetPadding(outerInstance.containerView.PaddingLeft, (int) valueAnimator.AnimatedValue, outerInstance.containerView.PaddingRight, outerInstance.containerView.PaddingBottom);
			}
		}

		public virtual void unSelect(bool setActiveColor, int animationDuration)
		{
			isActive = false;

			ValueAnimator animator = ValueAnimator.OfInt(containerView.PaddingTop, paddingTopInActive);
			animator.AddUpdateListener(new AnimatorUpdateListenerAnonymousInnerClass2(this));
			animator.SetDuration(animationDuration);
			animator.Start();

			labelView.SetTextColor(new Color(mInActiveColor));
			iconView.Selected = false;

			if (badgeItem != null)
			{
				badgeItem.UnSelect();
			}
		}

		private class AnimatorUpdateListenerAnonymousInnerClass2 : Java.Lang.Object, ValueAnimator.IAnimatorUpdateListener
		{
			private readonly BottomNavigationTab outerInstance;

			public AnimatorUpdateListenerAnonymousInnerClass2(BottomNavigationTab outerInstance)
			{
				this.outerInstance = outerInstance;
			}

			public void OnAnimationUpdate(ValueAnimator valueAnimator)
			{
				outerInstance.containerView.SetPadding(outerInstance.containerView.PaddingLeft, (int) valueAnimator.AnimatedValue, outerInstance.containerView.PaddingRight, outerInstance.containerView.PaddingBottom);
			}
		}

		public virtual void initialise(bool setActiveColor)
		{
			iconView.Selected = false;
			if (isInActiveIconSet)
			{
				StateListDrawable states = new StateListDrawable();
				states.AddState(new int[]{Android.Resource.Attribute.StateSelected }, mCompactIcon);
				states.AddState(new int[]{-Android.Resource.Attribute.StateSelected }, mCompactInActiveIcon);
				states.AddState(new int[]{}, mCompactInActiveIcon);
				iconView.SetImageDrawable(states);
			}
			else
			{
				if (setActiveColor)
				{
					DrawableCompat.SetTintList(mCompactIcon, new ColorStateList(new int[][]
					{
						new int[]{Android.Resource.Attribute.StateSelected},
						new int[]{-Android.Resource.Attribute.StateSelected},
						new int[]{}
					}, new int[]{mActiveColor, mInActiveColor, mInActiveColor}));
				}
				else
				{
					DrawableCompat.SetTintList(mCompactIcon, new ColorStateList(new int[][]
					{
						new int[]{Android.Resource.Attribute.StateSelected},
						new int[]{-Android.Resource.Attribute.StateSelected},
						new int[]{}
					}, new int[]{mBackgroundColor, mInActiveColor, mInActiveColor}));
				}
				iconView.SetImageDrawable(mCompactIcon);
			}
		}
	}

}