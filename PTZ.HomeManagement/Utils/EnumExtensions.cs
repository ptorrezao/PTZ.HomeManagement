using PTZ.HomeManagement.ExpirationReminder.Core.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace PTZ.HomeManagement.Utils
{
    public static class EnumExtensions
    {
        public static string GetDescription<T>(this T enumerationValue)
          where T : struct
        {
            var type = enumerationValue.GetType();
            if (!type.IsEnum)
            {
                throw new ArgumentException($"{nameof(enumerationValue)} must be of Enum type", nameof(enumerationValue));
            }
            var memberInfo = type.GetMember(enumerationValue.ToString());
            if (memberInfo.Length > 0)
            {
                var attrs = memberInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);

                if (attrs.Length > 0)
                {
                    return ((DescriptionAttribute)attrs[0]).Description;
                }
            }
            return enumerationValue.ToString();
        }

        public static string GetColor(this ReminderStateType enumerationValue)
        {
            var type = enumerationValue.GetType();
            if (!type.IsEnum)
            {
                throw new ArgumentException($"{nameof(enumerationValue)} must be of Enum type", nameof(enumerationValue));
            }

            var value = "";
            switch (enumerationValue)
            {
                case ReminderStateType.NonExpired:
                    value = "#4caf50";
                    break;
                case ReminderStateType.Expiring:
                    value = "#ffc107";
                    break;
                case ReminderStateType.Expired:
                    value = "#dd1e31";
                    break;
            }

            return value;
        }

        public static string GetIcon(this ReminderStateType enumerationValue)
        {
            var type = enumerationValue.GetType();
            if (!type.IsEnum)
            {
                throw new ArgumentException($"{nameof(enumerationValue)} must be of Enum type", nameof(enumerationValue));
            }

            var value = "";
            switch (enumerationValue)
            {
                case ReminderStateType.NonExpired:
                    value = "nc-align-center";
                    break;
                case ReminderStateType.Expiring:
                    value = "nc-notification-70";
                    break;
                case ReminderStateType.Expired:
                    value = "nc-time-alarm";
                    break;
            }

            return value;
        }
        
    }
}
