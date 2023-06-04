using System.Runtime.CompilerServices;

namespace MAUI_Library.Models.Enums;

public enum FilterOptionsEnum
{
    MyEvents,
    Closest,
    MostRecent
}

public static class EnumExtension
{

    public static string ToString(this FilterOptionsEnum value)
    {
        switch (value)
        {
            case FilterOptionsEnum.MyEvents:
                return "My events";

            case FilterOptionsEnum.Closest:
                return "Closest";

            case FilterOptionsEnum.MostRecent:
                return "Most recent";

            default:
                return value.ToString();
        }
    }
}

    

