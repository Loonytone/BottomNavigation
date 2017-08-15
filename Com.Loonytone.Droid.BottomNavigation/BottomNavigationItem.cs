
using Context = Android.Content.Context;
using Color = Android.Graphics.Color;
using Drawable = Android.Graphics.Drawables.Drawable;
using ContextCompat = Android.Support.V4.Content.ContextCompat;


namespace Com.Loonytone.Droid.BottomNavigation
{

	/// <summary>
	/// Class description : Holds data for tabs (i.e data structure which holds all data to paint a tab)
	/// 
	/// @author ashokvarma
	/// @version 1.0
	/// @since 19 Mar 2016
	/// </summary>
	public class BottomNavigationItem
	{

		protected int mIconResource;
		protected Drawable mIcon;

		protected int mInactiveIconResource;
		protected Drawable mInactiveIcon;
		protected bool inActiveIconAvailable = false;

		protected int mTitleResource;
		protected string mTitle;

		protected int mActiveColorResource;
		protected string mActiveColorCode;
		protected int mActiveColor;

		protected int mInActiveColorResource;
		protected string mInActiveColorCode;
		protected int mInActiveColor;

		protected BadgeItem mBadgeItem;

		/// <param name="mIconResource"> resource for the Tab icon. </param>
		/// <param name="mTitle">        title for the Tab. </param>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: public BottomNavigationItem(@DrawableRes int mIconResource, @NonNull String mTitle)
		public BottomNavigationItem(int mIconResource, string mTitle)
		{
			this.mIconResource = mIconResource;
			this.mTitle = mTitle;
		}

		/// <param name="mIcon">  drawable icon for the Tab. </param>
		/// <param name="mTitle"> title for the Tab. </param>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: public BottomNavigationItem(Android.Graphics.Drawables.Drawable mIcon, @NonNull String mTitle)
		public BottomNavigationItem(Drawable mIcon, string mTitle)
		{
			this.mIcon = mIcon;
			this.mTitle = mTitle;
		}

		/// <param name="mIcon">          drawable icon for the Tab. </param>
		/// <param name="mTitleResource"> resource for the title. </param>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: public BottomNavigationItem(Android.Graphics.Drawables.Drawable mIcon, @StringRes int mTitleResource)
		public BottomNavigationItem(Drawable mIcon, int mTitleResource)
		{
			this.mIcon = mIcon;
			this.mTitleResource = mTitleResource;
		}

		/// <param name="mIconResource">  resource for the Tab icon. </param>
		/// <param name="mTitleResource"> resource for the title. </param>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: public BottomNavigationItem(@DrawableRes int mIconResource, @StringRes int mTitleResource)
		public BottomNavigationItem(int mIconResource, int mTitleResource)
		{
			this.mIconResource = mIconResource;
			this.mTitleResource = mTitleResource;
		}

		/// <summary>
		/// By default library will switch the color of icon provided (in between active and in-active icons)
		/// This method is used, if people need to set different icons for active and in-active modes.
		/// </summary>
		/// <param name="mInactiveIcon"> in-active drawable icon </param>
		/// <returns> this, to allow builder pattern </returns>
		public virtual BottomNavigationItem setInactiveIcon(Drawable mInactiveIcon)
		{
			if (mInactiveIcon != null)
			{
				this.mInactiveIcon = mInactiveIcon;
				inActiveIconAvailable = true;
			}
			return this;
		}

		/// <summary>
		/// By default library will switch the color of icon provided (in between active and in-active icons)
		/// This method is used, if people need to set different icons for active and in-active modes.
		/// </summary>
		/// <param name="mInactiveIconResource"> resource for the in-active icon. </param>
		/// <returns> this, to allow builder pattern </returns>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: public BottomNavigationItem setInactiveIconResource(@DrawableRes int mInactiveIconResource)
		public virtual BottomNavigationItem SetInactiveIconResource(int mInactiveIconResource)
		{
			this.mInactiveIconResource = mInactiveIconResource;
			inActiveIconAvailable = true;
			return this;
		}


		/// <param name="colorResource"> resource for active color </param>
		/// <returns> this, to allow builder pattern </returns>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: public BottomNavigationItem setActiveColorResource(@ColorRes int colorResource)
		public virtual BottomNavigationItem SetActiveColorResource(int colorResource)
		{
			this.mActiveColorResource = colorResource;
			return this;
		}

