﻿using System;
using System.Collections.Generic;
using System.Web.Mvc;
using DaringCore.Extensions;

namespace DaringCore.Web
{
    public static class SelectListHelpers
    {
        /// <summary>
        /// Generate a select list for Razor
        /// Ussage: @Html.DropDownListFor(model => model.SomeSelectedString, SelectListHelpers.SelectListFromEnum(typeof(MyEnum), model.SomeSelectedString)
        /// </summary>
        /// <param name="type">enum Type</param>
        /// <param name="selected">Current selected value</param>
        /// <returns><![CDATA[IEnumerable<SelectListItem>]]></returns>
        public static IEnumerable<SelectListItem> SelectListFromEnum(Type type, object selected)
        {
            if (!type.IsEnum)
                throw new ArgumentException("Type is not an enum.");

            if (selected != null && selected.GetType() != type)
                throw new ArgumentException("Selected object is not " + type);

            var enums = new List<SelectListItem>();
            foreach (int value in Enum.GetValues(type))
            {
                var item = new SelectListItem { Value = Enum.GetName(type, value), Text = Enum.GetName(type, value).SplitCamelCase() };

                if (selected != null)
                    item.Selected = (int)selected == value;

                enums.Add(item);
            }

            return enums;
        }
    }
}
