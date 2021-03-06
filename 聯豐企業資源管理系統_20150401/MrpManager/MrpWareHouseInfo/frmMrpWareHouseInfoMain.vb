Imports System
Imports LFERP.SystemManager
Imports LFERP.Library.MrpManager.MrpWareHouseInfo
Imports LFERP.Library.MrpManager.MrpSelect
Imports LFERP.Library.MrpManager.MrpSetting
Public Class frmMrpWareHouseInfoMain
    Dim MWHIcon As New MrpWareHouseInfoController
    Dim MWHIEcon As New MrpWareHouseInfoEntryController
    Dim ds As New DataSet

#Region "載入事件"
    ''' <summary>
    ''' 加載
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub frmMrpWareHouseInfoMain_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        PowerUser()
        cmsReflash_Click(Nothing, Nothing)
    End Sub
#End Region

#Region "設置右擊菜單項是否可用"
    Private Sub GridControl1_MouseDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles Grid.MouseDown
        If e.Button = Windows.Forms.MouseButtons.Right Then
            SetRightClickMenuEnable()
        End If
    End Sub

    Private Sub SetRightClickMenuEnable()
        Dim mwi As New MrpWareHouseInfoInfo
        Dim mwiList As New List(Of MrpWareHouseInfoInfo)
        If GridView1.FocusedRowHandle >= 0 Then
            mwiList = MWHIcon.MrpWareHouseInfo_GetList(GridView1.GetFocusedRowCellValue("Ware_ID"), Nothing, Nothing, Nothing, Nothing, Nothing, Nothing)
            If mwiList.Count > 0 Then
                mwi = mwiList(0)
            Else
                MsgBox(GridView1.GetFocusedRowCellValue("Ware_ID") + "的庫存單號已被其他用戶刪除", MsgBoxStyle.Information, "提示")
                cmsReflash_Click(Nothing, Nothing)
                Exit Sub
            End If
        End If
        Try
            Dim c As ToolStripItem
            If GridView1.FocusedRowHandle < 0 Then
                For Each c In ContextMenuStrip1.Items
                    If (c.Name = "cmsAdd" Or c.Name = "cmsReflash") Then
                        c.Enabled = True
                    Else
                        c.Enabled = False
                    End If
                Next
            ElseIf mwi.CheckBit.Equals(True) Then
                For Each c In ContextMenuStrip1.Items
                    If (c.Name = "cmsEdit" Or c.Name = "cmsDel") Then
                        c.Enabled = False
                    Else
                        c.Enabled = True
                    End If
                Next
            Else
                For Each c In ContextMenuStrip1.Items
                    c.Enabled = True
                Next
            End If
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Information, "SetRightClickMenuEnable方法出錯")
        End Try
    End Sub
#End Region

