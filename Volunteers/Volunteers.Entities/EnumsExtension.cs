namespace Volunteers.Entities
{
    using System;
    using System.ComponentModel;
    using System.Reflection;

    /// <summary>
    /// Расширение для Enums
    /// </summary>
    public static class EnumsExtension
    {
        /// <summary>
        /// Метод для получения описания
        /// </summary>
        /// <param name="enumElement">Элемент перечисления</param>
        /// <returns></returns>
        public static string GetDescription(Enum enumElement)
        {
            Type type = enumElement.GetType();
            MemberInfo[] memInfo = type.GetMember(enumElement.ToString());
            if (memInfo.Length > 0)
            {
                object[] attrs = memInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);
                if (attrs.Length > 0)
                    return ((DescriptionAttribute)attrs[0]).Description;
            }

            return enumElement.ToString();
        }
    }
}