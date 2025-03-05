<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class Form1
    Inherits System.Windows.Forms.Form




    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.VdFramedControl = New vdControls.vdFramedControl()
        Me.ButOpen = New System.Windows.Forms.Button()
        Me.ButtonSave = New System.Windows.Forms.Button()
        Me.CheckBoxGravity = New System.Windows.Forms.CheckBox()
        Me.massageBox = New System.Windows.Forms.TextBox()
        Me.btnMirrirOff = New System.Windows.Forms.CheckBox()
        Me.Button_Undo = New System.Windows.Forms.Button()
        Me.Button_Redo = New System.Windows.Forms.Button()
        Me.MenuStrip1 = New System.Windows.Forms.MenuStrip()
        Me.Button_Print = New System.Windows.Forms.Button()
        Me.CheckBox_Delete = New System.Windows.Forms.CheckBox()
        Me.SuspendLayout()
        '
        'VdFramedControl
        '
        Me.VdFramedControl.AccessibleRole = System.Windows.Forms.AccessibleRole.Pane
        Me.VdFramedControl.DisplayPolarCoord = False
        Me.VdFramedControl.HistoryLines = CType(3UI, UInteger)
        Me.VdFramedControl.LoadCommandstxt = True
        Me.VdFramedControl.LoadMenutxt = True
        Me.VdFramedControl.Location = New System.Drawing.Point(0, 100)
        Me.VdFramedControl.Name = "VdFramedControl"
        Me.VdFramedControl.PropertyGridWidth = CType(300UI, UInteger)
        Me.VdFramedControl.Size = New System.Drawing.Size(1900, 900)
        Me.VdFramedControl.TabIndex = 1
        '
        'ButOpen
        '
        Me.ButOpen.Location = New System.Drawing.Point(12, 12)
        Me.ButOpen.Name = "ButOpen"
        Me.ButOpen.Size = New System.Drawing.Size(48, 41)
        Me.ButOpen.TabIndex = 3
        Me.ButOpen.Text = "Open"
        Me.ButOpen.UseVisualStyleBackColor = True
        '
        'ButtonSave
        '
        Me.ButtonSave.Location = New System.Drawing.Point(66, 12)
        Me.ButtonSave.Name = "ButtonSave"
        Me.ButtonSave.Size = New System.Drawing.Size(48, 41)
        Me.ButtonSave.TabIndex = 4
        Me.ButtonSave.Text = "Save"
        Me.ButtonSave.UseVisualStyleBackColor = True
        '
        'CheckBoxGravity
        '
        Me.CheckBoxGravity.AutoSize = True
        Me.CheckBoxGravity.Location = New System.Drawing.Point(120, 14)
        Me.CheckBoxGravity.Name = "CheckBoxGravity"
        Me.CheckBoxGravity.Size = New System.Drawing.Size(59, 17)
        Me.CheckBoxGravity.TabIndex = 7
        Me.CheckBoxGravity.Text = "Gravity"
        Me.CheckBoxGravity.UseVisualStyleBackColor = True
        '
        'massageBox
        '
        Me.massageBox.Location = New System.Drawing.Point(193, 12)
        Me.massageBox.Multiline = True
        Me.massageBox.Name = "massageBox"
        Me.massageBox.Size = New System.Drawing.Size(520, 84)
        Me.massageBox.TabIndex = 8
        '
        'btnMirrirOff
        '
        Me.btnMirrirOff.AutoSize = True
        Me.btnMirrirOff.Location = New System.Drawing.Point(120, 36)
        Me.btnMirrirOff.Name = "btnMirrirOff"
        Me.btnMirrirOff.Size = New System.Drawing.Size(67, 17)
        Me.btnMirrirOff.TabIndex = 9
        Me.btnMirrirOff.Text = "Mirror off"
        Me.btnMirrirOff.UseVisualStyleBackColor = True
        '
        'Button_Undo
        '
        Me.Button_Undo.Location = New System.Drawing.Point(732, 12)
        Me.Button_Undo.Name = "Button_Undo"
        Me.Button_Undo.Size = New System.Drawing.Size(75, 37)
        Me.Button_Undo.TabIndex = 10
        Me.Button_Undo.Text = "Undo"
        Me.Button_Undo.UseVisualStyleBackColor = True
        '
        'Button_Redo
        '
        Me.Button_Redo.Location = New System.Drawing.Point(732, 55)
        Me.Button_Redo.Name = "Button_Redo"
        Me.Button_Redo.Size = New System.Drawing.Size(75, 41)
        Me.Button_Redo.TabIndex = 11
        Me.Button_Redo.Text = "Redo"
        Me.Button_Redo.UseVisualStyleBackColor = True
        '
        'MenuStrip1
        '
        Me.MenuStrip1.Location = New System.Drawing.Point(0, 0)
        Me.MenuStrip1.Name = "MenuStrip1"
        Me.MenuStrip1.Size = New System.Drawing.Size(1920, 24)
        Me.MenuStrip1.TabIndex = 13
        Me.MenuStrip1.Text = "MenuStrip1"
        '
        'Button_Print
        '
        Me.Button_Print.Location = New System.Drawing.Point(66, 59)
        Me.Button_Print.Name = "Button_Print"
        Me.Button_Print.Size = New System.Drawing.Size(48, 23)
        Me.Button_Print.TabIndex = 14
        Me.Button_Print.Text = "Print"
        Me.Button_Print.UseVisualStyleBackColor = True
        '
        'CheckBox_Delete
        '
        Me.CheckBox_Delete.AutoSize = True
        Me.CheckBox_Delete.Location = New System.Drawing.Point(822, 14)
        Me.CheckBox_Delete.Name = "CheckBox_Delete"
        Me.CheckBox_Delete.Size = New System.Drawing.Size(57, 17)
        Me.CheckBox_Delete.TabIndex = 15
        Me.CheckBox_Delete.Text = "Delete"
        Me.CheckBox_Delete.UseVisualStyleBackColor = True
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1920, 1080)
        Me.Controls.Add(Me.CheckBox_Delete)
        Me.Controls.Add(Me.Button_Print)
        Me.Controls.Add(Me.Button_Redo)
        Me.Controls.Add(Me.Button_Undo)
        Me.Controls.Add(Me.btnMirrirOff)
        Me.Controls.Add(Me.massageBox)
        Me.Controls.Add(Me.CheckBoxGravity)
        Me.Controls.Add(Me.ButtonSave)
        Me.Controls.Add(Me.ButOpen)
        Me.Controls.Add(Me.VdFramedControl)
        Me.Controls.Add(Me.MenuStrip1)
        Me.MainMenuStrip = Me.MenuStrip1
        Me.Name = "Form1"
        Me.Text = "Form1"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub



    Friend WithEvents VdFramedControl As vdControls.vdFramedControl
    Friend WithEvents ButOpen As System.Windows.Forms.Button
    Friend WithEvents ButtonSave As System.Windows.Forms.Button
    Friend WithEvents CheckBoxGravity As CheckBox
    Friend WithEvents massageBox As TextBox
    Friend WithEvents btnMirrirOff As CheckBox
    Friend WithEvents Button_Undo As System.Windows.Forms.Button
    Friend WithEvents Button_Redo As System.Windows.Forms.Button
    Friend WithEvents MenuStrip1 As MenuStrip
    Friend WithEvents Button_Print As System.Windows.Forms.Button
    Friend WithEvents CheckBox_Delete As CheckBox
End Class
