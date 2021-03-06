Imports LFERP.Library.NmetalSampleManager.NmetalSampleOrdersMain
Imports LFERP.Library.NmetalSampleManager.NmetalSampleProcess
Imports LFERP.Library.NmetalSampleManager.NmetalSampleCollection
Imports LFERP.Library.NmetalSampleManager.NmetalSampInventoryCheck
Imports LFERP.Library.NmetalSampleManager.NmetalSampleDivert
Imports LFERP.Library.NmetalSampleManager.NmetalSampleWareInventory


Public Class frmNmetalSampleDivertAdd
#Region "属性"
    Dim ds As New DataSet
    Dim somcon As New NmetalSampleOrdersMainControler
    Dim prcon As New NmetalSampleProcessControl
    Dim Sccon As New NmetalSampleCollectionControler
    Dim sicom As New NmetalSampInventoryCheckControl
    Dim sdcon As New NmetalSampleDivertControl
    Dim SwCon As New NmetalSampleWareInventoryControler
    Private _EditItem As String '属性栏位
    Private _GetSD_ID As String
    Private boolCheck As Boolean
    Property EditItem() As String '属性
        Get
            Return _EditItem
        End Get
        Set(ByVal value As String)
            _EditItem = value
        End Set
    End Property
    Property GetSD_ID() As String '属性
        Get
            Return _GetSD_ID
        End Get
        Set(ByVal value As String)
            _GetSD_ID = value
        End Set
    End Property
#End Region

#Region "窗体载入"
    Private Sub frmSampleDivertAdd_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        CreateTable() '创建临时表
        Select Case EditItem
            Case EditEnumType.ADD
                SetD_ID("InUserID") '载入部门
                SetSO_ID() '載入訂單編號
                Me.lblTitle.Text = Me.Text + EditEnumValue(EditEnumType.ADD)
                Me.Text = lblTitle.Text
                Me.txtSD_ID.EditValue = "自動編號"
                Me.Grid1.ContextMenuStrip = Me.ContextMenuStrip1
            Case EditEnumType.EDIT
                SetD_ID("ALL") '载入部门
                SetSO_ID() '載入訂單編號
                Me.lblTitle.Text = Me.Text + EditEnumValue(EditEnumType.EDIT)
                Me.Text = lblTitle.Text
                Me.gluSE_OutD_ID.Enabled = False
                Me.gluSO_ID.Enabled = False
                Me.Grid1.ContextMenuStrip = Me.ContextMenuStrip1
                LoadData(GetSD_ID)
            Case EditEnumType.VIEW
                SetD_ID("ALL") '载入部门
                SetSO_ID() '載入訂單編號
                Me.lblTitle.Text = Me.Text + EditEnumValue(EditEnumType.VIEW)
                Me.Text = lblTitle.Text
                Me.cmdSave.Visible = False
                Me.gluSE_OutD_ID.Enabled = False
                Me.gluSO_ID.Enabled = False
                Me.txtSD_Remark.Enabled = False
                Me.gluInPS_NO.Enabled = False
                Me.Grid1.ContextMenuStrip = Nothing
                LoadData(GetSD_ID)
            Case EditEnumType.CHECK
                SetD_ID("ALL") '载入部门
                SetSO_ID() '載入訂單編號
                Me.lblTitle.Text = Me.Text + EditEnumValue(EditEnumType.CHECK)
                Me.Text = lblTitle.Text
                Me.XtraTabControl1.SelectedTabPage = XtraTabPage2
                Me.gluSE_OutD_ID.Enabled = False
                Me.gluSO_ID.Enabled = False
                Me.txtSD_Remark.Enabled = False
                Me.gluInPS_NO.Enabled = False
                Me.Grid1.ContextMenuStrip = Nothing
                LoadData(GetSD_ID)
        End Select
    End Sub
#End Region

#Region "数据载入"

