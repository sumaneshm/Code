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

Imports System.ComponentModel
Imports System.Windows.Media

Public Class LinkInfo
  Implements INotifyPropertyChanged

  Private mHtml As String
  Private mTitle As String
  Private mLength As Integer
  Private mColor As Color

  Public Event PropertyChanged(sender As Object, 
    e As PropertyChangedEventArgs) _
    Implements INotifyPropertyChanged.PropertyChanged

  Public Property Html() As String
    Get
      Return mHtml
    End Get
    Set(ByVal value As String)
      mHtml = value
      NotifyPropertyChanged("Html")
    End Set
  End Property

  Public Property Title() As String
    Get
      Return mTitle
    End Get
    Set(ByVal value As String)
      mTitle = value
      NotifyPropertyChanged("Title")
    End Set
  End Property

  Public Property Length() As Integer
    Get
      Return mLength
    End Get
    Set(ByVal value As Integer)
      mLength = value
      NotifyPropertyChanged("Length")
    End Set
  End Property

  Public Property Color() As Color
    Get
      Return mColor
    End Get
    Set(ByVal value As Color)
      mColor = value
      NotifyPropertyChanged("Color")
    End Set
  End Property

  Private Sub NotifyPropertyChanged(propertyName As String)
    RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(propertyName))
  End Sub
End Class
