using System;

namespace W8
{
    // هذا الكلاس يمثّل حساب بنكي
    public class BankAccount
    {
        // 1) بيانات حساسة => private (Data Hiding)
        private string _accountNumber;
        private decimal _balance;

        // 2) خاصية للقراءة فقط (Read-Only) => account number لا يتغيّر بعد الإنشاء
        public string AccountNumber
        {
            get { return _accountNumber; }
        }

        // A Property is not about how data is stored, but about how an object is perceived.
    
        // 3) خاصية للرصيد (Balance) للقراءة فقط من الخارج
        //   التعديل يكون فقط عبر Deposit / Withdraw
        public decimal Balance
        {
            get { return _balance; }
            private set { _balance = value; } // private set => مرونة في الداخل فقط
        }

        // 4) Constructor يهيّئ القيم (إعطاء قيمة مبدئية للـ readonly / private fields)
        public BankAccount(string accountNumber, decimal initialBalance)
        {
            // ممكن نضيف فحص هنا أيضًا
            if (string.IsNullOrWhiteSpace(accountNumber))
                throw new ArgumentException("Account number cannot be empty.");

            if (initialBalance < 0)
                throw new ArgumentException("Initial balance cannot be negative.");

            _accountNumber = accountNumber;
            Balance = initialBalance;
        }

        // 5) Method للإيداع (واجهة آمنة للتعديل)
        public void Deposit(decimal amount)
        {
            if (amount <= 0)
            {
                Console.WriteLine("Deposit amount must be positive.");
                return;
            }

            Balance += amount;
            Console.WriteLine($"Deposited {amount}. New balance = {Balance}");
        }

        // 6) Method للسحب مع قواعد (Validation)
        public void Withdraw(decimal amount)
        {
            if (amount <= 0)
            {
                Console.WriteLine("Withdraw amount must be positive.");
                return;
            }

            if (amount > Balance)
            {
                Console.WriteLine("Insufficient funds!");
                return;
            }

            Balance -= amount;
            Console.WriteLine($"Withdrew {amount}. New balance = {Balance}");
        }

        // 7) Method لعرض ملخّص الحساب (واجهة خارجية – Abstraction بسيطة)
        public void PrintSummary()
        {
            Console.WriteLine("==== Account Summary ====");
            Console.WriteLine($"Account: {AccountNumber}");
            Console.WriteLine($"Balance: {Balance}");
            Console.WriteLine("=========================");
        }
    }

    // هذا الكلاس يمثّل "العميل" الذي يستخدم BankAccount
    class Program
    {
        static void Main(string[] args)
        {
            // إنشاء حساب جديد (Object من نوع BankAccount)
            BankAccount acc = new BankAccount("JO-12345", 500m);

            acc.PrintSummary();

            // تجربة إيداعات وسحوبات صحيحة وخاطئة
            acc.Deposit(200m);    // OK
            acc.Deposit(-50m);   // خطأ – لن يضاف

            acc.Withdraw(100m);  // OK
            acc.Withdraw(1000m); // خطأ – رصيد غير كاف

            // لاحظ: لا يمكننا فعل هذا من الخارج:
            // acc._balance = 0;       // خطأ (private)
            // acc.Balance = 0;        // خطأ (set private)
            // acc.AccountNumber = ""; // خطأ (لا يوجد set)

            // هذا يوضّح Data Hiding + Flexibility + Reusability + Testability
            Console.ReadKey();
        }
    }
}

