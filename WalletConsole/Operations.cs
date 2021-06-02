using System;
using System.Collections.Generic;
using WalletLibrary.account;
using WalletLibrary.Wallet;


namespace WalletConsole
{
    class Operations
    {
        protected static List<string> costs = new List<string>();
        protected static List<string> income = new List<string>();
        internal static void OpenAccount(Wallet<Account> wallet)
        {
            Console.WriteLine("Enter account type:\n1. 'UAN'\n2. 'EURO'\n3. 'USD'");
            AccountType acType;
            string type = Convert.ToString(Console.ReadLine());
            switch (type)
            {
                case "1":
                    acType = AccountType.UaAccount;
                    break;
                case "2":
                    acType = AccountType.EuroAccount;
                    break;
                case "3":
                    acType = AccountType.UsdAccount;
                    break;
                default:
                    throw new ArgumentException("Invalid account type. Please check your input.");
            }
            Console.WriteLine("Enter the initial amount of money in the account:");
            decimal sum = Convert.ToDecimal(Console.ReadLine());
            wallet.OpenAccount(acType, sum, AccountHandler.OpenHandler, 
                AccountHandler.CloseHandler, AccountHandler.PutHandler,
                AccountHandler.WithdrawHandler);
        }
        internal static void AccountIdsList(Wallet<Account> wallet)
        {
            List<int> ids = wallet.GetListAccountsId;
            if (ids.Count == 0)
            {
                Console.WriteLine("Tere is no accounts");
                return;
            }
            Console.WriteLine("My accounts id:");
            for (int i = 0; i < ids.Count; i++)
            {
                Console.WriteLine(ids[i]);
            }
        }
        private static void CostsPut(List<string> list)
        {
            Console.WriteLine("Enter item:");
            string line = Console.ReadLine();
            if (line.Length >= 40 || line.Length == 0)
            {
                throw new Exception("Comment must be longer then zero but not bigger than 40");
            }
            list.Add(line);

        }
        internal static void ItemsEnter()
        {
            Console.WriteLine("Enter costs item:");
            string line;
            while ((line = Console.ReadLine()) != "")
            {
                if (line.Length >= 40 || line.Length == 0)
                {
                    throw new Exception("Comment must be longer then zero but not bigger than 40");
                }
                costs.Add(line);
            }
            Console.WriteLine("Enter income items:");
            while ((line = Console.ReadLine()) != "")
            {
                if (line.Length >= 40 || line.Length == 0)
                {
                    throw new Exception("Comment must be longer then zero but not bigger than 40");
                }
                income.Add(line);
            }

            Console.WriteLine("Input of items finished");
        }

        private static string EnterItems(List<string> list)
        {
            string name = null;
            if (list.Count == 0)
            {
                Console.WriteLine("At first you need to enter an item:");
                CostsPut(list);
                name = list[list.Count - 1];
            }
            else
            {
                Console.WriteLine("Do you want to use previous  items or enter new?");
                Console.WriteLine("1- previous \t2- new");
                string line = Console.ReadLine();
                if (line == "1")
                {
                    Console.WriteLine("Choose item :");
                    for (int i = 1; i <= list.Count; i++)
                    {
                        Console.WriteLine(Convert.ToString(i) + "  " + list[i - 1]);
                    }

                    int temp = Convert.ToInt32(Console.ReadLine());
                    for (int i = 1; i <= list.Count; i++)
                    {
                        if (temp == i)
                            name = list[i - 1];
                    }
                    if (name == null)
                        throw new ArgumentException("Invalid account type specified. Please check your input.");
                }
                else if (line == "2")
                {
                    CostsPut(list);
                    name = list[list.Count - 1];
                }
                else
                    throw new Exception("Input is not corect, check it");
            }
            if (name.Length == 0)
            {
                Console.WriteLine("Incorrectly entered comment. Repeat the procedure again.");
                throw new ArgumentException("Keyword must be > 0 ");
            }
            return name;
        }
        internal static void Put(Wallet<Account> wallet)
        {
            Console.WriteLine("Select account id:");
            AccountIdsList(wallet);
            int id = Convert.ToInt32(Console.ReadLine());
            string name =EnterItems(costs);
            Console.WriteLine("Enter amount of money to put on the account:");
            decimal sum = Convert.ToDecimal(Console.ReadLine());
            wallet.Put(id, sum, new InfoList(sum, name));
        }
        internal static void Withdraw(Wallet<Account> wallet)
        {
            Console.WriteLine("Select account id:");
            AccountIdsList(wallet);
            int id = Convert.ToInt32(Console.ReadLine());
            string name = EnterItems(income);
            Console.WriteLine("Enter amount of money to withdraw from the account:");
            decimal sum = Convert.ToDecimal(Console.ReadLine());
            wallet.Withdraw(id, sum, new InfoList(sum, name));
        }
        internal static void Transfer(Wallet<Account> wallet)
        {
            Console.WriteLine("Select account id:");
            AccountIdsList(wallet);
            Console.WriteLine("Choose the account from what you want to make a transfer: ");
            int id1 = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("Choose the account on what you want to make a transfer ");
            int id2 = Convert.ToInt32(Console.ReadLine());
            string name1 = "transfer from card";
            string name2 = "income on card";
            Console.WriteLine("Anter amount of money to trunsfer: ");
            decimal sum = Convert.ToDecimal(Console.ReadLine());
            wallet.Transfer(id1, id2, sum, new InfoList(sum, name1), new InfoList(sum, name2));
        }

