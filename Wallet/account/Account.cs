using System;


namespace WalletLibrary.account
{
    public abstract class Account : IAccount
    {
        protected internal virtual event AccountStateHandler OpenEvent;
        protected internal virtual event AccountStateHandler CloseEvent;
        protected internal virtual event AccountStateHandler PutEvent;
        protected internal virtual event AccountStateHandler WithdrawEvent;
        protected int _id; 
        static int counter = 0; 
        protected decimal _sum;
        public Account(decimal sum)
        {
            if (sum >= 0)
            {
                _sum = sum;
                _id = ++counter;
            }
            else
                throw new ArgumentException("Sum can't be negative");
        }
        public decimal CurrentSum
        {
            get{return _sum;}
        }

        public int Id
        {
            get { return _id; }
        }

          protected internal abstract void OnOpened();
        public virtual string AccountType
        { get;}
        public void Put(decimal sum)
        {
            _sum += sum;
            PutEvent?.Invoke(this, new AccountEvents("Account received  " + sum, sum));
        }
      
        
        public virtual decimal Withdraw(decimal sum)
        {
            decimal cash = 0;
            if (sum <= _sum)
            {
                _sum -= sum;
                cash = sum;
                WithdrawEvent?.Invoke(this, new AccountEvents("A sum " + sum + " was withdrawn " + _id, sum));
            }
            else
                WithdrawEvent?.Invoke(this, new AccountEvents("There is no enought money in account" + _id, sum));
            return cash;
        }

        protected internal virtual void Close()
        {
            CloseEvent?.Invoke(this, new AccountEvents("Account " + _id + " was closed. Final sum: " + CurrentSum, CurrentSum));
        }
    }
}
