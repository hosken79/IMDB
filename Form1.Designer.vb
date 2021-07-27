<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form1
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
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
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.bt_start = New System.Windows.Forms.Button()
        Me.lblStatus = New System.Windows.Forms.Label()
        Me.BW1 = New System.ComponentModel.BackgroundWorker()
        Me.SuspendLayout()
        '
        'bt_start
        '
        Me.bt_start.Location = New System.Drawing.Point(27, 25)
        Me.bt_start.Name = "bt_start"
        Me.bt_start.Size = New System.Drawing.Size(75, 23)
        Me.bt_start.TabIndex = 0
        Me.bt_start.Text = "Start"
        Me.bt_start.UseVisualStyleBackColor = True
        '
        'lblStatus
        '
        Me.lblStatus.AutoSize = True
        Me.lblStatus.Location = New System.Drawing.Point(142, 30)
        Me.lblStatus.Name = "lblStatus"
        Me.lblStatus.Size = New System.Drawing.Size(37, 13)
        Me.lblStatus.TabIndex = 2
        Me.lblStatus.Text = "Status"
        '
        'BW1
        '
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(361, 75)
        Me.Controls.Add(Me.lblStatus)
        Me.Controls.Add(Me.bt_start)
        Me.Name = "Form1"
        Me.Text = "IMDB Downloader"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents bt_start As Button
    Friend WithEvents lblStatus As Label
    Friend WithEvents BW1 As System.ComponentModel.BackgroundWorker
End Class
