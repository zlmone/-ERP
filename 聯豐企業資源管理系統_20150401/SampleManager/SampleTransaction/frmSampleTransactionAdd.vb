Imports LFERP.DataSetting
Imports LFERP.Library.SampleManager.SamplePace
Imports LFERP.Library.SampleManager.SamplePlan
Imports LFERP.Library.SampleManager.SampleProcess
Imports LFERP.Library.SampleManager.SampleSend
Imports LFERP.Library.SampleManager.SampleOrdersMain
Imports LFERP.Library.SampleManager.SampleOrdersSub
Imports LFERP.Library.SampleManager.SampleTransaction
Imports LFERP.Library.SampleManager.SampleCollection

Public Class frmSampleTransactionAdd
#Region "属性"
    Public ds As New DataSet
    Dim SampleSub As New SampleOrdersSubControler
    Dim SampleSubInfo As New SampleOrdersSubInfo
    Dim SampleMainInfo As New SampleOrdersMainInfo
    Dim SampleMain As New SampleOrdersMainControler

    Dim stcon As New SampleTransactionControler
    Dim sccon As New SampleCollectionControler
    Dim sscon As New SampleSendControler

    Dim SamplePlan As New SamplePlanControler
    Dim SamplePlanInfo As New SamplePlanInfo
    Dim SamplePace As New SamplePaceControler
    Dim SamplePaceInfo As New SamplePaceInfo
    Dim SampleSend As New SampleSendControler
    Dim SampleSendInfo As New SampleSendInfo

    Dim mtd As New CusterControler
    Private _EditItem As String '属性栏位
    Private _AutoID As String
    Private _EditValue As String
    Private _EditType As String

    Property AutoID() As String '属性
        Get
            Return _AutoID
        End Get
        Set(ByVal value As String)
            _AutoID = value
        End Set
    End Property

    Property EditItem() As String '属性
        Get
            Return _EditItem
        End Get
        Set(ByVal value As String)
            _EditItem = value
        End Set
    End Property
    Property EditValue() As String '属性
        Get
            Return _EditValue
        End Get
        Set(ByVal value As String)
            _EditValue = value
        End Set
    End Property
    Property EditType() As String '属性
        Get
            Return _EditType
        End Get
        Set(ByVal value As String)
            _EditType = value
        End Set
    End Property
#End Region

#Region "载入窗体"
    Private Sub frmSamplePace_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        CreateTable()

        gluStatusType.Properties.DisplayMember = "StatusTypeName"
        gluStatusType.Properties.ValueMember = "StatusType"
        gluStatusType.Properties.DataSource = stcon.SampleTransactionType_GetList(Nothing, True)

        txtAddUserID.Text = InUser
        Select Case EditItem
            Case "Add"
                dateAddDate.EditValue = Format(Now, "yyyy/MM/dd")
                Me.txtAddUserID.Text = InUserID
                Me.XtraTabPage2.PageVisible = False
            Case "Edit"
                Me.XtraTabPage2.PageVisible = False
                LoadData(EditValue)
            Case "Check"
                XtraTabControl1.SelectedTabPage = XtraTabPage2
                gluStatusType.EditValue = EditType
                LoadData(EditValue)
            Case "Look"
                LoadData(EditValue)
                If gluStatusType.EditValue = "C" Then
                    gluSP_ID.Visible = True
                    Label2.Visible = True
                Else
                    gluSP_ID.Visible = False
                    Label2.Visible = False
                End If
                gluStatusType.EditValue = EditType
                Me.cmdSave.Visible = False
        End Select
    End Sub
#End Region

#Region "创建临时表"
    ''' <summary>
    ''' 创建临时表
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub CreateTable()
        ds.Tables.Clear()
        With ds.Tables.Add("SampleTransaction")
            .Columns.Add("Code_ID", GetType(String))
            .Columns.Add("Qty", GetType(String))
            .Columns.Add("Remark", GetType(String))
            .Columns.Add("AutoID", GetType(Decimal))
        End With
        GridSampleTransaction.DataSource = ds.Tables("SampleTransaction")

        With ds.Tables.Add("DelSampleTransaction")
            .Columns.Add("AutoID", GetType(String))
        End With
    End Sub
