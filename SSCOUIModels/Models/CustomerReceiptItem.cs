using System;
using FPsxWPF.Controls;
using System.Globalization;

namespace SSCOUIModels.Models
{
    public class CustomerReceiptItem : ReceiptItem
    {
        public CustomerReceiptItem()
        {          
        }
        
        public CustomerReceiptItem(ReceiptItem item)
        {
            ReceiptItemToCustomerReceiptItem(item);
        }

        public CustomerReceiptItem(string currentItem)
        {
            CurrentItemToCustomerReceiptItem(currentItem);
        }

        /// <summary>
        /// Creates a new object that is a copy of the current instance.
        /// </summary> 
        public override Object Clone()
        {
            CustomerReceiptItem newItem = new CustomerReceiptItem();
            newItem.ReceiptItemToCustomerReceiptItem(base.Clone() as ReceiptItem);
            return newItem;
        }

        public string[] ExtraLines { get; private set; }

        public string Price { get; private set; }

        public string PriceWhole { get; private set; }

        public string PriceDecimal { get; private set; }

        public int Quantity { get; private set; }

        public bool Highlighted { get; private set; }

        public bool Restricted { get; private set; }

        public bool Reward { get; private set; }

        public bool Voidable { get; private set; }

        public bool Voided { get; private set; }

        public bool Coupon { get; private set; }

        public string Description { get; private set; }

        public string ItemCode { get; private set; }

        public bool VisualVerify { get; private set; }

        public bool ApprovalFlag { get; private set; }

        public bool IsItemRapOnly
        {
            get;
            private set;
        }

        public bool IsItemMessage
        {
            get;
            private set;
        }

        public bool IsStoreModeText
        {
            get;
            private set;
        }

        public bool IsApproved
        {
            get
            {
                return this.ApprovalFlag && (this.VisualVerify || this.Restricted);
            }
        }

        public bool IsIntervention
        {
            get
            {
                return !this.ApprovalFlag && !this.Voided && (this.VisualVerify || this.Restricted);
            }
        }

        public bool IsPartialPayment
        {
            get
            {
                return this.Variables.ContainsKey("ITEM_EXT_PRICE") && this.Variables.ContainsKey("ITEM_MSG");
            }
        }

        private void CurrentItemToCustomerReceiptItem(string currentItem)
        {
            string[] properties = currentItem.Split(CurrentItemPropertySeparator);
            foreach (string property in properties)
            {
                string[] nameValue = property.Split(CurrentItemNameValueSeparator);
                if (2 == nameValue.Length)
                {
                    if (nameValue[0].Equals(CurrentItemDescriptionProperty))
                    {
                        Description = nameValue[1];
                    }
                    else if (nameValue[0].Equals(CurrentItemItemCodeProperty))
                    {
                        ItemCode = nameValue[1];
                    }
                    else if (nameValue[0].Equals(CurrentItemQuantityProperty))
                    {
                        Quantity = int.Parse(nameValue[1]);
                    }
                }
            }
        }
        
        private void ReceiptItemToCustomerReceiptItem(ReceiptItem item)
        {
            TextColumns = item.TextColumns;
            TextColor = item.TextColor;
            TextWrap = item.TextWrap;
            BackgroundColor = item.BackgroundColor;
            Strikeout = item.Strikeout;
            Selectable = item.Selectable;
            Data = item.Data;
            Variables = item.Variables;
            SubItems = item.SubItems;
            LineNumber = item.LineNumber;
            int size = (int)Math.Ceiling(item.TextColumns.Length / 2.0) - 1;
            ExtraLines = new string[size > 0 ? size : 0];
            Description = item.TextColumns.Length > 0 ? item.TextColumns[0] : String.Empty;
            for (int i = 1; i <= ExtraLines.Length; i++)
            {
                ExtraLines[i - 1] = item.TextColumns[i * 2];
            }
            Price = item.TextColumns.Length > 1 ? item.TextColumns[1] : String.Empty;                        
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
            if (item.Variables.ContainsKey("ITEM_COUPON"))
            {
                Coupon = true;
            }
            if (item.Variables.ContainsKey("ITEM_CODE"))
            {
                ItemCode = item.Variables["ITEM_CODE"].ToString();
            }
            if (item.Variables.ContainsKey("ITEM_VISUALVERIFY"))
            {
                VisualVerify = true;
            }
            if (item.Variables.ContainsKey("ITEM_AUTOAPPROVAL_FLAG"))
            {
                ApprovalFlag = (Int32.Parse(item.Variables["ITEM_AUTOAPPROVAL_FLAG"]) == 1) ? true : false;
            }
            if (item.Variables.ContainsKey("ITEM_RAP_ONLY"))
            {
                IsItemRapOnly = true;
            }
            if (item.Variables.ContainsKey("ITEM_MSG"))
            {
                IsItemMessage = true;
            }
            if (item.Variables.ContainsKey("ITEM_SMTEXT"))
            {
                IsStoreModeText = true;
            }
            Voidable = item.Selectable;
            Voided = item.Strikeout;
        }

        private const string CurrentItemDescriptionProperty = "item-description";
        private const string CurrentItemItemCodeProperty = "upc";
        private const char CurrentItemNameValueSeparator = '=';
        private const char CurrentItemPropertySeparator = ';';
        private const string CurrentItemQuantityProperty = "quantity";
    }
}
