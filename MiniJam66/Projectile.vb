Imports GameMaker

Public Class ProjectileClass

    Private Location As Vector
    Private Velocity As Vector
    Private Destroyed As Boolean
    Private Size As Size
    Protected Animator As Animator

    Public Function GetLocationPoint() As Point
        Return Location.ToPoint()
    End Function
    Public Function GetLocationPointF() As PointF
        Return Location.ToPointF()
    End Function
    Public Function IsDestroyed() As Boolean
        Return Destroyed
    End Function

    Public Sub New(startingLocation As Vector, endingLocation As Vector, size As Size, speed As Double)
        Me.Location = startingLocation
        Me.Velocity = (endingLocation - startingLocation).ToUnitVector() * speed
        Me.Destroyed = False
        Me.Size = size
    End Sub

    Public Sub Move()
        Location += Velocity
        If Not Functions.Collision.RectangleRectangle(Me.Location.ToPointF(), Size, New PointF(-30, -30), CO.WINDOW_SIZE + New Size(60, 60)) Then 'Marge de manoeuvre au début
            Destroyed = True
        End If
        Animator.Age()
    End Sub

    Public Function PlayerHit(playerLocation As PointF) As Boolean
        Return Functions.Collision.RectangleRectangle(Location.ToPointF(), Size, playerLocation, CO.PLAYER_HITBOX_SIZE)
    End Function

    Public Function GetSprite()
        Return Animator.GetCurrentSprite()
    End Function

    
End Class
