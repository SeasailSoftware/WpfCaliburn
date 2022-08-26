using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using Seasail.i18N;

namespace Seasail.Extensions
{
    public static partial class TNHExtension
    {
        /// <summary>
        /// 把枚举转换成为列表
        /// </summary>
        public static List<EnumObject> ToList(this Type type)
        {
            List<EnumObject> list = new List<EnumObject>();
            foreach (object obj in Enum.GetValues(type))
            {
                list.Add(new EnumObject((Enum)obj));
            }
            return list;
        }

        public static string Description(this Enum eValue)
        {
            Type type = eValue.GetType();

            var flagsAttributs = type.GetCustomAttributes(typeof(FlagsAttribute), false);
            if (Convert.ToInt32(eValue) == 0 || !flagsAttributs.Any())
            {
                var nAttributes = type.GetField(eValue.ToString())
                .GetCustomAttributes(typeof(DescriptionAttribute), false);

                if (nAttributes.Any())
                    return (nAttributes.First() as DescriptionAttribute).Description;
            }
            else
            {
                List<string> list = new List<string>();
                foreach (int flag in Enum.GetValues(type))
                {
                    if (flag == 0)
                        continue;

                    Enum enumFlag = (Enum)Enum.ToObject(type, flag);
                    if (eValue.HasFlag(enumFlag))
                    {
                        var nAttributes = type.GetField(enumFlag.ToString())
                            .GetCustomAttributes(typeof(DescriptionAttribute), false);

                        if (nAttributes.Any())
                            list.Add((nAttributes.First() as DescriptionAttribute).Description);
                        else
                            list.Add(enumFlag.ToString());
                    }
                }

                if (list.Count > 0)
                {
                    return string.Join(", ", list);
                }
            }

            return eValue.ToString();
        }
    }

    /// <summary>
    /// 枚举、类型的值
    /// </summary>
    public class EnumObject
    {
        public EnumObject(Enum um, string picture = null)
        {
            this.ID = (int)Convert.ChangeType(um, typeof(int));
            this.Name = um.ToString();
            this.Description = um.ToString();
            this.Picture = picture;
        }

        public EnumObject(int id, string name)
        {
            this.ID = id;
            this.Name = this.Description = name;
            this.Picture = null;
        }

        public EnumObject(int id, string name, string description, string picture)
        {
            this.ID = id;
            this.Name = name;
            this.Description = description;
            this.Picture = picture;
        }

        public int ID { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string Picture { get; set; }
    }
}
