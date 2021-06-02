using System;
using System.Collections.Generic;

using WalletLibrary.account;

namespace WalletLibrary.Wallet
{
    public class Wallet<T> : IWallet<T> where T : Account
    { 
        private List<T> _wallet = new List<T>();
        private Dictionary<InfoList, int> HistoryPut = new Dictionary<InfoList, int>();
        private Dictionary<InfoList, int> HistoryWithdraw = new Dictionary<InfoList, int>();
        public Dictionary<InfoList, int> GetHistoryPutList
        {
            get { return HistoryPut; }
        }
        public Dictionary<InfoList, int> GetHistoryWithdrawList
        {
            get { return HistoryWithdraw; }
        }

        public void OpenAccount(AccountType type, decimal sum,
             AccountStateHandler openHandler, AccountStateHandler closeHandler,
           AccountStateHandler putHandler,
           AccountStateHandler withdrawHandler)
          
        {
            T newAccount = null;
            switch (type)
            {
                case AccountType.UaAccount:
                    newAccount = new UaAccount(sum) as T;
                    break;
                case AccountType.EuroAccount:
                    newAccount = new EuroAccount(sum) as T;
                    break;
                case AccountType.UsdAccount:
                    newAccount = new UsdAccount(sum) as T;
                    break;
            }
            if (newAccount == null)
            {
                throw new NullReferenceException("Unreal to create an account. Account is null object");
            }
            _wallet.Add(newAccount);
            newAccount.OpenEvent += openHandler;
            newAccount.CloseEvent += closeHandler;
            newAccount.PutEvent += putHandler;
            newAccount.WithdrawEvent += withdrawHandler;
           
          
            newAccount.OnOpened();
            int id = newAccount.Id;
            HistoryPut.Add(new InfoList(sum, "account opening"), id);
        }
        public void CloseAccount(int id)
        {
            T account = FindAccount(id);
            if (account == null)
            {
                throw new NullReferenceException("Unreal find account with id " + id);
            }
            account.Close();
            _wallet.Remove(account);
        }

        public T FindAccount(int id)
        {
            foreach (var instance in _wallet)
            {
                if (instance.Id == id)
                {
                    return instance;
                }
            }
            return null;
        }

        public List<int> GetListAccountsId
        {
            get
            {
                List<int> accountsIds = new List<int>();
                foreach (var account in _wallet)
                {
                    accountsIds.Add(account.Id);
                }
                return accountsIds;
            }
        }

        public int ReturnID(int id)
        {
            T account = FindAccount(id);
            return account.Id;
        }
        public void Transfer(int id1, int id2, decimal sum, InfoList list1, InfoList list2)
        {
            T account1 = FindAccount(id1);
            T account2 = FindAccount(id2);
            if (account1 == null)
                throw new Exception("Account with id" + id1 + " didn't find");
            if (account2 == null)
                throw new Exception("Account with id " + id2 + " didn't find");
            if (account1.CurrentSum < sum)
            {
                throw new Exception("There is no enought money for transfer");
            }

            account1.Withdraw(sum);
            HistoryWithdraw.Add(list1, id1);
            if (account1.AccountType != account2.AccountType)
            {
                if (account1.AccountType == "EUR")
                {
                    if (account2.AccountType == "UAN") sum *= Convert.ToDecimal(33.5);
                    if (account2.AccountType == "USD") sum *= Convert.ToDecimal(1.22);
                }
                if (account1.AccountType == "UAN")
                {
                    if (account2.AccountType == "EUR") sum /= Convert.ToDecimal(33.5);
                    if (account2.AccountType == "USD") sum /= Convert.ToDecimal(27.5);
                }
                if (account1.AccountType == "USD")
                {
                    if (account2.AccountType == "EUR") sum /= Convert.ToDecimal(0.82);
                    if (account2.AccountType == "UAN") sum *= Convert.ToDecimal(27.5);
                }

            }
            account2.Put(sum);
            HistoryPut.Add(new InfoList(sum, list2.Name), id2);
        }

