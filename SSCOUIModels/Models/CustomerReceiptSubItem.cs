using System;
using FPsxWPF.Controls;
using System.Globalization;

namespace SSCOUIModels.Models
{
    public class CustomerReceiptSubItem : ReceiptSubItem
    {
        public CustomerReceiptSubItem()
        {
        }

        public CustomerReceiptSubItem(ReceiptSubItem item)
        {
            ItemToCustomerReceiptSubItem(item);
        }

        /// <summary>
        /// Creates a new object that is a copy of the current instance.
        /// </summary> 
        public override Object Clone()
        {
            CustomerReceiptSubItem newItem = new CustomerReceiptSubItem();
            newItem.ItemToCustomerReceiptSubItem(base.Clone() as ReceiptSubItem);
            return newItem;
        }

        public string[] ExtraLines { get; set; }

        public string Price { get; set; }

        public string PriceWhole { get; set; }

        public string PriceDecimal { get; set; }

        public int Quantity { get; set; }

        public bool Highlighted { get; set; }

        public bool Restricted { get; set; }

        public bool Reward { get; set; }

        public bool Voidable { get; set; }

        public bool Voided { get; set; }

        public string Description { get; set; }

        private void ItemToCustomerReceiptSubItem(ReceiptSubItem item)
        {
            TextColumns = item.TextColumns;
            TextColor = item.TextColor;
            TextWrap = item.TextWrap;
            BackgroundColor = item.BackgroundColor;
            Strikeout = item.Strikeout;
            Data = item.Data;
            Variables = item.Variables;

            if (item.TextColumns.Length > 0)
            {
                ExtraLines = new string[(int)Math.Ceiling(item.TextColumns.Length / 2.0) - 1];
                Description = item.TextColumns[0];

                for (int i = 1; i <= ExtraLines.Length; i++)
                {
                    ExtraLines[i - 1] = item.TextColumns[i * 2];
                }

                Price = item.TextColumns.Length > 1 ? item.TextColumns[1] : String.Empty;
            }

            if (item.Variables != null)
            {
                Quantity = item.Variables.ContainsKey("ITEM_SOLDQTY") ? Int32.Parse(item.Variables["ITEM_SOLDQTY"]) : 1;
                if (item.Variables.ContainsKey("SUB_ITEM_DESCRIPTION"))
                {
                    Highlighted = true;
                }
                if (item.Variables.ContainsKey("ITEM_RESTRICTED"))
                {
                    Restricted = true;
                }
                if (item.Variables.ContainsKey("ITEM_REWARD"))
                {
                    Reward = true;
                }
            }
            Voided = item.Strikeout;
        }
    }
}
