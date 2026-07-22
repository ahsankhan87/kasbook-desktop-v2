using pos.UI;
using System;
using System.Text;
using System.Windows.Forms;

namespace pos
{
    public partial class frm_financial_management_help : Form
    {
        public frm_financial_management_help()
        {
            InitializeComponent();
        }

        private void frm_financial_management_help_Load(object sender, EventArgs e)
        {
            AppTheme.Apply(this);
            txtHelpEnglish.Text = BuildEnglishHelp();
            txtHelpArabic.Text = BuildArabicHelp();
            txtHelpEnglish.SelectionStart = 0;
            txtHelpEnglish.SelectionLength = 0;
            txtHelpArabic.SelectionStart = 0;
            txtHelpArabic.SelectionLength = 0;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private static string BuildEnglishHelp()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("Financial Management Guide");
            sb.AppendLine("========================");
            sb.AppendLine();
            sb.AppendLine("1) Purpose");
            sb.AppendLine("- Financial period management controls when accounting transactions are allowed.");
            sb.AppendLine("- It protects accounting integrity by locking reviewed periods.");
            sb.AppendLine("- It supports compliance, audit readiness, and controlled month-end/year-end operations.");
            sb.AppendLine();
            sb.AppendLine("2) Main Objects");
            sb.AppendLine("- Fiscal Year: Start and end boundaries for accounting year.");
            sb.AppendLine("- Financial Period: Monthly (or configured) segment inside a fiscal year.");
            sb.AppendLine("- Accounting Entries: Voucher header + lines posted in acc_entries_header and acc_entries.");
            sb.AppendLine();
            sb.AppendLine("3) Status Lifecycle");
            sb.AppendLine("- Open: Transactions are allowed.");
            sb.AppendLine("- Soft-Closed: Normal entry is blocked; period can be reopened by authorized users.");
            sb.AppendLine("- Hard-Locked: Final lock; no further posting/reopen through normal flow.");
            sb.AppendLine();
            sb.AppendLine("4) How the Process Works");
            sb.AppendLine("Step A - Open New Period");
            sb.AppendLine("- Creates the next period based on previous period end date.");
            sb.AppendLine("- New period starts in Open status.");
            sb.AppendLine();
            sb.AppendLine("Step B - Pre-Close Review");
            sb.AppendLine("- Use the Period Close Wizard checklist.");
            sb.AppendLine("- Validate posting completeness and out-of-balance scenarios.");
            sb.AppendLine("- Review period summary totals: debits, credits, journals, and transaction volume.");
            sb.AppendLine();
            sb.AppendLine("Step C - Soft Close");
            sb.AppendLine("- Requires user confirmation and PIN/password entry.");
            sb.AppendLine("- Optional actions can include auto depreciation and accrual reversal.");
            sb.AppendLine("- If checklist has pending issues, soft close should not proceed.");
            sb.AppendLine();
            sb.AppendLine("Step D - Reopen (Controlled)");
            sb.AppendLine("- Used only when corrections are required.");
            sb.AppendLine("- Requires reason + admin validation.");
            sb.AppendLine("- Every reopen should be justified and auditable.");
            sb.AppendLine();
            sb.AppendLine("Step E - Hard Lock");
            sb.AppendLine("- Final period lock after all reviews are complete.");
            sb.AppendLine("- Use carefully, typically after month-end sign-off.");
            sb.AppendLine();
            sb.AppendLine("5) Controls in Financial Period Management Screen");
            sb.AppendLine("- Open New Period: Creates next month.");
            sb.AppendLine("- Soft Close Period: Starts closure wizard.");
            sb.AppendLine("- Hard Lock Period: Final lock.");
            sb.AppendLine("- Reopen Period: Reopens eligible soft-closed period.");
            sb.AppendLine("- View Period Transactions: Drill-down review of posted vouchers.");
            sb.AppendLine("- Year-End Closing: Runs year-end validation/close workflow.");
            sb.AppendLine();
            sb.AppendLine("6) Checklist and Summary Meaning");
            sb.AppendLine("- Checklist indicates whether key closure conditions are satisfied.");
            sb.AppendLine("- Pending count > 0 means unresolved items remain.");
            sb.AppendLine("- Out-of-balance entries require immediate accounting correction.");
            sb.AppendLine();
            sb.AppendLine("7) Security and Governance");
            sb.AppendLine("- Actions are role-sensitive (view/edit/admin boundaries).");
            sb.AppendLine("- Soft close, hard lock, and reopen actions should be logged.");
            sb.AppendLine("- Keep user credentials private; never share PIN/password.");
            sb.AppendLine();
            sb.AppendLine("8) Best Practices");
            sb.AppendLine("- Reconcile bank, sales, purchases, and journal entries before soft close.");
            sb.AppendLine("- Resolve all checklist failures before locking.");
            sb.AppendLine("- Reopen only when necessary and document root cause.");
            sb.AppendLine("- Keep periods sequential and avoid date overlaps.");
            sb.AppendLine();
            sb.AppendLine("9) Common Issues");
            sb.AppendLine("- Cannot soft close: checklist has pending issues or invalid confirmation flow.");
            sb.AppendLine("- Cannot reopen: period is not soft-closed or user lacks admin authorization.");
            sb.AppendLine("- Wrong period status: refresh list and verify selected row.");
            sb.AppendLine();
            sb.AppendLine("10) Short Operational Policy");
            sb.AppendLine("- Daily: post and validate transactions.");
            sb.AppendLine("- Month-end: run checklist, review summary, soft close.");
            sb.AppendLine("- After approval: hard lock.");
            sb.AppendLine("- Year-end: execute year-end close with full validation.");
            return sb.ToString();
        }

