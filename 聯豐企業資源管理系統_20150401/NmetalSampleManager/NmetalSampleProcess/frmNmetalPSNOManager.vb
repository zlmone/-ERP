Imports LFERP.DataSetting
Imports LFERP.Library.NmetalSampleManager.NmetalSampleProcess
Imports LFERP.Library.NmetalSampleManager.NmetalProductionIssue

Public Class frmNmetalPSNOManager
    Dim ds As New DataSet
    Dim nsp As New NmetalSampleProcessControl
    Dim dc As New DepartmentControler

    ''' <summary>
    ''' 读取产品编号
    ''' </summary>
    ''' <remarks></remarks>
    Sub LoadPM_M_Code()
        glu_PM_M_Code.Properties.DisplayMember = "PM_M_Code"
        glu_PM_M_Code.Properties.ValueMember = "PM_M_Code"
        glu_PM_M_Code.Properties.DataSource = nsp.NmetalSampleProcessMain_GetList1(Nothing, Nothing, cbo_ProType.Text, Nothing, Nothing, Nothing, Nothing, Nothing)
    End Sub
    ''' <summary>
    ''' 读取部门
    ''' </summary>
    ''' <remarks></remarks>
    Sub LoadDepName()
        gluSE_OutD_ID.Properties.DisplayMember = "DepName"
        gluSE_OutD_ID.Properties.ValueMember = "DepID"
        gluSE_OutD_ID.Properties.DataSource = dc.BriName_GetList(Nothing, Nothing, "V")

    End Sub
    ''' <summary>
    ''' 创建临时表
    ''' </summary>
    ''' <remarks></remarks>
    Sub CreateTable()
        ds.Tables.Clear()
        With ds.Tables.Add("PS_NOTable")
            .Columns.Add("PS_NO", GetType(String))
            .Columns.Add("PS_Name", GetType(String))
            .Columns.Add("GoIn", GetType(Boolean))
        End With
        Grid1.DataSource = ds.Tables("PS_NOTable")

        With ds.Tables.Add("TypeTable")
            .Columns.Add("Type3ID", GetType(String))
        End With
        glu_Type.Properties.DisplayMember = "Type3ID"
        glu_Type.Properties.ValueMember = "Type3ID"
        glu_Type.Properties.DataSource = ds.Tables("TypeTable")
    End Sub
    Private Sub frmNmetalPSNOManager_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        CreateTable()
        LoadDepName()
        LoadPM_M_Code()
        gluSE_OutD_ID.Focus()
        gluSE_OutD_ID.Select()
    End Sub

    ''' <summary>
    ''' 产品编号更改
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub glu_PM_M_Code_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles glu_PM_M_Code.EditValueChanged
        ' On Error Resume Next
        Dim piL As New List(Of NmetalSampleProcessInfo)

        If glu_PM_M_Code.EditValue = "" Then
            Exit Sub
        End If
        piL = nsp.NmetalSampleProcessMain_GetList2(Nothing, glu_PM_M_Code.EditValue)
        ds.Tables("TypeTable").Clear()
        Dim i As Integer
        Dim row As DataRow
        For i = 0 To piL.Count - 1
            row = ds.Tables("TypeTable").NewRow
            row("Type3ID") = piL(i).Type3ID
            ds.Tables("TypeTable").Rows.Add(row)
        Next
        ds.Tables("PS_NOTable").Clear()
    End Sub
    ''' <summary>
    ''' 类型更改时
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub glu_Type_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles glu_Type.EditValueChanged
        If glu_PM_M_Code.EditValue = "" Or glu_Type.EditValue = "" Then
            Exit Sub
        End If

        ds.Tables("PS_NOTable").Clear()
        Dim pci As New List(Of NmetalSampleProcessInfo)
        pci = nsp.NmetalSampleProcessMain_GetList(Nothing, glu_PM_M_Code.EditValue, Nothing, glu_Type.EditValue, Nothing, Nothing, Nothing)
        If pci.Count = 0 Then
            Exit Sub
        End If

        Dim i As Integer
        For i = 0 To pci.Count - 1
            Dim row As DataRow
            row = ds.Tables("PS_NOTable").NewRow

            row("PS_NO") = pci(i).PS_NO
            row("PS_Name") = pci(i).PS_Name

            Dim dc As New NmetalProductionIssueControl
            Dim di As New List(Of NmetalProductionIssueInfo)
            di = dc.NmetalSampleProcess_GetList(Nothing, gluSE_OutD_ID.EditValue, Nothing, glu_PM_M_Code.EditValue, glu_Type.EditValue, pci(i).PS_NO)
            If di.Count = 0 Then
                row("GoIn") = False
            Else
                row("GoIn") = True
            End If
            ds.Tables("PS_NOTable").Rows.Add(row)
        Next

    End Sub
    ''' <summary>
    ''' 保存
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub cmdSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSave.Click

        If glu_PM_M_Code.EditValue = "" Then
            MsgBox("产品编号不能为空，请选择!")
            Exit Sub
        End If
        If glu_Type.EditValue = "" Then
            MsgBox("类型不能为空，请选择!")
            Exit Sub
        End If
        If ds.Tables("PS_NOTable").Rows.Count <= 0 Then
            MsgBox("子表无数据，请添加数据!", MsgBoxStyle.OkOnly, "提示")
            Exit Sub
        End If
        Dim dc As New NmetalProductionIssueControl
        Dim di As New NmetalProductionIssueInfo
        Dim bool As Boolean = dc.NmetalProductionIssue_Delete(Nothing, gluSE_OutD_ID.EditValue, glu_PM_M_Code.EditValue, glu_Type.EditValue, Nothing)
        If bool = False Then
            MsgBox("数据删除失败，无法保存！")
            Exit Sub
        End If

        Dim i As Integer
        For i = 0 To ds.Tables("PS_NOTable").Rows.Count - 1

            If ds.Tables("PS_NOTable").Rows(i)("GoIn") = True Then
                di.Dep_ID = gluSE_OutD_ID.EditValue
                di.Pro_Type = cbo_ProType.Text
                di.PM_M_code = glu_PM_M_Code.EditValue
                di.Type3ID = glu_Type.EditValue
                di.PS_NO = ds.Tables("PS_NOTable").Rows(i)("PS_NO")
                dc.NmetalProductionIssue_Add(di)
            End If
        Next
        MsgBox("保存成功!")
    End Sub
    ''' <summary>
    ''' 取消按钮
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub cmdExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdExit.Click
        Me.Close()
    End Sub
    ''' <summary>
    ''' 部门更改时
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub gluSE_OutD_ID_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles gluSE_OutD_ID.EditValueChanged
        glu_PM_M_Code.EditValue = ""
        glu_Type.EditValue = ""
    End Sub
End Class