#Region "载入数据"
    Sub LoadData(ByVal StrSD_ID As String)
        Dim sdlist As New List(Of NmetalSampleDivertInfo)
        sdlist = sdcon.NmetalSampleDivert_Getlist(Nothing, StrSD_ID, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing)
        If sdlist.Count = 0 Then
            Exit Sub
        Else
            Me.txtSD_ID.Text = sdlist(0).SD_ID
            Me.gluSO_ID.EditValue = sdlist(0).SD_InSO_ID
            Me.gluSE_OutD_ID.EditValue = sdlist(0).SD_InD_ID
            Me.gluInPS_NO.EditValue = sdlist(0).SD_InPS_NO
            Me.txtSD_Remark.Text = sdlist(0).SD_Remark
            Me.CheckA.Checked = sdlist(0).SD_Check
            boolCheck = sdlist(0).SD_Check
            If sdlist(0).SD_CheckAction = String.Empty Then
                Me.lblAction.Text = InUser
            Else
                Me.lblAction.Text = sdlist(0).SD_CheckActionName
            End If
            If sdlist(0).SD_CheckDate = Nothing Then
                Me.lblActionDate.Text = Format(Now, "yyyy/MM/dd").ToString
            Else
                Me.lblActionDate.Text = sdlist(0).SD_CheckDate
            End If
            Me.txt_checkremark.Text = sdlist(0).SD_CheckRemark

            If sdlist.Count = 0 Then
                Exit Sub
            Else
                ds.Tables("SampleDivert").Clear()
                Dim i As Integer
                For i = 0 To sdlist.Count - 1
                    Dim row As DataRow
                    row = ds.Tables("SampleDivert").NewRow
                    row("AutoID") = sdlist(i).AutoID
                    row("Code_ID") = sdlist(i).Code_ID
                    row("SD_Qty") = sdlist(i).SD_Qty
                    row("OutSO_ID") = sdlist(i).SD_OutSO_ID
                    row("SD_OutD_ID") = sdlist(i).SD_OutD_ID
                    row("SD_OutD_Name") = sdlist(i).SD_OutD_Name
                    row("SD_OutPS_NO") = sdlist(i).SD_OutPS_NO
                    row("SD_OutPS_Name") = sdlist(i).SD_OutPS_Name
                    row("OutSO_SampleID") = sdlist(i).OutSO_SampleID
                    row("OutPM_M_Code") = sdlist(i).OutPM_M_Code
                    row("SD_AddDate") = Format(sdlist(i).SD_AddDate, "yyyy-MM-dd HH:mm:ss")
                    ds.Tables("SampleDivert").Rows.Add(row)
                Next
            End If
        End If
    End Sub
#End Region
#End Region

#Region "控件载入数据"
    Sub SetD_ID(ByVal strValue As String) '载入部门
        Dim fc As New LFERP.Library.ProductionController.ProductionFieldControl
        If strValue = "ALL" Then
            gluSE_OutD_ID.Properties.DataSource = fc.ProductionFieldControl_GetList(Nothing, Nothing) '收发部门
        Else
            gluSE_OutD_ID.Properties.DataSource = fc.ProductionFieldControl_GetList(InUserID, Nothing) '收发部门
        End If
    End Sub

    Sub SetSO_ID() '載入訂單編號
        gluSO_ID.Properties.DataSource = somcon.NmetalSampleOrdersMain_GetListItem(Nothing, Nothing, Nothing, True)
    End Sub
#End Region

#Region "按健事件"
    Private Sub cmdExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdExit.Click
        Me.Close()
    End Sub
    Private Sub cmdSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSave.Click
        If (CheckSave() = False) Then
            Exit Sub
        End If

        Select Case EditItem
            Case EditEnumType.ADD
                SaveData(EditEnumType.ADD)
            Case EditEnumType.EDIT
                SaveData(EditEnumType.EDIT)
            Case EditEnumType.CHECK
                UpdateCheck()
        End Select
    End Sub
#End Region

