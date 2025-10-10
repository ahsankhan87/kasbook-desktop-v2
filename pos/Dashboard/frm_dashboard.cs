using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using POS.Core;

namespace pos.Dashboard
{
    public class frm_dashboard : Form
    {
        private readonly frm_main _main;
        private Panel welcomePanel;
        private FlowLayoutPanel quickAccessPanel;

        public frm_dashboard(frm_main main)
        {
            _main = main ?? throw new ArgumentNullException(nameof(main));
            InitializeUi();
        }

        private void InitializeUi()
        {
            this.Text = "Dashboard";
            this.FormBorderStyle = FormBorderStyle.None;
            this.ControlBox = false;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.WindowState = FormWindowState.Maximized;
            this.BackColor = Color.FromArgb(245, 247, 250);

            welcomePanel = new Panel
            {
                Height = 120,
                Dock = DockStyle.Top,
                BackColor = Color.FromArgb(41, 128, 185),
                Padding = new Padding(30, 20, 30, 20)
            };

            var lblWelcome = new Label
            {
                Text = $"Welcome back, {UsersModal.logged_in_username}!",
                Font = new Font("Segoe UI", 20F, FontStyle.Bold),
                ForeColor = Color.White,
                AutoSize = true,
                Location = new Point(30, 20)
            };

            var lblInfo = new Label
            {
                Text = $"{UsersModal.logged_in_branch_name} • {UsersModal.fiscal_year} • {DateTime.Now:dddd, MMMM dd, yyyy}",
                Font = new Font("Segoe UI", 10F),
                ForeColor = Color.FromArgb(220, 230, 240),
                AutoSize = true,
                Location = new Point(30, 60)
            };

            welcomePanel.Controls.AddRange(new Control[] { lblWelcome, lblInfo });

            var lblQuickAccess = new Label
            {
                Text = "Quick Access",
                Font = new Font("Segoe UI", 16F, FontStyle.Bold),
                ForeColor = Color.FromArgb(52, 73, 94),
                AutoSize = true,
                Location = new Point(20, 20),
                Padding = new Padding(10)
            };

            quickAccessPanel = new FlowLayoutPanel
            {
                AutoScroll = true,
                Padding = new Padding(20, 60, 20, 20),
                BackColor = Color.FromArgb(245, 247, 250),
                Dock = DockStyle.Fill
            };

            quickAccessPanel.Controls.Add(lblQuickAccess);
            LoadQuickAccessButtons();

            Controls.Add(quickAccessPanel);
            Controls.Add(welcomePanel);
        }

        private void LoadQuickAccessButtons()
        {
            var buttons = new List<DashboardButtonInfo>
            {
                new DashboardButtonInfo("New Sale", "Create a new sales invoice", Color.FromArgb(46, 204, 113), "??", _main.OpenNewSale),
                new DashboardButtonInfo("Products", "View and manage products", Color.FromArgb(52, 152, 219), "??", _main.OpenProducts),
                new DashboardButtonInfo("Customers", "Manage customer records", Color.FromArgb(155, 89, 182), "??", _main.OpenCustomers),
                new DashboardButtonInfo("Sales Report", "View detailed sales reports", Color.FromArgb(230, 126, 34), "??", _main.OpenSalesReport)
            };

            if (UsersModal.logged_in_user_level == 1)
            {
                buttons.AddRange(new[]
                {
                    new DashboardButtonInfo("Purchase", "Create purchase order", Color.FromArgb(231, 76, 60), "??", _main.OpenPurchase),
                    new DashboardButtonInfo("Suppliers", "Manage suppliers", Color.FromArgb(41, 128, 185), "??", _main.OpenSuppliers),
                    new DashboardButtonInfo("Reports", "Financial & inventory reports", Color.FromArgb(52, 73, 94), "??", _main.OpenReportsHome),
                    new DashboardButtonInfo("Settings", "System configuration", Color.FromArgb(127, 140, 141), "??", _main.OpenSettings)
                });
            }

            foreach (var btn in buttons)
            {
                quickAccessPanel.Controls.Add(CreateDashboardCard(btn));
            }
        }

        private Panel CreateDashboardCard(DashboardButtonInfo button)
        {
            var card = new Panel
            {
                Width = 220,
                Height = 160,
                Margin = new Padding(15),
                BackColor = Color.White,
                Cursor = Cursors.Hand,
                Tag = button
            };

            var colorBar = new Panel { Height = 6, Dock = DockStyle.Top, BackColor = button.Color };

            var icon = new Label
            {
                Text = button.Icon,
                Font = new Font("Segoe UI Emoji", 36F),
                TextAlign = ContentAlignment.MiddleCenter,
                AutoSize = false,
                Width = card.Width,
                Height = 70,
                Location = new Point(0, 20),
                ForeColor = button.Color
            };

            var title = new Label
            {
                Text = button.Title,
                Font = new Font("Segoe UI", 12F, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleCenter,
                AutoSize = false,
                Width = card.Width - 20,
                Height = 30,
                Location = new Point(10, 95),
                ForeColor = Color.FromArgb(44, 62, 80)
            };

            var description = new Label
            {
                Text = button.Description,
                Font = new Font("Segoe UI", 8.5F),
                TextAlign = ContentAlignment.TopCenter,
                AutoSize = false,
                Width = card.Width - 20,
                Height = 30,
                Location = new Point(10, 125),
                ForeColor = Color.FromArgb(149, 165, 166)
            };

            card.Controls.AddRange(new Control[] { colorBar, icon, title, description });

            EventHandler clickHandler = (s, e) => button.Action?.Invoke();
            card.Click += clickHandler;
            icon.Click += clickHandler;
            title.Click += clickHandler;
            description.Click += clickHandler;

            card.MouseEnter += (s, e) => { card.BackColor = Color.FromArgb(250, 252, 254); title.ForeColor = button.Color; card.Padding = new Padding(2); };
            card.MouseLeave += (s, e) => { card.BackColor = Color.White; title.ForeColor = Color.FromArgb(44, 62, 80); card.Padding = new Padding(0); };

            return card;
        }

        private class DashboardButtonInfo
        {
            public string Title { get; }
            public string Description { get; }
            public Color Color { get; }
            public string Icon { get; }
            public Action Action { get; }
            public DashboardButtonInfo(string title, string description, Color color, string icon, Action action)
            {
                Title = title; Description = description; Color = color; Icon = icon; Action = action;
            }
        }
    }
}
