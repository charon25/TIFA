Imports GameMaker

Public Class CO

    'Fenêtre
    Public Const WIDTH As Integer = 400
    Public Const HEIGHT As Integer = 400
    Public Shared ReadOnly WINDOW_SIZE As New Size(WIDTH, HEIGHT)
    Public Const RESOLUTION As Double = 72.0

    'Dossiers
    Public Const FILES_DIR As String = "files\"
    Public Const FONT_DIR As String = FILES_DIR & "fonts\"
    Public Const SOUNDS_DIR As String = FILES_DIR & "sounds\"
    Public Const FONT_PATH As String = FONT_DIR & "font.ttf"
    Public Const HIGHSCORE_PATH As String = FILES_DIR & "score.txt"

    'Jeu
    Public Const TICK_DURATION_TICKS_MIN As Long = 26 * 10000
    Public Enum GameStates As Integer
        Menu = 0
        Game = 1
        EndScreen = 2
    End Enum
    Public Const KEY1 As Keys = Keys.T
    Public Const KEY2 As Keys = Keys.Space
    Public Const BUTTON_BLINK_DURATION As Integer = 18
    Public Const MUSIC_DURATION_TICKS As Long = 10000L * (1000L * (60 * 3 + 52) + 2500)

    'Affichage
    Public Enum Textures
        Sugar = 0
        HealthBar = 1
        HealthbarFrame = 2
        Keys = 3
        CandyParticleSmall = 4
        CandyParticleMedium = 5
        CandyParticleLarge = 6
        VegetableParticleSmall = 7
        VegetableParticleMedium = 8
        VegetableParticleLarge = 9
        Star = 10
        Hints = 11
        CrossDL = 12
        CrossDR = 13
        CrossTL = 14
        CrossTR = 15
    End Enum
    Public Shared ReadOnly SUGAR_LOCATION As New Point(16, 6)
    Public Shared ReadOnly HEALTHBAR_RECTANGLE As New Rectangle(31, 9, 108, 14)
    Public Shared ReadOnly HEALTHBAR_FRAME_LOCATION As New Point(29, 8)
    Public Shared ReadOnly HINTS_LOCATION As New Point(126, 366)
    Public Shared ReadOnly KEYS_LOCATION As New Point(222, 366)
    Public Shared ReadOnly MENU_HIGH_SCORE_RECTANGLE As New Rectangle(222, 354, 125, 23)
    ''Police
    Public Shared ReadOnly SCORE_RECTANGLE As New Rectangle(29, 24, 111, 20)
    Public Const SCORE_FONT_SIZE As Integer = 15

    'Limites
    Public Const LIMIT_MARGIN As Integer = 48

    'Joueur
    Public Const PLAYER_SIZE As Integer = 32
    Public Shared ReadOnly PLAYER_CENTER_OFFSET As New Vector(PLAYER_SIZE / 2, PLAYER_SIZE / 2)
    Public Shared ReadOnly PLAYER_SIZE_S As New Size(PLAYER_SIZE, PLAYER_SIZE)
    Public Shared ReadOnly PLAYER_HITBOX_OFFSET As New Point(6, 3)
    Public Shared ReadOnly PLAYER_HITBOX_SIZE As New Size(PLAYER_SIZE - PLAYER_HITBOX_OFFSET.X / 2, PLAYER_SIZE - PLAYER_HITBOX_OFFSET.Y)
    Public Const BASE_PLAYER_SPEED As Double = 9.5
    Public Enum PlayerStates As Integer
        IdleDownLeft = 0
        IdleDownRight = 1
        IdleTopLeft = 2
        IdleTopRight = 3
        WalkDownLeft = 4
        WalkDownRight = 5
        WalkTopLeft = 6
        WalkTopRight = 7
        HurtLeft = 8
        HurtRight = 9
    End Enum
    Public Enum PlayerWalkAnimations As Integer
        DownLeft = 0
        DownRight = 1
        TopLeft = 2
        TopRight = 3
    End Enum
    Public Shared ReadOnly PLAYER_WALK_ANIMATION_COUNT As Integer = [Enum].GetNames(GetType(PlayerWalkAnimations)).Length
    Public Const PLAYER_WALK_ANIMATION_COOLDOWN As Integer = 7
    Public Const PLAYER_MAX_HEALTH As Double = 200.0
    Public Const PLAYER_HEALTH_DECAY As Double = 0.02
    Public Const PLAYER_HURT_STATE_DURATION As Integer = 15

    'Bonbons
    Public Enum CandyTypes As Integer
        Green = 0
        Blue = 1
        Red = 2
    End Enum
    Public Shared ReadOnly CANDY_TYPE_COUNT As Integer = [Enum].GetNames(GetType(CandyTypes)).Length
    Public Shared CANDY_SPEEDS() As Double = {4.0, 5.7, 6.8}
    Public Shared CANDY_POINTS() As Integer = {10, 15, 18}
    Public Shared CANDY_HEALINGS() As Double = {3.0, 5.0, 7.0}
    Public Const CANDY_WIDTH As Integer = 20
    Public Const CANDY_HEIGHT As Integer = 11
    Public Shared ReadOnly CANDY_SIZE As New Size(CANDY_WIDTH, CANDY_HEIGHT)
    Public Const CANDY_COOLDOWN_AVERAGE As Integer = 70
    Public Const CANDY_COOLDOWN_DEVIATION As Integer = 25
    Public Const CANDY_ANIMATION_COOLDOWN As Integer = 9

    'Légumes
    Public Enum VegetableTypes As Integer
        Carrot = 0
        Eggplant = 1
        Broccoli = 2
    End Enum
    Public Shared ReadOnly VEGETABLE_TYPE_COUNT As Integer = [Enum].GetNames(GetType(VegetableTypes)).Length
    Public Shared VEGETABLE_SPEEDS() As Double = {4.25, 6.5, 8.75}
    Public Shared VEGETABLE_DAMAGES() As Double = {5.0, 7.5, 10.0}
    Public Const VEGETABLE_WIDTH As Integer = 26
    Public Const VEGETABLE_HEIGHT As Integer = 26
    Public Shared ReadOnly VEGETABLE_SIZE As New Size(VEGETABLE_WIDTH, VEGETABLE_HEIGHT)
    Public Const VEGETABLE_COOLDOWN_AVERAGE As Integer = 60
    Public Const VEGETABLE_COOLDOWN_DEVIATION As Integer = 20
    Public Const VEGETABLE_ANIMATION_COOLDOWN As Integer = 10

    'Spawner
    Public Const COUNT_BEFORE_NEW_SPAWNER As Integer = 12
    Public Const CANDY_COUNT_BEFORE_VEGETABLES As Integer = 3

    'Particules
    Public Const PARTICLE_DURATION_MIN As Integer = 7
    Public Const PARTICLE_DURATION_MAX As Integer = 15
    Public Const PARTICLE_COUNT_MIN As Integer = 3
    Public Const PARTICLE_COUNT_MAX As Integer = 7
    Public Const PARTICLE_SPEED_MIN As Double = 1.5
    Public Const PARTICLE_SPEED_MAX As Double = 3.5

    'Ecran de fin
    Public Const END_SCREEN_WIDTH As Integer = 208
    Public Shared ReadOnly END_SCREEN_POINT As New Point(96, 144)
    Public Shared ReadOnly END_SCREEN_SCORE_RECTANGLE As New Rectangle(190, 195, 1000, 20)
    Public Const END_SCREEN_SCORE_FONT_SIZE As Integer = 30
    Public Shared ReadOnly END_SCREEN_STAR_POINT As Point = New Point(14, 53) + END_SCREEN_POINT
    Public Const END_SCREEN_COUNTER_START As Integer = 15

    'Croix directionnelle
    Public Const CROSS_WIDTH As Integer = 48
    Public Shared ReadOnly CROSS_POINT As New Point(344, 8)

    'Autres
    Public Shared Function NextDouble(ByRef rand As Random, min As Double, max As Double) As Double
        Return rand.NextDouble() * (max - min) + min
    End Function

End Class
