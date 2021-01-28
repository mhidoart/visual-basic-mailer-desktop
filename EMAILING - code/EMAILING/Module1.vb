Imports System.Data.OleDb
Imports Microsoft.Office.Interop



Imports System.Net
Imports System.Net.Mail

Module Module1
    Public SmtpServer As New SmtpClient()
    Public mail As MailMessage
    Public MyConnection As OleDbConnection
    Public dt2 As DataTable
    Public dt As DataTable
    Public connectionString As String
    Public db As OleDbConnection
    Public Function tous_feuille(source, name) As String()
        Dim Fichier As Excel.Application
        Dim FichierTest As Excel.Workbook
        Dim i As Integer
        Dim Temp As String = Nothing
        Try

            Fichier = CreateObject("Excel.Application")
            Fichier.Visible = False
            FichierTest = Fichier.Workbooks.Open(source) 'Chemin du fichier 

            Temp = ""

            For i = 1 To FichierTest.Sheets.Count
                Temp = Temp & FichierTest.Sheets(i).Name & ","
            Next i

            Fichier.Workbooks(name).Close()
            Fichier = Nothing
            Dim tab As String() = Temp.Split(",")
            Return tab
        Catch ex As Exception
            MsgBox("veuller choisir une bdd")
        End Try



    End Function
    Public Function open_connex() As Integer
        Dim cp As Integer
        connectionString = "Provider=Microsoft.ACE.OLEDB.12.0;  Data Source=C:\bdd comptes\bdd.accdb"
        Try
            db = New OleDbConnection(connectionString)
            db.Open()
            cp = 0
        Catch ex As Exception
            MsgBox("erreur de connexion ou base donnee n'exite pas verifier: C:\bdd comptes\bdd.accdb ")
            cp = 1
        End Try
        Return cp
    End Function

    Public Function charger_email() As comptes()
        Dim t As comptes() = Nothing
        Dim n As Integer = 0
        Dim succe As Integer = open_connex()
        If succe = 0 Then
            Dim sql As OleDbCommand
            Dim rs As OleDbDataReader
            Try
                sql = db.CreateCommand()
                sql.CommandText = "SELECT * FROM comptes"
                rs = sql.ExecuteReader()
          
                While (rs.Read())
                    ReDim Preserve t(n)
                 
                    t(n) = New comptes(rs(0), rs(1), rs(2))
                    t(n).pass = base64Decode(t(n).pass)
                    n = n + 1
                End While
            Catch ex As Exception
                MsgBox("echec de chargement des email depuis la base de donnee !!!!")
            End Try
            db.Close()
        End If


        Return t
    End Function

    Public Function chercher_email(id As Integer) As comptes
        Dim t As comptes = Nothing

        Dim succe As Integer = open_connex()
        If succe = 0 Then
            Dim sql As OleDbCommand
            Dim rs As OleDbDataReader
            Try
                sql = db.CreateCommand()
                sql.CommandText = "SELECT * FROM comptes WHERE id = '" & id & "'"
                rs = sql.ExecuteReader()
                While (rs.Read())
                    t = New comptes(rs(0), rs(1), rs(2))
                End While
            Catch ex As Exception
                MsgBox("echec de chargement des email depuis la base de donnee !!!!")
            End Try
            db.Close()
        End If


        Return t
    End Function

    Public Sub supprimer_save(name As String)
        open_connex()

        Dim sql As OleDbCommand
        Try
            'supprimer le contenu
            sql = db.CreateCommand()
            sql.CommandText = "DELETE * FROM contenu WHERE id_saves = '" & name & "'"
            sql.ExecuteNonQuery()

            'supprimer depuis saves
            sql = db.CreateCommand()
            sql.CommandText = "DELETE * FROM saves WHERE nom = '" & name & "'"
            sql.ExecuteNonQuery()

        Catch ex As Exception
            MsgBox("echec de suppression du sauvegarde !!!!")
        End Try
        db.Close()
    End Sub
    Public Function envoyer_un_mail(expediteur As String, pass As String, subject As String, body As String, destination As String, type_envoi As Integer) As Integer
        Dim cp As Integer

        Try

            
            mail = New MailMessage()
            mail.From = New MailAddress(expediteur)
            mail.To.Add(destination)
            mail.Subject = subject
            mail.Body = body
            If type_envoi = 0 Then
                mail.IsBodyHtml = True
            Else 
                mail.IsBodyHtml = False
            End If

            SmtpServer.Send(mail)
            cp = 0
        Catch ex As Exception
            cp = 1
        End Try



        Return cp
    End Function

    Public Function base64Encode(ByVal sData As String) As String

        Try
            Dim encData_Byte As Byte() = New Byte(sData.Length - 1) {}
            encData_Byte = System.Text.Encoding.UTF8.GetBytes(sData)
            Dim encodedData As String = Convert.ToBase64String(encData_Byte)
            Return (encodedData)

        Catch ex As Exception

            Throw (New Exception("Error is base64Encode" & ex.Message))

        End Try


    End Function

    Public Function base64Decode(ByVal sData As String) As String

        Dim encoder As New System.Text.UTF8Encoding()
        Dim utf8Decode As System.Text.Decoder = encoder.GetDecoder()
        Dim todecode_byte As Byte() = Convert.FromBase64String(sData)
        Dim charCount As Integer = utf8Decode.GetCharCount(todecode_byte, 0, todecode_byte.Length)
        Dim decoded_char As Char() = New Char(charCount - 1) {}
        utf8Decode.GetChars(todecode_byte, 0, todecode_byte.Length, decoded_char, 0)
        Dim result As String = New [String](decoded_char)
        Return result

    End Function
End Module
