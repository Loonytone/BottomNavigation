using Math = Java.Lang.Math;
using Context = Android.Content.Context;
using TypedArray = Android.Content.Res.TypedArray;
using Point = Android.Graphics.Point;
using TypedValue = Android.Util.TypedValue;
using Android.Views;
using Android.Runtime;
using Android.Util;

namespace Com.Loonytone.Droid.BottomNavigation.utils
{

    /// <summary>
    /// Class description : These are common utils and can be used for other projects as well
    /// 
    /// @author ashokvarma
    /// @version 1.0
    /// @since 19 Mar 2016
    /// </summary>
    public class Utils
    {

        private Utils()
        {
        }

        /// <param name="context"> used to get system services </param>
        /// <returns> screenWidth in pixels </returns>
        public static int getScreenWidth(Context context)
        {
            IWindowManager wm = context.GetSystemService(Context.WindowService).JavaCast<IWindowManager>();
            Point size = new Point();
            wm.DefaultDisplay.GetSize(size);
            return size.X;
        }

        /// <summary>
        /// This method can be extended to get all android attributes color, string, dimension ...etc
        /// </summary>
        /// <param name="context">          used to fetch android attribute </param>
        /// <param name="androidAttribute"> attribute codes like Resource.Attribute.colorAccent </param>
        /// <returns> in this case color of android attribute </returns>
        public static int fetchContextColor(Context context, int androidAttribute)
        {
            TypedValue typedValue = new TypedValue();

            TypedArray a = context.ObtainStyledAttributes(typedValue.Data, new int[] { androidAttribute });
            int color = a.GetColor(0, 0);

            a.Recycle();

            return color;
        }

        /// <param name="context"> used to fetch display metrics </param>
        /// <param name="dp">      dp value </param>
        /// <returns> pixel value </returns>
        public static int dp2px(Context context, float dp)
        {
            float px = TypedValue.ApplyDimension(ComplexUnitType.Dip, dp, context.Resources.DisplayMetrics);
            return Math.Round(px);
        }
    }

}