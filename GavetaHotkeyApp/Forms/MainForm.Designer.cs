namespace GavetaHotkeyApp.Forms;

partial class MainForm
{
    private System.ComponentModel.IContainer components = null;

    // Controles da Impressora
    private GroupBox grpPrinter;
    private Label lblPrinterTitle;
    private ComboBox cmbPrinter;
    private Button btnRefreshPrinters;
    private Label lblPrinterStatus;

    // Controles de Hotkeys
    private GroupBox grpHotkeys;
    private ListBox lstHotkeys;
    private Button btnAddHotkey;
    private Button btnRemoveHotkey;
    private Label lblHotkeyWarning;

    // Controles de Comportamento
    private GroupBox grpBehavior;
    private CheckBox chkStartup;
    private CheckBox chkMinimizeToTray;
    private CheckBox chkShowNotification;
    private CheckBox chkPlaySound;

    // Controles de Teste
    private GroupBox grpTest;
    private Button btnTestDrawer;
    private Label lblLastExecutionTitle;
    private Label lblLastExecution;

    // Botões de Ação
    private Panel pnlActions;
    private Button btnSave;
    private Button btnExport;
    private Button btnImport;

    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
        {
            components.Dispose();
        }
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        this.components = new System.ComponentModel.Container();
        this.SuspendLayout();
        
        // === Cores do tema ===
        var bgColor = Color.FromArgb(45, 45, 48);
        var groupBgColor = Color.FromArgb(37, 37, 38);
        var accentColor = Color.FromArgb(0, 120, 212);
        var textColor = Color.White;
        var buttonColor = Color.FromArgb(60, 60, 60);

        // === GRUPO IMPRESSORA ===
        this.grpPrinter = new GroupBox();
        this.lblPrinterTitle = new Label();
        this.cmbPrinter = new ComboBox();
        this.btnRefreshPrinters = new Button();
        this.lblPrinterStatus = new Label();

        this.grpPrinter.BackColor = groupBgColor;
        this.grpPrinter.ForeColor = textColor;
        this.grpPrinter.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        this.grpPrinter.Location = new Point(12, 12);
        this.grpPrinter.Size = new Size(360, 100);
        this.grpPrinter.Text = "🖨️ Impressora da Gaveta";

        this.lblPrinterTitle.AutoSize = true;
        this.lblPrinterTitle.Font = new Font("Segoe UI", 9F);
        this.lblPrinterTitle.ForeColor = Color.LightGray;
        this.lblPrinterTitle.Location = new Point(10, 30);
        this.lblPrinterTitle.Text = "Selecione a impressora:";

        this.cmbPrinter.BackColor = Color.FromArgb(30, 30, 30);
        this.cmbPrinter.ForeColor = textColor;
        this.cmbPrinter.FlatStyle = FlatStyle.Flat;
        this.cmbPrinter.Font = new Font("Segoe UI", 10F);
        this.cmbPrinter.DropDownStyle = ComboBoxStyle.DropDownList;
        this.cmbPrinter.Location = new Point(10, 50);
        this.cmbPrinter.Size = new Size(250, 25);
        this.cmbPrinter.SelectedIndexChanged += new EventHandler(this.cmbPrinter_SelectedIndexChanged);

        this.btnRefreshPrinters.BackColor = buttonColor;
        this.btnRefreshPrinters.FlatAppearance.BorderColor = Color.FromArgb(80, 80, 80);
        this.btnRefreshPrinters.FlatStyle = FlatStyle.Flat;
        this.btnRefreshPrinters.ForeColor = textColor;
        this.btnRefreshPrinters.Font = new Font("Segoe UI", 9F);
        this.btnRefreshPrinters.Location = new Point(270, 50);
        this.btnRefreshPrinters.Size = new Size(80, 26);
        this.btnRefreshPrinters.Text = "🔄 Atualizar";
        this.btnRefreshPrinters.Click += new EventHandler(this.btnRefreshPrinters_Click);

        this.lblPrinterStatus.AutoSize = true;
        this.lblPrinterStatus.Font = new Font("Segoe UI", 9F);
        this.lblPrinterStatus.ForeColor = Color.Orange;
        this.lblPrinterStatus.Location = new Point(10, 78);
        this.lblPrinterStatus.Text = "⚠ Selecione uma impressora";

        this.grpPrinter.Controls.Add(this.lblPrinterTitle);
        this.grpPrinter.Controls.Add(this.cmbPrinter);
        this.grpPrinter.Controls.Add(this.btnRefreshPrinters);
        this.grpPrinter.Controls.Add(this.lblPrinterStatus);

        // === GRUPO HOTKEYS ===
        this.grpHotkeys = new GroupBox();
        this.lstHotkeys = new ListBox();
        this.btnAddHotkey = new Button();
        this.btnRemoveHotkey = new Button();
        this.lblHotkeyWarning = new Label();

