namespace RobotControl
{
    partial class MainView
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.actionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newRobotToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addRobotIPToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mapToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.сlearToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveMapToolStripMenuItemToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadMapToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.StatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.label1 = new System.Windows.Forms.Label();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.Remove = new System.Windows.Forms.Button();
            this.timer2 = new System.Windows.Forms.Timer(this.components);
            this.Reconnect = new System.Windows.Forms.Button();
            this.menuStrip1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.actionsToolStripMenuItem,
            this.mapToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(792, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // actionsToolStripMenuItem
            // 
            this.actionsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newRobotToolStripMenuItem,
            this.addRobotIPToolStripMenuItem});
            this.actionsToolStripMenuItem.Name = "actionsToolStripMenuItem";
            this.actionsToolStripMenuItem.Size = new System.Drawing.Size(66, 20);
            this.actionsToolStripMenuItem.Text = "Объекты";
            // 
            // newRobotToolStripMenuItem
            // 
            this.newRobotToolStripMenuItem.Image = global::RobotControl.Resources.Robot;
            this.newRobotToolStripMenuItem.Name = "newRobotToolStripMenuItem";
            this.newRobotToolStripMenuItem.Size = new System.Drawing.Size(176, 22);
            this.newRobotToolStripMenuItem.Text = "Добавить Робота";
            this.newRobotToolStripMenuItem.Click += new System.EventHandler(this.newRobotToolStripMenuItem_Click);
            // 
            // addRobotIPToolStripMenuItem
            // 
            this.addRobotIPToolStripMenuItem.Image = global::RobotControl.Resources.Globe_Connected_icon;
            this.addRobotIPToolStripMenuItem.Name = "addRobotIPToolStripMenuItem";
            this.addRobotIPToolStripMenuItem.Size = new System.Drawing.Size(176, 22);
            this.addRobotIPToolStripMenuItem.Text = "Добавить Робота IP";
            this.addRobotIPToolStripMenuItem.Click += new System.EventHandler(this.addRobotIPToolStripMenuItem_Click);
            // 
            // mapToolStripMenuItem
            // 
            this.mapToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.сlearToolStripMenuItem,
            this.saveMapToolStripMenuItemToolStripMenuItem,
            this.loadMapToolStripMenuItem});
            this.mapToolStripMenuItem.Name = "mapToolStripMenuItem";
            this.mapToolStripMenuItem.Size = new System.Drawing.Size(50, 20);
            this.mapToolStripMenuItem.Text = "Карта";
            // 
            // сlearToolStripMenuItem
            // 
            this.сlearToolStripMenuItem.Image = global::RobotControl.Resources.eraser;
            this.сlearToolStripMenuItem.Name = "сlearToolStripMenuItem";
            this.сlearToolStripMenuItem.Size = new System.Drawing.Size(162, 22);
            this.сlearToolStripMenuItem.Text = "Очистить карту";
            this.сlearToolStripMenuItem.Click += new System.EventHandler(this.сlearToolStripMenuItem_Click);
            // 
            // saveMapToolStripMenuItemToolStripMenuItem
            // 
            this.saveMapToolStripMenuItemToolStripMenuItem.Image = global::RobotControl.Resources.mapka;
            this.saveMapToolStripMenuItemToolStripMenuItem.Name = "saveMapToolStripMenuItemToolStripMenuItem";
            this.saveMapToolStripMenuItemToolStripMenuItem.Size = new System.Drawing.Size(162, 22);
            this.saveMapToolStripMenuItemToolStripMenuItem.Text = "Сохранить карту";
            this.saveMapToolStripMenuItemToolStripMenuItem.Click += new System.EventHandler(this.saveMapToolStripMenuItemToolStripMenuItem_Click);
            // 
            // loadMapToolStripMenuItem
            // 
            this.loadMapToolStripMenuItem.Image = global::RobotControl.Resources.save;
            this.loadMapToolStripMenuItem.Name = "loadMapToolStripMenuItem";
            this.loadMapToolStripMenuItem.Size = new System.Drawing.Size(162, 22);
            this.loadMapToolStripMenuItem.Text = "Загрузить карту";
            this.loadMapToolStripMenuItem.Click += new System.EventHandler(this.loadMapToolStripMenuItem_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.StatusLabel1});
            this.statusStrip1.Location = new System.Drawing.Point(0, 551);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.statusStrip1.Size = new System.Drawing.Size(792, 22);
            this.statusStrip1.TabIndex = 1;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // StatusLabel1
            // 
            this.StatusLabel1.Name = "StatusLabel1";
            this.StatusLabel1.Size = new System.Drawing.Size(135, 17);
            this.StatusLabel1.Text = "Приложение запущено...";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.Location = new System.Drawing.Point(538, 32);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(140, 17);
            this.label1.TabIndex = 2;
            this.label1.Text = "Выбранный объект:";
            // 
            // listBox1
            // 
            this.listBox1.FormattingEnabled = true;
            this.listBox1.Location = new System.Drawing.Point(541, 61);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(239, 108);
            this.listBox1.TabIndex = 3;
            // 
            // Remove
            // 
            this.Remove.Location = new System.Drawing.Point(665, 182);
            this.Remove.Name = "Remove";
            this.Remove.Size = new System.Drawing.Size(115, 23);
            this.Remove.TabIndex = 4;
            this.Remove.Text = "Удалить робота";
            this.Remove.UseVisualStyleBackColor = true;
            this.Remove.Click += new System.EventHandler(this.Remove_Click);
            // 
            // timer2
            // 
            this.timer2.Enabled = true;
            this.timer2.Interval = 3000;
            this.timer2.Tick += new System.EventHandler(this.timer2_Tick);
            // 
            // Reconnect
            // 
            this.Reconnect.Enabled = false;
            this.Reconnect.Location = new System.Drawing.Point(541, 182);
            this.Reconnect.Name = "Reconnect";
            this.Reconnect.Size = new System.Drawing.Size(118, 23);
            this.Reconnect.TabIndex = 5;
            this.Reconnect.Text = "Переподключиться";
            this.Reconnect.UseVisualStyleBackColor = true;
            this.Reconnect.Click += new System.EventHandler(this.Reconnect_Click);
            // 
            // MainView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(792, 573);
            this.Controls.Add(this.Reconnect);
            this.Controls.Add(this.Remove);
            this.Controls.Add(this.listBox1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.menuStrip1);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MainMenuStrip = this.menuStrip1;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MainView";
            this.Text = "Основное Окно";
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.MainView_Paint);
            this.MouseClick += new System.Windows.Forms.MouseEventHandler(this.MainView_MouseClick);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem actionsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem newRobotToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem addRobotIPToolStripMenuItem;
        public System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.Label label1;
        public System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.Button Remove;
        public System.Windows.Forms.ToolStripStatusLabel StatusLabel1;
        private System.Windows.Forms.Timer timer2;
        private System.Windows.Forms.ToolStripMenuItem mapToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem сlearToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveMapToolStripMenuItemToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem loadMapToolStripMenuItem;
        public System.Windows.Forms.Button Reconnect;
    }
}

