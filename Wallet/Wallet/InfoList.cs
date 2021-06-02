using System;

namespace WalletLibrary.Wallet
{
    public class InfoList
    {
        public string Name { get; }
        public decimal Sum { get; }

        public InfoList(decimal sum, string name)
        {
            if (name.Length == 0)
                throw new Exception("Withdraw or put category is no entered");
            if (sum <= 0)
                throw new Exception("Summ must be bigger than zero");
            Name = name;
            Sum = sum;
        }

        public string ConvertInfoList()
        {
            string s1 = Convert.ToString(Name);
            string s2 = Convert.ToString(Sum);
            string temp = s1 + " - " + s2;
            return temp;
        }
    }
}
