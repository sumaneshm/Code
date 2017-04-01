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
                                                      Return Await Task.WhenAll(
                                                                From uri In uris
                                                                  Select DownloadItemAsync(New Uri(uri), cancellationToken.Token))
                                                  End Function)
        Catch exCancel As OperationCanceledException
            MessageBox.Show("Operation Cancelled")
        Catch ex As Exception
            MessageBox.Show(ex.ToString())
        End Try

        startButton.IsEnabled = True

    End Sub

    Private Shared Async Function DownloadItemAsync(ByVal itemUri As Uri,
        ByVal cancellationToken As CancellationToken) As Task(Of LinkInfo)

        Dim item As String
        Try
            Dim httpClient As New HttpClient
            httpClient.MaxResponseContentBufferSize = 1000000

            Dim message = New HttpRequestMessage(HttpMethod.Get, itemUri)
            Dim response = Await httpClient.SendAsync(message, cancellationToken)
            item = Await response.Content.ReadAsStringAsync()
        Catch
            item = String.Empty
        End Try

        Dim linkInfo As LinkInfo = New LinkInfo() With {
            .Length = item.Length, .Html = item, .Title = GetTitle(item)}

        PollItem(itemUri, linkInfo, cancellationToken)

        Return linkInfo
    End Function

    Private Shared Function GetTitle(ByVal html As String) As String
        If (html.Length.Equals(0)) Then
            Return "Not Found"
        End If

        Dim m As Match = Regex.Match(html, "(?<=<title.*>)([\s\S]*)(?=</title>)",
          RegexOptions.IgnoreCase)

        Return m.Value
    End Function

    Private Shared Async Sub PollItem(ByVal itemUri As Uri,
        ByVal link As LinkInfo, ByVal cancellationToken As CancellationToken)

        Dim r As New Random()
        Dim httpClient As New HttpClient
        httpClient.MaxResponseContentBufferSize = 1000000

        Try
            Do
                Await Task.Delay(5000, cancellationToken)
                Dim requestMessage = New HttpRequestMessage(HttpMethod.Get, itemUri)
                Dim response = Await httpClient.SendAsync(requestMessage,
                    cancellationToken)
                Dim item As String = Await response.Content.ReadAsStringAsync()

                If item.Length <> link.Length Then
                    link.Title = GetTitle(item)
                    link.Length = item.Length
                    link.Html = item
                    link.Color = Color.FromArgb(Convert.ToByte(255),
                      Convert.ToByte(r.Next(256)), Convert.ToByte(r.Next(256)),
                      Convert.ToByte(r.Next(256)))
                End If
            Loop
        Catch
        End Try

    End Sub

    Private Sub cancelButton_Click(sender As Object, e As RoutedEventArgs) Handles cancelButton.Click
        cancellationToken.Cancel()
    End Sub

End Class
