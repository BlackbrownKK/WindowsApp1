Imports VectorDraw.Professional.vdCollections
Imports VectorDraw.Geometry
Imports VectorDraw.Render
Imports VectorDraw.Serialize
Imports VectorDraw.Generics
Imports VectorDraw.Professional.vdFigures
Imports VectorDraw.Professional.vdPrimaries
Imports VectorDraw.Professional.vdObjects
Imports vdControls
Imports System.Security.Principal
Imports VDrawI5
Imports vdInsert = VectorDraw.Professional.vdFigures.vdInsert
Imports vdFigure = VectorDraw.Professional.vdPrimaries.vdFigure
Imports vdSelection = VectorDraw.Professional.vdCollections.vdSelection
Imports vdEntities = VectorDraw.Professional.vdCollections.vdEntities
Imports System.DirectoryServices.ActiveDirectory


Public Class Form1
    WithEvents Basedoc As VectorDraw.Professional.Control.VectorDrawBaseControl
    Dim WithEvents Doc As VectorDraw.Professional.vdObjects.vdDocument
    Dim ShowActionEntities As Boolean
    Dim GravityMod As Boolean


    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        Doc = VdFramedControl.BaseControl.ActiveDocument
        Basedoc = VdFramedControl.BaseControl
        ShowActionEntities = False
        GravityMod = CheckBoxGravity.Checked


    End Sub




    Private Sub Basedoc_vdMouseDown(ByVal e As System.Windows.Forms.MouseEventArgs, ByRef cancel As Boolean) Handles Basedoc.vdMouseDown
        If e.Button <> MouseButtons.Left Then Return
        If (Doc.CommandAction.OpenLoops > 0) Then Return

        Dim gpt As gPoint = Doc.ActiveLayOut.ActiveActionRender.World2Pixelmatrix.Transform(Doc.ActiveLayOut.OverAllActiveAction.MouseLocation)

        '' Dim selset As vdSelection = vd.BaseControl.ActiveDocument.ActionUtility.getUserSelection()
        Dim userpt As New POINT(CInt(gpt.x), CInt(gpt.y))




        Using fig As vdFigure = Doc.ActiveLayOut.GetEntityFromPoint(userpt, Doc.ActiveLayOut.ActiveActionRender.GlobalProperties.PickSize, False)


            '' Dim code As VectorDraw.Actions.StatusCode = vd.BaseControl.ActiveDocument.ActionUtility.getUserEntity(fig, userpt)

            If (Not fig Is Nothing) Then

                Dim chosenBlock = CType(fig, vdInsert)

                Dim selection As New vdSelection()
                selection.Add(chosenBlock)
                ' Save the initial y position of the chosen block before moving on the top voew
                Dim initialYonAftView As Double = chosenBlock.InsertionPoint.y


                Doc.CommandAction.CmdMove(selection, chosenBlock.InsertionPoint, Nothing)


                ' If GravityMod is True, get the adjusted Y position using GetGravityPoint
                If GravityMod AndAlso ToolsUtilites.StartsWithSU(chosenBlock.Layer.Name.ToString) Then

                    Dim adjustedY As Double = GetGravityPoint(chosenBlock)
                    chosenBlock.InsertionPoint = New gPoint(chosenBlock.InsertionPoint.x, adjustedY, 0)  ' Update the Y position
                End If

                moveMirrorBlocks(chosenBlock, initialYonAftView)
                Doc.Redraw(True)


            End If
        End Using
    End Sub

    Private Function getBlockNumber(name As String) As String
        ' Remove non-numeric characters
        Dim numericString As String = ""
        For Each ch As Char In name
            If Char.IsDigit(ch) Then
                numericString &= ch
            End If
        Next
        getBlockNumber = numericString
    End Function

    ' extracting the numeric parts from the layer names And compare them. 
    ' reusing existing getBlockNumber function to get the numeric part from both the moved block's layer and the current insert’s layer. 

    Private Sub moveMirrorBlocks(movedBlock As vdInsert, initialY As Double)

        ' Get the block number from the moved block's name
        Dim blockNumber As String = getBlockNumber(movedBlock.Block.Name)
        Dim blocks As vdSelection = New vdSelection
        Dim movedLayerNumber As String = getBlockNumber(movedBlock.Layer.Name) ' * getting movedBlock.Layer.Name
        Dim entities As vdEntities = Doc.ActiveLayOut.Entities

        ' If the moved block's layer starts with "U" (Top view)
        If ToolsUtilites.StartsWithU(movedBlock.Layer.ToString) Then
            For Each entity As vdFigure In entities
                If TypeOf entity Is vdInsert Then
                    Dim OtherViews As vdInsert = CType(entity, vdInsert)
                    Dim currentLayerNumber As String = getBlockNumber(OtherViews.Layer.Name) ' * For Each entity getting Layer.Name


                    If OtherViews.Block.Name.Contains(blockNumber) AndAlso currentLayerNumber = movedLayerNumber Then
                        ' If the related block is a Side view
                        If ToolsUtilites.StartsWithSU(OtherViews.Layer.Name) Then
                            OtherViews.InsertionPoint = New gPoint(movedBlock.InsertionPoint.x, OtherViews.InsertionPoint.y, 0)
                            OtherViews.Update()

                        End If

                        ' for Aft view
                        If ToolsUtilites.StartsWithAU(OtherViews.Layer.Name) Then
                            Dim deltaFromTopView = initialY - movedBlock.InsertionPoint.y
                            OtherViews.InsertionPoint = New gPoint(OtherViews.InsertionPoint.x + deltaFromTopView, OtherViews.InsertionPoint.y, 0)
                            OtherViews.Update()

                        End If

                    End If
                End If


            Next
        End If

        ' If the moved block's layer starts with "SU" (Side view)
        If ToolsUtilites.StartsWithSU(movedBlock.Layer.ToString) Then
            For Each entity As vdFigure In entities
                If TypeOf entity Is vdInsert Then
                    Dim OtherViews As vdInsert = CType(entity, vdInsert)
                    Dim currentLayerNumber As String = getBlockNumber(OtherViews.Layer.Name) ' * For Each entity getting Layer.Name


                    If OtherViews.Block.Name.Contains(blockNumber) AndAlso currentLayerNumber = movedLayerNumber Then
                        ' If the related block is a Top view
                        If ToolsUtilites.StartsWithU(OtherViews.Layer.Name) Then
                            OtherViews.InsertionPoint = New gPoint(movedBlock.InsertionPoint.x, OtherViews.InsertionPoint.y, 0)
                            OtherViews.Update()


                        End If

                        ' for Aft view
                        If ToolsUtilites.StartsWithAU(OtherViews.Layer.Name) Then
                            OtherViews.InsertionPoint = New gPoint(OtherViews.InsertionPoint.x, movedBlock.InsertionPoint.y, 0)
                            OtherViews.Update()

                        End If

                    End If
                End If


            Next
        End If


    End Sub


    Private Function GetGravityPoint(ByVal cargo As vdInsert) As Double

        Dim cargoResultY As Double = cargo.BoundingBox.LowerRight.y


        Dim YList As New List(Of Double) ' create list of double

        Dim entities As vdEntities = Doc.ActiveLayOut.Entities
        Debug.WriteLine("cargoResultY = " + cargoResultY.ToString)
        'Loop through all entities in the draw.
        For Each entity As vdFigure In entities

            Dim tempYOfEntity As Double = entity.BoundingBox.UpperLeft.y
            If ToolsUtilites.StartsWithS(entity.Layer.Name.ToString) AndAlso entity.BoundingBox IsNot Nothing AndAlso tempYOfEntity <= cargoResultY Then
                YList.Add(tempYOfEntity)
                Debug.WriteLine(tempYOfEntity.ToString)


            End If
        Next

        Dim DistanceToNextBody As Double = Double.MaxValue
        For Each number As Double In YList
            Dim YDelta As Double = cargoResultY - number
            If (YDelta <= DistanceToNextBody) Then
                DistanceToNextBody = YDelta
            End If
        Next



        ' If no valid entity was found, return the original cargo Y position
        If cargoResultY = Double.MaxValue Then
            Return cargo.InsertionPoint.y
        Else
            Return cargo.InsertionPoint.y - DistanceToNextBody
        End If
        Return cargoResultY

    End Function



    Private Sub Button2_Click_1(sender As Object, e As EventArgs) Handles ButOpen.Click
        ' Create and configure an OpenFileDialog instance.
        Dim openFileDialog As New OpenFileDialog()
        openFileDialog.Filter = "VectorDraw Files|*.vdcl;*.dwg;*.dxf"
        openFileDialog.Title = "Open VectorDraw File"

        If openFileDialog.ShowDialog() = DialogResult.OK Then
            If Doc.Open(openFileDialog.FileName) Then
                Doc.Redraw(True)
            Else
                MessageBox.Show("Failed to open file.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If
        End If
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles ButtonSave.Click
        Dim saveFileDialog As New SaveFileDialog()
        saveFileDialog.Filter = "VectorDraw Files|*.vdcl;*.dwg;*.dxf"
        saveFileDialog.Title = "Save VectorDraw File"
        If saveFileDialog.ShowDialog() = DialogResult.OK Then
            Try
                ' Attempt to save the document using the provided filename.
                If Doc.SaveAs(saveFileDialog.FileName) Then
                    MessageBox.Show("File saved successfully.", "Save", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Else
                    MessageBox.Show("Failed to save file.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                End If
            Catch ex As Exception
                MessageBox.Show("An error occurred while saving the file: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End If
    End Sub

    Private Sub Button1_Click_1(sender As Object, e As EventArgs)

    End Sub



    Private Sub CheckBoxGravity_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBoxGravity.CheckedChanged
        If (GravityMod) Then
            GravityMod = False
        Else
            GravityMod = True
        End If

    End Sub
End Class
