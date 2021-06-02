

namespace WalletLibrary.account
{
    public delegate void AccountStateHandler(object sender, AccountEvents e);
    public class AccountEvents
    {
        public string Message
        { get; private set; }
        public decimal Sum 
        { get; private set; }
        public AccountEvents(string _mes, decimal _sum)
        {
            Message = _mes;
            Sum = _sum;
        }
    }
}
