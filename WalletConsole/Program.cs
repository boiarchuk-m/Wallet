using System;
using WalletLibrary.account;
using WalletLibrary.Wallet;

namespace WalletConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            Wallet<Account> wallet = new Wallet<Account>();

            string description = "Commands:\n '1' - open a new account.\n '2' - put on the account.\n" +
                        " '3' - withdraw from the account.\n '4' - transfer to another account.\n " +
                                 "'5' - list of my accounts.\n '6' - account information about transactions.\n " +
                                 "'7' - transaction history of all accounts.\n '8' - Total put and withdraw sum of account.\n"+
                                 " '9' - check a sum of money for some items.\n '10' - close account.\n " +
                                 "'11' - current sum in account.\n '12' - enter expense and income item. \n"+
                                 " '13' - account type.\n"+
                                 " '14' - list of commands.\n '15' - exit.";
            Console.WriteLine(description);
            while (true)
            {
                try
                {
                    string command = Convert.ToString(Console.ReadLine());
                    switch (command)
                    {
                        case "1":
                            Operations.OpenAccount(wallet);
                            break;
                        case "2":
                            Operations.Put(wallet);
                            break;
                        case "3":
                            Operations.Withdraw(wallet);
                            break;
                        case "4":
                            Operations.Transfer(wallet);
                            break;
                        case "5":
                            Operations.AccountIdsList(wallet);
                            break;
                        case "6":
                            Operations.GetAccountInfo(wallet);
                            break;
                        case "7":
                            Operations.HistoryInfo(wallet);
                            break;
                        case "8":
                            Operations.TotalInfo(wallet);
                            break;
                        case "13":
                            Operations.TypeOfAccount(wallet);
                            break;
                        case "14":
                            Console.WriteLine(description);
                            break;
                        case "12":
                            Operations.ItemsEnter();
                            break;
                        case "9":
                            Operations.ExpenceItem(wallet);
                            break;
                        case "10":
                            Operations.CloseAccount(wallet);
                            break;
                        case "11":
                            Operations.CurrentSum(wallet);
                            break;
                        case "15":
                            Environment.Exit(0);
                            break;
                        default:
                            Console.WriteLine("Unrecognized command.");
                            break;
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    Console.WriteLine("The procedure was aborted. Check that the input values are correct.");
                }
            }
        }
    }
}