#Region "增加/刪除/修改/查看/審核/列印/刷新"
    ''' <summary>
    ''' 新增
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub cmsAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmsAdd.Click
        On Error Resume Next
        Dim fr As frmMrpWareHouseInfoAdd
        For Each fr In MDIMain.MdiChildren
            If TypeOf fr Is frmMrpWareHouseInfoAdd Then
                fr.Activate()
                Exit Sub
            End If
        Next
        fr = New frmMrpWareHouseInfoAdd
        fr.MdiParent = MDIMain
        fr.EditItem = EditEnumType.ADD
        fr.WindowState = FormWindowState.Maximized
        fr.Show()
    End Sub
    ''' <summary>
    ''' 修改
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub cmsEdit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmsEdit.Click
        On Error Resume Next
        Dim StrWare_ID As String = GridView1.GetFocusedRowCellValue("Ware_ID").ToString()
        If MWHIcon.MrpWareHouseInfo_GetList(StrWare_ID, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing)(0).CheckBit = False Then
            Dim fr As frmMrpWareHouseInfoAdd
            For Each fr In MDIMain.MdiChildren
                If TypeOf fr Is frmMrpWareHouseInfoAdd Then
                    fr.Activate()
                    Exit Sub
                End If
            Next
            fr = New frmMrpWareHouseInfoAdd
            fr.MdiParent = MDIMain
            fr.EditItem = EditEnumType.EDIT
            fr.EditValue = StrWare_ID
            fr.WindowState = FormWindowState.Maximized
            fr.Show()
        Else
            MsgBox("已經審核不能！", 60, "提示")
        End If
    End Sub
    ''' <summary>
    ''' 刪除
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub cmsDel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmsDel.Click
        Dim StrAutoID As String = GridView1.GetFocusedRowCellValue("AutoID").ToString()
        Dim StrWare_ID As String = GridView1.GetFocusedRowCellValue("Ware_ID").ToString()
        If MWHIcon.MrpWareHouseInfo_GetList(StrWare_ID, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing)(0).CheckBit = False Then
            Dim result As Windows.Forms.DialogResult = MessageBox.Show("是否確定刪除庫存單號：" + StrWare_ID + "？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
            If (result = Windows.Forms.DialogResult.Yes) Then
                If MWHIcon.MrpWareHouseInfo_Delete(StrAutoID) = True Then
                    If MWHIEcon.MrpWareHouseInfoEntry_DeleteAll(StrWare_ID) = False Then
                        MsgBox("刪除失敗！", 60, "提示")
                        Exit Sub
                    End If
                End If
            End If
            cmsReflash_Click(Nothing, Nothing)
        Else
            MsgBox("已經審核不能刪除！", 60, "提示")
        End If
    End Sub
    ''' <summary>
    ''' 查詢
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub cmsFind_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmsFind.Click
        'Dim fr As New frmSelect
        ''fr.FormText = "MRP庫存記錄"
        ''fr.TableName = "MrpWareHouseInfo"
        ''fr.ID = "Ware_ID"
        'fr.ShowDialog()
        'Dim sc As New Select_Controller
        'If String.IsNullOrEmpty(tempValue) = False Then
        '    Grid.DataSource = sc.MrpWareHouseInfo_GetList(tempValue)
        'End If
    End Sub
    ''' <summary>
    ''' 查看
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub cmsLook_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmsLook.Click
        On Error Resume Next
        Dim fr As frmMrpWareHouseInfoAdd
        For Each fr In MDIMain.MdiChildren
            If TypeOf fr Is frmMrpWareHouseInfoAdd Then
                fr.Activate()
                Exit Sub
            End If
        Next
        fr = New frmMrpWareHouseInfoAdd
        fr.MdiParent = MDIMain
        fr.EditItem = EditEnumType.VIEW
        fr.EditValue = GridView1.GetFocusedRowCellValue("Ware_ID").ToString
        fr.WindowState = FormWindowState.Maximized
        fr.Show()
    End Sub
    ''' <summary>
    ''' 審核
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub cmsCheck_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmsCheck.Click
        On Error Resume Next
        Dim fr As frmMrpWareHouseInfoAdd
        For Each fr In MDIMain.MdiChildren
            If TypeOf fr Is frmMrpWareHouseInfoAdd Then
                fr.Activate()
                Exit Sub
            End If
        Next
        fr = New frmMrpWareHouseInfoAdd
        fr.MdiParent = MDIMain
        fr.EditItem = EditEnumType.CHECK
        fr.EditValue = GridView1.GetFocusedRowCellValue("Ware_ID").ToString
        fr.WindowState = FormWindowState.Maximized
        fr.Show()
    End Sub
    ''' <summary>
    ''' 刷新
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub cmsReflash_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmsReflash.Click

        Dim msi As New List(Of MrpSettingInfo)
        Dim msc As New MrpSettingController

        Dim StrCheck As String = Nothing
        Dim StrUser As String = Nothing

        msi = msc.MrpSetting_GetList(InUserID)
        If msi.Count > 0 Then
            Select Case msi(0).warehouseCheckType
                Case "0,1"
                    StrCheck = Nothing
                Case "1"
                    StrCheck = "1"
                Case "0"
                    StrCheck = "0"
            End Select

            If msi(0).warehouseCreateUserID = "All" Then
                StrUser = Nothing
            Else
                StrUser = msi(0).warehouseCreateUserID
            End If

            Grid.DataSource = MWHIcon.MrpWareHouseInfo_GetList(Nothing, Nothing, StrCheck, StrUser, msi(0).warehouseBeginDate, Nothing, msi(0).warehouseDisplayNum)
        Else
            Grid.DataSource = MWHIcon.MrpWareHouseInfo_GetList(Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing)
        End If
        GridView1.ActiveFilterString = "MRP_ID like 'MI%'"

    End Sub
    '''' <summary>
    '''' 打印
    '''' </summary>
    '''' <param name="sender"></param>
    '''' <param name="e"></param>
    '''' <remarks></remarks>
    'Private Sub cmsPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmsPrint.Click
    '    Dim dss As New DataSet
    '    Dim ltc As New CollectionToDataSet
    '    'Dim strSO_ID As String = GridView1.GetFocusedRowCellValue("AutoId").ToString
    '    ltc.CollToDataSet(dss, "MrpWareHouseInfo", MWHIcon.MrpWareHouseInfo_GetList(Nothing, Nothing, Nothing, Nothing))
    '    PreviewRPT(dss, "rptMrpWareHouseInfo", "庫存記錄表", True, True)
    '    ltc = Nothing
    '    Me.Close()
    'End Sub
#End Region

#Region "聚焦改變獲得子表信息"
    ''' <summary>
    ''' 聚焦改變事件
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub GridView1_FocusedRowChanged(ByVal sender As System.Object, ByVal e As DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs) Handles GridView1.FocusedRowChanged
        SetRightClickMenuEnable()
        If GridView1.FocusedRowHandle < 0 Then
            GridControl1.DataSource = Nothing
        Else
            GetSubTable()
        End If
    End Sub
    ''' <summary>
    ''' 獲得子表信息
    ''' </summary>
    ''' <remarks></remarks>
    Sub GetSubTable()
        If GridView1.RowCount = 0 Then Exit Sub
        If GridView1.GetFocusedRowCellValue("Ware_ID").ToString IsNot Nothing Then
            GridControl1.DataSource = MWHIEcon.MrpWareHouseInfoEntry_GetList(GridView1.GetFocusedRowCellValue("Ware_ID").ToString)
        End If
    End Sub
#End Region

#Region "設置權限"
    '設置權限
    Sub PowerUser()
        Dim pmws As New PermissionModuleWarrantSubController
        Dim pmwiL As List(Of PermissionModuleWarrantSubInfo)
        pmwiL = pmws.PermissionModuleWarrantSub_GetList(InUserID, "480601")
        If pmwiL.Count > 0 Then
            If pmwiL.Item(0).PMWS_Value = "是" Then
                cmsAdd.Visible = True
                cmsAdd.Enabled = True
            End If

        End If
        pmwiL = pmws.PermissionModuleWarrantSub_GetList(InUserID, "480602")
        If pmwiL.Count > 0 Then
            If pmwiL.Item(0).PMWS_Value = "是" Then
                cmsEdit.Visible = True
                cmsEdit.Enabled = True
            End If
        End If
        pmwiL = pmws.PermissionModuleWarrantSub_GetList(InUserID, "480603") '審核
        If pmwiL.Count > 0 Then
            If pmwiL.Item(0).PMWS_Value = "是" Then
                cmsDel.Visible = True
                cmsDel.Enabled = True
            End If

        End If
        pmwiL = pmws.PermissionModuleWarrantSub_GetList(InUserID, "480604") '確認審核
        If pmwiL.Count > 0 Then
            If pmwiL.Item(0).PMWS_Value = "是" Then
                cmsCheck.Visible = True
                cmsCheck.Enabled = True
            End If

        End If
    End Sub
#End Region

    Private Sub 列印明細ToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles 列印明細ToolStripMenuItem.Click
        Dim dss As New DataSet
        Dim ltc As New CollectionToDataSet
        'Dim strSO_ID As String = GridView1.GetFocusedRowCellValue("AutoId").ToString
        ltc.CollToDataSet(dss, "MrpWareHouseInfoEntry", MWHIEcon.MrpWareHouseInfoEntry_GetList(GridView1.GetFocusedRowCellValue("Ware_ID").ToString))
        PreviewRPT(dss, "rptMrpWareHouseInfoEntry", "庫存記錄明細表", True, True)
        ltc = Nothing
        Me.Close()
    End Sub


    Private Sub tsm_PrintMRPWareHouseAll_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsm_PrintMRPWareHouseAll.Click
        On Error Resume Next
        Dim fr As MrpReportSelect
        For Each fr In MDIMain.MdiChildren
            If TypeOf fr Is MrpReportSelect Then
                fr.Activate()
                Exit Sub
            End If
        Next
        fr = New MrpReportSelect
        fr.intShowPage = 2
        fr.ShowDialog()
        fr.Focus()
    End Sub

    Private Sub tsm_PrintMRPWareHouseInfo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsm_PrintMRPWareHouseInfo.Click
        Dim dss As New DataSet
        Dim ltc1 As New CollectionToDataSet
        Dim ltc2 As New CollectionToDataSet
        Dim StrSend As String = String.Empty
        StrSend = InUser
        ltc1.CollToDataSet(dss, "MrpWareHouseInfoEntry", MWHIEcon.MrpWareHouseInfoEntry_GetList(GridView1.GetFocusedRowCellValue("Ware_ID").ToString))
        ltc2.CollToDataSet(dss, "MrpWareHouseInfo", MWHIcon.MrpWareHouseInfo_GetList(GridView1.GetFocusedRowCellValue("Ware_ID").ToString, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing))
        PreviewRPT1(dss, "rptMrpWareHouseInfo", "庫存記錄明細表", StrSend, StrSend, True, True)
        ltc1 = Nothing
        ltc2 = Nothing
    End Sub

#Region "導出Excel"
    Private Sub cmsExcel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmsExcel.Click, cmsSubExcel.Click
        Try
            If sender.Owner Is ContextMenuStrip1 Then
                ConrotlExportExcel(Grid)
            Else
                ConrotlExportExcel(GridControl1)
            End If
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Information, "提示")
        End Try
    End Sub
#End Region

End Class