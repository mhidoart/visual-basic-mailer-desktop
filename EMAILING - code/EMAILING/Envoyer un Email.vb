Public Class Envoyer_un_Email


    Private Sub Envoyer_un_Email_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.TabPage1.Text = "HTML"
        Me.TabPage2.Text = "SIMPLE text"
        Dim t As comptes() = charger_email()
        Dim i As Integer
        If t Is Nothing Then
            Me.ComboBox1.Text = "il faut ajouter des email dans partie admin !!"
        Else
            For i = 0 To UBound(t)
                Me.ComboBox1.Items.Add(t(i).adr)
            Next
            Me.ComboBox1.Text = Me.ComboBox1.Items(0).ToString
        End If
        
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Dim index As Integer = Me.TabControl1.SelectedIndex
        Dim t As comptes() = charger_email()
        Dim id As Integer
        Dim i As Integer
        For i = 0 To UBound(t)
            If t(i).adr = Me.ComboBox1.Text Then
                id = i
                i = UBound(t)
            End If
        Next
        Dim succee As Integer
        Try
            SmtpServer.Credentials = New Net.NetworkCredential(t(id).adr, t(id).pass)
            SmtpServer.Port = 587
            SmtpServer.Host = "smtp.gmail.com" 'l'host c'est google.
            SmtpServer.EnableSsl = True

            If Me.TabControl1.SelectedIndex = 0 Then
                succee = envoyer_un_mail(t(id).adr, t(id).pass, Me.TextBox4.Text, Me.TabControl1.SelectedTab.Controls.Item(0).Text, Me.TextBox3.Text, 0)
            Else
                succee = envoyer_un_mail(t(id).adr, t(id).pass, Me.TextBox4.Text, Me.TabControl1.SelectedTab.Controls.Item(0).Text, Me.TextBox3.Text, 1)
            End If
            If succee = 0 Then
                MsgBox("envoie avec succee !!")
            Else
                MsgBox("erreur d\'envoie !!")
            End If
        Catch ex As Exception
            MsgBox("erreur d\'envoie !!")
        End Try
     
    End Sub

   
    Private Sub LinkLabel1_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel1.LinkClicked
        Dim webAddress As String = "https://wordtohtml.net"
        Process.Start(webAddress)
    End Sub
End Class