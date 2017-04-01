' ----------------------------------------------------------------------------------
' Microsoft Developer & Platform Evangelism
' 
' Copyright (c) Microsoft Corporation. All rights reserved.
' 
' THIS CODE AND INFORMATION ARE PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND, 
' EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE IMPLIED WARRANTIES 
' OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE.
' ----------------------------------------------------------------------------------
' The example companies, organizations, products, domain names,
' e-mail addresses, logos, people, places, and events depicted
' herein are fictitious.  No association with any real company,
' organization, product, domain name, email address, logo, person,
' places, or events is intended or should be inferred.
' ----------------------------------------------------------------------------------

Imports System.Net.Http
Imports System.Text.RegularExpressions
Imports System.Collections.ObjectModel
Imports System.Threading
Imports AsyncLabLibrary

Class MainWindow

    Private cancellationToken As CancellationTokenSource

    Private Async Sub Start_Click(sender As Object, e As RoutedEventArgs) Handles startButton.Click
        cancellationToken = New CancellationTokenSource()

        Dim uris As New ObservableCollection(Of String)()

        startButton.IsEnabled = False

        Try
            Dim message = New HttpRequestMessage(HttpMethod.Get,
                "http://msdn.microsoft.com")
            Dim response = Await (New HttpClient()).SendAsync(message,
                cancellationToken.Token)
            Dim result As String = Await response.Content.ReadAsStringAsync()

            textBox1.Text = result

            Await Task.Run(Sub()
                               Dim mc As MatchCollection = Regex.Matches(result,
                                   "href\s*=\s*(?:\""(?<1>http://[^""]*)\"")",
                                   RegexOptions.IgnoreCase)
                               For Each m As Match In mc
                                   uris.Add(m.Groups(1).Value)
                               Next
                           End Sub)

            listBox1.ItemsSource = Await Task.Run(Async Function()
                                                      Return Await Task.WhenAll( _
                                                          From uri In uris _
                                                            Select Downloader.DownloadItemAsync(New Uri(uri), cancellationToken.Token))
                                                  End Function)
        Catch exCancel As OperationCanceledException
            MessageBox.Show("Operation Cancelled")
        Catch ex As Exception
            MessageBox.Show(ex.ToString())
        End Try

        startButton.IsEnabled = True

    End Sub

    Private Sub cancelButton_Click(sender As Object, e As RoutedEventArgs) Handles cancelButton.Click
        cancellationToken.Cancel()
    End Sub

End Class
