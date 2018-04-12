Module MainModule

    Private MyRedisStore As RedisStore

    Sub Main()
        MyRedisStore = New RedisStore(True)
        Dim storedKey As String = "key:AUserInput"
        Console.WriteLine("Enter a string: ")
        Dim userInput As String = Console.ReadLine
        If MyRedisStore.SetKey(storedKey, userInput) Then
            Console.WriteLine("Key stored successfully!")
        Else
            Console.WriteLine("Key storing failed!!")
            EndMe()
        End If
        Console.WriteLine("Press any key to retreive the data from the store...")
        Console.ReadKey()

        Console.WriteLine("Key {0} retreived successfully. Value is {1}", storedKey, MyRedisStore.GetKey(storedKey))
        EndMe()
    End Sub

    Private Sub EndMe()
        Console.Write("Press any key to exit...")
        Console.ReadKey()
        End
    End Sub

End Module
