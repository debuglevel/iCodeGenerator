using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using iCodeGenerator.DatabaseNavigator;
using iCodeGenerator.DatabaseStructure;
using iCodeGenerator.DataTypeConverter;
using iCodeGenerator.Generator;
using iCodeGenerator.Updater;
//using TD.SandBar;
using TD.SandDock;
//using ToolBar=TD.SandBar.ToolBar;

namespace iCodeGenerator.iCodeGeneratorGui
{
	public class MainWindow : Form
	{
		#region Constants

		public const string Version = "0.1.0";

		#endregion

		#region Attributes

		private DockContainer leftSandDock;
		private DockContainer rightSandDock;
		private DockContainer bottomSandDock;
        private DockContainer topSandDock;
        private DockControl uiNavigatorDock;
		private DocumentContainer uiTemplateContainer;
		private SandDockManager uiSandDockManager;
        //private SandBarManager uiSandBarManager;
		private DockControl uiTemplateDock;
        private RichTextBox uiTemplateTextBox;
        private NavigatorControl uiNavigatorControl;
		private Container components = null;
		private DockControl uiGenerateCodeDock;
        private RichTextBox uiGeneratedCodeTextBox;
		private OpenFileDialog uiOpenTemplateDialog;
        private SaveFileDialog uiSaveDialog;
		private Panel uiPropertiesPanel;
		private DockControl uiPropertiesDock;
        private PropertyGrid uiPropertyEditor;
		private static Table selectedTable = null;
        private static Table[] selectedTables = null;
		private OpenFileDialog uiOpenMergeDialog;
		private DataGrid uiCustomValuesDataGrid;
		private DockControl uiCustomValuesDock;
		private DataGridTableStyle uiCustomValuesGridStyle;
		private string _TempFilename;
		private DataGridTextBoxColumn uiName;
		private DataGridTextBoxColumn uiValue;
		private static string _TemplateFilename;
        private DataSet _CustomValues;
        private static string _CustomValuesFilename = AppDomain.CurrentDomain.BaseDirectory + "CustomValues.xml";
		private static string _InputTemplateFolder = String.Empty;
        private static string _OutputTemplateFolder = String.Empty;
        private DockControl uiSnippets;

		#endregion
        private MenuStrip uiMenuStrip;
        private ToolStripMenuItem fileToolStripMenuItem;
        private ToolStripMenuItem connectToolStripMenuItem;
        private ToolStripMenuItem disconnectToolStripMenuItem;
        private ToolStripMenuItem configureCOnnectionToolStripMenuItem;
        private ToolStripMenuItem openTemplateToolStripMenuItem;
        private ToolStripMenuItem saveTemplateToolStripMenuItem;
        private ToolStripMenuItem saveAsTemplateToolStripMenuItem;
        private ToolStripMenuItem saveResultToolStripMenuItem;
        private ToolStripMenuItem exitToolStripMenuItem;
        private ToolStripMenuItem editToolStripMenuItem;
        private ToolStripMenuItem uNdoToolStripMenuItem;
        private ToolStripMenuItem redoToolStripMenuItem;
        private ToolStripMenuItem cutToolStripMenuItem;
        private ToolStripMenuItem copyToolStripMenuItem;
        private ToolStripMenuItem pasteToolStripMenuItem;
        private ToolStripMenuItem viewToolStripMenuItem;
        private ToolStripMenuItem navigatorToolStripMenuItem;
        private ToolStripMenuItem propertiesToolStripMenuItem;
        private ToolStripMenuItem customValuesToolStripMenuItem;
        private ToolStripMenuItem generatorToolStripMenuItem;
        private ToolStripMenuItem generateCodeToolStripMenuItem;
        private ToolStripMenuItem fileGeneratorConfigurationToolStripMenuItem;
        private ToolStripMenuItem fileGenerateToolStripMenuItem;
        private ToolStripMenuItem openOutputFolderToolStripMenuItem;
        private ToolStripMenuItem helpToolStripMenuItem;
        private ToolStripMenuItem aboutToolStripMenuItem;
        private ToolStrip uiToolStrip;
        private ToolStripButton tsGenerateCode;
        private ToolStripButton tsGenerateFile;
        private ToolStripButton tsGenerateMerge;
        private Panel panel1;
        private TableLayoutPanel tableLayoutPanel1;
        private ToolStripContainer toolStripContainer1;
        private StatusStrip uiStatusStrip;



        private SnippetsHelper _SnippetsHelper;

		#region Properties

		public static string InputTemplateFolder
		{
			get { return _InputTemplateFolder; }
			set { _InputTemplateFolder = value; }
		}

		public static string OutputTemplateFolder
		{
			get { return _OutputTemplateFolder; }
			set { _OutputTemplateFolder = value; }
		}

		#endregion

		public MainWindow()
		{
			UpdateCheckerThread();
			InitializeComponent();
			InitializeCustomValuesDataGrid();
			LoadSnippets();
		}

		private void LoadSnippets()
		{
			_SnippetsHelper = new SnippetsHelper();
			foreach (string key in _SnippetsHelper.Snippets.Keys)
			{
				AddSnippetButton(key);
			}
		}

		private void AddSnippetButton(string s)
		{
			Button button = new Button();
			button.Text = s;
			button.Dock = DockStyle.Top;
			button.FlatStyle = FlatStyle.Popup;
			button.Font = new Font("Microsoft Sans Serif", 
				7.2567F, 
				FontStyle.Regular, 
				GraphicsUnit.Point, ((Byte) (0)));
			button.Click += new EventHandler(button_Click);
			uiSnippets.Controls.Add(button);
		}


		private void button_Click(object sender, EventArgs e)
		{
			Button b = (Button) sender;
			RichTextBox tbox = uiTemplateTextBox;
			tbox.Text = tbox.Text.Insert(tbox.SelectionStart, _SnippetsHelper.Snippets[b.Text].ToString());
		}

		private void UpdateCheckerThread()
		{
			Thread thread = new Thread(new ThreadStart(CheckUpdates));
			thread.IsBackground = true;
			thread.Start();
		}

		private void CheckUpdates()
		{
			UpdateChecker checker = new UpdateChecker();
			if (checker.LoadConfiguration("http://codegenerator.sourceforge.net/iCodeGenerator.php?id=" + Version))
			{
				if (checker.Version != Version)
				{
					UpdatesWindow window = new UpdatesWindow();
					window.Name = "Update Notice";
					window.Url = checker.ApplicationUrl;
					window.Notice = checker.LatestChanges;
					window.NewVersion = checker.Version;
					window.CurrentVersion = Version;
					window.ShowDialog();
				}
			}
		}

		private void InitializeCustomValuesDataGrid()
		{
			_CustomValues = new DataSet();
			DataTable dt = new DataTable();
			_CustomValues.Tables.Add(dt);
			_CustomValues.Tables[0].Columns.Add("Name");
			_CustomValues.Tables[0].Columns.Add("Value");
			if (File.Exists(_CustomValuesFilename))
			{
				_CustomValues.ReadXml(_CustomValuesFilename);
			}
			uiCustomValuesDataGrid.DataSource = _CustomValues.Tables[0];
		}

