using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WalletLibrary.account
{
    class UsdAccount : Account
    {
        protected internal override event AccountStateHandler OpenEvent;
        
        public override string AccountType
        {
            get{ return "USD";
            }
        }
        public UsdAccount(decimal sum) : base(sum)
        { }
        protected internal override void OnOpened()
        {
            OpenEvent?.Invoke(this, new AccountEvents("A new dollar account opened. Account Id:" + Id, _sum));
        }
    }
}
