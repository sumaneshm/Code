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

Class MainWindow

    Private Async Sub Start_Click(sender As Object, e As RoutedEventArgs) Handles startButton.Click
        Dim uris As New ObservableCollection(Of String)()

        startButton.IsEnabled = False

        Try
            Dim response = Await New HttpClient().GetAsync("http://msdn.microsoft.com")
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

            listBox1.ItemsSource = Await Task.WhenAll( _
              From uri In uris _
                Select DownloadItemAsync(New Uri(uri)))
        Catch ex As Exception
            MessageBox.Show(ex.ToString())
        End Try

        startButton.IsEnabled = True

    End Sub

    Private Shared Async Function DownloadItemAsync(
        ByVal itemUri As Uri) As Task(Of LinkInfo)

        Dim item As String
        Try
            Dim httpClient As New HttpClient
            httpClient.MaxResponseContentBufferSize = 1000000

            Dim response = Await httpClient.GetAsync(itemUri)
            item = Await response.Content.ReadAsStringAsync()
        Catch
            item = String.Empty
        End Try

        Dim linkInfo As LinkInfo = New LinkInfo() With {
            .Length = item.Length, .Html = item, .Title = GetTitle(item)}

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


End Class
