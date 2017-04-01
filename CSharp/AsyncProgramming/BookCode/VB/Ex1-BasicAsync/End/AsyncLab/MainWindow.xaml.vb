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

Class MainWindow

    Private Async Sub Start_Click(sender As Object, e As RoutedEventArgs) Handles startButton.Click
        Dim uris As New List(Of String)()

        startButton.IsEnabled = False

        Try
            Dim response = Await New HttpClient().GetAsync("http://msdn.microsoft.com")
            Dim result As String = Await response.Content.ReadAsStringAsync()

            textBox1.Text = result

            Dim mc As MatchCollection = Regex.Matches(result,
                "href\s*=\s*(?:\""(?<1>http://[^""]*)\"")", RegexOptions.IgnoreCase)
            For Each m As Match In mc
                uris.Add(m.Groups(1).Value)
            Next

            listBox1.ItemsSource = uris

        Catch ex As Exception
            MessageBox.Show(ex.ToString())
        End Try

        startButton.IsEnabled = True

    End Sub

End Class
