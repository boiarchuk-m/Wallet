using System;

using WalletLibrary.account;
using WalletLibrary.Wallet;


namespace WalletConsole
{
    public class AccountHandler
    {
        
        internal static void CloseHandler(object sender, AccountEvents e) => Console.WriteLine(e.Message);
        internal static void PutHandler(object sender, AccountEvents e) => Console.WriteLine(e.Message);
        internal static void WithdrawHandler(object sender, AccountEvents e) => Console.WriteLine(e.Message);
        internal static void OpenHandler(object sendler, AccountEvents e) => Console.WriteLine(e.Message);
    }
}
