Imports GameMaker

Public Class VegetableClass
    Inherits ProjectileClass

    Private Type As CO.VegetableTypes

    Public Function GetVegetableType() As CO.VegetableTypes
        Return Type
    End Function

    Public Sub New(startingLocation As Vector, endingLocation As Vector, type As CO.VegetableTypes)
        MyBase.New(startingLocation, endingLocation, CO.VEGETABLE_SIZE, CO.VEGETABLE_SPEEDS(type))
        Me.Type = type
        Select Case type
            Case CO.VegetableTypes.Carrot
                Animator = New Animator(Functions.Images.SplitSprites1DWidth(My.Resources.vegetable_carrot, CO.VEGETABLE_WIDTH
), CO.VEGETABLE_ANIMATION_COOLDOWN)
            Case CO.VegetableTypes.Eggplant
                Animator = New Animator(Functions.Images.SplitSprites1DWidth(My.Resources.vegetable_eggplant, CO.VEGETABLE_WIDTH
), CO.VEGETABLE_ANIMATION_COOLDOWN)
            Case CO.VegetableTypes.Broccoli
                Animator = New Animator(Functions.Images.SplitSprites1DWidth(My.Resources.vegetable_broccoli, CO.VEGETABLE_WIDTH
), CO.VEGETABLE_ANIMATION_COOLDOWN)
        End Select
    End Sub

End Class
