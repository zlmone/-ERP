Imports LFERP.Library.WareHouse.WareChange
Imports LFERP.Library.WareHouse
Imports LFERP.Library.Purchase.SharePurchase
Imports LFERP.FileManager
Public Class frmWareChange
#Region "属性"
    Dim ds As New DataSet
    Dim oldcheck As Boolean
    Dim strDPTID As String
#End Region

#Region "窗体载入"
    Private Sub frmWareChange_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Label14.Text = tempValue2
        Label15.Text = tempValue
        tempValue = ""
        tempValue2 = ""

        CreateTable()
        txtDate.Enabled = False
        txtNo.Enabled = False
        Select Case Label15.Text
            Case "WareChange"
                If Edit = False Then
                    txtDate.EditValue = Format(Now, "yyyy/MM/dd")
                    Label6.Text = UserName
                Else
                    CheckEdit1.Enabled = False
                    CheckEdit2.Enabled = False
                    checkRemark.Enabled = False
                    ReCheckRemark.Enabled = False
                    LoadData(Label14.Text)
                End If
                XtraTabControl1.SelectedTabPage = XtraTabPage1
            Case "PreView"
                LoadData(Label14.Text)
                ChangeAdd.Enabled = False
                ChangeDel.Enabled = False
                XtraTabControl1.SelectedTabPage = XtraTabPage1
                cmdSave.Visible = False
            Case "Check"
                LoadData(Label14.Text)
                checkdate.Text = Format(Now, "yyyy/MM/dd")
                ChangeAdd.Enabled = False
                ChangeDel.Enabled = False
                XtraTabControl1.SelectedTabPage = XtraTabPage2
            Case "ReCheck"
                LoadData(Label14.Text)
                ReCheckDate.Text = Format(Now, "yyyy/MM/dd")
                ChangeAdd.Enabled = False
                ChangeDel.Enabled = False
                XtraTabControl1.SelectedTabPage = XtraTabPage3
        End Select

        '加載附件供顯示
        GridFile.AutoGenerateColumns = False
        GridFile.RowHeadersWidth = 15
        Dim dt As New FileController
        GridFile.DataSource = dt.FileBond_GetList("50061", txtNo.EditValue, Nothing)
        GridFile.Refresh()
    End Sub
#End Region

#Region "创建临时表"
    Sub CreateTable()
        ds.Tables.Clear()
        With ds.Tables.Add("WareChange")
            .Columns.Add("IndexNo", GetType(String))
            .Columns.Add("M_Code", GetType(String))
            .Columns.Add("M_Name", GetType(String))
            .Columns.Add("M_Gauge", GetType(String))
            .Columns.Add("WI_Qty", GetType(Double))
            .Columns.Add("C_Qty", GetType(Double))
        End With
        With ds.Tables.Add("DelWareChange")
            .Columns.Add("IndexNo", GetType(String))
        End With
        Grid.DataSource = ds.Tables("WareChange")
    End Sub
#End Region

#Region "返回数据"
    Public Function LoadData(ByVal C_ChangeNO As String) As Boolean
        LoadData = True
        Dim ci As List(Of WareChangeInfo)
        Dim cc As New WareChangeControl
        Try
            ci = cc.WareChange_GetList(C_ChangeNO, Nothing, Nothing, Nothing, Nothing)
            If ci Is Nothing Then
                Exit Function
            End If
            Dim i As Integer

            For i = 0 To ci.Count - 1
                txtNo.Text = ci(i).C_ChangeNO
                ButtonEdit1.EditValue = ci(i).WH_Name
                strDPTID = ci(i).WH_ID
                txtDate.EditValue = Format(ci(i).C_Date, "yyyy/MM/dd")
                Label6.Text = ci(i).ActionName
                txtRemark.Text = ci(i).C_Remark

                Dim row As DataRow
                row = ds.Tables("WareChange").NewRow

                row("IndexNo") = ci(i).IndexNo
                row("M_Code") = ci(i).M_Code
                row("M_Name") = ci(i).M_Name
                row("M_Gauge") = ci(i).M_Gauge
                row("WI_Qty") = ci(i).WI_Qty
                row("C_Qty") = ci(i).C_Qty
                '---------------------------------------------------------------
                ds.Tables("WareChange").Rows.Add(row)

                If ci(i).C_Check = False Then
                    CheckEdit1.Checked = False
                Else
                    CheckEdit1.Checked = True
                End If

                oldcheck = ci(i).C_Check  '用於判斷審核變化

                checkdate.Text = Format(ci(i).C_CheckDate, "yyy/MM/dd")
                checkactionname.Text = ci(i).CheckActionName
                checkRemark.Text = ci(i).C_CheckRemark
                '---------------------------------------------------------------
                If ci(i).C_ReCheck = False Then
                    CheckEdit2.Checked = False
                Else
                    CheckEdit2.Checked = True
                End If

                ReCheckDate.Text = Format(ci(i).C_ReCheckDate, "yyy/MM/dd")
                ReCheckActionName.Text = ci(i).ReCheckActionName
                ReCheckRemark.Text = ci(i).C_ReCheckRemark
                '---------------------------------------------------------------
            Next
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Function
#End Region

