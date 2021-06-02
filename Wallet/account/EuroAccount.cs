

namespace WalletLibrary.account
{
    class EuroAccount :Account
    {
        protected internal override event AccountStateHandler OpenEvent;
        public override string AccountType
        {
            get{return "EUR";}
        }
        public EuroAccount(decimal sum) : base(sum)
        { }
        protected internal override void OnOpened()
        {
            OpenEvent?.Invoke(this, new AccountEvents("A new Euro account opened. Account Id: " + Id, _sum));
        }
    }
}