#Region "新增修改操作"
    Private Sub SaveData(ByVal editItem As String)
        Try
            Select Case editItem
                Case EditEnumType.ADD
                    Dim sdinfo As New NmetalSampleDivertInfo
                    sdinfo.SD_ID = SetSD_ID()
                    sdinfo.SD_InSO_ID = gluSO_ID.EditValue
                    sdinfo.SD_InD_ID = gluSE_OutD_ID.EditValue
                    sdinfo.SD_InPS_NO = gluInPS_NO.EditValue

                    sdinfo.SD_CardID = String.Empty
                    sdinfo.SD_AddUserID = InUserID
                    sdinfo.SD_AddDate = Format(Now, "yyyy/MM/dd HH:mm:ss")
                    sdinfo.SD_Remark = Me.txtSD_Remark.Text

                    Dim i As Integer
                    For i = 0 To ds.Tables("SampleDivert").Rows.Count - 1
                        With ds.Tables("SampleDivert")
                            sdinfo.Code_ID = IIf(IsDBNull(.Rows(i)("Code_ID")), Nothing, .Rows(i)("Code_ID"))
                            sdinfo.SD_OutSO_ID = IIf(IsDBNull(.Rows(i)("OutSO_ID")), Nothing, .Rows(i)("OutSO_ID"))
                            sdinfo.SD_OutD_ID = IIf(IsDBNull(.Rows(i)("SD_OutD_ID")), Nothing, .Rows(i)("SD_OutD_ID"))
                            sdinfo.SD_OutPS_NO = IIf(IsDBNull(.Rows(i)("SD_OutPS_NO")), Nothing, .Rows(i)("SD_OutPS_NO"))
                            sdinfo.SD_Qty = IIf(IsDBNull(.Rows(i)("SD_Qty")), 0, .Rows(i)("SD_Qty"))

                            If sdcon.NmetalSampleDivert_Add(sdinfo) = False Then
                                MsgBox("新增失敗，请檢查原因！")
                                Exit Sub
                            End If
                        End With
                    Next
                    MsgBox("新增成功！")

                Case EditEnumType.EDIT
                    '1.删除条码
                    If ds.Tables("DelSampleDivert").Rows.Count > 0 Then
                        Dim j As Integer
                        For j = 0 To ds.Tables("DelSampleDivert").Rows.Count - 1
                            sdcon.NmetalSampleDivert_Delete(ds.Tables("DelSampleDivert").Rows(j)("AutoID"), Nothing) '刪除当前选定的
                        Next
                    End If
                    '2.修改数据
                    Dim sdinfo As New NmetalSampleDivertInfo
                    sdinfo.SD_ID = Me.txtSD_ID.Text
                    sdinfo.SD_InSO_ID = gluSO_ID.EditValue
                    sdinfo.SD_InD_ID = gluSE_OutD_ID.EditValue
                    sdinfo.SD_InPS_NO = gluInPS_NO.EditValue
                    sdinfo.SD_CardID = String.Empty
                    sdinfo.SD_AddUserID = InUserID
                    sdinfo.SD_AddDate = Format(Now, "yyyy/MM/dd HH:mm:ss")
                    sdinfo.SD_ModifyUserID = InUserID
                    sdinfo.SD_ModifyDate = Format(Now, "yyyy/MM/dd HH:mm:ss")
                    sdinfo.SD_Remark = Me.txtSD_Remark.Text
                    Dim i As Integer
                    For i = 0 To ds.Tables("SampleDivert").Rows.Count - 1
                        With ds.Tables("SampleDivert")
                            sdinfo.Code_ID = IIf(IsDBNull(.Rows(i)("Code_ID")), Nothing, .Rows(i)("Code_ID"))
                            sdinfo.SD_OutSO_ID = IIf(IsDBNull(.Rows(i)("OutSO_ID")), Nothing, .Rows(i)("OutSO_ID"))
                            sdinfo.SD_OutD_ID = IIf(IsDBNull(.Rows(i)("SD_OutD_ID")), Nothing, .Rows(i)("SD_OutD_ID"))
                            sdinfo.SD_OutPS_NO = IIf(IsDBNull(.Rows(i)("SD_OutPS_NO")), Nothing, .Rows(i)("SD_OutPS_NO"))
                            sdinfo.SD_Qty = IIf(IsDBNull(.Rows(i)("SD_Qty")), 0, .Rows(i)("SD_Qty"))
                            sdinfo.AutoID = IIf(IsDBNull(.Rows(i)("AutoID")), 0, .Rows(i)("AutoID"))
                            If IIf(IsDBNull(.Rows(i)("AutoID")), 0, .Rows(i)("AutoID")) = 0 Then
                                If sdcon.NmetalSampleDivert_Add(sdinfo) = False Then
                                    MsgBox("修改失敗，请檢查原因！")
                                    Exit Sub
                                End If
                            Else
                                If sdcon.NmetalSampleDivert_Update(sdinfo) = False Then
                                    MsgBox("修改失敗，请檢查原因！")
                                    Exit Sub
                                End If
                            End If
                        End With
                    Next
                    MsgBox("修改成功！")
            End Select
            Me.Close()
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical, "新增修改错误")
        End Try
    End Sub
#End Region

#Region "创建临时表"
    Sub CreateTable()
        ds.Tables.Clear()
        With ds.Tables.Add("SampleDivert") '子配件表
            .Columns.Add("SD_ID", GetType(String))
            .Columns.Add("Code_ID", GetType(String))
            .Columns.Add("SD_Qty", GetType(Int32))
            .Columns.Add("OutSO_SampleID", GetType(String))
            .Columns.Add("OutSO_ID", GetType(String))
            .Columns.Add("OutPM_M_Code", GetType(String))
            .Columns.Add("SD_AddDate", GetType(String))
            .Columns.Add("SD_OutD_ID", GetType(String))
            .Columns.Add("SD_OutD_Name", GetType(String))
            .Columns.Add("SD_OutPS_NO", GetType(String))
            .Columns.Add("SD_OutPS_Name", GetType(String))
            .Columns.Add("AutoID", GetType(Decimal))
        End With
        Grid1.DataSource = ds.Tables("SampleDivert")

        With ds.Tables.Add("SampleDivertQty") '子配件表
            .Columns.Add("SD_Qty", GetType(Int32))
            .Columns.Add("SWI_Qty", GetType(Int32))
            .Columns.Add("SD_OutD_ID", GetType(String))
            .Columns.Add("SD_OutPS_NO", GetType(String))
            .Columns.Add("SD_OutD_Name", GetType(String))
            .Columns.Add("SD_OutPS_Name", GetType(String))

        End With
        Grid2.DataSource = ds.Tables("SampleDivertQty")


        With ds.Tables.Add("DelSampleDivert")
            .Columns.Add("AutoID", GetType(Decimal))
        End With
    End Sub
#End Region