        public void Put(int id, decimal sum, InfoList list)
        {
            if (list == null)
                throw new Exception("Category of costs didn't find ");
            T account = FindAccount(id);
            if (account == null)
                throw new Exception("Account with id " + id + " didnt find");
            account.Put(sum);
            HistoryPut.Add(list, id);
        }

        public void Withdraw(int id, decimal sum, InfoList list)
        {
            T account = FindAccount(id);
            if (account == null)
                throw new Exception("Account with id " + id + " didnt find");
            if (account.CurrentSum < sum)
                throw new Exception("Sum on the account is smoller then was set");
            account.Withdraw(sum);
            HistoryWithdraw.Add(list, id);
        }


        public string ReturnType(int id)
        {
            T account = FindAccount(id);
            return account.AccountType;
        }
        public decimal ReturnSum(int id)
        {
            T account = FindAccount(id);
            return account.CurrentSum;
        }
        public List<string> GetPutInfo(int id)
        {
            List<string> list = new List<string>();
            foreach (KeyValuePair<InfoList, int> keyValue in HistoryPut)
            {
                if (keyValue.Value == id)
                    list.Add((keyValue.Key).ConvertInfoList());
            }

            return list;

        }

        public List<string> GetWithdrawInfo(int id)
        {
            List<string> list = new List<string>();
            foreach (KeyValuePair<InfoList, int> keyValue in HistoryWithdraw)
            {
                if (keyValue.Value == id)
                    list.Add((keyValue.Key).ConvertInfoList());
            }

            return list;
        }


        public decimal TotalPutSum(int id)
        {
            decimal sum = 0;
            foreach (KeyValuePair<InfoList, int> keyValue in HistoryPut)
            {
                if (keyValue.Value == id)
                {
                    sum += Convert.ToDecimal(keyValue.Key.Sum);
                }
            }
            return sum;
        }

        public decimal TotalWithdrawSum(int id)
        {
            decimal sum = 0;
            foreach (KeyValuePair<InfoList, int> keyValue in HistoryWithdraw)
            {
                if (keyValue.Value == id)
                {
                    sum += Convert.ToDecimal(keyValue.Key.Sum);
                }
            }
            return sum;
        }

        public List<string> ItemExpense(int id, List<string> income)
        {
            List<string> list = new List<string>();
            decimal sum1;
            foreach (KeyValuePair<InfoList, int> keyValue in HistoryPut)
            {
                if (keyValue.Value == id)
                {
                    if (keyValue.Key.Name == "account opening")
                    {
                        sum1 = keyValue.Key.Sum;
                        list.Add(Convert.ToString("account opening" + " - " + sum1));
                    }
                    if (keyValue.Key.Name == "transfer on card")
                    {
                        sum1 = keyValue.Key.Sum;
                        list.Add(Convert.ToString("transfer on card" + " - " + sum1));
                    }
                }
            }
            for (int i = 0; i < income.Count; i++)
            {
                decimal sum = 0;
                foreach (KeyValuePair<InfoList, int> keyValue in HistoryPut)
                {
                    if (keyValue.Value == id)
                    {
                        if (keyValue.Key.Name == income[i])
                            sum += keyValue.Key.Sum;
                    }
                }
                list.Add(Convert.ToString(income[i] + " - " + sum));
            }
            return list;
        }

        public List<string> ItemWithdraw(int id, List<string> costs)
        {
            List<string> list = new List<string>();
            decimal sum1;
            foreach (KeyValuePair<InfoList, int> keyValue in HistoryWithdraw)
            {
                if (keyValue.Value == id)
                {
                   
                    if (keyValue.Key.Name == "transfer from card")
                    {
                        sum1 = keyValue.Key.Sum;
                        list.Add(Convert.ToString("transfer from card" + " - " + sum1));
                    }
                }
            }
            for (int i = 0; i < costs.Count; i++)
            {
                decimal sum = 0;
                foreach (KeyValuePair<InfoList, int> keyValue in HistoryWithdraw)
                {
                    if (keyValue.Value == id)
                    {
                        if (keyValue.Key.Name == costs[i])
                            sum += keyValue.Key.Sum; 
                    }
                }
                list.Add(Convert.ToString(costs[i] + " - " + sum));
            }
            return list;
        }
    }
}