		[STAThread]
		public static void Main()
		{
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
			Application.Run(new MainWindow());
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (components != null)
				{
					components.Dispose();
				}
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainWindow));
            this.uiSandDockManager = new TD.SandDock.SandDockManager();
            this.leftSandDock = new TD.SandDock.DockContainer();
            this.uiNavigatorDock = new TD.SandDock.DockControl();
            this.uiNavigatorControl = new iCodeGenerator.DatabaseNavigator.NavigatorControl();
            this.uiSnippets = new TD.SandDock.DockControl();
            this.uiPropertiesDock = new TD.SandDock.DockControl();
            this.uiPropertiesPanel = new System.Windows.Forms.Panel();
            this.uiPropertyEditor = new System.Windows.Forms.PropertyGrid();
            this.uiCustomValuesDock = new TD.SandDock.DockControl();
            this.uiCustomValuesDataGrid = new System.Windows.Forms.DataGrid();
            this.uiCustomValuesGridStyle = new System.Windows.Forms.DataGridTableStyle();
            this.uiName = new System.Windows.Forms.DataGridTextBoxColumn();
            this.uiValue = new System.Windows.Forms.DataGridTextBoxColumn();
            this.rightSandDock = new TD.SandDock.DockContainer();
            this.bottomSandDock = new TD.SandDock.DockContainer();
            this.topSandDock = new TD.SandDock.DockContainer();
            this.uiTemplateContainer = new TD.SandDock.DocumentContainer();
            this.uiTemplateDock = new TD.SandDock.DockControl();
            this.uiTemplateTextBox = new System.Windows.Forms.RichTextBox();
            this.uiGenerateCodeDock = new TD.SandDock.DockControl();
            this.uiGeneratedCodeTextBox = new System.Windows.Forms.RichTextBox();
            this.uiOpenTemplateDialog = new System.Windows.Forms.OpenFileDialog();
            this.uiSaveDialog = new System.Windows.Forms.SaveFileDialog();
            this.uiOpenMergeDialog = new System.Windows.Forms.OpenFileDialog();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.connectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.disconnectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.configureCOnnectionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openTemplateToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveTemplateToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveAsTemplateToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveResultToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.uNdoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.redoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.copyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pasteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.viewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.navigatorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.propertiesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.customValuesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.generatorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.generateCodeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.fileGeneratorConfigurationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.fileGenerateToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openOutputFolderToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.uiMenuStrip = new System.Windows.Forms.MenuStrip();
            this.uiToolStrip = new System.Windows.Forms.ToolStrip();
            this.tsGenerateCode = new System.Windows.Forms.ToolStripButton();
            this.tsGenerateFile = new System.Windows.Forms.ToolStripButton();
            this.tsGenerateMerge = new System.Windows.Forms.ToolStripButton();
            this.panel1 = new System.Windows.Forms.Panel();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.toolStripContainer1 = new System.Windows.Forms.ToolStripContainer();
            this.uiStatusStrip = new System.Windows.Forms.StatusStrip();
            this.leftSandDock.SuspendLayout();
            this.uiNavigatorDock.SuspendLayout();
            this.uiPropertiesDock.SuspendLayout();
            this.uiPropertiesPanel.SuspendLayout();
            this.uiCustomValuesDock.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uiCustomValuesDataGrid)).BeginInit();
            this.rightSandDock.SuspendLayout();
            this.uiTemplateContainer.SuspendLayout();
            this.uiTemplateDock.SuspendLayout();
            this.uiGenerateCodeDock.SuspendLayout();
            this.uiMenuStrip.SuspendLayout();
            this.uiToolStrip.SuspendLayout();
            this.panel1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.toolStripContainer1.BottomToolStripPanel.SuspendLayout();
            this.toolStripContainer1.ContentPanel.SuspendLayout();
            this.toolStripContainer1.TopToolStripPanel.SuspendLayout();
            this.toolStripContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // uiSandDockManager
            // 
            this.uiSandDockManager.DockingManager = TD.SandDock.DockingManager.Whidbey;
            this.uiSandDockManager.OwnerForm = this;
            // 
            // leftSandDock
            // 
            this.leftSandDock.Controls.Add(this.uiNavigatorDock);
            this.leftSandDock.Controls.Add(this.uiSnippets);
            this.leftSandDock.Dock = System.Windows.Forms.DockStyle.Left;
            this.leftSandDock.Guid = new System.Guid("4447f9b6-bb46-4444-9654-653e0fac0c75");
            this.leftSandDock.LayoutSystem = new TD.SandDock.SplitLayoutSystem(250, 400, System.Windows.Forms.Orientation.Horizontal, new TD.SandDock.LayoutSystemBase[] {
            ((TD.SandDock.LayoutSystemBase)(new TD.SandDock.ControlLayoutSystem(196, 513, new TD.SandDock.DockControl[] {
                        this.uiNavigatorDock,
                        this.uiSnippets}, this.uiNavigatorDock)))});
            this.leftSandDock.Location = new System.Drawing.Point(0, 0);
            this.leftSandDock.Manager = this.uiSandDockManager;
            this.leftSandDock.Name = "leftSandDock";
            this.leftSandDock.Size = new System.Drawing.Size(200, 513);
            this.leftSandDock.TabIndex = 2;
            // 
            // uiNavigatorDock
            // 
            this.uiNavigatorDock.Controls.Add(this.uiNavigatorControl);
            this.uiNavigatorDock.Guid = new System.Guid("be35efbb-904d-42cd-ab9a-e897e77040b6");
            this.uiNavigatorDock.Location = new System.Drawing.Point(0, 18);
            this.uiNavigatorDock.Name = "uiNavigatorDock";
            this.uiNavigatorDock.Size = new System.Drawing.Size(196, 472);
            this.uiNavigatorDock.TabIndex = 0;
            this.uiNavigatorDock.Text = "Navigator";
            // 
            // uiNavigatorControl
            // 
            this.uiNavigatorControl.ConnectionString = "";
            this.uiNavigatorControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.uiNavigatorControl.Location = new System.Drawing.Point(0, 0);
            this.uiNavigatorControl.Name = "uiNavigatorControl";
            this.uiNavigatorControl.ProviderType = iCodeGenerator.GenericDataAccess.DataProviderType.MSSQL;
            this.uiNavigatorControl.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.uiNavigatorControl.Size = new System.Drawing.Size(196, 472);
            this.uiNavigatorControl.TabIndex = 0;
            this.uiNavigatorControl.TableSelect += new iCodeGenerator.DatabaseNavigator.NavigatorControl.TableEventHandler(this.uiNavigatorControl_TableSelect);
            this.uiNavigatorControl.TablesSelect += new iCodeGenerator.DatabaseNavigator.NavigatorControl.TablesEventHandler(this.uiNavigatorControl_TablesSelect);
            this.uiNavigatorControl.DatabaseSelect += new iCodeGenerator.DatabaseNavigator.NavigatorControl.DatabaseEventHandler(this.uiNavigatorControl_DatabaseSelect);
            this.uiNavigatorControl.ColumnSelect += new iCodeGenerator.DatabaseNavigator.NavigatorControl.ColumnEventHandler(this.uiNavigatorControl_ColumnSelect);
            this.uiNavigatorControl.ColumnShowProperties += new iCodeGenerator.DatabaseNavigator.NavigatorControl.ColumnEventHandler(this.uiNavigatorControl_ColumnShowProperties);
            // 
            // uiSnippets
            // 
            this.uiSnippets.Guid = new System.Guid("a7b3b4f4-8ba9-4862-9b34-5e2c54880aec");
            this.uiSnippets.Location = new System.Drawing.Point(0, 18);
            this.uiSnippets.Name = "uiSnippets";
            this.uiSnippets.Size = new System.Drawing.Size(196, 472);
            this.uiSnippets.TabIndex = 1;
            this.uiSnippets.Text = "Snippets";
            // 
            // uiPropertiesDock
            // 
            this.uiPropertiesDock.Controls.Add(this.uiPropertiesPanel);
            this.uiPropertiesDock.Guid = new System.Guid("4f16c3df-5375-4159-85ae-522ff2daa5b4");
            this.uiPropertiesDock.Location = new System.Drawing.Point(4, 18);
            this.uiPropertiesDock.Name = "uiPropertiesDock";
            this.uiPropertiesDock.Size = new System.Drawing.Size(196, 213);
            this.uiPropertiesDock.TabIndex = 1;
            this.uiPropertiesDock.Text = "Properties";
            // 
            // uiPropertiesPanel
            // 
            this.uiPropertiesPanel.Controls.Add(this.uiPropertyEditor);
            this.uiPropertiesPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.uiPropertiesPanel.Location = new System.Drawing.Point(0, 0);
            this.uiPropertiesPanel.Name = "uiPropertiesPanel";
            this.uiPropertiesPanel.Size = new System.Drawing.Size(196, 213);
            this.uiPropertiesPanel.TabIndex = 0;
            // 
            // uiPropertyEditor
            // 
            this.uiPropertyEditor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.uiPropertyEditor.HelpVisible = false;
            this.uiPropertyEditor.LineColor = System.Drawing.SystemColors.ScrollBar;
            this.uiPropertyEditor.Location = new System.Drawing.Point(0, 0);
            this.uiPropertyEditor.Name = "uiPropertyEditor";
            this.uiPropertyEditor.Size = new System.Drawing.Size(196, 213);
            this.uiPropertyEditor.TabIndex = 0;
            this.uiPropertyEditor.ToolbarVisible = false;
            // 
            // uiCustomValuesDock
            // 
            this.uiCustomValuesDock.Controls.Add(this.uiCustomValuesDataGrid);
            this.uiCustomValuesDock.Guid = new System.Guid("efe8850a-621c-48c4-a603-1a963b397082");
            this.uiCustomValuesDock.Location = new System.Drawing.Point(4, 276);
            this.uiCustomValuesDock.Name = "uiCustomValuesDock";
            this.uiCustomValuesDock.Size = new System.Drawing.Size(196, 214);
            this.uiCustomValuesDock.TabIndex = 2;
            this.uiCustomValuesDock.Text = "Custom Values";
            this.uiCustomValuesDock.Leave += new System.EventHandler(this.uiCustomValuesDock_Leave);
            // 
            // uiCustomValuesDataGrid
            // 
            this.uiCustomValuesDataGrid.CaptionVisible = false;
            this.uiCustomValuesDataGrid.DataMember = "";
            this.uiCustomValuesDataGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.uiCustomValuesDataGrid.HeaderForeColor = System.Drawing.SystemColors.ControlText;
            this.uiCustomValuesDataGrid.Location = new System.Drawing.Point(0, 0);
            this.uiCustomValuesDataGrid.Name = "uiCustomValuesDataGrid";
            this.uiCustomValuesDataGrid.PreferredColumnWidth = 125;
            this.uiCustomValuesDataGrid.RowHeadersVisible = false;
            this.uiCustomValuesDataGrid.Size = new System.Drawing.Size(196, 214);
            this.uiCustomValuesDataGrid.TabIndex = 0;
            this.uiCustomValuesDataGrid.TableStyles.AddRange(new System.Windows.Forms.DataGridTableStyle[] {
            this.uiCustomValuesGridStyle});
            // 
            // uiCustomValuesGridStyle
            // 
            this.uiCustomValuesGridStyle.DataGrid = this.uiCustomValuesDataGrid;
            this.uiCustomValuesGridStyle.GridColumnStyles.AddRange(new System.Windows.Forms.DataGridColumnStyle[] {
            this.uiName,
            this.uiValue});
            this.uiCustomValuesGridStyle.HeaderForeColor = System.Drawing.SystemColors.ControlText;
            // 
            // uiName
            // 
            this.uiName.Format = "";
            this.uiName.FormatInfo = null;
            this.uiName.HeaderText = "Tag";
            this.uiName.MappingName = "Name";
            this.uiName.Width = 50;
            // 
            // uiValue
            // 
            this.uiValue.Format = "";
            this.uiValue.FormatInfo = null;
            this.uiValue.HeaderText = "Value";
            this.uiValue.MappingName = "Value";
            this.uiValue.Width = 75;
            // 
            // rightSandDock
            // 
            this.rightSandDock.Controls.Add(this.uiPropertiesDock);
            this.rightSandDock.Controls.Add(this.uiCustomValuesDock);
            this.rightSandDock.Dock = System.Windows.Forms.DockStyle.Right;
            this.rightSandDock.Guid = new System.Guid("5a7c170b-4aad-4827-bf24-de523a3bac08");
            this.rightSandDock.LayoutSystem = new TD.SandDock.SplitLayoutSystem(250, 400, System.Windows.Forms.Orientation.Horizontal, new TD.SandDock.LayoutSystemBase[] {
            ((TD.SandDock.LayoutSystemBase)(new TD.SandDock.ControlLayoutSystem(196, 254, new TD.SandDock.DockControl[] {
                        this.uiPropertiesDock}, this.uiPropertiesDock))),
            ((TD.SandDock.LayoutSystemBase)(new TD.SandDock.ControlLayoutSystem(196, 254, new TD.SandDock.DockControl[] {
                        this.uiCustomValuesDock}, this.uiCustomValuesDock)))});
            this.rightSandDock.Location = new System.Drawing.Point(586, 0);
            this.rightSandDock.Manager = this.uiSandDockManager;
            this.rightSandDock.Name = "rightSandDock";
            this.rightSandDock.Size = new System.Drawing.Size(200, 513);
            this.rightSandDock.TabIndex = 3;
            // 
            // bottomSandDock
            // 
            this.bottomSandDock.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.bottomSandDock.Guid = new System.Guid("864e047d-2a9d-4fba-bc18-f83754ae5214");
            this.bottomSandDock.LayoutSystem = new TD.SandDock.SplitLayoutSystem(250, 400);
            this.bottomSandDock.Location = new System.Drawing.Point(0, 513);
            this.bottomSandDock.Manager = this.uiSandDockManager;
            this.bottomSandDock.Name = "bottomSandDock";
            this.bottomSandDock.Size = new System.Drawing.Size(786, 0);
            this.bottomSandDock.TabIndex = 4;
            // 
            // topSandDock
            // 
            this.topSandDock.Dock = System.Windows.Forms.DockStyle.Top;
            this.topSandDock.Guid = new System.Guid("24d50ee0-b600-4b52-a431-7e9715efbf6d");
            this.topSandDock.LayoutSystem = new TD.SandDock.SplitLayoutSystem(250, 400);
            this.topSandDock.Location = new System.Drawing.Point(0, 0);
            this.topSandDock.Manager = this.uiSandDockManager;
            this.topSandDock.Name = "topSandDock";
            this.topSandDock.Size = new System.Drawing.Size(786, 0);
            this.topSandDock.TabIndex = 5;
            // 
            // uiTemplateContainer
            // 
            this.uiTemplateContainer.Controls.Add(this.uiTemplateDock);
            this.uiTemplateContainer.Controls.Add(this.uiGenerateCodeDock);
            this.uiTemplateContainer.Cursor = System.Windows.Forms.Cursors.Default;
            this.uiTemplateContainer.Guid = new System.Guid("e2d7d100-338b-414b-a8f5-696d61a3348a");
            this.uiTemplateContainer.LayoutSystem = new TD.SandDock.SplitLayoutSystem(250, 400, System.Windows.Forms.Orientation.Horizontal, new TD.SandDock.LayoutSystemBase[] {
            ((TD.SandDock.LayoutSystemBase)(new TD.SandDock.DocumentLayoutSystem(384, 511, new TD.SandDock.DockControl[] {
                        this.uiTemplateDock,
                        this.uiGenerateCodeDock}, this.uiTemplateDock)))});
            this.uiTemplateContainer.Location = new System.Drawing.Point(200, 0);
            this.uiTemplateContainer.Manager = null;
            this.uiTemplateContainer.Name = "uiTemplateContainer";
            this.uiTemplateContainer.Size = new System.Drawing.Size(386, 513);
            this.uiTemplateContainer.TabIndex = 10;
            // 
            // uiTemplateDock
            // 
            this.uiTemplateDock.Closable = false;
            this.uiTemplateDock.Controls.Add(this.uiTemplateTextBox);
            this.uiTemplateDock.Guid = new System.Guid("6065db1e-e6bf-4209-8cb9-03c7805d045a");
            this.uiTemplateDock.Location = new System.Drawing.Point(3, 23);
            this.uiTemplateDock.Name = "uiTemplateDock";
            this.uiTemplateDock.Size = new System.Drawing.Size(380, 487);
            this.uiTemplateDock.TabIndex = 0;
            this.uiTemplateDock.Text = "Template";
            // 
            // uiTemplateTextBox
            // 
            this.uiTemplateTextBox.AcceptsTab = true;
            this.uiTemplateTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.uiTemplateTextBox.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.uiTemplateTextBox.Location = new System.Drawing.Point(0, 0);
            this.uiTemplateTextBox.Name = "uiTemplateTextBox";
            this.uiTemplateTextBox.Size = new System.Drawing.Size(380, 487);
            this.uiTemplateTextBox.TabIndex = 0;
            this.uiTemplateTextBox.Text = "";
            // 
            // uiGenerateCodeDock
            // 
            this.uiGenerateCodeDock.Closable = false;
            this.uiGenerateCodeDock.Controls.Add(this.uiGeneratedCodeTextBox);
            this.uiGenerateCodeDock.Guid = new System.Guid("26ae2cc4-bc92-4191-b399-333e1ca9da35");
            this.uiGenerateCodeDock.Location = new System.Drawing.Point(3, 23);
            this.uiGenerateCodeDock.Name = "uiGenerateCodeDock";
            this.uiGenerateCodeDock.Size = new System.Drawing.Size(380, 463);
            this.uiGenerateCodeDock.TabIndex = 1;
            this.uiGenerateCodeDock.Text = "Generated Code";
            this.uiGenerateCodeDock.Enter += new System.EventHandler(this.uiGenerateCodeButton_Activate);
            // 
            // uiGeneratedCodeTextBox
            // 
            this.uiGeneratedCodeTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.uiGeneratedCodeTextBox.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.uiGeneratedCodeTextBox.Location = new System.Drawing.Point(0, 0);
            this.uiGeneratedCodeTextBox.Name = "uiGeneratedCodeTextBox";
            this.uiGeneratedCodeTextBox.ReadOnly = true;
            this.uiGeneratedCodeTextBox.Size = new System.Drawing.Size(380, 463);
            this.uiGeneratedCodeTextBox.TabIndex = 0;
            this.uiGeneratedCodeTextBox.Text = "";
            // 
            // uiOpenTemplateDialog
            // 
            this.uiOpenTemplateDialog.Filter = "All Files|*.*|Text Files|*.txt";
            this.uiOpenTemplateDialog.Title = "Open Template";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.connectToolStripMenuItem,
            this.disconnectToolStripMenuItem,
            this.configureCOnnectionToolStripMenuItem,
            this.openTemplateToolStripMenuItem,
            this.saveTemplateToolStripMenuItem,
            this.saveAsTemplateToolStripMenuItem,
            this.saveResultToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // connectToolStripMenuItem
            // 
            this.connectToolStripMenuItem.Name = "connectToolStripMenuItem";
            this.connectToolStripMenuItem.Size = new System.Drawing.Size(192, 22);
            this.connectToolStripMenuItem.Text = "Connect";
            this.connectToolStripMenuItem.Click += new System.EventHandler(this.uiConnect_Activate);
            // 
            // disconnectToolStripMenuItem
            // 
            this.disconnectToolStripMenuItem.Name = "disconnectToolStripMenuItem";
            this.disconnectToolStripMenuItem.Size = new System.Drawing.Size(192, 22);
            this.disconnectToolStripMenuItem.Text = "Disconnect";
            this.disconnectToolStripMenuItem.Click += new System.EventHandler(this.uiDisconnect_Activate);
            // 
            // configureCOnnectionToolStripMenuItem
            // 
            this.configureCOnnectionToolStripMenuItem.Name = "configureCOnnectionToolStripMenuItem";
            this.configureCOnnectionToolStripMenuItem.Size = new System.Drawing.Size(192, 22);
            this.configureCOnnectionToolStripMenuItem.Text = "Configure Connection";
            this.configureCOnnectionToolStripMenuItem.Click += new System.EventHandler(this.uiOpenConnectionString_Activate);
            // 
            // openTemplateToolStripMenuItem
            // 
            this.openTemplateToolStripMenuItem.Name = "openTemplateToolStripMenuItem";
            this.openTemplateToolStripMenuItem.Size = new System.Drawing.Size(192, 22);
            this.openTemplateToolStripMenuItem.Text = "Open Template";
            this.openTemplateToolStripMenuItem.Click += new System.EventHandler(this.uiOpenTemplateButton_Activate);
            // 
            // saveTemplateToolStripMenuItem
            // 
            this.saveTemplateToolStripMenuItem.Enabled = false;
            this.saveTemplateToolStripMenuItem.Name = "saveTemplateToolStripMenuItem";
            this.saveTemplateToolStripMenuItem.Size = new System.Drawing.Size(192, 22);
            this.saveTemplateToolStripMenuItem.Text = "Save Template";
            this.saveTemplateToolStripMenuItem.Click += new System.EventHandler(this.uiSaveTemplateButton_Activate);
            // 
            // saveAsTemplateToolStripMenuItem
            // 
            this.saveAsTemplateToolStripMenuItem.Name = "saveAsTemplateToolStripMenuItem";
            this.saveAsTemplateToolStripMenuItem.Size = new System.Drawing.Size(192, 22);
            this.saveAsTemplateToolStripMenuItem.Text = "Save As Template";
            this.saveAsTemplateToolStripMenuItem.Click += new System.EventHandler(this.uiSaveAsTemplateButton_Activate);
            // 
            // saveResultToolStripMenuItem
            // 
            this.saveResultToolStripMenuItem.Name = "saveResultToolStripMenuItem";
            this.saveResultToolStripMenuItem.Size = new System.Drawing.Size(192, 22);
            this.saveResultToolStripMenuItem.Text = "Save Result";
            this.saveResultToolStripMenuItem.Click += new System.EventHandler(this.uiSaveResultButton_Activate);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(192, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.uiExitMenuButton_Activate);
            // 
            // editToolStripMenuItem
            // 
            this.editToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.uNdoToolStripMenuItem,
            this.redoToolStripMenuItem,
            this.cutToolStripMenuItem,
            this.copyToolStripMenuItem,
            this.pasteToolStripMenuItem});
            this.editToolStripMenuItem.Name = "editToolStripMenuItem";
            this.editToolStripMenuItem.Size = new System.Drawing.Size(39, 20);
            this.editToolStripMenuItem.Text = "Edit";
            // 
            // uNdoToolStripMenuItem
            // 
            this.uNdoToolStripMenuItem.Name = "uNdoToolStripMenuItem";
            this.uNdoToolStripMenuItem.Size = new System.Drawing.Size(103, 22);
            this.uNdoToolStripMenuItem.Text = "Undo";
            this.uNdoToolStripMenuItem.Click += new System.EventHandler(this.uiUndoButton_Activate);
            // 
            // redoToolStripMenuItem
            // 
            this.redoToolStripMenuItem.Name = "redoToolStripMenuItem";
            this.redoToolStripMenuItem.Size = new System.Drawing.Size(103, 22);
            this.redoToolStripMenuItem.Text = "Redo";
            this.redoToolStripMenuItem.Click += new System.EventHandler(this.uiRedoButton_Activate);
            // 
            // cutToolStripMenuItem
            // 
            this.cutToolStripMenuItem.Name = "cutToolStripMenuItem";
            this.cutToolStripMenuItem.Size = new System.Drawing.Size(103, 22);
            this.cutToolStripMenuItem.Text = "Cut";
            this.cutToolStripMenuItem.Click += new System.EventHandler(this.uiCutButton_Activate);
            // 
            // copyToolStripMenuItem
            // 
            this.copyToolStripMenuItem.Name = "copyToolStripMenuItem";
            this.copyToolStripMenuItem.Size = new System.Drawing.Size(103, 22);
            this.copyToolStripMenuItem.Text = "Copy";
            this.copyToolStripMenuItem.Click += new System.EventHandler(this.uiCopyButton_Activate);
            // 
            // pasteToolStripMenuItem
            // 
            this.pasteToolStripMenuItem.Name = "pasteToolStripMenuItem";
            this.pasteToolStripMenuItem.Size = new System.Drawing.Size(103, 22);
            this.pasteToolStripMenuItem.Text = "Paste";
            this.pasteToolStripMenuItem.Click += new System.EventHandler(this.uiPasteButton_Activate);
            // 
            // viewToolStripMenuItem
            // 
            this.viewToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.navigatorToolStripMenuItem,
            this.propertiesToolStripMenuItem,
            this.customValuesToolStripMenuItem});
            this.viewToolStripMenuItem.Name = "viewToolStripMenuItem";
            this.viewToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.viewToolStripMenuItem.Text = "View";
            // 
            // navigatorToolStripMenuItem
            // 
            this.navigatorToolStripMenuItem.Name = "navigatorToolStripMenuItem";
            this.navigatorToolStripMenuItem.Size = new System.Drawing.Size(153, 22);
            this.navigatorToolStripMenuItem.Text = "Navigator";
            this.navigatorToolStripMenuItem.Click += new System.EventHandler(this.uiViewNavigatorButton_Activate);
            // 
            // propertiesToolStripMenuItem
            // 
            this.propertiesToolStripMenuItem.Name = "propertiesToolStripMenuItem";
            this.propertiesToolStripMenuItem.Size = new System.Drawing.Size(153, 22);
            this.propertiesToolStripMenuItem.Text = "Properties";
            this.propertiesToolStripMenuItem.Click += new System.EventHandler(this.uiViewPropertiesButton_Activate);
            // 
            // customValuesToolStripMenuItem
            // 
            this.customValuesToolStripMenuItem.Name = "customValuesToolStripMenuItem";
            this.customValuesToolStripMenuItem.Size = new System.Drawing.Size(153, 22);
            this.customValuesToolStripMenuItem.Text = "Custom Values";
            this.customValuesToolStripMenuItem.Click += new System.EventHandler(this.uiViewCustomValuesButton_Activate);
            // 
            // generatorToolStripMenuItem
            // 
            this.generatorToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.generateCodeToolStripMenuItem,
            this.fileGenerateToolStripMenuItem,
            this.fileGeneratorConfigurationToolStripMenuItem,
            this.openOutputFolderToolStripMenuItem});
            this.generatorToolStripMenuItem.Name = "generatorToolStripMenuItem";
            this.generatorToolStripMenuItem.Size = new System.Drawing.Size(71, 20);
            this.generatorToolStripMenuItem.Text = "Generator";
            // 
            // generateCodeToolStripMenuItem
            // 
            this.generateCodeToolStripMenuItem.Name = "generateCodeToolStripMenuItem";
            this.generateCodeToolStripMenuItem.Size = new System.Drawing.Size(224, 22);
            this.generateCodeToolStripMenuItem.Text = "(Generate Code)";
            this.generateCodeToolStripMenuItem.Click += new System.EventHandler(this.uiGenerateCodeButton_Activate);
            // 
            // fileGeneratorConfigurationToolStripMenuItem
            // 
            this.fileGeneratorConfigurationToolStripMenuItem.Name = "fileGeneratorConfigurationToolStripMenuItem";
            this.fileGeneratorConfigurationToolStripMenuItem.Size = new System.Drawing.Size(224, 22);
            this.fileGeneratorConfigurationToolStripMenuItem.Text = "File Generator Configuration";
            this.fileGeneratorConfigurationToolStripMenuItem.Click += new System.EventHandler(this.uiFileGeneratorConfigButton_Activate);
            // 
            // fileGenerateToolStripMenuItem
            // 
            this.fileGenerateToolStripMenuItem.Name = "fileGenerateToolStripMenuItem";
            this.fileGenerateToolStripMenuItem.Size = new System.Drawing.Size(224, 22);
            this.fileGenerateToolStripMenuItem.Text = "File Generate";
            this.fileGenerateToolStripMenuItem.Click += new System.EventHandler(this.uiFileGenerateButton_Activate);
            // 
            // openOutputFolderToolStripMenuItem
            // 
            this.openOutputFolderToolStripMenuItem.Name = "openOutputFolderToolStripMenuItem";
            this.openOutputFolderToolStripMenuItem.Size = new System.Drawing.Size(224, 22);
            this.openOutputFolderToolStripMenuItem.Text = "Open Output Folder";
            this.openOutputFolderToolStripMenuItem.Click += new System.EventHandler(this.uiOpenOutputFolder_Activate);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aboutToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.helpToolStripMenuItem.Text = "Help";
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.aboutToolStripMenuItem.Text = "About";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.uiAboutButton_Activate);
            // 
            // uiMenuStrip
            // 
            this.uiMenuStrip.Dock = System.Windows.Forms.DockStyle.None;
            this.uiMenuStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Visible;
            this.uiMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.editToolStripMenuItem,
            this.viewToolStripMenuItem,
            this.generatorToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.uiMenuStrip.Location = new System.Drawing.Point(3, 0);
            this.uiMenuStrip.Name = "uiMenuStrip";
            this.uiMenuStrip.Size = new System.Drawing.Size(247, 24);
            this.uiMenuStrip.Stretch = false;
            this.uiMenuStrip.TabIndex = 1;
            this.uiMenuStrip.Text = "menuStrip1";
            // 
            // uiToolStrip
            // 
            this.uiToolStrip.Dock = System.Windows.Forms.DockStyle.None;
            this.uiToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsGenerateCode,
            this.tsGenerateFile,
            this.tsGenerateMerge});
            this.uiToolStrip.Location = new System.Drawing.Point(250, 0);
            this.uiToolStrip.Name = "uiToolStrip";
            this.uiToolStrip.Size = new System.Drawing.Size(233, 25);
            this.uiToolStrip.TabIndex = 11;
            this.uiToolStrip.Text = "toolStrip1";
            // 
            // tsGenerateCode
            // 
            this.tsGenerateCode.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsGenerateCode.Image = ((System.Drawing.Image)(resources.GetObject("tsGenerateCode.Image")));
            this.tsGenerateCode.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsGenerateCode.Name = "tsGenerateCode";
            this.tsGenerateCode.Size = new System.Drawing.Size(66, 22);
            this.tsGenerateCode.Text = "(Generate)";
            this.tsGenerateCode.Click += new System.EventHandler(this.uiGenerateButton_Activate);
            // 
            // tsGenerateFile
            // 
            this.tsGenerateFile.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsGenerateFile.Image = ((System.Drawing.Image)(resources.GetObject("tsGenerateFile.Image")));
            this.tsGenerateFile.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsGenerateFile.Name = "tsGenerateFile";
            this.tsGenerateFile.Size = new System.Drawing.Size(79, 22);
            this.tsGenerateFile.Text = "File Generate";
            this.tsGenerateFile.Click += new System.EventHandler(this.uiFileGenerator_Activate);
            // 
            // tsGenerateMerge
            // 
            this.tsGenerateMerge.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsGenerateMerge.Image = ((System.Drawing.Image)(resources.GetObject("tsGenerateMerge.Image")));
            this.tsGenerateMerge.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsGenerateMerge.Name = "tsGenerateMerge";
            this.tsGenerateMerge.Size = new System.Drawing.Size(76, 22);
            this.tsGenerateMerge.Text = "Merge Code";
            this.tsGenerateMerge.Click += new System.EventHandler(this.uiMergeButton_Activate);
            // 
            // panel1
            // 
            this.panel1.AutoSize = true;
            this.panel1.Controls.Add(this.uiTemplateContainer);
            this.panel1.Controls.Add(this.leftSandDock);
            this.panel1.Controls.Add(this.rightSandDock);
            this.panel1.Controls.Add(this.topSandDock);
            this.panel1.Controls.Add(this.bottomSandDock);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(3, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(786, 513);
            this.panel1.TabIndex = 12;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.AutoSize = true;
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.panel1, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(792, 519);
            this.tableLayoutPanel1.TabIndex = 1;
            // 
            // toolStripContainer1
            // 
            // 
            // toolStripContainer1.BottomToolStripPanel
            // 
            this.toolStripContainer1.BottomToolStripPanel.Controls.Add(this.uiStatusStrip);
            // 
            // toolStripContainer1.ContentPanel
            // 
            this.toolStripContainer1.ContentPanel.Controls.Add(this.tableLayoutPanel1);
            this.toolStripContainer1.ContentPanel.Size = new System.Drawing.Size(792, 519);
            this.toolStripContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.toolStripContainer1.Location = new System.Drawing.Point(0, 0);
            this.toolStripContainer1.Name = "toolStripContainer1";
            this.toolStripContainer1.Size = new System.Drawing.Size(792, 566);
            this.toolStripContainer1.TabIndex = 12;
            this.toolStripContainer1.Text = "toolStripContainer1";
            // 
            // toolStripContainer1.TopToolStripPanel
            // 
            this.toolStripContainer1.TopToolStripPanel.Controls.Add(this.uiMenuStrip);
            this.toolStripContainer1.TopToolStripPanel.Controls.Add(this.uiToolStrip);
            // 
            // uiStatusStrip
            // 
            this.uiStatusStrip.Dock = System.Windows.Forms.DockStyle.None;
            this.uiStatusStrip.Location = new System.Drawing.Point(0, 0);
            this.uiStatusStrip.Name = "uiStatusStrip";
            this.uiStatusStrip.Size = new System.Drawing.Size(792, 22);
            this.uiStatusStrip.TabIndex = 1;
            this.uiStatusStrip.Text = "statusStrip1";
            // 
            // MainWindow
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(792, 566);
            this.Controls.Add(this.toolStripContainer1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.uiMenuStrip;
            this.Name = "MainWindow";
            this.Text = "iCode+Generator (MaKo | LGPL-licensed)";
            this.leftSandDock.ResumeLayout(false);
            this.uiNavigatorDock.ResumeLayout(false);
            this.uiPropertiesDock.ResumeLayout(false);
            this.uiPropertiesPanel.ResumeLayout(false);
            this.uiCustomValuesDock.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.uiCustomValuesDataGrid)).EndInit();
            this.rightSandDock.ResumeLayout(false);
            this.uiTemplateContainer.ResumeLayout(false);
            this.uiTemplateDock.ResumeLayout(false);
            this.uiGenerateCodeDock.ResumeLayout(false);
            this.uiMenuStrip.ResumeLayout(false);
            this.uiMenuStrip.PerformLayout();
            this.uiToolStrip.ResumeLayout(false);
            this.uiToolStrip.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.toolStripContainer1.BottomToolStripPanel.ResumeLayout(false);
            this.toolStripContainer1.BottomToolStripPanel.PerformLayout();
            this.toolStripContainer1.ContentPanel.ResumeLayout(false);
            this.toolStripContainer1.ContentPanel.PerformLayout();
            this.toolStripContainer1.TopToolStripPanel.ResumeLayout(false);
            this.toolStripContainer1.TopToolStripPanel.PerformLayout();
            this.toolStripContainer1.ResumeLayout(false);
            this.toolStripContainer1.PerformLayout();
            this.ResumeLayout(false);

		}

		#endregion

		private void uiExitMenuButton_Activate(object sender, EventArgs e)
		{
			Application.Exit();
		}

		private void uiNavigatorControl_DatabaseSelect(object sender, DatabaseEventArgs args)
		{
			uiStatusStrip.Text = args.Database.Name;
			uiPropertyEditor.SelectedObject = args.Database;
		}

		private void uiNavigatorControl_TableSelect(object sender, TableEventArgs args)
		{
			selectedTable = args.Table;
            uiStatusStrip.Text = args.Table.ParentDatabase.Name + " > " +
			                   args.Table.Name;
			uiPropertyEditor.SelectedObject = args.Table;
		}

        private void uiNavigatorControl_TablesSelect(object sender, TablesEventArgs args)
        {
            selectedTables = args.Tables;

            uiStatusStrip.Text = "";

            foreach (Table table in selectedTables)
            {
                uiStatusStrip.Text += table.ParentDatabase.Name + " > " +
                                   table.Name + " & ";
            }


            //uiPropertyEditor.SelectedObject = args.Table;
            uiPropertyEditor.SelectedObjects = args.Tables;
        }

		private void uiNavigatorControl_ColumnSelect(object sender, ColumnEventArgs args)
		{
            uiStatusStrip.Text = args.Column.ParentTable.ParentDatabase.Name + " > " +
			                   args.Column.ParentTable.Name + " > " +
			                   args.Column.Name;
			uiPropertyEditor.SelectedObject = args.Column;
		}

		private void uiGenerateButton_Activate(object sender, EventArgs e)
		{
			GenerateCode();
		}

		private void GenerateCode()
		{
			try
			{
				if (selectedTable == null) return;
				Client cgenerator = new Client();
				cgenerator.CustomValues = GetCustomValues();
				uiGeneratedCodeTextBox.Text = cgenerator.Parse(selectedTable, uiTemplateTextBox.Text);
			}
			catch (DataTypeManagerException ex)
			{
				MessageBox.Show(this, ex.Message, ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Warning);
			}
		}

		private IDictionary GetCustomValues()
		{
			IDictionary customValues = new Hashtable();
			foreach (DataRow row in _CustomValues.Tables[0].Rows)
			{
				customValues[row["Name"].ToString().ToUpper()] = row["Value"].ToString();
			}
			return customValues;
		}

		private void uiNavigatorControl_ColumnShowProperties(object sender, ColumnEventArgs args)
		{
			uiPropertiesDock.Open();
		}

		#region Copy, Paste & Methods

		[DllImport("user32.dll", CharSet=CharSet.Auto, CallingConvention=CallingConvention.Winapi)]
		private static extern IntPtr GetFocus();

		[DllImport("user32.dll", CharSet=CharSet.Auto)]
		private static extern IntPtr SendMessage(HandleRef hWnd, int msg, int wParam, int lParam);

		private const int WM_CUT = 0x300;
		private const int WM_COPY = 0x301;
		private const int WM_PASTE = 0x302;
		private const int WM_UNDO = 0x304;


		private void uiCutButton_Activate(object sender, EventArgs e)
		{
			Control focusedControl = GetFocusedControl();
			if (focusedControl is TextBoxBase)
				((TextBoxBase) focusedControl).Cut();
				//			else if (focusedControl is RichTextBox)
				//			{
				//				// Special application processing for this event
				//			}
			else
				SendMessage(new HandleRef(this, GetFocus()), WM_CUT, 0, 0);
		}

		private void uiCopyButton_Activate(object sender, EventArgs e)
		{
			Control focusedControl = GetFocusedControl();
			if (focusedControl is TextBoxBase)
				((TextBoxBase) focusedControl).Copy();
				//			else if (focusedControl is RichTextBox)
				//			{
				//				// Special application processing for this event
				//			}
			else
				SendMessage(new HandleRef(this, GetFocus()), WM_COPY, 0, 0);
		}

		private void uiPasteButton_Activate(object sender, EventArgs e)
		{
			Control focusedControl = GetFocusedControl();
			if (focusedControl is TextBoxBase)
				((TextBoxBase) focusedControl).Paste();
				//			else if (focusedControl is RichTextBox)
				//			{
				//				// Special application processing for this event
				//			}
			else
				SendMessage(new HandleRef(this, GetFocus()), WM_PASTE, 0, 0);
		}

		private void uiUndoButton_Activate(object sender, EventArgs e)
		{
			Control focusedControl = GetFocusedControl();
			if (focusedControl is TextBoxBase)
				((TextBoxBase) focusedControl).Undo();
				//			else if (focusedControl is RichTextBox)
				//			{
				//				// Special application processing for this event
				//			}
			else
				SendMessage(new HandleRef(this, GetFocus()), WM_UNDO, 0, 0);
		}

		private Control GetFocusedControl()
		{
			// Try and find the .net control instance with the focus
			IntPtr focus = GetFocus();
			if (focus == IntPtr.Zero)
				return null;
			else
				return FromHandle(focus);
		}

		private void uiRedoButton_Activate(object sender, EventArgs e)
		{
			Control focusedControl = GetFocusedControl();
			if (focusedControl is RichTextBox)
				((RichTextBox) focusedControl).Redo();
				//			else if (focusedControl is RichTextBox)
				//			{
				//				// Special application processing for this event
				//			}
			else
				SendMessage(new HandleRef(this, GetFocus()), WM_UNDO, 0, 0); // 
		}

		#endregion

		#region Methods

		private void uiViewNavigatorButton_Activate(object sender, EventArgs e)
		{
			if (uiNavigatorDock.IsOpen)
			{
				uiNavigatorDock.Close();
			}
			else
			{
				uiNavigatorDock.Open();
			}
		}

		private void uiViewPropertiesButton_Activate(object sender, EventArgs e)
		{
			if (uiPropertiesDock.IsOpen)
			{
				uiPropertiesDock.Close();
			}
			else
			{
				uiPropertiesDock.Open();
			}
		}

		private void uiViewCustomValuesButton_Activate(object sender, EventArgs e)
		{
			if (uiCustomValuesDock.IsOpen)
			{
				uiCustomValuesDock.Close();
			}
			else
			{
				uiCustomValuesDock.Open();
			}
		}

		private void uiAboutButton_Activate(object sender, EventArgs e)
		{
			new AboutWindow().ShowDialog();
		}

		private void uiOpenTemplateButton_Activate(object sender, EventArgs e)
		{
			if (uiOpenTemplateDialog.ShowDialog() == DialogResult.OK)
			{
				_TemplateFilename = uiOpenTemplateDialog.FileName;
				uiTemplateTextBox.LoadFile(_TemplateFilename, RichTextBoxStreamType.PlainText);

                saveTemplateToolStripMenuItem.Enabled = true;
			}
		}

		private void uiSaveTemplateButton_Activate(object sender, EventArgs e)
		{
			uiTemplateTextBox.SaveFile(_TemplateFilename, RichTextBoxStreamType.PlainText);
		}

		private void uiSaveAsTemplateButton_Activate(object sender, EventArgs e)
		{
			if (uiSaveDialog.ShowDialog() == DialogResult.OK)
			{
				uiTemplateTextBox.SaveFile(uiSaveDialog.FileName, RichTextBoxStreamType.PlainText);
			}
		}

		private void uiSaveResultButton_Activate(object sender, EventArgs e)
		{
			if (uiSaveDialog.ShowDialog() == DialogResult.OK)
			{
				uiGeneratedCodeTextBox.SaveFile(uiSaveDialog.FileName, RichTextBoxStreamType.PlainText);
			}
		}

		private void uiGenerateCodeButton_Activate(object sender, EventArgs e)
		{
			GenerateCode();
		}

		private void uiMergeButton_Activate(object sender, EventArgs e)
		{
			if (uiOpenMergeDialog.ShowDialog() == DialogResult.OK)
			{
				_TempFilename = CreateTempFile(uiGeneratedCodeTextBox.Text);
				try
				{
					RunMergeProcess("WinMerge.exe", _TempFilename, uiOpenMergeDialog.FileName);
				}
				catch (Win32Exception ex)
				{
					Debug.WriteLine(ex);
					MessageBox.Show(this,
					                "Merge unavailable.\nPlease install WinMerge to have this option or make sure it is in your PATH.\nhttp://winmerge.sourceforge.net/",
					                "Merge unavailable.", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				}
				uiGeneratedCodeTextBox.LoadFile(_TempFilename, RichTextBoxStreamType.PlainText);
				DeleteFile(ref _TempFilename);
			}
		}

		private static void RunMergeProcess(string mergeExec, string tempFilename, string oldFilename)
		{
			Process process = new Process();
			process.StartInfo.FileName = mergeExec;
			process.StartInfo.Arguments = "-e \"" + tempFilename + "\"" + " " + "\"" + oldFilename + "\"";
			process.Start();
			process.WaitForExit();
		}

		private static void DeleteFile(ref string filename)
		{
			File.Delete(filename);
			filename = String.Empty;
		}

		private string CreateTempFile(string textContent)
		{
			string filename = Path.GetTempFileName();
			FileInfo tempFileInfo = new FileInfo(filename);
			tempFileInfo.Attributes = FileAttributes.Temporary;
			StreamWriter writer = File.AppendText(filename);
			writer.Write(textContent);
			writer.Flush();
			writer.Close();
			return filename;
		}

		private void uiCustomValuesDock_Leave(object sender, EventArgs e)
		{
			FilterEmptyCustomValues();
			SaveCustomValues();
		}

		private void SaveCustomValues()
		{
			_CustomValues.WriteXml(_CustomValuesFilename);
		}

		private void FilterEmptyCustomValues()
		{
			DataRow[] rows = _CustomValues.Tables[0].Select();
			for (int i = 0; i < rows.Length; i++)
			{
				if (rows[i][0].ToString().Trim().Length == 0)
				{
					rows[i].Delete();
				}
				else
				{
					rows[i][0] = rows[i][0].ToString().ToUpper().Trim();
				}
			}
			uiCustomValuesDataGrid.Refresh();
		}

		private void uiFileGenerator_Activate(object sender, EventArgs e)
		{
			GenerateFiles();
		}

		private void uiFileGeneratorConfigButton_Activate(object sender, EventArgs e)
		{
			SelectDirectories();
		}

		private DialogResult SelectDirectories()
		{
			DirectorySelectionWindow selectionWindow = new DirectorySelectionWindow();
			return selectionWindow.ShowDialog(this);
		}

		private void uiFileGenerateButton_Activate(object sender, EventArgs e)
		{
			GenerateFiles();
		}

		private void GenerateFiles()
		{
            while ((IsValidFolder(_InputTemplateFolder) && IsValidFolder(_OutputTemplateFolder)) == false)
            {
                if (SelectDirectories() != DialogResult.OK)
                {
                    break;
                }
            }

			if (IsValidFolder(_InputTemplateFolder) && IsValidFolder(_OutputTemplateFolder))
			{
				try
				{
					FileGenerator generator = new FileGenerator();
					generator.OnComplete += new EventHandler(fileGenerator_Completed);
					generator.CustomValue = GetCustomValues();
                    //generator.Generate(selectedTable, _InputTemplateFolder, _OutputTemplateFolder);
                    generator.Generate(selectedTables, _InputTemplateFolder, _OutputTemplateFolder);
				}
				catch (Exception e)
				{
					MessageBox.Show(e.Message);
				}
			}

		}

		private void fileGenerator_Completed(object sender, EventArgs e)
		{
            uiStatusStrip.Text = "File Generation Completed";
            MessageBox.Show("File Generation Completed");
		}

		private void uiOpenOutputFolder_Activate(object sender, EventArgs e)
		{
			if (IsValidFolder(_OutputTemplateFolder))
			{
				Process.Start(_OutputTemplateFolder);
			}
			else
			{
				SelectDirectories();
			}
		}

		private bool IsValidFolder(string folder)
		{
			return folder.Length > 0 && Directory.Exists(folder);
		}

		#endregion

		private void uiOpenConnectionString_Activate(object sender, EventArgs e)
		{
			uiNavigatorControl.ShowEditConnectionStringDialog();
		}

		private void uiConnect_Activate(object sender, EventArgs e)
		{
			uiNavigatorControl.Connect();
		}

		private void uiDisconnect_Activate(object sender, EventArgs e)
		{
			uiNavigatorControl.Disconnect();
		}
	}
}