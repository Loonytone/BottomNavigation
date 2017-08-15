namespace Com.Loonytone.Droid.BottomNavigation
{
    public enum Mode
    {
        MODE_DEFAULT = 0,
        MODE_FIXED = 1,
        MODE_SHIFTING = 2,
    }

    public enum BackgroundStyle
    {
        BACKGROUND_STYLE_DEFAULT = 0,
        BACKGROUND_STYLE_STATIC = 1,
        BACKGROUND_STYLE_RIPPLE = 2,
    }




    //JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
    //ORIGINAL LINE: @IntDef({FAB_BEHAVIOUR_TRANSLATE_AND_STICK, FAB_BEHAVIOUR_DISAPPEAR, FAB_BEHAVIOUR_TRANSLATE_OUT}) @Retention(RetentionPolicy.SOURCE) public class FabBehaviour extends System.Attribute
    public enum FabBehaviour
    {
        FAB_BEHAVIOUR_TRANSLATE_AND_STICK = 0,
        FAB_BEHAVIOUR_DISAPPEAR = 1,
        FAB_BEHAVIOUR_TRANSLATE_OUT = 2,
    }
}