#Region "保存前檢查輸入數據是否正確"
    Private Function CheckSave() As Boolean
        Dim bo As Boolean = False
        CheckSave = True
        If gluSE_OutD_ID.EditValue = Nothing Then
            MsgBox("请选择收发部門！", MsgBoxStyle.Information, "提示")
            gluSE_OutD_ID.Focus()
            bo = True
        ElseIf gluSO_ID.EditValue = Nothing Then
            MsgBox("请选择订单编号不能为空！", MsgBoxStyle.Information, "提示")
            gluSO_ID.Focus()
            bo = True
        ElseIf gluInPS_NO.EditValue = Nothing Then
            MsgBox("请选择收入工序！", MsgBoxStyle.Information, "提示")
            gluInPS_NO.Focus()
            bo = True
        End If
        '-----------------------------------------------------------------------------------
        For i As Integer = 0 To ds.Tables("SampleDivert").Rows.Count - 1
            Dim strCode_ID As String = String.Empty
            Dim strOutD_ID As String = String.Empty
            Dim strSD_OutPS_NO As String = String.Empty
            Dim strOutSO_ID As String = String.Empty
            With ds.Tables("SampleDivert") '子配件表
                strCode_ID = IIf(IsDBNull(.Rows(i)("Code_ID")), Nothing, .Rows(i)("Code_ID"))
                strOutD_ID = IIf(IsDBNull(.Rows(i)("SD_OutD_ID")), Nothing, .Rows(i)("SD_OutD_ID"))
                strSD_OutPS_NO = IIf(IsDBNull(.Rows(i)("SD_OutPS_NO")), Nothing, .Rows(i)("SD_OutPS_NO"))
                strOutSO_ID = IIf(IsDBNull(.Rows(i)("OutSO_ID")), Nothing, .Rows(i)("OutSO_ID"))

                '1.部门不相同不可转移
                If gluSE_OutD_ID.EditValue <> strOutD_ID Then
                    Grid1.Focus()
                    GridView9.FocusedRowHandle = i
                    MsgBox("部门不相同不可转移！", MsgBoxStyle.Information, "提示")
                    bo = True
                End If

                '2.样办单号相同不可转移
                If gluSO_ID.EditValue = strOutSO_ID Then
                    Grid1.Focus()
                    GridView9.FocusedRowHandle = i
                    MsgBox("样办单号相同不可转移！", MsgBoxStyle.Information, "提示")
                    bo = True
                End If
                '3.条码是否被改动过
                Dim scclist As New List(Of NmetalSampleCollectionInfo)
                scclist = Sccon.NmetalSampleCollection_Getlist(Nothing, strCode_ID, Nothing, Nothing, Nothing, Nothing, False, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing)
                If scclist.Count > 0 Then
                    '4.采集表部门不存在此条码"-------------------------
                    If scclist(0).D_ID <> gluSE_OutD_ID.EditValue Then
                        txtM_Code.Text = String.Empty
                        txtM_Code.Focus()
                        lblCode.Text = "采集表部门不存在此条码"
                        bo = True
                    End If
                    '5.条码状态是否为在产
                    If scclist(0).StatusType <> "Z" Then
                        txtM_Code.Text = String.Empty
                        txtM_Code.Focus()
                        lblCode.Text = "采集表条码状态不是在产不能转移！"
                        bo = True
                    End If
                Else
                    txtM_Code.Text = String.Empty
                    txtM_Code.Focus()
                    lblCode.Text = "采集表不存在此条码"
                    bo = True
                End If

            End With
        Next
        '-----------------------------------------------------------------------是否存在库存
        ds.Tables("SampleDivertQty").Clear()
        For j As Integer = 0 To ds.Tables("SampleDivert").Rows.Count - 1
            Dim strD_IDA As String = String.Empty
            Dim strPS_NOA As String = String.Empty
            Dim strD_NameA As String = String.Empty
            Dim strPS_NameA As String = String.Empty

            Dim strD_IDB As String = String.Empty
            Dim strPS_NOB As String = String.Empty
            Dim strD_NameB As String = String.Empty
            Dim strPS_NameB As String = String.Empty
            Dim intQty As Integer = 0
            Dim boolTrue As Boolean = False

            With ds.Tables("SampleDivert") '子配件表
                strD_IDA = IIf(IsDBNull(.Rows(j)("SD_OutD_ID")), Nothing, .Rows(j)("SD_OutD_ID"))
                strPS_NOA = IIf(IsDBNull(.Rows(j)("SD_OutPS_NO")), Nothing, .Rows(j)("SD_OutPS_NO"))
                strD_NameA = IIf(IsDBNull(.Rows(j)("SD_OutD_ID")), Nothing, .Rows(j)("SD_OutD_Name"))
                strPS_NameA = IIf(IsDBNull(.Rows(j)("SD_OutPS_NO")), Nothing, .Rows(j)("SD_OutPS_Name"))
            End With

            For m As Integer = 0 To ds.Tables("SampleDivertQty").Rows.Count - 1
                With ds.Tables("SampleDivertQty") '子配件表
                    strD_IDB = IIf(IsDBNull(.Rows(m)("SD_OutD_ID")), Nothing, .Rows(m)("SD_OutD_ID"))
                    strPS_NOB = IIf(IsDBNull(.Rows(m)("SD_OutPS_NO")), Nothing, .Rows(m)("SD_OutPS_NO"))
                    strD_NameB = IIf(IsDBNull(.Rows(m)("SD_OutD_ID")), Nothing, .Rows(m)("SD_OutD_Name"))
                    strPS_NameB = IIf(IsDBNull(.Rows(m)("SD_OutPS_NO")), Nothing, .Rows(m)("SD_OutPS_Name"))
                    intQty = IIf(IsDBNull(.Rows(m)("SD_Qty")), 0, .Rows(m)("SD_Qty"))
                End With
                If strD_IDA = strD_IDB And strPS_NOA = strPS_NOB Then
                    boolTrue = True
                    ds.Tables("SampleDivertQty").Rows(m)("SD_Qty") = intQty + 1
                End If
            Next

            If boolTrue = False Then
                Dim rowA As DataRow
                rowA = ds.Tables("SampleDivertQty").NewRow
                rowA("SD_OutD_ID") = strD_IDA
                rowA("SD_OutPS_NO") = strPS_NOA
                rowA("SD_OutD_Name") = strD_NameA
                rowA("SD_OutPS_Name") = strPS_NameA
                rowA("SD_Qty") = 1
                Dim swilist As New List(Of NmetalSampleWareInventoryInfo)
                swilist = SwCon.NmetalSampleWareInventory_Getlist(Nothing, Nothing, strPS_NOA, Nothing, False, strD_IDA)
                If swilist.Count > 0 Then
                    rowA("SWI_Qty") = swilist(0).SWI_Qty
                Else
                    rowA("SWI_Qty") = 0
                End If
                ds.Tables("SampleDivertQty").Rows.Add(rowA)
            End If
        Next

        For m As Integer = 0 To ds.Tables("SampleDivertQty").Rows.Count - 1
            Dim strD_IDC As String = String.Empty
            Dim strPS_NOC As String = String.Empty
            Dim intQtyX As Integer = 0

            With ds.Tables("SampleDivertQty") '子配件表
                strD_IDC = IIf(IsDBNull(.Rows(m)("SD_OutD_ID")), Nothing, .Rows(m)("SD_OutD_ID"))
                strPS_NOC = IIf(IsDBNull(.Rows(m)("SD_OutPS_NO")), Nothing, .Rows(m)("SD_OutPS_NO"))
                intQtyX = IIf(IsDBNull(.Rows(m)("SD_Qty")), 0, .Rows(m)("SD_Qty"))
            End With
            '6.扣账库存查询---李超20131217修正
            Dim SwInfo As New NmetalSampleWareInventoryInfo
            Dim SwList As New List(Of NmetalSampleWareInventoryInfo)
            Dim intSetQty As Integer = 0
            SwList = SwCon.NmetalSampleWareInventory_Getlist(Nothing, Nothing, strPS_NOC, Nothing, False, strD_IDC)

            If SwList.Count > 0 Then
                If SwList(0).SWI_Qty >= intQtyX Then
                Else
                    MsgBox("部门库存不够,請檢查原因!", MsgBoxStyle.Information, "提示")
                    bo = True
                End If
            Else
                MsgBox("部门库存不够,請檢查原因!", MsgBoxStyle.Information, "提示")
                bo = True
            End If
        Next

        If bo = True Then
            CheckSave = False
            Exit Function
        End If
    End Function
