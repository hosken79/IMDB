Imports System.Net
Imports System.Net.WebClient
Imports System.IO
Imports System.IO.Compression

Public Class Form1
    Private Sub bt_start_Click(sender As Object, e As EventArgs) Handles bt_start.Click
        bt_start.Enabled = False
        BW1.RunWorkerAsync()
    End Sub

    Private Sub setLabelTxt(ByVal text As String, ByVal lblStatus As Label)
        If lblStatus.InvokeRequired Then
            lblStatus.Invoke(New setLabelTxtInvoker(AddressOf setLabelTxt), text, lblStatus)
        Else
            lblStatus.Text = text
        End If
    End Sub
    Private Delegate Sub setLabelTxtInvoker(ByVal text As String, ByVal lblStatus As Label)

    Private Sub CreateOutputList(ByVal StringData As String)
        Dim strFile As String = Path.GetDirectoryName(Application.ExecutablePath) + "\List.txt"
        Dim sw As StreamWriter
        Try
            If (Not File.Exists(strFile)) Then
                sw = File.CreateText(strFile)
                sw.WriteLine(StringData)
            Else
                sw = File.AppendText(strFile)
                sw.WriteLine(StringData)
            End If

            sw.Close()
        Catch ex As IOException
            MsgBox("Error writing to log file.")
        End Try
    End Sub

    Private Function DownloadFile(ByVal webaddress As String, ByVal fileName As String) As Boolean
        DownloadFile = False
        Try

            If File.Exists(Path.GetDirectoryName(Application.ExecutablePath) + "\" + fileName) Then
                File.Delete(Path.GetDirectoryName(Application.ExecutablePath) + "\" + fileName)
            End If

            Using client As New WebClient()
                client.DownloadFile(webaddress, fileName)
            End Using

            If File.Exists(Path.GetDirectoryName(Application.ExecutablePath) + "\" + fileName) Then
                DownloadFile = True
            End If

        Catch ex As Exception
            MsgBox("Failed to Download File")
        End Try
    End Function

    Private Function ExtractFile(ByVal FilePath As String, ByVal fileName As String) As Boolean
        ExtractFile = False
        Try
            If File.Exists(FilePath + "output.txt") Then
                File.Delete(FilePath + "output.txt")
            End If

            DecompressFile(FilePath + fileName, FilePath + "output.txt")


            If File.Exists(FilePath + "output.txt") Then
                ExtractFile = True
            End If

        Catch ex As Exception
            MsgBox("Failed to Extract File")
        End Try
    End Function

    Public Sub DecompressFile(ByVal sourceFile As String, ByVal destinationFile As String)

        ' make sure the source file is there
        If File.Exists(sourceFile) = False Then
            Throw New FileNotFoundException
        End If

        ' Create the streams and byte arrays needed
        Dim sourceStream As FileStream = Nothing
        Dim destinationStream As FileStream = Nothing
        Dim decompressedStream As GZipStream = Nothing
        Dim quartetBuffer As Byte() = Nothing

        Try
            ' Read in the compressed source stream
            sourceStream = New FileStream(sourceFile, FileMode.Open)

            ' Create a compression stream pointing to the destiantion stream
            decompressedStream = New GZipStream(sourceStream, CompressionMode.Decompress, True)

            ' Read the footer to determine the length of the destiantion file
            quartetBuffer = New Byte(4) {}
            Dim position As Integer = CType(sourceStream.Length, Integer) - 4
            sourceStream.Position = position
            sourceStream.Read(quartetBuffer, 0, 4)
            sourceStream.Position = 0
            Dim checkLength As Integer = BitConverter.ToInt32(quartetBuffer, 0)

            Dim buffer(checkLength + 100) As Byte
            Dim offset As Integer = 0
            Dim total As Integer = 0

            ' Read the compressed data into the buffer
            While True
                Dim bytesRead As Integer = decompressedStream.Read(buffer, offset, 100)
                If bytesRead = 0 Then
                    Exit While
                End If
                offset += bytesRead
                total += bytesRead
            End While

            ' Now write everything to the destination file
            destinationStream = New FileStream(destinationFile, FileMode.Create)
            destinationStream.Write(buffer, 0, total)

            ' and flush everyhting to clean out the buffer
            destinationStream.Flush()

        Catch ex As ApplicationException
            MessageBox.Show(ex.Message, "An Error occured during compression", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            ' Make sure we allways close all streams
            If Not (sourceStream Is Nothing) Then
                sourceStream.Close()
            End If
            If Not (decompressedStream Is Nothing) Then
                decompressedStream.Close()
            End If
            If Not (destinationStream Is Nothing) Then
                destinationStream.Close()
            End If
        End Try

    End Sub

    Private Sub BW1_DoWork(sender As Object, e As System.ComponentModel.DoWorkEventArgs) Handles BW1.DoWork

        Dim remoteUri As String = "https://datasets.imdbws.com/name.basics.tsv.gz"
        Dim fileName As String = "name.basics.tsv.gz"
        Dim OkToContinue As Boolean = False

        setLabelTxt("Downloading File", lblStatus)
        OkToContinue = DownloadFile(remoteUri, fileName)
        If OkToContinue = True Then
            setLabelTxt("Extracting Data", lblStatus)
            OkToContinue = ExtractFile(Path.GetDirectoryName(Application.ExecutablePath) + "\", fileName)

            If OkToContinue = True Then
                setLabelTxt("Loading Data", lblStatus)
                Me.Refresh()

                If File.Exists(Path.GetDirectoryName(Application.ExecutablePath) + "\List.txt") Then
                    File.Delete(Path.GetDirectoryName(Application.ExecutablePath) + "\List.txt")
                End If
                For Each line As String In System.IO.File.ReadAllLines(Path.GetDirectoryName(Application.ExecutablePath) + "\" + "output.txt")
                    Dim fields() As String = line.Split(vbTab)
                    If fields(4).Length > 7 Then
                        If RTrim(LTrim(fields(4))).Substring(0, 8) = "producer" Then
                            If (RTrim(LTrim(fields(3))) = "\N") Then
                                CreateOutputList(fields(0))
                            End If
                        End If
                    End If
                Next
            End If
        End If


        setLabelTxt("Finished", lblStatus)


    End Sub

    Private Sub BW1_RunWorkerCompleted(sender As Object, e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles BW1.RunWorkerCompleted
        bt_start.Enabled = True
        If MsgBox("List.txt Contains Results Do you want to open file ?", vbYesNo) = MsgBoxResult.Yes Then
            System.Diagnostics.Process.Start(Path.GetDirectoryName(Application.ExecutablePath) + "\List.txt")
        End If
    End Sub
End Class
