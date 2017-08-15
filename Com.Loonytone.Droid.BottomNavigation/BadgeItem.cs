
using Context = Android.Content.Context;
using Color = Android.Graphics.Color;
using ContextCompat = Android.Support.V4.Content.ContextCompat;
using ViewCompat = Android.Support.V4.View.ViewCompat;
using ViewPropertyAnimatorCompat = Android.Support.V4.View.ViewPropertyAnimatorCompat;
using IViewPropertyAnimatorListener = Android.Support.V4.View.IViewPropertyAnimatorListener;
using Android.Views;
using Android.Widget;
using Java.Lang.Ref;

namespace Com.Loonytone.Droid.BottomNavigation
{
	/// <summary>
	/// Class description : Holds and manages data for badges
	/// (i.e data structure which holds all data to paint a badge and updates badges when changes are made)
	/// 
	/// @author ashokvarma
	/// @version 1.0
	/// @since 21 Apr 2016
	/// </summary>
	public class BadgeItem
	{

		private int mBackgroundColorResource;
		private string mBackgroundColorCode;
		private int mBackgroundColor = Color.Red;

		private int mTextColorResource;
		private string mTextColorCode;
		private int mTextColor = Color.White;

		private string mText;

		private int mBorderColorResource;
		private string mBorderColorCode;
		private int mBorderColor = Color.White;

		private int mBorderWidth = 0;

		private GravityFlags mGravity = GravityFlags.Top | GravityFlags.End;
		private bool mHideOnSelect;

        //TextView
        private WeakReference mTextViewRef;

		private bool mIsHidden = false;

		private int mAnimationDuration = 200;


		///////////////////////////////////////////////////////////////////////////
		// Public setter methods
		///////////////////////////////////////////////////////////////////////////

		/// <param name="colorResource"> resource for background color </param>
		/// <returns> this, to allow builder pattern </returns>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: public BadgeItem setBackgroundColorResource(@ColorRes int colorResource)
		public virtual BadgeItem SetBackgroundColorResource(int colorResource)
		{
			this.mBackgroundColorResource = colorResource;
			refreshDrawable();
			return this;
		}

		/// <param name="colorCode"> color code for background color </param>
		/// <returns> this, to allow builder pattern </returns>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: public BadgeItem setBackgroundColor(@Nullable String colorCode)
		public virtual BadgeItem SetBackgroundColor(string colorCode)
		{
			this.mBackgroundColorCode = colorCode;
			refreshDrawable();
			return this;
		}

		/// <param name="color"> background color </param>
		/// <returns> this, to allow builder pattern </returns>
		public virtual BadgeItem SetBackgroundColor(int color)
		{
			this.mBackgroundColor = color;
			refreshDrawable();
			return this;
		}

		/// <param name="colorResource"> resource for text color </param>
		/// <returns> this, to allow builder pattern </returns>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: public BadgeItem setTextColorResource(@ColorRes int colorResource)
		public virtual BadgeItem SetTextColorResource(int colorResource)
		{
			this.mTextColorResource = colorResource;
			SetTextColor();
			return this;
		}

		/// <param name="colorCode"> color code for text color </param>
		/// <returns> this, to allow builder pattern </returns>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: public BadgeItem setTextColor(@Nullable String colorCode)
		public virtual BadgeItem SetTextColor(string colorCode)
		{
			this.mTextColorCode = colorCode;
			SetTextColor();
			return this;
		}

		/// <param name="color"> text color </param>
		/// <returns> this, to allow builder pattern </returns>
		public virtual BadgeItem SetTextColor(int color)
		{
			this.mTextColor = color;
			SetTextColor();
			return this;
		}

		/// <param name="text"> text to be set in badge (this shouldn't be empty text) </param>
		/// <returns> this, to allow builder pattern </returns>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: public BadgeItem setText(@Nullable CharSequence text)
		public virtual BadgeItem SetText(string text)
		{
			this.mText = text;
			if (WeakReferenceValid)
			{
				TextView textView = (TextView)mTextViewRef.Get();
				if (!string.IsNullOrWhiteSpace(text))
				{
					textView.Text = text;
				}
			}
			return this;
		}

		/// <param name="colorResource"> resource for border color </param>
		/// <returns> this, to allow builder pattern </returns>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: public BadgeItem setBorderColorResource(@ColorRes int colorResource)
		public virtual BadgeItem SetBorderColorResource(int colorResource)
		{
			this.mBorderColorResource = colorResource;
			refreshDrawable();
			return this;
		}

