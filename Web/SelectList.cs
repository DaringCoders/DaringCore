using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Web
{
    public static class SelectExtensions
    {
        public static HtmlString DropDownList(this HtmlHelper helper, string name, Type type, object selected)
        {
            if (!type.IsEnum)
                throw new ArgumentException("Type is not an enum.");

            if (selected != null && selected.GetType() != type)
                throw new ArgumentException("Selected object is not " + type);

            var enums = new List<SelectListItem>();
            foreach (int value in Enum.GetValues(type))
            {
                var item = new SelectListItem { Value = Enum.GetName(type, value), Text = Enum.GetName(type, value) };

                if (selected != null)
                    item.Selected = (int)selected == value;

                enums.Add(item);
            }

            return System.Web.Mvc.Html.SelectExtensions.DropDownList(helper, name, enums, "--Select--");
        }
    }
}