#End Region

#Region "控件事件"
    Private Sub gluSO_ID_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles gluSO_ID.EditValueChanged
        If gluSO_ID.EditValue <> String.Empty Then
            '1.给产品编号填值
            Dim strM As String = gluSO_ID.EditValue
            Dim mc As New NmetalSampleOrdersMainControler
            txtPM_M_Code.Text = mc.NmetalSampleOrdersMain_GetList(strM, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing)(0).PM_M_Code
            LoadDataSource()
        End If
    End Sub

    Sub LoadDataSource()
        Dim splist As New List(Of NmetalSampleProcessInfo)
        splist = prcon.NmetalSampleProcessMain_GetList(Nothing, txtPM_M_Code.Text, Nothing, Nothing, Nothing, Nothing, Nothing)
        gluInPS_NO.Properties.DataSource = splist
    End Sub

    Private Sub txtM_Code_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtM_Code.KeyDown
        If e.KeyCode = Keys.Enter Then
            '1.条码重复
            Dim strM_Code As String
            Dim i As Integer
            strM_Code = Trim(UCase(Me.txtM_Code.Text))
            For i = 0 To ds.Tables("SampleDivert").Rows.Count - 1
                If strM_Code = ds.Tables("SampleDivert").Rows(i)("Code_ID") Then
                    txtM_Code.Text = String.Empty
                    txtM_Code.Focus()
                    lblCode.Text = "条码重复"
                    Exit Sub
                End If
            Next

            Dim OutSO_ID As String = String.Empty
            Dim OutPM_M_Code As String = String.Empty
            Dim SD_OutD_ID As String = String.Empty
            Dim SD_OutD_Name As String = String.Empty
            Dim OutSO_SampleID As String = String.Empty
            Dim SD_OutPS_NO As String = String.Empty
            Dim SD_OutPS_Name As String = String.Empty

            Dim scclist As New List(Of NmetalSampleCollectionInfo)
            scclist = Sccon.NmetalSampleCollection_Getlist(Nothing, strM_Code, Nothing, Nothing, Nothing, Nothing, False, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing)
            If scclist.Count > 0 Then
                '2.采集表部门不存在此条码"-------------------------
                If scclist(0).D_ID <> gluSE_OutD_ID.EditValue Then
                    txtM_Code.Text = String.Empty
                    txtM_Code.Focus()
                    lblCode.Text = "采集表部门不存在此条码"
                    Exit Sub
                End If
                '3.条码状态是否为在产
                If scclist(0).StatusType <> "Z" Then
                    txtM_Code.Text = String.Empty
                    txtM_Code.Focus()
                    lblCode.Text = "采集表条码状态不是在产不能转移！"
                    Exit Sub
                End If

                OutSO_ID = scclist(0).SO_ID
                '5.客戶是否存在此訂單條碼
                If OutSO_ID <> String.Empty Then
                    Dim somlist As New List(Of NmetalSampleOrdersMainInfo)
                    somlist = somcon.NmetalSampleOrdersMain_GetList(OutSO_ID, Nothing, Nothing, Nothing, Nothing, Nothing, True)
                    If somlist.Count > 0 Then
                        OutSO_SampleID = somlist(0).SO_SampleID
                        OutPM_M_Code = somlist(0).PM_M_Code
                    End If
                End If
                '5.订单编号不能相同
                If scclist(0).SO_ID = gluSO_ID.EditValue Then
                    txtM_Code.Text = String.Empty
                    txtM_Code.Focus()
                    lblCode.Text = "转入板单不能与转出板单相同！"
                    Exit Sub
                End If

                SD_OutD_ID = scclist(0).D_ID
                SD_OutD_Name = scclist(0).D_Dep
                '6.初始部門工序
                Dim silist As New List(Of NmetalSampInventoryCheckInfo)
                silist = sicom.NmetalSampInventoryCheckUpdate_GetList(strM_Code)
                If silist.Count > 0 Then
                    If silist(0).InPS_NO <> String.Empty Then
                        SD_OutPS_NO = silist(0).InPS_NO
                        SD_OutPS_Name = silist(0).InPS_Name
                    Else
                        SD_OutPS_NO = silist(0).OutPS_NO
                        SD_OutPS_Name = silist(0).OutPS_Name
                    End If
                End If
            Else
                txtM_Code.Text = String.Empty
                txtM_Code.Focus()
                lblCode.Text = "采集表不存在此条码"
                Exit Sub
            End If

            '3.插入临时表
            Dim row As DataRow
            row = ds.Tables("SampleDivert").NewRow
            row("Code_ID") = strM_Code
            row("SD_ID") = String.Empty

            row("OutSO_SampleID") = OutSO_SampleID
            row("OutSO_ID") = OutSO_ID
            row("OutPM_M_Code") = OutPM_M_Code
            row("SD_OutD_ID") = SD_OutD_ID
            row("SD_OutD_Name") = SD_OutD_Name
            row("SD_OutPS_NO") = SD_OutPS_NO
            row("SD_OutPS_Name") = SD_OutPS_Name
            row("SD_Qty") = 1
            row("SD_AddDate") = Format(Now, "yyyy-MM-dd HH:mm:ss")
            ds.Tables("SampleDivert").Rows.Add(row)


            '-----------------------------------------------------------------------是否存在库存
            ds.Tables("SampleDivertQty").Clear()
            For j As Integer = 0 To ds.Tables("SampleDivert").Rows.Count - 1
                Dim strD_IDA As String = String.Empty
                Dim strPS_NOA As String = String.Empty
                Dim strD_NameA As String = String.Empty
                Dim strPS_NameA As String = String.Empty

                Dim strD_IDB As String = String.Empty
                Dim strPS_NOB As String = String.Empty
                Dim strD_NameB As String = String.Empty
                Dim strPS_NameB As String = String.Empty
                Dim intQty As Integer = 0
                Dim intSWI_Qty As Integer = 0
                Dim boolTrue As Boolean = False

                With ds.Tables("SampleDivert") '子配件表
                    strD_IDA = IIf(IsDBNull(.Rows(j)("SD_OutD_ID")), Nothing, .Rows(j)("SD_OutD_ID"))
                    strPS_NOA = IIf(IsDBNull(.Rows(j)("SD_OutPS_NO")), Nothing, .Rows(j)("SD_OutPS_NO"))
                    strD_NameA = IIf(IsDBNull(.Rows(j)("SD_OutD_ID")), Nothing, .Rows(j)("SD_OutD_Name"))
                    strPS_NameA = IIf(IsDBNull(.Rows(j)("SD_OutPS_NO")), Nothing, .Rows(j)("SD_OutPS_Name"))
                End With

                For m As Integer = 0 To ds.Tables("SampleDivertQty").Rows.Count - 1
                    With ds.Tables("SampleDivertQty") '子配件表
                        strD_IDB = IIf(IsDBNull(.Rows(m)("SD_OutD_ID")), Nothing, .Rows(m)("SD_OutD_ID"))
                        strPS_NOB = IIf(IsDBNull(.Rows(m)("SD_OutPS_NO")), Nothing, .Rows(m)("SD_OutPS_NO"))
                        strD_NameB = IIf(IsDBNull(.Rows(m)("SD_OutD_ID")), Nothing, .Rows(m)("SD_OutD_Name"))
                        strPS_NameB = IIf(IsDBNull(.Rows(m)("SD_OutPS_NO")), Nothing, .Rows(m)("SD_OutPS_Name"))
                        intQty = IIf(IsDBNull(.Rows(m)("SD_Qty")), 0, .Rows(m)("SD_Qty"))
                        intSWI_Qty = IIf(IsDBNull(.Rows(m)("SWI_Qty")), 0, .Rows(m)("SWI_Qty"))
                    End With
                    If strD_IDA = strD_IDB And strPS_NOA = strPS_NOB Then
                        boolTrue = True
                        If intSWI_Qty < intQty + 1 Then
                            MsgBox("部门库存不够,請檢查原因!", MsgBoxStyle.Information, "提示")
                            Exit Sub
                        End If
                        ds.Tables("SampleDivertQty").Rows(m)("SD_Qty") = intQty + 1
                    End If
                Next

                If boolTrue = False Then
                    Dim rowA As DataRow
                    rowA = ds.Tables("SampleDivertQty").NewRow
                    rowA("SD_OutD_ID") = strD_IDA
                    rowA("SD_OutPS_NO") = strPS_NOA
                    rowA("SD_OutD_Name") = strD_NameA
                    rowA("SD_OutPS_Name") = strPS_NameA
                    rowA("SD_Qty") = 1
                    Dim swilist As New List(Of NmetalSampleWareInventoryInfo)
                    swilist = SwCon.NmetalSampleWareInventory_Getlist(Nothing, Nothing, strPS_NOA, Nothing, False, strD_IDA)
                    If swilist.Count > 0 Then
                        rowA("SWI_Qty") = swilist(0).SWI_Qty
                    Else
                        rowA("SWI_Qty") = 0
                    End If
                    ds.Tables("SampleDivertQty").Rows.Add(rowA)
                End If
            Next


            Me.txtM_Code.Text = String.Empty
            Me.lblCode.Text = String.Empty
        End If
    End Sub
