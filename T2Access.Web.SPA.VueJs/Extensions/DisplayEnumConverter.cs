using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;

namespace T2Access.Web.SPA.VueJs
{
    public class DisplayEnumConverter : StringEnumConverter
    {


        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var array = new JArray();
            using (var tempWriter = array.CreateWriter())
            {
                base.WriteJson(tempWriter, value, serializer);
            }

            var token = array.Single();

            if (token.Type == JTokenType.String && value != null)
            {

                var enumType = value.GetType();
                string enumValue = Enum.GetName(enumType, value);
                MemberInfo member = enumType.GetMember(enumValue)[0];
                object[] attrs = member.GetCustomAttributes(typeof(DisplayAttribute), false);
                string name = ((DisplayAttribute)attrs[0]).Name;
                string group = ((DisplayAttribute)attrs[0]).GroupName;

                if (((DisplayAttribute)attrs[0]).ResourceType != null)
                {
                    name = ((DisplayAttribute)attrs[0]).GetName();
                    group = ((DisplayAttribute)attrs[0]).GetDescription();
                }

                token = group + "_" + name;
                // token = (JValue)string.Join(", ", token.ToString().Split(',').Select(s => s.Trim()).Select(s => (!char.IsNumber(s[0]) && s[0] != '-') ? name + s : s).ToArray());
            }

            token.WriteTo(writer);
        }

    }

}