using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using PowerPOS;

namespace LocalDAL
{
    public class DocumentInfo
    {
        private string _DocumentType;
        public string DocumentType
        {
            get { return _DocumentType; }
            set { _DocumentType = value; }
        }

        private DateTime _DocumentDate;
        public DateTime DocumentDate
        {
            get { return _DocumentDate; }
            set { _DocumentDate = value; }
        }
        
        private string _DocumentNo;
        public string DocumentNo
        {
            get { return _DocumentNo; }
            set { _DocumentNo = value; }
        }
        
        private string _MembershipInfo;
        public string MembershipInfo
        {
            get { return _MembershipInfo; }
            set { _MembershipInfo = value; }
        }
        
        private string _MembershipAddress;
        public string MembershipAddress
        {
            get { return _MembershipAddress; }
            set { _MembershipAddress = value; }
        }
        
        private string _CompanyName;
        public string CompanyName
        {
            get { return _CompanyName; }
            set { _CompanyName = value; }
        }
        
        private string _CompanyAddress;
        public string CompanyAddress
        {
            get { return _CompanyAddress; }
            set { _CompanyAddress = value; }
        }
        
        private string _TermsCondition;
        public string TermsCondition
        {
            get { return _TermsCondition; }
            set { _TermsCondition = value; }
        }
        
        private string _LogoURL;
        public string LogoURL
        {
            get { return _LogoURL; }
            set { _LogoURL = value; }
        }

        private string _ItemCode;
        public string ItemCode
        {
            get { return _ItemCode; }
            set { _ItemCode = value; }
        }

        private string _Description;
        public string Description
        {
            get { return _Description; }
            set { _Description = value; }
        }

        private decimal _Quantity;
        public decimal Quantity
        {
            get { return _Quantity; }
            set { _Quantity = value; }
        }

        private decimal _DiscPercent;
        public decimal DiscPercent
        {
            get { return _DiscPercent; }
            set { _DiscPercent = value; }
        }

        private decimal _UnitPrice;
        public decimal UnitPrice
        {
            get { return _UnitPrice; }
            set { _UnitPrice = value; }
        }

        private decimal _LineTotal;
        public decimal LineTotal
        {
            get { return _LineTotal; }
            set { _LineTotal = value; }
        }

        private string _Remarks;
        public string Remarks
        {
            get { return _Remarks; }
            set { _Remarks = value; }
        }
    }
}