		/// <param name="colorCode"> color code for border color </param>
		/// <returns> this, to allow builder pattern </returns>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: public BadgeItem setBorderColor(@Nullable String colorCode)
		public virtual BadgeItem SetBorderColor(string colorCode)
		{
			this.mBorderColorCode = colorCode;
			refreshDrawable();
			return this;
		}

		/// <param name="color"> border color </param>
		/// <returns> this, to allow builder pattern </returns>
		public virtual BadgeItem SetBorderColor(int color)
		{
			this.mBorderColor = color;
			refreshDrawable();
			return this;
		}

		/// <param name="borderWidth"> border width in pixels </param>
		/// <returns> this, to allow builder pattern </returns>
		public virtual BadgeItem SetBorderWidth(int borderWidth)
		{
			this.mBorderWidth = borderWidth;
			refreshDrawable();
			return this;
		}

		/// <param name="gravity"> gravity of badge (TOP|LEFT ..etc) </param>
		/// <returns> this, to allow builder pattern </returns>
		public virtual BadgeItem SetGravity(GravityFlags gravity)
		{
			this.mGravity = gravity;
			if (WeakReferenceValid)
			{
				TextView textView = (TextView)mTextViewRef.Get();
				FrameLayout.LayoutParams layoutParams = (FrameLayout.LayoutParams) textView.LayoutParameters;
				layoutParams.Gravity = gravity;
				textView.LayoutParameters = layoutParams;
			}
			return this;
		}

		/// <param name="hideOnSelect"> if true hides badge on tab selection </param>
		/// <returns> this, to allow builder pattern </returns>
		public virtual BadgeItem SetHideOnSelect(bool hideOnSelect)
		{
			this.mHideOnSelect = hideOnSelect;
			return this;
		}

		/// <param name="animationDuration"> hide and show animation time </param>
		/// <returns> this, to allow builder pattern </returns>
		public virtual BadgeItem SetAnimationDuration(int animationDuration)
		{
			this.mAnimationDuration = animationDuration;
			return this;
		}

		///////////////////////////////////////////////////////////////////////////
		// Library only access method
		///////////////////////////////////////////////////////////////////////////

		/// <summary>
		/// Internal method used to update view when ever changes are made
		/// </summary>
		/// <param name="mTextView"> badge textView </param>
		/// <returns> this, to allow builder pattern </returns>
		public virtual BadgeItem SetTextView(TextView mTextView)
		{
			this.mTextViewRef = new WeakReference(mTextView);
			return this;
		}

		/// <param name="context"> to fetch color </param>
		/// <returns> background color </returns>
		public virtual int GetBackgroundColor(Context context)
		{
			if (this.mBackgroundColorResource != 0)
			{
				return ContextCompat.GetColor(context, mBackgroundColorResource);
			}
			else if (!string.IsNullOrWhiteSpace(mBackgroundColorCode))
			{
				return Color.ParseColor(mBackgroundColorCode);
			}
			else
			{
				return mBackgroundColor;
			}
		}

		/// <param name="context"> to fetch color </param>
		/// <returns> text color </returns>
		public virtual int GetTextColor(Context context)
		{
			if (this.mTextColorResource != 0)
			{
				return ContextCompat.GetColor(context, mTextColorResource);
			}
			else if (!string.IsNullOrWhiteSpace(mTextColorCode))
			{
				return Color.ParseColor(mTextColorCode);
			}
			else
			{
				return mTextColor;
			}
		}

		/// <returns> text that needs to be set in badge </returns>
		public virtual string Text
		{
			get
			{
				return mText;
			}
		}

		/// <param name="context"> to fetch color </param>
		/// <returns> border color </returns>
		public virtual int GetBorderColor(Context context)
		{
			if (this.mBorderColorResource != 0)
			{
				return ContextCompat.GetColor(context, mBorderColorResource);
			}
			else if (!string.IsNullOrWhiteSpace(mBorderColorCode))
			{
				return Color.ParseColor(mBorderColorCode);
			}
			else
			{
				return mBorderColor;
			}
		}

		/// <returns> border width </returns>
		public virtual int BorderWidth
		{
			get
			{
				return mBorderWidth;
			}
		}

		/// <returns> gravity of badge </returns>
		public virtual GravityFlags Gravity
		{
			get
			{
				return mGravity;
			}
		}

		/// <returns> should hide on selection ? </returns>
		protected virtual bool HideOnSelect
		{
			get
			{
				return mHideOnSelect;
			}
		}

		/// <returns> reference to text-view </returns>
		protected virtual WeakReference getTextView()
		{
			return mTextViewRef;
		}


		///////////////////////////////////////////////////////////////////////////
		// Internal Methods
		///////////////////////////////////////////////////////////////////////////