#Region "自动流水号"
    ''' <summary>
    ''' 自動獲得更改單單號
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Function GetChangeNO() As String
        Dim str As String
        str = CStr(Format(Now, "yyMM"))
        Dim ai As New WareChangeInfo
        Dim ac As New WareChangeControl

        ai = ac.WareChange_GetNO(str)

        If ai Is Nothing Then
            GetChangeNO = "C" & str & "00001"
        Else
            GetChangeNO = "C" & str & Mid((CInt(Mid(ai.C_ChangeNO, 6)) + 100001), 2)
        End If
    End Function
#End Region

#Region "新增事件"
    Sub DataNew()
        Dim wi As New WareChangeInfo
        Dim wic As New WareChangeControl
        Dim i As Integer
        wi.C_ChangeNO = GetChangeNO()
        txtNo.Text = GetChangeNO()
        wi.WH_ID = strDPTID
        wi.C_Date = txtDate.EditValue
        wi.C_Action = InUserID
        wi.C_Remark = txtRemark.Text

        For i = 0 To ds.Tables("WareChange").Rows.Count - 1
            wi.M_Code = ds.Tables("WareChange").Rows(i)("M_Code")
            If IsDBNull(ds.Tables("WareChange").Rows(i)("WI_Qty")) Then
                wi.WI_Qty = 0
            Else
                wi.WI_Qty = ds.Tables("WareChange").Rows(i)("WI_Qty")
            End If
            If IsDBNull(ds.Tables("WareChange").Rows(i)("C_Qty")) Then
                wi.C_Qty = 0
            Else
                wi.C_Qty = ds.Tables("WareChange").Rows(i)("C_Qty")
            End If
            wic.WareChange_Add(wi)
        Next
        MsgBox("已保存,單號: " & txtNo.Text & " ")
        Me.Close()
    End Sub
#End Region

#Region "修改事件"
    Sub DataEdit()
        '更新刪除的記錄
        If ds.Tables("DelWareChange").Rows.Count > 0 Then
            Dim j As Integer
            For j = 0 To ds.Tables("DelWareChange").Rows.Count - 1

                Dim odc As New WareChangeControl

                If Not IsDBNull(ds.Tables("DelWareChange").Rows(j)("IndexNo")) Then
                    odc.WareChange_Delete(Nothing, ds.Tables("DelWareChange").Rows(j)("IndexNo"))
                End If
            Next j
        End If

        Dim i As Integer
        For i = 0 To ds.Tables("WareChange").Rows.Count - 1

            If Not IsDBNull(ds.Tables("WareChange").Rows(i)("IndexNo")) Then   '只是修改

                Dim wi As New WareChangeInfo
                Dim wic As New WareChangeControl

                wi.IndexNo = ds.Tables("WareChange").Rows(i)("IndexNo")
                wi.WH_ID = strDPTID
                wi.C_Date = txtDate.EditValue
                wi.C_Action = InUserID
                wi.C_Remark = txtRemark.Text

                wi.M_Code = ds.Tables("WareChange").Rows(i)("M_Code")

                If IsDBNull(ds.Tables("WareChange").Rows(i)("WI_Qty")) Then
                    wi.WI_Qty = 0
                Else
                    wi.WI_Qty = ds.Tables("WareChange").Rows(i)("WI_Qty")
                End If
                If IsDBNull(ds.Tables("WareChange").Rows(i)("C_Qty")) Then
                    wi.C_Qty = 0
                Else
                    wi.C_Qty = ds.Tables("WareChange").Rows(i)("C_Qty")
                End If

                wic.WareChange_Update(wi)

            ElseIf IsDBNull(ds.Tables("WareChange").Rows(i)("IndexNo")) Then   '修改新增

                Dim wi As New WareChangeInfo
                Dim wic As New WareChangeControl

                wi.C_ChangeNO = txtNo.Text
                wi.WH_ID = strDPTID
                wi.C_Date = txtDate.EditValue
                wi.C_Action = InUserID
                wi.C_Remark = txtRemark.Text

                wi.M_Code = ds.Tables("WareChange").Rows(i)("M_Code")

                If IsDBNull(ds.Tables("WareChange").Rows(i)("WI_Qty")) Then
                    wi.WI_Qty = 0
                Else
                    wi.WI_Qty = ds.Tables("WareChange").Rows(i)("WI_Qty")
                End If
                If IsDBNull(ds.Tables("WareChange").Rows(i)("C_Qty")) Then
                    wi.C_Qty = 0
                Else
                    wi.C_Qty = ds.Tables("WareChange").Rows(i)("C_Qty")
                End If

                wic.WareChange_Add(wi)
            End If
        Next

        MsgBox("已修改！")
        Me.Close()
    End Sub
