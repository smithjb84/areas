' TODO: This module exists as a convenient location for the code that does the real
'       work when a command is executed.  If you're converting VBA macros into add-in 
'       commands you can copy the macros here, make changes to make them VB.NET compatible, 
'       and change any references to "ThisApplication" to "g_inventorApplication".  The example 
'       command in StandardAddInServer.vb demonstrates running the "SampleCommandFunction" below.

Public Module CommandFunctions
    ' Example function that's called when the sample command is executed.
    Public Sub SampleCommandFunction()
        Dim oDoc As Inventor.Document = g_inventorApplication.ActiveDocument

        Dim oSelSet As Inventor.SelectSet = oDoc.SelectSet

        Dim oEval As Inventor.SurfaceEvaluator
        Dim obj As Object
        Dim appearance() As Object
        Dim areas() As Object

        Dim i As Integer
        i = 0
        Dim j As Integer
        i = 0
        Dim k As Integer
        i = 0
        Dim x As Integer
        i = 0

        If oSelSet.Count = 0 Then
            MsgBox("No Faces Selected")
            Exit Sub
        End If

        Dim isFace As Boolean
        isFace = False

        For Each obj In oSelSet
            If (TypeOf obj Is Inventor.Face) Then
                isFace = True
                Exit For
            End If
        Next

        If isFace = False Then
            MsgBox("No Faces Selected. Please set the select command to faces and edges")
            Exit Sub
        End If

        For Each obj In oSelSet
            If (TypeOf obj Is Inventor.Face) Then
                ReDim Preserve appearance(i)
                ReDim Preserve areas(i)
                oEval = obj.Evaluator
                appearance(i) = obj.appearance.DisplayName
                areas(i) = oEval.Area
                i = i + 1
            End If
        Next

        Dim allAppearance() As Object
        Dim allAreas() As Object
        ReDim Preserve allAppearance(0)
        ReDim Preserve allAreas(0)

        i = 0
        j = 0
        Dim Found As Boolean
        For Each Itema In appearance
            Found = False
            k = 0
            For Each Itemb In allAppearance
                If Itema = Itemb Then
                    Found = True
                    Exit For
                End If
                k = k + 1
            Next
            If Found = True Then
                allAreas(k) = allAreas(k) + areas(i)
            Else
                ReDim Preserve allAppearance(j)
                ReDim Preserve allAreas(j)
                allAppearance(j) = appearance(i)
                allAreas(j) = areas(i)
                j = j + 1
            End If
            i = i + 1
        Next

        Dim total
        Dim message As String

        x = 0
        For Each Item In allAppearance
            total = total + allAreas(x)
            x = x + 1
        Next

        Dim uom As Inventor.UnitsOfMeasure = oDoc.UnitsOfMeasure
        Dim oLengthUnit = uom.LengthUnits
        Dim odivide As Single
        Dim ounit As String
        If oLengthUnit = Inventor.UnitsTypeEnum.kMillimeterLengthUnits Then
            ounit = "m"
            odivide = 10000
        Else
            ounit = "in"
            odivide = 6.452
        End If

        message = "Total Surface Area = " + CStr(Math.Round(total / odivide, 3)) + ounit + Chr(178) + vbCrLf + vbCrLf
        x = 0
        For Each Item In allAppearance
            message = message + "Surface area for " + allAppearance(x) + " = " + CStr(Math.Round(allAreas(x) / odivide, 3)) + ounit + Chr(178) + vbCrLf
            x = x + 1
        Next

        MsgBox(message)
    End Sub
End Module
