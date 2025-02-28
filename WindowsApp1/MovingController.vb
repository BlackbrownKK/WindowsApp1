Imports VectorDraw.Geometry
Imports vdInsert = VectorDraw.Professional.vdFigures.vdInsert
Imports vdFigure = VectorDraw.Professional.vdPrimaries.vdFigure
Imports vdEntities = VectorDraw.Professional.vdCollections.vdEntities

Public Class MovingController



    Private WithEvents Doc As VectorDraw.Professional.vdObjects.vdDocument

    Dim SideViewEntiti As vdInsert
    Dim TopWDViewEntiti As vdInsert
    Dim TopTDViewEntiti As vdInsert
    Dim TopTTViewEntiti As vdInsert
    Dim AftViewEntiti As vdInsert

    Dim initialX As Double
    Dim initialY As Double
    Dim initialZ As Double

    Dim movedBlock As vdInsert
    Dim AllEntities As vdEntities


    ' Constructor to initialize with a document reference
    Public Sub New(document As VectorDraw.Professional.vdObjects.vdDocument)
        Me.Doc = document
    End Sub


    Private Sub intializeViewsEntities(movedBlock As vdInsert)

        Me.movedBlock = movedBlock

        ' getting the Layer name from the moved block's name
        Dim LayerNumber As String = ToolsUtilites.getLayerNumber(movedBlock.Layer.Name).ToString
        ' getting the Layer number from the moved block's name
        Dim movedLayerNumber As Integer = ToolsUtilites.getLayerNumber(movedBlock.Layer.Name)

        Me.AllEntities = Doc.ActiveLayOut.Entities

        For Each otherViewsEntity As vdFigure In AllEntities
            If TypeOf otherViewsEntity Is vdInsert Then
                Dim otherViewsTemp As vdInsert = CType(otherViewsEntity, vdInsert)
                ' * For Each entity getting Layer number
                Dim otherViewsEntityLayerNumber As Integer = ToolsUtilites.getLayerNumber(otherViewsTemp.Layer.Name)
                If otherViewsEntity.Layer.Name.Contains(LayerNumber) AndAlso otherViewsEntityLayerNumber = movedLayerNumber Then
                    Select Case True
                        Case ToolsUtilites.StartsWithSU(otherViewsTemp.Layer.ToString)
                            SideViewEntiti = otherViewsTemp
                            Debug.WriteLine("Side View Found: " & otherViewsTemp.Layer.Name)
                        Case ToolsUtilites.StartsWithU(otherViewsTemp.Layer.ToString)
                            TopWDViewEntiti = otherViewsTemp
                            Debug.WriteLine("Top WD View Found: " & otherViewsTemp.Layer.Name)
                        Case ToolsUtilites.StartsWithC1U(otherViewsTemp.Layer.ToString)
                            TopTDViewEntiti = otherViewsTemp
                            Debug.WriteLine("Top TD View Found: " & otherViewsTemp.Layer.Name)
                        Case ToolsUtilites.StartsWithC2U(otherViewsTemp.Layer.ToString)
                            TopTTViewEntiti = otherViewsTemp
                            Debug.WriteLine("Top TT View Found: " & otherViewsTemp.Layer.Name)
                        Case ToolsUtilites.StartsWithAU(otherViewsTemp.Layer.ToString)
                            AftViewEntiti = otherViewsTemp
                            Debug.WriteLine("Aft View Found: " & otherViewsTemp.Layer.Name)
                        Case Else
                            Debug.WriteLine("No matching view found for: " & otherViewsTemp.Layer.Name)
                    End Select

                End If
            End If
        Next

    End Sub

    Public Sub moveMirrorBlocksOpt2(movedBlock As vdInsert, initialX As Double, initialY As Double, initialZ As Double)
        intializeViewsEntities(movedBlock)

        Me.initialX = initialX
        Me.initialY = initialY
        Me.initialZ = initialZ


        If movedBlock.Equals(TopWDViewEntiti) Then
            TopWDMoving()
        ElseIf movedBlock.Equals(TopTDViewEntiti) Then
            TopTDMoving()
        ElseIf movedBlock.Equals(TopTTViewEntiti) Then
            TopTTMoving()
        ElseIf movedBlock.Equals(SideViewEntiti) Then
            SideViewMoving()
        ElseIf movedBlock.Equals(AftViewEntiti) Then
            AftViewMoving()
        End If

        VisibleOnForAllViews()

    End Sub

    Private Sub VisibleOnForAllViews()

        SideViewEntiti.Layer.Frozen = False
        TopWDViewEntiti.Layer.Frozen = False
        TopTDViewEntiti.Layer.Frozen = False
        TopTTViewEntiti.Layer.Frozen = False
        AftViewEntiti.Layer.Frozen = False

        SideViewEntiti.Layer.On = True
        TopWDViewEntiti.Layer.On = True
        TopTDViewEntiti.Layer.On = True
        TopTTViewEntiti.Layer.On = True
        AftViewEntiti.Layer.On = True

        SideViewEntiti.Layer.Document.Redraw(True)
        TopWDViewEntiti.Layer.Document.Redraw(True)
        TopTDViewEntiti.Layer.Document.Redraw(True)
        TopTTViewEntiti.Layer.Document.Redraw(True)
        AftViewEntiti.Layer.Document.Redraw(True)

    End Sub

    Private Sub AftViewMoving()
        Dim deltaYFromTopView = movedBlock.InsertionPoint.y - initialY
        Dim deltaXFromTopView = movedBlock.InsertionPoint.x - initialX

        SideViewEntiti.InsertionPoint = New gPoint(
                         SideViewEntiti.InsertionPoint.x,
                         movedBlock.InsertionPoint.y,
                         SideViewEntiti.InsertionPoint.z + deltaXFromTopView)
        SideViewEntiti.Update()

        TopWDViewEntiti.InsertionPoint = New gPoint(
                         TopWDViewEntiti.InsertionPoint.x,
                         TopWDViewEntiti.InsertionPoint.y + deltaXFromTopView,
                         TopWDViewEntiti.InsertionPoint.z + deltaYFromTopView)
        TopWDViewEntiti.Update()

        TopTDViewEntiti.InsertionPoint = New gPoint(
                      TopTDViewEntiti.InsertionPoint.x,
                      TopTDViewEntiti.InsertionPoint.y + deltaXFromTopView,
                      TopTDViewEntiti.InsertionPoint.z + deltaYFromTopView)
        TopTDViewEntiti.Update()

        TopTTViewEntiti.InsertionPoint = New gPoint(
                      TopTTViewEntiti.InsertionPoint.x,
                      TopTTViewEntiti.InsertionPoint.y + deltaXFromTopView,
                      TopTTViewEntiti.InsertionPoint.z + deltaYFromTopView)
        TopTTViewEntiti.Update()


    End Sub

    Private Sub SideViewMoving()
        Dim deltaYFromTopView = movedBlock.InsertionPoint.y - initialY
        Dim deltaXFromTopView = movedBlock.InsertionPoint.x - initialX

        AftViewEntiti.InsertionPoint = New gPoint(
                                AftViewEntiti.InsertionPoint.x,
                                movedBlock.InsertionPoint.y,
                                AftViewEntiti.InsertionPoint.z + deltaXFromTopView)
        AftViewEntiti.Update()

        TopWDViewEntiti.InsertionPoint = New gPoint(
                                movedBlock.InsertionPoint.x,
                                TopWDViewEntiti.InsertionPoint.y,
                                TopWDViewEntiti.InsertionPoint.z + deltaYFromTopView)
        TopWDViewEntiti.Update()

        TopTDViewEntiti.InsertionPoint = New gPoint(
                            movedBlock.InsertionPoint.x,
                            TopTDViewEntiti.InsertionPoint.y,
                            TopTDViewEntiti.InsertionPoint.z + deltaYFromTopView)
        TopTDViewEntiti.Update()

        TopTTViewEntiti.InsertionPoint = New gPoint(
                           movedBlock.InsertionPoint.x,
                           TopTTViewEntiti.InsertionPoint.y,
                           TopTTViewEntiti.InsertionPoint.z + deltaYFromTopView)
        TopTTViewEntiti.Update()



    End Sub

    Private Sub TopTTMoving()
        Dim deltaXFromTopView = movedBlock.InsertionPoint.x - initialX
        Dim deltaYFromTopView = movedBlock.InsertionPoint.y - initialY

        SideViewEntiti.InsertionPoint = New gPoint(
                             movedBlock.InsertionPoint.x,
                             SideViewEntiti.InsertionPoint.y,
                             SideViewEntiti.InsertionPoint.z + deltaYFromTopView)
        SideViewEntiti.Update()

        AftViewEntiti.InsertionPoint = New gPoint(
                               AftViewEntiti.InsertionPoint.x + deltaYFromTopView,
                               AftViewEntiti.InsertionPoint.y,
                               AftViewEntiti.InsertionPoint.z + deltaXFromTopView)
        AftViewEntiti.Update()

        TopWDViewEntiti.InsertionPoint = New gPoint(
                              movedBlock.InsertionPoint.x,
                              movedBlock.InsertionPoint.y,
                              TopWDViewEntiti.InsertionPoint.z)
        TopWDViewEntiti.Update()

        TopTDViewEntiti.InsertionPoint = New gPoint(
                            movedBlock.InsertionPoint.x,
                            movedBlock.InsertionPoint.y,
                            TopTTViewEntiti.InsertionPoint.z)
        TopTDViewEntiti.Update()
    End Sub

    Private Sub TopTDMoving()

        Dim deltaXFromTopView = movedBlock.InsertionPoint.x - initialX
        Dim deltaYFromTopView = movedBlock.InsertionPoint.y - initialY

        SideViewEntiti.InsertionPoint = New gPoint(
                             movedBlock.InsertionPoint.x,
                             SideViewEntiti.InsertionPoint.y,
                             SideViewEntiti.InsertionPoint.z + deltaYFromTopView)
        SideViewEntiti.Update()

        AftViewEntiti.InsertionPoint = New gPoint(
                               AftViewEntiti.InsertionPoint.x + deltaYFromTopView,
                               AftViewEntiti.InsertionPoint.y,
                               AftViewEntiti.InsertionPoint.z + deltaXFromTopView)
        AftViewEntiti.Update()

        TopWDViewEntiti.InsertionPoint = New gPoint(
                              movedBlock.InsertionPoint.x,
                              movedBlock.InsertionPoint.y,
                              TopWDViewEntiti.InsertionPoint.z)
        TopWDViewEntiti.Update()

        TopTTViewEntiti.InsertionPoint = New gPoint(
                            movedBlock.InsertionPoint.x,
                            movedBlock.InsertionPoint.y,
                            TopTTViewEntiti.InsertionPoint.z)
        TopTTViewEntiti.Update()
    End Sub

    Private Sub TopWDMoving()

        Dim deltaXFromTopView = movedBlock.InsertionPoint.x - initialX
        Dim deltaYFromTopView = movedBlock.InsertionPoint.y - initialY

        SideViewEntiti.InsertionPoint = New gPoint(
                             movedBlock.InsertionPoint.x,
                             SideViewEntiti.InsertionPoint.y,
                             SideViewEntiti.InsertionPoint.z + deltaYFromTopView)
        SideViewEntiti.Update()

        AftViewEntiti.InsertionPoint = New gPoint(
                               AftViewEntiti.InsertionPoint.x + deltaYFromTopView,
                               AftViewEntiti.InsertionPoint.y,
                               AftViewEntiti.InsertionPoint.z + deltaXFromTopView)
        AftViewEntiti.Update()

        TopTDViewEntiti.InsertionPoint = New gPoint(
                              movedBlock.InsertionPoint.x,
                              movedBlock.InsertionPoint.y,
                              TopTDViewEntiti.InsertionPoint.z)
        TopTDViewEntiti.Update()

        TopTTViewEntiti.InsertionPoint = New gPoint(
                            movedBlock.InsertionPoint.x,
                            movedBlock.InsertionPoint.y,
                            TopTTViewEntiti.InsertionPoint.z)
        TopTTViewEntiti.Update()
    End Sub

    Public Sub moveMirrorBlocks(movedBlock As vdInsert, initialX As Double, initialY As Double, initialZ As Double)
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