        private static string BuildArabicHelp()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("دليل إدارة الفترات المالية");
            sb.AppendLine("========================");
            sb.AppendLine();
            sb.AppendLine("1) الهدف");
            sb.AppendLine("- إدارة الفترات المالية تنظم متى يُسمح بالقيود المحاسبية.");
            sb.AppendLine("- تحافظ على سلامة الحسابات عبر إقفال الفترات بعد مراجعتها.");
            sb.AppendLine("- تدعم الالتزام والتدقيق وإجراءات الإقفال الشهري والسنوي.");
            sb.AppendLine();
            sb.AppendLine("2) العناصر الأساسية");
            sb.AppendLine("- السنة المالية: حدود بداية ونهاية السنة المحاسبية.");
            sb.AppendLine("- الفترة المالية: جزء شهري (أو حسب الإعداد) داخل السنة.");
            sb.AppendLine("- القيود: رأس القيد وبنوده في acc_entries_header و acc_entries.");
            sb.AppendLine();
            sb.AppendLine("3) دورة الحالة");
            sb.AppendLine("- Open: الفترة مفتوحة ويُسمح بالتسجيل.");
            sb.AppendLine("- Soft-Closed: إغلاق أولي، يمكن إعادة الفتح بصلاحية.");
            sb.AppendLine("- Hard-Locked: قفل نهائي بعد اكتمال جميع المراجعات.");
            sb.AppendLine();
            sb.AppendLine("4) طريقة العمل");
            sb.AppendLine("الخطوة أ - فتح فترة جديدة");
            sb.AppendLine("- يتم إنشاء الفترة التالية اعتماداً على نهاية آخر فترة.");
            sb.AppendLine("- تبدأ الحالة كـ Open.");
            sb.AppendLine();
            sb.AppendLine("الخطوة ب - المراجعة قبل الإقفال");
            sb.AppendLine("- استخدم معالج إقفال الفترة وقائمة التحقق.");
            sb.AppendLine("- تأكد من اكتمال الترحيل وعدم وجود فروقات.");
            sb.AppendLine("- راجع ملخص الفترة: المدين، الدائن، القيود، وعدد الحركات.");
            sb.AppendLine();
            sb.AppendLine("الخطوة ج - الإقفال الأولي Soft Close");
            sb.AppendLine("- يتطلب تأكيد المستخدم وإدخال كلمة المرور/الرقم السري.");
            sb.AppendLine("- يمكن تنفيذ خيارات إضافية مثل الاستهلاك وعكس الاستحقاقات.");
            sb.AppendLine("- إذا كانت قائمة التحقق فيها عناصر معلقة فلن يكتمل الإقفال.");
            sb.AppendLine();
            sb.AppendLine("الخطوة د - إعادة الفتح (مقيد)");
            sb.AppendLine("- تُستخدم عند الحاجة لتصحيح أخطاء.");
            sb.AppendLine("- تتطلب سبباً واضحاً وتأكيد صلاحية المسؤول.");
            sb.AppendLine("- يجب أن تكون العملية قابلة للتدقيق.");
            sb.AppendLine();
            sb.AppendLine("الخطوة هـ - القفل النهائي Hard Lock");
            sb.AppendLine("- قفل نهائي بعد اكتمال المراجعات.");
            sb.AppendLine("- يُنفذ عادة بعد اعتماد إقفال الشهر.");
            sb.AppendLine();
            sb.AppendLine("5) عناصر الشاشة الرئيسية لإدارة الفترات");
            sb.AppendLine("- Open New Period: فتح الفترة التالية.");
            sb.AppendLine("- Soft Close Period: بدء معالج الإغلاق الأولي.");
            sb.AppendLine("- Hard Lock Period: تنفيذ القفل النهائي.");
            sb.AppendLine("- Reopen Period: إعادة فتح فترة مغلقة أولياً عند الحاجة.");
            sb.AppendLine("- View Period Transactions: عرض تفاصيل حركات الفترة.");
            sb.AppendLine("- Year-End Closing: تشغيل إقفال نهاية السنة.");
            sb.AppendLine();
            sb.AppendLine("6) معنى قائمة التحقق والملخص");
            sb.AppendLine("- قائمة التحقق توضح جاهزية الفترة للإقفال.");
            sb.AppendLine("- إذا كان Pending Count أكبر من صفر فهناك عناصر غير منجزة.");
            sb.AppendLine("- وجود قيود غير متوازنة يتطلب تصحيحاً فورياً.");
            sb.AppendLine();
            sb.AppendLine("7) الأمان والحوكمة");
            sb.AppendLine("- العمليات مرتبطة بالصلاحيات (عرض/تعديل/مسؤول).");
            sb.AppendLine("- الإقفال، القفل النهائي، وإعادة الفتح يجب أن تكون مسجلة.");
            sb.AppendLine("- لا تشارك كلمة المرور أو الرقم السري.");
            sb.AppendLine();
            sb.AppendLine("8) أفضل الممارسات");
            sb.AppendLine("- طابق البنك والمبيعات والمشتريات والقيود قبل الإقفال.");
            sb.AppendLine("- عالج جميع أخطاء قائمة التحقق قبل القفل.");
            sb.AppendLine("- لا تعيد الفتح إلا للضرورة مع توثيق السبب.");
            sb.AppendLine("- حافظ على تسلسل الفترات بدون تداخل بالتواريخ.");
            sb.AppendLine();
            sb.AppendLine("9) مشاكل شائعة");
            sb.AppendLine("- فشل الإقفال الأولي: عناصر معلقة أو تأكيد غير مكتمل.");
            sb.AppendLine("- فشل إعادة الفتح: الفترة ليست Soft-Closed أو لا توجد صلاحية.");
            sb.AppendLine("- عرض حالة غير صحيحة: حدّث القائمة وتأكد من الصف المحدد.");
            sb.AppendLine();
            sb.AppendLine("10) سياسة تشغيل مختصرة");
            sb.AppendLine("- يومياً: تسجيل ومراجعة الحركات.");
            sb.AppendLine("- نهاية الشهر: قائمة التحقق ثم الملخص ثم الإقفال الأولي.");
            sb.AppendLine("- بعد الاعتماد: القفل النهائي.");
            sb.AppendLine("- نهاية السنة: تنفيذ الإقفال السنوي مع المراجعات.");
            return sb.ToString();
        }
    }
}