#End Region

#Region "审核事件"
    Sub UpdateCheck()
        If oldcheck = CheckEdit1.Checked Then
            MsgBox("審核狀態未改變，請更改狀態後再保存……")
            Exit Sub
        End If
        Dim wi As New WareChangeInfo
        Dim wic As New WareChangeControl

        wi.C_ChangeNO = txtNo.Text
        wi.C_Check = CheckEdit1.Checked
        wi.C_CheckAction = InUserID
        wi.C_CheckDate = Format(Now, "yyyy/MM/dd")
        wi.C_CheckRemark = checkRemark.Text

        If wic.WareChange_UpdateCheck(wi) = True Then
            MsgBox("審核成功!", , "提示")
        Else
            MsgBox("審核失敗,請檢查原因!", , "提示")
        End If

        Dim pi As New SharePurchaseInfo
        Dim pc As New SharePurchaseController

        Dim i As Integer
        For i = 0 To ds.Tables("WareChange").Rows.Count - 1
            pi.M_Code = ds.Tables("WareChange").Rows(i)("M_Code")
            pi.WH_ID = strDPTID
            pi.WI_Qty = CDbl(ds.Tables("WareChange").Rows(i)("C_Qty"))
            pc.UpdateWareInventory_WIQty2(pi)
        Next
        Me.Close()
    End Sub
#End Region

#Region "復核事件"
    Sub UpdateReCheck()
        Dim wi As New WareChangeInfo
        Dim wic As New WareChangeControl

        wi.C_ChangeNO = txtNo.Text
        wi.C_ReCheck = CheckEdit2.Checked
        wi.C_ReCheckAction = InUserID
        wi.C_ReCheckDate = Format(Now, "yyyy/MM/dd")
        wi.C_ReCheckRemark = ReCheckRemark.Text

        If wic.WareChange_UpdateReCheck(wi) = True Then
            MsgBox("復核成功!", , "提示")
        Else
            MsgBox("復核失敗,請檢查原因!", , "提示")
        End If
        Me.Close()
    End Sub
#End Region

#Region "按键事件"
    Private Sub cmdSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSave.Click
        Select Case Label15.Text
            Case "WareChange"
                If Edit = False Then
                    DataNew()
                Else
                    DataEdit()
                End If
            Case "Check"
                UpdateCheck()
            Case "ReCheck"
                UpdateReCheck()
        End Select
    End Sub
    Private Sub cmdExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdExit.Click
        Me.Close()
    End Sub
#End Region

