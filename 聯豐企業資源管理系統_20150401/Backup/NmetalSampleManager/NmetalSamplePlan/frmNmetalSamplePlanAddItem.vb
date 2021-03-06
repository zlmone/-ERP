Imports LFERP.Library.NmetalSampleManager.NmetalSampleOrdersMain
Imports LFERP.Library.NmetalSampleManager.NmetalSamplePlan
Imports LFERP.Library.NmetalSampleManager.NmetalSampleProcessMain
Public Class frmNmetalSamplePlanAddItem
    Dim ds As New DataSet
    Public SampleList As New List(Of NmetalSamplePlanInfo)


    Private Sub frmSamplePlanAddItem_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        CreateTable()
        Dim mtd As New NmetalSampleOrdersMainControler
        Dim som As New List(Of NmetalSampleOrdersMainInfo)
        som = mtd.NmetalSampleOrdersMain_GetListItem(txtSO_ID.Text, Nothing, Nothing, True)
        Dim i As Integer
        For i = 0 To som.Count - 1
            Dim row As DataRow = ds.Tables("SamplePlanAdd").NewRow
            row("SS_Edition") = som.Item(i).SS_Edition
            row("PM_M_Code") = som.Item(i).PM_M_Code
            row("M_Code") = som.Item(i).M_Code
            row("M_Name") = som.Item(i).M_Name
            row("YesNo") = False
            ds.Tables("SamplePlanAdd").Rows.Add(row)
        Next
    End Sub
    ''' <summary>
    ''' 创建临时表
    ''' </summary>
    ''' <remarks></remarks>
    Sub CreateTable()
        ds.Tables.Clear()
        With ds.Tables.Add("SamplePlanAdd")
            .Columns.Add("YesNo", GetType(Boolean))
            .Columns.Add("SS_Edition", GetType(String))
            .Columns.Add("PM_M_Code", GetType(String))
            .Columns.Add("M_Code", GetType(String))
            .Columns.Add("M_Name", GetType(String))
            .Columns.Add("AutoID", GetType(Decimal))
        End With
        Grid.DataSource = ds.Tables("SamplePlanAdd")
    End Sub

    ''' <summary>
    ''' 保存数据事件
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub cmdSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSave.Click
        Dim m As Integer
        Dim T As Boolean
        T = False
        For m = 0 To ds.Tables("SamplePlanAdd").Rows.Count - 1
            If IIf(IsDBNull(ds.Tables("SamplePlanAdd").Rows(m)("YesNo")), False, ds.Tables("SamplePlanAdd").Rows(m)("YesNo")) Then
                T = True
            End If
        Next

        If T = False Then
            MsgBox("沒有选择资料无法保存,请选择！", MsgBoxStyle.Information, "溫馨提示")
            Exit Sub
        End If

        ''''''''''''''''''''''''''''''''''''''
        Dim i As Integer
        For i = 0 To ds.Tables("SamplePlanAdd").Rows.Count - 1
            If ds.Tables("SamplePlanAdd").Rows(i)("YesNo").ToString <> String.Empty And IIf(IsDBNull(ds.Tables("SamplePlanAdd").Rows(i)("YesNo")), False, ds.Tables("SamplePlanAdd").Rows(i)("YesNo")) Then
                With ds.Tables("SamplePlanAdd")
                    Dim SamplePlanInfo As New NmetalSamplePlanInfo
                    SamplePlanInfo.SS_Edition = .Rows(i)("SS_Edition").ToString
                    SamplePlanInfo.PM_M_Code = .Rows(i)("PM_M_Code").ToString
                    SamplePlanInfo.M_Code = .Rows(i)("M_Code").ToString
                    SamplePlanInfo.M_Name = .Rows(i)("M_Name").ToString
                    SampleList.Add(SamplePlanInfo)
                End With
            End If
        Next
        Me.Close()
    End Sub
    ''' <summary>
    ''' 是否全选
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub CheckEdit2_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckEdit2.CheckedChanged
        Dim i As Integer
        If CheckEdit2.Checked = True Then
            For i = 0 To ds.Tables("SamplePlanAdd").Rows.Count - 1
                ds.Tables("SamplePlanAdd").Rows(i)("YesNo") = True
            Next
        Else
            For i = 0 To ds.Tables("SamplePlanAdd").Rows.Count - 1
                ds.Tables("SamplePlanAdd").Rows(i)("YesNo") = False
            Next
        End If
    End Sub
End Class