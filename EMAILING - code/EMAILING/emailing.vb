Imports System.Net.Mail
Imports System.Text.RegularExpressions
Imports System.Data.OleDb
Public Class emailing
    Dim MyCommand As System.Data.OleDb.OleDbDataAdapter
    Dim c As pour_envoie() = Nothing
    Dim f As pour_envoie() = Nothing
    Dim temp As DataTable
    Private Sub Button2_Click(sender As Object, e As EventArgs)

    End Sub
    Public Sub filtre_email(champ As String, ByRef correct As pour_envoie(), ByRef fake As pour_envoie())

        Dim email As New Regex("^[_a-z0-9-]+(.[a-z0-9-]+)@[a-z0-9-]+(.[a-z0-9-]+)*(.[a-z]{2,4})$")
        correct = Nothing
        fake = Nothing
        Dim nc As Integer = 0
        Dim nf As Integer = 0
        Try
            MyCommand = New System.Data.OleDb.OleDbDataAdapter("select * from [" & champ & "$]", MyConnection)
            dt = New DataTable
            MyCommand.Fill(dt)
            Dim row As DataRow

            For Each row In dt.Rows
                If email.IsMatch(row(0).ToString) Then
                    Me.DataGridView1.Rows.Add(row(0).ToString, row(1).ToString, row(2).ToString)
                Else
                    Me.DataGridView2.Rows.Add(row(0).ToString, row(1).ToString, row(2).ToString)
                End If

            Next
            correct = Nothing
            For i = 0 To Me.DataGridView1.RowCount - 1
                ReDim Preserve correct(i)
                correct(i) = New pour_envoie(Me.DataGridView1.Rows(i).Cells(0).Value, Me.DataGridView1.Rows(i).Cells(1).Value, Me.DataGridView1.Rows(i).Cells(2).Value)
            Next
            For i = 0 To Me.DataGridView2.RowCount - 1
                ReDim Preserve fake(i)
                fake(i) = New pour_envoie(Me.DataGridView2.Rows(i).Cells(0).Value, Me.DataGridView2.Rows(i).Cells(1).Value, Me.DataGridView2.Rows(i).Cells(2).Value)
            Next
            If c IsNot Nothing Then
                Label4.Text = "number of emails : " & UBound(correct) + 1
            Else
                Label4.Text = "number of emails : 0"
            End If
            If f IsNot Nothing Then
                Label5.Text = "number of emails : " & UBound(fake) + 1
            Else
                Label5.Text = "number of emails : 0"
            End If
        Catch ex As Exception
            MsgBox("erreur de chargement")
        End Try
      
       

    End Sub
    Private Sub select_avec_filtre(champ As String)
        MyCommand = New System.Data.OleDb.OleDbDataAdapter("select * from [" & champ & "$]", MyConnection)
        Dim cp_erreur As Integer = 0
        dt = New DataTable
        MyCommand.Fill(dt)
        Dim row As DataRow
        Dim adresse As MailAddress
        For Each row In dt.Rows
            ' row.Delete()
            'adresse = New MailAddress(row(0).ToString)
            Dim email As New Regex("^[_a-z0-9-]+(.[a-z0-9-]+)@[a-z0-9-]+(.[a-z0-9-]+)*(.[a-z]{2,4})$")

            If email.IsMatch(row(0).ToString) Then

            Else
                cp_erreur = cp_erreur + 1
                row.Delete()

            End If
        Next

        Label5.Text = "number of emails : " & cp_erreur
        temp = dt
        DataGridView1.DataSource = dt

        MyCommand = New System.Data.OleDb.OleDbDataAdapter("select * from [" & champ & "$]", MyConnection)
        dt2 = New DataTable
        MyCommand.Fill(dt2)
        cp_erreur = 0
        For Each row In dt2.Rows
            ' row.Delete()
            ' adresse = New MailAddress(row(0).ToString)
            Dim email As New Regex("^[_a-z0-9-]+(.[a-z0-9-]+)@[a-z0-9-]+(.[a-z0-9-]+)*(.[a-z]{2,4})$")

            If email.IsMatch(row(0).ToString) Then
                row.Delete()
                cp_erreur = cp_erreur + 1
            Else


            End If
        Next
        Label4.Text = "number of emails : " & cp_erreur
        Me.DataGridView2.DataSource = dt2
    End Sub
    Private Sub emailing_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        open_connex()

    End Sub

    Private Sub ComboBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox1.SelectedIndexChanged
        Dim k As String = Me.ComboBox1.SelectedItem.ToString
        ' select_avec_filtre(k)
        Me.DataGridView1.Rows.Clear()
        Me.DataGridView2.Rows.Clear()

        filtre_email(k, c, f)
      

    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        Dim k As Envoyer_un_Email = New Envoyer_un_Email
        k.ShowDialog()

    End Sub

   
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Try
            Dim k As envoie_a_tous = New envoie_a_tous
            k.tab = c
            k.ShowDialog()
        Catch ex As Exception
            MsgBox("veuiller choisir une base donnee excel")
        End Try
    End Sub

    Private Sub Button2_Click_1(sender As Object, e As EventArgs) Handles Button2.Click
        Me.OpenFileDialog1.ShowDialog()

        Dim path As String = Me.OpenFileDialog1.FileName

        Try

            MyConnection = New System.Data.OleDb.OleDbConnection _
    ("provider=Microsoft.ACE.OLEDB.12.0;Data Source= " & path & "; Extended Properties=Excel 12.0;")

            Dim f As String() = tous_feuille(path, OpenFileDialog1.SafeFileName)
            Dim i As Integer
            If f IsNot Nothing Then
                Me.ComboBox1.Items.Clear()
                For i = 0 To f.Count() - 2
                    Me.ComboBox1.Items.Add(f(i))
                Next
                Me.ComboBox1.Text = f(0)

            Else
                MsgBox("feuille vide !!")
            End If

            '-------------------------------------------------------------------------------------------
            lbl_connex.Text = "(connection etabli avec succee !!)"
            '-------------------------------------------------------------------------------------------
        Catch ex As Exception
            MsgBox("erreur de connection au BDD")
        End Try
    End Sub

    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click
        Dim k As String = InputBox("donner un nom pur cette base de donnee :", "choisir un nom :")



        If k <> "" Then


            Dim succe As Integer = open_connex()
            If succe = 0 Then
                Dim sql As OleDbCommand
                Dim rs As Integer

                sql = db.CreateCommand()
                sql.CommandText = "INSERT INTO saves(nom) VALUES ('" & k & "')"
                Try

                    rs = sql.ExecuteNonQuery
                    If rs <> 0 Then
                        Dim i As Integer
                        For i = 0 To UBound(c)
                            sql.CommandText = "INSERT INTO contenu(adr,nom,prenom,id_saves) VALUES ('" & Me.DataGridView1.Rows(i).Cells(0).Value & "','" & Me.DataGridView1.Rows(i).Cells(1).Value & "','" & Me.DataGridView1.Rows(i).Cells(2).Value & "','" & k & "')"
                            rs = sql.ExecuteNonQuery

                        Next
                        MsgBox("sauvegarde avec succee !! ")
                    Else
                        MsgBox("echec sauvegarder !! ")
                    End If


                    MsgBox("verrifier que la base de donnee existe !!!!")
                Catch ex As Exception
                    MsgBox("nom de Sauvegare existe deja !!")


                   
                End Try

                db.Close()
            End If
        End If
    End Sub

    Private Sub DataGridView1_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView1.CellContentClick

    End Sub

    Private Sub DataGridView2_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView2.CellContentClick

    End Sub
End Class