using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms.Design;
using System.ComponentModel.Design;
using System.ComponentModel;

namespace StarlitTwit
{
    public class TabPageExCollectionEditor : CollectionEditor
    {
        public TabPageExCollectionEditor(Type type)
            : base(type)
        {
        }
        
        //protected override object CreateInstance(Type itemType)
        //{
        //    return typeof(TabPageEx);
        //}

        //protected override Type[] CreateNewItemTypes()
        //{
        //    return new Type[] { typeof(TabPageEx) };
        //}

        protected override Type CreateCollectionItemType()
        {
            return typeof(TabPageEx);
        }

        protected override string GetDisplayText(object value)
        {
            if (value is TabPageEx) {
                var tabpg = value as TabPageEx;
                return tabpg.Text;
            }
            return base.GetDisplayText(value);
        }
    }
}
