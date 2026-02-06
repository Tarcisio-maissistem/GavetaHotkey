namespace GavetaHotkeyApp.Forms;

partial class HotkeyRecorderForm
{
    private System.ComponentModel.IContainer components = null;
    private Label lblTitle;
    private Label lblCurrentKeys;
    private Label lblStatus;
    private Label lblHint;
    private Button btnCancel;

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
        this.lblTitle = new Label();
        this.lblCurrentKeys = new Label();
        this.lblStatus = new Label();
        this.lblHint = new Label();
        this.btnCancel = new Button();
        this.SuspendLayout();

        // lblTitle
        this.lblTitle.AutoSize = false;
        this.lblTitle.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
        this.lblTitle.ForeColor = Color.White;
        this.lblTitle.Location = new Point(12, 20);
        this.lblTitle.Size = new Size(300, 25);
        this.lblTitle.Text = "🎹 Pressione a combinação desejada";
        this.lblTitle.TextAlign = ContentAlignment.MiddleCenter;

        // lblCurrentKeys
        this.lblCurrentKeys.AutoSize = false;
        this.lblCurrentKeys.BackColor = Color.FromArgb(30, 30, 30);
        this.lblCurrentKeys.BorderStyle = BorderStyle.FixedSingle;
        this.lblCurrentKeys.Font = new Font("Segoe UI", 18F, FontStyle.Bold);
        this.lblCurrentKeys.ForeColor = Color.FromArgb(0, 120, 212);
        this.lblCurrentKeys.Location = new Point(12, 60);
        this.lblCurrentKeys.Size = new Size(300, 60);
        this.lblCurrentKeys.Text = "...";
        this.lblCurrentKeys.TextAlign = ContentAlignment.MiddleCenter;

        // lblStatus
        this.lblStatus.AutoSize = false;
        this.lblStatus.Font = new Font("Segoe UI", 9F);
        this.lblStatus.ForeColor = Color.LightGray;
        this.lblStatus.Location = new Point(12, 130);
        this.lblStatus.Size = new Size(300, 20);
        this.lblStatus.Text = "Aguardando teclas...";
        this.lblStatus.TextAlign = ContentAlignment.MiddleCenter;

        // lblHint
        this.lblHint.AutoSize = false;
        this.lblHint.Font = new Font("Segoe UI", 8F);
        this.lblHint.ForeColor = Color.Gray;
        this.lblHint.Location = new Point(12, 155);
        this.lblHint.Size = new Size(300, 20);
        this.lblHint.Text = "Use CTRL, SHIFT ou ALT + uma tecla";
        this.lblHint.TextAlign = ContentAlignment.MiddleCenter;

        // btnCancel
        this.btnCancel.BackColor = Color.FromArgb(60, 60, 60);
        this.btnCancel.FlatAppearance.BorderColor = Color.FromArgb(80, 80, 80);
        this.btnCancel.FlatStyle = FlatStyle.Flat;
        this.btnCancel.Font = new Font("Segoe UI", 9F);
        this.btnCancel.ForeColor = Color.White;
        this.btnCancel.Location = new Point(110, 185);
        this.btnCancel.Size = new Size(100, 30);
        this.btnCancel.Text = "Cancelar (ESC)";
        this.btnCancel.UseVisualStyleBackColor = false;
        this.btnCancel.Click += new EventHandler(this.btnCancel_Click);

        // HotkeyRecorderForm
        this.AutoScaleDimensions = new SizeF(7F, 15F);
        this.AutoScaleMode = AutoScaleMode.Font;
        this.BackColor = Color.FromArgb(45, 45, 48);
        this.ClientSize = new Size(324, 230);
        this.Controls.Add(this.lblTitle);
        this.Controls.Add(this.lblCurrentKeys);
        this.Controls.Add(this.lblStatus);
        this.Controls.Add(this.lblHint);
        this.Controls.Add(this.btnCancel);
        this.FormBorderStyle = FormBorderStyle.FixedDialog;
        this.KeyPreview = true;
        this.MaximizeBox = false;
        this.MinimizeBox = false;
        this.Name = "HotkeyRecorderForm";
        this.ShowInTaskbar = false;
        this.StartPosition = FormStartPosition.CenterParent;
        this.Text = "Gravar Atalho";
        this.KeyDown += new KeyEventHandler(this.HotkeyRecorderForm_KeyDown);
        this.KeyUp += new KeyEventHandler(this.HotkeyRecorderForm_KeyUp);
        this.ResumeLayout(false);
    }
}
