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
        Me.SuspendLayout()
        '
        'VdFramedControl
        '
        Me.VdFramedControl.AccessibleRole = System.Windows.Forms.AccessibleRole.Pane
        Me.VdFramedControl.DisplayPolarCoord = False
        Me.VdFramedControl.HistoryLines = CType(3UI, UInteger)
        Me.VdFramedControl.LoadCommandstxt = True
        Me.VdFramedControl.LoadMenutxt = True
        Me.VdFramedControl.Location = New System.Drawing.Point(0, 50)
        Me.VdFramedControl.Name = "VdFramedControl"
        Me.VdFramedControl.PropertyGridWidth = CType(300UI, UInteger)
        Me.VdFramedControl.Size = New System.Drawing.Size(1900, 900)
        Me.VdFramedControl.TabIndex = 1
        '
        'ButOpen
        '
        Me.ButOpen.Location = New System.Drawing.Point(12, 10)
        Me.ButOpen.Name = "ButOpen"
        Me.ButOpen.Size = New System.Drawing.Size(48, 23)
        Me.ButOpen.TabIndex = 3
        Me.ButOpen.Text = "Open"
        Me.ButOpen.UseVisualStyleBackColor = True
        '
        'ButtonSave
        '
        Me.ButtonSave.Location = New System.Drawing.Point(66, 10)
        Me.ButtonSave.Name = "ButtonSave"
        Me.ButtonSave.Size = New System.Drawing.Size(75, 23)
        Me.ButtonSave.TabIndex = 4
        Me.ButtonSave.Text = "Save"
        Me.ButtonSave.UseVisualStyleBackColor = True
        '
        'CheckBoxGravity
        '
        Me.CheckBoxGravity.AutoSize = True
        Me.CheckBoxGravity.Location = New System.Drawing.Point(147, 14)
        Me.CheckBoxGravity.Name = "CheckBoxGravity"
        Me.CheckBoxGravity.Size = New System.Drawing.Size(59, 17)
        Me.CheckBoxGravity.TabIndex = 7
        Me.CheckBoxGravity.Text = "Gravity"
        Me.CheckBoxGravity.UseVisualStyleBackColor = True
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1920, 1080)
        Me.Controls.Add(Me.CheckBoxGravity)
        Me.Controls.Add(Me.ButtonSave)
        Me.Controls.Add(Me.ButOpen)
        Me.Controls.Add(Me.VdFramedControl)
        Me.Name = "Form1"
        Me.Text = "Form1"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub


    Friend WithEvents VdFramedControl As vdControls.vdFramedControl
    Friend WithEvents ButOpen As Button
    Friend WithEvents ButtonSave As Button
    Friend WithEvents CheckBoxGravity As CheckBox
End Class