        this.grpHotkeys.BackColor = groupBgColor;
        this.grpHotkeys.ForeColor = textColor;
        this.grpHotkeys.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        this.grpHotkeys.Location = new Point(12, 120);
        this.grpHotkeys.Size = new Size(360, 150);
        this.grpHotkeys.Text = "⌨️ Atalhos Configurados";

        this.lstHotkeys.BackColor = Color.FromArgb(30, 30, 30);
        this.lstHotkeys.ForeColor = accentColor;
        this.lstHotkeys.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
        this.lstHotkeys.BorderStyle = BorderStyle.FixedSingle;
        this.lstHotkeys.Location = new Point(10, 25);
        this.lstHotkeys.Size = new Size(250, 85);

        this.btnAddHotkey.BackColor = accentColor;
        this.btnAddHotkey.FlatAppearance.BorderSize = 0;
        this.btnAddHotkey.FlatStyle = FlatStyle.Flat;
        this.btnAddHotkey.ForeColor = textColor;
        this.btnAddHotkey.Font = new Font("Segoe UI", 9F);
        this.btnAddHotkey.Location = new Point(270, 25);
        this.btnAddHotkey.Size = new Size(80, 35);
        this.btnAddHotkey.Text = "➕ Adicionar";
        this.btnAddHotkey.Click += new EventHandler(this.btnAddHotkey_Click);

        this.btnRemoveHotkey.BackColor = buttonColor;
        this.btnRemoveHotkey.FlatAppearance.BorderColor = Color.FromArgb(80, 80, 80);
        this.btnRemoveHotkey.FlatStyle = FlatStyle.Flat;
        this.btnRemoveHotkey.ForeColor = textColor;
        this.btnRemoveHotkey.Font = new Font("Segoe UI", 9F);
        this.btnRemoveHotkey.Location = new Point(270, 70);
        this.btnRemoveHotkey.Size = new Size(80, 35);
        this.btnRemoveHotkey.Text = "➖ Remover";
        this.btnRemoveHotkey.Click += new EventHandler(this.btnRemoveHotkey_Click);

        this.lblHotkeyWarning.AutoSize = false;
        this.lblHotkeyWarning.Font = new Font("Segoe UI", 8F);
        this.lblHotkeyWarning.ForeColor = Color.Orange;
        this.lblHotkeyWarning.Location = new Point(10, 115);
        this.lblHotkeyWarning.Size = new Size(340, 30);
        this.lblHotkeyWarning.Text = "⚠ A gaveta será aberta sempre na impressora selecionada acima";

        this.grpHotkeys.Controls.Add(this.lstHotkeys);
        this.grpHotkeys.Controls.Add(this.btnAddHotkey);
        this.grpHotkeys.Controls.Add(this.btnRemoveHotkey);
        this.grpHotkeys.Controls.Add(this.lblHotkeyWarning);

        // === GRUPO COMPORTAMENTO ===
        this.grpBehavior = new GroupBox();
        this.chkStartup = new CheckBox();
        this.chkMinimizeToTray = new CheckBox();
        this.chkShowNotification = new CheckBox();
        this.chkPlaySound = new CheckBox();

        this.grpBehavior.BackColor = groupBgColor;
        this.grpBehavior.ForeColor = textColor;
        this.grpBehavior.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        this.grpBehavior.Location = new Point(12, 280);
        this.grpBehavior.Size = new Size(360, 100);
        this.grpBehavior.Text = "🔧 Comportamento";

        this.chkStartup.AutoSize = true;
        this.chkStartup.Font = new Font("Segoe UI", 9F);
        this.chkStartup.ForeColor = textColor;
        this.chkStartup.Location = new Point(10, 28);
        this.chkStartup.Text = "Iniciar com Windows";

        this.chkMinimizeToTray.AutoSize = true;
        this.chkMinimizeToTray.Font = new Font("Segoe UI", 9F);
        this.chkMinimizeToTray.ForeColor = textColor;
        this.chkMinimizeToTray.Location = new Point(10, 52);
        this.chkMinimizeToTray.Text = "Minimizar para bandeja do sistema";
        this.chkMinimizeToTray.Checked = true;

        this.chkShowNotification.AutoSize = true;
        this.chkShowNotification.Font = new Font("Segoe UI", 9F);
        this.chkShowNotification.ForeColor = textColor;
        this.chkShowNotification.Location = new Point(200, 28);
        this.chkShowNotification.Text = "Mostrar notificação";
        this.chkShowNotification.Checked = true;

        this.chkPlaySound.AutoSize = true;
        this.chkPlaySound.Font = new Font("Segoe UI", 9F);
        this.chkPlaySound.ForeColor = textColor;
        this.chkPlaySound.Location = new Point(200, 52);
        this.chkPlaySound.Text = "Reproduzir som";
        this.chkPlaySound.Checked = true;

        this.grpBehavior.Controls.Add(this.chkStartup);
        this.grpBehavior.Controls.Add(this.chkMinimizeToTray);
        this.grpBehavior.Controls.Add(this.chkShowNotification);
        this.grpBehavior.Controls.Add(this.chkPlaySound);