		private void refreshDrawable()
		{
			if (WeakReferenceValid)
			{
				TextView textView = (TextView)mTextViewRef.Get();
				textView.SetBackgroundDrawable(BottomNavigationHelper.getBadgeDrawable(this, textView.Context));
			}
		}

		private void SetTextColor()
		{
			if (WeakReferenceValid)
			{
				TextView textView = (TextView)mTextViewRef.Get();
				textView.SetTextColor(new Color(GetTextColor(textView.Context)));
			}
		}

		private bool WeakReferenceValid
		{
			get
			{
				return mTextViewRef != null && mTextViewRef.Get() != null;
			}
		}

		///////////////////////////////////////////////////////////////////////////
		// Internal call back methods
		///////////////////////////////////////////////////////////////////////////

		/// <summary>
		/// callback from bottom navigation tab when it is selected
		/// </summary>
		internal virtual void Select()
		{
			if (mHideOnSelect)
			{
				Hide(true);
			}
		}

		/// <summary>
		/// callback from bottom navigation tab when it is un-selected
		/// </summary>
		internal virtual void UnSelect()
		{
			if (mHideOnSelect)
			{
				Show(true);
			}
		}

		///////////////////////////////////////////////////////////////////////////
		// Public functionality methods
		///////////////////////////////////////////////////////////////////////////

		/// <returns> this, to allow builder pattern </returns>
		public virtual BadgeItem Toggle()
		{
			return Toggle(true);
		}

		/// <param name="animate"> whether to animate the change </param>
		/// <returns> this, to allow builder pattern </returns>
		public virtual BadgeItem Toggle(bool animate)
		{
			if (mIsHidden)
			{
				return Show(animate);
			}
			else
			{
				return Hide(animate);
			}
		}

		/// <returns> this, to allow builder pattern </returns>
		public virtual BadgeItem Show()
		{
			return Show(true);
		}

		/// <param name="animate"> whether to animate the change </param>
		/// <returns> this, to allow builder pattern </returns>
		public virtual BadgeItem Show(bool animate)
		{
			mIsHidden = false;
			if (WeakReferenceValid)
			{
				TextView textView = (TextView)mTextViewRef.Get();
				if (animate)
				{
					textView.ScaleX = 0;
					textView.ScaleY = 0;
					textView.Visibility = ViewStates.Visible;
					ViewPropertyAnimatorCompat animatorCompat = ViewCompat.Animate(textView);
					animatorCompat.Cancel();
					animatorCompat.SetDuration(mAnimationDuration);
					animatorCompat.ScaleX(1).ScaleY(1);
					animatorCompat.SetListener(null);
					animatorCompat.Start();
				}
				else
				{
					textView.ScaleX = 1;
					textView.ScaleY = 1;
					textView.Visibility = ViewStates.Visible;
				}
			}
			return this;
		}

		/// <returns> this, to allow builder pattern </returns>
		public virtual BadgeItem Hide()
		{
			return Hide(true);
		}

		/// <param name="animate"> whether to animate the change </param>
		/// <returns> this, to allow builder pattern </returns>
		public virtual BadgeItem Hide(bool animate)
		{
			mIsHidden = true;
			if (WeakReferenceValid)
			{
				TextView textView = (TextView)mTextViewRef.Get();
				if (animate)
				{
					ViewPropertyAnimatorCompat animatorCompat = ViewCompat.Animate(textView);
					animatorCompat.Cancel();
					animatorCompat.SetDuration(mAnimationDuration);
					animatorCompat.ScaleX(0).ScaleY(0);
					animatorCompat.SetListener(new ViewPropertyAnimatorListenerAnonymousInnerClass(this));
					animatorCompat.Start();
				}
				else
				{
					textView.Visibility = ViewStates.Gone;
				}
			}
			return this;
		}

		private class ViewPropertyAnimatorListenerAnonymousInnerClass :Java.Lang.Object, IViewPropertyAnimatorListener
		{
			private readonly BadgeItem outerInstance;

			public ViewPropertyAnimatorListenerAnonymousInnerClass(BadgeItem outerInstance)
			{
				this.outerInstance = outerInstance;
			}

			public void OnAnimationStart(View view)
			{
				// Empty body
			}

			public void OnAnimationEnd(View view)
			{
				view.Visibility = ViewStates.Gone;
			}

			public void OnAnimationCancel(View view)
			{
				view.Visibility = ViewStates.Gone;
			}
		}

		/// <returns> if the badge is hidden </returns>
		public virtual bool Hidden
		{
			get
			{
				return mIsHidden;
			}
		}
	}

}