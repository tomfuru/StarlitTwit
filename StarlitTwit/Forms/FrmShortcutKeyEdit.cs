using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace StarlitTwit
{
    public partial class FrmShortcutKeyEdit : Form
    {
        //-------------------------------------------------------------------------------
        #region Constructor
        //-------------------------------------------------------------------------------
        //
        public FrmShortcutKeyEdit(ShortcutKeyData keydata)
        {
            InitializeComponent();

            keyInputGrid1.AddLabel("●メインフォームのショートカット");
            var dic1 = ConvertDictionary(keydata.MainFormShortcutDic);
            var list1 = new List<Tuple<string, KeyData, object>>();
            foreach (var val in (ShortcutType_MainForm[])Enum.GetValues(typeof(ShortcutType_MainForm))) {
                if (dic1.ContainsKey(val)) {
                    list1.Add(new Tuple<string, KeyData, object>(val.ToString(), dic1[val], val));
                }
                else {
                    list1.Add(new Tuple<string, KeyData, object>(val.ToString(), null, val));
                }
            }
            keyInputGrid1.AddItems(list1);

            keyInputGrid1.AddLabel("●発言選択時のショートカット");
            var dic2 = ConvertDictionary(keydata.StatusShortcutDic);
            var list2 = new List<Tuple<string, KeyData, object>>();
            foreach (var val in (ShortcutType_Status[])Enum.GetValues(typeof(ShortcutType_Status))) {
                if (dic2.ContainsKey(val)) {
                    list2.Add(new Tuple<string, KeyData, object>(val.ToString(), dic2[val], val));
                }
                else {
                    list2.Add(new Tuple<string, KeyData, object>(val.ToString(), null, val));
                }
            }
            keyInputGrid1.AddItems(list2);
        }
        #endregion (Constructor)

        //-------------------------------------------------------------------------------
        #region +GetShortcutKeyData ショートカットキーデータ取得
        //-------------------------------------------------------------------------------
        //
        public ShortcutKeyData GetShortcutKeyData()
        {
            ShortcutKeyData skd = new ShortcutKeyData();
            foreach (var item in keyInputGrid1.GetItems()) {
                if (item.Item2 is ShortcutType_MainForm) {
                    skd.MainFormShortcutDic.Add(item.Item1, (ShortcutType_MainForm)item.Item2);
                }
                else {
                    skd.StatusShortcutDic.Add(item.Item1, (ShortcutType_Status)item.Item2);
                }
            }
            return skd;
        }
        #endregion (GetShortcutKeyData)

        //-------------------------------------------------------------------------------
        #region btnOK_Click OKボタン
        //-------------------------------------------------------------------------------
        //
        private void btnOK_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }
        #endregion (btnOK_Click)
        //-------------------------------------------------------------------------------
        #region btnCansel_Click キャンセルボタン
        //-------------------------------------------------------------------------------
        //
        private void btnCansel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }
        #endregion (btnCansel_Click)

        //-------------------------------------------------------------------------------
        #region -ConvertDictionary 辞書を変換
        //-------------------------------------------------------------------------------
        //
        private SerializableDictionary<U, T> ConvertDictionary<T, U>(SerializableDictionary<T, U> dic)
        {
            var newDic = new SerializableDictionary<U, T>();
            foreach (var pair in dic) {
                if (newDic.ContainsKey(pair.Value)) { throw new ArgumentException("同じValueがあります｡"); }
                newDic.Add(pair.Value, pair.Key);
            }
            return newDic;
        }
        #endregion (-ConvertDictionary)
    }
}
