Imports DevExpress.XtraGrid.Columns
Imports DevExpress.XtraGrid.Views.Base
Imports DevExpress.XtraGrid.Views.BandedGrid
Imports DevExpress.XtraEditors.Repository

Imports System.Data.SqlClient

Imports LFERP.Library.Product
Imports LFERP.Library.ProductProcess
Imports LFERP.Library.Production.ProductionDetailMainMonthSum
Imports LFERP.Library.Production.ProductionFieldDaySummaryInput
Imports LFERP.Library.Production.ProductionFieldDaySummaryCreat

Imports LFERPDB

Public Class frmProductionDetailMainMonthMain
    Dim DSHead As New DataSet

    Dim ds As New DataSet

    Dim Ex_Count As Integer
    Dim Ex_ID(50) As String
    Dim Ex_Type_Name(50) As String
    Dim Ex_CO_ID(50) As String
    Dim Ex_Sum_Content(50) As String

    Dim WeekBuff(10) As Date

    Dim PressSign As String
    Dim MonthWeekJS As Integer
    Dim CONN As String

    Private Sub AdvBandedGridView1_CustomDrawCell(ByVal sender As Object, ByVal e As DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventArgs) Handles AdvBandedGridView1.CustomDrawCell
        Dim strTemp As String = "0"
        If IsDBNull(AdvBandedGridView1.GetRowCellDisplayText(e.RowHandle, e.Column)) = False And e.Column.FieldName <> "PM_M_Code" And e.Column.FieldName <> "PM_Type" And e.Column.FieldName <> "FiledDate" Then

            strTemp = AdvBandedGridView1.GetRowCellDisplayText(e.RowHandle, e.Column)

            If Val(strTemp) > 0 Then
                e.Appearance.BackColor = Color.DeepSkyBlue
            End If

            Select Case PressSign

                Case "1"
                    Dim inputmonth As Integer

                    inputmonth = Val(Format(CDate(Me.YearDate.EditValue), "MM"))
                    Dim days1 As Integer
                    If inputmonth = 12 Then
                        days1 = 31
                    Else
                        Dim dt As New DateTime(DateTime.Today.Year, inputmonth, 1)                 '|
                        days1 = dt.AddMonths(1).DayOfYear - dt.DayOfYear
                    End If
                    ''-----------------------------
                    strTemp = AdvBandedGridView1.GetRowCellDisplayText(e.RowHandle, e.Column)

                    Dim i As Integer
                    Dim strTempA, strTempB As String

                    For i = 1 To days1
                        If e.Column.FieldName = "AA" & Format(i, "00") Then
                            strTempA = AdvBandedGridView1.GetRowCellDisplayText(e.RowHandle, "AA" & Format(i, "00"))
                            strTempB = AdvBandedGridView1.GetRowCellDisplayText(e.RowHandle, "FF" & Format(i, "00"))

                            If Val(strTempA) < Val(strTempB) Then
                                e.Appearance.BackColor = Color.Red
                            End If

                            Exit Sub
                        End If
                    Next

                Case "2"

                    ''-----------------------------
                    strTemp = AdvBandedGridView1.GetRowCellDisplayText(e.RowHandle, e.Column)

                    Dim i As Integer
                    Dim strTempA, strTempB As String

                    For i = 1 To MonthWeekJS
                        If e.Column.FieldName = "第" & Format(i, "00") & "周AA" Then
                            strTempA = AdvBandedGridView1.GetRowCellDisplayText(e.RowHandle, "第" & Format(i, "00") & "周AA")
                            strTempB = AdvBandedGridView1.GetRowCellDisplayText(e.RowHandle, "第" & Format(i, "00") & "周FF")

                            If Val(strTempA) < Val(strTempB) Then
                                e.Appearance.BackColor = Color.Red
                            End If

                            Exit Sub
                        End If
                    Next

                Case "3"
                    ''-----------------------------
                    strTemp = AdvBandedGridView1.GetRowCellDisplayText(e.RowHandle, e.Column)

                    Dim i As Integer
                    Dim strTempA, strTempB As String

                    For i = 1 To 12
                        If e.Column.FieldName = "Z" & Format(i, "00") & "月AA" Then
                            strTempA = AdvBandedGridView1.GetRowCellDisplayText(e.RowHandle, "Z" & Format(i, "00") & "月AA")
                            strTempB = AdvBandedGridView1.GetRowCellDisplayText(e.RowHandle, "Z" & Format(i, "00") & "月FF")

                            If Val(strTempA) < Val(strTempB) Then
                                e.Appearance.BackColor = Color.Red
                            End If

                            Exit Sub
                        End If
                    Next


            End Select




        End If
    End Sub

    Private Sub cmdExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdExit.Click
        Me.Close()
    End Sub

    Private Sub ExportStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExportStripMenuItem.Click

        SaveFileDialog1.Title = "导出Excel"
        '        SaveFileDialog1.Filter = "Excel文件(*.xls)|*.xls"
        SaveFileDialog1.Filter = "(*.xls)|*.xls|(*.xlsx)|*.xlsx"
        Dim dialogResult__1 As DialogResult = SaveFileDialog1.ShowDialog(Me)
        If dialogResult__1 = Windows.Forms.DialogResult.OK Then
            GridControlExcel.ExportToExcelOld(SaveFileDialog1.FileName)
            DevExpress.XtraEditors.XtraMessageBox.Show("保存成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If
    End Sub


    Function Ds_ProductionDay(ByVal MonthDay As String, ByVal PM_M_Code As String, ByVal PM_Type As String) As Boolean

        Dim i, j As Integer

        Dim strStat_Date, strEnd_Date As String
        ''---------------------------------------------------------------------------------
        Dim intInputMonth As Integer '这是你输入的月份                                '|
        intInputMonth = Val(Format(CDate(MonthDay), "MM"))                            '| 

        Dim dt As New DateTime(DateTime.Today.Year, intInputMonth, 1)                 '|
        '计算该月份的天数
        Dim days As Integer = dt.AddMonths(1).DayOfYear - dt.DayOfYear                '|
        If days < 0 Or days > 31 Then
            days = 31
        End If
        strStat_Date = (dt.AddDays(0).ToString("yyyy/MM/dd"))                         '|
        strEnd_Date = (dt.AddDays(days - 1).ToString("yyyy/MM/dd"))                   '|
        ''---------------------------------------------------------------------------------
        Dim BZ As String


        Dim view As BandedGridView = TryCast(Me.AdvBandedGridView1, BandedGridView)
        view.Bands.Clear()
        view.Columns.Clear()

        'view.BeginUpdate();
        view.BeginUpdate()
        '开始视图的编辑，防止触发其他事件
        view.BeginDataUpdate()
        '开始数据的编辑            
        '添加列标题
        'view.OptionsView.ShowColumnHeaders = False
        view.OptionsView.ColumnAutoWidth = False

        view.OptionsView.EnableAppearanceEvenRow = True '';                   //是否启用偶数行外观
        view.OptionsView.EnableAppearanceOddRow = True '';                     //是否启用奇数行外观
        view.OptionsCustomization.AllowColumnResizing = False ';              //是否允许调整列宽
        view.OptionsView.ShowFooter = False

        view.OptionsView.AllowCellMerge = False

        ''DSHead

        DSHead.Tables.Clear()
        DSHead.Clear()

        Dim SQLStr As String = ""
        Dim SumSQLStr As String = ""
        Dim SumJS As Integer
        Dim dsFile(1000) As String

        Dim Sql As String = ""

        view.Bands.AddBand("產品編號")
        view.Bands.AddBand("類型")


        For i = 1 To days
            view.Bands.AddBand(Format(CDate(Me.YearDate.EditValue), "yyyy年MM月") & Format(i, "00") & "日")
            view.Bands(i + 1).AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center

            For j = 1 To Ex_Count
                view.Bands(i + 1).Children.AddBand(Trim(Ex_Type_Name(j)))
                view.Bands(i + 1).Children(j - 1).AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center

                SumJS = SumJS + 1
                Dim TempS1, TempS2 As String

                TempS1 = Trim(Ex_ID(j))
                TempS2 = CStr(Format(i, "00"))
                dsFile(SumJS) = TempS1 + TempS2

                SQLStr = SQLStr + dsFile(SumJS) + ","
            Next
        Next


        SQLStr = "PM_M_Code,PM_Type" + "," + Mid(SQLStr, 1, Len(SQLStr) - 1)


        Dim TableName As String
        TableName = "ProductionDetailMainMonthSum"


        Dim StrWhere As String
        StrWhere = "FiledDate ='" + Format(CDate(MonthDay), "yyy-MM") + "'"
        If PM_M_Code Is Nothing Then
        Else
            StrWhere = StrWhere + "and PM_M_Code ='" + PM_M_Code + "'"
        End If

        If PM_Type Is Nothing Then
        Else
            StrWhere = StrWhere + "and PM_Type ='" + PM_Type + "'"
        End If

        Sql = "select " + SQLStr + " from " + TableName + " where " + StrWhere



        ''-----------------------------------------------------
        Dim conn1 As New SqlConnection(CONN)
        Dim sqladapter As New SqlDataAdapter(Sql, conn1)
        conn1.Open()

        sqladapter.Fill(DSHead)
        conn1.Close()
        ''綁定數據字段
        Me.GridControlExcel.DataSource = DSHead.Tables(0)

        ' GridControl1.Dispose()

        'GridControl1.DataSource = DSHead.Tables(0)


        If DSHead.Tables(0).Rows.Count <= 0 Then
            'BZ = "Err"
            'Exit Function
        End If
        '------------------------------------------------------------------------------------
        SumJS = 0

        view.Columns("PM_M_Code").OwnerBand = view.Bands(0) 'PM_Type
        view.Columns("PM_Type").OwnerBand = view.Bands(1) '

        view.Columns("PM_M_Code").Width = 125
        view.Columns("PM_M_Code").OptionsColumn.ReadOnly = True
        view.Columns("PM_Type").Width = 125
        view.Columns("PM_Type").OptionsColumn.ReadOnly = True
        view.Columns("PM_M_Code").OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.False

        view.Bands(0).Fixed = FixedStyle.Left
        view.Bands(1).Fixed = FixedStyle.Left

        On Error Resume Next
        For i = 1 To days
            For j = 1 To Ex_Count
                SumJS = SumJS + 1
                view.Columns(dsFile(SumJS)).OwnerBand = view.Bands(i + 1).Children(j - 1)
                view.Columns(dsFile(SumJS)).OptionsColumn.ReadOnly = True
                view.Columns(dsFile(SumJS)).Width = 65
            Next
        Next

        view.EndDataUpdate()
        '结束数据的编辑
        view.EndUpdate()
        '结束视图的编辑
    End Function

    Sub CrateTable()
        With ds.Tables.Add("PM_M_Code")
            .Columns.Add("PM_M_Code", GetType(String))
            .Columns.Add("PM_JiYu", GetType(String))
        End With

        PM_M_Code.Properties.DisplayMember = "PM_M_Code"
        PM_M_Code.Properties.ValueMember = "PM_M_Code"
        PM_M_Code.Properties.DataSource = ds.Tables("PM_M_Code")

        LoadPM_M_Code()
    End Sub

    Private Sub frmProductionDetailMainMonthMain_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim ai As New LFERPDataBase
        CONN = ai.LoadConnStr

        YearDate.EditValue = Format(Now, "yyyy年MM月")
        ' YearDate.EditValue = "2012年8月"

        CrTable()

        If LoadProductionFieldDaySummaryInputType() = False Then
            MsgBox("統計的工序類型為無記錄!")
            Exit Sub
        End If

        RadioButton1.Checked = True
        RadioButton1_Click(Nothing, Nothing)

    End Sub

    Private Sub CollectStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CollectStripMenuItem.Click
        frmProductionDetailMainMonthSum.ShowDialog()
        frmProductionDetailMainMonthSum.Dispose()
    End Sub

    ''' <summary>
    ''' 載入 每道 工序需要統計的類型
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Function LoadProductionFieldDaySummaryInputType() As Boolean
        LoadProductionFieldDaySummaryInputType = True

        Dim ptc As New ProductionFieldDaySummaryInputControl
        Dim ptl As New List(Of ProductionFieldDaySummaryInputInfo)
        Dim j As Integer

        ptl = ptc.ProductionFieldDaySummaryInputType_GetList(Nothing, Nothing, Nothing, "MonthSum")

        Ex_Count = ptl.Count

        If Ex_Count <= 0 Then
            LoadProductionFieldDaySummaryInputType = False
            Exit Function
        End If

        For j = 1 To Ex_Count
            Ex_ID(j) = ptl(j - 1).ID
            Ex_Type_Name(j) = ptl(j - 1).TypeName
            Ex_CO_ID(j) = ptl(j - 1).CO_ID
            Ex_Sum_Content(j) = ptl(j - 1).Sum_Content
        Next
    End Function

    Private Sub cmdSelect_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSelect.Click


        Dim strPM_M_Code As String
        Dim strPM_Type As String

        If PM_M_Code.EditValue = "全部" Or PM_M_Code.EditValue Is Nothing Or PM_M_Code.EditValue = "" Then
            strPM_M_Code = Nothing
        Else
            strPM_M_Code = PM_M_Code.EditValue
        End If

        If gluType.EditValue = "全部" Or gluType.EditValue Is Nothing Or gluType.EditValue = "" Then
            strPM_Type = Nothing
        Else
            strPM_Type = gluType.EditValue
        End If

        Ds_ProductionDay(Format(CDate(YearDate.EditValue), "yyyy-MM"), strPM_M_Code, strPM_Type)

    End Sub

    Sub CrTable()
        With ds.Tables.Add("PM_M_Code")
            .Columns.Add("PM_M_Code", GetType(String))
            .Columns.Add("PM_JiYu", GetType(String))
        End With

        PM_M_Code.Properties.DisplayMember = "PM_M_Code"
        PM_M_Code.Properties.ValueMember = "PM_M_Code"
        PM_M_Code.Properties.DataSource = ds.Tables("PM_M_Code")

        With ds.Tables.Add("TPM_Type")
            .Columns.Add("PM_Type", GetType(String))
        End With

        gluType.Properties.DisplayMember = "PM_Type"
        gluType.Properties.ValueMember = "PM_Type"
        gluType.Properties.DataSource = ds.Tables("TPM_Type")


        LoadPM_M_Code()
    End Sub

    Private Sub PM_M_Code_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PM_M_Code.EditValueChanged
        If PM_M_Code.EditValue Is Nothing Or PM_M_Code.EditValue = "" Then
            Exit Sub
        End If

        ds.Tables("TPM_Type").Clear()

        Dim strPM_M_Code As String

        If PM_M_Code.EditValue = "全部" Then
            strPM_M_Code = Nothing
        Else
            strPM_M_Code = PM_M_Code.EditValue
        End If


        Dim pcc As New ProcessMainControl
        Dim pcl As New List(Of ProcessMainInfo)

        pcl = pcc.ProcessMain_GetList1(Nothing, PM_M_Code.EditValue, "生產加工", Nothing)

        Dim row As DataRow
        Dim j As Integer

        row = ds.Tables("TPM_Type").NewRow
        row("PM_Type") = "全部"
        ds.Tables("TPM_Type").Rows.Add(row)

        If pcl.Count > 0 Then
            For j = 0 To pcl.Count - 1
                row = ds.Tables("TPM_Type").NewRow
                row("PM_Type") = pcl(j).Type3ID
                ds.Tables("TPM_Type").Rows.Add(row)
            Next
        End If

    End Sub


    ''' <summary>
    ''' 載入產品編號 (進入生產工藝的）
    ''' </summary>
    ''' <remarks></remarks>
    Sub LoadPM_M_Code()
        ''------------------------------------------------
        Dim row As DataRow
        Dim j As Integer
        Dim mpi As List(Of ProductionFieldDaySummaryCreatInfo)
        Dim mpc As New ProductionFieldDaySummaryCreatControl

        mpi = mpc.ProductionFieldDaySummarySF_ProcessMainGetList(Nothing)

        row = ds.Tables("PM_M_Code").NewRow
        row("PM_M_Code") = "全部"
        row("PM_JiYu") = "全部"
        ds.Tables("PM_M_Code").Rows.Add(row)

        If mpi.Count > 0 Then
            For j = 0 To mpi.Count - 1
                row = ds.Tables("PM_M_Code").NewRow
                row("PM_M_Code") = mpi(j).PM_M_Code
                row("PM_JiYu") = mpi(j).PM_JiYu
                ds.Tables("PM_M_Code").Rows.Add(row)
            Next
        End If
    End Sub


    Private Sub Label3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Label3.Click
        Dim view As BandedGridView = TryCast(Me.AdvBandedGridView1, BandedGridView)
        View.Bands.Clear()
    End Sub



    Function Ds_ProductionWeek(ByVal MonthDay As String, ByVal PM_M_Code As String, ByVal PM_Type As String) As Boolean

        Dim i, j As Integer

        Dim strStat_Date, strEnd_Date As String
        ''---------------------------------------------------------------------------------
        Dim intInputMonth As Integer '这是你输入的月份                                '|
        intInputMonth = Val(Format(CDate(MonthDay), "MM"))                            '| 

        Dim dt As New DateTime(DateTime.Today.Year, intInputMonth, 1)                 '|
        '计算该月份的天数
        Dim days As Integer = dt.AddMonths(1).DayOfYear - dt.DayOfYear                '|
        If days <= 0 Or days > 31 Then
            days = 31
        End If
        strStat_Date = (dt.AddDays(0).ToString("yyyy/MM/dd"))                         '|
        strEnd_Date = (dt.AddDays(days - 1).ToString("yyyy/MM/dd"))                   '|
        ''---------------------------------------------------------------------------------
        Dim BZ As String


        Dim view As BandedGridView = TryCast(Me.AdvBandedGridView1, BandedGridView)
        view.Bands.Clear()
        view.Columns.Clear()

        'view.BeginUpdate();
        view.BeginUpdate()
        '开始视图的编辑，防止触发其他事件
        view.BeginDataUpdate()
        '开始数据的编辑            
        '添加列标题
        'view.OptionsView.ShowColumnHeaders = False
        view.OptionsView.ColumnAutoWidth = False

        view.OptionsView.EnableAppearanceEvenRow = True '';                   //是否启用偶数行外观
        view.OptionsView.EnableAppearanceOddRow = True '';                     //是否启用奇数行外观
        view.OptionsCustomization.AllowColumnResizing = False ';              //是否允许调整列宽
        view.OptionsView.ShowFooter = False

        ''DSHead

        DSHead.Tables.Clear()
        DSHead.Clear()

        Dim SQLStr As String = ""
        Dim SumSQLStr As String = ""
        Dim SumJS As Integer
        Dim dsFile(1000) As String

        Dim Sql As String = ""




        Dim WeekBuff(10) As Date
        Dim WeekBuffA(10, 10) As String
        ''----------------------------------------------------------------------------------------------------------------
        Dim monthid As String
        monthid = Format(CDate(MonthDay), "yyyyMM")

        Dim yx, mx As String
        yx = monthid.Substring(0, 4)
        mx = monthid.Substring(4)

        Dim totalweeks As Integer

        Dim FirstDayOfMonth As Date = New DateTime(yx, mx, 1)
        Dim LastDayOfMonth As Date = New DateTime(yx, mx, Date.DaysInMonth(yx, mx))
        Dim d As Date = FirstDayOfMonth
        Dim ed(0) As Date

        Dim j1, k1 As Int16
        Dim bzbz As String = ""

        For i1 As Int16 = 0 To Date.DaysInMonth(yx, mx) - 1

            If d.DayOfWeek = DayOfWeek.Monday Then
                ReDim Preserve ed(j1)
                ed(j1) = d
                j1 = j1 + 1

                If i1 = 0 Then
                    bzbz = "Y"
                End If
            End If



            If Format(d, "yyyyMM") = monthid Then
                For k1 = 1 To Ex_Count
                    If bzbz = "Y" Then
                        WeekBuffA(j1, k1) = WeekBuffA(j1, k1) + Trim(Ex_ID(k1)) + Format(d, "dd") + "+"
                    Else
                        WeekBuffA(j1 + 1, k1) = WeekBuffA(j1 + 1, k1) + Trim(Ex_ID(k1)) + Format(d, "dd") + "+"
                    End If
                Next
            End If



            d = d.AddDays(1)
        Next

        For i1 As Int16 = 0 To ed.Length - 1
            WeekBuff(i) = ed(i)
        Next

        totalweeks = ed.Length
        If ed(ed.Length - 1) <= LastDayOfMonth And bzbz <> "Y" Then
            totalweeks = totalweeks + 1
        End If

        MonthWeekJS = totalweeks ''變色要

        ''----------------------------------------------------------------------------------------------------------------
        Dim TempStr As String = ""
        Dim ii, jj, COUNT As Integer

        COUNT = totalweeks


        view.Bands.AddBand("產品編號")
        view.Bands.AddBand("類型")

        For ii = 1 To COUNT
            view.Bands.AddBand(Format(CDate(Me.YearDate.EditValue), "yyyy年MM月") & "第" & Format(ii, "00") & "周")
            view.Bands(ii + 1).AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center

            For jj = 1 To Ex_Count
                view.Bands(ii + 1).Children.AddBand(Trim(Ex_Type_Name(jj)))
                view.Bands(ii + 1).Children(jj - 1).AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center

                SumJS = SumJS + 1
                dsFile(SumJS) = Trim("第" + Format(ii, "00") & "周" & Ex_ID(jj))

                TempStr = TempStr + "sum(" + Mid(WeekBuffA(ii, jj), 1, Len(WeekBuffA(ii, jj)) - 1) + ") as " + Trim("第" + Format(ii, "00") & "周" & Ex_ID(jj)) + ","
            Next
        Next

        TempStr = "select PM_M_Code,PM_Type," + Mid(TempStr, 1, Len(TempStr) - 1) + " from ProductionDetailMainMonthSum"

        SQLStr = "PM_M_Code,PM_Type" + "," + Mid(TempStr, 1, Len(TempStr) - 1)


        Dim StrWhere As String
        StrWhere = "FiledDate ='" + Format(CDate(MonthDay), "yyyy-MM") + "'"
        If PM_M_Code Is Nothing Then
        Else
            StrWhere = StrWhere + "and PM_M_Code ='" + PM_M_Code + "'"
        End If

        If PM_Type Is Nothing Then
        Else
            StrWhere = StrWhere + "and PM_Type ='" + PM_Type + "'"
        End If

        Sql = TempStr + " where " + StrWhere + " group by PM_M_Code,PM_Type"



        ''-----------------------------------------------------
        Dim conn1 As New SqlConnection(CONN)
        Dim sqladapter As New SqlDataAdapter(Sql, conn1)
        conn1.Open()

        sqladapter.Fill(DSHead)
        conn1.Close()
        ''綁定數據字段
        Me.GridControlExcel.DataSource = DSHead.Tables(0)

        ' GridControl1.Dispose()

        'GridControl1.DataSource = DSHead.Tables(0)


        If DSHead.Tables(0).Rows.Count <= 0 Then
            'BZ = "Err"
            'Exit Function
        End If
        '------------------------------------------------------------------------------------
        SumJS = 0

        view.Columns("PM_M_Code").OwnerBand = view.Bands(0) 'PM_Type
        view.Columns("PM_Type").OwnerBand = view.Bands(1) '

        view.Columns("PM_M_Code").Width = 125
        view.Columns("PM_M_Code").OptionsColumn.ReadOnly = True
        view.Columns("PM_Type").Width = 125
        view.Columns("PM_Type").OptionsColumn.ReadOnly = True
        view.Columns("PM_M_Code").OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.True

        view.Bands(0).Fixed = FixedStyle.Left
        view.Bands(1).Fixed = FixedStyle.Left

        On Error Resume Next
        For i = 1 To COUNT
            For j = 1 To Ex_Count
                SumJS = SumJS + 1
                view.Columns(dsFile(SumJS)).OwnerBand = view.Bands(i + 1).Children(j - 1)
                view.Columns(dsFile(SumJS)).OptionsColumn.ReadOnly = True
                view.Columns(dsFile(SumJS)).Width = 65
            Next
        Next

        view.EndDataUpdate()
        '结束数据的编辑
        view.EndUpdate()
        '结束视图的编辑
    End Function


    Function Ds_ProductionMonth(ByVal Year_A As String, ByVal PM_M_Code As String, ByVal PM_Type As String) As Boolean

        Dim i, j, k As Integer
        Dim BZ As String


        Dim view As BandedGridView = TryCast(Me.AdvBandedGridView1, BandedGridView)
        view.Bands.Clear()
        view.Columns.Clear()

        'view.BeginUpdate();
        view.BeginUpdate()
        '开始视图的编辑，防止触发其他事件
        view.BeginDataUpdate()
        '开始数据的编辑            
        '添加列标题
        'view.OptionsView.ShowColumnHeaders = False
        view.OptionsView.ColumnAutoWidth = False

        view.OptionsView.EnableAppearanceEvenRow = True '';                   //是否启用偶数行外观
        view.OptionsView.EnableAppearanceOddRow = True '';                     //是否启用奇数行外观
        view.OptionsCustomization.AllowColumnResizing = False ';              //是否允许调整列宽
        view.OptionsView.ShowFooter = False

        view.OptionsView.AllowCellMerge = True


        ''DSHead

        DSHead.Tables.Clear()
        DSHead.Clear()

        Dim SQLStr As String = ""
        Dim SumSQLStr As String = ""
        Dim SumJS As Integer
        Dim dsFile(1000) As String

        Dim Sql As String = ""

        view.Bands.AddBand("產品編號")
        view.Bands.AddBand("類型")

        Dim tempstr As String = ""
        Dim tempstr1 As String = ""
        Dim tempstrZ As String = ""

        Dim strwhereHZ As String
        strwhereHZ = "and a.PM_M_Code =PM_M_Code and a.PM_Type = PM_Type"

        For i = 1 To 12
            view.Bands.AddBand(Year_A & "年" & Format(i, "00") & "月")
            view.Bands(i + 1).AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center


            Dim days1 As Integer
            If i = 12 Then
                days1 = 31
            Else
                Dim dt As New DateTime(DateTime.Today.Year, i, 1)                 '|
                days1 = dt.AddMonths(1).DayOfYear - dt.DayOfYear
            End If
            For j = 1 To Ex_Count
                tempstr = ""

                For k = 1 To days1
                    tempstr = tempstr + Trim(Ex_ID(j)) & Format(k, "00") + "+"
                Next
                tempstr = Mid(tempstr, 1, Len(tempstr) - 1)
                tempstr1 = "sum (" + tempstr + ")"

                tempstr1 = "(select " + tempstr1 + " from ProductionDetailMainMonthSum where FiledDate='" + Trim(Year_A + "-" + Format(i, "00")) + "'" + strwhereHZ + ") as Z" + Format(i, "00") + "月" + Trim(Ex_ID(j)) + ","
                tempstrZ = tempstrZ + tempstr1


                SumJS = SumJS + 1
                dsFile(SumJS) = "Z" + Format(i, "00") + "月" + Trim(Ex_ID(j))

                view.Bands(i + 1).Children.AddBand(Trim(Ex_Type_Name(j)))
                view.Bands(i + 1).Children(j - 1).AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
            Next
        Next


        SQLStr = " PM_M_Code,PM_Type" + "," + Mid(tempstrZ, 1, Len(tempstrZ) - 1)

        Dim StrWhere As String
        StrWhere = "FiledDate like'" + Year_A + "%'"
        If PM_M_Code Is Nothing Then
        Else
            StrWhere = StrWhere + "and PM_M_Code ='" + PM_M_Code + "'"
        End If

        If PM_Type Is Nothing Then
        Else
            StrWhere = StrWhere + "and PM_Type ='" + PM_Type + "'"
        End If

        Sql = "select " + SQLStr + " from ProductionDetailMainMonthSum a" + " where " + StrWhere + "group by PM_M_Code,PM_Type"



        Dim days As Integer
        ''-----------------------------------------------------
        Dim conn1 As New SqlConnection(CONN)
        Dim sqladapter As New SqlDataAdapter(Sql, conn1)
        conn1.Open()

        sqladapter.Fill(DSHead, "Year")
        conn1.Close()
        ''綁定數據字段
        Me.GridControlExcel.DataSource = DSHead.Tables("Year")

        ' GridControl1.Dispose()

        'GridControl1.DataSource = DSHead.Tables(0)


        If DSHead.Tables("Year").Rows.Count <= 0 Then
            'BZ = "Err"
            'Exit Function
        End If
        '------------------------------------------------------------------------------------
        SumJS = 0

        view.Columns("PM_M_Code").OwnerBand = view.Bands(0) 'PM_Type
        view.Columns("PM_Type").OwnerBand = view.Bands(1) '

        view.Columns("PM_M_Code").Width = 125
        view.Columns("PM_M_Code").OptionsColumn.ReadOnly = True
        view.Columns("PM_Type").Width = 125
        view.Columns("PM_Type").OptionsColumn.ReadOnly = True
        view.Columns("PM_M_Code").OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.True

        view.Bands(0).Fixed = FixedStyle.Left
        view.Bands(1).Fixed = FixedStyle.Left

        On Error Resume Next
        For i = 1 To 12
            For j = 1 To Ex_Count
                SumJS = SumJS + 1
                view.Columns(dsFile(SumJS)).OwnerBand = view.Bands(i + 1).Children(j - 1)
                view.Columns(dsFile(SumJS)).OptionsColumn.ReadOnly = True
                view.Columns(dsFile(SumJS)).Width = 65
            Next
        Next

        view.EndDataUpdate()
        '结束数据的编辑
        view.EndUpdate()
        '结束视图的编辑
    End Function





    Function UpdateTOYera(ByVal _year As String) As Boolean
        UpdateTOYera = True

        Dim ii As Integer
        If DSHead.Tables("Year").Rows.Count > 0 Then
        Else
            UpdateTOYera = False
            Exit Function
        End If

        'Dim _year As String
        '_year = Format(CDate(YearDate.EditValue), "yyyy")

        Dim DeleteStr As String

        DeleteStr = "delete from ProductionDetailMainMonthSumYear where FiledDate like '" + _year + "%'"
        Dim fcd As New ProductionFieldDaySummaryCreatControl

        If fcd.ProductionFieldDaySummaryTempAddUpdate(DeleteStr) = True Then
        Else
            UpdateTOYera = False
            MsgBox("刪除失敗！")
            Exit Function
        End If

        ''-------------------------------------------------------------------
        Dim i, j As Integer
        Dim updateStr1 As String = ""
        Dim updateStr2 As String = ""
        Dim tepmstr As String = ""


        For ii = 0 To DSHead.Tables("Year").Rows.Count - 1

            Dim _PM_M_Code As String = DSHead.Tables("Year").Rows(ii)("PM_M_Code")
            Dim _PM_Type As String = DSHead.Tables("Year").Rows(ii)("PM_Type")


            If _PM_M_Code = _PM_Type Then
                For i = 1 To 12
                    updateStr1 = ""
                    updateStr2 = ""

                    For j = 1 To Ex_Count
                        tepmstr = "Z" + Format(i, "00") + "月" + Trim(Ex_ID(j))
                        updateStr1 = updateStr1 + Trim(Ex_ID(j)) + ","

                        updateStr2 = updateStr2 + "'" + Str(Val(DSHead.Tables("Year").Rows(ii)(tepmstr).ToString)) + "'" + ","
                    Next

                    updateStr1 = "PM_M_Code,PM_Type,FiledDate," + Mid(updateStr1, 1, Len(updateStr1) - 1)
                    updateStr2 = "'" + _PM_M_Code + "','" + _PM_Type + "','" + Trim(_year + "-" + Format(i, "00")) + "'," + Mid(updateStr2, 1, Len(updateStr2) - 1)
                    Dim insertStr As String 'values

                    insertStr = "insert ProductionDetailMainMonthSumYear (" + updateStr1 + ") values (" + updateStr2 + ")"


                    Dim fcU As New ProductionFieldDaySummaryCreatControl

                    If fcU.ProductionFieldDaySummaryTempAddUpdate(insertStr) = True Then
                    Else
                        MsgBox("更新失敗！")

                        Exit Function
                    End If
                Next
            End If
        Next

    End Function


    Function Ds_ProductionYear(ByVal Year_A As String, ByVal PM_M_Code As String, ByVal PM_Type As String) As Boolean

        Dim j As Integer



        Dim view As BandedGridView = TryCast(Me.AdvBandedGridView1, BandedGridView)
        view.Bands.Clear()
        view.Columns.Clear()

        'view.BeginUpdate();
        view.BeginUpdate()
        '开始视图的编辑，防止触发其他事件
        view.BeginDataUpdate()
        '开始数据的编辑            
        '添加列标题
        'view.OptionsView.ShowColumnHeaders = False
        '  view.OptionsView.ColumnAutoWidth = True

        view.OptionsView.EnableAppearanceEvenRow = True  '';                   //是否启用偶数行外观
        view.OptionsView.EnableAppearanceOddRow = True '';                     //是否启用奇数行外观
        view.OptionsCustomization.AllowColumnResizing = False ';              //是否允许调整列宽
        view.OptionsView.ShowFooter = False

        view.OptionsView.AllowCellMerge = True


        ''DSHead

        DSHead.Tables.Clear()
        DSHead.Clear()

        Dim SQLStr As String = ""
        Dim SumSQLStr As String = ""
        Dim SumJS As Integer
        Dim dsFile(1000) As String

        Dim Sql As String = ""

        view.Bands.AddBand("產品編號")
        view.Bands.AddBand("類型")
        view.Bands.AddBand("月份")
        view.Bands.AddBand("統計類")
        view.Bands(3).AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        For j = 1 To Ex_Count

            SumJS = SumJS + 1
            dsFile(SumJS) = Trim(Ex_ID(j))
            SumSQLStr = SumSQLStr + Trim(Ex_ID(j)) + ","
            view.Bands(3).Children.AddBand(Trim(Ex_Type_Name(j)))
            view.Bands(3).Children(j - 1).AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Next


        SQLStr = " PM_M_Code,PM_Type,FiledDate" + "," + Mid(SumSQLStr, 1, Len(SumSQLStr) - 1)

        Dim StrWhere As String
        StrWhere = "FiledDate like'" + Year_A + "%'"
        If PM_M_Code Is Nothing Then
        Else
            StrWhere = StrWhere + "and PM_M_Code ='" + PM_M_Code + "'"
        End If

        If PM_Type Is Nothing Then
        Else
            StrWhere = StrWhere + "and PM_Type ='" + PM_Type + "'"
        End If

        Sql = "select " + SQLStr + " from ProductionDetailMainMonthSumYear" + " where " + StrWhere

        ''-----------------------------------------------------
        Dim conn1 As New SqlConnection(CONN)
        Dim sqladapter As New SqlDataAdapter(Sql, conn1)
        conn1.Open()

        sqladapter.Fill(DSHead)
        conn1.Close()
        ''綁定數據字段
        Me.GridControlExcel.DataSource = DSHead.Tables(0)

        '------------------------------------------------------------------------------------
        SumJS = 0

        view.Columns("PM_M_Code").OwnerBand = view.Bands(0) 'PM_Type
        view.Columns("PM_Type").OwnerBand = view.Bands(1) '
        view.Columns("FiledDate").OwnerBand = view.Bands(2)

        view.Columns("FiledDate").Width = 125
        view.Columns("PM_M_Code").Width = 125
        view.Columns("PM_M_Code").OptionsColumn.ReadOnly = True
        view.Columns("PM_Type").Width = 125
        view.Columns("PM_Type").OptionsColumn.ReadOnly = True
        view.Columns("PM_M_Code").OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.True

        view.Bands(0).Fixed = FixedStyle.Left
        view.Bands(1).Fixed = FixedStyle.Left
        view.Bands(2).Fixed = FixedStyle.Left


        For j = 1 To Ex_Count
            SumJS = SumJS + 1
            view.Columns(dsFile(SumJS)).OwnerBand = view.Bands(3).Children(j - 1)
            view.Columns(dsFile(SumJS)).OptionsColumn.ReadOnly = True
            view.Columns(dsFile(SumJS)).Width = 100
        Next

        view.EndDataUpdate()
        '结束数据的编辑
        view.EndUpdate()
        '结束视图的编辑

        view.OptionsView.AllowCellMerge = True
        view.Columns("PM_M_Code").OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.True
        view.Bands(0).Columns(0).OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.True

    End Function


    Private Sub RadioButton1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles RadioButton1.Click, RadioButton2.Click, RadioButton3.Click, RadioButton4.Click
        Dim strPM_M_Code As String
        Dim strPM_Type As String

        If PM_M_Code.EditValue = "全部" Or PM_M_Code.EditValue Is Nothing Or PM_M_Code.EditValue = "" Then
            strPM_M_Code = Nothing
        Else
            strPM_M_Code = PM_M_Code.EditValue
        End If

        If gluType.EditValue = "全部" Or gluType.EditValue Is Nothing Or gluType.EditValue = "" Then
            strPM_Type = Nothing
        Else
            strPM_Type = gluType.EditValue
        End If

        If RadioButton1.Checked = True Then
            PressSign = "1"
            Ds_ProductionDay(Format(CDate(YearDate.EditValue), "yyyy-MM"), strPM_M_Code, strPM_Type)
            UpdateToolStripMenuItem.Enabled = False
        End If


        If RadioButton2.Checked = True Then
            PressSign = "2"
            Ds_ProductionWeek(Format(CDate(YearDate.EditValue), "yyyy-MM"), strPM_M_Code, strPM_Type)

            UpdateToolStripMenuItem.Enabled = False
        End If

        If RadioButton3.Checked = True Then
            PressSign = "3"
            Ds_ProductionMonth(Format(CDate(YearDate.EditValue), "yyyy"), strPM_M_Code, strPM_Type)

            UpdateToolStripMenuItem.Enabled = True

        End If


        If RadioButton4.Checked = True Then
            PressSign = ""
            Ds_ProductionYear(Format(CDate(YearDate.EditValue), "yyyy"), strPM_M_Code, Nothing)

            UpdateToolStripMenuItem.Enabled = False
        End If
    End Sub


    Private Sub UpdateToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles UpdateToolStripMenuItem.Click
        Dim _year As String
        _year = Format(CDate(YearDate.EditValue), "yyyy")

        If UpdateTOYera(_year) = True Then
            MsgBox("更新成功!")
        End If
    End Sub

    Private Sub RadioButton1_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RadioButton1.CheckedChanged

    End Sub
End Class