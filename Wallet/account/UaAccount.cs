using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WalletLibrary.account
{
    class UaAccount : Account
    {
        protected internal override event AccountStateHandler OpenEvent;
        public UaAccount(decimal sum) : base(sum)
        { }
        protected internal override void OnOpened()
        {
            OpenEvent?.Invoke(this, new AccountEvents("A new hrivnia account opened. Account Id: " + Id, _sum));
        }
        public override string AccountType
        { 
            get{return "UAN"; }
        }
    }
}
