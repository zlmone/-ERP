﻿<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmNmetalSampleUpdateManual_Number
    Inherits System.Windows.Forms.Form

    'Form 重写 Dispose，以清理组件列表。
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing AndAlso components IsNot Nothing Then
            components.Dispose()
        End If
        MyBase.Dispose(disposing)
    End Sub

    'Windows 窗体设计器所必需的
    Private components As System.ComponentModel.IContainer

    '注意: 以下过程是 Windows 窗体设计器所必需的
    '可以使用 Windows 窗体设计器修改它。
    '不要使用代码编辑器修改它。
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.Label1 = New System.Windows.Forms.Label
        Me.txtSO_ID = New DevExpress.XtraEditors.TextEdit
        Me.txtPM_M_Code = New DevExpress.XtraEditors.TextEdit
        Me.Label12 = New System.Windows.Forms.Label
        Me.txtSS_Edition = New DevExpress.XtraEditors.TextEdit
        Me.Label9 = New System.Windows.Forms.Label
        Me.Label10 = New System.Windows.Forms.Label
        Me.GroupBox1 = New System.Windows.Forms.GroupBox
        Me.Label3 = New System.Windows.Forms.Label
        Me.txt_NewManual_Number = New DevExpress.XtraEditors.TextEdit
        Me.Label2 = New System.Windows.Forms.Label
        Me.txt_OldManual_Number = New DevExpress.XtraEditors.TextEdit
        Me.btn_exit = New DevExpress.XtraEditors.SimpleButton
        Me.btn_save = New DevExpress.XtraEditors.SimpleButton
        Me.PictureBox1 = New System.Windows.Forms.PictureBox
        CType(Me.txtSO_ID.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtPM_M_Code.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtSS_Edition.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GroupBox1.SuspendLayout()
        CType(Me.txt_NewManual_Number.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txt_OldManual_Number.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.Label1.Font = New System.Drawing.Font("標楷體", 15.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(136, Byte))
        Me.Label1.Location = New System.Drawing.Point(5, 6)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(142, 21)
        Me.Label1.TabIndex = 46
        Me.Label1.Text = "更改手册编号"
        '
        'txtSO_ID
        '
        Me.txtSO_ID.Enabled = False
        Me.txtSO_ID.Location = New System.Drawing.Point(101, 44)
        Me.txtSO_ID.Name = "txtSO_ID"
        Me.txtSO_ID.Properties.Appearance.Font = New System.Drawing.Font("新細明體", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(136, Byte))
        Me.txtSO_ID.Properties.Appearance.Options.UseFont = True
        Me.txtSO_ID.Size = New System.Drawing.Size(199, 24)
        Me.txtSO_ID.TabIndex = 113
        '
        'txtPM_M_Code
        '
        Me.txtPM_M_Code.Enabled = False
        Me.txtPM_M_Code.Location = New System.Drawing.Point(101, 100)
        Me.txtPM_M_Code.Name = "txtPM_M_Code"
        Me.txtPM_M_Code.Properties.Appearance.Font = New System.Drawing.Font("新細明體", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(136, Byte))
        Me.txtPM_M_Code.Properties.Appearance.Options.UseFont = True
        Me.txtPM_M_Code.Size = New System.Drawing.Size(199, 24)
        Me.txtPM_M_Code.TabIndex = 112
        '
        'Label12
        '
        Me.Label12.AutoSize = True
        Me.Label12.BackColor = System.Drawing.Color.Transparent
        Me.Label12.Font = New System.Drawing.Font("新細明體", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(136, Byte))
        Me.Label12.ForeColor = System.Drawing.Color.Black
        Me.Label12.Location = New System.Drawing.Point(8, 48)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(91, 15)
        Me.Label12.TabIndex = 108
        Me.Label12.Text = "订单编号(&H):"
        '
        'txtSS_Edition
        '
        Me.txtSS_Edition.Enabled = False
        Me.txtSS_Edition.Location = New System.Drawing.Point(101, 72)
        Me.txtSS_Edition.Name = "txtSS_Edition"
        Me.txtSS_Edition.Properties.Appearance.Font = New System.Drawing.Font("新細明體", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(136, Byte))
        Me.txtSS_Edition.Properties.Appearance.Options.UseFont = True
        Me.txtSS_Edition.Size = New System.Drawing.Size(199, 24)
        Me.txtSS_Edition.TabIndex = 110
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.BackColor = System.Drawing.Color.Transparent
        Me.Label9.Font = New System.Drawing.Font("新細明體", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(136, Byte))
        Me.Label9.ForeColor = System.Drawing.Color.Black
        Me.Label9.Location = New System.Drawing.Point(8, 76)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(91, 15)
        Me.Label9.TabIndex = 109
        Me.Label9.Text = "版  本  号(&B):"
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.BackColor = System.Drawing.Color.Transparent
        Me.Label10.CausesValidation = False
        Me.Label10.Font = New System.Drawing.Font("新細明體", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(136, Byte))
        Me.Label10.ForeColor = System.Drawing.Color.Black
        Me.Label10.Location = New System.Drawing.Point(9, 104)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(90, 15)
        Me.Label10.TabIndex = 111
        Me.Label10.Text = "产品编号(&C):"
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.Label3)
        Me.GroupBox1.Controls.Add(Me.txt_NewManual_Number)
        Me.GroupBox1.Controls.Add(Me.Label2)
        Me.GroupBox1.Controls.Add(Me.txt_OldManual_Number)
        Me.GroupBox1.ForeColor = System.Drawing.Color.Blue
        Me.GroupBox1.Location = New System.Drawing.Point(8, 131)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(293, 88)
        Me.GroupBox1.TabIndex = 114
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "更改手册编号"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.BackColor = System.Drawing.Color.Transparent
        Me.Label3.CausesValidation = False
        Me.Label3.Font = New System.Drawing.Font("新細明體", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(136, Byte))
        Me.Label3.ForeColor = System.Drawing.Color.Black
        Me.Label3.Location = New System.Drawing.Point(18, 58)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(106, 15)
        Me.Label3.TabIndex = 105
        Me.Label3.Text = "新手册编号(&N):"
        '
        'txt_NewManual_Number
        '
        Me.txt_NewManual_Number.Location = New System.Drawing.Point(128, 54)
        Me.txt_NewManual_Number.Name = "txt_NewManual_Number"
        Me.txt_NewManual_Number.Properties.Appearance.Font = New System.Drawing.Font("新細明體", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(136, Byte))
        Me.txt_NewManual_Number.Properties.Appearance.Options.UseFont = True
        Me.txt_NewManual_Number.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
        Me.txt_NewManual_Number.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric
        Me.txt_NewManual_Number.Properties.Mask.EditMask = "d"
        Me.txt_NewManual_Number.Size = New System.Drawing.Size(146, 24)
        Me.txt_NewManual_Number.TabIndex = 106
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.BackColor = System.Drawing.Color.Transparent
        Me.Label2.CausesValidation = False
        Me.Label2.Font = New System.Drawing.Font("新細明體", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(136, Byte))
        Me.Label2.ForeColor = System.Drawing.Color.Black
        Me.Label2.Location = New System.Drawing.Point(18, 23)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(106, 15)
        Me.Label2.TabIndex = 103
        Me.Label2.Text = "旧手册编号(&O):"
        '
        'txt_OldManual_Number
        '
        Me.txt_OldManual_Number.Enabled = False
        Me.txt_OldManual_Number.Location = New System.Drawing.Point(128, 18)
        Me.txt_OldManual_Number.Name = "txt_OldManual_Number"
        Me.txt_OldManual_Number.Properties.Appearance.Font = New System.Drawing.Font("新細明體", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(136, Byte))
        Me.txt_OldManual_Number.Properties.Appearance.Options.UseFont = True
        Me.txt_OldManual_Number.Size = New System.Drawing.Size(146, 24)
        Me.txt_OldManual_Number.TabIndex = 104
        '
        'btn_exit
        '
        Me.btn_exit.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btn_exit.Image = Global.LFERP.My.Resources.Resources.SharingRequestDeny
        Me.btn_exit.Location = New System.Drawing.Point(192, 225)
        Me.btn_exit.Name = "btn_exit"
        Me.btn_exit.Size = New System.Drawing.Size(80, 29)
        Me.btn_exit.TabIndex = 116
        Me.btn_exit.Text = "取消(&E)"
        '
        'btn_save
        '
        Me.btn_save.Image = Global.LFERP.My.Resources.Resources.ReviseContents
        Me.btn_save.Location = New System.Drawing.Point(38, 225)
        Me.btn_save.Name = "btn_save"
        Me.btn_save.Size = New System.Drawing.Size(80, 29)
        Me.btn_save.TabIndex = 115
        Me.btn_save.Text = "更改(&S)"
        '
        'PictureBox1
        '
        Me.PictureBox1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.PictureBox1.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.PictureBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.PictureBox1.Location = New System.Drawing.Point(0, 0)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New System.Drawing.Size(309, 33)
        Me.PictureBox1.TabIndex = 45
        Me.PictureBox1.TabStop = False
        '
        'frmNmetalSampleUpdateManual_Number
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(309, 262)
        Me.Controls.Add(Me.btn_exit)
        Me.Controls.Add(Me.btn_save)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.txtSO_ID)
        Me.Controls.Add(Me.txtPM_M_Code)
        Me.Controls.Add(Me.Label12)
        Me.Controls.Add(Me.txtSS_Edition)
        Me.Controls.Add(Me.Label9)
        Me.Controls.Add(Me.Label10)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.PictureBox1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmNmetalSampleUpdateManual_Number"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "更改手册编号"
        CType(Me.txtSO_ID.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtPM_M_Code.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtSS_Edition.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        CType(Me.txt_NewManual_Number.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txt_OldManual_Number.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents PictureBox1 As System.Windows.Forms.PictureBox
    Friend WithEvents txtSO_ID As DevExpress.XtraEditors.TextEdit
    Friend WithEvents txtPM_M_Code As DevExpress.XtraEditors.TextEdit
    Friend WithEvents Label12 As System.Windows.Forms.Label
    Friend WithEvents txtSS_Edition As DevExpress.XtraEditors.TextEdit
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents txt_NewManual_Number As DevExpress.XtraEditors.TextEdit
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents txt_OldManual_Number As DevExpress.XtraEditors.TextEdit
    Friend WithEvents btn_exit As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents btn_save As DevExpress.XtraEditors.SimpleButton
End Class
