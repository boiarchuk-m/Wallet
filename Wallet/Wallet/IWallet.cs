
using WalletLibrary.account;

namespace WalletLibrary.Wallet
{
    public enum AccountType
    {
        UaAccount,
        EuroAccount,
        UsdAccount
    }
    public interface IWallet<T>
    {
        void OpenAccount(AccountType type, decimal sum,
            AccountStateHandler putHandler,
          AccountStateHandler withdrawHandler, AccountStateHandler closeHandler,
          AccountStateHandler openHandler);

        void CloseAccount(int id);
        int ReturnID(int id);
        void Put(int id, decimal sum, InfoList list);
        void Withdraw(int id, decimal sum, InfoList list);
        void Transfer(int id1, int id2, decimal sum, InfoList list1, InfoList list2);
    }
}