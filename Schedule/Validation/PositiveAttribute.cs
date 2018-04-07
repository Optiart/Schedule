using System.ComponentModel.DataAnnotations;

namespace Schedule
{
    public class PositiveAttribute : ValidationAttribute
    {
        public PositiveAttribute()
        {
        }

        public override bool IsValid(object value)
        {
            switch (value)
            {
                case int i:
                    return i > 0;

                case uint ui:
                    return ui > 0;

                case double d:
                    return d > 0d;

                case float f:
                    return f > 0f;

                case decimal d:
                    return d > 0;

                case short s:
                    return s > 0;

                case ushort us:
                    return us > 0;

                case long l:
                    return l > 0;

                case ulong ul:
                    return ul > 0;

                case byte b:
                    return b > 0;

                default:
                    return false;
            }
        }
    }
}