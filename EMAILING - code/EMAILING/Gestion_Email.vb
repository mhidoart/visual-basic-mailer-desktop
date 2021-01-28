Imports System.Text.RegularExpressions
Imports System.Data.OleDb

Public Class Gestion_Email
    Dim t As comptes()
    Private Sub Gestion_Email_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.TextBox3.Visible = False
        clear()
    End Sub

    Private Sub clear()
        Me.TextBox3.Text = ""
        Me.TextBox1.Text = ""
        Me.TextBox2.Text = ""
        Me.DataGridView1.Rows.Clear()
        t = charger_email()
        Dim i As Integer
        If t Is Nothing Then
            Me.Label1.Text = ("base donne vide ou n'existe pas !!")
        Else
            Try
                For i = 0 To UBound(t)
                    Me.DataGridView1.Rows.Add(t(i).adr)
                Next
            Catch ex As Exception

            End Try

        End If

    End Sub




    Private Sub DataGridView1_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView1.CellClick
        Dim index As Integer = e.RowIndex
        Dim elem As String = Me.DataGridView1.Rows(index).Cells(0).ToString
        Me.TextBox1.Text = t(index).adr
        Me.TextBox2.Text = t(index).pass
        Me.TextBox3.Text = t(index).id

    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        If Me.TextBox3.Text = "" Then
            Dim email As New Regex("^[_a-z0-9-]+(.[a-z0-9-]+)@[a-z0-9-]+(.[a-z0-9-]+)*(.[a-z]{2,4})$")
            If email.IsMatch(Me.TextBox1.Text) And Me.TextBox2.Text <> "" Then
                Dim succe As Integer = open_connex()
                If succe = 0 Then
                    Dim sql As OleDbCommand
                    Dim rs As Integer
                    Try
                        sql = db.CreateCommand()

                     
                        sql.CommandText = "INSERT INTO comptes(adr,pass) VALUES ('" & Me.TextBox1.Text & "','" & base64Encode(Me.TextBox2.Text) & "')"
                        rs = sql.ExecuteNonQuery
                        If rs <> 0 Then
                            MsgBox("ajout avec succee !! ")

                        Else
                            MsgBox("echec d ajout !! ")
                        End If
                    Catch ex As Exception
                        MsgBox("verrifier que la base de donnee existe !!!!")
                    End Try

                    db.Close()
                    clear()
                End If
            Else
                MsgBox("syntax erronee !!")
            End If
        Else
            MsgBox("il faut d ''abord clicker sur clear avant l''ajout")
        End If
      

    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        Dim succe As Integer = open_connex()
        If succe = 0 Then
            Dim sql As OleDbCommand
            Dim rs As Integer
            Try
                sql = db.CreateCommand()
                sql.CommandText = "DELETE FROM comptes WHERE id=" & Val(Me.TextBox3.Text)
                rs = sql.ExecuteNonQuery
                If rs <> 0 Then
                    MsgBox("suppression avec succee !! ")

                Else
                    MsgBox("echec de la suppression !! ")
                End If
            Catch ex As Exception
                MsgBox("verrifier que la base de donnee existe !!!!")
            End Try

            db.Close()
            clear()
        End If
       
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        clear()
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Dim email As New Regex("^[_a-z0-9-]+(.[a-z0-9-]+)@[a-z0-9-]+(.[a-z0-9-]+)*(.[a-z]{2,4})$")
        If email.IsMatch(Me.TextBox1.Text) And Me.TextBox2.Text <> "" Then
            Dim succe As Integer = open_connex()
            If succe = 0 Then
                Dim sql As OleDbCommand
                Dim rs As Integer


                Try
                    sql = db.CreateCommand()

                    Dim byt As Byte() = System.Text.Encoding.UTF8.GetBytes(TextBox1.Text)

                    sql.CommandText = "UPDATE comptes SET adr='" & Me.TextBox1.Text & "', pass='" & base64Encode(Me.TextBox2.Text) & "' WHERE [id]=" & Val(Me.TextBox3.Text)
                    rs = sql.ExecuteNonQuery
                    If rs <> 0 Then
                        MsgBox("Mise a jour  avec succee !! ")

                    Else
                        MsgBox("echec de la mise a jour !! ")
                    End If
                Catch ex As Exception
                    MsgBox("verrifier que la base de donnee existe !!!!")
                End Try

                db.Close()
                clear()
            End If
        Else
            MsgBox("syntax  erronee !!")
        End If
    End Sub


    Private Sub PictureBox2_Click(sender As Object, e As EventArgs) Handles PictureBox2.Click

    End Sub
End Class