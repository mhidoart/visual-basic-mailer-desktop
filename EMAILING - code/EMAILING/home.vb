
Imports System.Net

Public Class home

    Private Sub home_Load(sender As Object, e As EventArgs) Handles MyBase.Load
    
    End Sub

    Private Sub TestToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles TestToolStripMenuItem.Click
        Dim emailing_form As emailing = New emailing
        emailing_form.Show()

    End Sub

   
    Private Sub FilesToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles FilesToolStripMenuItem.Click
        Dim k As Gestion_Email = New Gestion_Email
        k.ShowDialog()

    End Sub

   

    Private Sub EnvoToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles EnvoToolStripMenuItem.Click
        sauvegardes.Show()
    End Sub
End Class
