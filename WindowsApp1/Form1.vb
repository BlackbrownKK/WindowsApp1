Imports VectorDraw.Geometry
Imports vdControls
Imports vdInsert = VectorDraw.Professional.vdFigures.vdInsert
Imports vdFigure = VectorDraw.Professional.vdPrimaries.vdFigure
Imports vdSelection = VectorDraw.Professional.vdCollections.vdSelection
Imports vdEntities = VectorDraw.Professional.vdCollections.vdEntities


Public Class Form1

    WithEvents Basedoc As VectorDraw.Professional.Control.VectorDrawBaseControl
    Dim WithEvents Doc As VectorDraw.Professional.vdObjects.vdDocument
    Dim ShowActionEntities As Boolean
    Dim GravityMod As Boolean
    Private MovingCtrl As MovingController
    Dim TextMassage As String


    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Doc = VdFramedControl.BaseControl.ActiveDocument
        Basedoc = VdFramedControl.BaseControl
        ShowActionEntities = False
        GravityMod = CheckBoxGravity.Checked
        MovingCtrl = New MovingController(Doc) ' Initialize MovingController
    End Sub




    Private Sub Basedoc_vdMouseDown(ByVal e As MouseEventArgs, ByRef cancel As Boolean) Handles Basedoc.vdMouseDown

        If e.Button <> MouseButtons.Left Then Return
        If (Doc.CommandAction.OpenLoops > 0) Then Return


        Dim gpt As gPoint = Doc.ActiveLayOut.ActiveActionRender.World2Pixelmatrix.Transform(Doc.ActiveLayOut.OverAllActiveAction.MouseLocation)
        Dim userpt As New POINT(CInt(gpt.x), CInt(gpt.y))
        Using fig As vdFigure = Doc.ActiveLayOut.GetEntityFromPoint(userpt, Doc.ActiveLayOut.ActiveActionRender.GlobalProperties.PickSize, False)
            If (Not fig Is Nothing) Then
                Dim chosenBlock = CType(fig, vdInsert)
                Dim selection As New vdSelection()
                selection.Add(chosenBlock)


                If (CheckBox_Delete.Checked) Then
                    Doc.ActiveLayOut.Entities.RemoveItem(chosenBlock) ' Remove the entity
                    Doc.Redraw(True) ' Refresh the UI to reflect changes
                    Return
                End If

                ' Save the initial XYZ position of the chosen block before moving on the top view. It is for Aft view.
                Dim initialXofAftView As Double = chosenBlock.InsertionPoint.x
                Dim initialYofAftView As Double = chosenBlock.InsertionPoint.y
                Dim initialZofAftView As Double = chosenBlock.InsertionPoint.z



                TextMassage = $"Before moving: X: {chosenBlock.InsertionPoint.x} , Y: {chosenBlock.InsertionPoint.y}, Z: {chosenBlock.InsertionPoint.z}"

                Doc.CommandAction.CmdMove(selection, chosenBlock.InsertionPoint, Nothing)
                ' If GravityMod is True, get the adjusted Y position using GetGravityPoint
                If GravityMod AndAlso ToolsUtilites.StartsWithSU(chosenBlock.Layer.Name.ToString) Then ' for GravityMod is on
                    Dim adjustedY As Double = GetGravityPoint(chosenBlock)
                    chosenBlock.InsertionPoint = New gPoint(chosenBlock.InsertionPoint.x, adjustedY, 0)  ' Update the Y position
                End If

                If Not btnMirrirOff.Checked Then
                    MovingCtrl.moveMirrorBlocks(chosenBlock, initialXofAftView, initialYofAftView, initialZofAftView)
                    TextMassage += $"{Environment.NewLine} After moving: X: {chosenBlock.InsertionPoint.x}, Y: {chosenBlock.InsertionPoint.y}, Z: {chosenBlock.InsertionPoint.z}, {Environment.NewLine}{MovingCtrl.MessageBoxText()}"
                    massageBox.Text = TextMassage
                    Doc.Redraw(True)
                End If
            End If

        End Using
    End Sub



    Private Function GetGravityPoint(ByVal cargo As vdInsert) As Double
        Dim cargoResultY As Double = cargo.BoundingBox.LowerRight.y
        Dim YList As New List(Of Double) ' create list of double
        Dim entities As vdEntities = Doc.ActiveLayOut.Entities

        'Loop through all entities in the draw.
        For Each entity As vdFigure In entities
            Dim tempYOfEntity As Double = entity.BoundingBox.UpperLeft.y
            If ToolsUtilites.StartsWithS(entity.Layer.Name.ToString) AndAlso entity.BoundingBox IsNot Nothing AndAlso tempYOfEntity <= cargoResultY Then
                YList.Add(tempYOfEntity)

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
        openFileDialog.Filter = "VectorDraw Files|*.vdcl;*.dwg;*.dxf;*.vdml;"
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





    Private Sub CheckBoxGravity_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBoxGravity.CheckedChanged
        If (GravityMod) Then
            GravityMod = False
        Else
            GravityMod = True
        End If

    End Sub

    Private Sub massageBox_TextChanged(sender As Object, e As EventArgs) Handles massageBox.TextChanged
        ' Disable the event temporarily to prevent feedback loop
        RemoveHandler massageBox.TextChanged, AddressOf massageBox_TextChanged
        massageBox.Text = TextMassage
        ' Re-enable the event after updating the text
        AddHandler massageBox.TextChanged, AddressOf massageBox_TextChanged
    End Sub

    Private Sub btnMirrirOff_CheckedChanged(sender As Object, e As EventArgs) Handles btnMirrirOff.CheckedChanged

    End Sub

    Private Sub Button_Undo_Click(sender As Object, e As EventArgs) Handles Button_Undo.Click
        Doc.CommandAction.Undo("")
    End Sub

    Private Sub Button_Redo_Click(sender As Object, e As EventArgs) Handles Button_Redo.Click
        Doc.CommandAction.Redo()
    End Sub




    Private Sub Button_Print_Click(sender As Object, e As EventArgs) Handles Button_Print.Click
        Doc.CommandAction.CmdPrintDialog(Nothing, Nothing)
    End Sub

    Private Sub CheckBox1_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox_Delete.CheckedChanged

    End Sub
End Class
