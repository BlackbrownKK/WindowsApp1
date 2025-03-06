Imports VectorDraw.Geometry
Imports vdInsert = VectorDraw.Professional.vdFigures.vdInsert
Imports vdFigure = VectorDraw.Professional.vdPrimaries.vdFigure
Imports vdEntities = VectorDraw.Professional.vdCollections.vdEntities
Imports System.Text



Public Class MovingController



    Private WithEvents Doc As VectorDraw.Professional.vdObjects.vdDocument

    Dim SideViewEntity As vdInsert
    Dim TopWDViewEntity As vdInsert
    Dim TopTDViewEntity As vdInsert
    Dim TopTTViewEntity As vdInsert
    Dim AftViewEntity As vdInsert

    Dim initialXOfSideViewEntity As Double
    Dim initialYOfSideViewEntity As Double
    Dim initialZOfSideViewEntity As Double

    Dim initialXOfTopWDViewEntity As Double
    Dim initialYOfTopWDViewEntity As Double
    Dim initialZOfTopWDViewEntity As Double

    Dim initialXOfTopTDViewEntity As Double
    Dim initialYOfTopTDViewEntity As Double
    Dim initialZOfTopTDViewEntity As Double

    Dim initialXOfTopTTViewEntity As Double
    Dim initialYOfTopTTViewEntity As Double
    Dim initialZOfTopTTViewEntity As Double

    Dim initialXOfAftViewEntity As Double
    Dim initialYOfAftViewEntity As Double
    Dim initialZOfAftViewEntity As Double




    Dim P180 As vdInsert
    Dim C1P180 As vdInsert
    Dim C2P180 As vdInsert

    Dim TopWDAltitudeMarker As Double
    Dim TopTDAltitudeMarker As Double
    Dim TopTTAltitudeMarker As Double


    Public TowViewMassage As String

    Dim initialXOfMovedBlock As Double
    Dim initialYOfMovedBlock As Double
    Dim initialZOfMovedBlock As Double



    Dim movedBlock As vdInsert
    Dim AllEntityes As vdEntities


    ' Constructor to initialize with a document reference
    Public Sub New(document As VectorDraw.Professional.vdObjects.vdDocument)
        Me.Doc = document
    End Sub

    Public Sub moveMirrorBlocks(movedBlock As vdInsert, initialX As Double, initialY As Double, initialZ As Double)
        intializeViewsEntities(movedBlock)

        Me.initialXOfMovedBlock = initialX
        Me.initialYOfMovedBlock = initialY
        Me.initialZOfMovedBlock = initialZ


        If movedBlock.Equals(TopWDViewEntity) Then
            initialXOfTopWDViewEntity = initialX
            initialYOfTopWDViewEntity = initialY
            initialZOfTopWDViewEntity = initialZ
            TopWDMoving()

        ElseIf movedBlock.Equals(TopTDViewEntity) Then
            initialXOfTopTDViewEntity = initialX
            initialYOfTopTDViewEntity = initialY
            initialZOfTopTDViewEntity = initialZ
            TopTDMoving()
        ElseIf movedBlock.Equals(TopTTViewEntity) Then
            initialXOfTopTTViewEntity = initialX
            initialYOfTopTTViewEntity = initialY
            initialZOfTopTTViewEntity = initialZ
            TopTTMoving()
        ElseIf movedBlock.Equals(SideViewEntity) Then
            initialXOfSideViewEntity = initialX
            initialYOfSideViewEntity = initialY
            initialZOfSideViewEntity = initialZ
            SideViewMoving()
        ElseIf movedBlock.Equals(AftViewEntity) Then
            initialXOfAftViewEntity = initialX
            initialYOfAftViewEntity = initialY
            initialZOfAftViewEntity = initialZ
            AftViewMoving()
        End If

        VisibleOnForAllViews()

    End Sub

    Private Sub intializeViewsEntities(movedBlock As vdInsert)

        Me.movedBlock = movedBlock

        ' getting the Layer name from the moved block's name
        Dim LayerNumber As String = ToolsUtilites.getLayerNumber(movedBlock.Layer.Name).ToString
        ' getting the Layer number from the moved block's name


        Me.AllEntityes = Doc.ActiveLayOut.Entities

        For Each otherViewsEntity As vdFigure In AllEntityes
            If TypeOf otherViewsEntity Is vdInsert Then
                Dim tempEntity As vdInsert = CType(otherViewsEntity, vdInsert)
                Select Case True
                    Case ToolsUtilites.topWDAltitudeMarkerRecognizer(tempEntity.Layer.ToString)
                        TopWDAltitudeMarker = tempEntity.InsertionPoint.z
                    Case ToolsUtilites.topTDAltitudeMarkerRecognizer(tempEntity.Layer.ToString)
                        TopTDAltitudeMarker = tempEntity.InsertionPoint.z
                    Case ToolsUtilites.topTTAltitudeMarkerRecognizer(tempEntity.Layer.ToString)
                        TopTTAltitudeMarker = tempEntity.InsertionPoint.z
                    Case ToolsUtilites.topP180Recognizer(tempEntity.Layer.Name)
                        P180 = tempEntity
                    Case ToolsUtilites.topC1P180Recognizer(tempEntity.Layer.Name)
                        C1P180 = tempEntity
                    Case ToolsUtilites.topC2P180Recognizer(tempEntity.Layer.Name)
                        C2P180 = tempEntity
                End Select

                ' * For Each entity check Layer number
                If otherViewsEntity.Layer.Name.Contains(LayerNumber) Then
                    Select Case True
                        Case ToolsUtilites.StartsWithSU(tempEntity.Layer.ToString)
                            SideViewEntity = tempEntity
                            initialXOfSideViewEntity = tempEntity.InsertionPoint.x
                            initialYOfSideViewEntity = tempEntity.InsertionPoint.y
                            initialZOfSideViewEntity = tempEntity.InsertionPoint.z

                        Case ToolsUtilites.StartsWithU(tempEntity.Layer.ToString)
                            TopWDViewEntity = tempEntity
                            initialXOfTopWDViewEntity = tempEntity.InsertionPoint.x
                            initialYOfTopWDViewEntity = tempEntity.InsertionPoint.y
                            initialZOfTopWDViewEntity = tempEntity.InsertionPoint.z

                        Case ToolsUtilites.StartsWithC1U(tempEntity.Layer.ToString)
                            TopTDViewEntity = tempEntity
                            initialXOfTopTDViewEntity = tempEntity.InsertionPoint.x
                            initialYOfTopTDViewEntity = tempEntity.InsertionPoint.y
                            initialZOfTopTDViewEntity = tempEntity.InsertionPoint.z

                        Case ToolsUtilites.StartsWithC2U(tempEntity.Layer.ToString)
                            TopTTViewEntity = tempEntity
                            initialXOfTopTTViewEntity = tempEntity.InsertionPoint.x
                            initialYOfTopTTViewEntity = tempEntity.InsertionPoint.y
                            initialZOfTopTTViewEntity = tempEntity.InsertionPoint.z

                        Case ToolsUtilites.StartsWithAU(tempEntity.Layer.ToString)
                            AftViewEntity = tempEntity
                            initialXOfAftViewEntity = tempEntity.InsertionPoint.x
                            initialYOfAftViewEntity = tempEntity.InsertionPoint.y
                            initialZOfAftViewEntity = tempEntity.InsertionPoint.z
                        Case Else
                    End Select
                End If
            End If
        Next


        If TopTDViewEntity Is Nothing AndAlso TopWDViewEntity IsNot Nothing Then
            TopTDViewEntity = TopWDViewEntity
            ' Check if the new layer already exists
            Dim newLayerTempName As String = "C1" + TopWDViewEntity.Layer.Name
            Dim TopTDViewEntitynewLayer As VectorDraw.Professional.vdPrimaries.vdLayer = Doc.Layers.FindName(newLayerTempName)

            ' If the layer does not exist, create a new one with the same properties
            If TopTDViewEntitynewLayer Is Nothing Then
                TopTDViewEntitynewLayer = New VectorDraw.Professional.vdPrimaries.vdLayer(Doc, newLayerTempName)

                ' Copy properties from TopWDViewEntity's Layer
                TopTDViewEntitynewLayer.PenColor = TopWDViewEntity.Layer.PenColor
                TopTDViewEntitynewLayer.LineType = TopWDViewEntity.Layer.LineType
                TopTDViewEntitynewLayer.Frozen = TopWDViewEntity.Layer.Frozen
                TopTDViewEntitynewLayer.On = TopWDViewEntity.Layer.On
                TopTDViewEntitynewLayer.LineWeight = TopWDViewEntity.Layer.LineWeight

                ' Add the new layer to the document
                Doc.Layers.AddItem(TopTDViewEntitynewLayer)
            End If


            'Dim clonedBlock As VectorDraw.Professional.vdPrimaries.vdInsert = CType(TopWDViewEntity.Clone(Doc), vdInsert)
            ' If clonedBlock.Block IsNot Nothing Then
            'clonedBlock.Block.Update()
            'End If
            ' Clone the entity first before assigning the layer
            TopTDViewEntity = CType(TopWDViewEntity.Clone(Doc), vdInsert)
            TopTDViewEntity.Layer = TopTDViewEntitynewLayer  ' Assign the new layer

            ' Add the cloned entity to the document
            Doc.ActiveLayOut.Entities.AddItem(TopTDViewEntity)

            ' Debugging message
            Debug.WriteLine("New layer TD assigned: " & TopTDViewEntity.Layer.Name)

            ' Refresh document



            Dim deltaYOfWD As Double
            deltaYOfWD = P180.InsertionPoint.y - TopWDViewEntity.InsertionPoint.y
            TopTDViewEntity.InsertionPoint = New gPoint(
                  SideViewEntity.InsertionPoint.x,
                  C1P180.InsertionPoint.y + deltaYOfWD,
                  TopWDViewEntity.InsertionPoint.z)

            TopTDViewEntity.Layer.Frozen = False
            TopTDViewEntity.Layer.On = True
            TopTDViewEntity.Update()

            Doc.Redraw(True)

        End If

        If TopTTViewEntity Is Nothing AndAlso TopWDViewEntity IsNot Nothing Then
            TopTTViewEntity = TopWDViewEntity
            ' Check if the new layer already exists
            Dim newLayerName As String = "C2" + TopWDViewEntity.Layer.Name
            Dim TopTTViewEntitynewLayer As VectorDraw.Professional.vdPrimaries.vdLayer = Doc.Layers.FindName(newLayerName)

            ' If the layer does not exist, create a new one with the same properties
            If TopTTViewEntitynewLayer Is Nothing Then
                TopTTViewEntitynewLayer = New VectorDraw.Professional.vdPrimaries.vdLayer(Doc, newLayerName)

                ' Copy properties from TopWDViewEntity's Layer
                TopTTViewEntitynewLayer.PenColor = TopWDViewEntity.Layer.PenColor
                TopTTViewEntitynewLayer.LineType = TopWDViewEntity.Layer.LineType
                TopTTViewEntitynewLayer.Frozen = TopWDViewEntity.Layer.Frozen
                TopTTViewEntitynewLayer.On = TopWDViewEntity.Layer.On
                TopTTViewEntitynewLayer.LineWeight = TopWDViewEntity.Layer.LineWeight

                ' Add the new layer to the document
                Doc.Layers.AddItem(TopTTViewEntitynewLayer)
            End If

            ' Clone the entity first before assigning the layer
            TopTDViewEntity = CType(TopWDViewEntity.Clone(Doc), vdInsert)
            TopTDViewEntity.Layer = TopTTViewEntitynewLayer  ' Assign the new layer

            ' Add the cloned entity to the document
            Doc.ActiveLayOut.Entities.AddItem(TopTDViewEntity)

            ' Debugging message
            Debug.WriteLine("New layer TT assigned: " & TopTTViewEntity.Layer.Name)


            ' Refresh document



            Dim deltaYOfWD As Double
            deltaYOfWD = P180.InsertionPoint.y - TopWDViewEntity.InsertionPoint.y
            TopTDViewEntity.InsertionPoint = New gPoint(
                  SideViewEntity.InsertionPoint.x,
                  C2P180.InsertionPoint.y + deltaYOfWD,
                  TopWDViewEntity.InsertionPoint.z)

            TopTDViewEntity.Layer.Frozen = False
            TopTDViewEntity.Layer.On = True
            TopTDViewEntity.Update()

            Doc.Redraw(True)


        End If

        Doc.Redraw(True) ' Refresh the drawing after adding new entities

        Debug.WriteLine("TopWDAltitudeMarker = " + TopWDAltitudeMarker.ToString)
        Debug.WriteLine("TopTDAltitudeMarker = " + TopTDAltitudeMarker.ToString)
        Debug.WriteLine("TopTTAltitudeMarker = " + TopTTAltitudeMarker.ToString)
        Debug.WriteLine("P180 was ok\ " + P180.Layer.Name)
        Debug.WriteLine("C1P180 was ok\ " + C1P180.Layer.Name)
        Debug.WriteLine("C2P180 was ok\ " + C2P180.Layer.Name)
        Debug.WriteLine("Side Layer name is" + SideViewEntity.Layer.Name)
        Debug.WriteLine("TopWD Layer name is" + TopWDViewEntity.Layer.Name)
        Debug.WriteLine("TopTD Layer name is" + TopTDViewEntity.Layer.Name)
        Debug.WriteLine("TopTT Layer name is" + TopTTViewEntity.Layer.Name)
        Debug.WriteLine("Aft Layer name is" + AftViewEntity.Layer.Name)
    End Sub
    Private Sub VisibleOnForAllViews()
        'Debug.WriteLine("x:" + movedBlock.InsertionPoint.x.ToString + " y:" + movedBlock.InsertionPoint.y.ToString + " z:" + movedBlock.InsertionPoint.z.ToString)
        Try
            If SideViewEntity IsNot Nothing AndAlso SideViewEntity.Layer IsNot Nothing Then
                SideViewEntity.Layer.Frozen = False
                SideViewEntity.Layer.On = True
                If SideViewEntity.Layer.Document IsNot Nothing Then SideViewEntity.Layer.Document.Redraw(True)
            End If
            'start
            'WD
            If TopWDViewEntity IsNot Nothing AndAlso TopWDViewEntity.Layer IsNot Nothing AndAlso ToolsUtilites.altitudeComperator(TopWDViewEntity.InsertionPoint.z, TopWDAltitudeMarker) Then

                TopWDViewEntity.Layer.Frozen = False
                TopWDViewEntity.Layer.On = True
                TopWDViewEntity.FadeEffect = 0
                TopWDViewEntity.visibility = False

                TopTDViewEntity.Layer.Frozen = True
                TopTDViewEntity.Layer.On = False
                TopTDViewEntity.visibility = True

                TopTTViewEntity.Layer.Frozen = True
                TopTTViewEntity.Layer.On = False
                TopTTViewEntity.visibility = True

                TowViewMassage = "WD -> on; TD -> off; TT -> off."

            ElseIf TopWDViewEntity IsNot Nothing AndAlso TopWDViewEntity.Layer IsNot Nothing AndAlso Not ToolsUtilites.altitudeComperator(TopWDViewEntity.InsertionPoint.z, TopWDAltitudeMarker) Then
                TopWDViewEntity.Layer.Frozen = True
                TopWDViewEntity.Layer.On = False
                TopWDViewEntity.visibility = True
            End If
            If TopWDViewEntity.Layer.Document IsNot Nothing Then TopWDViewEntity.Layer.Document.Redraw(True)

            'TD
            If TopTDViewEntity IsNot Nothing AndAlso TopTDViewEntity.Layer IsNot Nothing AndAlso ToolsUtilites.altitudeComperator(TopTDViewEntity.InsertionPoint.z, TopTDAltitudeMarker) AndAlso Not ToolsUtilites.altitudeComperator(TopTDViewEntity.InsertionPoint.z, TopWDAltitudeMarker) Then
                TopTDViewEntity.Layer.Frozen = False
                TopTDViewEntity.Layer.On = True
                TopTDViewEntity.visibility = False
                TopTDViewEntity.FadeEffect = 0
                TopTDViewEntity.TransparencyMethod = vdFigure.TransparencyMethodEnum.Default

                TopWDViewEntity.Layer.Frozen = True
                TopWDViewEntity.Layer.On = False
                TopWDViewEntity.visibility = True

                TopTTViewEntity.Layer.Frozen = True
                TopTTViewEntity.Layer.On = False
                TopTTViewEntity.visibility = True

                TowViewMassage = "WD -> off; TD -> on; TT -> off; "
                TopTDViewEntity.Layer.Document.Redraw(True)

            ElseIf TopTDViewEntity IsNot Nothing AndAlso TopTDViewEntity.Layer IsNot Nothing AndAlso Not ToolsUtilites.altitudeComperator(TopTDViewEntity.InsertionPoint.z, TopTDAltitudeMarker) AndAlso Not ToolsUtilites.altitudeComperator(TopTDViewEntity.InsertionPoint.z, TopWDAltitudeMarker) Then
                TopTDViewEntity.Layer.Frozen = True
                TopTDViewEntity.Layer.On = False
                TopTDViewEntity.visibility = True
            End If
            If TopTDViewEntity.Layer.Document IsNot Nothing Then TopTDViewEntity.Layer.Document.Redraw(True)


            'TT

            If TopTTViewEntity IsNot Nothing AndAlso TopTTViewEntity.Layer IsNot Nothing AndAlso ToolsUtilites.altitudeComperator(TopTTViewEntity.InsertionPoint.z, TopTTAltitudeMarker) AndAlso Not ToolsUtilites.altitudeComperator(TopTTViewEntity.InsertionPoint.z, TopTDAltitudeMarker) Then
                TopTTViewEntity.Layer.Frozen = False
                TopTTViewEntity.Layer.On = True
                TopTTViewEntity.FadeEffect = 0
                TopTTViewEntity.visibility = False

                TopWDViewEntity.Layer.Frozen = True
                TopWDViewEntity.Layer.On = False
                TopWDViewEntity.visibility = True

                TopTDViewEntity.Layer.Frozen = True
                TopTDViewEntity.Layer.On = False
                TopTDViewEntity.visibility = True

                TowViewMassage = "WD -> off; TD -> off; TT -> on; "


            ElseIf TopTTViewEntity IsNot Nothing AndAlso TopTTViewEntity.Layer IsNot Nothing AndAlso ToolsUtilites.altitudeComperator(TopTTViewEntity.InsertionPoint.z, TopTDAltitudeMarker) Then
                TopTTViewEntity.Layer.Frozen = True
                TopTTViewEntity.Layer.On = False
                TopTTViewEntity.visibility = True
            End If
            If TopTTViewEntity.Layer.Document IsNot Nothing Then TopTTViewEntity.Layer.Document.Redraw(True)
            'finish


            If AftViewEntity IsNot Nothing AndAlso AftViewEntity.Layer IsNot Nothing Then
                AftViewEntity.Layer.Frozen = False
                AftViewEntity.Layer.On = True
                If AftViewEntity.Layer.Document IsNot Nothing Then AftViewEntity.Layer.Document.Redraw(True)
            End If



        Catch ex As Exception
            ' Log error if needed
        End Try
    End Sub



    Public Function MessageBoxText() As String
        Dim sb As New StringBuilder()
        Dim format As String = "{0,-7},{1,-12},{2,8:F0},{3,8:F0},{4,8:F0},-,{5,8:F0},{6,8:F0},{7,8:F0},|,{8,7:F0},{9,7:F0},{10,7:F0}"

        sb.AppendLine()
        sb.AppendLine("View,Layer Name,X,Y,Z,-,X,Y,Z,|,ΔX,ΔY,ΔZ")
 
        If TopWDViewEntity IsNot Nothing Then
            sb.AppendLine(String.Format(format, "Top WD", TopWDViewEntity.Layer.Name,
            initialXOfTopWDViewEntity, initialYOfTopWDViewEntity, initialZOfTopWDViewEntity,
            TopWDViewEntity.InsertionPoint.x, TopWDViewEntity.InsertionPoint.y, TopWDViewEntity.InsertionPoint.z,
            TopWDViewEntity.InsertionPoint.x - initialXOfTopWDViewEntity,
            TopWDViewEntity.InsertionPoint.y - initialYOfTopWDViewEntity,
            TopWDViewEntity.InsertionPoint.z - initialZOfTopWDViewEntity))
        End If

        If TopTDViewEntity IsNot Nothing Then
            sb.AppendLine(String.Format(format, "Top TD", TopTDViewEntity.Layer.Name,
            initialXOfTopTDViewEntity, initialYOfTopTDViewEntity, initialZOfTopTDViewEntity,
            TopTDViewEntity.InsertionPoint.x, TopTDViewEntity.InsertionPoint.y, TopTDViewEntity.InsertionPoint.z,
            TopTDViewEntity.InsertionPoint.x - initialXOfTopTDViewEntity,
            TopTDViewEntity.InsertionPoint.y - initialYOfTopTDViewEntity,
            TopTDViewEntity.InsertionPoint.z - initialZOfTopTDViewEntity))
        End If

        If TopTTViewEntity IsNot Nothing Then
            sb.AppendLine(String.Format(format, "Top TT", TopTTViewEntity.Layer.Name,
            initialXOfTopTTViewEntity, initialYOfTopTTViewEntity, initialZOfTopTTViewEntity,
            TopTTViewEntity.InsertionPoint.x, TopTTViewEntity.InsertionPoint.y, TopTTViewEntity.InsertionPoint.z,
            TopTTViewEntity.InsertionPoint.x - initialXOfTopTTViewEntity,
            TopTTViewEntity.InsertionPoint.y - initialYOfTopTTViewEntity,
            TopTTViewEntity.InsertionPoint.z - initialZOfTopTTViewEntity))
        End If

        If SideViewEntity IsNot Nothing Then
            sb.AppendLine(String.Format(format, "Top S", SideViewEntity.Layer.Name,
            initialXOfSideViewEntity, initialYOfSideViewEntity, initialZOfSideViewEntity,
            SideViewEntity.InsertionPoint.x, SideViewEntity.InsertionPoint.y, SideViewEntity.InsertionPoint.z,
            SideViewEntity.InsertionPoint.x - initialXOfSideViewEntity,
            SideViewEntity.InsertionPoint.y - initialYOfSideViewEntity,
            SideViewEntity.InsertionPoint.z - initialZOfSideViewEntity))
        End If

        If AftViewEntity IsNot Nothing Then
            sb.AppendLine(String.Format(format, "Top A", AftViewEntity.Layer.Name,
            initialXOfAftViewEntity, initialYOfAftViewEntity, initialZOfAftViewEntity,
            AftViewEntity.InsertionPoint.x, AftViewEntity.InsertionPoint.y, AftViewEntity.InsertionPoint.z,
            AftViewEntity.InsertionPoint.x - initialXOfAftViewEntity,
            AftViewEntity.InsertionPoint.y - initialYOfAftViewEntity,
            AftViewEntity.InsertionPoint.z - initialZOfAftViewEntity))
        End If


        Return TowViewMassage
    End Function


    Public Sub PopulateTable(ByRef View As DataGridView)
        ' Clear any existing data
        View.Columns.Clear()
        View.Rows.Clear()


        ' Define columns
        View.Columns.Add("View", "View")
        View.Columns.Add("Layer Name", "Layer Name")
        View.Columns.Add("X Before", "X Before")
        View.Columns.Add("Y Before", "Y Before")
        View.Columns.Add("Z Before", "Z Before")
        View.Columns.Add("-", "-") ' Separator
        View.Columns.Add("X After", "X After")
        View.Columns.Add("Y After", "Y After")
        View.Columns.Add("Z After", "Z After")
        View.Columns.Add("|", "|") ' Separator
        View.Columns.Add("ΔX", "ΔX")
        View.Columns.Add("ΔY", "ΔY")
        View.Columns.Add("ΔZ", "ΔZ")

        If TopWDViewEntity IsNot Nothing Then
            ' Add WD rows
            View.Rows.Add("Top WD",
                      TopWDViewEntity.Layer.Name,
                      CInt(initialXOfTopWDViewEntity),
                      CInt(initialYOfTopWDViewEntity),
                      CInt(initialZOfTopWDViewEntity),
                      "-",
                      CInt(TopWDViewEntity.InsertionPoint.x),
                      CInt(TopWDViewEntity.InsertionPoint.y),
                      CInt(TopWDViewEntity.InsertionPoint.z),
                      "-",
                       CInt(TopWDViewEntity.InsertionPoint.x - initialXOfTopWDViewEntity),
                       CInt(TopWDViewEntity.InsertionPoint.y - initialYOfTopWDViewEntity),
                       CInt(TopWDViewEntity.InsertionPoint.z - initialZOfTopWDViewEntity)
                       )

            ' Auto-size columns for better readability
            View.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells
            View.Refresh()
        End If

        If TopTDViewEntity IsNot Nothing Then
            ' Add TD rows
            View.Rows.Add("Top TD",
                      TopTDViewEntity.Layer.Name,
                      CInt(initialXOfTopTDViewEntity),
                      CInt(initialYOfTopTDViewEntity),
                      CInt(initialZOfTopTDViewEntity),
                      "-",
                      CInt(TopTDViewEntity.InsertionPoint.x),
                      CInt(TopTDViewEntity.InsertionPoint.y),
                      CInt(TopTDViewEntity.InsertionPoint.z),
                      "-",
                       CInt(TopTDViewEntity.InsertionPoint.x - initialXOfTopTDViewEntity),
                       CInt(TopTDViewEntity.InsertionPoint.y - initialYOfTopTDViewEntity),
                       CInt(TopTDViewEntity.InsertionPoint.z - initialZOfTopTDViewEntity)
                       )

            ' Auto-size columns for better readability
            View.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells
            View.Refresh()
        End If

        If TopTTViewEntity IsNot Nothing Then
            ' Add TT rows
            View.Rows.Add("Top TT",
                      TopTTViewEntity.Layer.Name,
                      CInt(initialXOfTopTTViewEntity),
                      CInt(initialYOfTopTTViewEntity),
                      CInt(initialZOfTopTTViewEntity),
                      "-",
                      CInt(TopTTViewEntity.InsertionPoint.x),
                      CInt(TopTTViewEntity.InsertionPoint.y),
                      CInt(TopTTViewEntity.InsertionPoint.z),
                      "-",
                       CInt(TopTTViewEntity.InsertionPoint.x - initialXOfTopTTViewEntity),
                       CInt(TopTTViewEntity.InsertionPoint.y - initialYOfTopTTViewEntity),
                       CInt(TopTTViewEntity.InsertionPoint.z - initialZOfTopTTViewEntity)
                       )

            ' Auto-size columns for better readability
            View.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells
            View.Refresh()
        End If

        If SideViewEntity IsNot Nothing Then
            ' Add S rows
            View.Rows.Add("Side",
                      SideViewEntity.Layer.Name,
                      CInt(initialXOfSideViewEntity),
                      CInt(initialYOfSideViewEntity),
                      CInt(initialZOfSideViewEntity),
                      "-",
                      CInt(SideViewEntity.InsertionPoint.x),
                      CInt(SideViewEntity.InsertionPoint.y),
                      CInt(SideViewEntity.InsertionPoint.z),
                      "-",
                       CInt(SideViewEntity.InsertionPoint.x - initialXOfSideViewEntity),
                       CInt(SideViewEntity.InsertionPoint.y - initialYOfSideViewEntity),
                       CInt(SideViewEntity.InsertionPoint.z - initialZOfSideViewEntity)
                       )

            ' Auto-size columns for better readability
            View.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells
            View.Refresh()
        End If

        If AftViewEntity IsNot Nothing Then
            ' Add A rows
            View.Rows.Add("AFT",
                      TopWDViewEntity.Layer.Name,
                      CInt(initialXOfAftViewEntity),
                      CInt(initialYOfAftViewEntity),
                      CInt(initialZOfAftViewEntity),
                      "-",
                      CInt(AftViewEntity.InsertionPoint.x),
                      CInt(AftViewEntity.InsertionPoint.y),
                      CInt(AftViewEntity.InsertionPoint.z),
                      "-",
                       CInt(AftViewEntity.InsertionPoint.x - initialXOfAftViewEntity),
                       CInt(AftViewEntity.InsertionPoint.y - initialYOfAftViewEntity),
                       CInt(AftViewEntity.InsertionPoint.z - initialZOfAftViewEntity)
                       )

            ' Auto-size columns for better readability
            View.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells
            View.Refresh()
        End If


    End Sub
    'TowViewMassage = sb.ToString()
    'Return TowViewMassage


    Private Sub AftViewMoving()
        Dim deltaYFromTopView = movedBlock.InsertionPoint.y - initialYOfMovedBlock
        Dim deltaXFromTopView = movedBlock.InsertionPoint.x - initialXOfMovedBlock

        SideViewEntity.InsertionPoint = New gPoint(
                         SideViewEntity.InsertionPoint.x,
                         movedBlock.InsertionPoint.y,
                         SideViewEntity.InsertionPoint.z + deltaXFromTopView)
        SideViewEntity.Update()

        TopWDViewEntity.InsertionPoint = New gPoint(
                         TopWDViewEntity.InsertionPoint.x,
                         TopWDViewEntity.InsertionPoint.y + deltaXFromTopView,
                         TopWDViewEntity.InsertionPoint.z + deltaYFromTopView)
        TopWDViewEntity.Update()

        TopTDViewEntity.InsertionPoint = New gPoint(
                      TopTDViewEntity.InsertionPoint.x,
                      TopTDViewEntity.InsertionPoint.y + deltaXFromTopView,
                      TopTDViewEntity.InsertionPoint.z + deltaYFromTopView)
        TopTDViewEntity.Update()

        TopTTViewEntity.InsertionPoint = New gPoint(
                      TopTTViewEntity.InsertionPoint.x,
                      TopTTViewEntity.InsertionPoint.y + deltaXFromTopView,
                      TopTTViewEntity.InsertionPoint.z + deltaYFromTopView)
        TopTTViewEntity.Update()


    End Sub

    Private Sub SideViewMoving()
        Dim deltaYFromTopView = movedBlock.InsertionPoint.y - initialYOfMovedBlock
        Dim deltaXFromTopView = movedBlock.InsertionPoint.x - initialXOfMovedBlock

        AftViewEntity.InsertionPoint = New gPoint(
                                AftViewEntity.InsertionPoint.x,
                                movedBlock.InsertionPoint.y,
                                AftViewEntity.InsertionPoint.z + deltaXFromTopView)
        AftViewEntity.Update()

        TopWDViewEntity.InsertionPoint = New gPoint(
                                movedBlock.InsertionPoint.x,
                                TopWDViewEntity.InsertionPoint.y,
                                TopWDViewEntity.InsertionPoint.z + deltaYFromTopView)
        TopWDViewEntity.Update()

        TopTDViewEntity.InsertionPoint = New gPoint(
                            movedBlock.InsertionPoint.x,
                            TopTDViewEntity.InsertionPoint.y,
                            TopTDViewEntity.InsertionPoint.z + deltaYFromTopView)
        TopTDViewEntity.Update()

        TopTTViewEntity.InsertionPoint = New gPoint(
                           movedBlock.InsertionPoint.x,
                           TopTTViewEntity.InsertionPoint.y,
                           TopTTViewEntity.InsertionPoint.z + deltaYFromTopView)
        TopTTViewEntity.Update()



    End Sub

    Private Sub TopTTMoving()
        Dim deltaXFromTopView = movedBlock.InsertionPoint.x - initialXOfMovedBlock
        Dim deltaYFromTopView = movedBlock.InsertionPoint.y - initialYOfMovedBlock

        SideViewEntity.InsertionPoint = New gPoint(
                             movedBlock.InsertionPoint.x,
                             SideViewEntity.InsertionPoint.y,
                             SideViewEntity.InsertionPoint.z + deltaYFromTopView)
        SideViewEntity.Update()

        AftViewEntity.InsertionPoint = New gPoint(
                               AftViewEntity.InsertionPoint.x + deltaYFromTopView,
                               AftViewEntity.InsertionPoint.y,
                               AftViewEntity.InsertionPoint.z + deltaXFromTopView)
        AftViewEntity.Update()

        TopWDViewEntity.InsertionPoint = New gPoint(
                              movedBlock.InsertionPoint.x,
                              movedBlock.InsertionPoint.y,
                              TopWDViewEntity.InsertionPoint.z)
        TopWDViewEntity.Update()

        TopTDViewEntity.InsertionPoint = New gPoint(
                            movedBlock.InsertionPoint.x,
                            movedBlock.InsertionPoint.y,
                            TopTTViewEntity.InsertionPoint.z)
        TopTDViewEntity.Update()
    End Sub

    Private Sub TopTDMoving()

        Dim deltaXFromTopView = movedBlock.InsertionPoint.x - initialXOfMovedBlock
        Dim deltaYFromTopView = movedBlock.InsertionPoint.y - initialYOfMovedBlock

        SideViewEntity.InsertionPoint = New gPoint(
                             movedBlock.InsertionPoint.x,
                             SideViewEntity.InsertionPoint.y,
                             SideViewEntity.InsertionPoint.z + deltaYFromTopView)
        SideViewEntity.Update()

        AftViewEntity.InsertionPoint = New gPoint(
                               AftViewEntity.InsertionPoint.x + deltaYFromTopView,
                               AftViewEntity.InsertionPoint.y,
                               AftViewEntity.InsertionPoint.z + deltaXFromTopView)
        AftViewEntity.Update()

        TopWDViewEntity.InsertionPoint = New gPoint(
                              movedBlock.InsertionPoint.x,
                              movedBlock.InsertionPoint.y,
                              TopWDViewEntity.InsertionPoint.z)
        TopWDViewEntity.Update()

        TopTTViewEntity.InsertionPoint = New gPoint(
                            movedBlock.InsertionPoint.x,
                            movedBlock.InsertionPoint.y,
                            TopTTViewEntity.InsertionPoint.z)
        TopTTViewEntity.Update()
    End Sub

    Private Sub TopWDMoving()

        Dim deltaXFromTopView = movedBlock.InsertionPoint.x - initialXOfMovedBlock
        Dim deltaYFromTopView = movedBlock.InsertionPoint.y - initialYOfMovedBlock

        SideViewEntity.InsertionPoint = New gPoint(
                             movedBlock.InsertionPoint.x,
                             SideViewEntity.InsertionPoint.y,
                             SideViewEntity.InsertionPoint.z + deltaYFromTopView)
        SideViewEntity.Update()

        AftViewEntity.InsertionPoint = New gPoint(
                               AftViewEntity.InsertionPoint.x + deltaYFromTopView,
                               AftViewEntity.InsertionPoint.y,
                               AftViewEntity.InsertionPoint.z + deltaXFromTopView)
        AftViewEntity.Update()

        TopTDViewEntity.InsertionPoint = New gPoint(
                              movedBlock.InsertionPoint.x,
                              movedBlock.InsertionPoint.y,
                              TopTDViewEntity.InsertionPoint.z)
        TopTDViewEntity.Update()

        TopTTViewEntity.InsertionPoint = New gPoint(
                            movedBlock.InsertionPoint.x,
                            movedBlock.InsertionPoint.y,
                            TopTTViewEntity.InsertionPoint.z)
        TopTTViewEntity.Update()
    End Sub

    Public Sub moveMirrorBlocksOldWay(movedBlock As vdInsert, initialX As Double, initialY As Double, initialZ As Double)
        intializeViewsEntities(movedBlock)


        Dim LayerNumber As String = ToolsUtilites.getLayerNumber(movedBlock.Layer.Name).ToString
        Dim movedLayerNumber As Integer = ToolsUtilites.getLayerNumber(movedBlock.Layer.Name) ' * getting movedBlock.Layer.Name
        Dim entities As vdEntities = Doc.ActiveLayOut.Entities

        ' create at once collection ot this block all entity
        ' If the moved block's layer starts with "U" (Top view)


        If ToolsUtilites.StartsWithU(movedBlock.Layer.ToString) Or ToolsUtilites.StartsWithC1U(movedBlock.Layer.ToString) Or ToolsUtilites.StartsWithC2U(movedBlock.Layer.ToString) Then
            For Each entity As vdFigure In entities
                If TypeOf entity Is vdInsert Then
                    Dim OtherViews As vdInsert = CType(entity, vdInsert)
                    Dim currentLayerNumber As Integer = ToolsUtilites.getLayerNumber(OtherViews.Layer.Name) ' * For Each entity getting Layer.Name


                    If OtherViews.Layer.Name.Contains(LayerNumber) AndAlso currentLayerNumber = movedLayerNumber Then

                        ' If the related block is a Side view
                        If ToolsUtilites.StartsWithSU(OtherViews.Layer.Name) Then
                            Dim deltaYFromTopView = movedBlock.InsertionPoint.y - initialY
                            OtherViews.InsertionPoint = New gPoint(
                                movedBlock.InsertionPoint.x,
                                OtherViews.InsertionPoint.y,
                                OtherViews.InsertionPoint.x + deltaYFromTopView)

                            OtherViews.Update()
                        End If
                        ' for Aft view
                        If ToolsUtilites.StartsWithAU(OtherViews.Layer.Name) Then
                            Dim deltaXFromTopView = movedBlock.InsertionPoint.x - initialX
                            Dim deltaYFromTopView = movedBlock.InsertionPoint.y - initialY

                            OtherViews.InsertionPoint = New gPoint(
                                OtherViews.InsertionPoint.x + deltaYFromTopView,
                                OtherViews.InsertionPoint.y,
                                OtherViews.InsertionPoint.z + deltaXFromTopView)
                            OtherViews.Update()

                        End If
                    End If
                    'OtherViews.Layer.Document.Redraw(True)
                    OtherViews.Update()
                End If
            Next
        End If

        ' If the moved block's layer starts with "SU" (Side view)
        If ToolsUtilites.StartsWithSU(movedBlock.Layer.ToString) Then
            For Each entity As vdFigure In entities
                If TypeOf entity Is vdInsert Then
                    Dim OtherViews As vdInsert = CType(entity, vdInsert)
                    Dim currentLayerNumber As Integer = ToolsUtilites.getLayerNumber(OtherViews.Layer.Name) ' * For Each entity getting Layer.Name
                    Dim deltaYFromTopView = movedBlock.InsertionPoint.y - initialY
                    Dim deltaXFromTopView = movedBlock.InsertionPoint.x - initialX


                    If OtherViews.Layer.Name.Contains(LayerNumber) AndAlso currentLayerNumber = movedLayerNumber Then
                        ' If the related block is a Top view


                        If ToolsUtilites.StartsWithU(OtherViews.Layer.Name) Then

                            OtherViews.InsertionPoint = New gPoint(
                                movedBlock.InsertionPoint.x,
                                OtherViews.InsertionPoint.y,
                                OtherViews.InsertionPoint.z + deltaYFromTopView)
                            ' Set transparency to fully opaque (no fading)
                            Dim newColor As VectorDraw.Professional.vdObjects.vdColor = OtherViews.Layer.PenColor
                            newColor.SystemColor = Color.FromArgb(255, newColor.SystemColor) ' Alpha = 255 (fully opaque)
                            OtherViews.Layer.PenColor = newColor

                            OtherViews.Layer.Frozen = False
                            OtherViews.Layer.On = True

                            OtherViews.Layer.Document.Redraw(True)
                            OtherViews.Update()


                        ElseIf ToolsUtilites.StartsWithC1U(OtherViews.Layer.Name) Then

                            OtherViews.InsertionPoint = New gPoint(
                                movedBlock.InsertionPoint.x,
                                OtherViews.InsertionPoint.y,
                                OtherViews.InsertionPoint.z + deltaYFromTopView)
                            OtherViews.Layer.Frozen = False
                            OtherViews.Layer.On = True
                            OtherViews.Layer.Document.Redraw(True)
                            OtherViews.Update()


                        ElseIf ToolsUtilites.StartsWithC2U(OtherViews.Layer.Name) Then

                            OtherViews.InsertionPoint = New gPoint(
                                movedBlock.InsertionPoint.x,
                                OtherViews.InsertionPoint.y,
                                OtherViews.InsertionPoint.z + deltaYFromTopView)
                            OtherViews.Layer.Frozen = False
                            OtherViews.Layer.On = True
                            OtherViews.Layer.Document.Redraw(True)
                            OtherViews.Update()
                        End If

                        ' for Aft view
                        If ToolsUtilites.StartsWithAU(OtherViews.Layer.Name) Then


                            OtherViews.InsertionPoint = New gPoint(
                                OtherViews.InsertionPoint.x,
                                movedBlock.InsertionPoint.y,
                                OtherViews.InsertionPoint.z + deltaXFromTopView)
                            OtherViews.Update()
                        End If

                    End If
                End If
            Next
        End If

        If ToolsUtilites.StartsWithAU(movedBlock.Layer.ToString) Then
            For Each entity As vdFigure In entities
                If TypeOf entity Is vdInsert Then
                    Dim OtherViews As vdInsert = CType(entity, vdInsert)
                    Dim currentLayerNumber As Integer = ToolsUtilites.getLayerNumber(OtherViews.Layer.Name) ' * For Each entity getting Layer.Name
                    Dim deltaYFromTopView = movedBlock.InsertionPoint.y - initialY
                    Dim deltaXFromTopView = movedBlock.InsertionPoint.x - initialX

                    If OtherViews.Layer.Name.Contains(LayerNumber) AndAlso currentLayerNumber = movedLayerNumber Then
                        ' If the related block is a Top view
                        If ToolsUtilites.StartsWithSU(OtherViews.Layer.Name) Then

                            OtherViews.InsertionPoint = New gPoint(
                              OtherViews.InsertionPoint.x,
                              movedBlock.InsertionPoint.y,
                              OtherViews.InsertionPoint.z + deltaXFromTopView)

                            OtherViews.Layer.Frozen = False
                            OtherViews.Layer.On = True
                            OtherViews.Layer.Document.Redraw(True)
                            OtherViews.Update()

                        End If

                        If ToolsUtilites.StartsWithU(OtherViews.Layer.Name) Then
                            OtherViews.InsertionPoint = New gPoint(
                              OtherViews.InsertionPoint.x,
                              OtherViews.InsertionPoint.y + deltaXFromTopView,
                              OtherViews.InsertionPoint.z + deltaYFromTopView)
                            OtherViews.Layer.Frozen = False
                            OtherViews.Layer.On = True
                            OtherViews.Layer.Document.Redraw(True)
                            OtherViews.Update()
                        End If

                        If ToolsUtilites.StartsWithC1U(OtherViews.Layer.Name) Then
                            OtherViews.InsertionPoint = New gPoint(
                              OtherViews.InsertionPoint.x,
                              OtherViews.InsertionPoint.y + deltaXFromTopView,
                              OtherViews.InsertionPoint.z + deltaYFromTopView)
                            OtherViews.Layer.Frozen = False
                            OtherViews.Layer.On = True
                            OtherViews.Layer.Document.Redraw(True)
                            OtherViews.Update()
                        End If

                        If ToolsUtilites.StartsWithC2U(OtherViews.Layer.Name) Then
                            OtherViews.InsertionPoint = New gPoint(
                              OtherViews.InsertionPoint.x,
                              OtherViews.InsertionPoint.y + deltaXFromTopView,
                              OtherViews.InsertionPoint.z + deltaYFromTopView)
                            OtherViews.Layer.Frozen = False
                            OtherViews.Layer.On = True
                            OtherViews.Layer.Document.Redraw(True)
                            OtherViews.Update()
                        End If


                    End If
                End If
            Next
        End If

    End Sub


End Class
