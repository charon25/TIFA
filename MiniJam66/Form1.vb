Imports GameMaker, System.IO

Public Class Form1

    'Jeu
    Private gState As CO.GameStates
    Private limits As Rectangle
    Private rand As Random
    Private HighScore As String
    Private LastTickTime As Long

    'Joueur
    Dim player As PlayerClass

    'Projectiles
    Private candys As List(Of CandyClass)
    Private candysToRemove As List(Of CandyClass)
    Private vegetables As List(Of VegetableClass)
    Private vegetablesToRemove As List(Of VegetableClass)
    Private spawners As List(Of SpawnerClass)

    'Particules
    Private particles As List(Of ParticleClass)
    Private particlesToRemove As List(Of ParticleClass)

    'Affichage
    Private chunk As PictureBox
    Public Images As List(Of Bitmap)
    Private MenuAnimator As Animator
    Private EndScreenAnimator As Animator
    Private EndScreenCounter As Integer
    Private IsNewHighScore As Boolean

    'Police
    Private PFC As Drawing.Text.PrivateFontCollection
    Private foundFont As Boolean
    Private SF As StringFormat

    'Audio
    Private AudioPlayer As SoundsManager
    Private MusicStartTick As Long

    '------------------------------------------------------------------------------------------------------------------------------

    'LOAD
    Private Sub Form1_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        GlobalInit()
    End Sub

    'INITIALISATION
    Private Sub GlobalInit()
        CreateFilesAndFolders()
        rand = New Random()
        InitAudio()
        InitFont()
        InitDisplay()
        Try
            HighScore = File.ReadAllText(CO.HIGHSCORE_PATH)
        Catch ex As Exception
            HighScore = "---"
        End Try
        gState = CO.GameStates.Menu
        gameTimer.Start()
    End Sub
    Private Sub InitAudio()
        AudioPlayer = New SoundsManager(CO.SOUNDS_DIR, "N", rand, "audioplayer0")
        AudioPlayer.AddSound("candyN1", My.Resources.sound_candyN1)
        AudioPlayer.AddSound("candyN2", My.Resources.sound_candyN2)
        AudioPlayer.AddSound("vegetableN1", My.Resources.sound_vegetableN1)
        AudioPlayer.AddSound("vegetableN2", My.Resources.sound_vegetableN2)
        AudioPlayer.AddSound("lose", My.Resources.sound_lose)
        AudioPlayer.AddSound("highscore", My.Resources.sound_highscore)
        AudioPlayer.AddSound("newgame", My.Resources.sound_newgame)
        AudioPlayer.AddSound("spawn", My.Resources.sound_spawn)
        AudioPlayer.AddSound("music", My.Resources.sound_music)

        MusicStartTick = 0
    End Sub
    Private Sub InitFont()
        PFC = New Drawing.Text.PrivateFontCollection()
        If File.Exists(CO.FONT_PATH) Then
            Try
                PFC.AddFontFile(CO.FONT_PATH)
                foundFont = True
            Catch ex As Exception
                foundFont = False
            End Try
        Else
            foundFont = False
        End If

        SF = New StringFormat()
        SF.Alignment = StringAlignment.Near
        SF.LineAlignment = StringAlignment.Center
    End Sub
    Private Sub CreateFilesAndFolders()
        Try
            If Not Directory.Exists(CO.FILES_DIR) Then
                Directory.CreateDirectory(CO.FILES_DIR)
            End If
            If Not Directory.Exists(CO.FONT_DIR) Then
                Directory.CreateDirectory(CO.FONT_DIR)
            End If
            If Not Directory.Exists(CO.SOUNDS_DIR) Then
                Directory.CreateDirectory(CO.SOUNDS_DIR)
            End If
            If Not File.Exists(CO.FONT_PATH) Then
                File.WriteAllBytes(CO.FONT_PATH, My.Resources.font)
            End If
            If Not File.Exists(CO.HIGHSCORE_PATH) Then
                File.WriteAllText(CO.HIGHSCORE_PATH, "---")
            End If
        Catch ex As Exception
            MsgBox("An error has occured when trying to save files and folders. Please make sure the game has all the necessary rights and is placed in a proper folder, and then restart it.", MsgBoxStyle.Critical, "Error")
        End Try
    End Sub
    Private Sub InitGame()
        'Sons
        AudioPlayer.CloseAllButSomeExceptions("music")
        AudioPlayer.PlaySound("newgame")

        'Joueur
        limits = New Rectangle(CO.LIMIT_MARGIN, CO.LIMIT_MARGIN, CO.WIDTH - 2 * CO.LIMIT_MARGIN, CO.HEIGHT - 2 * CO.LIMIT_MARGIN)
        player = New PlayerClass(New Vector(CO.WIDTH / 2, CO.HEIGHT / 2), limits)

        'Listes
        candys = New List(Of CandyClass)
        candysToRemove = New List(Of CandyClass)
        vegetables = New List(Of VegetableClass)
        vegetablesToRemove = New List(Of VegetableClass)
        spawners = New List(Of SpawnerClass)
        spawners.Add(New SpawnerClass(rand))
        particles = New List(Of ParticleClass)
        particlesToRemove = New List(Of ParticleClass)

        'State
        gState = CO.GameStates.Game
    End Sub
    Private Sub InitDisplay()
        chunk = New PictureBox()
        With chunk
            .Size = New Size(CO.WIDTH, CO.HEIGHT)
            .Location = New Point(0, 0)
            .BackColor = Color.Transparent
            .Enabled = False
            .Visible = True
            .SendToBack()
        End With
        gamePanel.Controls.Add(chunk)
        InitImages()
    End Sub
    Private Sub InitImages()
        Images = New List(Of Bitmap)
        Images.Add(My.Resources.sugar)
        Images.Add(My.Resources.health_bar)
        Images.Add(My.Resources.healthbar_frame)
        Images.Add(My.Resources.keys)
        Images.Add(My.Resources.part_candy_small)
        Images.Add(My.Resources.part_candy_medium)
        Images.Add(My.Resources.part_candy_large)
        Images.Add(My.Resources.part_vegetable_small)
        Images.Add(My.Resources.part_vegetable_medium)
        Images.Add(My.Resources.part_vegetable_large)
        Images.Add(My.Resources.star)
        Images.Add(My.Resources.hints)
        Images.AddRange(Functions.Images.SplitSprites1DWidth(My.Resources.cross, CO.CROSS_WIDTH))
        For i As Integer = 0 To Images.Count - 1
            Images(i).SetResolution(CO.RESOLUTION, CO.RESOLUTION)
        Next

        MenuAnimator = New Animator(Functions.Images.SplitSprites1DWidth(My.Resources.menu_background, CO.WIDTH), CO.BUTTON_BLINK_DURATION)
        EndScreenAnimator = New Animator(Functions.Images.SplitSprites1DWidth(My.Resources.end_screen, CO.END_SCREEN_WIDTH), CO.BUTTON_BLINK_DURATION)
    End Sub

    'TOUCHES
    Private Sub Form1_KeyDown(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        If gState = CO.GameStates.Game Then
            If e.KeyCode = CO.KEY1 Then
                player.ChangeYDirection(True)
            ElseIf e.KeyCode = CO.KEY2 Then
                player.ChangeXDirection(True)
            End If
        End If
    End Sub
    Private Sub Form1_KeyUp(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyUp
        Select Case gState
            Case CO.GameStates.Menu
                If e.KeyCode = CO.KEY2 Then
                    InitGame()
                End If
            Case CO.GameStates.Game
                If e.KeyCode = CO.KEY1 Then
                    player.ChangeYDirection(False)
                ElseIf e.KeyCode = CO.KEY2 Then
                    player.ChangeXDirection(False)
                End If
            Case CO.GameStates.EndScreen
                If EndScreenCounter <= 0 AndAlso e.KeyCode = CO.KEY2 Then
                    InitGame()
                End If
        End Select
    End Sub

    'PROJECTILES
    Public Sub CreateCandy(candy As CandyClass)
        AudioPlayer.PlaySound("spawn")
        candys.Add(candy)
    End Sub
    Public Sub CreateVegetable(vegetable As VegetableClass)
        AudioPlayer.PlaySound("spawn")
        vegetables.Add(vegetable)
    End Sub

    'AFFICHAGE
    Private Sub Draw()
        Dim img As New Bitmap(My.Resources.background)
        img.SetResolution(CO.RESOLUTION, CO.RESOLUTION)
        Dim g As Graphics = Graphics.FromImage(img)
        g.TextRenderingHint = Drawing.Text.TextRenderingHint.SystemDefault

        'Joueur
        Dim playerImage As Bitmap = player.GetSprite()
        playerImage.SetResolution(CO.RESOLUTION, CO.RESOLUTION)
        g.DrawImage(playerImage, player.GetLocationPoint)

        'Particules
        Dim particleImage As Bitmap
        For Each particle As ParticleClass In particles
            particleImage = particle.GetSprite()
            particleImage.SetResolution(CO.RESOLUTION, CO.RESOLUTION)
            g.DrawImage(particleImage, particle.GetLocationPoint())
        Next

        'Bonbons
        Dim candyImage As Bitmap
        For Each candy As CandyClass In candys
            candyImage = candy.GetSprite()
            candyImage.SetResolution(CO.RESOLUTION, CO.RESOLUTION)
            g.DrawImage(candyImage, candy.GetLocationPoint())
        Next

        'Légumes
        Dim vegetableImage As Bitmap
        For Each vegetable As VegetableClass In vegetables
            vegetableImage = vegetable.GetSprite()
            vegetableImage.SetResolution(CO.RESOLUTION, CO.RESOLUTION)
            g.DrawImage(vegetableImage, vegetable.GetLocationPoint())
        Next

        'Vie
        g.DrawImage(Images(CO.Textures.HealthbarFrame), CO.HEALTHBAR_FRAME_LOCATION)
        g.DrawImage(Images(CO.Textures.HealthBar), New Rectangle(CO.HEALTHBAR_RECTANGLE.X, CO.HEALTHBAR_RECTANGLE.Y, CO.HEALTHBAR_RECTANGLE.Width * player.GetHealth() / CO.PLAYER_MAX_HEALTH, CO.HEALTHBAR_RECTANGLE.Height), New Rectangle(0, 0, CO.HEALTHBAR_RECTANGLE.Width * player.GetHealth() / CO.PLAYER_MAX_HEALTH, CO.HEALTHBAR_RECTANGLE.Height), GraphicsUnit.Pixel)
        g.DrawImage(Images(CO.Textures.Sugar), CO.SUGAR_LOCATION)

        'Touches et aides
        g.DrawImage(Images(CO.Textures.Keys), CO.KEYS_LOCATION)
        g.DrawImage(Images(CO.Textures.Hints), CO.HINTS_LOCATION)
        g.DrawImage(Images(player.GetCrossDirection()), CO.CROSS_POINT)

        'Score
        If foundFont Then
            g.DrawString("Score : " & player.GetScore(), New Font(PFC.Families(0), CO.SCORE_FONT_SIZE, FontStyle.Regular), Brushes.Black, CO.SCORE_RECTANGLE, SF)
        Else
            g.DrawString("Score : " & player.GetScore(), New Font("Arial", CO.SCORE_FONT_SIZE, FontStyle.Regular), Brushes.Black, CO.SCORE_RECTANGLE, SF)
        End If

        'Affichage
        chunk.Image = img
        chunk.Refresh()
    End Sub
    'PARTICULES
    Private Sub CreateCandyParticles()
        Dim count As Integer = rand.Next(CO.PARTICLE_COUNT_MIN, CO.PARTICLE_DURATION_MAX + 1)
        Dim origin As Vector = player.GetCenter()
        For i As Integer = 1 To count
            particles.Add(ParticleClass.CreateCandyParticle(origin, rand))
        Next
    End Sub
    Private Sub CreateVegetablesParticles()
        Dim count As Integer = rand.Next(CO.PARTICLE_COUNT_MIN, CO.PARTICLE_DURATION_MAX + 1)
        Dim origin As Vector = player.GetCenter()
        For i As Integer = 1 To count
            particles.Add(ParticleClass.CreateVegetableParticle(origin, rand))
        Next
    End Sub

    'MENU
    Private Sub DrawMenu()
        Dim img As New Bitmap(MenuAnimator.GetCurrentSprite())
        img.SetResolution(CO.RESOLUTION, CO.RESOLUTION)
        Dim g As Graphics = Graphics.FromImage(img)
        g.TextRenderingHint = Drawing.Text.TextRenderingHint.SystemDefault

        'Best score
        If foundFont Then
            g.DrawString(HighScore, New Font(PFC.Families(0), 30), New SolidBrush(Color.FromArgb(45, 45, 45)), CO.MENU_HIGH_SCORE_RECTANGLE, SF)
        Else

        End If

        chunk.Image = img
        chunk.Refresh()
    End Sub

    'FIN DU JEU
    Private Sub Lose()
        AudioPlayer.CloseAllButSomeExceptions("music")
        EndScreenCounter = CO.END_SCREEN_COUNTER_START
        gState = CO.GameStates.EndScreen
        Dim highestScore As String
        Try
            highestScore = File.ReadAllText(CO.HIGHSCORE_PATH)
        Catch ex As Exception
            highestScore = "---"
        End Try
        IsNewHighScore = Not IsNumeric(highestScore) OrElse highestScore < player.GetScore()
        If IsNewHighScore Then
            Try
                File.WriteAllText(CO.HIGHSCORE_PATH, player.GetScore())
            Catch ex As Exception : End Try
            AudioPlayer.PlaySound("highscore")
        Else
            AudioPlayer.PlaySound("lose")
        End If
        DrawEndScreen()
    End Sub
    Private Sub DrawEndScreen()
        Dim img As New Bitmap(My.Resources.background)
        img.SetResolution(CO.RESOLUTION, CO.RESOLUTION)
        Dim g As Graphics = Graphics.FromImage(img)
        g.TextRenderingHint = Drawing.Text.TextRenderingHint.SystemDefault

        'Ecran de fin de jeu
        Dim endScreenImage As Bitmap = EndScreenAnimator.GetCurrentSprite()
        endScreenImage.SetResolution(CO.RESOLUTION, CO.RESOLUTION)
        g.DrawImage(endScreenImage, CO.END_SCREEN_POINT)

        'Score
        If foundFont Then
            g.DrawString(player.GetScore(), New Font(PFC.Families(0), CO.END_SCREEN_SCORE_FONT_SIZE), Brushes.Black, CO.END_SCREEN_SCORE_RECTANGLE, SF)
        Else
            g.DrawString(player.GetScore(), New Font("Arial", CO.END_SCREEN_SCORE_FONT_SIZE / 2), Brushes.Black, CO.END_SCREEN_SCORE_RECTANGLE, SF)
        End If

        'Etoile
        If IsNewHighScore Then
            g.DrawImage(Images(CO.Textures.Star), CO.END_SCREEN_STAR_POINT)
        End If


        chunk.Image = img
        chunk.Refresh()
    End Sub

    'TIMER DE JEU
    Private Sub gameTimer_Tick(sender As System.Object, e As System.EventArgs) Handles gameTimer.Tick
        If Date.Now.Ticks - LastTickTime < CO.TICK_DURATION_TICKS_MIN Then
            Threading.Thread.Sleep((CO.TICK_DURATION_TICKS_MIN - Date.Now.Ticks + LastTickTime) / 10000)
        End If
        LastTickTime = Date.Now.Ticks
        If Date.Now.Ticks - MusicStartTick > CO.MUSIC_DURATION_TICKS Then
            AudioPlayer.PlaySound("music")
            MusicStartTick = Date.Now.Ticks
        End If
        If gState = CO.GameStates.Menu Then
            MenuAnimator.Age()
            DrawMenu()
        ElseIf gState = CO.GameStates.Game Then
            'Joueur
            player.Age()
            Dim playerHitboxLocation As PointF = player.GetLocationPointF() + CO.PLAYER_HITBOX_OFFSET
            If player.GetHealth() <= 0 Then
                Lose()
            End If

            'Spawners
            For Each spawner As SpawnerClass In spawners
                spawner.Age()
            Next
            If spawners.Count > 0 AndAlso spawners(0).CanAddNewSpawner() Then
                spawners.Add(New SpawnerClass(rand))
            End If

            'Bonbons
            For Each candy As CandyClass In candys
                candy.Move()
                If player.HitCandy(candy) Then
                    AudioPlayer.PlaySound("candy")
                    CreateCandyParticles()
                    candysToRemove.Add(candy)
                End If
                If candy.IsDestroyed() Then
                    candysToRemove.Add(candy)
                End If
            Next
            For Each candy As CandyClass In candysToRemove
                candys.Remove(candy)
            Next
            candysToRemove.Clear()

            'Légumes
            For Each vegetable As VegetableClass In vegetables
                vegetable.Move()
                If player.HitVegetable(vegetable) Then
                    AudioPlayer.PlaySound("vegetable")
                    CreateVegetablesParticles()
                    vegetablesToRemove.Add(vegetable)
                End If
                If vegetable.IsDestroyed() Then
                    vegetablesToRemove.Add(vegetable)
                End If
            Next
            For Each vegetable As VegetableClass In vegetablesToRemove
                vegetables.Remove(vegetable)
            Next
            vegetablesToRemove.Clear()

            'Particules
            For Each particle As ParticleClass In particles
                particle.Age()
                If particle.IsDestroyed() Then
                    particlesToRemove.Add(particle)
                End If
            Next
            For Each particle As ParticleClass In particlesToRemove
                particles.Remove(particle)
            Next
            particlesToRemove.Clear()

            Draw()
        ElseIf gState = CO.GameStates.EndScreen Then
            If EndScreenCounter > 0 Then EndScreenCounter -= 1
            EndScreenAnimator.Age()
            DrawEndScreen()
        End If
    End Sub
End Class