		/// <param name="colorCode"> color code for active color </param>
		/// <returns> this, to allow builder pattern </returns>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: public BottomNavigationItem setActiveColor(@Nullable String colorCode)
		public virtual BottomNavigationItem setActiveColor(string colorCode)
		{
			this.mActiveColorCode = colorCode;
			return this;
		}

		/// <param name="color"> active color </param>
		/// <returns> this, to allow builder pattern </returns>
		public virtual BottomNavigationItem setActiveColor(int color)
		{
			this.mActiveColor = color;
			return this;
		}

		/// <param name="colorResource"> resource for in-active color </param>
		/// <returns> this, to allow builder pattern </returns>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: public BottomNavigationItem setInActiveColorResource(@ColorRes int colorResource)
		public virtual BottomNavigationItem SetInActiveColorResource(int colorResource)
		{
			this.mInActiveColorResource = colorResource;
			return this;
		}

		/// <param name="colorCode"> color code for in-active color </param>
		/// <returns> this, to allow builder pattern </returns>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: public BottomNavigationItem setInActiveColor(@Nullable String colorCode)
		public virtual BottomNavigationItem setInActiveColor(string colorCode)
		{
			this.mInActiveColorCode = colorCode;
			return this;
		}

		/// <param name="color"> in-active color </param>
		/// <returns> this, to allow builder pattern </returns>
		public virtual BottomNavigationItem setInActiveColor(int color)
		{
			this.mInActiveColor = color;
			return this;
		}

		/// <param name="badgeItem"> badge that needs to be displayed for this tab </param>
		/// <returns> this, to allow builder pattern </returns>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: public BottomNavigationItem setBadgeItem(@Nullable BadgeItem badgeItem)
		public virtual BottomNavigationItem SetBadgeItem(BadgeItem badgeItem)
		{
			this.mBadgeItem = badgeItem;
			return this;
		}

		/// <param name="context"> to fetch drawable </param>
		/// <returns> icon drawable </returns>
		public virtual Drawable getIcon(Context context)
		{
			if (this.mIconResource != 0)
			{
				return ContextCompat.GetDrawable(context, this.mIconResource);
			}
			else
			{
				return this.mIcon;
			}
		}

		/// <param name="context"> to fetch resource </param>
		/// <returns> title string </returns>
		public virtual string getTitle(Context context)
		{
			if (this.mTitleResource != 0)
			{
				return context.GetString(this.mTitleResource);
			}
			else
			{
				return this.mTitle;
			}
		}

		/// <param name="context"> to fetch resources </param>
		/// <returns> in-active icon drawable </returns>
		public virtual Drawable getInactiveIcon(Context context)
		{
			if (this.mInactiveIconResource != 0)
			{
				return ContextCompat.GetDrawable(context, this.mInactiveIconResource);
			}
			else
			{
				return this.mInactiveIcon;
			}
		}

		/// <returns> if in-active icon is set </returns>
		public virtual bool InActiveIconAvailable
		{
			get
			{
				return inActiveIconAvailable;
			}
		}

		/// <param name="context"> to fetch color </param>
		/// <returns> active color (or) -1 if no color is specified </returns>
		public virtual int getActiveColor(Context context)
		{
			if (this.mActiveColorResource != 0)
			{
				return ContextCompat.GetColor(context, mActiveColorResource);
			}
			else if (!string.IsNullOrWhiteSpace(mActiveColorCode))
			{
				return Color.ParseColor(mActiveColorCode);
			}
			else if (this.mActiveColor != 0)
			{
				return mActiveColor;
			}
			else
			{
				return -1;
			}
		}

		/// <param name="context"> to fetch color </param>
		/// <returns> in-active color (or) -1 if no color is specified </returns>
		public virtual int getInActiveColor(Context context)
		{
			if (this.mInActiveColorResource != 0)
			{
				return ContextCompat.GetColor(context, mInActiveColorResource);
			}
			else if (!string.IsNullOrWhiteSpace(mInActiveColorCode))
			{
				return Color.ParseColor(mInActiveColorCode);
			}
			else if (this.mInActiveColor != 0)
			{
				return mInActiveColor;
			}
			else
			{
				return -1;
			}
		}

		/// <returns> badge item that needs to set to respective view </returns>
		public virtual BadgeItem BadgeItem
		{
			get
			{
				return mBadgeItem;
			}
		}

	}

}