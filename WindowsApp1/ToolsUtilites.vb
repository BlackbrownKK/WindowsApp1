Imports System.Text.RegularExpressions
Imports VDrawI5

Public Class ToolsUtilites

    'layer whose name starts with "SU" (indicating a Side view),
    'the code adjusts related "Aft" views to have the Y position match the Side view.
    Public Shared Function StartsWithSU(ByVal input As String) As Boolean
        input = input.ToLower
        Return input.StartsWith("su", StringComparison.OrdinalIgnoreCase) OrElse input.StartsWith("sq", StringComparison.OrdinalIgnoreCase)
    End Function

    'if the related block is an "Aft" view (identified by the "AU" prefix in the layer name),
    'its X position should be adjusted to match the Y position of the Top view.
    Public Shared Function StartsWithAU(ByVal input As String) As Boolean
        input = input.ToLower
        Return input.StartsWith("au", StringComparison.OrdinalIgnoreCase) OrElse input.StartsWith("aq", StringComparison.OrdinalIgnoreCase)
    End Function

    'If the moved block is on a layer whose name starts with "U" (indicating a Top view),
    'the code ensures that other blocks related to this view (e.g., Aft and Side views)
    'have their insertion points updated accordingly.
    Public Shared Function StartsWithU(ByVal input As String) As Boolean
        input = input.ToLower
        Return input.StartsWith("u", StringComparison.OrdinalIgnoreCase) OrElse input.StartsWith("q", StringComparison.OrdinalIgnoreCase)
    End Function

    Public Shared Function StartsWithC1U(ByVal input As String) As Boolean
        input = input.ToLower
        Return input.StartsWith("c1u", StringComparison.OrdinalIgnoreCase)
    End Function
    Public Shared Function StartsWithC2U(ByVal input As String) As Boolean
        input = input.ToLower
        Return input.StartsWith("c2u", StringComparison.OrdinalIgnoreCase)
    End Function

    Public Shared Function StartsWithS(ByVal input As String) As Boolean
        input = input.ToLower
        Return input.StartsWith("s", StringComparison.OrdinalIgnoreCase)
    End Function


    ' extracting the numeric parts from the layer names And compare them. 
    ' reusing existing getBlockNumber function to get the numeric part from both the moved block's layer and the current insert’s layer. 
    Public Shared Function getLayerNumber(name As String) As Integer
        Dim match As Match = Regex.Match(name, "\d+$") ' Match digits at the end of the string
        If match.Success Then
            Return Convert.ToInt32(match.Value)
        End If
        Return 0 ' Return 0 if no number is found
    End Function


    Public Shared Function topWDAltitudeMarkerRecognizer(name As String) As Boolean
        If String.IsNullOrEmpty(name) Then Return False ' Prevents null reference issues
        Return name.ToLower().Contains("p130184") ' Checks if "p108" exists anywhere in the string
    End Function

    Public Shared Function topTDAltitudeMarkerRecognizer(name As String) As Boolean
        If String.IsNullOrEmpty(name) Then Return False ' Prevents null reference issues
        Return name.ToLower().Contains("c1p148218") ' Checks if "c1p108" exists anywhere in the string
    End Function

    Public Shared Function topTTAltitudeMarkerRecognizer(name As String) As Boolean
        If String.IsNullOrEmpty(name) Then Return False ' Prevents null reference issues
        Return name.ToLower().Contains("c2p146660") ' Checks if "c2p108" exists anywhere in the string
    End Function

    Public Shared Function altitudeComperator(inputZ As Double, altitudeMarker As Double) As Boolean
        Return inputZ >= altitudeMarker
    End Function

    Public Shared Function topP180Recognizer(name As String) As Boolean
        If String.IsNullOrEmpty(name) Then Return False ' Prevents null reference issues
        Return name.ToLower().StartsWith("p108687") ' Checks if "c2p108" exists anywhere in the string
    End Function

    Public Shared Function topC1P180Recognizer(name As String) As Boolean
        If String.IsNullOrEmpty(name) Then Return False ' Prevents null reference issues
        Return name.ToLower().StartsWith("c1p108694") ' Checks if "c2p108" exists anywhere in the string
    End Function

    Public Shared Function topC2P180Recognizer(name As String) As Boolean
        If String.IsNullOrEmpty(name) Then Return False ' Prevents null reference issues
        Return name.ToLower().StartsWith("c2p108694") ' Checks if "c2p108" exists anywhere in the string
    End Function


End Class
