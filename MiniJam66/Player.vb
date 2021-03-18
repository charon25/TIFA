Imports GameMaker

Public Class PlayerClass

    Private Location As Vector
    Private Velocity As Vector
    Private XDir, YDir As Integer
    Private Limits As Rectangle
    Private State As CO.PlayerStates
    Private IdleSprites() As Bitmap
    Private HurtSprites() As Bitmap
    Private Animators() As Animator '0  : dl | 1 : dr | 2 : tl | 3 : tr
    Private Score, PickedUpCandys As Integer
    Private Health As Double
    Private HurtCooldown As Integer

    Public Function GetLocationPoint() As Point
        Return Location.ToPoint()
    End Function
    Public Function GetLocationPointF() As PointF
        Return Location.ToPointF()
    End Function
    Public Function GetScore() As Integer
        Return Score
    End Function
    Public Function GetHealth() As Double
        Return Health
    End Function
    Public Function GetCenter() As Vector
        Return Location + CO.PLAYER_CENTER_OFFSET
    End Function
    Public Function GetPickedUpCandyCount() As Integer
        Return PickedUpCandys
    End Function
    Public Function GetCrossDirection() As CO.Textures
        Select Case (2 * XDir + YDir)
            Case -1 'DL
                Return CO.Textures.CrossDL
            Case 3 'DR
                Return CO.Textures.CrossDR
            Case -3 'TL
                Return CO.Textures.CrossTL
            Case 1 'TR
                Return CO.Textures.CrossTR
        End Select
        Return CO.Textures.CrossDL
    End Function

    Public Sub New(ByRef location As Vector, limits As Rectangle)
        Me.Location = location
        Me.Velocity = Vector.NullVector()
        Me.XDir = 1
        Me.YDir = 1
        Me.Limits = limits
        ReDim Me.Animators(CO.PLAYER_WALK_ANIMATION_COUNT - 1)
        Me.Animators(0) = New Animator(Functions.Images.SplitSprites1DWidth(My.Resources.player_walk_dl, CO.PLAYER_SIZE), CO.PLAYER_WALK_ANIMATION_COOLDOWN)
        Me.Animators(1) = New Animator(Functions.Images.SplitSprites1DWidth(My.Resources.player_walk_dr, CO.PLAYER_SIZE), CO.PLAYER_WALK_ANIMATION_COOLDOWN)
        Me.Animators(2) = New Animator(Functions.Images.SplitSprites1DWidth(My.Resources.player_walk_tl, CO.PLAYER_SIZE), CO.PLAYER_WALK_ANIMATION_COOLDOWN)
        Me.Animators(3) = New Animator(Functions.Images.SplitSprites1DWidth(My.Resources.player_walk_tr, CO.PLAYER_SIZE), CO.PLAYER_WALK_ANIMATION_COOLDOWN)
        Me.IdleSprites = Functions.Images.SplitSprites1DWidth(My.Resources.player_idle, CO.PLAYER_SIZE)
        Me.HurtSprites = Functions.Images.SplitSprites1DWidth(My.Resources.player_hurt, CO.PLAYER_SIZE)
        Me.Score = 0
        Me.PickedUpCandys = 0
        Me.Health = CO.PLAYER_MAX_HEALTH / 2
        Me.HurtCooldown = 0
    End Sub

    Public Sub Age()
        Location += Velocity.ToUnitVector() * CO.BASE_PLAYER_SPEED
        Location.X = Math.Max(Limits.X, Math.Min(Location.X, Limits.X + Limits.Width - CO.PLAYER_SIZE))
        Location.Y = Math.Max(Limits.Y, Math.Min(Location.Y, Limits.Y + Limits.Height - CO.PLAYER_SIZE))
        For i As Integer = 0 To CO.PLAYER_WALK_ANIMATION_COUNT - 1
            Animators(i).Age()
        Next
        UpdateState()
        If HurtCooldown > 0 Then
            HurtCooldown -= 1
            If XDir = 1 Then
                State = CO.PlayerStates.HurtRight
            Else
                State = CO.PlayerStates.HurtLeft
            End If
        End If
        Health = Math.Max(0, Health - CO.PLAYER_HEALTH_DECAY)
    End Sub

    Public Sub ChangeXDirection(move As Boolean)
        If move Then
            Velocity.X = XDir
        Else
            Velocity.X = 0
            XDir *= -1
        End If
        UpdateState()
    End Sub
    Public Sub ChangeYDirection(move As Boolean)
        If move Then
            Velocity.Y = YDir
        Else
            Velocity.Y = 0
            YDir *= -1
        End If
        UpdateState()
    End Sub
    Private Sub UpdateState()
        State = 0
        If Velocity.X <> 0 OrElse Velocity.Y <> 0 Then
            State += 4
        End If
        Select Case (2 * XDir + YDir)
            Case 3 'DR
                State += 1
            Case -3 'TL
                State += 2
            Case 1 'TR
                State += 3
        End Select
    End Sub

    Public Function HitCandy(candy As CandyClass) As Boolean
        If Functions.Collision.RectangleRectangle(Location.ToPointF() + CO.PLAYER_HITBOX_OFFSET, CO.PLAYER_HITBOX_SIZE, candy.GetLocationPointF(), CO.CANDY_SIZE) Then
            Dim candyType As CO.CandyTypes = candy.GetCandyType()
            Score += CO.CANDY_POINTS(candyType)
            PickedUpCandys += 1
            Health = Math.Min(CO.PLAYER_MAX_HEALTH, Health + CO.CANDY_HEALINGS(candyType))
            Return True
        Else
            Return False
        End If
    End Function
    Public Function HitVegetable(vegetable As VegetableClass) As Boolean
        If HurtCooldown = 0 AndAlso Functions.Collision.RectangleRectangle(Location.ToPointF() + CO.PLAYER_HITBOX_OFFSET, CO.PLAYER_HITBOX_SIZE, vegetable.GetLocationPointF(), CO.VEGETABLE_SIZE) Then
            Health = Math.Max(0, Health - CO.CANDY_POINTS(vegetable.GetVegetableType()))
            HurtCooldown = CO.PLAYER_HURT_STATE_DURATION
            Return True
        Else
            Return False
        End If
    End Function

    Public Function GetSprite() As Bitmap
        Select Case State
            Case Is < 4 'Idle
                Return IdleSprites(State)
            Case Is < 8 'Marche
                Return Animators(State - 4).GetCurrentSprite()
            Case Is < 10
                If HurtCooldown Mod 6 < 3 Then
                    Return HurtSprites(State - 8)
                Else
                    Return New Bitmap(CO.PLAYER_SIZE, CO.PLAYER_SIZE)
                End If
            Case Else
                Return IdleSprites(0)
        End Select
    End Function
End Class
