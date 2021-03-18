Imports GameMaker

Public Class CandyClass
    Inherits ProjectileClass

    Private Type As CO.CandyTypes

    Public Function GetCandyType() As CO.CandyTypes
        Return Type
    End Function

    Public Sub New(startingLocation As Vector, endingLocation As Vector, type As CO.CandyTypes)
        MyBase.New(startingLocation, endingLocation, CO.CANDY_SIZE, CO.CANDY_SPEEDS(type))
        Me.Type = type
        Select Case type
            Case CO.CandyTypes.Blue
                Animator = New Animator(Functions.Images.SplitSprites1DWidth(My.Resources.candy_blue, CO.CANDY_WIDTH), CO.CANDY_ANIMATION_COOLDOWN)
            Case CO.CandyTypes.Green
                Animator = New Animator(Functions.Images.SplitSprites1DWidth(My.Resources.candy_green, CO.CANDY_WIDTH), CO.CANDY_ANIMATION_COOLDOWN)
            Case CO.CandyTypes.Red
                Animator = New Animator(Functions.Images.SplitSprites1DWidth(My.Resources.candy_red, CO.CANDY_WIDTH), CO.CANDY_ANIMATION_COOLDOWN)
        End Select
    End Sub

End Class