#End Region

#Region "审核程序"
    ''' <summary>
    ''' 审核程序
    ''' </summary>
    ''' <remarks></remarks>
    Sub UpdateCheck()
        '1.审核事件
        If Me.CheckA.Checked = boolCheck Then
            MsgBox("审核状态没有改变，请檢查原因！")
            Exit Sub
        End If

        Dim SSI As New NmetalSampleDivertInfo
        SSI.SD_ID = txtSD_ID.Text
        SSI.SD_Check = CheckA.Checked
        SSI.SD_CheckDate = Format(Now, "yyyy-MM-dd HH:mm:ss").ToString
        SSI.SD_CheckAction = InUserID
        SSI.SD_CheckRemark = txt_checkremark.Text

        If sdcon.NmetalSampleDivert_Check(SSI) = False Then
            MsgBox("审核失敗，请檢查原因！")
            Exit Sub
        End If
        If CheckA.Checked Then
            MsgBox("审核成功", 60, "提示")
        Else
            MsgBox("取消审核成功", 60, "提示")
        End If
        '2:扣账事件
        Dim strCode_ID As String = String.Empty
        Dim strOutD_ID As String = String.Empty
        Dim strSD_OutPS_NO As String = String.Empty

        Dim strInD_ID As String = gluSE_OutD_ID.EditValue
        Dim strSD_InPS_NO As String = gluInPS_NO.EditValue
        '-----------------------------------------------------------------------是否存在库存
        For i As Integer = 0 To ds.Tables("SampleDivert").Rows.Count - 1
            With ds.Tables("SampleDivert") '子配件表
                strCode_ID = IIf(IsDBNull(.Rows(i)("Code_ID")), Nothing, .Rows(i)("Code_ID"))
                strOutD_ID = IIf(IsDBNull(.Rows(i)("SD_OutD_ID")), Nothing, .Rows(i)("SD_OutD_ID"))
                strSD_OutPS_NO = IIf(IsDBNull(.Rows(i)("SD_OutPS_NO")), Nothing, .Rows(i)("SD_OutPS_NO"))

                '2.1 扣账----------------------------------------------------
                Dim SwOutinfo As New NmetalSampleWareInventoryInfo
                Dim SwOutList As New List(Of NmetalSampleWareInventoryInfo)
                Dim intOutQty As Integer = 0
                SwOutList = SwCon.NmetalSampleWareInventory_Getlist(Nothing, Nothing, strSD_OutPS_NO, Nothing, False, strOutD_ID)
                If SwOutList.Count > 0 Then
                    If SwOutList(0).SWI_Qty >= 1 Then
                        intOutQty = SwOutList(0).SWI_Qty
                    Else
                        MsgBox("3部门库存不够,請檢查原因!", MsgBoxStyle.Information, "提示")
                        Exit Sub
                    End If
                Else
                    MsgBox("4部门库存不够,請檢查原因!", MsgBoxStyle.Information, "提示")
                    Exit Sub
                End If

                SwOutinfo.SWI_Qty = intOutQty - 1
                SwOutinfo.ModifyDate = Format(Now, "yyyy/MM/dd HH:mm:ss")
                SwOutinfo.ModifyUserID = InUserID
                SwOutinfo.D_ID = strOutD_ID
                SwOutinfo.PS_NO = strSD_OutPS_NO
                If SwCon.NmetalSampleWareInventory_Update(SwOutinfo) = False Then
                    MsgBox("發料扣賬失敗,請檢查原因!", MsgBoxStyle.Information, "提示")
                    Exit Sub
                End If

                '2.2 入账----------------------------------------------------
                Dim SwInInfo As New NmetalSampleWareInventoryInfo
                Dim SwInList As New List(Of NmetalSampleWareInventoryInfo)
                Dim intInQty As Integer = 0
                SwInList = SwCon.NmetalSampleWareInventory_Getlist(Nothing, Nothing, strSD_InPS_NO, Nothing, False, strInD_ID)
                If SwInList.Count > 0 Then
                    intInQty = SwInList(0).SWI_Qty
                End If
                SwInInfo.SWI_Qty = intInQty + 1
                SwInInfo.ModifyDate = Format(Now, "yyyy/MM/dd")
                SwInInfo.ModifyUserID = InUserID
                SwInInfo.D_ID = strInD_ID
                SwInInfo.PS_NO = strSD_InPS_NO
                If SwCon.NmetalSampleWareInventory_Update(SwInInfo) = False Then
                    MsgBox("發料入賬失敗,請檢查原因!", MsgBoxStyle.Information, "提示")
                    Exit Sub
                End If

                '2.3 修改采集表条码样办单号。产品编号，等
                Dim scclist As New List(Of NmetalSampleCollectionInfo)
                Dim sciItem As New NmetalSampleCollectionInfo
                scclist = Sccon.NmetalSampleCollection_Getlist(Nothing, strCode_ID, Nothing, Nothing, Nothing, Nothing, False, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing)
                If scclist.Count > 0 Then
                    sciItem.Remark = scclist(0).Remark
                    sciItem.PM_M_Code = txtPM_M_Code.Text
                    sciItem.AddUserID = scclist(0).AddUserID
                    sciItem.AddDate = scclist(0).AddDate
                    sciItem.ModifyUserID = InUserID
                    sciItem.ModifyDate = Format(Now, "yyyy/MM/dd HH:mm:ss")
                    sciItem.Code_ID = scclist(0).Code_ID
                    sciItem.PM_Type = scclist(0).PM_Type
                    sciItem.SO_ID = gluSO_ID.EditValue
                    sciItem.SS_Edition = scclist(0).SS_Edition
                    sciItem.SP_ID = scclist(0).SP_ID
                    Sccon.NmetalSampleCollection_Update(sciItem)
                    Sccon.NmetalSampleCollection_UpdateC(strCode_ID, strInD_ID)
                End If
                '2.4修改采集部门
            End With
        Next

        Me.Close()
    End Sub
