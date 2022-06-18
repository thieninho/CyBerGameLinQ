using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static CyberGameManage.fComputerManager;
using static CyberGameManage.fMessageBox;

namespace CyberGameManage.Payment
{
    internal class Facade
    {
        private static Facade _instance;

        private AccountService accountService;
        private PaymentService paymentService;
        private VATService vatService;
        private EmailService emailService;
        private SmsService smsService;

        private Facade()
        {
            accountService = new AccountService();
            paymentService = new PaymentService();
            vatService = new VATService();
            emailService = new EmailService();
            smsService = new SmsService();
        }

        public static Facade getInstance()
        {
            if (_instance == null)
                _instance = new Facade();
            return _instance;
        }

        public void buyProductByCash(string name)
        {
            
            paymentService.PaymentByCash();
            vatService.FreeVAT();
            accountService.GetAccout(name);
            //emailService.SendMail(name);
        }

        public void buyProductByMomo(string mail, string mobilePhone)
        {
            //accountService.GetAccout(name);
            paymentService.PaymentByMomo();
            vatService.FreeVAT();
            smsService.sendSMS(mobilePhone);
            emailService.SendMail(mail);
        }
    }
}
