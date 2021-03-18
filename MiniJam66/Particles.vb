Imports GameMaker

Public Class ParticleClass

    Private Location As Vector
    Private Velocity As Vector
    Private TimeLeft As Integer
    Private Destroyed As Boolean
    Protected Sprite As Bitmap

    Public Function GetLocationPoint() As Point
        Return Location.ToPoint()
    End Function
    Public Function IsDestroyed() As Boolean
        Return Destroyed
    End Function

    Sub New(location As Vector, velocity As Vector, cooldown As Integer, sprite As Bitmap)
        Me.Location = location
        Me.Velocity = velocity
        Me.TimeLeft = cooldown
        Me.Sprite = sprite
        Me.Destroyed = False
    End Sub

    Public Sub Age()
        Location += Velocity
        TimeLeft -= 1
        Destroyed = (TimeLeft <= 0)
    End Sub

    Public Function GetSprite() As Bitmap
        Return Sprite
    End Function
    

    'METHODE STATIQUE

    Public Shared Function CreateCandyParticle(origin As Vector, rand As Random) As ParticleClass
        Dim vx As Double = CO.NextDouble(rand, -1, 1)
        Dim velocity As Vector
        If rand.Next(2) = 0 Then
            velocity = New Vector(vx, -Math.Sqrt(1 - vx * vx))
        Else
            velocity = New Vector(vx, +Math.Sqrt(1 - vx * vx))
        End If
        Dim size As Integer = rand.Next(3)
        Return New ParticleClass(origin, velocity * CO.NextDouble(rand, CO.PARTICLE_SPEED_MIN, CO.PARTICLE_SPEED_MAX), rand.Next(CO.PARTICLE_DURATION_MIN, CO.PARTICLE_DURATION_MAX + 1), Form1.Images(size + 4))
    End Function
    Public Shared Function CreateVegetableParticle(origin As Vector, rand As Random) As ParticleClass
        Dim vx As Double = CO.NextDouble(rand, -1, 1)
        Dim velocity As Vector
        If rand.Next(2) = 0 Then
            velocity = New Vector(vx, -Math.Sqrt(1 - vx * vx))
        Else
            velocity = New Vector(vx, +Math.Sqrt(1 - vx * vx))
        End If
        Dim size As Integer = rand.Next(3)
        Return New ParticleClass(origin, velocity * CO.NextDouble(rand, CO.PARTICLE_SPEED_MIN, CO.PARTICLE_SPEED_MAX), rand.Next(CO.PARTICLE_DURATION_MIN, CO.PARTICLE_DURATION_MAX + 1), Form1.Images(size + 7))
    End Function


End Class