#End Region

#Region "自动流水单号"
    Function SetSD_ID() As String
        Dim oi As New NmetalSampleDivertInfo
        Dim StrSD As String
        StrSD = "SD" & Format(Now, "yyMM")
        oi = sdcon.NmetalSampleDivert_GetID(StrSD)
        If oi Is Nothing Then
            SetSD_ID = "SD" + Format(Now, "yyMM") + "00001"
        Else
            SetSD_ID = "SD" + Format(Now, "yyMM") + Mid(CStr(CInt(Mid(oi.SD_ID, 7)) + 1000000001), 6)
        End If
    End Function
#End Region

#Region "菜单按键事件"
    Private Sub cmdDelSub_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdDelSub.Click
        If GridView9.RowCount = 0 Then Exit Sub
        Dim DelTemp As String
        DelTemp = GridView9.GetRowCellDisplayText(GridView9.GetSelectedRows(0), "AutoID")

        If DelTemp <> String.Empty Then
            Dim row As DataRow = ds.Tables("DelSampleDivert").NewRow
            row("AutoID") = ds.Tables("SampleDivert").Rows(GridView9.FocusedRowHandle)("AutoID")
            ds.Tables("DelSampleDivert").Rows.Add(row)
        End If
        ds.Tables("SampleDivert").Rows.RemoveAt(GridView9.GetSelectedRows(0))
    End Sub
#End Region

End Class