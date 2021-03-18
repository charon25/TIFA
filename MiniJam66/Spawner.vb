Imports GameMaker

Public Class SpawnerClass

    Private rand As Random

    Private CandyTimeLeft As Integer
    Private VegetableTimeLeft As Integer
    Private SpawnedCandyCount, SpawnedVegetableCount As Integer
    Private HasAddedNewSpawner As Boolean

    Public Function GetSpawnedProjectilesCount() As Integer
        Return SpawnedCandyCount + SpawnedVegetableCount
    End Function

    Public Sub New(rand As Random)
        Me.rand = rand
        ResetCandyTimer(True)
        Me.SpawnedCandyCount = 0
        Me.SpawnedVegetableCount = 0
        Me.HasAddedNewSpawner = False
    End Sub

    Private Sub ResetCandyTimer(Optional start As Boolean = False)
        If start Then
            CandyTimeLeft = rand.Next(CO.CANDY_COOLDOWN_AVERAGE - CO.CANDY_COOLDOWN_DEVIATION, CO.CANDY_COOLDOWN_AVERAGE + CO.CANDY_COOLDOWN_DEVIATION + 1) + rand.Next(-CO.CANDY_COOLDOWN_AVERAGE / 2, 0)
        Else
            CandyTimeLeft = rand.Next(CO.CANDY_COOLDOWN_AVERAGE - CO.CANDY_COOLDOWN_DEVIATION, CO.CANDY_COOLDOWN_AVERAGE + CO.CANDY_COOLDOWN_DEVIATION + 1)
        End If
    End Sub
    Private Sub ResetVegetableTimer()
        VegetableTimeLeft = rand.Next(CO.VEGETABLE_COOLDOWN_AVERAGE - CO.VEGETABLE_COOLDOWN_DEVIATION, CO.VEGETABLE_COOLDOWN_AVERAGE + CO.VEGETABLE_COOLDOWN_DEVIATION + 1)
    End Sub
    Private Function SpawnCandy() As CandyClass
        Dim L As Double = 2 * (CO.WIDTH + CO.HEIGHT)
        Dim startingPoint As Double = CO.NextDouble(rand, 0, L)
        Dim I As Integer = Math.Floor(4 * startingPoint / L)
        Dim endingPoint As Double = CO.NextDouble(rand, 0, L / 4)
        startingPoint = startingPoint Mod (L / 4)

        Dim startingLocation As Vector = Vector.NullVector()
        Dim endingLocation As Vector = Vector.NullVector()
        Select Case I
            Case 0
                startingLocation = New Vector(startingPoint, -CO.CANDY_HEIGHT)
                endingLocation = New Vector(endingPoint, CO.HEIGHT)
            Case 1
                startingLocation = New Vector(CO.WIDTH + CO.CANDY_WIDTH, startingPoint)
                endingLocation = New Vector(0, endingPoint)
            Case 2
                startingLocation = New Vector(startingPoint, CO.HEIGHT + CO.CANDY_HEIGHT)
                endingLocation = New Vector(endingPoint, 0)
            Case 3
                startingLocation = New Vector(-CO.CANDY_WIDTH, startingPoint)
                endingLocation = New Vector(CO.WIDTH, endingPoint)
        End Select

        Return New CandyClass(startingLocation, endingLocation, rand.Next(CO.CANDY_TYPE_COUNT))
    End Function
    Private Function SpawnVegetable() As VegetableClass
        Dim L As Double = 2 * (CO.WIDTH + CO.HEIGHT)
        Dim startingPoint As Double = CO.NextDouble(rand, 0, L)
        Dim I As Integer = Math.Floor(4 * startingPoint / L)
        Dim endingPoint As Double = CO.NextDouble(rand, 0, L / 4)
        startingPoint = startingPoint Mod (L / 4)

        Dim startingLocation As Vector = Vector.NullVector()
        Dim endingLocation As Vector = Vector.NullVector()
        Select Case I
            Case 0
                startingLocation = New Vector(startingPoint, -CO.VEGETABLE_HEIGHT)
                endingLocation = New Vector(endingPoint, CO.HEIGHT)
            Case 1
                startingLocation = New Vector(CO.WIDTH + CO.VEGETABLE_WIDTH, startingPoint)
                endingLocation = New Vector(0, endingPoint)
            Case 2
                startingLocation = New Vector(startingPoint, CO.HEIGHT + CO.VEGETABLE_HEIGHT)
                endingLocation = New Vector(endingPoint, 0)
            Case 3
                startingLocation = New Vector(-CO.VEGETABLE_WIDTH, startingPoint)
                endingLocation = New Vector(CO.WIDTH, endingPoint)
        End Select

        Return New VegetableClass(startingLocation, endingLocation, rand.Next(CO.CANDY_TYPE_COUNT))
    End Function

    Public Sub Age()
        CandyTimeLeft -= 1
        If CandyTimeLeft <= 0 Then
            ResetCandyTimer()
            Form1.CreateCandy(SpawnCandy())
            SpawnedCandyCount += 1
            HasAddedNewSpawner = False
        End If
        VegetableTimeLeft -= 1
        If VegetableTimeLeft <= 0 AndAlso SpawnedCandyCount >= CO.CANDY_COUNT_BEFORE_VEGETABLES Then
            ResetVegetableTimer()
            Form1.CreateVegetable(SpawnVegetable())
            SpawnedVegetableCount += 1
            HasAddedNewSpawner = False
        End If
    End Sub
    Public Function CanAddNewSpawner() As Boolean
        If Not HasAddedNewSpawner AndAlso GetSpawnedProjectilesCount() > 0 AndAlso GetSpawnedProjectilesCount() Mod CO.COUNT_BEFORE_NEW_SPAWNER = 0 Then
            HasAddedNewSpawner = True
            Return True
        End If
        Return False
    End Function

End Class
