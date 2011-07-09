using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms.Design;
using System.ComponentModel.Design;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Collections;
using System.Windows.Forms;

namespace StarlitTwit
{
    public class TabControlExDesigner : ParentControlDesigner
    {
        bool _tabControlSelected = false;

        public override void Initialize(IComponent component)
        {
            base.Initialize(component);

            TabControlEx control = component as TabControlEx;
            Debug.Assert(control != null, "componentはTabControlExでなければならない");

            ISelectionService svc = (ISelectionService)GetService(typeof(ISelectionService));
            if (svc != null) {
                svc.SelectionChanged += this.OnSelectionChanged;
            }

        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) {
                ISelectionService svc = (ISelectionService)GetService(typeof(ISelectionService));
                if (svc != null) {
                    svc.SelectionChanged -= this.OnSelectionChanged;
                }
            }
            base.Dispose(disposing);
        }

        private void OnGotFocus(object sender, EventArgs e)
        {
            
        } 

        //-------------------------------------------------------------------------------
        #region +[override]InitializeNewComponent
        //-------------------------------------------------------------------------------
        //
        public override void InitializeNewComponent(System.Collections.IDictionary defaultValues)
        {
            base.InitializeNewComponent(defaultValues);

            this.OnAdd(this, EventArgs.Empty);

            MemberDescriptor member = TypeDescriptor.GetProperties(Component)["Controls"];

            TabControlEx tc = (TabControlEx)Component;
            if (tc != null) {
                tc.SelectedIndex = 0;
            }
        }
        #endregion (+[override]InitializeNewComponent)

        //-------------------------------------------------------------------------------
        #region -OnAdd 追加時
        //-------------------------------------------------------------------------------
        //
        private void OnAdd(object sender, EventArgs e)
        {
            TabControlEx tc = (TabControlEx)Component;
            IDesignerHost host = (IDesignerHost)GetService(typeof(IDesignerHost));
            if (host == null) { return; }

            DesignerTransaction t = null;
            try {
                try {
                    t = host.CreateTransaction();
                }
                catch (CheckoutException ex) {
                    if (ex == CheckoutException.Canceled) { return; }
                    throw ex;
                }
                MemberDescriptor member = TypeDescriptor.GetProperties(tc)["Controls"];
                TabPageEx page = (TabPageEx)host.CreateComponent(typeof(TabPageEx));

                string pageText = null;
                PropertyDescriptor nameProp = TypeDescriptor.GetProperties(page)["Name"];
                if (nameProp != null && nameProp.PropertyType == typeof(string)) {
                    pageText = (string)nameProp.GetValue(page);
                }

                if (pageText != null) {
                    PropertyDescriptor textProperty = TypeDescriptor.GetProperties(page)["Text"];
                    Debug.Assert(textProperty != null, "TextというPropertyが存在しない");
                    if (textProperty != null) {
                        textProperty.SetValue(page, pageText);
                    }
                }

                PropertyDescriptor styleProp = TypeDescriptor.GetProperties(page)["UseVisualStyleBackColor"];
                if (styleProp != null && styleProp.PropertyType == typeof(bool) && !styleProp.IsReadOnly && styleProp.IsBrowsable) {
                    styleProp.SetValue(page, true);
                }

                tc.Controls.Add(page);
                tc.SelectedIndex = tc.TabCount - 1;
            }
            finally {
                if (t != null) { t.Commit(); }
            }
        }
        #endregion (OnAdd)

        private void OnSelectionChanged(object sender, EventArgs e)
        {
            ISelectionService svc = (ISelectionService)GetService(typeof(ISelectionService));
            _tabControlSelected = false;

            if (svc != null) {
                ICollection selComponents = svc.GetSelectedComponents();

                TabControlEx tabControl = (TabControlEx)Component;

                foreach (object comp in selComponents) {

                    if (comp == tabControl) { _tabControlSelected = true; }

                    TabPageEx page = GetTabPageOfComponent(comp);

                    if (page != null && page.Parent == tabControl) {
                        _tabControlSelected = false; //this is for HitTest purposes
                        tabControl.SelectedTab = page;
                        tabControl.Refresh();
                        //SelectionManager selMgr = (SelectionManager)GetService(typeof(SelectionManager));
                        //selMgr.Refresh();
                        break;
                    }
                }
            } 
        }

        private TabPageEx GetTabPageOfComponent(object comp)
        {
            if (!(comp is Control)) {
                return null;
            }

            Control c = (Control)comp;
            while (c != null && !(c is TabPageEx)) {
                c = c.Parent;
            }
            return (TabPageEx)c; 
        }

        protected override bool GetHitTest(Point point)
        {
            TabControlEx tc = Control as TabControlEx;
            if (_tabControlSelected) {
                Point hitTest = Control.PointToClient(point);
                return !tc.DisplayRectangle.Contains(hitTest);
            }
            return false;
        }
    }
}
