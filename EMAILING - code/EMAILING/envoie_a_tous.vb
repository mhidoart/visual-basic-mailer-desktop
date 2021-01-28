Public Class envoie_a_tous
    Public tab As pour_envoie()
    Private Sub envoie_a_tous_Load(sender As Object, e As EventArgs) Handles MyBase.Load

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
        Me.Button2.Visible = False


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
        SmtpServer.Credentials = New Net.NetworkCredential(t(id).adr, t(id).pass)
        SmtpServer.Port = 587
        SmtpServer.Host = "smtp.gmail.com" 'l'host c'est google.
        SmtpServer.EnableSsl = True
        Dim cp_succee As Integer = 0
        Dim cp_error As Integer = 0
        If tab IsNot Nothing Then
            ProgressBar1.Minimum = 0
            ProgressBar1.Maximum = UBound(tab)

            For i = 0 To UBound(tab)
                ProgressBar1.Value = i
                Dim succee As Integer
                If Me.TabControl1.SelectedIndex = 0 Then

                    succee = envoyer_un_mail(t(id).adr, t(id).pass, Me.TextBox4.Text, Me.TabControl1.SelectedTab.Controls.Item(0).Text, tab(i).adr, 0)

                Else
                    succee = envoyer_un_mail(t(id).adr, t(id).pass, Me.TextBox4.Text, Me.TabControl1.SelectedTab.Controls.Item(0).Text, tab(i).adr, 1)

                End If
                If succee = 0 Then

                    cp_succee = cp_succee + 1
                Else
                    cp_error = cp_error + 1
                End If
            Next
            Me.Button2.Visible = True
            MsgBox("fin d\'envoie avec " & cp_succee & " Succee et " & cp_error & " Erreurs")

        End If

        Me.Close()

    End Sub

   
  
   
End Class