#End Region

#Region "載入数据"
    ''' <summary>
    ''' 載入数据
    ''' </summary>
    Sub LoadData(ByVal strTR_ID As String) '返回数据
        Dim som As New List(Of SampleTransactionInfo)
        som = stcon.SampleTransaction_Getlist(Nothing, Nothing, Nothing, strTR_ID, Nothing, Nothing, Nothing)
        If som.Count = 0 Then
            Exit Sub
        Else
            Me.txtTR_ID.Text = som(0).TR_ID
            Me.dateAddDate.Text = som(0).AddDate
            Me.gluStatusType.EditValue = som(0).StatusType

            Me.gluSP_ID.Text = som(0).SP_ID

            Me.txtAddUserID.Text = som(0).AddUserName
            Me.CheckEdit2.Checked = som(0).CheckBit
            '.....................................................
            Dim i As Integer
            ds.Tables("SampleTransaction").Clear()
            For i = 0 To som.Count - 1
                Dim row As DataRow
                row = ds.Tables("SampleTransaction").NewRow
                row("Code_ID") = som(i).Code_ID
                row("Qty") = som(i).Qty
                row("Remark") = som(i).Remark
                row("AutoID") = som(i).AutoID
                ds.Tables("SampleTransaction").Rows.Add(row)
            Next
        End If
    End Sub
#End Region

#Region "子表事件"
    ''' <summary>
    ''' 表格新增
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub tsmNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsmNew.Click
        Dim fr As New frmSampleBarcode
        fr = New frmSampleBarcode
        fr.Lbl_Title.Text = "条码採集"
        fr.EditItem = "SampleTransaction"

        fr.ShowDialog()

        Dim i As Integer
        For i = 0 To fr.TransactionList.Count - 1
            Dim row As DataRow
            row = ds.Tables("SampleTransaction").NewRow
            row("Code_ID") = fr.TransactionList(i).Code_ID
            row("Qty") = fr.TransactionList(i).Qty
            row("Remark") = fr.TransactionList(i).Remark

            ds.Tables("SampleTransaction").Rows.Add(row)
        Next
    End Sub

    ''' <summary>
    ''' 刪除事件
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub tsmDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsmDelete.Click
        If GridView1.RowCount = 0 Then Exit Sub

        Dim DelTemp As String
        DelTemp = GridView1.GetRowCellDisplayText(GridView1.GetSelectedRows(0), "AutoID")
        If DelTemp <> String.Empty Then
            Dim row As DataRow = ds.Tables("DelSampleTransaction").NewRow
            row("AutoID") = ds.Tables("SampleTransaction").Rows(GridView1.FocusedRowHandle)("AutoID")
            ds.Tables("DelSampleTransaction").Rows.Add(row)
        End If
        ds.Tables("SampleTransaction").Rows.RemoveAt(GridView1.GetSelectedRows(0))
    End Sub
#End Region

#Region "按键事件"
    Private Sub cmdExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdExit.Click
        Me.Close()
    End Sub
    ''' <summary>
    ''' 保存事件
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub cmdSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSave.Click
        If DataCheckEmpty() = 0 Then
            Exit Sub
        End If
        Select Case EditItem
            Case "Add"
                DataNew()
            Case "Edit"
                DataEdit()
            Case "Check"
                UpdateCheck()
        End Select
    End Sub

    Private Sub gluStatusType_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles gluStatusType.EditValueChanged
        If gluStatusType.EditValue = "C" Then
            gluSP_ID.Visible = True
            Label2.Visible = True
            gluSP_ID.Properties.DisplayMember = "SP_ID"
            gluSP_ID.Properties.ValueMember = "AutoID"

            If EditItem = "Look" Then
                gluSP_ID.Properties.DataSource = sscon.SampleSendSP_GetList(Nothing, Nothing, Nothing)
            Else
                gluSP_ID.Properties.DataSource = sscon.SampleSendSP_GetList(Nothing, Nothing, 1)
            End If
        Else
            gluSP_ID.Visible = False
            Label2.Visible = False
        End If
    End Sub
#End Region

#Region "审核程序"
    ''' <summary>
    ''' 审核程序
    ''' </summary>
    ''' <remarks></remarks>
    Sub UpdateCheck()

        Dim stinfo As New SampleTransactionInfo
        stinfo.TR_ID = txtTR_ID.Text
        stinfo.CheckBit = CheckEdit2.Checked
        stinfo.CheckDate = Format(Now, "yyyy/MM/dd").ToString
        stinfo.CheckUserID = InUserID
        stinfo.CheckRemark = MemoEdit2.Text
        If stcon.SampleTransaction_UpdateCheck(stinfo) = False Then
            MsgBox("审核失敗，请檢查原因！")
            Exit Sub
        End If

        '-----------------------------------------
        Dim som As New List(Of SampleTransactionInfo)
        Dim sccon As New SampleCollectionControler
        som = stcon.SampleTransaction_Getlist(Nothing, Nothing, Nothing, txtTR_ID.Text, Nothing, Nothing, Nothing)

        Dim i As Integer
        Dim strCode_ID As String = String.Empty
        Dim StrStatusType As String = String.Empty
        Dim strSP_ID As String = String.Empty
        For i = 0 To som.Count - 1
            strCode_ID = som(i).Code_ID

            If Me.gluStatusType.EditValue = "A" Then
                StrStatusType = "E"
            Else
                StrStatusType = Me.gluStatusType.EditValue
            End If
            If CheckEdit2.Checked = False Then
                StrStatusType = String.Empty
            End If

            If sccon.SampleCollection_UpdateA(strCode_ID, StrStatusType) = False Then
                MsgBox("修改類型失敗，请檢查原因！")
                Exit Sub
            End If

            If gluStatusType.EditValue = "C" Then
                If sccon.SampleCollection_UpdateB(strCode_ID, gluSP_ID.Text) = False Then
                    MsgBox("修改寄送單失敗，请檢查原因！")
                    Exit Sub
                End If
            End If
        Next
        MsgBox("审核成功")
        Me.Close()
    End Sub
#End Region

#Region "新增修改程序"
    ''' <summary>
    ''' 新增程序
    ''' </summary>
    ''' <remarks></remarks>
    Sub DataNew() '新增
        Dim sslist As New List(Of SampleSendInfo)
        Dim stinfo As New SampleTransactionInfo
        stinfo.TR_ID = GetTR_ID()
        stinfo.AddDate = dateAddDate.Text
        stinfo.AddUserID = InUserID
        stinfo.StatusType = Me.gluStatusType.EditValue
        If gluStatusType.EditValue = "C" Then
            If Me.gluSP_ID.EditValue <> String.Empty Then
                sslist = sscon.SampleSendSP_GetList(Nothing, gluSP_ID.EditValue, Nothing)
                If sslist.Count > 0 Then
                    stinfo.SP_ID = sslist(0).AutoID
                    stinfo.SO_ID = sslist(0).SO_ID
                    stinfo.SS_Edition = sslist(0).SS_Edition
                End If
            End If
        End If
        Dim i As Integer
        For i = 0 To ds.Tables("SampleTransaction").Rows.Count - 1
            With ds.Tables("SampleTransaction")
                stinfo.Code_ID = IIf(IsDBNull(.Rows(i)("Code_ID")), Nothing, .Rows(i)("Code_ID"))
                stinfo.Qty = IIf(IsDBNull(.Rows(i)("Qty")), 0, .Rows(i)("Qty"))
                stinfo.Remark = IIf(IsDBNull(.Rows(i)("Remark")), Nothing, .Rows(i)("Remark"))
                stinfo.AutoID = IIf(IsDBNull(.Rows(i)("AutoID")), 0, .Rows(i)("AutoID"))

                If IIf(IsDBNull(.Rows(i)("AutoID")), 0, .Rows(i)("AutoID")) > 0 Then
                    If stcon.SampleTransaction_Update(stinfo) = False Then
                        MsgBox("新增失敗，请檢查原因！")
                        Exit Sub
                    End If
                Else
                    If stcon.SampleTransaction_Add(stinfo) = False Then
                        MsgBox("新增失敗，请檢查原因！")
                        Exit Sub
                    End If
                End If
            End With
        Next
        MsgBox("新增成功")
        Me.Close()
    End Sub

    ''' <summary>
    '''修改
    ''' </summary>
    ''' <remarks></remarks>
    Sub DataEdit()
        '更新刪除列表記錄
        If ds.Tables("DelSampleTransaction").Rows.Count > 0 Then
            Dim j As Integer
            For j = 0 To ds.Tables("DelSampleTransaction").Rows.Count - 1
                stcon.SampleTransaction_Delete(ds.Tables("DelSampleTransaction").Rows(j)("AutoID"), Nothing) '刪除当前选定的
            Next
        End If
        Dim sslist As New List(Of SampleSendInfo)
        Dim stinfo As New SampleTransactionInfo
        stinfo.TR_ID = Me.txtTR_ID.Text
        stinfo.AddDate = dateAddDate.Text
        stinfo.AddUserID = InUserID
        stinfo.StatusType = Me.gluStatusType.EditValue
        stinfo.ModifyDate = Format(Now, "yyyy/MM/dd")
        stinfo.ModifyUserID = InUserID


        If gluStatusType.EditValue = "C" Then
            If Me.gluSP_ID.EditValue <> String.Empty Then
                sslist = sscon.SampleSendSP_GetList(Nothing, gluSP_ID.EditValue, Nothing)
                If sslist.Count > 0 Then
                    stinfo.SP_ID = sslist(0).AutoID
                    stinfo.SO_ID = sslist(0).SO_ID
                    stinfo.SS_Edition = sslist(0).SS_Edition
                End If
            End If
        End If

        Dim i As Integer
        For i = 0 To ds.Tables("SampleTransaction").Rows.Count - 1
            With ds.Tables("SampleTransaction")
                stinfo.Code_ID = IIf(IsDBNull(.Rows(i)("Code_ID")), Nothing, .Rows(i)("Code_ID"))
                stinfo.Qty = IIf(IsDBNull(.Rows(i)("Qty")), 0, .Rows(i)("Qty"))
                stinfo.Remark = IIf(IsDBNull(.Rows(i)("Remark")), Nothing, .Rows(i)("Remark"))
                stinfo.AutoID = IIf(IsDBNull(.Rows(i)("AutoID")), 0, .Rows(i)("AutoID"))
                If IIf(IsDBNull(.Rows(i)("AutoID")), 0, .Rows(i)("AutoID")) > 0 Then
                    If stcon.SampleTransaction_Update(stinfo) = False Then
                        MsgBox("修改失敗，请檢查原因！")
                        Exit Sub
                    End If
                Else
                    If stcon.SampleTransaction_Add(stinfo) = False Then
                        MsgBox("修改失敗，请檢查原因！")
                        Exit Sub
                    End If
                End If
            End With
        Next
        MsgBox("修改成功！")
        Me.Close()
    End Sub
#End Region

#Region "检查数据"
    ''' <summary>
    ''' 是否为空
    ''' </summary>
    ''' <remarks></remarks>
    Function DataCheckEmpty() As Integer
        If txtAddUserID.Text = String.Empty Then
            MsgBox("創建人員不能为空,请输入！")
            txtAddUserID.Focus()
            DataCheckEmpty = 0
            Exit Function
        End If
        If gluStatusType.Text = String.Empty Then
            MsgBox("類型不能为空,请输入！")
            gluStatusType.Focus()
            DataCheckEmpty = 0
            Exit Function
        End If
        If gluStatusType.EditValue = "C" Then
            If gluSP_ID.Text = String.Empty Then
                MsgBox("寄送單號不能为空,请输入！")
                gluSP_ID.Focus()
                DataCheckEmpty = 0
                Exit Function
            End If
        End If

        Dim i As Integer
        Dim strCode_ID As String = String.Empty

        For i = 0 To ds.Tables("SampleTransaction").Rows.Count - 1
            If IsDBNull(ds.Tables("SampleTransaction").Rows(i)("Code_ID")) Then
                MsgBox("样办订单号不能为空，请输入样办订单号！", 64, "提示")
                GridSampleTransaction.Focus()
                GridView1.FocusedRowHandle = i
                DataCheckEmpty = 0
                Exit Function
            End If

            Dim scconList As New List(Of SampleCollectionInfo)
            strCode_ID = ds.Tables("SampleTransaction").Rows(i)("Code_ID")
            Dim strType As String
            Dim strTyeName As String
            scconList = sccon.SampleCollection_Getlist(Nothing, strCode_ID, Nothing, Nothing, Nothing, Nothing, False, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing)

            If scconList.Count <= 0 Then
                MsgBox("此條碼在采集表中沒有記錄！", 64, "提示")
                GridSampleTransaction.Focus()
                GridView1.FocusedRowHandle = i
                DataCheckEmpty = 0
                Exit Function
            Else
                strType = scconList(0).StatusType
                strTyeName = scconList(0).StatusTypeName
                If strTyeName = "" Then
                    strTyeName = "[採集中]"
                End If
            End If

            Select Case gluStatusType.EditValue
                Case "A" '入庫
                    If strType = "" Or strType = "B" Or strType = "Z" Or strType = "M" Then
                    Else
                        MsgBox("此條碼" + strCode_ID + "為" + strTyeName + "狀態請檢查采集數據！", 64, "提示")
                        GridSampleTransaction.Focus()
                        GridView1.FocusedRowHandle = i
                        DataCheckEmpty = 0
                        Exit Function
                    End If
                Case "B" '出庫
                    If strType <> "E" Then
                        MsgBox("此條碼" + strCode_ID + "為" + strTyeName + "狀態請檢查采集數據！", 64, "提示")
                        GridSampleTransaction.Focus()
                        GridView1.FocusedRowHandle = i
                        DataCheckEmpty = 0
                        Exit Function
                    End If
                Case "C" '寄送
                    If strType <> "E" Then
                        MsgBox("此條碼" + strCode_ID + "為" + strTyeName + "狀態請檢查采集數據！", 64, "提示")
                        GridSampleTransaction.Focus()
                        GridView1.FocusedRowHandle = i
                        DataCheckEmpty = 0
                        Exit Function
                    End If
                Case "D" '
                    If strType <> "E" Then
                        MsgBox("此條碼" + strCode_ID + "為" + strTyeName + "狀態請檢查采集數據！", 64, "提示")
                        GridSampleTransaction.Focus()
                        GridView1.FocusedRowHandle = i
                        DataCheckEmpty = 0
                        Exit Function
                    End If
            End Select
        Next
        DataCheckEmpty = 1
    End Function
#End Region

#Region "自动流水号"
    ''' <summary>
    ''' 自動流水号
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Function GetTR_ID() As String
        Dim stcon As New SampleTransactionControler
        Dim stinfo As New SampleTransactionInfo
        Dim ndate As String = "TR" + Format(Now(), "yyMM")
        stinfo = stcon.SampleTransaction_Get(ndate)
        If stinfo Is Nothing Then
            GetTR_ID = "TR" + Format(Now, "yyMM") + "0001"
        Else
            GetTR_ID = "TR" + Format(Now, "yyMM") + Mid(CInt(Mid(stinfo.TR_ID, 7)) + 1000000001, 7)
        End If
    End Function
#End Region

End Class