        // === GRUPO TESTE ===
        this.grpTest = new GroupBox();
        this.btnTestDrawer = new Button();
        this.lblLastExecutionTitle = new Label();
        this.lblLastExecution = new Label();

        this.grpTest.BackColor = groupBgColor;
        this.grpTest.ForeColor = textColor;
        this.grpTest.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        this.grpTest.Location = new Point(12, 390);
        this.grpTest.Size = new Size(360, 80);
        this.grpTest.Text = "🧪 Teste e Diagnóstico";

        this.btnTestDrawer.BackColor = Color.FromArgb(46, 160, 67);
        this.btnTestDrawer.FlatAppearance.BorderSize = 0;
        this.btnTestDrawer.FlatStyle = FlatStyle.Flat;
        this.btnTestDrawer.ForeColor = textColor;
        this.btnTestDrawer.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        this.btnTestDrawer.Location = new Point(10, 30);
        this.btnTestDrawer.Size = new Size(180, 35);
        this.btnTestDrawer.Text = "🔓 Testar Abertura";
        this.btnTestDrawer.Click += new EventHandler(this.btnTestDrawer_Click);

        this.lblLastExecutionTitle.AutoSize = true;
        this.lblLastExecutionTitle.Font = new Font("Segoe UI", 8F);
        this.lblLastExecutionTitle.ForeColor = Color.Gray;
        this.lblLastExecutionTitle.Location = new Point(200, 30);
        this.lblLastExecutionTitle.Text = "Última execução:";

        this.lblLastExecution.AutoSize = true;
        this.lblLastExecution.Font = new Font("Segoe UI", 9F);
        this.lblLastExecution.ForeColor = Color.LightGray;
        this.lblLastExecution.Location = new Point(200, 48);
        this.lblLastExecution.Text = "Nenhuma";

        this.grpTest.Controls.Add(this.btnTestDrawer);
        this.grpTest.Controls.Add(this.lblLastExecutionTitle);
        this.grpTest.Controls.Add(this.lblLastExecution);

        // === PAINEL DE AÇÕES ===
        this.pnlActions = new Panel();
        this.btnSave = new Button();
        this.btnExport = new Button();
        this.btnImport = new Button();

        this.pnlActions.BackColor = Color.FromArgb(30, 30, 30);
        this.pnlActions.Location = new Point(0, 480);
        this.pnlActions.Size = new Size(385, 50);

        this.btnSave.BackColor = accentColor;
        this.btnSave.FlatAppearance.BorderSize = 0;
        this.btnSave.FlatStyle = FlatStyle.Flat;
        this.btnSave.ForeColor = textColor;
        this.btnSave.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        this.btnSave.Location = new Point(12, 10);
        this.btnSave.Size = new Size(150, 32);
        this.btnSave.Text = "💾 Salvar";
        this.btnSave.Click += new EventHandler(this.btnSave_Click);

        this.btnExport.BackColor = buttonColor;
        this.btnExport.FlatAppearance.BorderColor = Color.FromArgb(80, 80, 80);
        this.btnExport.FlatStyle = FlatStyle.Flat;
        this.btnExport.ForeColor = textColor;
        this.btnExport.Font = new Font("Segoe UI", 9F);
        this.btnExport.Location = new Point(175, 10);
        this.btnExport.Size = new Size(95, 32);
        this.btnExport.Text = "📤 Exportar";
        this.btnExport.Click += new EventHandler(this.btnExport_Click);

        this.btnImport.BackColor = buttonColor;
        this.btnImport.FlatAppearance.BorderColor = Color.FromArgb(80, 80, 80);
        this.btnImport.FlatStyle = FlatStyle.Flat;
        this.btnImport.ForeColor = textColor;
        this.btnImport.Font = new Font("Segoe UI", 9F);
        this.btnImport.Location = new Point(278, 10);
        this.btnImport.Size = new Size(95, 32);
        this.btnImport.Text = "📥 Importar";
        this.btnImport.Click += new EventHandler(this.btnImport_Click);

        this.pnlActions.Controls.Add(this.btnSave);
        this.pnlActions.Controls.Add(this.btnExport);
        this.pnlActions.Controls.Add(this.btnImport);

        // === FORM PRINCIPAL ===
        this.AutoScaleDimensions = new SizeF(7F, 15F);
        this.AutoScaleMode = AutoScaleMode.Font;
        this.BackColor = bgColor;
        this.ClientSize = new Size(385, 530);
        this.Controls.Add(this.grpPrinter);
        this.Controls.Add(this.grpHotkeys);
        this.Controls.Add(this.grpBehavior);
        this.Controls.Add(this.grpTest);
        this.Controls.Add(this.pnlActions);
        this.FormBorderStyle = FormBorderStyle.FixedSingle;
        this.MaximizeBox = false;
        this.Name = "MainForm";
        this.StartPosition = FormStartPosition.CenterScreen;
        this.Text = "GavetaHotkeyApp - Configuração";
        this.ResumeLayout(false);
        this.PerformLayout();
    }
}