        internal static void TypeOfAccount(Wallet<Account> wallet)
        {
            Console.WriteLine("Select account id:");
            AccountIdsList(wallet);
            int id = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine(wallet.ReturnType(id));
        }
        internal static void GetAccountInfo(Wallet<Account> wallet)
        {
            Console.WriteLine("Select account id:");
            AccountIdsList(wallet);
            int id = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("Operations with account:");
            Console.WriteLine("Replenishment:");
            List<string> list = wallet.GetPutInfo(id);
            for(int i=0; i<list.Count; i++)
            {
                Console.WriteLine(list[i]);
            }
            if (list.Count == 0)
                Console.WriteLine(0);
            list = wallet.GetWithdrawInfo(id);
            Console.WriteLine("Withdrawal: ");
            for (int i = 0; i < list.Count; i++)
            {
                Console.WriteLine(list[i]);
            }
            if (list.Count == 0)
                Console.WriteLine(0);
        }
        internal static void HistoryInfo(Wallet<Account> wallet)
        {
            Console.WriteLine("Operations of replenishment: ");
            foreach (KeyValuePair<InfoList, int> keyValue in wallet.GetHistoryPutList)
            {
                Console.WriteLine(keyValue.Value + " - " + (keyValue.Key).ConvertInfoList());
            }
            if (wallet.GetHistoryPutList.Count == 0)
                Console.WriteLine(0);
            Console.WriteLine("Operations of withdrawal ");
            foreach (KeyValuePair<InfoList, int> keyValue in wallet.GetHistoryWithdrawList)
            {
                Console.WriteLine(keyValue.Value + " - " + (keyValue.Key).ConvertInfoList());
            }
            if (wallet.GetHistoryWithdrawList.Count == 0)
                Console.WriteLine(0);
        }
        internal static void CloseAccount(Wallet<Account> wallet)
        {
            Console.WriteLine("Select account id:");
            AccountIdsList(wallet);
            int id = Convert.ToInt32(Console.ReadLine());
            wallet.CloseAccount(id);
        }
        internal static void CurrentSum(Wallet<Account> wallet)
        {
            Console.WriteLine("Select account id:");
            AccountIdsList(wallet);
            int id = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine(wallet.ReturnSum(id));
        }
        internal static void TotalInfo(Wallet<Account> wallet)
        {
            Console.WriteLine("Select account id:");
            AccountIdsList(wallet);
            int id = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("Replenishment: ");
            decimal sum = wallet.TotalPutSum(id);
            Console.WriteLine(sum);
            Console.WriteLine("Withdrawal: ");
            sum = wallet.TotalWithdrawSum(id);
            Console.WriteLine(sum);
        }
        internal static void ExpenceItem(Wallet<Account> wallet)
        {
            Console.WriteLine("Select account id:");
            AccountIdsList(wallet);
            int id = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("Replenishment:");
            List<string> list = wallet.ItemExpense(id, income);
            for (int i = 0; i < list.Count; i++)
            {
                Console.WriteLine(list[i]);
            }
            Console.WriteLine("Withdrawal: ");
            list = wallet.ItemWithdraw(id, costs);
            for (int i = 0; i < list.Count; i++)
            {
                Console.WriteLine(list[i]);
            }
        }
    }
}
 