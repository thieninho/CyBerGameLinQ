using CyberGameManage.DTO;
using System.Collections.Generic;
using System.Data;
using System.Linq;
namespace CyberGameManage.DAO
{
    class AccountDAO
    {
        private static AccountDAO instance;
        public static AccountDAO Instance
        {
            get { if (instance == null) instance = new AccountDAO(); return instance; }
            private set { instance = value; }
        }
        private AccountDAO() { }
        public bool Login(string userName, string passWord)
        {
            string query = "USP_Login @userName , @passWord";

            DataTable result = DataProvider.Instance.ExecuteQuery(query, new object[] { userName, passWord });

            return result.Rows.Count > 0;
        }
        public bool UpdateAccount(string userName, string displayName, string pass, string newPass)
        {
            int result = DataProvider.Instance.ExecuteNonQuery("exec USP_UpdateAccount @userName , @displayName , @password , @newPassword", new object[] { userName, displayName, pass, newPass });

            return result >0;
        }

        public List<AccountDTO> GetListAccount()
        {
            var listAcc = (from acc in DataProvider.Instance.db().Accounts
                           select new AccountDTO(acc.UserName, acc.DisplayName, acc.Type, acc.PassWord)).ToList();
            return listAcc;
        }

        public AccountDTO GetAccountByUserName(string userName)
        {
            var account = (from acc in DataProvider.Instance.db().Accounts
                           where acc.UserName == userName
                           select new AccountDTO(acc.UserName, acc.DisplayName, acc.Type, acc.PassWord)).FirstOrDefault();
            if (account != null)
            {
                return account;
            }
            return null;
        }
        public bool InsertAccount(string name, string displayName, int type)
        {
            var account = new Account
            {
                UserName = name,
                DisplayName = displayName,
                PassWord = "123",
                Type = type,
            };
            DataProvider.Instance.db().Accounts.InsertOnSubmit(account);
            DataProvider.Instance.db().SubmitChanges();
            return true;
        }

        public bool UpdateAccount(string name, string displayName, int type)
        {
            var account = DataProvider.Instance.db().Accounts.Single(acc => acc.UserName == name);
            account.DisplayName = displayName;
            account.Type = type;
            DataProvider.Instance.db().SubmitChanges();
            return true;
        }

        public bool DeleteAccount(string name)
        {
            var account = DataProvider.Instance.db().Accounts.Single(acc => acc.UserName == name);
            DataProvider.Instance.db().Accounts.DeleteOnSubmit(account);
            DataProvider.Instance.db().SubmitChanges();
            return true;
        }

        public bool ResetPassword(string name)
        {

            var account = DataProvider.Instance.db().Accounts.Single(acc => acc.UserName == name);
            account.PassWord = "0";
            DataProvider.Instance.db().SubmitChanges();
            return true;
        }
    }
}
