namespace AssemblyExplorer
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose( bool disposing )
        {
            if ( disposing && ( components != null ) )
            {
                components.Dispose();
            }
            base.Dispose( disposing );
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of the current method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.splitContainerVertical = new System.Windows.Forms.SplitContainer();
            this.splitContainerLeft = new System.Windows.Forms.SplitContainer();
            this.treeView = new AutonomousTreeView();
            this.imageListTreeView = new System.Windows.Forms.ImageList(this.components);
            this.treeViewDefinition = new AutonomousTreeView();
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tabPageProperties = new System.Windows.Forms.TabPage();
            this.labelElementType = new System.Windows.Forms.Label();
            this.propertyGrid = new System.Windows.Forms.PropertyGrid();
            this.tabPageMsil = new System.Windows.Forms.TabPage();
            this.textBoxMsil = new System.Windows.Forms.TextBox();
            this.menuStrip = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.fileOpenMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripMenuItemOptions = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.exitMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openModuleFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.saveMsilFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.splitContainerVertical.Panel1.SuspendLayout();
            this.splitContainerVertical.Panel2.SuspendLayout();
            this.splitContainerVertical.SuspendLayout();
            this.splitContainerLeft.Panel1.SuspendLayout();
            this.splitContainerLeft.Panel2.SuspendLayout();
            this.splitContainerLeft.SuspendLayout();
            this.tabControl.SuspendLayout();
            this.tabPageProperties.SuspendLayout();
            this.tabPageMsil.SuspendLayout();
            this.menuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainerVertical
            // 
            this.splitContainerVertical.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainerVertical.Location = new System.Drawing.Point(5, 27);
            this.splitContainerVertical.Name = "splitContainerVertical";
            // 
            // splitContainerVertical.Panel1
            // 
            this.splitContainerVertical.Panel1.Controls.Add(this.splitContainerLeft);
            // 
            // splitContainerVertical.Panel2
            // 
            this.splitContainerVertical.Panel2.Controls.Add(this.tabControl);
            this.splitContainerVertical.Size = new System.Drawing.Size(930, 369);
            this.splitContainerVertical.SplitterDistance = 484;
            this.splitContainerVertical.TabIndex = 0;
            this.splitContainerVertical.Text = "splitContainer1";
            // 
            // splitContainerLeft
            // 
            this.splitContainerLeft.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerLeft.Location = new System.Drawing.Point(0, 0);
            this.splitContainerLeft.Name = "splitContainerLeft";
            this.splitContainerLeft.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainerLeft.Panel1
            // 
            this.splitContainerLeft.Panel1.Controls.Add(this.treeView);
            // 
            // splitContainerLeft.Panel2
            // 
            this.splitContainerLeft.Panel2.Controls.Add(this.treeViewDefinition);
            this.splitContainerLeft.Size = new System.Drawing.Size(484, 369);
            this.splitContainerLeft.SplitterDistance = 217;
            this.splitContainerLeft.TabIndex = 0;
            this.splitContainerLeft.Text = "splitContainer2";
            // 
            // treeView
            // 
            this.treeView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeView.ImageIndex = 0;
            this.treeView.ImageList = this.imageListTreeView;
            this.treeView.Location = new System.Drawing.Point(0, 0);
            this.treeView.Name = "treeView";
            this.treeView.SelectedImageIndex = 0;
            this.treeView.Size = new System.Drawing.Size(484, 217);
            this.treeView.TabIndex = 0;
            this.treeView.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView_AfterSelect);
            // 
            // imageListTreeView
            // 
            this.imageListTreeView.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListTreeView.ImageStream")));
            this.imageListTreeView.TransparentColor = System.Drawing.Color.Fuchsia;
            this.imageListTreeView.Images.SetKeyName(0, "browser-1.bmp");
            this.imageListTreeView.Images.SetKeyName(1, "browser-2.bmp");
            this.imageListTreeView.Images.SetKeyName(2, "browser-3.bmp");
            this.imageListTreeView.Images.SetKeyName(3, "browser-4.bmp");
            this.imageListTreeView.Images.SetKeyName(4, "browser-5.bmp");
            this.imageListTreeView.Images.SetKeyName(5, "browser-6.bmp");
            this.imageListTreeView.Images.SetKeyName(6, "browser-7.bmp");
            this.imageListTreeView.Images.SetKeyName(7, "browser-8.bmp");
            this.imageListTreeView.Images.SetKeyName(8, "browser-9.bmp");
            this.imageListTreeView.Images.SetKeyName(9, "browser-10.bmp");
            this.imageListTreeView.Images.SetKeyName(10, "browser-11.bmp");
            this.imageListTreeView.Images.SetKeyName(11, "browser-12.bmp");
            this.imageListTreeView.Images.SetKeyName(12, "browser-13.bmp");
            this.imageListTreeView.Images.SetKeyName(13, "browser-14.bmp");
            this.imageListTreeView.Images.SetKeyName(14, "browser-15.bmp");
            this.imageListTreeView.Images.SetKeyName(15, "browser-16.bmp");
            this.imageListTreeView.Images.SetKeyName(16, "browser-17.bmp");
            this.imageListTreeView.Images.SetKeyName(17, "browser-18.bmp");
            this.imageListTreeView.Images.SetKeyName(18, "browser-19.bmp");
            this.imageListTreeView.Images.SetKeyName(19, "browser-20.bmp");
            this.imageListTreeView.Images.SetKeyName(20, "browser-21.bmp");
            this.imageListTreeView.Images.SetKeyName(21, "browser-22.bmp");
            this.imageListTreeView.Images.SetKeyName(22, "browser-23.bmp");
            this.imageListTreeView.Images.SetKeyName(23, "browser-24.bmp");
            this.imageListTreeView.Images.SetKeyName(24, "browser-25.bmp");
            this.imageListTreeView.Images.SetKeyName(25, "browser-26.bmp");
            this.imageListTreeView.Images.SetKeyName(26, "browser-27.bmp");
            this.imageListTreeView.Images.SetKeyName(27, "browser-28.bmp");
            this.imageListTreeView.Images.SetKeyName(28, "browser-29.bmp");
            this.imageListTreeView.Images.SetKeyName(29, "browser-30.bmp");
            this.imageListTreeView.Images.SetKeyName(30, "browser-31.bmp");
            this.imageListTreeView.Images.SetKeyName(31, "browser-32.bmp");
            this.imageListTreeView.Images.SetKeyName(32, "browser-33.bmp");
            this.imageListTreeView.Images.SetKeyName(33, "browser-34.bmp");
            this.imageListTreeView.Images.SetKeyName(34, "browser-35.bmp");
            this.imageListTreeView.Images.SetKeyName(35, "browser-36.bmp");
            this.imageListTreeView.Images.SetKeyName(36, "browser-37.bmp");
            this.imageListTreeView.Images.SetKeyName(37, "browser-38.bmp");
            this.imageListTreeView.Images.SetKeyName(38, "browser-39.bmp");
            this.imageListTreeView.Images.SetKeyName(39, "browser-40.bmp");
            this.imageListTreeView.Images.SetKeyName(40, "browser-41.bmp");
            this.imageListTreeView.Images.SetKeyName(41, "browser-42.bmp");
            this.imageListTreeView.Images.SetKeyName(42, "browser-43.bmp");
            this.imageListTreeView.Images.SetKeyName(43, "browser-44.bmp");
            this.imageListTreeView.Images.SetKeyName(44, "browser-45.bmp");
            this.imageListTreeView.Images.SetKeyName(45, "browser-46.bmp");
            this.imageListTreeView.Images.SetKeyName(46, "browser-47.bmp");
            this.imageListTreeView.Images.SetKeyName(47, "browser-48.bmp");
            this.imageListTreeView.Images.SetKeyName(48, "browser-49.bmp");
            this.imageListTreeView.Images.SetKeyName(49, "browser-50.bmp");
            this.imageListTreeView.Images.SetKeyName(50, "browser-51.bmp");
            this.imageListTreeView.Images.SetKeyName(51, "browser-52.bmp");
            this.imageListTreeView.Images.SetKeyName(52, "browser-53.bmp");
            this.imageListTreeView.Images.SetKeyName(53, "browser-54.bmp");
            this.imageListTreeView.Images.SetKeyName(54, "browser-55.bmp");
            this.imageListTreeView.Images.SetKeyName(55, "browser-56.bmp");
            this.imageListTreeView.Images.SetKeyName(56, "browser-57.bmp");
            this.imageListTreeView.Images.SetKeyName(57, "browser-58.bmp");
            this.imageListTreeView.Images.SetKeyName(58, "browser-59.bmp");
            this.imageListTreeView.Images.SetKeyName(59, "browser-60.bmp");
            this.imageListTreeView.Images.SetKeyName(60, "browser-61.bmp");
            this.imageListTreeView.Images.SetKeyName(61, "browser-62.bmp");
            this.imageListTreeView.Images.SetKeyName(62, "browser-63.bmp");
            this.imageListTreeView.Images.SetKeyName(63, "browser-64.bmp");
            this.imageListTreeView.Images.SetKeyName(64, "browser-65.bmp");
            this.imageListTreeView.Images.SetKeyName(65, "browser-66.bmp");
            this.imageListTreeView.Images.SetKeyName(66, "browser-67.bmp");
            this.imageListTreeView.Images.SetKeyName(67, "browser-68.bmp");
            this.imageListTreeView.Images.SetKeyName(68, "browser-69.bmp");
            this.imageListTreeView.Images.SetKeyName(69, "browser-70.bmp");
            this.imageListTreeView.Images.SetKeyName(70, "browser-71.bmp");
            this.imageListTreeView.Images.SetKeyName(71, "browser-72.bmp");
            this.imageListTreeView.Images.SetKeyName(72, "browser-73.bmp");
            this.imageListTreeView.Images.SetKeyName(73, "browser-74.bmp");
            this.imageListTreeView.Images.SetKeyName(74, "browser-75.bmp");
            this.imageListTreeView.Images.SetKeyName(75, "browser-76.bmp");
            this.imageListTreeView.Images.SetKeyName(76, "browser-77.bmp");
            this.imageListTreeView.Images.SetKeyName(77, "browser-78.bmp");
            this.imageListTreeView.Images.SetKeyName(78, "browser-79.bmp");
            this.imageListTreeView.Images.SetKeyName(79, "browser-80.bmp");
            this.imageListTreeView.Images.SetKeyName(80, "browser-81.bmp");
            this.imageListTreeView.Images.SetKeyName(81, "browser-82.bmp");
            this.imageListTreeView.Images.SetKeyName(82, "browser-83.bmp");
            this.imageListTreeView.Images.SetKeyName(83, "browser-84.bmp");
            this.imageListTreeView.Images.SetKeyName(84, "browser-85.bmp");
            this.imageListTreeView.Images.SetKeyName(85, "browser-86.bmp");
            this.imageListTreeView.Images.SetKeyName(86, "browser-87.bmp");
            this.imageListTreeView.Images.SetKeyName(87, "browser-88.bmp");
            this.imageListTreeView.Images.SetKeyName(88, "browser-89.bmp");
            this.imageListTreeView.Images.SetKeyName(89, "browser-90.bmp");
            this.imageListTreeView.Images.SetKeyName(90, "browser-91.bmp");
            this.imageListTreeView.Images.SetKeyName(91, "browser-92.bmp");
            this.imageListTreeView.Images.SetKeyName(92, "browser-93.bmp");
            this.imageListTreeView.Images.SetKeyName(93, "browser-94.bmp");
            this.imageListTreeView.Images.SetKeyName(94, "browser-95.bmp");
            this.imageListTreeView.Images.SetKeyName(95, "browser-96.bmp");
            this.imageListTreeView.Images.SetKeyName(96, "browser-97.bmp");
            this.imageListTreeView.Images.SetKeyName(97, "browser-98.bmp");
            this.imageListTreeView.Images.SetKeyName(98, "browser-99.bmp");
            this.imageListTreeView.Images.SetKeyName(99, "browser-100.bmp");
            this.imageListTreeView.Images.SetKeyName(100, "browser-101.bmp");
            this.imageListTreeView.Images.SetKeyName(101, "browser-102.bmp");
            this.imageListTreeView.Images.SetKeyName(102, "browser-103.bmp");
            this.imageListTreeView.Images.SetKeyName(103, "browser-104.bmp");
            this.imageListTreeView.Images.SetKeyName(104, "browser-105.bmp");
            this.imageListTreeView.Images.SetKeyName(105, "browser-106.bmp");
            this.imageListTreeView.Images.SetKeyName(106, "browser-107.bmp");
            this.imageListTreeView.Images.SetKeyName(107, "browser-108.bmp");
            this.imageListTreeView.Images.SetKeyName(108, "browser-109.bmp");
            this.imageListTreeView.Images.SetKeyName(109, "browser-110.bmp");
            this.imageListTreeView.Images.SetKeyName(110, "browser-111.bmp");
            this.imageListTreeView.Images.SetKeyName(111, "browser-112.bmp");
            this.imageListTreeView.Images.SetKeyName(112, "browser-113.bmp");
            this.imageListTreeView.Images.SetKeyName(113, "browser-114.bmp");
            this.imageListTreeView.Images.SetKeyName(114, "browser-115.bmp");
            this.imageListTreeView.Images.SetKeyName(115, "browser-116.bmp");
            this.imageListTreeView.Images.SetKeyName(116, "browser-117.bmp");
            this.imageListTreeView.Images.SetKeyName(117, "browser-118.bmp");
            this.imageListTreeView.Images.SetKeyName(118, "browser-119.bmp");
            this.imageListTreeView.Images.SetKeyName(119, "browser-120.bmp");
            this.imageListTreeView.Images.SetKeyName(120, "folder.bmp");
            this.imageListTreeView.Images.SetKeyName(121, "assembly.bmp");
            this.imageListTreeView.Images.SetKeyName(122, "parameter.bmp");
            // 
            // treeViewDefinition
            // 
            this.treeViewDefinition.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeViewDefinition.ImageIndex = 0;
            this.treeViewDefinition.ImageList = this.imageListTreeView;
            this.treeViewDefinition.Location = new System.Drawing.Point(0, 0);
            this.treeViewDefinition.Name = "treeViewDefinition";
            this.treeViewDefinition.SelectedImageIndex = 0;
            this.treeViewDefinition.Size = new System.Drawing.Size(484, 148);
            this.treeViewDefinition.TabIndex = 0;
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.tabPageProperties);
            this.tabControl.Controls.Add(this.tabPageMsil);
            this.tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl.Location = new System.Drawing.Point(0, 0);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(442, 369);
            this.tabControl.TabIndex = 3;
            this.tabControl.SelectedIndexChanged += new System.EventHandler(this.tabControl_SelectedIndexChanged);
            // 
            // tabPageProperties
            // 
            this.tabPageProperties.Controls.Add(this.labelElementType);
            this.tabPageProperties.Controls.Add(this.propertyGrid);
            this.tabPageProperties.Location = new System.Drawing.Point(4, 22);
            this.tabPageProperties.Name = "tabPageProperties";
            this.tabPageProperties.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageProperties.Size = new System.Drawing.Size(434, 343);
            this.tabPageProperties.TabIndex = 0;
            this.tabPageProperties.Text = "Properties";
            this.tabPageProperties.UseVisualStyleBackColor = true;
            // 
            // labelElementType
            // 
            this.labelElementType.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labelElementType.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelElementType.Location = new System.Drawing.Point(181, 3);
            this.labelElementType.Name = "labelElementType";
            this.labelElementType.Size = new System.Drawing.Size(242, 17);
            this.labelElementType.TabIndex = 3;
            this.labelElementType.Text = "<Element Type Here>";
            this.labelElementType.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // propertyGrid
            // 
            this.propertyGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.propertyGrid.Location = new System.Drawing.Point(3, 0);
            this.propertyGrid.Name = "propertyGrid";
            this.propertyGrid.Size = new System.Drawing.Size(428, 340);
            this.propertyGrid.TabIndex = 2;
            // 
            // tabPageMsil
            // 
            this.tabPageMsil.Controls.Add(this.textBoxMsil);
            this.tabPageMsil.Location = new System.Drawing.Point(4, 22);
            this.tabPageMsil.Name = "tabPageMsil";
            this.tabPageMsil.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageMsil.Size = new System.Drawing.Size(434, 343);
            this.tabPageMsil.TabIndex = 1;
            this.tabPageMsil.Text = "MSIL";
            this.tabPageMsil.UseVisualStyleBackColor = true;
            // 
            // textBoxMsil
            // 
            this.textBoxMsil.BackColor = System.Drawing.Color.White;
            this.textBoxMsil.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxMsil.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.textBoxMsil.Location = new System.Drawing.Point(3, 3);
            this.textBoxMsil.Multiline = true;
            this.textBoxMsil.Name = "textBoxMsil";
            this.textBoxMsil.ReadOnly = true;
            this.textBoxMsil.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBoxMsil.Size = new System.Drawing.Size(428, 337);
            this.textBoxMsil.TabIndex = 0;
            this.textBoxMsil.WordWrap = false;
            // 
            // menuStrip
            // 
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem});
            this.menuStrip.Location = new System.Drawing.Point(0, 0);
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.Size = new System.Drawing.Size(932, 24);
            this.menuStrip.TabIndex = 1;
            this.menuStrip.Text = "menuStrip";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileOpenMenuItem,
            this.toolStripSeparator1,
            this.toolStripMenuItemOptions,
            this.toolStripSeparator2,
            this.exitMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.F4)));
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "&File";
            // 
            // fileOpenMenuItem
            // 
            this.fileOpenMenuItem.Name = "fileOpenMenuItem";
            this.fileOpenMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.fileOpenMenuItem.Size = new System.Drawing.Size(155, 22);
            this.fileOpenMenuItem.Text = "&Open...";
            this.fileOpenMenuItem.Click += new System.EventHandler(this.fileOpenMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(152, 6);
            // 
            // toolStripMenuItemOptions
            // 
            this.toolStripMenuItemOptions.Name = "toolStripMenuItemOptions";
            this.toolStripMenuItemOptions.Size = new System.Drawing.Size(155, 22);
            this.toolStripMenuItemOptions.Text = "O&ptions...";
            this.toolStripMenuItemOptions.Click += new System.EventHandler(this.toolStripMenuItemOptions_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(152, 6);
            // 
            // exitMenuItem
            // 
            this.exitMenuItem.Name = "exitMenuItem";
            this.exitMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.F4)));
            this.exitMenuItem.Size = new System.Drawing.Size(155, 22);
            this.exitMenuItem.Text = "Ex&it";
            this.exitMenuItem.Click += new System.EventHandler(this.exitMenuItem_Click);
            // 
            // openModuleFileDialog
            // 
            this.openModuleFileDialog.DefaultExt = "dll";
            this.openModuleFileDialog.Filter = ".NET Modules (*.dll, *.exe)|*.dll;*.exe";
            // 
            // saveMsilFileDialog
            // 
            this.saveMsilFileDialog.DefaultExt = "il";
            // 
            // MainForm
            // 
            this.ClientSize = new System.Drawing.Size(932, 402);
            this.Controls.Add(this.splitContainerVertical);
            this.Controls.Add(this.menuStrip);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip;
            this.Name = "MainForm";
            this.Text = "PostSharp Explorer";
            this.splitContainerVertical.Panel1.ResumeLayout(false);
            this.splitContainerVertical.Panel2.ResumeLayout(false);
            this.splitContainerVertical.ResumeLayout(false);
            this.splitContainerLeft.Panel1.ResumeLayout(false);
            this.splitContainerLeft.Panel2.ResumeLayout(false);
            this.splitContainerLeft.ResumeLayout(false);
            this.tabControl.ResumeLayout(false);
            this.tabPageProperties.ResumeLayout(false);
            this.tabPageMsil.ResumeLayout(false);
            this.tabPageMsil.PerformLayout();
            this.menuStrip.ResumeLayout(false);
            this.menuStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion



		private System.Windows.Forms.SplitContainer splitContainerVertical;
		private System.Windows.Forms.ImageList imageListTreeView;
		private System.Windows.Forms.SplitContainer splitContainerLeft;
		private System.Windows.Forms.TextBox textBoxMsil;
		private System.Windows.Forms.MenuStrip menuStrip;
		private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem fileOpenMenuItem;
		private System.Windows.Forms.ToolStripMenuItem exitMenuItem;
		private System.Windows.Forms.PropertyGrid propertyGrid;
		private AutonomousTreeView treeView;
		private AutonomousTreeView treeViewDefinition;
		private System.Windows.Forms.OpenFileDialog openModuleFileDialog;
        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tabPageProperties;
        private System.Windows.Forms.TabPage tabPageMsil;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.Label labelElementType;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemOptions;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.SaveFileDialog saveMsilFileDialog;
    }
}

