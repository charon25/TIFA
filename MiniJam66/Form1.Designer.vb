<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form1
    Inherits System.Windows.Forms.Form

    'Form remplace la méthode Dispose pour nettoyer la liste des composants.
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

    'Requise par le Concepteur Windows Form
    Private components As System.ComponentModel.IContainer

    'REMARQUE : la procédure suivante est requise par le Concepteur Windows Form
    'Elle peut être modifiée à l'aide du Concepteur Windows Form.  
    'Ne la modifiez pas à l'aide de l'éditeur de code.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Form1))
        Me.gameTimer = New System.Windows.Forms.Timer(Me.components)
        Me.gamePanel = New System.Windows.Forms.Panel()
        Me.SuspendLayout()
        '
        'gameTimer
        '
        Me.gameTimer.Interval = 17
        '
        'gamePanel
        '
        Me.gamePanel.Location = New System.Drawing.Point(0, 0)
        Me.gamePanel.Name = "gamePanel"
        Me.gamePanel.Size = New System.Drawing.Size(400, 400)
        Me.gamePanel.TabIndex = 0
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.White
        Me.ClientSize = New System.Drawing.Size(400, 400)
        Me.Controls.Add(Me.gamePanel)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "Form1"
        Me.Text = "TIFA"
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents gameTimer As System.Windows.Forms.Timer
    Friend WithEvents gamePanel As System.Windows.Forms.Panel

End Class
