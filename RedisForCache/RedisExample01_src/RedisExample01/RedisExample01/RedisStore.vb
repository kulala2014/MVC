Imports ServiceStack.Redis
''' <summary>
''' The redis store encapsulation class around the ServiceStack redis client 
''' </summary>
''' <remarks>This class is cumulatively constructed across the tutorial and is not broken.</remarks>
Public Class RedisStore

#Region " Properties "

    Private _sourceClient As RedisClient
    Public ReadOnly Property SourceClient() As RedisClient
        Get
            Return _sourceClient
        End Get
    End Property

#End Region

#Region " Constructors "

    Public Sub New()
        MyClass.New(False)
    End Sub

    Public Sub New(ByVal ForceCheckServer As Boolean)
        _sourceClient = New RedisClient
        If ForceCheckServer AndAlso Not IsServerAlive() Then
            Throw New Exception("The server has not been started!")
        End If
    End Sub

#End Region

    Public Function IsServerAlive() As Boolean
        Try
            Return SourceClient.Ping
        Catch ex As Exception
            Return False
        End Try
    End Function

#Region " Functionalities "

#Region " Get/Set Keys "

    Public Function SetKey(ByVal key As String, ByVal value As String) As Boolean
        Return SourceClient.Set(key, value)
    End Function

    Public Function SetKey(Of T)(ByVal key As String, ByVal value As T) As Boolean
        Return SourceClient.Set(Of T)(key, value)
    End Function

    Public Function GetKey(ByVal key As String) As String
        Return Helper.GetString(SourceClient.Get(key))
    End Function

    Public Function GetKey(Of T)(ByVal key As String) As T
        Return SourceClient.Get(Of T)(key)
    End Function

#End Region

#End Region

End Class

Public Class Helper

    Private Shared ReadOnly UTF8EncObj As New System.Text.UTF8Encoding()

    Public Shared Function GetBytes(ByVal source As Object) As Byte()
        Return UTF8EncObj.GetBytes(source)
    End Function

    Public Shared Function GetString(ByVal sourceBytes As Byte()) As String
        Return UTF8EncObj.GetString(sourceBytes)
    End Function

End Class