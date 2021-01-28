Imports System.Data.OleDb
Public Class sauvegardes
    Dim sql As OleDbCommand
    Dim t As pour_envoie()
    Private Sub sauvegardes_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        remplir_les_datagrides()
    End Sub
    Private Sub remplir_les_datagrides()
        Try
            Me.DataGridView1.Rows.Clear()
            Me.DataGridView2.Rows.Clear()
        Catch ex As Exception

        End Try
        Dim rs As OleDbDataReader
        open_connex()
        sql = db.CreateCommand()
        sql.CommandText = "SELECT * FROM saves"
        rs = sql.ExecuteReader()
        While (rs.Read())
            Me.DataGridView1.Rows.Add(rs(0))
        End While
        db.Close()
        Label2.Text = "Nb sauvegardes : " & Me.DataGridView1.RowCount
    End Sub
    Private Sub contenu1(nom As String)
        open_connex()
        Dim MyCommand As New System.Data.OleDb.OleDbDataAdapter("SELECT * FROM contenu WHERE id_saves ='" & nom & "'", db)
        dt = New DataTable
        MyCommand.Fill(dt)
        Me.DataGridView2.DataSource = dt
        db.Close()
        Me.Label4.Text = "nb email : " & dt.Rows.Count
    End Sub
   


    Private Sub DataGridView1_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView1.CellClick
        Try
            contenu1(Me.DataGridView1.Rows(e.RowIndex).Cells(e.ColumnIndex).Value)

        Catch ex As Exception

        End Try
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim c As pour_envoie() = Nothing
        Dim i As Integer
        Try
            For i = 0 To Me.DataGridView2.RowCount - 1
                ReDim Preserve c(i)
                c(i) = New pour_envoie(Me.DataGridView2.Rows(i).Cells(1).Value, Me.DataGridView2.Rows(i).Cells(2).Value, Me.DataGridView2.Rows(i).Cells(3).Value)

            Next

            Dim k As envoie_a_tous = New envoie_a_tous
            k.tab = c
            k.ShowDialog()
        Catch ex As Exception
            MsgBox("veuiller choisir une base donnee excel")
        End Try
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click

        supprimer_save(Me.DataGridView1.CurrentCell.Value)
        remplir_les_datagrides()
    End Sub
End Class