#Region "表格新增数据行"
    Private Sub ChangeAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ChangeAdd.Click
        tempCode = ""
        tempValue6 = "倉庫管理"
        frmBOMSelect.ShowDialog()
        If frmBOMSelect.XtraTabControl1.SelectedTabPageIndex = 0 Then
            '增加記錄
            If tempCode = "" Then
                Exit Sub
            Else
                AddRow(tempCode)
            End If
        ElseIf frmBOMSelect.XtraTabControl1.SelectedTabPageIndex = 1 Then
            Dim i, n As Integer
            Dim arr(n) As String
            arr = Split(tempValue7, ",")
            n = Len(Replace(tempValue7, ",", "," & "*")) - Len(tempValue7)
            For i = 0 To n
                If arr(i) = "" Then
                    Exit Sub
                End If
                AddRow(arr(i))
            Next
        ElseIf frmBOMSelect.XtraTabControl1.SelectedTabPageIndex = 2 Then
            Dim i, n As Integer
            Dim arr(n) As String
            arr = Split(tempValue8, ",")
            n = Len(Replace(tempValue8, ",", "," & "*")) - Len(tempValue8)
            For i = 0 To n

                If arr(i) = "" Then
                    Exit Sub
                End If
                AddRow(arr(i))
            Next
        End If
        tempValue7 = ""
        tempValue8 = ""
    End Sub
#End Region

#Region "添加数据行"
    Sub AddRow(ByVal StrCode As String)
        Dim row As DataRow
        row = ds.Tables("WareChange").NewRow
        If StrCode = "" Then
        Else
            Dim i As Integer
            For i = 0 To ds.Tables("WareChange").Rows.Count - 1
                If StrCode = ds.Tables("WareChange").Rows(i)("M_Code") Then
                    MsgBox("一張單不允許有重復物料編碼....")
                    Exit Sub
                End If
            Next
            Dim mc As New LFERP.Library.Material.MaterialController
            Dim objInfo As New LFERP.Library.Material.MaterialInfo
            objInfo = mc.MaterialCode_Get(StrCode)

            Dim wi As LFERP.Library.WareHouse.WareInventory.WareInventoryInfo
            Dim wc As New LFERP.Library.WareHouse.WareInventory.WareInventoryMTController
            wi = wc.WareInventory_GetSub(StrCode, strDPTID)

            Dim StrQty As Double
            If wi Is Nothing Then
                StrQty = 0
            Else
                StrQty = wi.WI_Qty
            End If

            row("IndexNo") = Nothing
            row("M_Code") = objInfo.M_Code
            row("M_Name") = objInfo.M_Name
            row("M_Gauge") = objInfo.M_Gauge
            row("WI_Qty") = StrQty
            ds.Tables("WareChange").Rows.Add(row)
        End If
        GridView1.MoveLast()
    End Sub
#End Region

#Region "表格删除事件"
    Private Sub ChangeDel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ChangeDel.Click
        If GridView1.RowCount = 0 Then Exit Sub
        Dim DelTemp As String
        DelTemp = GridView1.GetRowCellDisplayText(ArrayToString(GridView1.GetSelectedRows()), "IndexNo")

        If DelTemp = "IndexNo" Then
        Else
            '在刪除表中增加被刪除的記錄
            Dim row As DataRow = ds.Tables("DelWareChange").NewRow
            row("IndexNo") = DelTemp
            ds.Tables("DelWareChange").Rows.Add(row)
        End If
        ds.Tables("WareChange").Rows.RemoveAt(CInt(ArrayToString(GridView1.GetSelectedRows())))
    End Sub
#End Region

#Region "按键事件"
    Private Sub ButtonEdit1_ButtonClick(ByVal sender As Object, ByVal e As DevExpress.XtraEditors.Controls.ButtonPressedEventArgs) Handles ButtonEdit1.ButtonClick
        On Error Resume Next
        tempValue3 = "505007"
        frmWareHouseSelect.ShowDialog()
        If frmWareHouseSelect.SelectWareID <> "" Then
            strDPTID = frmWareHouseSelect.SelectWareID
            Dim wi As List(Of WareHouseInfo)
            Dim wc As New WareHouseController

            Dim strWHID As String
            strWHID = Mid(strDPTID, 1, 3)
            wi = wc.WareHouse_Get(strWHID)
            ButtonEdit1.Text = wi(0).WH_Name & "-" & frmWareHouseSelect.SelectWareName
        Else
            Exit Sub
        End If
    End Sub
#End Region

#Region "打开选择文件"
    Private Sub popFileShowOpen_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles popFileShowOpen.Click
        '打開選擇文件
        Dim dt As New FileController
        If GridFile.Rows.Count = 0 Then Exit Sub
        dt.File_Open(Nothing, Nothing, GridFile.CurrentRow.Cells("F_No").Value.ToString)
    End Sub
#End Region

End Class