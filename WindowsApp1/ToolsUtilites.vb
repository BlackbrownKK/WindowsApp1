Public Class ToolsUtilites

    'layer whose name starts with "SU" (indicating a Side view),
    'the code adjusts related "Aft" views to have the Y position match the Side view.
    Public Shared Function StartsWithSU(ByVal input As String) As Boolean
        Return input.StartsWith("SU", StringComparison.OrdinalIgnoreCase)
    End Function

    'if the related block is an "Aft" view (identified by the "AU" prefix in the layer name),
    'its X position should be adjusted to match the Y position of the Top view.
    Public Shared Function StartsWithAU(ByVal input As String) As Boolean
        Return input.StartsWith("AU", StringComparison.OrdinalIgnoreCase)
    End Function

    'If the moved block is on a layer whose name starts with "U" (indicating a Top view),
    'the code ensures that other blocks related to this view (e.g., Aft and Side views)
    'have their insertion points updated accordingly.
    Public Shared Function StartsWithU(ByVal input As String) As Boolean
        Return input.StartsWith("U", StringComparison.OrdinalIgnoreCase)
    End Function

    Public Shared Function StartsWithS(ByVal input As String) As Boolean
        Return input.StartsWith("S", StringComparison.OrdinalIgnoreCase)
    End Function

End Class
